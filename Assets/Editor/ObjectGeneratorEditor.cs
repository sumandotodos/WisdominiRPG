#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Reflection;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

//[CustomEditor(typeof())]
[CustomEditor(typeof(ObjectGeneratorScript))]
public class ObjectGeneratorEditor : Editor {

	public Texture tex;
	bool haveSprite;

	List<List<string>> stringCollector;

	int indexOfStringInList(string[] list, string str) {

		for (int i = 0; i < list.Length; ++i) {
			if (str.Equals (list [i]))
				return i;
		}
		return 0;
	}

	string removeExtension(string filename) 
	{
		int i;
		char[] array;
		array = filename.ToCharArray ();
		for (i = filename.Length - 1; i > 0; --i) {
			if (array [i] == '.')
				break;
		}
		if (i >= 0) {
			return filename.Substring (0, i);
		} else
			return filename;
	}

	public override void OnInspectorGUI() 
	{
		stringCollector = new List<List<string>> ();
		List<string> currentStringCollector;

		int strIndex = 0;
		int ne;
		int selected = 0;

		string[] targetList;
		string[] sfxList;
		string[] musList;

		DrawDefaultInspector ();

		int tlLength;

		int sfxLength;

		int musLength;

		int popupSelection;

		string[] options = new string[] { 
			"onPickup", 
			"onOverlap", 
			"onEndOverlap", 
			"onAwake",
			"onInteract",
			"customEvent1", 
			"customEvent2", 	
			"customEvent3" };

		string[] actions = new string[] {
			"Add To Inventory", 		// 0
			"Destroy", 					// 1
			"Send Message...", 			// 2
			"Start action...", 			// 3
			"Delay...", 				// 4
			"Update Gauge...",			// 5
			"Play Sound...", 			// 6
			"Play Music...",			// 7
			"Change location...",		// 8
			"Go To Hell",				// 9
			"Block Player Controls",	// 10
			"Unblock Player Controls",	// 11
			"Loop",						// 12
			"New Branch Target...",		// 13
			"Branch to...",				// 14
			"Conditional branch...",	// 15
			"Store data...",			// 16
			"Update data...",			// 17
			"Alert...",					// 18
			"ChispAlert...", 			// 19
			"Close ChispAlert",			// 20
			"Save Game",				// 21
			"Delete Save Game", 		// 22
			"Hide",						// 23
			"Run event...",				// 24
			"Replace program...",		// 25
			"Disable interaction",		// 26
			"Active",					// 27
			"Desactive",				// 28
			"Enable interaction",		// 29
			"Alert from controller",		// 30
			"Dip music"						// 31

		}; 

		string[] actionsMethodName = new string[] {
			"addToInventory", 
			"destroy", 
			"sendMessage", 
			"startAction", 
			"delay", 
			"updateGauge",
			"playSound", 
			"playMusic",
			"setLocation",
			"goToHell",
			"blockPlayerControls",
			"unblockPlayerControls",
			"loop",
			"makeLabel",
			"branchTo",
			"if",
			"storeData",
			"updateData",
			"alert",
			"chispAlert",
			"closeChispAlert",
			"saveGame",
			"deleteSaveGame",
			"hide",
			"runEvent",
			"replaceProgram",
			"disableInteraction",
			"active",
			"desactive",
			"enableInteraction",
			"alertFromController",
			"dipmusic"
		}; 

		string[] gauges = new string[] {
			"Fire Energy",
			"Water Energy",
			"Air Energy",
			"Earth Energy",
			"Blue Mana",
			"Red Mana"
		};

		string[] dataTypes = new string[] 
		{
			"Integer", "Float", "Boolean", "String"
		};

		ObjectGeneratorScript ogRef = (ObjectGeneratorScript)target;

		if (ogRef.rgb == null)
			ogRef.initialize();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		if (GUILayout.Button ("Copy sprites")) 
		{
			string res = "";

			List<Texture[]> images;
			images = new List<Texture[]> ();
			images.Add (ogRef.images);

			for (int i = 0; i < images.Count; ++i) 
			{
				res += (images [i].Length + "\n");
				for (int j = 0; j < images [i].Length; ++j) 
				{
					res += (AssetDatabase.GetAssetPath (images [i] [j]) + "\n");
				}
				res += "|";
			}
			EditorGUIUtility.systemCopyBuffer = res;
		}

		if (GUILayout.Button ("Paste sprites")) 
		{
			string data = EditorGUIUtility.systemCopyBuffer;
			string[] blocks = data.Split ('|');
			if (blocks.Length == 2) 
			{
				string[] lines = blocks [0].Split ('\n');
				int imgLength;
				int.TryParse (lines [0], out imgLength);
				ogRef.imagesSprite = new Sprite[imgLength];
				for (int j = 1; j < lines.Length-1; ++j) {
					string path = lines [j];

					Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite> (path);
					ogRef.imagesSprite [j-1] = tex;
				}
			}
		}

		EditorGUILayout.Space ();

		if (GUILayout.Button ("Fix material"))
		{
			Material Mat;

			if (ogRef.spriteHolder == null) {
				haveSprite = false;
			} else {
				haveSprite = true;
			}

			if (ogRef.commonMat) 
			{
				string matPath = "Assets/Resources/GenericMaterials/Common/" + ogRef.nameMat + ".mat";
				Mat = AssetDatabase.LoadAssetAtPath<Material> (matPath);
				if (haveSprite) 
				{
					if (Mat == null) 
					{
						Mat = new Material (Shader.Find ("Sprites/Default"));
						AssetDatabase.CreateAsset (Mat, "Assets/Resources/GenericMaterials/Common/" + ogRef.nameMat + ".mat");
					}

					ogRef.spriteHolder.GetComponent<SpriteRenderer> ().material = Mat;

					if (ogRef.spriteHolder != null) 
					{
						ogRef.spriteHolder.GetComponent<SpriteRenderer> ().sprite = ogRef.imagesSprite [0];	
					} 
				}

				else 
				{
					if (Mat == null) 
					{
						Mat = new Material (Shader.Find ("Mobile/Diffuse"));
						AssetDatabase.CreateAsset (Mat, "Assets/Resources/GenericMaterials/Common/" + ogRef.nameMat + ".mat");
					}
					ogRef.GetComponentInChildren<MeshRenderer> ().material = Mat;
				}
				return;
			}

			if (haveSprite) 
			{
				if (SceneManager.GetActiveScene ().name.Contains ("Plane")) {
					string plane;

					if (SceneManager.GetActiveScene ().name.Contains ("+") || SceneManager.GetActiveScene ().name.Contains ("-")) {
						plane = SceneManager.GetActiveScene ().name.Substring (6, 7);
					} else {
						plane = SceneManager.GetActiveScene ().name.Substring (6, 6);
					}

					string lvl = SceneManager.GetActiveScene ().name.Substring (0, 6);
					string matPath = "Assets/Resources/GenericMaterials/" + lvl + "/" + plane + "/" + ogRef.nameMat + ".mat";
					Mat = AssetDatabase.LoadAssetAtPath<Material> (matPath);

					if (Mat == null) 
					{
						Mat = new Material (Shader.Find ("Sprites/Default"));

						if (!AssetDatabase.IsValidFolder ("Assets/Resources/GenericMaterials/" + lvl)) 
						{
							AssetDatabase.CreateFolder ("Assets/Resources/GenericMaterials", lvl);
						} 

						if (!AssetDatabase.IsValidFolder ("Assets/Resources/GenericMaterials/" + lvl + "/" + plane))
						{
							AssetDatabase.CreateFolder ("Assets/Resources/GenericMaterials/" + lvl, plane);
						}

						AssetDatabase.CreateAsset (Mat, "Assets/Resources/GenericMaterials/" + lvl + "/" + plane + "/" + ogRef.nameMat + ".mat");
					}
					ogRef.spriteHolder.GetComponent<SpriteRenderer> ().material = Mat;

					if (ogRef.spriteHolder != null) 
					{
						ogRef.spriteHolder.GetComponent<SpriteRenderer> ().sprite = ogRef.imagesSprite [0];	
					} 
				} 

				else {
					string lvl = SceneManager.GetActiveScene ().name;
					string matPath = "Assets/Resources/GenericMaterials/" + lvl + "/" + ogRef.nameMat;
					Mat = AssetDatabase.LoadAssetAtPath<Material> (matPath);

					if (Mat == null) 
					{
						Mat = new Material (Shader.Find ("Sprites/Default"));

						if (!AssetDatabase.IsValidFolder ("Assets/Resources/GenericMaterials/" + lvl)) 
						{
							AssetDatabase.CreateFolder ("Assets/Resources/GenericMaterials", lvl);
						} 

						AssetDatabase.CreateAsset (Mat, "Assets/Resources/GenericMaterials/" + lvl + "/" + ogRef.nameMat + ".mat");
					}
					ogRef.spriteHolder.GetComponent<SpriteRenderer> ().material = Mat;

					if (ogRef.spriteHolder != null) 
					{
						ogRef.spriteHolder.GetComponent<SpriteRenderer> ().sprite = ogRef.imagesSprite [0];	
					} 
				}
			}

			else 
			{
				if (SceneManager.GetActiveScene ().name.Contains ("Plane")) {
					string plane;

					if (SceneManager.GetActiveScene ().name.Contains ("+") || SceneManager.GetActiveScene ().name.Contains ("-")) {
						plane = SceneManager.GetActiveScene ().name.Substring (6, 7);
					} else {
						plane = SceneManager.GetActiveScene ().name.Substring (6, 6);
					}

					string lvl = SceneManager.GetActiveScene ().name.Substring (0, 6);
					string matPath = "Assets/Resources/GenericMaterials/" + lvl + "/" + plane + "/" + ogRef.nameMat + ".mat";
					Mat = AssetDatabase.LoadAssetAtPath<Material> (matPath);

					if (Mat == null) 
					{
						Mat = new Material (Shader.Find ("Mobile/Diffuse"));

						if (!AssetDatabase.IsValidFolder ("Assets/Resources/GenericMaterials/" + lvl)) 
						{
							AssetDatabase.CreateFolder ("Assets/Resources/GenericMaterials", lvl);
						} 

						if (!AssetDatabase.IsValidFolder ("Assets/Resources/GenericMaterials/" + lvl + "/" + plane))
						{
							AssetDatabase.CreateFolder ("Assets/Resources/GenericMaterials/" + lvl, plane);
						}

						AssetDatabase.CreateAsset (Mat, "Assets/Resources/GenericMaterials/" + lvl + "/" + plane + "/" + ogRef.nameMat + ".mat");
					}
					//ogRef.spriteHolder.GetComponent<SpriteRenderer> ().material = Mat;
					ogRef.GetComponentInChildren<MeshRenderer> ().material = Mat;

				} 
				else {
					string lvl = SceneManager.GetActiveScene ().name;
					string matPath = "Assets/Resources/GenericMaterials/" + lvl + "/" + ogRef.nameMat;
					Mat = AssetDatabase.LoadAssetAtPath<Material> (matPath);

					if (Mat == null) 
					{
						Mat = new Material (Shader.Find ("Mobile/Diffuse"));

						if (!AssetDatabase.IsValidFolder ("Assets/Resources/GenericMaterials/" + lvl)) 
						{
							AssetDatabase.CreateFolder ("Assets/Resources/GenericMaterials", lvl);
						} 

						AssetDatabase.CreateAsset (Mat, "Assets/Resources/GenericMaterials/" + lvl + "/" + ogRef.nameMat + ".mat");
					}
					ogRef.GetComponentInChildren<MeshRenderer> ().material = Mat;
				}
			}
		}

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		// Reset button cleans up the data structure
		if (GUILayout.Button("Reset")) 
		{
			ogRef.initialize ();
		}

		// Build list of possible message targets
		// Plus "By name..." option in case a target
		// is not available on design-time

		tlLength = ogRef.messageTargets.Length;
		targetList = new string[tlLength + 1];
		targetList [0] = "By name...";
		for (int m = 0; m < tlLength; ++m) {
			GameObject anObject;
			anObject = ogRef.messageTargets [m];
			if (anObject != null)
				targetList [m+1] = ogRef.messageTargets [m].name;
			else
				targetList [m+1] = "<no reference>";
		}

		sfxLength = ogRef.soundEffects.Length;
		sfxList = new string[sfxLength + 1];
		sfxList [0] = "By name...";
		for (int m = 0; m < sfxLength; ++m) {
			AudioClip anObject;
			anObject = ogRef.soundEffects [m];
			if (anObject != null)
				sfxList [m + 1] = ogRef.soundEffects [m].name;
			else
				sfxList [m+1] = "<no reference>";			
		}

		musLength = ogRef.music.Length;
		musList = new string[musLength + 1];
		musList [0] = "By name...";
		for (int m = 0; m < musLength; ++m) {
			AudioClip anObject;
			anObject = ogRef.music [m];
			if (anObject != null)
				musList [m+1] = ogRef.music [m].name;
			else
				musList [m+1] = "<no reference>";
		}

		ne = ogRef.getEvents ();

		/* Name field */
		EditorGUILayout.LabelField ("Name: ");
		ogRef.name = EditorGUILayout.TextField (ogRef.name);

		/* is pickable field */
		//EditorGUILayout.LabelField ("Pickable: ");
		ogRef.pickable = EditorGUILayout.ToggleLeft (" Pickable", ogRef.pickable);


		//if (ogRef.getTex()) {
		//	EditorGUI.DrawPreviewTexture (Rect (0, 0, 40, 40), ogRef.getTex());
		//}

		bool deleted;
		int deleteInstruction = -1;
		int insertInstruction = -1;
		int pasteEvent = -1;

		for (int i = 0; i < ne; ++i) { // loop through all programs (events)
			int editorSelected;
			editorSelected = EditorGUILayout.Popup ("Event: ", ogRef.getTypeOfEvent(i), options);
			ogRef.setTypeOfEvent (i, editorSelected);
			ogRef.alterEvent (i, options [editorSelected]);


			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			currentStringCollector = new List<string> ();
			stringCollector.Add (currentStringCollector);



			int na;
			na = ogRef.nActions (i); // number of actions of event i

			string prevAction;
			int itemByString;
			string textFieldItem;

			EditorGUI.indentLevel++;

			strIndex = 0;
			for (int j = 0; j < na; ++j) {  // loop through all actions (instructions) for event i
				// instruction j in program i

				if (GUILayout.Button ("+", GUILayout.Width (30))) { // if + button pressed...
					//ogRef.addAction(i);
					insertInstruction = j;
					//ogRef.addInstructionToProgram (i, "None"); // to be altered later
				}

				deleted = false;

				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();

				int act;
				act = EditorGUILayout.Popup ("Action: ", ogRef.getTypeOfAction(i, j), actions, GUILayout.Width (300));
				//EditorGUILayout.LabelField ("Blocking: ");
				switch(act) {


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
					textFieldItem = ogRef.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1
					itemByString = indexOfStringInList (targetList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("To whom: ", itemByString, targetList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						//prevAction = ogRef.getInstructionOp (i, j); 
						string prevSelect = ogRef.getInstructionParameter (i, j, 1);
						if (indexOfStringInList (targetList, prevSelect) != 0) { // reset textFieldItem
							textFieldItem = "";
						}
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					} else { // not by name
						textFieldItem = targetList [popupSelection];

					}
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);


					messageList.Add ("By name...");

					methods = null;

					//if (popupSelection == 0) { // if target was By Name...
					if (!textFieldItem.Equals ("")) {
						GameObject go = GameObject.Find (textFieldItem); // find GO with that name...
						if (go != null) {
							WisdominiObject wo = go.GetComponent<WisdominiObject> ();
							if (wo != null) {
								methods = wo.GetType ().GetMethods ();
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
					}


					//}

					string textFieldItem2 = ogRef.getInstructionParameter (i, j, 2); // make room for 2nd parameter
					int itemByString2 = indexOfStringInList (messageList.ToArray (), textFieldItem2);
					popupSelection = EditorGUILayout.Popup ("What message: ", itemByString2, messageList.ToArray (), GUILayout.Width (200));
					if (popupSelection == 0) { // By name...
						//prevAction = ogRef.getInstructionOp (i, j); 
						string prevSelect = ogRef.getInstructionParameter (i, j, 2);
						if (indexOfStringInList (messageList.ToArray (), prevSelect) != 0) { // reset textFieldItem
							textFieldItem2 = "";
						}
						textFieldItem2 = EditorGUILayout.TextField (textFieldItem2, GUILayout.Width (200));
					} else { // not by name
						textFieldItem2 = messageList.ToArray () [popupSelection];

					}
					ogRef.setInstructionParameter (i, j, 2, textFieldItem2);


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
								string textFieldItemParam = ogRef.getInstructionParameter (i, j, 3+pi); // make room for 2nd parameter
								EditorGUILayout.LabelField (par [pi].Name + " (" + translateType (par [pi].ParameterType.Name) + ") : ");
								textFieldItemParam = EditorGUILayout.TextField (textFieldItemParam, GUILayout.Width (200));
								ogRef.setInstructionParameter (i, j, 3+pi, textFieldItemParam);
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
				 *
				case 3: // Start Action...

					messageList = new List<string> ();
					messageIndex = new List<int> ();


					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1
					itemByString = indexOfStringInList (targetList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("From whom: ", itemByString, targetList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						//prevAction = ogRef.getInstructionOp (i, j); 
						string prevSelect = ogRef.getInstructionParameter (i, j, 1);
						if (indexOfStringInList (targetList, prevSelect) != 0) { // reset textFieldItem
							textFieldItem = "";
						}
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					} else { // not by name
						textFieldItem = targetList [popupSelection];

					}
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);


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

					textFieldItem2 = ogRef.getInstructionParameter (i, j, 2); // make room for 2nd parameter
					itemByString2 = indexOfStringInList (messageList.ToArray (), textFieldItem2);
					popupSelection = EditorGUILayout.Popup ("What action: ", itemByString2, messageList.ToArray (), GUILayout.Width (200));
					if (popupSelection == 0) { // By name...
						//prevAction = ogRef.getInstructionOp (i, j); 
						string prevSelect = ogRef.getInstructionParameter (i, j, 2);
						if (indexOfStringInList (messageList.ToArray (), prevSelect) != 0) { // reset textFieldItem
							textFieldItem2 = "";
						}
						textFieldItem2 = EditorGUILayout.TextField (textFieldItem2, GUILayout.Width (200));
					} else { // not by name
						textFieldItem2 = messageList.ToArray () [popupSelection];

					}
					ogRef.setInstructionParameter (i, j, 2, textFieldItem2);

					string isBlockingStr = ogRef.getInstructionParameter (i, j, 3); // make room for 2nd parameter
					bool isBlocking;
					if (isBlockingStr.Equals ("blocking"))
						isBlocking = true;
					else
						isBlocking = false;
					
					/*
					 * Onto the parameters...
					 */
					/*

					EditorGUI.indentLevel++;

					if (popupSelection > 0) { // make sure a method from the drop down is selected......

						if (methods != null) {
							ParameterInfo[] par = methods [messageIndex [popupSelection - 1]].GetParameters ();
							if (par.Length == 1) {
								EditorGUILayout.LabelField ("(no parameters)");
							}
							for (int pi = 1; pi < par.Length; ++pi) {
								string textFieldItemParam = ogRef.getInstructionParameter (i, j, 3+pi); // make room for 2nd parameter
								EditorGUILayout.LabelField (par [pi].Name + " (" + translateType (par [pi].ParameterType.Name) + ") : ");
								textFieldItemParam = EditorGUILayout.TextField (textFieldItemParam, GUILayout.Width (200));
								ogRef.setInstructionParameter (i, j, 3+pi, textFieldItemParam);
							}

						}


					} else { // find out available parameters:
						// not for now, doesn't make a lot of sense... ??
					}

					EditorGUI.indentLevel--;

					/*
					 *  Blocking or non-blocking action??
					 */ 
					/*
					isBlocking = EditorGUILayout.ToggleLeft (" Blocking ", isBlocking);
					if (isBlocking)
						ogRef.setInstructionParameter (i, j, 3, "blocking");
					else ogRef.setInstructionParameter(i, j, 3, "non-blocking");


					EditorGUI.indentLevel--;


					break;
					*/

				case 3: // Start Action...

					messageList = new List<string> ();
					messageIndex = new List<int> ();


					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1
					itemByString = indexOfStringInList (targetList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("From whom: ", itemByString, targetList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						//prevAction = ogRef.getInstructionOp (i, j); 
						string prevSelect = ogRef.getInstructionParameter (i, j, 1);
						if (indexOfStringInList (targetList, prevSelect) != 0) { // reset textFieldItem
							textFieldItem = "";
						}
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					} else { // not by name
						textFieldItem = targetList [popupSelection];

					}
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);


					messageList.Add ("By name...");

					methods = null;

					//if (popupSelection == 0) { // if target was By Name...
					if (!textFieldItem.Equals ("")) {
						GameObject go = GameObject.Find (textFieldItem); // find GO with that name...
						if (go != null) {
							WisdominiObject wo = go.GetComponent<WisdominiObject> ();
							if (wo != null) {
								methods = wo.GetType ().GetMethods ();
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
					}


					//}

					textFieldItem2 = ogRef.getInstructionParameter (i, j, 2); // make room for 2nd parameter
					itemByString2 = indexOfStringInList (messageList.ToArray (), textFieldItem2);
					popupSelection = EditorGUILayout.Popup ("What action: ", itemByString2, messageList.ToArray (), GUILayout.Width (200));
					if (popupSelection == 0) { // By name...
						//prevAction = ogRef.getInstructionOp (i, j); 
						string prevSelect = ogRef.getInstructionParameter (i, j, 2);
						if (indexOfStringInList (messageList.ToArray (), prevSelect) != 0) { // reset textFieldItem
							textFieldItem2 = "";
						}
						textFieldItem2 = EditorGUILayout.TextField (textFieldItem2, GUILayout.Width (200));
					} else { // not by name
						textFieldItem2 = messageList.ToArray () [popupSelection];

					}
					ogRef.setInstructionParameter (i, j, 2, textFieldItem2);

					string isBlockingStr = ogRef.getInstructionParameter (i, j, 3); // make room for 2nd parameter
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
								string textFieldItemParam = ogRef.getInstructionParameter (i, j, 3+pi); // make room for 2nd parameter
								EditorGUILayout.LabelField (par [pi].Name + " (" + translateType (par [pi].ParameterType.Name) + ") : ");
								textFieldItemParam = EditorGUILayout.TextField (textFieldItemParam, GUILayout.Width (200));
								ogRef.setInstructionParameter (i, j, 3+pi, textFieldItemParam);
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
						ogRef.setInstructionParameter (i, j, 3, "blocking");
					else ogRef.setInstructionParameter(i, j, 3, "non-blocking");


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
					textFieldItem = ogRef.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1
					prevAction = ogRef.getInstructionOp (i, j); 
					if(!prevAction.Equals("delay")) { // If we just changed action, set to 0.0
						textFieldItem = "0.0";
					}
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (130));
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);
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
					textFieldItem = ogRef.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1
					itemByString = indexOfStringInList(sfxList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("Sound: ", itemByString, sfxList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						prevAction = ogRef.getInstructionOp (i, j); 
						if (!prevAction.Equals ("delay")) { // If we just changed action, set to ""
							textFieldItem = "";
						}
						textFieldItem = EditorGUILayout.TextField ("", GUILayout.Width (200));
					} else { // not by name
						textFieldItem = sfxList[popupSelection];

					}
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;



				case 7: // Play music
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1
					itemByString = indexOfStringInList(musList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("Music: ", itemByString, musList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						prevAction = ogRef.getInstructionOp (i, j); 
						if (!prevAction.Equals ("delay")) { // If we just changed action, set to ""
							textFieldItem = "";
						}
						textFieldItem = EditorGUILayout.TextField ("", GUILayout.Width (200));
					} else { // not by name
						textFieldItem = musList[popupSelection];

					}
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;



				case 8: // change location
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1
					/*
					DirectoryInfo scenesPath = new DirectoryInfo (Application.dataPath);
					FileInfo[] files = scenesPath.GetFiles ("*.unity", SearchOption.AllDirectories);
					string[] fileList;
					int fileListLength = files.Length + 1;
					fileList = new string[fileListLength];
					fileList [0] = "By name...";
					for (int k = 1; k < fileListLength; ++k) {
						fileList [k] = removeExtension(files [k-1].Name);
					}
*/
					string[] fileList;
					fileList = new string[1];
					fileList [0] = "By name...";
					itemByString = 0;//indexOfStringInList(fileList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("New location: ", itemByString, fileList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						//prevAction = ogRef.getInstructionOp (i, j); 
						//if (!prevAction.Equals ("setLocation")) { // If we just changed action, set to ""
						//	textFieldItem = "";
						//}
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					}
					else { // not by name
						textFieldItem = fileList[popupSelection];

					}

					ogRef.setInstructionParameter (i, j, 1, textFieldItem);

					EditorGUI.indentLevel--;
					break;



				case 9: // go to hell
					EditorGUI.indentLevel++;
					EditorGUILayout.Popup ("New hell: ", 0, gauges, GUILayout.Width (270));
					EditorGUI.indentLevel--;
					break;



				case 10: // block player controls
					break;



				case 11: // unblock player controls
					break;



				case 12: // loop program
					break;



				case 13: // make target
					EditorGUI.indentLevel++;

					textFieldItem = ogRef.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1

					prevAction = ogRef.getInstructionOp (i, j); 
					if (!prevAction.Equals ("makeLabel")) { // If we just changed action, set to ""
						textFieldItem = "";
					}
					EditorGUILayout.LabelField ("Target name: ");
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));

					ogRef.setInstructionParameter (i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;

					break;




				case 14: // branch to target...
					EditorGUI.indentLevel++;
					string[] labelList;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1
					labelList = ogRef.getAllLabels(i);
					itemByString = indexOfStringInList(labelList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("Target: ", itemByString, labelList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						//prevAction = ogRef.getInstructionOp (i, j); 
						string prevSelect = ogRef.getInstructionParameter (i, j, 1);
						if (indexOfStringInList (labelList, prevSelect) != 0) { // reset textFieldItem
							textFieldItem = "";
						}
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					} else { // not by name
						textFieldItem = labelList[popupSelection];

					}
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;



					// if things change, you have to reset shared fields !!!!
				case 15: // conditional branch


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
					textFieldItem = ogRef.getInstructionParameter (i, j, 4);
					string[] conditionList = { "Constant", "Query", "Retrieve data" };
					popUpItem = indexOfStringInList (conditionList, textFieldItem);
					op1type = popUpItem;
					popUpItem = EditorGUILayout.Popup (popUpItem, conditionList, GUILayout.Width (270));
					ogRef.setInstructionParameter (i, j, 4, conditionList [popUpItem]);
					EditorGUI.indentLevel++;
					if (popUpItem == 0) { // Constant selected, stored at index 6
						op1ConstantStr = ogRef.getInstructionParameter (i, j, 6);
						EditorGUILayout.LabelField ("Constant 1:");
						op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
						ogRef.setInstructionParameter (i, j, 6, op1ConstantStr);

					} else if (popUpItem == 2) { // retrieve data selected
						// index 6: key
						// index 7: type
						op1ConstantStr = ogRef.getInstructionParameter (i, j, 6);
						EditorGUILayout.PrefixLabel ("Key: ");
						op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
						ogRef.setInstructionParameter (i, j, 6, op1ConstantStr);

						op1ConstantStr = ogRef.getInstructionParameter (i, j, 7);
						selected = indexOfStringInList (dataTypes, op1ConstantStr);
						selected = EditorGUILayout.Popup (selected, dataTypes, GUILayout.Width (120));
						ogRef.setInstructionParameter (i, j, 7, dataTypes [selected]);

						startOfValue2Parameters = 8;

					} else if (popUpItem == 1) { // Query Selected

						// cipotera padre here
						EditorGUI.indentLevel++;
						textFieldItem = ogRef.getInstructionParameter (i, j, 6); // Make sure there is room for parameter #1
						itemByString = indexOfStringInList (targetList, textFieldItem);
						popupSelection = EditorGUILayout.Popup ("Who: ", itemByString, targetList, GUILayout.Width (270));
						if (popupSelection == 0) { // By name...
							//prevAction = ogRef.getInstructionOp (i, j); 
							string prevSelect = ogRef.getInstructionParameter (i, j, 6);
							if (indexOfStringInList (targetList, prevSelect) != 0) { // reset textFieldItem
								textFieldItem = "";
							}
							textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
						} else { // not by name
							textFieldItem = targetList [popupSelection];

						}
						ogRef.setInstructionParameter (i, j, 6, textFieldItem);

						messageList = new List<string> ();
						messageIndex = new List<int> ();
						//messageList.Add ("By name...");

						methods = null;

						//if (popupSelection == 0) { // if target was By Name...
						if (!textFieldItem.Equals ("")) {
							GameObject go = GameObject.Find (textFieldItem); // find GO with that name...
							if (go != null) {
								WisdominiObject wissu = go.GetComponent<WisdominiObject> ();
								methods = wissu.GetType ().GetMethods ();
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

						textFieldItem2 = ogRef.getInstructionParameter (i, j, 7); // make room for 2nd parameter
						itemByString2 = indexOfStringInList (messageList.ToArray (), textFieldItem2);
						popupSelection = EditorGUILayout.Popup ("Query: ", itemByString2, messageList.ToArray (), GUILayout.Width (270));
						/*if (popupSelection == 0) { // By name...
							//prevAction = ogRef.getInstructionOp (i, j); 
							string prevSelect = ogRef.getInstructionParameter (i, j, 7);
							if (indexOfStringInList (messageList.ToArray (), prevSelect) != 0) { // reset textFieldItem
								textFieldItem2 = "";
							}
							textFieldItem2 = EditorGUILayout.TextField (textFieldItem2, GUILayout.Width (200));
						} else { // not by name*/
						textFieldItem2 = messageList.ToArray () [popupSelection];
						/*
						}*/
						ogRef.setInstructionParameter (i, j, 7, textFieldItem2);



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
									string textFieldItemParam = ogRef.getInstructionParameter (i, j, 8 + pi); // make room for 2nd parameter
									EditorGUILayout.LabelField (par [pi].Name + " (" + translateType (par [pi].ParameterType.Name) + ") : ");
									textFieldItemParam = EditorGUILayout.TextField (textFieldItemParam, GUILayout.Width (200));
									ogRef.setInstructionParameter (i, j, 8 + pi, textFieldItemParam);
								}
								ogRef.getInstructionParameter (i, j, 8 + par.Length);
								ogRef.setInstructionParameter (i, j, 8 + par.Length, "\\"); // end of parameters
								startOfValue2Parameters = 8 + par.Length + 1;

							}


						} else { // find out available parameters:
							// not for now, doesn't make a lot of sense... ??
						}

					} // end of cipotera aqui

					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField ("Operator:");
					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					string[] opList = { "=", "<", ">", ">=", "<=", "!=", "In range", "Not in range", "Is true", "Is false" };
					int popUpItem2;
					int popUpItem3;
					popUpItem2 = indexOfStringInList (opList, textFieldItem);
					popUpItem2 = EditorGUILayout.Popup (popUpItem2, opList, GUILayout.Width (270));
					ogRef.setInstructionParameter (i, j, 1, opList [popUpItem2]);
					EditorGUI.indentLevel--;
					EditorGUI.indentLevel--;
					if (popUpItem2 < 6) {  // binary operator
						EditorGUILayout.LabelField ("Value 2:");
						textFieldItem = ogRef.getInstructionParameter (i, j, 5);
						popUpItem3 = indexOfStringInList (conditionList, textFieldItem);
						popUpItem3 = EditorGUILayout.Popup (popUpItem3, conditionList, GUILayout.Width (270));
						ogRef.setInstructionParameter (i, j, 5, conditionList [popUpItem3]);
						EditorGUI.indentLevel++;
						if (popUpItem3 == 0) { // Constant selected
							op1ConstantStr = ogRef.getInstructionParameter (i, j, startOfValue2Parameters);
							EditorGUILayout.LabelField ("Constant 2:");
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters, op1ConstantStr);
						} else if (popUpItem3 == 2) { // retrieve data selected
							// index 6: key
							// index 7: type
							op1ConstantStr = ogRef.getInstructionParameter (i, j, startOfValue2Parameters);
							EditorGUILayout.PrefixLabel ("Key: ");
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters, op1ConstantStr);

							op1ConstantStr = ogRef.getInstructionParameter (i, j, startOfValue2Parameters + 1);
							selected = indexOfStringInList (dataTypes, op1ConstantStr);
							selected = EditorGUILayout.Popup (selected, dataTypes, GUILayout.Width (120));
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters + 1, dataTypes [selected]);


						} else { // Query Selected
							// cipotera padre here

							// cipotera padre here
							EditorGUI.indentLevel++;
							textFieldItem = ogRef.getInstructionParameter (i, j, startOfValue2Parameters); // Make sure there is room for parameter #1
							itemByString = indexOfStringInList (targetList, textFieldItem);
							popupSelection = EditorGUILayout.Popup ("Who: ", itemByString, targetList, GUILayout.Width (270));
							if (popupSelection == 0) { // By name...
								//prevAction = ogRef.getInstructionOp (i, j); 
								string prevSelect = ogRef.getInstructionParameter (i, j, startOfValue2Parameters);
								if (indexOfStringInList (targetList, prevSelect) != 0) { // reset textFieldItem
									textFieldItem = "";
								}
								textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
							} else { // not by name
								textFieldItem = targetList [popupSelection];

							}
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters, textFieldItem);

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

							textFieldItem2 = ogRef.getInstructionParameter (i, j, startOfValue2Parameters + 1); // make room for 2nd parameter
							itemByString2 = indexOfStringInList (messageList.ToArray (), textFieldItem2);
							popupSelection = EditorGUILayout.Popup ("Query: ", itemByString2, messageList.ToArray (), GUILayout.Width (270));
							/*if (popupSelection == 0) { // By name...
							//prevAction = ogRef.getInstructionOp (i, j); 
							string prevSelect = ogRef.getInstructionParameter (i, j, 7);
							if (indexOfStringInList (messageList.ToArray (), prevSelect) != 0) { // reset textFieldItem
								textFieldItem2 = "";
							}
							textFieldItem2 = EditorGUILayout.TextField (textFieldItem2, GUILayout.Width (200));
						} else { // not by name*/
							textFieldItem2 = messageList.ToArray () [popupSelection];
							/*
						}*/
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters + 1, textFieldItem2);



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
										string textFieldItemParam = ogRef.getInstructionParameter (i, j, startOfValue2Parameters + 2 + pi); // make room for 2nd parameter
										EditorGUILayout.LabelField (par [pi].Name + " (" + translateType (par [pi].ParameterType.Name) + ") : ");
										textFieldItemParam = EditorGUILayout.TextField (textFieldItemParam, GUILayout.Width (200));
										ogRef.setInstructionParameter (i, j, startOfValue2Parameters + 2 + pi, textFieldItemParam);
									}
									//ogRef.getInstructionParameter (i, j, startOfValue2Parameters+2+par.Length);
									//ogRef.setInstructionParameter(i, j, startOfValue2Parameters+2+par.Length, "\\"); // end of parameters
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
							op1ConstantStr = ogRef.getInstructionParameter (i, j, startOfValue2Parameters);
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters, op1ConstantStr);

							EditorGUILayout.LabelField ("to:");
							op1ConstantStr = ogRef.getInstructionParameter (i, j, startOfValue2Parameters + 1);
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters + 1, op1ConstantStr);

						}
						if (opList [popUpItem2].Equals ("Not in range")) {

							EditorGUILayout.LabelField ("Not in range from:");
							op1ConstantStr = ogRef.getInstructionParameter (i, j, startOfValue2Parameters);
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters, op1ConstantStr);

							EditorGUILayout.LabelField ("to:");
							op1ConstantStr = ogRef.getInstructionParameter (i, j, startOfValue2Parameters + 1);
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters + 1, op1ConstantStr);
						}

					}

					/* Destination labels */

					EditorGUILayout.LabelField ("If condition met, branch to:");


					EditorGUI.indentLevel++;

					textFieldItem = ogRef.getInstructionParameter (i, j, 2); // Make sure there is room for parameter #1
					labelList = ogRef.getAllLabels (i);
					itemByString = indexOfStringInList (labelList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("Target: ", itemByString, labelList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						//prevAction = ogRef.getInstructionOp (i, j); 
						string prevSelect = ogRef.getInstructionParameter (i, j, 2);
						if (indexOfStringInList (labelList, prevSelect) != 0) { // reset textFieldItem
							textFieldItem = "";
						}
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					} else { // not by name
						textFieldItem = labelList [popupSelection];

					}
					ogRef.setInstructionParameter (i, j, 2, textFieldItem);
					EditorGUI.indentLevel--;



					EditorGUILayout.LabelField ("If not, branch to:");

					EditorGUI.indentLevel++;

					textFieldItem = ogRef.getInstructionParameter (i, j, 3); // Make sure there is room for parameter #1
					labelList = ogRef.getAllLabels (i);
					itemByString = indexOfStringInList (labelList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("Target: ", itemByString, labelList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						//prevAction = ogRef.getInstructionOp (i, j); 
						string prevSelect = ogRef.getInstructionParameter (i, j, 3);
						if (indexOfStringInList (labelList, prevSelect) != 0) { // reset textFieldItem
							textFieldItem = "";
						}
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					} else { // not by name
						textFieldItem = labelList [popupSelection];

					}
					ogRef.setInstructionParameter (i, j, 3, textFieldItem);

					if (popUpItem == 2) {
						EditorGUI.indentLevel--;
						EditorGUI.indentLevel--;
					}

					EditorGUI.indentLevel--;
					EditorGUI.indentLevel--;

					EditorGUI.indentLevel = 0;
					break;


				case 16: // Store data...
					// 0: "storeData"   1: DataType   2: key  3: value
					EditorGUI.indentLevel++;

					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (dataTypes, textFieldItem);
					selected = EditorGUILayout.Popup ("Data type: ", selected, dataTypes, GUILayout.Width (270));
					ogRef.setInstructionParameter (i, j, 1, dataTypes [selected]);

					textFieldItem = ogRef.getInstructionParameter (i, j, 2);
					EditorGUILayout.PrefixLabel ("Key: ");
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (270));
					ogRef.setInstructionParameter (i, j, 2, textFieldItem);

					if (dataTypes [selected].Equals ("String")) {
						//textFieldItem = ogRef.getInstructionParameter (i, j, 3);
						//textFieldItem = "String";
						//ogRef.setInstructionParameter (i, j, 3, textFieldItem);
						EditorGUILayout.PrefixLabel ("String: ");
						textFieldItem = ogRef.getInstructionParameter (i, j, 3);
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
						ogRef.setInstructionParameter (i, j, 3, textFieldItem);
					}
					if (dataTypes [selected].Equals ("Integer")) {
						//textFieldItem = ogRef.getInstructionParameter (i, j, 3);
						//textFieldItem = "Int";
						//ogRef.setInstructionParameter (i, j, 3, textFieldItem);
						EditorGUILayout.PrefixLabel ("Integer: ");
						textFieldItem = ogRef.getInstructionParameter (i, j, 3);
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
						ogRef.setInstructionParameter (i, j, 3, textFieldItem);
					}
					if (dataTypes [selected].Equals ("Float")) {
						//textFieldItem = ogRef.getInstructionParameter (i, j, 3);
						//textFieldItem = "Float";
						//ogRef.setInstructionParameter (i, j, 3, textFieldItem);
						EditorGUILayout.PrefixLabel ("Float: ");
						textFieldItem = ogRef.getInstructionParameter (i, j, 3);
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
						ogRef.setInstructionParameter (i, j, 3, textFieldItem);
					}
					if (dataTypes [selected].Equals ("Boolean")) {
						//textFieldItem = ogRef.getInstructionParameter (i, j, 3);
						//textFieldItem = "Bool";
						//ogRef.setInstructionParameter (i, j, 3, textFieldItem);
						EditorGUILayout.PrefixLabel ("Boolean: ");
						textFieldItem = ogRef.getInstructionParameter (i, j, 3);
						bool b;
						bool.TryParse (textFieldItem, out b);
						b = EditorGUILayout.Toggle (b);
						if (b)
							textFieldItem = "true";
						else
							textFieldItem = "false";
						ogRef.setInstructionParameter (i, j, 3, textFieldItem);
					}

					EditorGUI.indentLevel--;
					break;


				case 17: // update data

					// 0 "updateData"   1 DataType   2 Key   3 Operation   4 Constant/Retrieval  5 Value/Key

					EditorGUI.indentLevel++;
					bool opIsBoolean = false;
					bool opIsString = false;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (dataTypes, textFieldItem);
					selected = EditorGUILayout.Popup ("Data type: ", selected, dataTypes, GUILayout.Width (270));
					ogRef.setInstructionParameter (i, j, 1, dataTypes [selected]);

					textFieldItem = ogRef.getInstructionParameter (i, j, 2);
					EditorGUILayout.PrefixLabel ("Key: ");
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (270));
					ogRef.setInstructionParameter (i, j, 2, textFieldItem);

					if (dataTypes [selected].Equals ("Boolean")) {

						opIsBoolean = true;
					}
					if (dataTypes [selected].Equals ("String")) {
						opIsString = true;
					}

					textFieldItem = ogRef.getInstructionParameter (i, j, 3);
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
					string[] onlyset = new string[] { "Set", "Invert" };
					string[] setConcatenate = new string[] { "Concatenate", "Set" };
					if (opIsBoolean) { // only Set allowed
						selected = indexOfStringInList (onlyset, textFieldItem);
						selected = EditorGUILayout.Popup (selected, onlyset, GUILayout.Width (270));
						ogRef.setInstructionParameter (i, j, 3, onlyset [selected]);
					} else if (opIsString) {
						selected = indexOfStringInList (setConcatenate, textFieldItem);
						selected = EditorGUILayout.Popup (selected, setConcatenate, GUILayout.Width (270));
						ogRef.setInstructionParameter (i, j, 3, setConcatenate[selected]);

					}
					else {
						selected = indexOfStringInList (mathoperations, textFieldItem);
						selected = EditorGUILayout.Popup (selected, mathoperations, GUILayout.Width (270));
						ogRef.setInstructionParameter (i, j, 3, mathoperations [selected]);
					}
					EditorGUI.indentLevel--;
					EditorGUILayout.PrefixLabel ("Second operator: ");

					textFieldItem = ogRef.getInstructionParameter (i, j, 4);
					string[] secondOperator = new string[] {
						"Constant",
						"Retrieve data"
					};
					selected = indexOfStringInList (secondOperator, textFieldItem);
					selected = EditorGUILayout.Popup (selected, secondOperator, GUILayout.Width (180));
					ogRef.setInstructionParameter (i, j, 4, secondOperator [selected]);

					if (secondOperator [selected].Equals ("Retrieve data")) {
						EditorGUILayout.PrefixLabel ("Key: ");
					}
					if (opIsBoolean) {
						textFieldItem = ogRef.getInstructionParameter (i, j, 5);
						bool b;
						bool.TryParse (textFieldItem, out b);
						b = EditorGUILayout.Toggle (b);
						if (b)
							textFieldItem = "true";
						else
							textFieldItem = "false";
						ogRef.setInstructionParameter (i, j, 5, textFieldItem);
					} else {
						textFieldItem = ogRef.getInstructionParameter (i, j, 5);
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
						ogRef.setInstructionParameter (i, j, 5, textFieldItem);
					}
					//} else {
					EditorGUI.indentLevel--;
					break;

					/* 0: alert    1: msg    2: autotimeout (0 for never)  3: blocking   4: rosetta id */
				case 18: // alert
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					textFieldItem = EditorGUILayout.TextArea (textFieldItem, GUILayout.MinWidth (80), GUILayout.MaxWidth (270), GUILayout.ExpandWidth (false), GUILayout.ExpandHeight (true));
					currentStringCollector.Add (textFieldItem);
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);

					textFieldItem = ogRef.getInstructionParameter (i, j, 2);
					EditorGUILayout.LabelField ("Autoclose delay (0 for never):");
					textFieldItem = EditorGUILayout.TextArea (textFieldItem, GUILayout.Width (80));
					ogRef.setInstructionParameter (i, j, 2, textFieldItem);


					textFieldItem = ogRef.getInstructionParameter (i, j, 3);
					bool waitForTouch = false;
					if (textFieldItem.Equals ("wait"))
						waitForTouch = true;
					waitForTouch = EditorGUILayout.ToggleLeft (" Wait until read", waitForTouch);
					if (waitForTouch)
						textFieldItem = "wait";
					else
						textFieldItem = "don't wait";
					ogRef.setInstructionParameter (i, j, 3, textFieldItem);


					textFieldItem = ogRef.getInstructionParameter (i, j, 4);
					textFieldItem = ogRef.name + i + "_" + strIndex;
					ogRef.setInstructionParameter (i, j, 4, textFieldItem);
					++strIndex;

					EditorGUI.indentLevel--;

					break;


					/* 0: chispAlert    1: msg  2: blocking   3: rosetta id */
				case 19: // chispAlert
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					textFieldItem = EditorGUILayout.TextArea (textFieldItem, GUILayout.MinWidth (80), GUILayout.MaxWidth (270), GUILayout.ExpandWidth (false), GUILayout.ExpandHeight (true));
					currentStringCollector.Add (textFieldItem);
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);


					textFieldItem = ogRef.getInstructionParameter (i, j, 2);
					waitForTouch = false;
					if (textFieldItem.Equals ("wait"))
						waitForTouch = true;
					waitForTouch = EditorGUILayout.ToggleLeft (" Wait until read", waitForTouch);
					if (waitForTouch)
						textFieldItem = "wait";
					else
						textFieldItem = "don't wait";
					ogRef.setInstructionParameter (i, j, 2, textFieldItem);


					textFieldItem = ogRef.getInstructionParameter (i, j, 3);
					textFieldItem = ogRef.name + i + "_" + strIndex;
					ogRef.setInstructionParameter (i, j, 3, textFieldItem);
					++strIndex;

					EditorGUI.indentLevel--;

					break;

				case 20: // close chispAlert    
					break;

				case 21: // save game
					break;

				case 22:
					break;

				case 23:
					break;

				case 24:
					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (options, textFieldItem);
					selected = EditorGUILayout.Popup (selected, options);
					textFieldItem = options [selected];
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);
					break;

				case 25: // replace program
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					string[] nextProgramName = new string[ogRef.NextProgram.Length];
					for (int w = 0; w < ogRef.NextProgram.Length; ++w) {
						nextProgramName [w] = ogRef.NextProgram [w].name;
					}
					selected = indexOfStringInList (nextProgramName, textFieldItem);
					EditorGUILayout.LabelField ("Next program object: ", GUILayout.Width (200));
					selected = EditorGUILayout.Popup (selected, nextProgramName, GUILayout.Width (200));
					if (ogRef.NextProgram.Length > 0)
						ogRef.setInstructionParameter (i, j, 1, nextProgramName [selected]);
					else
						ogRef.setInstructionParameter (i, j, 1, "");
					EditorGUILayout.LabelField ("Event: ", GUILayout.Width (200));

					int objectIndex = selected;

					if (ogRef.NextProgram.Length > 0) {
						textFieldItem = ogRef.getInstructionParameter (i, j, 2);
						selected = indexOfStringInList (ogRef.NextProgram [objectIndex].eventNames (), textFieldItem);
						selected = EditorGUILayout.Popup (selected, ogRef.NextProgram [objectIndex].eventNames (), GUILayout.Width (200));
						if (ogRef.NextProgram [objectIndex].eventNames ().Length > 0) {
							ogRef.setInstructionParameter (i, j, 2, ogRef.NextProgram [objectIndex].eventNames () [selected]);
						}
					}

					textFieldItem = ogRef.getInstructionParameter (i, j, 3);
					bool immediate;
					bool.TryParse (textFieldItem, out immediate);
					bool newImmediate;
					newImmediate = EditorGUILayout.ToggleLeft ("Continue execution", immediate);
					textFieldItem = newImmediate.ToString ();
					ogRef.setInstructionParameter (i, j, 3, textFieldItem);


					EditorGUI.indentLevel--;
					break;

				case 26:
					break;

				case 27:
					break;

				case 28:
					break;

				case 31: // dip music
					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField ("Seconds: ");
					textFieldItem = ogRef.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1
					prevAction = ogRef.getInstructionOp (i, j); 
					if(!prevAction.Equals("dipmusic")) { // If we just changed action, set to 0.0
						textFieldItem = "0.0";
					}
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (130));
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;
					

				}


				if (GUILayout.Button ("-", GUILayout.Width (30))) { // if - button pressed...
					//ogRef.deleteInstructionFromProgram(i, j);
					deleted = true;
					deleteInstruction = j;
					//ogRef.addAction(i);
					//ogRef.addInstructionToProgram (i, "None"); // to be altered later
				}

				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();

				if (deleted == false) {
					ogRef.setTypeOfAction (i, j, act);
					ogRef.setInstructionOp (i, j, actionsMethodName [act]);
				}

			} // end of j

			if (deleteInstruction != -1) {
				ogRef.deleteInstructionFromProgram(i, deleteInstruction);
				ogRef.removeAction (i, deleteInstruction);
				deleteInstruction = -1;
			}

			if (insertInstruction != -1) {
				ogRef.insertInstructionIntoProgram (i, insertInstruction, "None");
				ogRef.insertAction (i, insertInstruction);
				insertInstruction = -1;
			}

			if (GUILayout.Button ("+", GUILayout.Width (30))) { // if + button pressed...
				ogRef.addAction(i);
				ogRef.addInstructionToProgram (i, "None"); // to be altered later
			}



			EditorGUI.indentLevel--;

			if (GUILayout.Button ("Remove event", GUILayout.Width(100))) { // if Remove button pressed...
				ogRef.removeEvent (i); // tell target to remove event i
			}
			if (GUILayout.Button ("Copy event", GUILayout.Width(100))) { 
				EditorGUIUtility.systemCopyBuffer = ogRef.programToString (i); 
			}
			if (GUILayout.Button ("Paste event", GUILayout.Width(100))) { 
				//EditorGUIUtility.systemCopyBuffer = ogRef.programToString (i); 
				pasteEvent = i;
			}

		} // end of i

		if (pasteEvent != -1) {
			ogRef.parseProgram(pasteEvent, EditorGUIUtility.systemCopyBuffer, actionsMethodName);
			pasteEvent = -1;
		}

		if (GUILayout.Button("Add event", GUILayout.Width(100))) {
			ogRef.addEvent ();
			ogRef.addProgram ();
			ogRef.registerEventName ("none"); // to be altered later
		}


		if (GUILayout.Button("Generate StringBanks")) 
		{
			//ogRef.addEvent ();
			if (!ogRef.name.Equals ("")) {
				Object prefab;
				
				/* Create stringbank for all events on this program */
				int nsb = stringCollector.Count;
				for (int index = 0; index < nsb; ++index) {
					GameObject newStringBankGO = new GameObject ();
					StringBank newStringBank = newStringBankGO.AddComponent<StringBank> ();
					newStringBank.reset ();
					newStringBank.phrase = new string[stringCollector [index].Count];
					newStringBank.extra = ogRef.name + index + "_";
					for (int jindex = 0; jindex < stringCollector [index].Count; ++jindex) {
						newStringBank.phrase [jindex] = stringCollector [index] [jindex];
					}

                    PrefabUtility.SaveAsPrefabAssetAndConnect(newStringBankGO, "Assets/Prefabs/StringBanks/ProgramStringBanks/StringBank(" + ogRef.name + "Event" + index + ").prefab", InteractionMode.AutomatedAction);
				}
			}
		}

		if (GUI.changed) {
			EditorUtility.SetDirty (ogRef);
			EditorSceneManager.MarkSceneDirty (ogRef.gameObject.scene);
		}
	}



	public string translateType(string SystemType) {

		if (SystemType.Equals ("Int32"))
			return "int";

		if (SystemType.Equals ("Single"))
			return "float";

		return SystemType;

	}



}

#endif