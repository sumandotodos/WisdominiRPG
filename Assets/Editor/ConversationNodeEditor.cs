#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

struct Connection {

	public int originNipple;
	public int originWindow;
	public int destNipple;
	public int destWindow;

	public bool Equals( Connection con ){

		if ((originNipple == con.originNipple) &&
		   (originWindow == con.originWindow) &&
		   (destNipple == con.destNipple) &&
		   (destWindow == con.destWindow))
			return true;

		return false;

	}

}

public class ConversationNodeEditor : EditorWindow {

	public GameObject rootGO;
	public ConversationGenerator root;
	public List<ConversationNode> windows = new List<ConversationNode> ();
	int x, y;
	Vector2 mousePos;
	public string folder;
	bool connectionMode = false;
	bool dragMode = false;
	Vector2 dragPoint = Vector2.zero;
	Vector2 currentConnectionRootPoint;
	int currentWindow, currentNipple;
	List<Connection> connections = new List<Connection> ();
	List<string> instantiatedWindows = new List<string>();

	int windowToDelete = -1;
	int outputNippleToDelete = -1;
	int inputNippleToDelete = -1;

	List<string> generatedWindows;

	float maxWinX = 0;
	float maxWinY = 0;

	public int subProgram = 0;

	Vector2 scrollPosition = Vector2.zero;

	// add connection if such a connection does not exist
	public void addConnection(int originWindow, int originNipple, int destWindow, int destNipple) {

		bool connFound = false;
		int connIndex = 0;

		Connection newCon = new Connection ();
		newCon.originNipple = originNipple;
		newCon.originWindow = originWindow;
		newCon.destNipple = destNipple;
		newCon.destWindow = destWindow;

		bool found = false;
		for (int k = 0; k < connections.Count; ++k) {
			if (connections [k].Equals(newCon))
				found = true;
		}

		if (found) {
			//windows [newCon.originWindow].setLinkTarget (newCon.originNipple, windows [newCon.destWindow].windowTitle, 
			//	newCon.destNipple);
			return;
		}

		// check for a preexisting connection with same origin, but different destination
		for (int i = 0; i < connections.Count; ++i) {
			if (connections [i].originNipple == originNipple &&
				connections [i].originWindow == originWindow) {
				connFound = true;
				connIndex = i;
			}
		}

		if (connFound) { // delete old
			connections.RemoveAt(connIndex);
		} 

		connections.Add (newCon);
		windows [newCon.originWindow].setLinkTarget (newCon.originNipple, windows [newCon.destWindow].windowTitle, 
				newCon.destNipple);



	}

	public ConversationNode addNode(string name, int x, int y) {
		
		ConversationNode conversationNode = ScriptableObject.CreateInstance<ConversationNode> ();

		GameObject newGO = new GameObject ();
		newGO.name = name;

		ConversationGenerator newConversation = newGO.AddComponent<ConversationGenerator> ();
		newConversation.initialize ();
		newConversation.reset ();
		newConversation.addProgram ();
		newConversation.addEvent ();

		newConversation.windowX = x;
		newConversation.windowY = y;

		conversationNode.folder = folder;
		conversationNode.windowTitle = name;
		conversationNode.windowRect = new Rect(x, y, 300, 360);
		conversationNode.root = newConversation;
		conversationNode.rootGO = newGO;
		conversationNode.theEditor = this;
		windows.Add (conversationNode);

        PrefabUtility.SaveAsPrefabAssetAndConnect(newGO, "Assets/Resources/Prefabs/NodeEditorObjects/" + folder + "/" + name + ".prefab", InteractionMode.AutomatedAction);

		return conversationNode;

	}

	public void initialize() {

		x = 15; y = 15;

		ConversationNode conversationNode = ScriptableObject.CreateInstance<ConversationNode> ();
		if(rootGO.name.EndsWith("(Clone)")) {
			rootGO.name = rootGO.name.Substring(0, rootGO.name.Length - "(Clone)".Length);
		}
		conversationNode.folder = folder;
		conversationNode.windowTitle = rootGO.name;
		conversationNode.windowRect = new Rect(root.windowX, root.windowY, 300, 360);
		conversationNode.root = root;
		conversationNode.rootGO = rootGO;
		conversationNode.theEditor = this;
		windows.Add (conversationNode);

	}

