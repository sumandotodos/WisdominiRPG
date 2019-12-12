#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Reflection;

//[CustomEditor(typeof())]
[CustomEditor(typeof(CharacterGenerator))]
public class CharacterGeneratorEditor : Editor {

	/* references */

	public Texture tex;


	List<List<string>> stringCollector;
	// pick up says, asks and alerts



	/* properties */

	int eyedropIndex = 0;
	bool coordsAvailable;
	float raycastX, raycastZ;
	Color[] indexedColor;
	const int nColors = 6;
	int currentColor = 0;
	//List<colorBranch> colorList;

	bool withReplace;
	string replaceOld;
	string replaceNew;

	bool editingCoords = false;

	/* methods */

	int indexOfStringInList(string[] list, string str) {

		for (int i = 0; i < list.Length; ++i) {
			if (str.Equals (list [i]))
				return i;
		}
		return 0;

	}

	string removeExtension(string filename) {

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

	void OnSceneGUI()
	{

		Event e = Event.current;

		if (!editingCoords) {
			return;
		}

		// We use hotControl to lock focus onto the editor (to prevent deselection)
		int controlID = GUIUtility.GetControlID(FocusType.Passive);

		switch (Event.current.GetTypeForControl(controlID))
		{
		case EventType.MouseDown:
			GUIUtility.hotControl = controlID;
			//Debug.Log("Mouse Down!");
			Ray ray = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				Vector3 myPos = hit.point;
				//PlaceObject(myPos);
				coordsAvailable = true;
				raycastX = myPos.x;
				raycastZ = myPos.z;
			}
			editingCoords = false;
			//Event.current.Use();
			break;

		case EventType.MouseUp:
			GUIUtility.hotControl = 0;
			//Event.current.Use();
			break;
		}
	}

	public override void OnInspectorGUI() 
	{
		stringCollector = new List<List<string>> ();
		List<string> currentStringCollector;

		int currentIndex = 0;
		currentColor = 0;

		int ne;
		int selected = 0;
		int strIndex = 0;

		string[] targetList;
		string[] sfxList;
		string[] musList;

		indexedColor = new Color[nColors];
		indexedColor [0] = Color.red;
		indexedColor [1] = new Color (0.3f, 0.5f, 1.0f);
		indexedColor [2] = Color.green;
		indexedColor [3] = Color.yellow;
		indexedColor [4] = Color.magenta;
		indexedColor [5] = Color.cyan;

		DrawDefaultInspector ();

		int tlLength;

		int sfxLength;

		int musLength;

		int popupSelection;

		string[] options = new string[] { 
			"onAwake",
			"onSpeak",
			"onHit",
			"onObstacleProximity",
			"onOverlap",
			"customEvent1", 
			"customEvent2", 	
			"customEvent3" };

		string[] removeUs = new string[] {
			"Tonta",
			"Gilipó",
			"Eres",
			"La",
			"Gente",
			"Mala",
			"Viejoven"
		};

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
			"Stop event...",			// 22
			"Resume event...",			// 23
			"Store data...",			// 24
			"Update data...",			// 25
			"Replace program...", 		// 26
			"Record conversation...",	// 27
			"Alert...",					// 28
			"ChispAlert...",			// 29
			"Follow Player",			// 30
			"Stop following Player", 	// 31
			"Say Image...",				// 32
			"Say from StringBank...",	// 33
			"Send mail...",				// 34
			"Save Game",				// 35
			"Delete Saved Game",		// 36
			"Run event...",				// 37
			"Play custom animation...",	// 38
			"Stop custom animations",	// 39
			"Hide",						// 40
			"Disable Interaction",		// 41
			"Disable answer...",		// 42
			"Enable answer...",			// 43
			"Reset answers",			// 44
			"Set Autopilot Mode...",	// 45
			"Fade in", 					// 46
			"Fade out",					// 47
			"Retrieve player pos",		// 48
			"Close chispAlert"			// 49
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
			"stopEvent",
			"resumeEvent",
			"storeData",
			"updateData",
			"replaceProgram",
			"recordConversation",
			"alert",
			"chispalert",
			"followPlayer",
			"stopFollowingPlayer", 
			"sayImage",
			"sayFromStringBank",
			"sendMail",
			"saveGame",
			"deleteSavedGame",
			"runEvent",
			"playCustomAnim",
			"stopCustomAnim",
			"hide",
			"disableInteraction",
			"disableAnswer",
			"enableAnswer",
			"resetAnswers",
			"setAutopilotMode",
			"fadeIn",
			"fadeOut",
			"retrievePlayerPos",
			"closeChispAlert"
		}; 

