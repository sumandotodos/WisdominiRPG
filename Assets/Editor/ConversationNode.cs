#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ConversationNode : BaseNode {
	
	public GameObject rootGO;

	public ConversationGenerator root;

	public string folder;

	const bool DEBUG = false;

	List<List<string>> stringCollector;
	// pick up says, asks and alerts
	public List<ConversationNode> otherNodes = new List<ConversationNode>(); // only root node points to other

	public ConversationNodeEditor theEditor;


	public string[] actionsMethodName = new string[] {
		"addToInventory", 
		"destroy", 
		"sendMessage", 
		"startAction", 
		"delay", 
		"updateGauge",
		"playSound", 
		"playMusic",
		"setLocation",
		"setWalkingState",
		"walkTo", 
		"say",
		"ask", 
		"setMiniature",
		"setDialogueColor",
		"clearDialogue",
		"blockPlayerControls",
		"unblockPlayerControls",
		"loop",
		"makeLabel",
		"branchTo",
		"if",
		"pauseEvent",
		"resumeEvent",
		"storeData",
		"updateData",
		"replaceProgram",
		"recordConversation",
		"alert",
		"chispalert",
		"followPlayer",
		"sayImage",
		"sayFromStringBank",
		"sendMail",
		"saveGame",
		"runEvent", 
		"stopEvent",
		"disableAnswer",
		"enableAnswer",
		"resetAnswers",
		"fullscreenImage"

	}; 



	int indexOfStringInList(string[] list, string str) {

		for (int i = 0; i < list.Length; ++i) {
			if (str.Equals (list [i]))
				return i;
		}
		return 0;

	}

	public void initialize() {



	}


	/* This check is a tad simplistic, but will do */
	bool hasCorrectLinkSyntax(string s) {

		int middle = s.IndexOf ('/');
		if (middle != -1)
			return true;
		else
			return false;

	}


	public void setLinkTarget(int nNipple, string target, int targetNipple) {

		int originInstructionFromNipple;
		int destInstructionFromNipple;
		int originOpFromNipple;

		ConversationNode other = null;


		for (int i = 0; i < theEditor.windows.Count; ++i) {
			if (theEditor.windows [i].windowTitle.Equals (target))
				other = theEditor.windows [i];
		}
		if (other == null)
			return; // something went very wrong


		originInstructionFromNipple = instructionOfOutNipple [nNipple];

		if (other.instructionOfInNipple != null) {
			if (targetNipple < other.instructionOfInNipple.Count) {
				destInstructionFromNipple = other.instructionOfInNipple [targetNipple];
			}
			else
				destInstructionFromNipple = 0;
		}
		else
			destInstructionFromNipple = 0;
		originOpFromNipple = opOfNipple [nNipple];


		root.setInstructionParameter(0, originInstructionFromNipple, originOpFromNipple, target + "/" + destInstructionFromNipple);


	}

	public override void DrawWindow() {
		
		//return;
		int nNipple = 0;
		int popupSelection;
		int selected;

		stringCollector = new List<List<string>> ();
		List<string> currentStringCollector;

		if (Event.current.type == EventType.Repaint) {
			nipples = new List<NippleRef> ();
			outputNipples = new List<Rect> ();
			inputNipples = new List<Rect> ();
			instructionOfOutNipple = new List<int> ();
			instructionOfInNipple = new List<int> ();
			opOfNipple = new List<int> ();
		}

		string[] actions = new string[] {
			"Add To Inventory",     	// 0
			"Destroy", 					// 1
			"Send Message...", 			// 2
			"Start action...", 			// 3
			"Delay...", 				// 4
			"Update Gauge...",			// 5
			"Play Sound...", 			// 6
			"Play Music...",			// 7
			"Change location...",		// 8
			"Set Walking State...",		// 9
			"Walk to...", 				// 10
			"Say...",					// 11
			"Ask...", 					// 12
			"Set Miniature...",			// 13
			"Set dialogue color...",	// 14
			"Clear dialogue",			// 15
			"Block Player Controls",	// 16
			"Unblock Player Controls",	// 17
			"Loop",						// 18
			"New Branch Target...",		// 19
			"Branch to...",				// 20
			"Conditional branch...",	// 21
			"Pause event...",			// 22
			"Resume event...",			// 23
			"Store data...",			// 24
			"Update data...",			// 25
			"Replace program...", 		// 26
			"Record conversation...",	// 27
			"Alert",					// 28
			"ChispAlert",				// 29
			"Follow Player",			// 30
			"Say Image...",				// 31
			"Say from StringBank...",	// 32
			"Send mail...",				// 33
			"Save Game",				// 34
			"Start event...",			// 35
			"Stop event...",			// 36
			"Disable answer...",		// 37
			"Enable answer...",			// 38
			"Reset answers",			// 39
			"Full screen image..."		// 40
		}; 


		string[] gauges = new string[] {
			"Fire Energy",
			"Water Energy",
			"Air Energy",
			"Earth Energy",
			"Space Energy",
			"Time Energy",
			"Wind Energy",
			"Blue Mana",
			"Red Mana"
		};

		string[] walkingState = new string[] {

			"Standing", 
			"Walking"

		};

		string[] walkingDirection = new string[] {

			"Left",
			"Left back", 
			"Back",
			"Right back",
			"Right",
			"Right front",
			"Front",
			"Left front", 
			"Custom..."

		};

		string[] options = new string[] { 
			"onAwake",
			"onSpeak",
			"onHit",
			"onObstacleProximity",
			"onPickup", 
			"onOverlap", 
			"onEndOverlap", 
			"onInteract",
			"customEvent1", 
			"customEvent2", 	
			"customEvent3" };

		string[] colours = new string[] {

			"Red", "Green", "Blue", 
			"Yellow", "Cyan", "Magenta", 
			"Purple", "Dark grey", "Light grey",
			"Black", "Light black", "Dark black", 
			"Pale green", "Pale pink", "Pale blue",
			"Dirty yellow", "Teal", "Custom..."

		};

		string[] dataTypes = new string[] {

			"Integer", "Float", "Boolean", "String"

		};


		string[] sfxList;
		string[] musList;

		int sfxLength = root.soundEffects.Length;
		sfxList = new string[sfxLength + 1];
		sfxList [0] = "By name...";
		for (int m = 0; m < sfxLength; ++m) {
			AudioClip anObject;
			anObject = root.soundEffects [m];
			if (anObject != null)
				sfxList [m+1] = root.soundEffects [m].name;
			else
				sfxList [m+1] = "<no reference>";

		}


		int musLength = root.music.Length;
		musList = new string[musLength + 1];
		musList [0] = "By name...";
		for (int m = 0; m < musLength; ++m) {
			AudioClip anObject;
			anObject = root.music [m];
			if (anObject != null)
				musList [m+1] = root.music [m].name;
			else
				musList [m+1] = "<no reference>";

		}



		int tlLength = root.messageTargets.Length;
		string[] targetList = new string[tlLength + 1];
		targetList [0] = "By name...";
		for (int m = 0; m < tlLength; ++m) {
			GameObject anObject;
			anObject = root.messageTargets [m];
			if (anObject != null)
				targetList [m+1] = root.messageTargets [m].name;
			else
				targetList [m+1] = "<no reference>";
		}

		bool deleted;
		int deleteInstruction = -1;
		int insertInstruction = -1;
		int pasteEvent = -1;

		for (int i = 0; i < 1; ++i) { // just program #0, please
			int editorSelected;
			//editorSelected = EditorGUILayout.Popup ("Event: ", root.getTypeOfEvent(i), options);
			//root.setTypeOfEvent (i, editorSelected);
			//root.alterEvent (i, options [editorSelected]);

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			currentStringCollector = new List<string> ();
			stringCollector.Add (currentStringCollector);

			Texture nippleTexture = Resources.Load<Texture> ("nipple");

			int na;
			na = root.nActions (i); // number of actions of event i

			string prevAction;
			int itemByString;
			string textFieldItem;

			EditorGUI.indentLevel++;

			int strIndex = 0;
			for (int j = 0; j < na; ++j) {  // loop through all actions (instructions) for event i
				// instruction j in program i



				if (GUILayout.Button ("+", GUILayout.Width (30))) { // if + button pressed...
					//root.addAction(i);
					//Rect r = this.windowRect;
					//r.height += 100.0f;
					//this.windowRect = r;
					EditorUtility.SetDirty (this);
					insertInstruction = j;
					adjustProgramReferences (j, 1);
					//root.addInstructionToProgram (i, "None"); // to be altered later
				}

				deleted = false;

				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();

				int act;

				Rect rect;
				rect = EditorGUILayout.GetControlRect ();
				Rect newRect = new Rect (rect.x, rect.y + 18, 16, 16);
				EditorGUI.DrawPreviewTexture (newRect, nippleTexture);
				if (Event.current.type == EventType.Repaint) {
					inputNipples.Add (newRect);
					instructionOfInNipple.Add (j);
				}
			
				EditorGUILayout.TextField ("Action:");
				act = root.getTypeOfAction (i, j);
				act = EditorGUILayout.Popup (act, actions);

				//EditorGUILayout.LabelField ("Blocking: ");
				switch (act) {


				/*
				 * Add to Inventory
				 * 
				 * n. of params: 0
				 * 
				 *   syntax: addToInventory
				 * 
				 */
				case 0: 
					break;


				case 1: // Destroy
					break;


				/*
				 * Send message
				 * 
				 * n. of params: min 3
				 * 
				 *   syntax: sendMessage <target> <message> <param>*
				 * 
				 */
				case 2: 

					List<string> messageList = new List<string> ();
					List<int> messageIndex = new List<int> ();
					MethodInfo[] methods;

					EditorGUI.indentLevel++;
					textFieldItem = root.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1
					itemByString = indexOfStringInList (targetList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("To whom: ", itemByString, targetList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						//prevAction = root.getInstructionOp (i, j); 
						string prevSelect = root.getInstructionParameter (i, j, 1);
						if (indexOfStringInList (targetList, prevSelect) != 0) { // reset textFieldItem
							textFieldItem = "";
						}
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					} else { // not by name
						textFieldItem = targetList [popupSelection];

					}
					root.setInstructionParameter (i, j, 1, textFieldItem);


					messageList.Add ("By name...");

					methods = null;

					//if (popupSelection == 0) { // if target was By Name...
					if (!textFieldItem.Equals ("")) {
						GameObject go = GameObject.Find (textFieldItem); // find GO with that name...
						if (go != null) {
							methods = go.GetComponent<WisdominiObject> ().GetType ().GetMethods ();
							for (int methi = 0; methi < methods.Length; ++methi) {
								string wisName;
								wisName = methods [methi].Name;
								if (wisName.StartsWith ("_wm_")) {
									messageList.Add (wisName.Substring (4));
									messageIndex.Add (methi);
								}
							}
						}
					}


					//}

					string textFieldItem2 = root.getInstructionParameter (i, j, 2); // make room for 2nd parameter
					int itemByString2 = indexOfStringInList (messageList.ToArray (), textFieldItem2);
					popupSelection = EditorGUILayout.Popup ("What message: ", itemByString2, messageList.ToArray (), GUILayout.Width (200));
					if (popupSelection == 0) { // By name...
						//prevAction = root.getInstructionOp (i, j); 
						string prevSelect = root.getInstructionParameter (i, j, 2);
						if (indexOfStringInList (messageList.ToArray (), prevSelect) != 0) { // reset textFieldItem
							textFieldItem2 = "";
						}
						textFieldItem2 = EditorGUILayout.TextField (textFieldItem2, GUILayout.Width (200));
					} else { // not by name
						textFieldItem2 = messageList.ToArray () [popupSelection];

					}
					root.setInstructionParameter (i, j, 2, textFieldItem2);


					/*
					 * Onto the parameters...
					 */

					EditorGUI.indentLevel++;

					if (popupSelection > 0) { // make sure a method from the drop down is selected......

						if (methods != null) {
							ParameterInfo[] par = methods [messageIndex [popupSelection - 1]].GetParameters ();
							if (par.Length == 0) {
								EditorGUILayout.LabelField ("(no parameters)");
							}

							for (int pi = 0; pi < par.Length; ++pi) {
								string translatedType = translateType (par [pi].ParameterType.Name);
								string textFieldItemParam = root.getInstructionParameter (i, j, 3 + pi); // make room for 2nd parameter
								EditorGUILayout.LabelField (par [pi].Name + " (" + translatedType + ") : ");

								textFieldItemParam = EditorGUILayout.TextField (textFieldItemParam, GUILayout.Width (200));
								root.setInstructionParameter (i, j, 3 + pi, textFieldItemParam);
							}

						}


					} else { // find out available parameters:
						// not for now, doesn't make a lot of sense... ??
					}

					EditorGUI.indentLevel--;


					EditorGUI.indentLevel--;


					break;


				/*
				 * Start action
				 * 
				 * n. of params: min 4
				 * 
				 *   syntax: sendMessage <target> <message> <blocking> <param>*
				 * 
				 */
				case 3: // Start Action...

					messageList = new List<string> ();
					messageIndex = new List<int> ();


					EditorGUI.indentLevel++;
					textFieldItem = root.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1
					itemByString = indexOfStringInList (targetList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("From whom: ", itemByString, targetList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						//prevAction = root.getInstructionOp (i, j); 
						string prevSelect = root.getInstructionParameter (i, j, 1);
						if (indexOfStringInList (targetList, prevSelect) != 0) { // reset textFieldItem
							textFieldItem = "";
						}
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					} else { // not by name
						textFieldItem = targetList [popupSelection];

					}
					root.setInstructionParameter (i, j, 1, textFieldItem);


					messageList.Add ("By name...");

					methods = null;

					//if (popupSelection == 0) { // if target was By Name...
					if (!textFieldItem.Equals ("")) {
						GameObject go = GameObject.Find (textFieldItem); // find GO with that name...
						if (go != null) {
							methods = go.GetComponent<MonoBehaviour> ().GetType ().GetMethods ();
							for (int methi = 0; methi < methods.Length; ++methi) {
								string wisName;
								wisName = methods [methi].Name;
								if (wisName.StartsWith ("_wa_")) {
									messageList.Add (wisName.Substring (4));
									messageIndex.Add (methi);
								}
							}
						}
					}


					//}

					textFieldItem2 = root.getInstructionParameter (i, j, 2); // make room for 2nd parameter
					itemByString2 = indexOfStringInList (messageList.ToArray (), textFieldItem2);
					popupSelection = EditorGUILayout.Popup ("What action: ", itemByString2, messageList.ToArray (), GUILayout.Width (200));
					if (popupSelection == 0) { // By name...
						//prevAction = root.getInstructionOp (i, j); 
						string prevSelect = root.getInstructionParameter (i, j, 2);
						if (indexOfStringInList (messageList.ToArray (), prevSelect) != 0) { // reset textFieldItem
							textFieldItem2 = "";
						}
						textFieldItem2 = EditorGUILayout.TextField (textFieldItem2, GUILayout.Width (200));
					} else { // not by name
						textFieldItem2 = messageList.ToArray () [popupSelection];

					}
					root.setInstructionParameter (i, j, 2, textFieldItem2);

					string isBlockingStr = root.getInstructionParameter (i, j, 3); // make room for 2nd parameter
					bool isBlocking;
					if (isBlockingStr.Equals ("blocking"))
						isBlocking = true;
					else
						isBlocking = false;

					/*
					 * Onto the parameters...
					 */

					EditorGUI.indentLevel++;

					if (popupSelection > 0) { // make sure a method from the drop down is selected......

						if (methods != null) {
							ParameterInfo[] par = methods [messageIndex [popupSelection - 1]].GetParameters ();
							if (par.Length == 1) {
								EditorGUILayout.LabelField ("(no parameters)");
							}
							for (int pi = 1; pi < par.Length; ++pi) {
								string textFieldItemParam = root.getInstructionParameter (i, j, 3 + pi); // make room for 2nd parameter
								EditorGUILayout.LabelField (par [pi].Name + " (" + translateType (par [pi].ParameterType.Name) + ") : ");
								textFieldItemParam = EditorGUILayout.TextField (textFieldItemParam, GUILayout.Width (200));
								root.setInstructionParameter (i, j, 3 + pi, textFieldItemParam);
							}

						}


					} else { // find out available parameters:
						// not for now, doesn't make a lot of sense... ??
					}

					EditorGUI.indentLevel--;

					/*
					 *  Blocking or non-blocking action??
					 */ 

					isBlocking = EditorGUILayout.ToggleLeft (" Blocking ", isBlocking);
					if (isBlocking)
						root.setInstructionParameter (i, j, 3, "blocking");
					else
						root.setInstructionParameter (i, j, 3, "non-blocking");


					EditorGUI.indentLevel--;


					break;



				/*
				 * Send message
				 * 
				 * n. of params: 2
				 * 
				 *   syntax: delay <seconds> 
				 * 
				 */
				case 4: // Delay...
					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField ("Seconds: ");
					textFieldItem = root.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1
					prevAction = root.getInstructionOp (i, j); 
					if (!prevAction.Equals ("delay")) { // If we just changed action, set to 0.0
						textFieldItem = "0.0";
					}
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (130));
					root.setInstructionParameter (i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;



				case 5:
					EditorGUI.indentLevel++;
					EditorGUILayout.Popup ("Gauge: ", 0, gauges, GUILayout.Width (270));
					GUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField ("Increment: ");
					EditorGUILayout.TextField ("0", GUILayout.Width (130));
					GUILayout.EndHorizontal ();
					EditorGUI.indentLevel--;
					break;


				case 6: // Play sound
					EditorGUI.indentLevel++;
					textFieldItem = root.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1
					//itemByString = indexOfStringInList (sfxList, textFieldItem);
					//popupSelection = EditorGUILayout.Popup ("Sound: ", itemByString, sfxList, GUILayout.Width (270));
					//if (popupSelection == 0) { // By name...
						//prevAction = root.getInstructionOp (i, j); 
						//if (!prevAction.Equals ("delay")) { // If we just changed action, set to ""
						//	textFieldItem = "";
						//}
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					//} //else { // not by name
						//textFieldItem = sfxList [popupSelection];

					//}
					root.setInstructionParameter (i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;



				case 7: // Play music
					EditorGUI.indentLevel++;
					popupSelection = EditorGUILayout.Popup ("Music: ", 0, musList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						EditorGUILayout.TextField ("", GUILayout.Width (200));
					}
					EditorGUI.indentLevel--;
					break;


				/*
				case 8: // change location
					EditorGUI.indentLevel++;
					textFieldItem = root.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1

					DirectoryInfo scenesPath = new DirectoryInfo (Application.dataPath);
					FileInfo[] files = scenesPath.GetFiles ("*.unity", SearchOption.AllDirectories);

					string[] fileList;
					int fileListLength = files.Length + 1;
					fileList = new string[fileListLength];
					fileList [0] = "By name...";
					for (int k = 1; k < fileListLength; ++k) {
						fileList [k] = removeExtension(files [k-1].Name);
					}

					itemByString = indexOfStringInList(fileList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("New location: ", itemByString, fileList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						prevAction = root.getInstructionOp (i, j); 
						if (!prevAction.Equals ("setLocation")) { // If we just changed action, set to ""
							textFieldItem = "";
						}
						textFieldItem = EditorGUILayout.TextField ("", GUILayout.Width (200));
					}
					else { // not by name
						textFieldItem = fileList[popupSelection];

					}

					root.setInstructionParameter (i, j, 1, textFieldItem);

					EditorGUI.indentLevel--;
					break;
*/


				
				
				/*
					 * 0: say   1: text   2: wait/no wait flag  3: position (NPC1, NPC2, Player)  4: rosetta string id
					 * 
					 */
				case 11: // say
					EditorGUI.indentLevel++;
					textFieldItem = root.getInstructionParameter (i, j, 1);
					textFieldItem = EditorGUILayout.TextArea (textFieldItem, GUILayout.Width (200));
					currentStringCollector.Add (textFieldItem);
					root.setInstructionParameter (i, j, 1, textFieldItem);

					textFieldItem = root.getInstructionParameter (i, j, 2);
					bool waitForTouch = false;
					if (textFieldItem.Equals ("wait"))
						waitForTouch = true;
					waitForTouch = EditorGUILayout.ToggleLeft (" Wait until read", waitForTouch);
					if (waitForTouch)
						textFieldItem = "wait";
					else
						textFieldItem = "don't wait";
					root.setInstructionParameter (i, j, 2, textFieldItem);

					textFieldItem = root.getInstructionParameter (i, j, 3);
					bool isnpc1 = false;
					bool isnpc2 = false;
					bool isplayer = false;
					bool newnpc1;
					bool newnpc2;
					bool newplayer;
					if (textFieldItem.Equals ("NPC1"))
						isnpc1 = true;
					if (textFieldItem.Equals ("NPC2"))
						isnpc2 = true;
					if (textFieldItem.Equals ("Player"))
						isplayer = true;

					EditorGUILayout.BeginHorizontal ();
					newnpc1 = EditorGUILayout.ToggleLeft ("NPC1", isnpc1, GUILayout.Width (90));
					newnpc2 = EditorGUILayout.ToggleLeft ("NPC2", isnpc2, GUILayout.Width (90));
					newplayer = EditorGUILayout.ToggleLeft ("Player", isplayer, GUILayout.Width (90));
					EditorGUILayout.EndHorizontal ();

					if (newnpc1 && !isnpc1) {
						root.setInstructionParameter (i, j, 3, "NPC1");

					}
					if (newnpc2 && !isnpc2) {
						root.setInstructionParameter (i, j, 3, "NPC2");

					}
					if (newplayer && !isplayer) {
						root.setInstructionParameter (i, j, 3, "Player");

					}

					textFieldItem = root.getInstructionParameter (i, j, 4);
					textFieldItem = "Conv_" + folder + "_" + windowTitle + "_" + strIndex;
					if(textFieldItem.StartsWith("Conv__")) {
						textFieldItem = textFieldItem;
					}
					root.setInstructionParameter (i, j, 4, textFieldItem);
					++strIndex;

					EditorGUI.indentLevel--;
					break;



				/*
				 * 
				 * 0: ask   1: number of answers   2...: answers...  2+nanswers... targets 		2+2*nanswers ... rosetta id
				 * 													format: <string>/<number> means link
				 */
				case 12: // ask
					EditorGUI.indentLevel++;

					textFieldItem = root.getInstructionParameter (i, j, 1);
					if (textFieldItem.Equals (""))
						textFieldItem = "0";
					
					GUILayout.Label ("Number of answers:");
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (80));


					int nAnswers;
					bool ok;

					ok = int.TryParse (textFieldItem, out nAnswers);
					if (!ok) {
						nAnswers = 0;
						textFieldItem = "0";
					}
					root.setInstructionParameter (i, j, 1, textFieldItem);
					int index;
					EditorGUI.indentLevel++;
					for (index = 0; index < nAnswers; ++index) {
						EditorGUILayout.LabelField ("Answer " + (index + 1) + ":");
						textFieldItem = root.getInstructionParameter (i, j, 2 + index);
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (270));
						currentStringCollector.Add (textFieldItem);
						root.setInstructionParameter (i, j, 2 + index, textFieldItem);


						textFieldItem = root.getInstructionParameter (i, j, 2 + index + 2 * nAnswers);
						textFieldItem = "Conv_" + folder + "_" + windowTitle + "_" + strIndex;
						root.setInstructionParameter (i, j, 2 + index + 2 * nAnswers, textFieldItem);
						++strIndex;

						textFieldItem = root.getInstructionParameter (i, j, 2 + index + nAnswers);

						// we would like to potentially create a new window, instance of prefab `textFieldItem`
						// first chech if such a window has been instantiated

						checkLinks (textFieldItem, nNipple);

						rect = EditorGUILayout.GetControlRect ();
						newRect = new Rect (rect.x + 300 - 24, rect.y - 18, 16, 16);
						EditorGUI.DrawPreviewTexture (newRect, nippleTexture);

						if (Event.current.type == EventType.Repaint) {
							
							outputNipples.Add (newRect);
							instructionOfOutNipple.Add (j);
							opOfNipple.Add (2 + nAnswers + index);
							NippleRef newNippleRef;
							newNippleRef.instruction = j;
							newNippleRef.answer = index;
							nipples.Add (newNippleRef);
						}

						if(DEBUG) EditorGUILayout.TextField (textFieldItem);
						root.setInstructionParameter (i, j, 2 + index + nAnswers, textFieldItem);
						GUI.backgroundColor = Color.white;

						++nNipple;

					} // end of index

					EditorGUI.indentLevel--;

					EditorGUI.indentLevel--;
					break;

				case 13: // set Miniature
					EditorGUI.indentLevel++;

					// Find out how many miniatures we do have
					int nMiniatures;
					nMiniatures = 0;
					if (root.neutralMiniature != null)
						++nMiniatures;
					if (root.happyMiniature != null)
						++nMiniatures;
					if (root.sadMiniature != null)
						++nMiniatures;
					if (root.worriedMiniature != null)
						++nMiniatures;


					// we now know how many miniatures we have
					string[] miniatureList = new string[nMiniatures];
					index = 0;
					if (root.neutralMiniature != null) {
						miniatureList [index++] = "Neutral";

					}
					if (root.happyMiniature != null) {
						miniatureList [index++] = "Happy";

					}
					if (root.sadMiniature != null) {
						miniatureList [index++] = "Sad";

					}
					if (root.worriedMiniature != null) {
						miniatureList [index++] = "Worried";

					}

					textFieldItem = root.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (miniatureList, textFieldItem);
					selected = EditorGUILayout.Popup ("Miniature: ", selected, miniatureList, GUILayout.Width (270));
					root.setInstructionParameter (i, j, 1, miniatureList [selected]);

					textFieldItem = root.getInstructionParameter (i, j, 2);
					isnpc1 = false;
					isnpc2 = false;
					isplayer = false;
					if (textFieldItem.Equals ("NPC1"))
						isnpc1 = true;
					if (textFieldItem.Equals ("NPC2"))
						isnpc2 = true;
					if (textFieldItem.Equals ("Player"))
						isplayer = true;

					EditorGUILayout.BeginHorizontal ();
					newnpc1 = EditorGUILayout.ToggleLeft ("NPC1", isnpc1, GUILayout.Width (90));
					newnpc2 = EditorGUILayout.ToggleLeft ("NPC2", isnpc2, GUILayout.Width (90));
					newplayer = EditorGUILayout.ToggleLeft ("Player", isplayer, GUILayout.Width (90));
					EditorGUILayout.EndHorizontal ();

					if (newnpc1 && !isnpc1) {
						root.setInstructionParameter (i, j, 2, "NPC1");

					}
					if (newnpc2 && !isnpc2) {
						root.setInstructionParameter (i, j, 2, "NPC2");

					}
					if (newplayer && !isplayer) {
						root.setInstructionParameter (i, j, 2, "Player");

					}

					EditorGUI.indentLevel--;
					break;

				case 14: // set dialogue colour
					EditorGUI.indentLevel++;
					textFieldItem = root.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (colours, textFieldItem);
					EditorGUILayout.LabelField ("Color:");
					selected = EditorGUILayout.Popup (selected, colours, GUILayout.Width (270));
					if (colours [selected].Equals ("Custom...")) {
						textFieldItem = root.getInstructionParameter (i, j, 2);
						Color theColour;
						theColour = parseColor (textFieldItem);
						theColour = EditorGUILayout.ColorField (theColour, GUILayout.Width (120));
						string strRep;
						strRep = colorToString (theColour);
						root.setInstructionParameter (i, j, 2, strRep);
					}
					root.setInstructionParameter (i, j, 1, colours [selected]);
					EditorGUI.indentLevel--;
					break;

				case 15:

					break;

				case 16: // block player controls
					break;



				case 17: // unblock player controls
					break;



				case 18: // loop program
					break;



				case 19: // make target

					EditorGUI.indentLevel++;

					textFieldItem = root.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1

					prevAction = root.getInstructionOp (i, j); 
					if (!prevAction.Equals ("makeLabel")) { // If we just changed action, set to ""
						textFieldItem = "";
					}
					EditorGUILayout.LabelField ("Target name: ");
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));

					root.setInstructionParameter (i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;

					break;




				case 20: // branch to target...

					EditorGUI.indentLevel++;
					string[] labelList;

					textFieldItem = root.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1

					checkLinks (textFieldItem, nNipple);

					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));

					rect = EditorGUILayout.GetControlRect ();
					newRect = new Rect (rect.x + 300 - 24, rect.y - 18, 16, 16);
					EditorGUI.DrawPreviewTexture (newRect, nippleTexture);

					if (Event.current.type == EventType.Repaint) {

						outputNipples.Add (newRect);
						instructionOfOutNipple.Add (j);
						opOfNipple.Add (1);
						NippleRef newNippleRef;
						newNippleRef.instruction = j;
						newNippleRef.answer = 0;
						nipples.Add (newNippleRef);
					}

					++nNipple;

					root.setInstructionParameter (i, j, 1, textFieldItem);



					GUI.backgroundColor = Color.white;

					EditorGUI.indentLevel--;
					break;



				// if things change, you have to reset shared fields !!!!
				case 21: // conditional branch

					int op1type;
					int op2type;
					int query1Length;
					int query2Length;
					string op1ConstantStr;
					int startOfValue2Parameters;

					startOfValue2Parameters = 7;

					string popUpItemStr;
					int popUpItem;
					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField ("Value 1:");
					textFieldItem = root.getInstructionParameter (i, j, 4);
					string[] conditionList = { "Constant", "Query", "Retrieve data" };
					popUpItem = indexOfStringInList (conditionList, textFieldItem);
					op1type = popUpItem;
					popUpItem = EditorGUILayout.Popup (popUpItem, conditionList, GUILayout.Width (270));
					root.setInstructionParameter (i, j, 4, conditionList [popUpItem]);
					EditorGUI.indentLevel++;
					if (popUpItem == 0) { // Constant selected, stored at index 6
						op1ConstantStr = root.getInstructionParameter (i, j, 6);
						EditorGUILayout.LabelField ("Constant 1:");
						op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
						root.setInstructionParameter (i, j, 6, op1ConstantStr);

					} else if (popUpItem == 2) { // retrieve data selected
						// index 6: key
						// index 7: type
						op1ConstantStr = root.getInstructionParameter (i, j, 6);
						EditorGUILayout.PrefixLabel ("Key: ");
						op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
						root.setInstructionParameter (i, j, 6, op1ConstantStr);

						op1ConstantStr = root.getInstructionParameter (i, j, 7);
						selected = indexOfStringInList (dataTypes, op1ConstantStr);
						selected = EditorGUILayout.Popup (selected, dataTypes, GUILayout.Width (120));
						root.setInstructionParameter (i, j, 7, dataTypes [selected]);

						startOfValue2Parameters = 8;

					} else if (popUpItem == 1) { // Query Selected

						// cipotera padre here
						EditorGUI.indentLevel++;
						textFieldItem = root.getInstructionParameter (i, j, 6); // Make sure there is room for parameter #1
						itemByString = indexOfStringInList (targetList, textFieldItem);
						popupSelection = EditorGUILayout.Popup ("Who: ", itemByString, targetList, GUILayout.Width (270));
						if (popupSelection == 0) { // By name...
							//prevAction = root.getInstructionOp (i, j); 
							string prevSelect = root.getInstructionParameter (i, j, 6);
							if (indexOfStringInList (targetList, prevSelect) != 0) { // reset textFieldItem
								textFieldItem = "";
							}
							textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
						} else { // not by name
							textFieldItem = targetList [popupSelection];

						}
						root.setInstructionParameter (i, j, 6, textFieldItem);

						messageList = new List<string> ();
						messageIndex = new List<int> ();
						//messageList.Add ("By name...");

						methods = null;

						//if (popupSelection == 0) { // if target was By Name...
						if (!textFieldItem.Equals ("")) {
							GameObject go = GameObject.Find (textFieldItem); // find GO with that name...
							if (go != null) {
								methods = go.GetComponent<MonoBehaviour> ().GetType ().GetMethods ();
								for (int methi = 0; methi < methods.Length; ++methi) {
									string wisName;
									wisName = methods [methi].Name;
									if (wisName.StartsWith ("_wm_")) {
										messageList.Add (wisName.Substring (4));
										messageIndex.Add (methi);
										// WARNING hide void methods
									}
								}
							}
						}

						if (messageList.Count == 0) {
							messageList.Add ("<no query available>");
						}


						//}

						textFieldItem2 = root.getInstructionParameter (i, j, 7); // make room for 2nd parameter
						itemByString2 = indexOfStringInList (messageList.ToArray (), textFieldItem2);
						popupSelection = EditorGUILayout.Popup ("Query: ", itemByString2, messageList.ToArray (), GUILayout.Width (270));
						/*if (popupSelection == 0) { // By name...
							//prevAction = root.getInstructionOp (i, j); 
							string prevSelect = root.getInstructionParameter (i, j, 7);
							if (indexOfStringInList (messageList.ToArray (), prevSelect) != 0) { // reset textFieldItem
								textFieldItem2 = "";
							}
							textFieldItem2 = EditorGUILayout.TextField (textFieldItem2, GUILayout.Width (200));
						} else { // not by name*/
						textFieldItem2 = messageList.ToArray () [popupSelection];
						/*
						}*/
						root.setInstructionParameter (i, j, 7, textFieldItem2);



						// Onto the parameters...


						EditorGUI.indentLevel++;

						ParameterInfo[] par;

						if (popupSelection >= 0) { // make sure a method from the drop down is selected......

							if (methods != null) {
								par = methods [messageIndex [popupSelection]].GetParameters ();
								if (par.Length == 0) {
									EditorGUILayout.LabelField ("(no parameters)");
								}
								for (int pi = 0; pi < par.Length; ++pi) {
									string textFieldItemParam = root.getInstructionParameter (i, j, 8 + pi); // make room for 2nd parameter
									EditorGUILayout.LabelField (par [pi].Name + " (" + translateType (par [pi].ParameterType.Name) + ") : ");
									textFieldItemParam = EditorGUILayout.TextField (textFieldItemParam, GUILayout.Width (200));
									root.setInstructionParameter (i, j, 8 + pi, textFieldItemParam);
								}
								root.getInstructionParameter (i, j, 8 + par.Length);
								root.setInstructionParameter (i, j, 8 + par.Length, "\\"); // end of parameters
								startOfValue2Parameters = 8 + par.Length + 1;

							}


						} else { // find out available parameters:
							// not for now, doesn't make a lot of sense... ??
						}

					} // end of cipotera aqui

					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField ("Operator:");
					textFieldItem = root.getInstructionParameter (i, j, 1);
					string[] opList = { "=", "<", ">", ">=", "<=", "!=", "In range", "Not in range", "Is true", "Is false" };
					int popUpItem2;
					int popUpItem3;
					popUpItem2 = indexOfStringInList (opList, textFieldItem);
					popUpItem2 = EditorGUILayout.Popup (popUpItem2, opList, GUILayout.Width (270));
					root.setInstructionParameter (i, j, 1, opList [popUpItem2]);
					EditorGUI.indentLevel--;
					EditorGUI.indentLevel--;
					if (popUpItem2 < 6) {  // binary operator
						EditorGUILayout.LabelField ("Value 2:");
						textFieldItem = root.getInstructionParameter (i, j, 5);
						popUpItem3 = indexOfStringInList (conditionList, textFieldItem);
						popUpItem3 = EditorGUILayout.Popup (popUpItem3, conditionList, GUILayout.Width (270));
						root.setInstructionParameter (i, j, 5, conditionList [popUpItem3]);
						EditorGUI.indentLevel++;
						if (popUpItem3 == 0) { // Constant selected
							op1ConstantStr = root.getInstructionParameter (i, j, startOfValue2Parameters);
							EditorGUILayout.LabelField ("Constant 2:");
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							root.setInstructionParameter (i, j, startOfValue2Parameters, op1ConstantStr);
						} else if (popUpItem3 == 2) { // retrieve data selected
							// index 6: key
							// index 7: type
							op1ConstantStr = root.getInstructionParameter (i, j, startOfValue2Parameters);
							EditorGUILayout.PrefixLabel ("Key: ");
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							root.setInstructionParameter (i, j, startOfValue2Parameters, op1ConstantStr);

							op1ConstantStr = root.getInstructionParameter (i, j, startOfValue2Parameters + 1);
							selected = indexOfStringInList (dataTypes, op1ConstantStr);
							selected = EditorGUILayout.Popup (selected, dataTypes, GUILayout.Width (120));
							root.setInstructionParameter (i, j, startOfValue2Parameters + 1, dataTypes [selected]);


						} else { // Query Selected
							// cipotera padre here

							// cipotera padre here
							EditorGUI.indentLevel++;
							textFieldItem = root.getInstructionParameter (i, j, startOfValue2Parameters); // Make sure there is room for parameter #1
							itemByString = indexOfStringInList (targetList, textFieldItem);
							popupSelection = EditorGUILayout.Popup ("Who: ", itemByString, targetList, GUILayout.Width (270));
							if (popupSelection == 0) { // By name...
								//prevAction = root.getInstructionOp (i, j); 
								string prevSelect = root.getInstructionParameter (i, j, startOfValue2Parameters);
								if (indexOfStringInList (targetList, prevSelect) != 0) { // reset textFieldItem
									textFieldItem = "";
								}
								textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
							} else { // not by name
								textFieldItem = targetList [popupSelection];

							}
							root.setInstructionParameter (i, j, startOfValue2Parameters, textFieldItem);

							messageList = new List<string> ();
							messageIndex = new List<int> ();
							//messageList.Add ("By name...");

							methods = null;

							//if (popupSelection == 0) { // if target was By Name...
							if (!textFieldItem.Equals ("")) {
								GameObject go = GameObject.Find (textFieldItem); // find GO with that name...
								if (go != null) {
									methods = go.GetComponent<MonoBehaviour> ().GetType ().GetMethods ();
									for (int methi = 0; methi < methods.Length; ++methi) {
										string wisName;
										wisName = methods [methi].Name;
										if (wisName.StartsWith ("_wm_")) {
											messageList.Add (wisName.Substring (4));
											messageIndex.Add (methi);
											// WARNING hide void methods
										}
									}
								}
							}

							if (messageList.Count == 0) {
								messageList.Add ("<no query available>");
							}


							//}

							textFieldItem2 = root.getInstructionParameter (i, j, startOfValue2Parameters + 1); // make room for 2nd parameter
							itemByString2 = indexOfStringInList (messageList.ToArray (), textFieldItem2);
							popupSelection = EditorGUILayout.Popup ("Query: ", itemByString2, messageList.ToArray (), GUILayout.Width (270));
							/*if (popupSelection == 0) { // By name...
							//prevAction = root.getInstructionOp (i, j); 
							string prevSelect = root.getInstructionParameter (i, j, 7);
							if (indexOfStringInList (messageList.ToArray (), prevSelect) != 0) { // reset textFieldItem
								textFieldItem2 = "";
							}
							textFieldItem2 = EditorGUILayout.TextField (textFieldItem2, GUILayout.Width (200));
						} else { // not by name*/
							textFieldItem2 = messageList.ToArray () [popupSelection];
							/*
						}*/
							root.setInstructionParameter (i, j, startOfValue2Parameters + 1, textFieldItem2);



							// Onto the parameters...


							EditorGUI.indentLevel++;

							ParameterInfo[] par;

							if (popupSelection >= 0) { // make sure a method from the drop down is selected......

								if (methods != null) {
									par = methods [messageIndex [popupSelection]].GetParameters ();
									if (par.Length == 0) {
										EditorGUILayout.LabelField ("(no parameters)");
									}
									for (int pi = 0; pi < par.Length; ++pi) {
										string textFieldItemParam = root.getInstructionParameter (i, j, startOfValue2Parameters + 2 + pi); // make room for 2nd parameter
										EditorGUILayout.LabelField (par [pi].Name + " (" + translateType (par [pi].ParameterType.Name) + ") : ");
										textFieldItemParam = EditorGUILayout.TextField (textFieldItemParam, GUILayout.Width (200));
										root.setInstructionParameter (i, j, startOfValue2Parameters + 2 + pi, textFieldItemParam);
									}
									//root.getInstructionParameter (i, j, startOfValue2Parameters+2+par.Length);
									//root.setInstructionParameter(i, j, startOfValue2Parameters+2+par.Length, "\\"); // end of parameters
									//startOfValue2Parameters = 8+par.Length+1;

								}


							} else { // find out available parameters:
								// not for now, doesn't make a lot of sense... ??
							}


						}
						EditorGUI.indentLevel--;
					} else { // unary operator

						if (opList [popUpItem2].Equals ("In range")) {

							EditorGUILayout.LabelField ("In range from:");
							op1ConstantStr = root.getInstructionParameter (i, j, startOfValue2Parameters);
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							root.setInstructionParameter (i, j, startOfValue2Parameters, op1ConstantStr);

							EditorGUILayout.LabelField ("to:");
							op1ConstantStr = root.getInstructionParameter (i, j, startOfValue2Parameters + 1);
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							root.setInstructionParameter (i, j, startOfValue2Parameters + 1, op1ConstantStr);

						}
						if (opList [popUpItem2].Equals ("Not in range")) {

							EditorGUILayout.LabelField ("Not in range from:");
							op1ConstantStr = root.getInstructionParameter (i, j, startOfValue2Parameters);
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							root.setInstructionParameter (i, j, startOfValue2Parameters, op1ConstantStr);

							EditorGUILayout.LabelField ("to:");
							op1ConstantStr = root.getInstructionParameter (i, j, startOfValue2Parameters + 1);
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							root.setInstructionParameter (i, j, startOfValue2Parameters + 1, op1ConstantStr);
						}

					}

					/* Destination labels */

					EditorGUILayout.LabelField ("If condition met, branch to:");


					EditorGUI.indentLevel++;

					textFieldItem = root.getInstructionParameter (i, j, 2); // Make sure there is room for parameter #1
			
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));

					checkLinks (textFieldItem, nNipple);

					rect = EditorGUILayout.GetControlRect ();
					newRect = new Rect (rect.x + 300 - 24, rect.y - 18, 16, 16);
					EditorGUI.DrawPreviewTexture (newRect, nippleTexture);

					if (Event.current.type == EventType.Repaint) {

						outputNipples.Add (newRect);
						instructionOfOutNipple.Add (j);
						opOfNipple.Add (2);
						NippleRef newNippleRef;
						newNippleRef.instruction = j;
						newNippleRef.answer = 0;
						nipples.Add (newNippleRef);
					}

					++nNipple;
			
					root.setInstructionParameter (i, j, 2, textFieldItem);
					EditorGUI.indentLevel--;



					EditorGUILayout.LabelField ("If not, branch to:");

					EditorGUI.indentLevel++;

					textFieldItem = root.getInstructionParameter (i, j, 3); // Make sure there is room for parameter #1
				
					checkLinks (textFieldItem, nNipple);

					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));

					rect = EditorGUILayout.GetControlRect ();
					newRect = new Rect (rect.x + 300 - 24, rect.y - 18, 16, 16);
					EditorGUI.DrawPreviewTexture (newRect, nippleTexture);

					if (Event.current.type == EventType.Repaint) {

						outputNipples.Add (newRect);
						instructionOfOutNipple.Add (j);
						opOfNipple.Add (3);
						NippleRef newNippleRef;
						newNippleRef.instruction = j;
						newNippleRef.answer = 1;
						nipples.Add (newNippleRef);
					}

					++nNipple;
					//} else { // not by name
					//	textFieldItem = labelList [popupSelection];

					//}
					root.setInstructionParameter (i, j, 3, textFieldItem);

					EditorGUI.indentLevel--;



					EditorGUI.indentLevel--;
					break;


				case 24: // Store data...
					// 0: "storeData"   1: DataType   2: key  3: value
					EditorGUI.indentLevel++;

					textFieldItem = root.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (dataTypes, textFieldItem);
					selected = EditorGUILayout.Popup ("Data type: ", selected, dataTypes, GUILayout.Width (270));
					root.setInstructionParameter (i, j, 1, dataTypes [selected]);

					textFieldItem = root.getInstructionParameter (i, j, 2);
					EditorGUILayout.PrefixLabel ("Key: ");
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (270));
					root.setInstructionParameter (i, j, 2, textFieldItem);

					if (dataTypes [selected].Equals ("String")) {
						//textFieldItem = root.getInstructionParameter (i, j, 3);
						//textFieldItem = "String";
						//root.setInstructionParameter (i, j, 3, textFieldItem);
						EditorGUILayout.PrefixLabel ("String: ");
						textFieldItem = root.getInstructionParameter (i, j, 3);
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
						root.setInstructionParameter (i, j, 3, textFieldItem);
					}
					if (dataTypes [selected].Equals ("Integer")) {
						//textFieldItem = root.getInstructionParameter (i, j, 3);
						//textFieldItem = "Int";
						//root.setInstructionParameter (i, j, 3, textFieldItem);
						EditorGUILayout.PrefixLabel ("Integer: ");
						textFieldItem = root.getInstructionParameter (i, j, 3);
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
						root.setInstructionParameter (i, j, 3, textFieldItem);
					}
					if (dataTypes [selected].Equals ("Float")) {
						//textFieldItem = root.getInstructionParameter (i, j, 3);
						//textFieldItem = "Float";
						//root.setInstructionParameter (i, j, 3, textFieldItem);
						EditorGUILayout.PrefixLabel ("Float: ");
						textFieldItem = root.getInstructionParameter (i, j, 3);
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
						root.setInstructionParameter (i, j, 3, textFieldItem);
					}
					if (dataTypes [selected].Equals ("Boolean")) {
						//textFieldItem = root.getInstructionParameter (i, j, 3);
						//textFieldItem = "Bool";
						//root.setInstructionParameter (i, j, 3, textFieldItem);
						EditorGUILayout.PrefixLabel ("Boolean: ");
						textFieldItem = root.getInstructionParameter (i, j, 3);
						bool b;
						bool.TryParse (textFieldItem, out b);
						b = EditorGUILayout.Toggle (b);
						if (b)
							textFieldItem = "true";
						else
							textFieldItem = "false";
						root.setInstructionParameter (i, j, 3, textFieldItem);
					}

					EditorGUI.indentLevel--;
					break;


				case 25: // update data

					// 0 "updateData"   1 DataType   2 Key   3 Operation   4 Constant/Retrieval  5 Value/Key

					EditorGUI.indentLevel++;
					bool opIsBoolean = false;
					bool opIsString = false;
					textFieldItem = root.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (dataTypes, textFieldItem);
					selected = EditorGUILayout.Popup ("Data type: ", selected, dataTypes, GUILayout.Width (270));
					root.setInstructionParameter (i, j, 1, dataTypes [selected]);

					textFieldItem = root.getInstructionParameter (i, j, 2);
					EditorGUILayout.PrefixLabel ("Key: ");
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (270));
					root.setInstructionParameter (i, j, 2, textFieldItem);

					if (dataTypes [selected].Equals ("Boolean")) {

						opIsBoolean = true;
					}
					if (dataTypes [selected].Equals ("String")) {
						opIsString = true;
					}

					textFieldItem = root.getInstructionParameter (i, j, 3);
					EditorGUI.indentLevel++;
					EditorGUILayout.PrefixLabel ("Operation: ");
					string[] mathoperations = new string[] {
						"Add", 
						"Substract",
						"Maximum",
						"Minimum",
						"Multiply",
						"Divide",
						"Set"
					};
					string[] onlyset = new string[] { "Set" };
					string[] setConcatenate = new string[] { "Concatenate", "Set" };
					if (opIsBoolean) { // only Set allowed
						root.setInstructionParameter (i, j, 3, "Set");
						EditorGUILayout.Popup (0, onlyset, GUILayout.Width (270));
					} else if (opIsString) {
						selected = indexOfStringInList (setConcatenate, textFieldItem);
						selected = EditorGUILayout.Popup (selected, setConcatenate, GUILayout.Width (270));
						root.setInstructionParameter (i, j, 3, setConcatenate [selected]);

					} else {
						selected = indexOfStringInList (mathoperations, textFieldItem);
						selected = EditorGUILayout.Popup (selected, mathoperations, GUILayout.Width (270));
						root.setInstructionParameter (i, j, 3, mathoperations [selected]);
					}
					EditorGUI.indentLevel--;
					EditorGUILayout.PrefixLabel ("Second operator: ");

					textFieldItem = root.getInstructionParameter (i, j, 4);
					string[] secondOperator = new string[] {
						"Constant",
						"Retrieve data"
					};
					selected = indexOfStringInList (secondOperator, textFieldItem);
					selected = EditorGUILayout.Popup (selected, secondOperator, GUILayout.Width (180));
					root.setInstructionParameter (i, j, 4, secondOperator [selected]);

					if (secondOperator [selected].Equals ("Retrieve data")) {
						EditorGUILayout.PrefixLabel ("Key: ");
					}
					if (opIsBoolean) {
						textFieldItem = root.getInstructionParameter (i, j, 5);
						bool b;
						bool.TryParse (textFieldItem, out b);
						b = EditorGUILayout.Toggle (b);
						if (b)
							textFieldItem = "true";
						else
							textFieldItem = "false";
						root.setInstructionParameter (i, j, 5, textFieldItem);
					} else {
						textFieldItem = root.getInstructionParameter (i, j, 5);
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
						root.setInstructionParameter (i, j, 5, textFieldItem);
					}
					//} else {



					//}



					EditorGUI.indentLevel--;
					break;

				case 26: // replace program
					EditorGUI.indentLevel++;
					textFieldItem = root.getInstructionParameter (i, j, 1);
					string[] nextProgramName = new string[root.NextProgram.Length];
					for (int w = 0; w < root.NextProgram.Length; ++w) {
						nextProgramName [w] = root.NextProgram [w].name;
					}
					selected = indexOfStringInList (nextProgramName, textFieldItem);
					EditorGUILayout.LabelField ("Next program object: ", GUILayout.Width (200));
					selected = EditorGUILayout.Popup (selected, nextProgramName, GUILayout.Width (200));
					if (root.NextProgram.Length > 0)
						root.setInstructionParameter (i, j, 1, nextProgramName [selected]);
					else
						root.setInstructionParameter (i, j, 1, "");
					EditorGUILayout.LabelField ("Event: ", GUILayout.Width (200));

					int objectIndex = selected;

					if (root.NextProgram.Length > 0) {
						textFieldItem = root.getInstructionParameter (i, j, 2);
						selected = indexOfStringInList (root.NextProgram [objectIndex].eventNames (), textFieldItem);
						selected = EditorGUILayout.Popup (selected, root.NextProgram [objectIndex].eventNames (), GUILayout.Width (200));
						if (root.NextProgram [objectIndex].eventNames ().Length > 0) {
							root.setInstructionParameter (i, j, 2, root.NextProgram [objectIndex].eventNames () [selected]);
						}
					}

					textFieldItem = root.getInstructionParameter (i, j, 3);
					bool immediate;
					bool.TryParse (textFieldItem, out immediate);
					bool newImmediate;
					newImmediate = EditorGUILayout.ToggleLeft ("Continue execution", immediate);
					textFieldItem = newImmediate.ToString ();
					root.setInstructionParameter (i, j, 3, textFieldItem);


					EditorGUI.indentLevel--;
					break;

				case 27: // record conversation
					EditorGUI.indentLevel++;
					textFieldItem = root.getInstructionParameter (i, j, 1);
					EditorGUILayout.PrefixLabel ("Title: ");
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					currentStringCollector.Add (textFieldItem);
					root.setInstructionParameter (i, j, 1, textFieldItem);

					textFieldItem = root.getInstructionParameter (i, j, 2);
					//textFieldItem = root.name + i + "_" + strIndex;
					textFieldItem = "Conv_" + folder + "_" + windowTitle + "_" + strIndex;
					root.setInstructionParameter (i, j, 2, textFieldItem);
					++strIndex;

					EditorGUI.indentLevel--;
					break;

				case 28: // alert
					EditorGUI.indentLevel++;
					textFieldItem = root.getInstructionParameter (i, j, 1);
					textFieldItem = EditorGUILayout.TextArea (textFieldItem, GUILayout.MinWidth (80), GUILayout.MaxWidth (270), GUILayout.ExpandWidth (false), GUILayout.ExpandHeight (false));
					currentStringCollector.Add (textFieldItem);
					root.setInstructionParameter (i, j, 1, textFieldItem);

					textFieldItem = root.getInstructionParameter (i, j, 2);
					EditorGUILayout.LabelField ("Autoclose delay (0 for never):");
					textFieldItem = EditorGUILayout.TextArea (textFieldItem, GUILayout.Width (80));
					root.setInstructionParameter (i, j, 2, textFieldItem);


					textFieldItem = root.getInstructionParameter (i, j, 3);
					waitForTouch = false;
					if (textFieldItem.Equals ("wait"))
						waitForTouch = true;
					waitForTouch = EditorGUILayout.ToggleLeft (" Wait until read", waitForTouch);
					if (waitForTouch)
						textFieldItem = "wait";
					else
						textFieldItem = "don't wait";
					root.setInstructionParameter (i, j, 3, textFieldItem);


					textFieldItem = root.getInstructionParameter (i, j, 4);
					//textFieldItem = root.name + i + "_" + strIndex;
                    textFieldItem = "Conv_" + folder + "_" + windowTitle + "_" + strIndex;
                    root.setInstructionParameter (i, j, 4, textFieldItem);
					++strIndex;

					EditorGUI.indentLevel--;

					break;
				case 30: // follow player
					break;

				case 31: // say image
					EditorGUI.indentLevel++;

					string[] imgList = new string[root.sayImages.Length];

					for (int idx = 0; idx < root.sayImages.Length; ++idx) {
						if (root.sayImages [idx] != null) {
							imgList [idx] = AssetDatabase.GetAssetPath (root.sayImages [idx]);
						} else
							imgList [idx] = "";
					}

					textFieldItem = root.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (imgList, textFieldItem);
					selected = EditorGUILayout.Popup (selected, imgList, GUILayout.Width (200));
					textFieldItem = imgList [selected];
					root.setInstructionParameter (i, j, 1, textFieldItem);

					textFieldItem = root.getInstructionParameter (i, j, 2);
					waitForTouch = false;
					if (textFieldItem.Equals ("wait"))
						waitForTouch = true;
					waitForTouch = EditorGUILayout.ToggleLeft (" Wait until read", waitForTouch);
					if (waitForTouch)
						textFieldItem = "wait";
					else
						textFieldItem = "don't wait";
					root.setInstructionParameter (i, j, 2, textFieldItem);

					textFieldItem = root.getInstructionParameter (i, j, 3);
					isnpc1 = false;
					isnpc2 = false;
					isplayer = false;

					if (textFieldItem.Equals ("NPC1"))
						isnpc1 = true;
					if (textFieldItem.Equals ("NPC2"))
						isnpc2 = true;
					if (textFieldItem.Equals ("Player"))
						isplayer = true;

					EditorGUILayout.BeginHorizontal ();
					newnpc1 = EditorGUILayout.ToggleLeft ("NPC1", isnpc1, GUILayout.Width (90));
					newnpc2 = EditorGUILayout.ToggleLeft ("NPC2", isnpc2, GUILayout.Width (90));
					newplayer = EditorGUILayout.ToggleLeft ("Player", isplayer, GUILayout.Width (90));
					EditorGUILayout.EndHorizontal ();

					if (newnpc1 && !isnpc1) {
						root.setInstructionParameter (i, j, 3, "NPC1");

					}
					if (newnpc2 && !isnpc2) {
						root.setInstructionParameter (i, j, 3, "NPC2");

					}
					if (newplayer && !isplayer) {
						root.setInstructionParameter (i, j, 3, "Player");

					}


					EditorGUI.indentLevel--;
					break;

				case 32: // say from stringbank
					EditorGUI.indentLevel++;

					string[] banks = new string[root.stringBank.Length];

					for (int k = 0; k < root.stringBank.Length; ++k) {
						banks [k] = root.stringBank [k].name;
					}

					textFieldItem = root.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (banks, textFieldItem);
					selected = EditorGUILayout.Popup (selected, banks, GUILayout.Width (200));
					textFieldItem = banks [selected];

					EditorGUI.indentLevel--;
					break;

				case 33: // send mail     1: the contents id
					EditorGUI.indentLevel++;

					textFieldItem = root.getInstructionParameter (i, j, 1);
					EditorGUILayout.PrefixLabel ("Content id: ");
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					root.setInstructionParameter (i, j, 1, textFieldItem);

					EditorGUI.indentLevel--;
					break;

				case 34:
					break;

				case 35:
					EditorGUI.indentLevel++;
					textFieldItem = root.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (options, textFieldItem);
					selected = EditorGUILayout.Popup (selected, options, GUILayout.Width (160));
					textFieldItem = options [selected];
					root.setInstructionParameter (i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;

				case 36:
					EditorGUI.indentLevel++;
					textFieldItem = root.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (options, textFieldItem);
					selected = EditorGUILayout.Popup (selected, options, GUILayout.Width (160));
					textFieldItem = options [selected];
					root.setInstructionParameter (i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;

				case 37: // disable answer
					EditorGUI.indentLevel++;
					textFieldItem = root.getInstructionParameter (i, j, 2);
					//EditorGUILayout.BeginHorizontal ();

					bool all;
					bool.TryParse (textFieldItem, out all);
					all = EditorGUILayout.ToggleLeft ("All", all);
					if (all)
						textFieldItem = "true";
					else
						textFieldItem = "false";
					root.setInstructionParameter (i, j, 2, textFieldItem);


					textFieldItem = root.getInstructionParameter (i, j, 3);


					EditorGUILayout.BeginHorizontal ();
					bool retrieve;
					bool.TryParse (textFieldItem, out retrieve);

					retrieve = EditorGUILayout.ToggleLeft ("Retrieve value", retrieve, GUILayout.Width (90));
					if (EditorGUILayout.ToggleLeft ("Constant", !retrieve, GUILayout.Width (90))) {
						retrieve = false;
					} else
						retrieve = true;

					EditorGUILayout.EndHorizontal ();

					if (retrieve)
						textFieldItem = "true";
					else
						textFieldItem = "false";
					root.setInstructionParameter (i, j, 3, textFieldItem);
					//EditorGUILayout.EndHorizontal ();

					textFieldItem = root.getInstructionParameter(i, j, 1);
					textFieldItem = EditorGUILayout.TextField(textFieldItem, GUILayout.Width(120));
					root.setInstructionParameter(i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;

				case 38: // enable answer
					EditorGUI.indentLevel++;
					textFieldItem = root.getInstructionParameter (i, j, 2);
					//EditorGUILayout.BeginHorizontal ();
				
					bool.TryParse (textFieldItem, out all);
					all = EditorGUILayout.ToggleLeft ("All", all);
					if (all)
						textFieldItem = "true";
					else
						textFieldItem = "false";
					root.setInstructionParameter (i, j, 2, textFieldItem);


					textFieldItem = root.getInstructionParameter (i, j, 3);


					EditorGUILayout.BeginHorizontal ();
					bool.TryParse (textFieldItem, out retrieve);

					retrieve = EditorGUILayout.ToggleLeft ("Retrieve value", retrieve, GUILayout.Width (120));
					if (EditorGUILayout.ToggleLeft ("Constant", !retrieve, GUILayout.Width (120))) {
						retrieve = false;
					} else
						retrieve = true;

					EditorGUILayout.EndHorizontal ();

					if (retrieve)
						textFieldItem = "true";
					else
						textFieldItem = "false";
					root.setInstructionParameter (i, j, 3, textFieldItem);
					//EditorGUILayout.EndHorizontal ();

					textFieldItem = root.getInstructionParameter(i, j, 1);
					textFieldItem = EditorGUILayout.TextField(textFieldItem, GUILayout.Width(120));
					root.setInstructionParameter(i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;

				case 39:
					EditorGUI.indentLevel++;

					EditorGUI.indentLevel--;
					break;

				case 40:
					EditorGUI.indentLevel++;

					textFieldItem = root.getInstructionParameter (i, j, 1);
					EditorGUILayout.LabelField ("Name or index:");
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (150));
					root.setInstructionParameter (i, j, 1, textFieldItem);

					EditorGUI.indentLevel--;
					break;

				}

			
				if (GUILayout.Button ("-", GUILayout.Width (30))) { // if - button pressed...
					//root.deleteInstructionFromProgram(i, j);
					deleted = true;
					deleteInstruction = j;
					adjustProgramReferences (j, -1);
					EditorUtility.SetDirty (this);
					//Rect r = this.windowRect;
					//r.height -= 100.0f;
					//this.windowRect = r;
					//root.addAction(i);
					//root.addInstructionToProgram (i, "None"); // to be altered later
				}

				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();

				if (deleted == false) {
					root.setTypeOfAction (i, j, act);
					root.setInstructionOp (i, j, actionsMethodName [act]);
				}

			} // end of j

			if (deleteInstruction != -1) {
				root.deleteInstructionFromProgram (i, deleteInstruction);
				root.removeAction (i, deleteInstruction);
				deleteInstruction = -1;
			}

			if (insertInstruction != -1) {
				root.insertInstructionIntoProgram (i, insertInstruction, "None");
				root.insertAction (i, insertInstruction);
				insertInstruction = -1;
			}

			if (GUILayout.Button ("+", GUILayout.Width (30))) { // if + button pressed...
				root.addAction (i);
				root.addInstructionToProgram (i, "None"); // to be altered later
				EditorUtility.SetDirty (this);
			}



			EditorGUI.indentLevel--;


			if (root.isRoot) {

		
				if (GUILayout.Button ("Copy program", GUILayout.Width (100))) { 
					
					theEditor.programToString();

				}
				if (GUILayout.Button ("Paste program", GUILayout.Width (100))) { 
					
					theEditor.parseProgram(EditorGUIUtility.systemCopyBuffer, actionsMethodName, this, this.root.name);
				}

			}

		} // end of i

		if (pasteEvent != -1) {
			root.parseProgram (pasteEvent, EditorGUIUtility.systemCopyBuffer, actionsMethodName);
			pasteEvent = 1;
		}
					

		if (root.isRoot) {

			if (GUILayout.Button ("Create/update prefab")) {
				
				bool finish = false;
				int node = 0;
				ConversationNode curNode = this;
				while(!finish) {
					ListString2D c1 = curNode.root.currentProgram;
					ListString2D c2 = curNode.root.programs [0];
					curNode.root.currentProgram = curNode.root.programs [0];

                    PrefabUtility.SaveAsPrefabAssetAndConnect(curNode.root.gameObject, "Assets/Resources/Prefabs/NodeEditorObjects/" + folder + "/" + curNode.windowTitle + ".prefab", InteractionMode.AutomatedAction);
			
					int nsb = curNode.stringCollector.Count;
					for (int index = 0; index < nsb; ++index) { // nsb is supposed to be 1 (number of programs/events)
						GameObject newStringBankGO = new GameObject ();
						StringBank newStringBank = newStringBankGO.AddComponent<StringBank> ();
						newStringBank.reset ();
						newStringBank.phrase = new string[curNode.stringCollector [index].Count];
						//newStringBank.extra = curNode.root.name + index + "_";
						newStringBank.extra = "Conv_" + folder + "_" + curNode.windowTitle + "_";
						for (int jindex = 0; jindex < curNode.stringCollector [index].Count; ++jindex) {
							newStringBank.phrase [jindex] = curNode.stringCollector [index] [jindex];
						}

                        PrefabUtility.SaveAsPrefabAssetAndConnect(newStringBankGO, "Assets/Prefabs/StringBanks/ProgramStringBanks/StringBank(Conversation_" + folder + "_" + curNode.windowTitle + ").prefab", InteractionMode.AutomatedAction);

                    }
					if (node <= theEditor.windows.Count) {
						curNode = theEditor.windows [node++];
					} else
						finish = true;
				}


			}
						
			if (GUILayout.Button ("Reset")) {

				//root.initialize ();
				root.reset ();
				root.addEvent ();
				root.addProgram ();
				root.registerEventName ("none"); // to be altered later

			}

		}

		/* set window height */
		if (Event.current.type == EventType.Repaint) {
			Rect last = GUILayoutUtility.GetLastRect ();
			Rect r = this.windowRect;
			r.height = last.y + 60.0f;
			this.windowRect = r;
			EditorUtility.SetDirty (this);
		}

	} // end of DrawWindow



	public string translateType(string SystemType) {

		if (SystemType.Equals ("Int32"))
			return "int";

		if (SystemType.Equals ("Single"))
			return "float";

		return SystemType;

	}

	public void adjustProgramReferences(int insertionPoint, int delta) {

		string thisName = root.name;

		for (int w = 0; w < theEditor.windows.Count; ++w) {

			//string thisName = theEditor.windows[w].root.name;
			ListString2D prog = theEditor.windows[w].root.programs [0];

			for (int i = 0; i < prog.theList.Count; ++i) {

				ListString1D instr = prog.theList [i];
				if (instr.theList [0].Equals ("branchTo")) {
					if (i >= insertionPoint) {
						string[] targetFields = instr.theList [1].Split ('/');
						if (targetFields [0].Equals (thisName)) {
							int targetIndex;
							int.TryParse (targetFields [1], out targetIndex);
							targetIndex += delta;
							instr.theList [1] = thisName + "/" + targetIndex; // update nipple destination
						}
					}
				}
				if (instr.theList [0].Equals ("if")) {
					if (i >= insertionPoint) {

					}
				}
				if (instr.theList [0].Equals ("ask")) {
					if (i >= insertionPoint) {

					}
				}

			}

		}

	}


	public override void DrawCurves() {



	}


	Color parseColor(string s) {

		int tempVal, AVal, ZVal, ZeroVal, NineVal;

		Color res = Color.black;

		char cH, cL;
		int[] channelValue = new int[4];

		if (s.Length != 9)
			return res;

		if (s [0] != '#')
			return res;

		AVal = (int)'A';
		ZVal = (int)'Z';
		ZeroVal = (int)'0';
		NineVal = (int)'9';

		for(int i=0; i<4; ++i) {
			cH = s [i*2 + 1]; cL = s [i*2 + 2];
			channelValue[i] = 0;
			tempVal = (int)cL;

			if(cL >= '0' && cL <= '9') {
				channelValue[i] += ((int)cL) - ((int)'0');
			}
			else if(cL >= 'A' && cL <= 'F') {

				channelValue[i] += (10 + ((int)cL) - ((int)'A'));
			}
			else if(cL >= 'a' && cL <= 'z') {
				channelValue[i] += (10 + ((int)cL) - ((int)'a'));
			}
			if(cH >= '0' && cH <= '9') {
				channelValue[i] += (((int)cH) - ((int)'0'))<<4;
			}
			else if(cH >= 'A' && cH <= 'F') {
				channelValue[i] += (10 + (((int)cH) - ((int)'A')))<<4;
			}
			else if(cH >= 'a' && cH <= 'z') {
				channelValue[i] += (10 + (((int)cH) - ((int)'a')))<<4;
			}

		}

		res.r = ((float)channelValue [0]) / 255.0f;
		res.g = ((float)channelValue [1]) / 255.0f;
		res.b = ((float)channelValue [2]) / 255.0f;
		res.a = ((float)channelValue [3]) / 255.0f;

		return res;

	}

	public string colorToString(Color c) {

		string res = "#";

		int channelValue;
		int HighVal, LoVal;
		float[] vals = new float[4];

		string append = " ";

		vals [0] = c.r;
		vals [1] = c.g;
		vals [2] = c.b;
		vals [3] = c.a;

		for (int i = 0; i < 4; ++i) {
			channelValue = (int)(vals[i] * 255.0f);
			LoVal = channelValue % 16;
			HighVal = channelValue >> 4;

			switch (HighVal) {
			case 0:
				res = res + "0";
				break;
			case 1:
				res = res + "1";
				break;
			case 2:
				res = res + "2";
				break;
			case 3:
				res = res + "3";
				break;
			case 4:
				res = res + "4";
				break;
			case 5:
				res = res + "5";
				break;
			case 6:
				res = res + "6";
				break;
			case 7:
				res = res + "7";
				break;
			case 8:
				res = res + "8";
				break;
			case 9:
				res = res + "9";
				break;
			case 10:
				res = res + "A";
				break;
			case 11:
				res = res + "B";
				break;
			case 12:
				res = res + "C";
				break;
			case 13:
				res = res + "D";
				break;
			case 14:
				res = res + "E";
				break;
			case 15:
				res = res + "F";
				break;
			}
			switch (LoVal) {
			case 0:
				res = res + "0";
				break;
			case 1:
				res = res + "1";
				break;
			case 2:
				res = res + "2";
				break;
			case 3:
				res = res + "3";
				break;
			case 4:
				res = res + "4";
				break;
			case 5:
				res = res + "5";
				break;
			case 6:
				res = res + "6";
				break;
			case 7:
				res = res + "7";
				break;
			case 8:
				res = res + "8";
				break;
			case 9:
				res = res + "9";
				break;
			case 10:
				res = res + "A";
				break;
			case 11:
				res = res + "B";
				break;
			case 12:
				res = res + "C";
				break;
			case 13:
				res = res + "D";
				break;
			case 14:
				res = res + "E";
				break;
			case 15:
				res = res + "F";
				break;
			}


		}

		return res;
	}

	void checkLinks(string s, int numNipple) {

		if (hasCorrectLinkSyntax (s)) { // target is a link, not a label...
			int endOfField = s.IndexOf('/');
			string target = s.Substring (0, endOfField);
			string targetInstrStr = s.Substring (endOfField + 1);
			int targetInstrInt;
			if (int.TryParse (targetInstrStr, out targetInstrInt)) {

				// update subProgram index
				int indexInt = 0;
				int sp = 0;

				int startOfField = target.IndexOf ('_');
				if (startOfField == -1) {
					sp = 0;
				} else {
					string indexStr = target.Substring (startOfField + 1);
					if (int.TryParse (indexStr, out indexInt)) {
						sp = indexInt + 1;
					}
				} 
				if (sp > theEditor.subProgram)
					theEditor.subProgram = sp;

				int xPos = 350 * (indexInt + 1);

				int targetWindow = 0;
				int thisWindow = 0;

				// find out if `target` window already exists, also find out index of this window
				bool found = false;
				for (int k = 0; k < theEditor.windows.Count; ++k) {
					if (theEditor.windows [k].windowTitle.Equals (target)) {
						targetWindow = k;
						found = true;
					}
					if (theEditor.windows [k].windowTitle.Equals (this.windowTitle)) {
						thisWindow = k;
					}
				}

				if (!found) { // create window if it's not been created yet
					// Instantiate prefab
					string nodeName = target;
					ConversationNode newNode = new ConversationNode ();
					newNode.folder = folder;
					newNode.windowTitle = target;

					newNode.theEditor = theEditor;
					//editor.windows.Add (newNode);
					GameObject woGO = Instantiate (Resources.Load ("Prefabs/NodeEditorObjects/" + folder + "/" + nodeName, typeof(GameObject))) as GameObject;
					ConversationGenerator wo = woGO.GetComponent<ConversationGenerator> ();
					newNode.rootGO = woGO;
					newNode.root = wo;
					newNode.root.music = root.music;
					newNode.root.soundEffects = root.soundEffects;
					newNode.root.sayImages = root.sayImages;
					newNode.windowRect = new Rect (newNode.root.windowX, newNode.root.windowY, 300, 240);
					theEditor.windows.Add (newNode);
				}


				// then, add the link (this function checks for duplicates)
				theEditor.addConnection(thisWindow, numNipple, targetWindow, targetInstrInt);

			} // end of if(TryParse

		}// end of if correctSyntax

	}

	public void parseProgram(string prg) {
		//root.parseProgram(0, prg, 
	}

}

#endif