	void OnGUI() {
		//return;

		float maxW, maxH;
		maxW = this.position.width;
		if (maxWinX > maxW)
			maxW = maxWinX;
		maxH = this.position.height;
		if (maxWinY > maxH)
			maxH = maxWinY;
		//scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(this.position.width), GUILayout.Height(this.position.height));
		//scrollPosition = GUILayout.BeginScrollView (scrollPosition);
		scrollPosition = GUI.BeginScrollView(new Rect(0, 0, this.position.width, this.position.height),
			scrollPosition, new Rect(0, 0, maxW+300, maxH+300));

		Event e = Event.current;

		mousePos = e.mousePosition;

		if(e.button == 1 && !connectionMode) { //right click



			if(e.type == EventType.MouseDown) {

				bool clickedOnWindow = false;
				bool clickedOnNipple = false;
				windowToDelete = -1;
				inputNippleToDelete = -1;
				outputNippleToDelete = -1;
				int selectIndex = -1;

				for(int i=0; i< windows.Count; ++i) {

					if(windows[i].windowRect.Contains(mousePos)) {

						selectIndex = i;
						clickedOnWindow = true;
						windowToDelete = i;
						break;

					}

				}

				if (clickedOnWindow) {

					// see if we clicked on an output nipple
					for (int j = 0; j < windows [windowToDelete].outputNipples.Count; ++j) {
						
							Rect absRect = windows [windowToDelete].outputNipples [j];
							absRect.x += windows [windowToDelete].windowRect.x;
							absRect.y += windows [windowToDelete].windowRect.y;
							if (absRect.Contains (mousePos)) {
								
								outputNippleToDelete = j;
								clickedOnNipple = true;
								clickedOnWindow = false;
								
							}
					}

				
					// if we didn't check input nipples
					if (outputNippleToDelete == -1) {
						for (int j = 0; j < windows [windowToDelete].inputNipples.Count; ++j) {

							Rect absRect = windows [windowToDelete].inputNipples [j];
							absRect.x += windows [windowToDelete].windowRect.x;
							absRect.y += windows [windowToDelete].windowRect.y;
							if (absRect.Contains (mousePos)) {

								inputNippleToDelete = j;
								clickedOnNipple = true;
								clickedOnWindow = false;

							}
						}

					}

				}


				if(clickedOnWindow) {

					GenericMenu menu = new GenericMenu();
					menu.AddItem (new GUIContent ("Delete Node"), false, ContextCallback, "deleteNode");

					menu.ShowAsContext();
					e.Use();

				}
				else if(clickedOnNipple) {

					GenericMenu menu = new GenericMenu();
					menu.AddItem (new GUIContent ("Delete Connection"), false, ContextCallback, "deleteConnection");

					menu.ShowAsContext();
					e.Use();

				}
				else {
					
					e.Use();

				}

			}

		}

		if (e.type == EventType.MouseDown && e.button == 0 && !connectionMode) { // left click
			// check in we clicked on a output nipple
			dragMode = true;
			dragPoint = mousePos;
			for (int i = 0; i < windows.Count; ++i) {
				List<Rect> nippleList = windows [i].outputNipples;
				if (windows [i].windowRect.Contains (mousePos))
					dragMode = false;
				for (int j = 0; j < nippleList.Count; ++j) {
					Rect absRect = nippleList [j];
					absRect.x += windows [i].windowRect.x;
					absRect.y += windows [i].windowRect.y;
					if (absRect.Contains (mousePos)) {
						connectionMode = true;
						currentConnectionRootPoint = new Vector2 (absRect.center.x, absRect.center.y);
						currentWindow = i;
						currentNipple = j;
						e.Use ();
					}
				}
			}

		}

		if (dragMode == true) {

			Vector2 diff = mousePos - dragPoint;
			scrollPosition = diff;

		}

		if (e.type == EventType.MouseUp)
			dragMode = false;

		if (e.type == EventType.MouseUp && connectionMode) {

			connectionMode = false;

			int destWindow = 0;

			int destInstruction = 0;

			bool windowFound = false;
			bool nippleFound = false;

			// first, look out for input nipples

			for (int i = 0; i < windows.Count; ++i) {
				for (int j = 0; j < windows [i].inputNipples.Count; ++j) {
					Rect testRect;
					testRect = windows [i].inputNipples [j];
					testRect.x += windows [i].windowRect.x;
					testRect.y += windows [i].windowRect.y;
					if (testRect.Contains (mousePos)) {
						destInstruction = j;
						destWindow = i;
						nippleFound = true;
					}
				}
			}

			if (nippleFound == false) { // try entire window

				for (int i = 0; i < windows.Count; ++i) {
					Rect windowRect = windows [i].windowRect;
					if (windowRect.Contains (mousePos)) {
						windowFound = true;
						destWindow = i;
						destInstruction = 0;
					}
				}

				/* no nipple, no window: create new node */
				if (windowFound == false) {
			

					/* time to create a new window */
					ConversationNode conversationNode = ScriptableObject.CreateInstance<ConversationNode> ();
					conversationNode.windowTitle = rootGO.name;
					conversationNode.windowRect = new Rect (mousePos.x, mousePos.y, 300, 360);

					conversationNode.rootGO = new GameObject ();
					conversationNode.rootGO.name = rootGO.name + "_" + subProgram;
					conversationNode.windowTitle = rootGO.name + "_" + subProgram++;

					conversationNode.theEditor = this;

					conversationNode.root = conversationNode.rootGO.AddComponent<ConversationGenerator> ();
					conversationNode.root.initialize ();

					conversationNode.folder = folder;
					//conversationNode.root.reset ();
					conversationNode.root.addEvent ();
					conversationNode.root.addProgram ();
					conversationNode.root.isRoot = false;
					// inherit all references
					conversationNode.root.soundEffects = root.soundEffects;
					conversationNode.root.music = root.music;
					conversationNode.root.messageTargets = root.messageTargets;

					conversationNode.root.registerEventName ("program");
					//conversationNode.root.reset ();

					destWindow = windows.Count;
					destInstruction = 0;

					windows [0].otherNodes.Add (conversationNode);
					windows.Add (conversationNode);

				}

			}


			/* about to create a new connection */
			/* find out if there is already a connection */
			/* sourcing from same output nipple and */
			/* replace it */

			addConnection (currentWindow, currentNipple, destWindow, destInstruction);



			e.Use ();
		}

		if (connectionMode) {
			DrawNodeCurve (currentConnectionRootPoint, mousePos);
		}

		BeginWindows();

		maxWinX = 0;
		maxWinY = 0;

		for(int i = 0; i < windows.Count; ++i) {

			BaseNode win = windows [i];
			if (win) {
				windows [i].root.windowX = windows [i].windowRect.x;
				windows [i].root.windowY = windows [i].windowRect.y;
				windows [i].windowRect = GUI.Window (i, windows [i].windowRect, DrawNodeWindow, windows [i].windowTitle);
				if (windows [i].windowRect.xMax > maxWinX)
					maxWinX = windows [i].windowRect.xMax;
				if (windows [i].windowRect.yMax > maxWinY)
					maxWinY = windows [i].windowRect.yMax;
			}

		}

		EndWindows();
		List<Rect> lr = new List<Rect>();
		for (int i = 0; i < connections.Count; ++i) {
			Rect absRect = windows [connections[i].originWindow].outputNipples[connections[i].originNipple];
			absRect.x += windows [connections[i].originWindow].windowRect.x;
			absRect.y += windows [connections[i].originWindow].windowRect.y;
			Vector2 startPoint = new Vector2 (absRect.center.x, absRect.center.y);
			int destWindow = connections [i].destWindow;
			int destNipple = connections [i].destNipple;
			Vector2 endPoint = Vector2.zero;
			if (destNipple < windows [destWindow].inputNipples.Count) { // connect to nipple
				lr = windows[destWindow].inputNipples;
				absRect = windows [destWindow].inputNipples [destNipple];
				absRect.x += windows [connections [i].destWindow].windowRect.x;
				absRect.y += windows [connections [i].destWindow].windowRect.y;
				endPoint = new Vector2 (absRect.center.x, absRect.center.y);
			} 
			else { // not enough nipples
				endPoint = new Vector2 (windows[destWindow].windowRect.x, windows[destWindow].windowRect.y+3);
			}
			DrawNodeCurve (startPoint, endPoint);
			int adf;
			adf = lr.Count;
		}

		GUI.EndScrollView();

	}