		string[] gauges = new string[] {
			"Fire Energy",
			"Water Energy",
			"Air Energy",
			"Earth Energy",
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

		string[] autopilotModes = new string[] {

			"Absolute", "Relative"

		};

		CharacterGenerator ogRef = (CharacterGenerator)target;


		if (GUILayout.Button ("Copy Textures")) {

			string res = "";

			List<Texture[]> images;
			images = new List<Texture[]> ();
			images.Add (ogRef.Idle);
			images.Add (ogRef.WalkingFront);
			images.Add (ogRef.WalkingFrontLeft);
			images.Add (ogRef.WalkingLeft);
			images.Add (ogRef.WalkingLeftBack);
			images.Add (ogRef.WalkingBack);
			images.Add (ogRef.WalkingBackRight);
			images.Add (ogRef.WalkingRight);
			images.Add (ogRef.WalkingRightFront);
			images.Add (ogRef.customAnimation1);
			images.Add (ogRef.customAnimation2);
			images.Add (ogRef.customAnimation3);

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

		if (GUILayout.Button ("Copy sprites")) {

			string res = "";

			List<Sprite[]> images;
			images = new List<Sprite[]> ();
			images.Add (ogRef.IdleSprite);
			images.Add (ogRef.WalkingFrontSprite);
			images.Add (ogRef.WalkingFrontLeftSprite);
			images.Add (ogRef.WalkingLeftSprite);
			images.Add (ogRef.WalkingLeftBackSprite);
			images.Add (ogRef.WalkingBackSprite);
			images.Add (ogRef.WalkingBackRightSprite);
			images.Add (ogRef.WalkingRightSprite);
			images.Add (ogRef.WalkingRightFrontSprite);
			images.Add (ogRef.customAnimation1Sprite);
			images.Add (ogRef.customAnimation2Sprite);
			images.Add (ogRef.customAnimation3Sprite);

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
			if (blocks.Length == 13) 
			{
				string[] lines = blocks [0].Split ('\n');
				int imgLength;
				int.TryParse (lines [0], out imgLength);
				ogRef.IdleSprite = new Sprite[imgLength];
				for (int j = 1; j < lines.Length-1; ++j) {
					string path = lines [j];
					if (withReplace)
						path = path.Replace (replaceOld, replaceNew);
					Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite> (path);
					ogRef.IdleSprite [j-1] = tex;
					if (withReplace) {
						withReplace = !withReplace;
						withReplace = !withReplace;
					}
				}

				// i = 1;
				lines = blocks [1].Split ('\n');
				int.TryParse (lines [0], out imgLength);
				ogRef.WalkingFrontSprite = new Sprite[imgLength];
				for (int j = 1; j < lines.Length-1; ++j) {
					string path = lines [j];
					if (withReplace)
						path = path.Replace (replaceOld, replaceNew);
					Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite> (path);
					ogRef.WalkingFrontSprite [j-1] = tex;
					if (withReplace) {
						withReplace = !withReplace;
						withReplace = !withReplace;
					}
				}

				// i = 2;
				lines = blocks [2].Split ('\n');
				int.TryParse (lines [0], out imgLength);
				ogRef.WalkingFrontLeftSprite = new Sprite[imgLength];
				for (int j = 1; j < lines.Length-1; ++j) {
					string path = lines [j];
					if (withReplace)
						path = path.Replace (replaceOld, replaceNew);
					Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite> (path);
					ogRef.WalkingFrontLeftSprite [j-1] = tex;
					if (withReplace) {
						withReplace = !withReplace;
						withReplace = !withReplace;
					}
				}

				// i = 3;
				lines = blocks [3].Split ('\n');
				int.TryParse (lines [0], out imgLength);
				ogRef.WalkingLeftSprite = new Sprite[imgLength];
				for (int j = 1; j < lines.Length-1; ++j) {
					string path = lines [j];
					if (withReplace)
						path = path.Replace (replaceOld, replaceNew);
					Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite> (path);
					ogRef.WalkingLeftSprite [j-1] = tex;
					if (withReplace) {
						withReplace = !withReplace;
						withReplace = !withReplace;
					}
				}

				// i = 4;
				lines = blocks [4].Split ('\n');
				int.TryParse (lines [0], out imgLength);
				ogRef.WalkingLeftBackSprite = new Sprite[imgLength];
				for (int j = 1; j < lines.Length-1; ++j) {
					string path = lines [j];
					if (withReplace)
						path = path.Replace (replaceOld, replaceNew);
					Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite> (path);
					ogRef.WalkingLeftBackSprite [j-1] = tex;
					if (withReplace) {
						withReplace = !withReplace;
						withReplace = !withReplace;
					}
				}

				// i = 5;
				lines = blocks [5].Split ('\n');
				int.TryParse (lines [0], out imgLength);
				ogRef.WalkingBackSprite = new Sprite[imgLength];
				for (int j = 1; j < lines.Length-1; ++j) {
					string path = lines [j];
					if (withReplace)
						path = path.Replace (replaceOld, replaceNew);
					Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite> (path);
					ogRef.WalkingBackSprite [j-1] = tex;
					if (withReplace) {
						withReplace = !withReplace;
						withReplace = !withReplace;
					}
				}

				// i = 6;
				lines = blocks [6].Split ('\n');
				int.TryParse (lines [0], out imgLength);
				ogRef.WalkingBackRightSprite = new Sprite[imgLength];
				for (int j = 1; j < lines.Length-1; ++j) {
					string path = lines [j];
					if (withReplace)
						path = path.Replace (replaceOld, replaceNew);
					Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite> (path);
					ogRef.WalkingBackRightSprite [j-1] = tex;
					if (withReplace) {
						withReplace = !withReplace;
						withReplace = !withReplace;
					}
				}

				// i = 7;
				lines = blocks [7].Split ('\n');
				int.TryParse (lines [0], out imgLength);
				ogRef.WalkingRightSprite = new Sprite[imgLength];
				for (int j = 1; j < lines.Length-1; ++j) {
					string path = lines [j];
					if (withReplace)
						path = path.Replace (replaceOld, replaceNew);
					Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite> (path);
					ogRef.WalkingRightSprite [j-1] = tex;
					if (withReplace) {
						withReplace = !withReplace;
						withReplace = !withReplace;
					}
				}

				// i = 8;
				lines = blocks [8].Split ('\n');
				int.TryParse (lines [0], out imgLength);
				ogRef.WalkingRightFrontSprite = new Sprite[imgLength];
				for (int j = 1; j < lines.Length-1; ++j) {
					string path = lines [j];
					if (withReplace)
						path = path.Replace (replaceOld, replaceNew);
					Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite> (path);
					ogRef.WalkingRightFrontSprite [j-1] = tex;
					if (withReplace) {
						withReplace = !withReplace;
						withReplace = !withReplace;
					}
				}

				lines = blocks [9].Split ('\n');
				int.TryParse (lines [0], out imgLength);
				ogRef.customAnimation1Sprite = new Sprite[imgLength];
				for (int j = 1; j < lines.Length-1; ++j) {
					string path = lines [j];
					if (withReplace)
						path = path.Replace (replaceOld, replaceNew);
					Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite> (path);
					ogRef.customAnimation1Sprite [j-1] = tex;
					if (withReplace) {
						withReplace = !withReplace;
						withReplace = !withReplace;
					}
				}

				lines = blocks [10].Split ('\n');
				int.TryParse (lines [0], out imgLength);
				ogRef.customAnimation2Sprite = new Sprite[imgLength];
				for (int j = 1; j < lines.Length-1; ++j) {
					string path = lines [j];
					if (withReplace)
						path = path.Replace (replaceOld, replaceNew);
					Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite> (path);
					ogRef.customAnimation2Sprite [j-1] = tex;
					if (withReplace) {
						withReplace = !withReplace;
						withReplace = !withReplace;
					}
				}

				lines = blocks [11].Split ('\n');
				int.TryParse (lines [0], out imgLength);
				ogRef.customAnimation3Sprite = new Sprite[imgLength];
				for (int j = 1; j < lines.Length-1; ++j) {
					string path = lines [j];
					if (withReplace)
						path = path.Replace (replaceOld, replaceNew);
					Sprite tex = AssetDatabase.LoadAssetAtPath<Sprite> (path);
					ogRef.customAnimation3Sprite [j-1] = tex;
					if (withReplace) {
						withReplace = !withReplace;
						withReplace = !withReplace;
					}
				}
			}
		}

		EditorGUILayout.Space ();

		if (GUILayout.Button ("Fix material")) 
		{
			Material Mat;

			if (SceneManager.GetActiveScene().name.Contains("Plane"))
			{
				string plane;
				if (SceneManager.GetActiveScene ().name.Contains ("+") || SceneManager.GetActiveScene ().name.Contains ("-")) 
				{
					plane = SceneManager.GetActiveScene ().name.Substring (6, 7);
				} else 
				{
					plane = SceneManager.GetActiveScene ().name.Substring (6, 6);
				}

				string lvl = SceneManager.GetActiveScene ().name.Substring (0, 6);
				string matPath = "Assets/Resources/CharacterMaterials/" + lvl + "/" + plane + "/" + ogRef.name + ".mat";
				Mat = AssetDatabase.LoadAssetAtPath<Material> (matPath);

				if (Mat == null) 
				{
					Mat = new Material (Shader.Find ("Sprites/Default"));

					if (!AssetDatabase.IsValidFolder ("Assets/Resources/CharacterMaterials/" + lvl)) 
					{
						AssetDatabase.CreateFolder ("Assets/Resources/CharacterMaterials", lvl);
					} 

					if (!AssetDatabase.IsValidFolder ("Assets/Resources/CharacterMaterials/" + lvl + "/" + plane))
					{
						AssetDatabase.CreateFolder ("Assets/Resources/CharacterMaterials/" + lvl, plane);
					}

					AssetDatabase.CreateAsset (Mat, "Assets/Resources/CharacterMaterials/" + lvl + "/" + plane + "/" + ogRef.name + ".mat");
				}
				ogRef.spriteHolder.GetComponent<SpriteRenderer> ().material = Mat;
				ogRef.spriteHolder.GetComponent<SpriteRenderer> ().sprite = ogRef.IdleSprite [0];
			}
			else 
			{
				string lvl = SceneManager.GetActiveScene ().name;
				string matPath = "Assets/Resources/CharacterMaterials/" + lvl + "/" + ogRef.name;
				Mat = AssetDatabase.LoadAssetAtPath<Material> (matPath);

				if (Mat == null)
				{
					Mat = new Material (Shader.Find ("Sprites/Default"));

					if (!AssetDatabase.IsValidFolder ("Assets/Resources/CharacterMaterials/" + lvl)) 
					{
						AssetDatabase.CreateFolder ("Assets/Resources/CharacterMaterials", lvl);
					} 

					AssetDatabase.CreateAsset (Mat, "Assets/Resources/CharacterMaterials/" + lvl + "/" + ogRef.name + ".mat");
				}
				ogRef.spriteHolder.GetComponent<SpriteRenderer> ().material = Mat;
				ogRef.spriteHolder.GetComponent<SpriteRenderer> ().sprite = ogRef.IdleSprite [0];
			}
		}

		withReplace = EditorGUILayout.ToggleLeft ("With replacement", withReplace);
		if (withReplace) {
			EditorGUILayout.LabelField ("Replace: ");
			replaceOld = EditorGUILayout.TextField (replaceOld);
			EditorGUILayout.LabelField ("With: ");
			replaceNew = EditorGUILayout.TextField (replaceNew);
		}

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        // some quick hackaton shitcode to import the fast dialogues
        if (GUILayout.Button("Import fast dialogue"))
        {
            ogRef.initialize();
            string path = EditorUtility.OpenFilePanel("Fast dialogue", "", "fdl");
            string contents = File.ReadAllText(path);
            // add event
            ogRef.addEvent();
            ogRef.addProgram();
            ogRef.registerEventName(options[1]); // on speak
            // process text
            string[] lines = contents.Split('\n');
            bool exitAsk = false;
            int instr = 0;
            currentStringCollector = new List<string>();
            foreach (string line in lines)
            {
                // gather parameters
                string speaker = "Player";
                bool ask = false;
                int askOption = 0;
                string text = "";
                if (line.StartsWith("J:"))
                {
                    speaker = "Player";
                    text = line.Substring(2);
                }
                if (line.StartsWith("1:"))
                {
                    speaker = "NPC1";
                    text = line.Substring(2);
                }
                if(line.StartsWith("2:"))
                {
                    speaker = "NPC2";
                    text = line.Substring(2);
                }
                if(line.StartsWith("J1:"))
                {
                    ask = true;
                    text = line.Substring(2);
                }
                if(line.StartsWith("J2:"))
                {
                    ask = true;
                    askOption = 1;
                    exitAsk = true;
                    text = line.Substring(2);
                }

                // make program
                if(ask == false)
                {
                    Debug.Log("Speaker: " + speaker);
                    ogRef.addAction(0);
                    ogRef.addInstructionToProgram(0, "None");
                    ogRef.setTypeOfAction(0, instr, 11);
                    ogRef.setInstructionOp(0, instr, actionsMethodName[11]);
                    ogRef.setInstructionParameter(0, instr, 1, text);
                    ogRef.setInstructionParameter(0, instr, 2, "wait");
                    ogRef.setInstructionParameter(0, instr, 3, speaker);
                    ++instr;
                }
                else
                {

                }
            }
            ogRef.alterEvent(0, options[1]);
        }


        EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();



		// Reset button cleans up the data structure

		if (GUILayout.Button("Reset")) {
			ogRef.initialize ();
		}


		// clear instruction color list
		for (int k = 0; k < ogRef.instructionColor.Count; ++k) {
			for (int j = 0; j < ogRef.instructionColor [k].theList.Count; ++j) {
				ogRef.instructionColor [k].theList [j] = -1;
			}
		}
		//ogRef.instructionColor [i].theList [targetInstr];

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
				sfxList [m+1] = ogRef.soundEffects [m].name;
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
		//ogRef.pickable = EditorGUILayout.ToggleLeft (" Pickable", ogRef.pickable);


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
			if (GUILayout.Button ("Copy event", GUILayout.Width(100))) { 
				EditorGUIUtility.systemCopyBuffer = ogRef.programToString (i); 
			}


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

				Color instrColor = Color.white;

				if (ogRef.instructionColor != null) {

					if (ogRef.instructionColor.Count > i) {
						
						if (ogRef.instructionColor [i].theList.Count > j) {
						
							int instrColorIndex;
							instrColorIndex = ogRef.instructionColor [i].theList [j];
							if (instrColorIndex != -1) {
								instrColor = indexedColor [instrColorIndex];
							}
						
						}

					}

				}
				GUI.color = instrColor;
				act = EditorGUILayout.Popup ("Action: ", ogRef.getTypeOfAction(i, j), actions, GUILayout.Width (300));
				GUI.color = Color.white;
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
							
							WisdominiObject[] ws = go.GetComponents<WisdominiObject> ();

							foreach(WisdominiObject w in ws) {
								System.Type t = w.GetType ();
								methods = t.GetMethods (); //go.GetComponent<WisdominiObject> ().GetType ().GetMethods ();
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
								string translatedType = translateType (par [pi].ParameterType.Name);
								string textFieldItemParam = ogRef.getInstructionParameter (i, j, 3+pi); // make room for 2nd parameter
								EditorGUILayout.LabelField (par [pi].Name + " (" + translatedType + ") : ");

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
					popupSelection = EditorGUILayout.Popup ("Music: ", 0, musList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						EditorGUILayout.TextField("", GUILayout.Width(200));
					}
					EditorGUI.indentLevel--;
					break;



				case 8: // change location
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1

//					DirectoryInfo scenesPath = new DirectoryInfo (Application.dataPath);
//					FileInfo[] files = scenesPath.GetFiles ("*.unity", SearchOption.AllDirectories);

					string[] fileList;
					int fileListLength = 1;//files.Length + 1;
					fileList = new string[fileListLength];
					fileList [0] = "By name...";
//					for (int k = 1; k < fileListLength; ++k) {
//						fileList [k] = removeExtension(files [k-1].Name);
//					}

					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					itemByString = indexOfStringInList(fileList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("New location: ", itemByString, fileList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						//prevAction = ogRef.getInstructionOp (i, j); 
//						if (!prevAction.Equals ("setLocation")) { // If we just changed action, set to ""
//							textFieldItem = "";
//						}
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					}
					else { // not by name
						textFieldItem = fileList[popupSelection];

					}

					ogRef.setInstructionParameter (i, j, 1, textFieldItem);

					EditorGUI.indentLevel--;
					break;



				case 9: // set walking state
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					//EditorGUILayout.EnumPopup("Walking state: ", 0, CharacterWalkingState, GUILayout.Width (270));
					//EditorGUILayout.EnumPopup("Walking direction: ", 0, CharacterDirection, GUILayout.Width (270));
					selected = indexOfStringInList (walkingState, textFieldItem);
					selected = EditorGUILayout.Popup ("Walking state: ", selected, walkingState, GUILayout.Width (270));
					ogRef.setInstructionParameter (i, j, 1, walkingState [selected]);

					textFieldItem = ogRef.getInstructionParameter (i, j, 2);
					selected = indexOfStringInList (walkingDirection, textFieldItem);
					selected = EditorGUILayout.Popup ("Direction: ", selected, walkingDirection, GUILayout.Width (270));
					if (walkingDirection [selected].Equals ("Custom...")) {
						EditorGUILayout.LabelField ("Bearing: ");
						textFieldItem = ogRef.getInstructionParameter (i, j, 3);
						if (textFieldItem.Equals ("")) {
							textFieldItem = "0.0";
						}
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (80));
						ogRef.setInstructionParameter (i, j, 3, textFieldItem); 

					}
					ogRef.setInstructionParameter (i, j, 2, walkingDirection [selected]);
					EditorGUILayout.LabelField ("Speed: ");
					textFieldItem = ogRef.getInstructionParameter (i, j, 4);
					if (textFieldItem.Equals ("")) {
						textFieldItem = ogRef.walkingSpeed.ToString ();
					}
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (80));
					ogRef.setInstructionParameter (i, j, 4, textFieldItem);
					EditorGUI.indentLevel--;
					break;

				case 10: // walk to...
					EditorGUI.indentLevel++;
					Vector2 walkCoords;
					if (coordsAvailable && (eyedropIndex == currentIndex)) {
						coordsAvailable = false;
						walkCoords = new Vector2 (raycastX, raycastZ);
					} else {
						textFieldItem = ogRef.getInstructionParameter (i, j, 1);
						float xCoord;
						float.TryParse (textFieldItem, out xCoord);
						textFieldItem = ogRef.getInstructionParameter (i, j, 2);
						float zCoord;
						float.TryParse (textFieldItem, out zCoord);
						walkCoords = new Vector2 (xCoord, zCoord);

					}
					EditorGUILayout.BeginHorizontal ();
					walkCoords = EditorGUILayout.Vector2Field ("X,Z coords: ", walkCoords);
					ogRef.setInstructionParameter (i, j, 1, "" + walkCoords.x);
					ogRef.setInstructionParameter (i, j, 2, "" + walkCoords.y);
					if (GUILayout.Button (Resources.Load<Texture> ("eyedropicon"), GUILayout.Width (20), GUILayout.Height (20))) {
						eyedropIndex = currentIndex;
						editingCoords = true;
					}
					EditorGUILayout.EndHorizontal ();
					//EditorGUILayout.LabelField ("Tip: You can click on the scene preview instead of typing in the coordinates");
					textFieldItem = ogRef.getInstructionParameter (i, j, 3);
					if (textFieldItem.Equals ("blocking"))
						isBlocking = true;
					else
						isBlocking = false;
					isBlocking = EditorGUILayout.ToggleLeft (" Blocking ", isBlocking);
					if (isBlocking)
						ogRef.setInstructionParameter (i, j, 3, "blocking");
					else
						ogRef.setInstructionParameter (i, j, 3, "non-blocking");
					EditorGUI.indentLevel--;
					++currentIndex;
					break;

					/*
					 * 0: say   1: text   2: wait/no wait flag  3: position (NPC1, NPC2, Player)  4: rosetta string id
					 * 
					 */
				case 11: // say
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					textFieldItem = EditorGUILayout.TextArea (textFieldItem, GUILayout.MinWidth (80), GUILayout.MaxWidth (270), GUILayout.ExpandWidth (false), GUILayout.ExpandHeight (true));
					currentStringCollector.Add (textFieldItem);
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);

					textFieldItem = ogRef.getInstructionParameter (i, j, 2);
					bool waitForTouch = false;
					if (textFieldItem.Equals ("wait"))
						waitForTouch = true;
					waitForTouch = EditorGUILayout.ToggleLeft (" Wait until read", waitForTouch);
					if (waitForTouch)
						textFieldItem = "wait";
					else
						textFieldItem = "don't wait";
					ogRef.setInstructionParameter (i, j, 2, textFieldItem);

					textFieldItem = ogRef.getInstructionParameter (i, j, 3);
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
						ogRef.setInstructionParameter (i, j, 3, "NPC1");

					}
					if (newnpc2 && !isnpc2) {
						ogRef.setInstructionParameter (i, j, 3, "NPC2");

					}
					if (newplayer && !isplayer) {
						ogRef.setInstructionParameter (i, j, 3, "Player");

					}

