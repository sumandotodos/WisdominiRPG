#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class NodeEditor : EditorWindow {
	
	public List<BaseNode> windows = new List<BaseNode> ();
	private Vector2 mousePos;
	private BaseNode selectednode;
	private bool makeTransitionMode = false;
	const int maxNodesPerRow = 6;
	static public GameObject nodeEditorGarbageGO;
	string currentFolder;
	FolderChooserNode folderChooser;
	Rect scrollPosition = new Rect();


	[MenuItem("Window/Node based conversation editor")]
	static void ShowEditor() {

		int x = 25, y = 55;
		int nodesInRow = 0;

		GameObject lwoGO;
		ConversationGenerator lwo;

		NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();


		editor.scrollPosition.x = 0;
		editor.scrollPosition.y = 0;
		//nodeEditorGarbageGO = new GameObject ();
		//nodeEditorGarbageGO.name = "_editorGarbage";

		FolderChooserNode folderNode = ScriptableObject.CreateInstance<FolderChooserNode> ();
		folderNode.windowTitle = "Choose Folder:";
		folderNode.windowRect = new Rect(10, 10, 500, 80);
		folderNode.setNodeEditor(editor);
		editor.windows.Add (folderNode);

		string path = Application.dataPath + "/Resources/Prefabs/NodeEditorObjects/";

		DirectoryInfo info = new DirectoryInfo(path);
	
		FileInfo[] fileInfo = info.GetFiles();

		foreach (FileInfo file in fileInfo) {
			
			string fname = file.Name;
			if (fname.EndsWith (".prefab")) {
				string nodeName = fname.Substring (0, fname.Length - ".prefab".Length);
				ObjectNode newNode = new ObjectNode ();
				newNode.currentFolder = ".";
				newNode.windowTitle = nodeName;
				newNode.windowRect = new Rect(x, y, 180, 240);
				editor.windows.Add (newNode);
				lwoGO = Instantiate(Resources.Load("Prefabs/NodeEditorObjects/" + nodeName, typeof(GameObject))) as GameObject;
				//lwoGO.transform.parent = nodeEditorGarbageGO.transform;
				lwo = lwoGO.GetComponent<ConversationGenerator> ();

				newNode.theObject = lwoGO;
				newNode.theWO = lwo;

				++nodesInRow;
				x += 180 + 15;
				if (nodesInRow >= maxNodesPerRow) {
					
					x = 15;
					y += 240 + 15;
					nodesInRow = 0;
					Debug.Log (nodesInRow);
				}
			}
		}

	}

	public void updateFolder(string newFolder) {

		currentFolder = newFolder;

		windows.RemoveRange (1, windows.Count - 1);

		int x = 25, y = 55;
		int nodesInRow = 0;


		GameObject lwoGO;
		ConversationGenerator lwo;

		//NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();

		//nodeEditorGarbageGO = new GameObject ();
		//nodeEditorGarbageGO.name = "_editorGarbage";


		//windows.Add (folderChooser);

		string path = Application.dataPath + "/Resources/Prefabs/NodeEditorObjects/" + currentFolder;

		DirectoryInfo info = new DirectoryInfo(path);

		FileInfo[] fileInfo = info.GetFiles();

		foreach (FileInfo file in fileInfo) {

			string fname = file.Name;
			if (fname.EndsWith (".prefab")) {
				string nodeName = fname.Substring (0, fname.Length - ".prefab".Length);
				ObjectNode newNode = ObjectNode.CreateInstance <ObjectNode> ();
				newNode.currentFolder = currentFolder;
				newNode.windowTitle = nodeName;
				newNode.windowRect = new Rect(x, y, 180, 240);

				lwoGO = Instantiate(Resources.Load("Prefabs/NodeEditorObjects/" + currentFolder + "/"+ nodeName, typeof(GameObject))) as GameObject;
				//lwoGO.transform.parent = nodeEditorGarbageGO.transform;
				lwo = lwoGO.GetComponent<ConversationGenerator> ();
				newNode.theObject = lwoGO;
				newNode.theWO = lwo;
				if (newNode.theWO.isRoot) {
					windows.Add (newNode);
				
					//newNode.events = new List<string> ();
					//string[] evNames = lwo.getEvents ();
					//for (int i = 0; i < evNames.Length; ++i) {
					//	newNode.addEvent (evNames [i]);
					//}
					++nodesInRow;
					x += 180 + 15;
					if (nodesInRow >= maxNodesPerRow) {
						x = 15;
						y += 180 + 15;
						nodesInRow = 0;
					}

				} // end of if isRoot
			}
		}


	}

	void OnGUI() {

		Event e = Event.current;

		mousePos = e.mousePosition;

		if(e.button == 1 && !makeTransitionMode) {

			if(e.type == EventType.MouseDown) {

				bool clickedOnWindow = false;
				int selectIndex = -1;

				for(int i=0; i< windows.Count; ++i) {

					if(windows[i].windowRect.Contains(mousePos)) {

						selectIndex = i;
						clickedOnWindow = true;
						break;
					}
				}

				if(!clickedOnWindow) {

					GenericMenu menu = new GenericMenu();
					menu.AddItem (new GUIContent ("Add Conversation Node"), false, ContextCallback, "conversationNode");
					//menu.AddItem(new GUIContent("Add Input Node"), false, ContextCallback, "inputNode");
					//menu.AddItem(new GUIContent("Add Output Node"), false, ContextCallback, "outputNode");
					//menu.AddItem(new GUIContent("Add Calculation Node"), false, ContextCallback, "calcNode");
					//menu.AddItem(new GUIContent("Add Comparison Node"), false, ContextCallback, "compNode");

					menu.ShowAsContext();
					e.Use();

				}
				else {
					/*
					GenericMenu menu = new GenericMenu();
					menu.AddItem(new GUIContent("Make Transition"), false, ContextCallback, "makeTransition");
					menu.AddSeparator("");
					menu.AddItem(new GUIContent("Delete Node"), false, ContextCallback, "deleteNode");

					menu.ShowAsContext();
					*/
					e.Use();

				}

			}

		}
		/*
		else if(e.button == 0 && e.type == EventType.MouseDown && makeTransitionMode) {

			bool clickedOnWindow = false;
			int selectIndex = -1;

			for(int i=0; i<windows.Count; i++) {

				if(windows[i].windowRect.Contains(mousePos)) {

					selectIndex = i;
					clickedOnWindow = true;
					break;

				}

			}

			if(clickedOnWindow && !windows[selectIndex].Equals(selectednode)) {

				windows[selectIndex].SetInput((BaseInputNode) selectednode, mousePos);
				makeTransitionMode = false;
				selectednode = null;

			}

			if (!clickedOnWindow) {
				makeTransitionMode = false;
				selectednode = null;
			}

			e.Use ();

		}

		else if(e.button == 0 && e.type == EventType.MouseDown && !makeTransitionMode) {

			bool clickedOnWindow = false;
			int selectIndex = -1;

			for(int i=0; i<windows.Count; i++) {

				if(windows[i].windowRect.Contains(mousePos)) {

					selectIndex = i;
					clickedOnWindow = true;
					break;

				}

			}

			if(clickedOnWindow) {
					
				BaseInputNode nodeToChange = windows[selectIndex].ClickedOnInput(mousePos);

				if(nodeToChange != null) {

					selectednode = nodeToChange;
					makeTransitionMode = true;

				}

			}
				
		}

		if(makeTransitionMode && selectednode != null) {

			Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);

			DrawNodeCurve(selectednode.windowRect, mouseRect);

			Repaint();

		}*/

		//GUI.BeginGroup (scrollPosition);

		foreach (BaseNode n in windows) {

			if(n!=null)
			n.DrawCurves();

		}

		BeginWindows();

		for(int i = 0; i < windows.Count; ++i) {

			BaseNode win = windows [i];
			if (win) {
				windows [i].windowRect = GUI.Window (i, windows [i].windowRect, DrawNodeWindow, windows [i].windowTitle);
			}

		}

		EndWindows();

		//GUI.EndGroup ();

	}


	void DrawNodeWindow(int id) {

			windows[id].DrawWindow();
			GUI.DragWindow();

	}

	void ContextCallback(object obj) {

		string clb = obj.ToString();


		if (clb.Equals ("conversationNode")) {

			GameObject newGO = new GameObject ();
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

			windows.Add(objectNode);

		}


	}

	public static void DrawNodeCurve(Rect start, Rect end) {

		Vector3 startPos = new Vector3(start.x + start.width/2, start.y + start.height/2, 0 );
		Vector3 endPos = new Vector3(end.x + end.width/2, end.y + end.height/2, 0);
		Vector3 startTan = startPos + Vector3.right * 50;
		Vector3 endTan = endPos + Vector3.left * 50;
		Color shadowColor = new Color(0, 0, 0, 0.06f);

		for(int i = 0; i < 3; ++i) {

			Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowColor, null, (i+1)*5);

		}

		Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);

	}

	void OnDestroy() {
		//DestroyImmediate (nodeEditorGarbageGO);
		windows.Clear ();
	}

}

#endif