	void ContextCallback(object obj) {

		string clb = obj.ToString();


		if (clb.Equals ("deleteNode")) {

			if ((windowToDelete != -1) && (inputNippleToDelete == -1) && (outputNippleToDelete == -1)) { // just the window, please

				//List <int> markForRemoval = new List<int>();
				// delete all connections that converge or diverge from that window
				bool removed;
				for (int i = 0; i < connections.Count; ++i) {
					
					removed = false;
					if ((connections [i].originWindow == windowToDelete)) {
						
						connections.RemoveAt (i);
						removed = true;
						//markForRemoval.Add (i);

					}

					else if ((connections [i].destWindow == windowToDelete)) {

						int originWindow = connections [i].originWindow;

						// find out origin (output) nipple
						int oInstr, oOp;
						oInstr = windows [originWindow].instructionOfOutNipple [connections [i].originNipple];
						oOp = windows [originWindow].opOfNipple [connections [i].originNipple];

						windows [originWindow].root.setInstructionParameter (0, oInstr, oOp, ""); // reset name of connection

						connections.RemoveAt (i);
						removed = true;
						//markForRemoval.Add (i);

					}
					if (removed)
						--i; //reevaluate same i

				}
				// and finally, delete the window itself
				windows.RemoveAt(windowToDelete);

			}
			/*GameObject newGO = new GameObject ();
			ConversationGenerator wo = newGO.AddComponent<ConversationGenerator> ();
			wo.initialize ();
			wo.reset ();
			wo.addEvent ();
			wo.addProgram ();
			wo.registerEventName ("none"); // to be altered later

			ObjectNode objectNode = ScriptableObject.CreateInstance<ObjectNode> ();
			objectNode.initialize ();

			objectNode.currentFolder = currentFolder;
			objectNode.windowTitle = "New Conversation";
			objectNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 240);
			objectNode.theObject = newGO;
			objectNode.theWO = wo;

			objectNode.theWO.isRoot = true;

			windows.Add(objectNode);*/




		}

		if (clb.Equals ("deleteConnection")) {

			// find out which connection we want to delete

			if (outputNippleToDelete == -1) { // we want to delete an input nipple (connection.destination)

				for (int j = 0; j < connections.Count; ++j) {

					if ((connections [j].destWindow == windowToDelete) && (connections [j].destNipple == inputNippleToDelete)) {

						// delete connections[j]
						int oInstr, oOp;
						oInstr = windows [connections [j].originWindow].instructionOfOutNipple [connections [j].originNipple];
						oOp = windows [connections [j].originWindow].opOfNipple [connections [j].originNipple];

						windows [connections [j].originWindow].root.setInstructionParameter (0, oInstr, oOp, ""); // reset name of connection

						connections.RemoveAt (j);
						return;

					}

				}

			} else { // we want to delete an output nipple (connection.origin)

				for (int j = 0; j < connections.Count; ++j) {

					if ((connections [j].originWindow == windowToDelete) && (connections [j].originNipple == outputNippleToDelete)) {

						// delete connections[j]
						int oInstr, oOp;
						oInstr = windows [connections [j].originWindow].instructionOfOutNipple [connections [j].originNipple];
						oOp = windows [connections [j].originWindow].opOfNipple [connections [j].originNipple];

						windows [connections [j].originWindow].root.setInstructionParameter (0, oInstr, oOp, ""); // reset name of connection

						connections.RemoveAt (j);
						return;

					}

				}

			}

		}


	}