					textFieldItem = ogRef.getInstructionParameter (i, j, 4);
					if (!textFieldItem.StartsWith ("Conv_")) {
						textFieldItem = ogRef.name + i + "_" + strIndex; // simple node syntax
						ogRef.setInstructionParameter (i, j, 4, textFieldItem);
					}
					++strIndex;
                   

					EditorGUI.indentLevel--;
					break;



				/*
				 * 
				 * 0: ask   1: number of answers   2...: answers...  2+nanswers... targets 2+2*nanswers ... rosetta id
				 * 
				 */
				case 12: // ask
					EditorGUI.indentLevel++;

					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					if (textFieldItem.Equals (""))
						textFieldItem = "0";
					EditorGUILayout.LabelField ("Number of answers:");
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (80));
					int nAnswers;
					bool ok;
					ok = int.TryParse (textFieldItem, out nAnswers);
					if (!ok) {
						nAnswers = 0;
						textFieldItem = "0";
					}
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);
					int index;
					EditorGUI.indentLevel++;
					for (index = 0; index < nAnswers; ++index) {
						EditorGUILayout.LabelField ("Answer " + (index+1) +":");
						textFieldItem = ogRef.getInstructionParameter (i, j, 2+index);
						textFieldItem = EditorGUILayout.TextField (textFieldItem);
						currentStringCollector.Add (textFieldItem);
						ogRef.setInstructionParameter (i, j, 2+index, textFieldItem);


						textFieldItem = ogRef.getInstructionParameter (i, j, 2+index+2*nAnswers);
						textFieldItem = ogRef.name + i + "_" + strIndex;
						ogRef.setInstructionParameter (i, j, 2+index+2*nAnswers, textFieldItem);
						++strIndex;



						textFieldItem = ogRef.getInstructionParameter (i, j, 2+index+nAnswers); // Make sure there is room for parameter #1

						int targetInstr = ogRef.getIndexOfLabel (i, textFieldItem);
						if (targetInstr > -1) {
							int prevColor = ogRef.instructionColor [i].theList [targetInstr];
							if (prevColor != -1) {
								GUI.backgroundColor = indexedColor [prevColor];
							} 
							else {
								GUI.backgroundColor = indexedColor [currentColor];
								/* register color for branch target */
								if (ogRef.instructionColor != null) {
									if (ogRef.instructionColor.Count > i) {
										targetInstr = ogRef.getIndexOfLabel (i, textFieldItem);
										if ((targetInstr > -1) && (ogRef.instructionColor [i].theList.Count > targetInstr)) {
											ogRef.instructionColor [i].theList [targetInstr] = currentColor;
										}
									}
								}
								currentColor = (currentColor + 1) % nColors;
							}
						}
						

						string[] labels = ogRef.getAllLabels(i);
						itemByString = indexOfStringInList(labels, textFieldItem);
						popupSelection = EditorGUILayout.Popup ("Target: ", itemByString, labels, GUILayout.Width (270));
						if (popupSelection == 0) { // By name...
							//prevAction = ogRef.getInstructionOp (i, j); 
							string prevSelect = ogRef.getInstructionParameter (i, j, 2+index+nAnswers);
							if (indexOfStringInList (labels, prevSelect) != 0) { // reset textFieldItem
								textFieldItem = "";
							}
							textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
						} else { // not by name
							textFieldItem = labels[popupSelection];

						}




						ogRef.setInstructionParameter (i, j, 2+index+nAnswers, textFieldItem);
						GUI.backgroundColor = Color.white;


					}
					EditorGUI.indentLevel--;

					EditorGUI.indentLevel--;
					break;

				case 13: // set Miniature
					EditorGUI.indentLevel++;

					// Find out how many miniatures we do have
					int nMiniatures;
					nMiniatures = 0;
					if (ogRef.neutralMiniature != null)
						++nMiniatures;
					if (ogRef.blinkMiniature != null)
						++nMiniatures;
					if (ogRef.sadMiniature != null)
						++nMiniatures;
					if (ogRef.worriedMiniature != null)
						++nMiniatures;


					// we now know how many miniatures we have
					string[] miniatureList = new string[nMiniatures];
					index = 0;
					if (ogRef.neutralMiniature != null) {
						miniatureList [index++] = "Neutral";

					}
					if (ogRef.blinkMiniature != null) {
						miniatureList [index++] = "Happy";

					}
					if (ogRef.sadMiniature != null) {
						miniatureList [index++] = "Sad";

					}
					if (ogRef.worriedMiniature != null) {
						miniatureList [index++] = "Worried";

					}

					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (miniatureList, textFieldItem);
					selected = EditorGUILayout.Popup ("Miniature: ", selected, miniatureList, GUILayout.Width (270));
					ogRef.setInstructionParameter (i, j, 1, miniatureList[selected]);

					textFieldItem = ogRef.getInstructionParameter (i, j, 2);
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
						ogRef.setInstructionParameter (i, j, 2, "NPC1");

					}
					if (newnpc2 && !isnpc2) {
						ogRef.setInstructionParameter (i, j, 2, "NPC2");

					}
					if (newplayer && !isplayer) {
						ogRef.setInstructionParameter (i, j, 2, "Player");

					}

					EditorGUI.indentLevel--;
					break;

				case 14: // set dialogue colour
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (colours, textFieldItem);
					EditorGUILayout.LabelField ("Color:");
					selected = EditorGUILayout.Popup (selected, colours, GUILayout.Width (270));
					if (colours [selected].Equals ("Custom...")) {
						textFieldItem = ogRef.getInstructionParameter (i, j, 2);
						Color theColour;
						theColour = parseColor (textFieldItem);
						theColour = EditorGUILayout.ColorField (theColour, GUILayout.Width (120));
						string strRep;
						strRep = colorToString (theColour);
						ogRef.setInstructionParameter (i, j, 2, strRep);
					}
					ogRef.setInstructionParameter (i, j, 1, colours[selected]);
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




				case 20: // branch to target...
					
					EditorGUI.indentLevel++;
					string[] labelList;

					textFieldItem = ogRef.getInstructionParameter (i, j, 1); // Make sure there is room for parameter #1

					// choose color
					int targetInst;
					targetInst = ogRef.getIndexOfLabel (i, textFieldItem);
					if (targetInst > -1) {
						int prevColor = ogRef.instructionColor [i].theList [targetInst];
						if (prevColor != -1) {
							GUI.backgroundColor = indexedColor [prevColor];
						} 
						else {
							GUI.backgroundColor = indexedColor [currentColor];
							/* register color for branch target */
							if (ogRef.instructionColor != null) {
								if (ogRef.instructionColor.Count > i) {
									targetInst = ogRef.getIndexOfLabel (i, textFieldItem);
									if ((targetInst > -1) && (ogRef.instructionColor [i].theList.Count > targetInst)) {
										ogRef.instructionColor [i].theList [targetInst] = currentColor;
									}
								}
							}
							currentColor = (currentColor + 1) % nColors;
						}
					}


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
						op1ConstantStr = ogRef.getInstructionParameter(i, j, 6);
						EditorGUILayout.PrefixLabel("Key: ");
						op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
						ogRef.setInstructionParameter (i, j, 6, op1ConstantStr);

						op1ConstantStr = ogRef.getInstructionParameter (i, j, 7);
						selected = indexOfStringInList (dataTypes, op1ConstantStr);
						selected = EditorGUILayout.Popup (selected, dataTypes, GUILayout.Width(120));
						ogRef.setInstructionParameter (i, j, 7, dataTypes [selected]);

						startOfValue2Parameters = 8;

					}
					else if(popUpItem == 1) { // Query Selected

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
								methods = go.GetComponent<WisdominiObject> ().GetType ().GetMethods ();
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

						ParameterInfo [] par;

						if (popupSelection >= 0) { // make sure a method from the drop down is selected......

							if (methods != null) {
								par = methods [messageIndex [popupSelection]].GetParameters ();
								if (par.Length == 0) {
									EditorGUILayout.LabelField ("(no parameters)");
								}
								for (int pi = 0; pi < par.Length; ++pi) {
									string textFieldItemParam = ogRef.getInstructionParameter (i, j, 8+pi); // make room for 2nd parameter
									EditorGUILayout.LabelField (par [pi].Name + " (" + translateType (par [pi].ParameterType.Name) + ") : ");
									textFieldItemParam = EditorGUILayout.TextField (textFieldItemParam, GUILayout.Width (200));
									ogRef.setInstructionParameter (i, j, 8+pi, textFieldItemParam);
								}
								ogRef.getInstructionParameter (i, j, 8+par.Length);
								ogRef.setInstructionParameter(i, j, 8+par.Length, "\\"); // end of parameters
								startOfValue2Parameters = 8+par.Length+1;

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
					ogRef.setInstructionParameter (i, j, 1, opList[popUpItem2]);
					EditorGUI.indentLevel--;
					EditorGUI.indentLevel--;
					if (popUpItem2 < 6) {  // binary operator
						EditorGUILayout.LabelField ("Value 2:");
						textFieldItem = ogRef.getInstructionParameter (i, j, 5);
						popUpItem3 = indexOfStringInList (conditionList, textFieldItem);
						popUpItem3 = EditorGUILayout.Popup (popUpItem3, conditionList, GUILayout.Width (270));
						ogRef.setInstructionParameter (i, j, 5, conditionList[popUpItem3]);
						EditorGUI.indentLevel++;
						if (popUpItem3 == 0) { // Constant selected
							op1ConstantStr = ogRef.getInstructionParameter (i, j, startOfValue2Parameters);
							EditorGUILayout.LabelField ("Constant 2:");
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters, op1ConstantStr);
						} 
						else if (popUpItem3 == 2) { // retrieve data selected
							// index 6: key
							// index 7: type
							op1ConstantStr = ogRef.getInstructionParameter(i, j, startOfValue2Parameters);
							EditorGUILayout.PrefixLabel("Key: ");
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters, op1ConstantStr);

							op1ConstantStr = ogRef.getInstructionParameter (i, j, startOfValue2Parameters+1);
							selected = indexOfStringInList (dataTypes, op1ConstantStr);
							selected = EditorGUILayout.Popup (selected, dataTypes, GUILayout.Width(120));
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters+1, dataTypes [selected]);


						}
						else { // Query Selected
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

							textFieldItem2 = ogRef.getInstructionParameter (i, j, startOfValue2Parameters+1); // make room for 2nd parameter
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
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters+1, textFieldItem2);



							// Onto the parameters...


							EditorGUI.indentLevel++;

							ParameterInfo [] par;

							if (popupSelection >= 0) { // make sure a method from the drop down is selected......

								if (methods != null) {
									par = methods [messageIndex [popupSelection]].GetParameters ();
									if (par.Length == 0) {
										EditorGUILayout.LabelField ("(no parameters)");
									}
									for (int pi = 0; pi < par.Length; ++pi) {
										string textFieldItemParam = ogRef.getInstructionParameter (i, j, startOfValue2Parameters+2+pi); // make room for 2nd parameter
										EditorGUILayout.LabelField (par [pi].Name + " (" + translateType (par [pi].ParameterType.Name) + ") : ");
										textFieldItemParam = EditorGUILayout.TextField (textFieldItemParam, GUILayout.Width (200));
										ogRef.setInstructionParameter (i, j, startOfValue2Parameters+2+pi, textFieldItemParam);
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
					} 
					else { // unary operator

						if (opList[popUpItem2].Equals ("In range")) {

							EditorGUILayout.LabelField ("In range from:");
							op1ConstantStr = ogRef.getInstructionParameter (i, j, startOfValue2Parameters);
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters, op1ConstantStr);

							EditorGUILayout.LabelField ("to:");
							op1ConstantStr = ogRef.getInstructionParameter (i, j, startOfValue2Parameters+1);
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters+1, op1ConstantStr);

						}
						if (opList[popUpItem2].Equals ("Not in range")) {

							EditorGUILayout.LabelField ("Not in range from:");
							op1ConstantStr = ogRef.getInstructionParameter (i, j, startOfValue2Parameters);
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters, op1ConstantStr);

							EditorGUILayout.LabelField ("to:");
							op1ConstantStr = ogRef.getInstructionParameter (i, j, startOfValue2Parameters+1);
							op1ConstantStr = EditorGUILayout.TextField (op1ConstantStr, GUILayout.Width (200));
							ogRef.setInstructionParameter (i, j, startOfValue2Parameters+1, op1ConstantStr);
						}

					}

					/* Destination labels */

					EditorGUILayout.LabelField ("If condition met, branch to:");


					EditorGUI.indentLevel++;

					textFieldItem = ogRef.getInstructionParameter (i, j, 2); // Make sure there is room for parameter #1
					labelList = ogRef.getAllLabels(i);
					itemByString = indexOfStringInList(labelList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("Target: ", itemByString, labelList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						//prevAction = ogRef.getInstructionOp (i, j); 
						string prevSelect = ogRef.getInstructionParameter (i, j, 2);
						if (indexOfStringInList (labelList, prevSelect) != 0) { // reset textFieldItem
							textFieldItem = "";
						}
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					} else { // not by name
						textFieldItem = labelList[popupSelection];

					}
					ogRef.setInstructionParameter (i, j, 2, textFieldItem);
					EditorGUI.indentLevel--;



					EditorGUILayout.LabelField ("If not, branch to:");

					EditorGUI.indentLevel++;

					textFieldItem = ogRef.getInstructionParameter (i, j, 3); // Make sure there is room for parameter #1
					labelList = ogRef.getAllLabels(i);
					itemByString = indexOfStringInList(labelList, textFieldItem);
					popupSelection = EditorGUILayout.Popup ("Target: ", itemByString, labelList, GUILayout.Width (270));
					if (popupSelection == 0) { // By name...
						//prevAction = ogRef.getInstructionOp (i, j); 
						string prevSelect = ogRef.getInstructionParameter (i, j, 3);
						if (indexOfStringInList (labelList, prevSelect) != 0) { // reset textFieldItem
							textFieldItem = "";
						}
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					} else { // not by name
						textFieldItem = labelList[popupSelection];

					}
					ogRef.setInstructionParameter (i, j, 3, textFieldItem);

					EditorGUI.indentLevel--;



					EditorGUI.indentLevel--;
					break;

				case 22: // pause event
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (options, textFieldItem);
					selected = EditorGUILayout.Popup (selected, options, GUILayout.Width (160));
					textFieldItem = options [selected];
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;

				case 23: // resume event
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (options, textFieldItem);
					selected = EditorGUILayout.Popup (selected, options, GUILayout.Width (160));
					textFieldItem = options [selected];
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;

				case 24: // Store data...
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


				case 25: // update data

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
					


					//}



					EditorGUI.indentLevel--;
					break;

				case 26: // replace program
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

				case 27: // record conversation
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					EditorGUILayout.PrefixLabel ("Title: ");
					textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
					currentStringCollector.Add (textFieldItem);
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);

					textFieldItem = ogRef.getInstructionParameter (i, j, 2);
					textFieldItem = ogRef.name + i + "_" + strIndex;
					ogRef.setInstructionParameter (i, j, 2, textFieldItem);
					++strIndex;

					EditorGUI.indentLevel--;
					break;

					/* 0: alert    1: msg    2: autotimeout (0 for never)  3: blocking   4: rosetta id */
				case 28: // alert
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
					waitForTouch = false;
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

				case 29: // chispAlert
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

				case 30: // follow player
					break;

				case 31: // stop following player
					break;

				case 32: // say image
					EditorGUI.indentLevel++;
					EditorGUILayout.PrefixLabel ("Second operator: ");
					string[] imgList = new string[ogRef.sayImages.Length];

					for (int idx = 0; idx < ogRef.sayImages.Length; ++idx) {
						if (ogRef.sayImages [idx] != null) {
							imgList [idx] = AssetDatabase.GetAssetPath(ogRef.sayImages [idx]);
						} else
							imgList [idx] = "";
					}

					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (imgList, textFieldItem);
					selected = EditorGUILayout.Popup (selected, imgList, GUILayout.Width (200));
					textFieldItem = imgList [selected];
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
						ogRef.setInstructionParameter (i, j, 3, "NPC1");

					}
					if (newnpc2 && !isnpc2) {
						ogRef.setInstructionParameter (i, j, 3, "NPC2");

					}
					if (newplayer && !isplayer) {
						ogRef.setInstructionParameter (i, j, 3, "Player");

					}


					EditorGUI.indentLevel--;
					break;

				case 33: // say from stringbank
					EditorGUI.indentLevel++;

					string[] banks = new string[ogRef.stringBank.Length];

					for (int k = 0; k < ogRef.stringBank.Length; ++k) {
						banks [k] = ogRef.stringBank [k].name;
					}

					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (banks, textFieldItem);
					selected = EditorGUILayout.Popup (selected, banks, GUILayout.Width (200));
					textFieldItem = banks [selected];

					EditorGUI.indentLevel--;
					break;

				case 34: // send mail     1: the contents id
					EditorGUI.indentLevel++;

						textFieldItem = ogRef.getInstructionParameter (i, j, 1);
						EditorGUILayout.PrefixLabel ("Content id: ");
						textFieldItem = EditorGUILayout.TextField (textFieldItem, GUILayout.Width (200));
						ogRef.setInstructionParameter (i, j, 1, textFieldItem);

					EditorGUI.indentLevel--;
					break;

				case 35:
					break;

				case 36:
					break;

				case 37:
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (options, textFieldItem);
					selected = EditorGUILayout.Popup (selected, options, GUILayout.Width (160));
					textFieldItem = options [selected];
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;

				case 38:
					string[] animNumber = { "1", "2", "3" };
					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField ("Animation number:");
					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (animNumber, textFieldItem);
					selected = EditorGUILayout.Popup (selected, animNumber, GUILayout.Width (160));
					textFieldItem = animNumber [selected];
					ogRef.setInstructionParameter (i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;

				case 39:
					break;

				case 40:
					break;

				case 41:
					break;

				case 42: // disable answer
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 2);
					//EditorGUILayout.BeginHorizontal ();

					bool all;
					bool.TryParse (textFieldItem, out all);
					all = EditorGUILayout.ToggleLeft ("All", all);
					if (all)
						textFieldItem = "true";
					else
						textFieldItem = "false";
					ogRef.setInstructionParameter (i, j, 2, textFieldItem);


					textFieldItem = ogRef.getInstructionParameter (i, j, 3);


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
					ogRef.setInstructionParameter (i, j, 3, textFieldItem);
					//EditorGUILayout.EndHorizontal ();

					textFieldItem = ogRef.getInstructionParameter(i, j, 1);
					textFieldItem = EditorGUILayout.TextField(textFieldItem, GUILayout.Width(120));
					ogRef.setInstructionParameter(i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;

				case 43: // enable answer
					EditorGUI.indentLevel++;
					textFieldItem = ogRef.getInstructionParameter (i, j, 2);
					//EditorGUILayout.BeginHorizontal ();

					bool.TryParse (textFieldItem, out all);
					all = EditorGUILayout.ToggleLeft ("All", all);
					if (all)
						textFieldItem = "true";
					else
						textFieldItem = "false";
					ogRef.setInstructionParameter (i, j, 2, textFieldItem);


					textFieldItem = ogRef.getInstructionParameter (i, j, 3);


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
					ogRef.setInstructionParameter (i, j, 3, textFieldItem);
					//EditorGUILayout.EndHorizontal ();

					textFieldItem = ogRef.getInstructionParameter(i, j, 1);
					textFieldItem = EditorGUILayout.TextField(textFieldItem, GUILayout.Width(120));
					ogRef.setInstructionParameter(i, j, 1, textFieldItem);
					EditorGUI.indentLevel--;
					break;

				case 44:
					EditorGUI.indentLevel++;

					EditorGUI.indentLevel--;
					break;

				case 45:
					EditorGUI.indentLevel++;

					textFieldItem = ogRef.getInstructionParameter (i, j, 1);
					selected = indexOfStringInList (autopilotModes, textFieldItem);
					if ((selected != 0) && (selected != 1))
						selected = 0;
					selected = EditorGUILayout.Popup (selected, autopilotModes);
					textFieldItem = autopilotModes [selected];
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

		if (GUILayout.Button("Generate StringBanks")) {
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

}

#endif