	void DrawNodeWindow(int id) {

		windows[id].DrawWindow();
		if(connectionMode == false) GUI.DragWindow();

	}

	public static void DrawNodeCurve(Vector2 start, Vector2 end) {

		Vector3 startPos = new Vector3(start.x , start.y , 0 );
		Vector3 endPos = new Vector3(end.x, end.y , 0);
		Vector3 startTan = startPos + Vector3.right * 50;
		Vector3 endTan = endPos + Vector3.left * 50;
		Color shadowColor = new Color(0, 0, 0, 0.06f);

		for(int i = 0; i < 3; ++i) {

			Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowColor, null, (i+1)*5);

		}

		Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);

	}

	public void programToString() {

		string prg = "";

		for (int i = 0; i < windows.Count; ++i) {

			prg += "Node " + windows [i].windowTitle + " " + windows[i].root.windowX + " " + windows[i].root.windowY + "\n$\n";
			prg += windows [i].root.programToString (0);
			prg += "$\nEnd Node\n";

		}

		EditorGUIUtility.systemCopyBuffer = prg;

	}
		
	public void parseProgram(string prg, string[] actionsNames, ConversationNode firstNode, string nodeName) {

		string newName = root.name;
		string characterName = root.ConversationFolderName;

		ConversationNode workingNode = firstNode;

		string[] lines = prg.Split ('\n');

		if (lines.Length == 0)
			return;

		string[] firstLineTokens = lines [0].Split (' ');

		if (firstLineTokens.Length < 2)
			return;

		string oldName = firstLineTokens [1];

		int state = 0;
		int nInstructions = 0;

		if (firstNode != null) {
			generatedWindows = new List<string> ();
			generatedWindows.Add (oldName);
			firstNode.root.reset ();
			firstNode.root.addProgram ();
		} else {
			generatedWindows.Add (nodeName);
		}


		string currentNodeName = "";
		int winX, winY;

		bool mustFinish = false;

		foreach (string line in lines) {

			if (line.StartsWith ("Node")) {
				//state = 1;
				string[] args = line.Split (' ');
				currentNodeName = args [1];
				if (firstNode != null) { // first node
					currentNodeName = currentNodeName.Replace(oldName, newName);
				}
				if (nodeName.Equals (currentNodeName)) {
					mustFinish = true;
					int.TryParse (args [2], out winX);
					int.TryParse (args [3], out winY);
					if (firstNode == null) {
						workingNode = addNode (currentNodeName.Replace (oldName, newName), winX, winY);
					}
				}
			} 

			else if (line.StartsWith ("End Node")) {
				//state = 0;

				if(mustFinish) return; // first Node only for the moment
			} 

			else if (line.StartsWith ("$")) {
				// do nothing??
			}

			else {

				if (mustFinish) { // process instructions only if mustFinish is set

					string[] arg = line.Split ('|');

					if (arg [0].Equals ("branchTo")) {
						// we have some replacement to do...
						//arg [1] = arg [1].Replace (oldName, newName);
						// add a new window with branch target name
						string[] branchTargetComponents = arg[1].Split('/');

						// call recursively
						if(!generatedWindows.Contains(branchTargetComponents[0])) {
							parseProgram(prg, actionsNames, null, branchTargetComponents[0]);
						}

						arg [1] = arg [1].Replace (oldName, newName);

					}

					if (arg [0].Equals ("ask")) {

						int numberOfBranches;
						int.TryParse (arg [1], out numberOfBranches);

						for (int i = 0; i < numberOfBranches; ++i) {
						
							string[] branchTargetComponents = arg [2+numberOfBranches+i].Split ('/');

							// call recursively
							if (!generatedWindows.Contains (branchTargetComponents [0])) {
								parseProgram (prg, actionsNames, null, branchTargetComponents [0]);
							}

							arg [2+numberOfBranches+i] = arg [2+numberOfBranches+i].Replace (oldName, newName);

						}

					}

					if (arg [0].Equals ("if")) {


						for (int i = 0; i < 2; ++i) {

							string[] branchTargetComponents = arg [2+i].Split ('/');

							// call recursively
							if (!generatedWindows.Contains (branchTargetComponents [0])) {
								parseProgram (prg, actionsNames, null, branchTargetComponents [0]);
							}

							arg [2+i] = arg [2+i].Replace (oldName, newName);

						}

					}

					int actionIndex = Utils.indexOfStringInList (actionsNames, arg [0]);
					workingNode.root.addInstructionToProgram (arg);
					EditorUtility.SetDirty (workingNode);
					workingNode.root.addAction (0);
					workingNode.root.setAction (0, nInstructions, actionIndex);
					++nInstructions;

				}

			}

		}




	}


}

#endif