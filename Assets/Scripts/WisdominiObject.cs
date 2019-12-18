using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Reflection;

/* A WisdominiObject can send and wait for actions to end */

enum Comparator { Equals, GreaterThan, LesserThan, GreaterOrEqual, LesserOrEqual, NotEqual, InRange, NotInRange,
	IsTrue, IsNotTrue };

[System.Serializable]
public class WisdominiObject : MonoBehaviour {

	[HideInInspector]
	public List<Stack> programStacks; // a stack for each program programStacks[i].Push( programs[i] );
	[HideInInspector]
	public List<Stack> programStacksPointers; //
	[HideInInspector]
	public List<ListString2D> originalPrograms; // List of original programs

	[HideInInspector]
	public int chivoExpiatorio;

	[HideInInspector]
	public string TestString;
	[HideInInspector]
	public string[] TestStringArray;
	//[SerializeProperty]
	[HideInInspector]
	public int testInteger;
	/*{
		get { return testInteger; }
		set {
			testInteger = value;
		}
	}*/



	WWW wwwres;

	public float opacitySpeed = 1.0f;
	float targetOpacity = 1.0f;
	float opacity = 1.0f;
	Material material = null;


	[HideInInspector]
	public bool isDoingAction;

	[HideInInspector]
	public int programNotification = -1;

	[HideInInspector]
	public bool isWaitingForActionToComplete;

	[HideInInspector]
	public List<bool> programIsWaitingForActionToComplete;

	[HideInInspector]
	public WisdominiObject waitingRef;

	[HideInInspector]
	public ListString2D currentProgram;

	[HideInInspector]
	public List<ListString2D> programs;

	[HideInInspector]
	public List<string> eventNamesInOrder; // which event must start which program number

	[HideInInspector]
	public List<bool> isProgramRunning;

	[HideInInspector]
	public List<int> programPointer_;

	[HideInInspector]
	public int programPointer;

	[HideInInspector]
	public float startTime;

	[HideInInspector]
	public List<float> delayTime;

	bool firstUpdate;
	[HideInInspector]
	public List<bool> waitingForAnswer;

	public string ConversationFolderName = "";

	[HideInInspector]
	public List<ListInt1D> instructionColor;
	ListInt1D currentColorList;

	MasterControllerScript mcRef;
	DataStorage ds;
	[HideInInspector]
	public Rosetta rosetta;

	[HideInInspector]
	public bool preventAwake = false;

	[HideInInspector]
	public int answerIndex; // this will hold the index of the answer chosen by the player during the last ask command

	const float MAXDELAY = 1000000.0f; // A lot of time

	bool isRecordingConversation;
	int convIndex = 0;
	int strIndex = 0;

	/*
	public void processMessage (string msg) {

	}
	*/

	public void setInstructionOp(int programIndex, int instructionIndex, string op) {

		ListString2D program = programs [programIndex];
		//ListString2D program_theList = program
		ListString1D instruction = program.theList[instructionIndex];
	

		instruction.theList [0] = op;
	}


	public string getInstructionOp(int programIndex, int instructionIndex) {

		return programs [programIndex].theList [instructionIndex].theList [0];
	}

	/*
	 * A wants to wait for B's action to finish. This is called in B, with A as argument
	 */
	public void registerWaitingObject(WisdominiObject refrn) {

		programNotification = -1;
		waitingRef = refrn;

	}

	public void registerWaitingObject(WisdominiObject refrn, int progNumber) {

		programNotification = progNumber;
		waitingRef = refrn;

	}

	/*
	 * A wants to wait for B's action to finish. This is called by A, passing B as argument
	 */
	public void waitForActionToComplete(WisdominiObject other) {

		other.registerWaitingObject (this);
		isWaitingForActionToComplete = true;

	}

	public void waitForActionToComplete(WisdominiObject other, int progNumber) {

		other.registerWaitingObject (this, progNumber);
		programIsWaitingForActionToComplete[progNumber] = true;

	}

	/*
	 * A is waiting for B's action to finish. When action is finished, this is the method called in A
	 */
	public void finishAction() {

		isWaitingForActionToComplete = false;

	}

	/*
	 * A is waiting for B's action to finish. When action is finished, this is the method called in A
	 */
	public void finishAction(int prgNumber) {

		programIsWaitingForActionToComplete[prgNumber] = false;

	}

	/*
	 * A is waiting for B's action to finish. When action is finished, this is the method called by B
	 */
	public void notifyFinishAction() {

		if (waitingRef != null) {

			if (programNotification != -1) {
				waitingRef.finishAction (programNotification);
			}
			else 
			 waitingRef.finishAction ();
			waitingRef = null; // release reference

		}

	}


	public void reset() {

		isWaitingForActionToComplete = false;
		programs = new List<ListString2D> ();
		currentProgram = null;
		programPointer_ = new List<int> ();
		waitingForAnswer = new List<bool> ();
		eventNamesInOrder = new List<string> ();
		delayTime = new List<float> ();
		isProgramRunning = new List<bool> ();
		instructionColor = new List<ListInt1D> (); // a color for each instruction of each program
		programIsWaitingForActionToComplete = new List<bool> ();
		programPointer = 0;
		//delayTime = 0.0f;
		startTime = 0.0f;


	}


	public void _wm_fadein() {
		Renderer rend = this.GetComponentInChildren<Renderer> ();
		material = rend.material;
		opacity = 0.0f;
		targetOpacity = 1.0f;
	}

	public void _wm_fadeout() {
		Renderer rend = this.GetComponentInChildren<Renderer> ();
		material = rend.material;
		opacity = 1.0f;
		targetOpacity = 0.0f;
	}


	public void Start () {

		firstUpdate = true;



		/* fun, runtime stuff */
		programStacks = new List<Stack> ();
		programStacksPointers = new List<Stack> ();
		originalPrograms = new List<ListString2D> ();
		for(int i = 0; i<programs.Count; ++i) {
			Stack newStack = new Stack ();
			programStacks.Add (newStack);
			Stack newProgramIndexStack = new Stack ();
			programStacksPointers.Add (newProgramIndexStack);
			originalPrograms.Add (programs[i]);
		}

		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();
		ds = mcRef.getStorage ();


	}


	public void addProgram() {

		ListString2D newProgram = new ListString2D ();



		programs.Add (newProgram); 
		programPointer_.Add (0);
		waitingForAnswer.Add (false);
		isProgramRunning.Add (false);
		ListInt1D newColorList = new ListInt1D ();

		instructionColor.Add (newColorList);
		programIsWaitingForActionToComplete.Add (false);
		delayTime.Add (0.0f);

		currentProgram = newProgram; 
		currentColorList = newColorList;

	}


	public string getInstructionParameter (int programIndex, int instructionIndex, int paramIndex) {

		if (programIndex < programs.Count) {

			if (instructionIndex < programs [programIndex].theList.Count) {

				if (paramIndex < programs [programIndex].theList [instructionIndex].theList.Count) {

				} 
				else {

					// add as many dummy parameters as needed
					int previousLength = programs [programIndex].theList [instructionIndex].theList.Count;
					int maxi = 1 + paramIndex - previousLength;
					for (int i = 0; i < maxi; ++i) {
						programs [programIndex].theList [instructionIndex].theList.Add ("");
					}


				}
				return programs [programIndex].theList [instructionIndex].theList [paramIndex];

			}

		} 

		return ("error");

	}

	public void setInstructionParameter (int programIndex, int instructionIndex, int paramIndex, string prm) {

		if (programIndex < programs.Count) {


            if (instructionIndex < programs [programIndex].theList.Count) {

                while (programs[programIndex].theList[instructionIndex].theList.Count < (paramIndex + 1))
                {
                    programs[programIndex].theList[instructionIndex].theList.Add("");
                }
                programs [programIndex].theList [instructionIndex].theList[paramIndex] = prm;

			}

        } 

	}

	public void registerEventName(string ev) {

		eventNamesInOrder.Add (ev);

	}

	public void alterEvent(int index, string newev) {

		eventNamesInOrder [index] = newev;

	}



	/*
	 *	Returns a program index from event name
	 */

	public int programIndexFromEventName(string ev) {

		int i;
		for (i = 0; i < eventNamesInOrder.Count; ++i) {
			if (eventNamesInOrder [i].Equals (ev))
				return i;
		}
		return -1; // if not found, first program please NO VALE, TIENE QUE SER -1

	}

	public void addInstructionToProgram(params string[] p) {

		ListString1D newList = new ListString1D ();
		newList.theList.AddRange (p);
		currentProgram.theList.Add (newList);
		currentColorList.theList.Add (-1); // no color

	}

	public void addInstructionToProgram(int programIndex, params string[] p) {

		currentProgram = programs [programIndex];
		currentColorList = instructionColor [programIndex];
		addInstructionToProgram (p);

	}

	public void deleteInstructionFromProgram(int programIndex, int instrIndex) {

		ListString2D prg;
		ListInt1D col;

		int i;

		prg = programs [programIndex];
		col = instructionColor [programIndex];

		if (instrIndex < prg.theList.Count) {
			
			/*if (instrIndex < (prg.theList.Count - 1)) {
				for (i = instrIndex; i < prg.theList.Count - 1; ++i) {

					prg.theList [i] = prg.theList [i + 1];

				}


			}*/

			//prg.theList [prg.theList.Count - 1] = new ListString1D (); 
			//prg.theList.RemoveAt (prg.theList.Count - 1);
			prg.theList.RemoveAt (instrIndex);
			col.theList.RemoveAt (instrIndex);

		}

	}

	public void insertInstructionIntoProgram(int programIndex, int insertIndex, params string[] p) {

		ListString2D prg;
		ListInt1D col;

		int i;

		prg = programs [programIndex];
		col = instructionColor [programIndex];

		/*prg.theList.Add (new ListString1D ());

		for (i = prg.theList.Count-1; i > insertIndex; --i) {

			prg.theList [i] = prg.theList [i-1];

		}

		prg.theList [insertIndex] = new ListString1D ();
		prg.theList [insertIndex].theList.AddRange (p);
		*/
		ListString1D newInstr = new ListString1D ();

		newInstr.theList.AddRange (p);
		prg.theList.Insert (insertIndex, newInstr);
		col.theList.Insert (insertIndex, -1);

	}

	public string instructionToString(int programIndex, int instrIndex) {

		string res = "";

		for (int i = 0; i < programs [programIndex].theList[instrIndex].theList.Count; ++i) {

			if (i > 0)
				res = res + "|";
			res = res + programs [programIndex].theList [instrIndex].theList [i];

		}

		return res;
	}

	public string programToString(int programIndex) {

		string res = "";

		for (int i = 0; i < programs [programIndex].theList.Count; ++i) {

			string nextInstruction = instructionToString (programIndex, i);
			if (!nextInstruction.Equals ("None")) {
				Debug.Log (nextInstruction);
				res = res + nextInstruction + "\n";
			}

		}

		return res;

	}
		

	public void startProgram(int prg) {

		if (prg >= programs.Count)
			return;
		isProgramRunning [prg] = true;
		programPointer_ [prg] = 0;
		programIsWaitingForActionToComplete [prg] = false;
		delayTime [prg] = 0.0f;
		isRecordingConversation = false;

	}

	public void stopProgram(int prg) {
		if (prg >= programs.Count)
			return;
		isProgramRunning [prg] = false;
	}

	public void UpdateProgram() {

		bool hotLinkProgram = false;



		for(int i=0; i<programs.Count; ++i) {

			if (i == 1) {
				if (isProgramRunning [i]) {
					i = 1;
				}
			}

			if (!isProgramRunning [i])
				continue;

			if (this.name.Equals ("NPCPerdidos")) {
				hotLinkProgram = true;
				hotLinkProgram = false;
			}

			currentProgram = programs [i];
			programPointer = programPointer_ [i];

			if (delayTime[i] > 0.0f) { // we are delaying
				if ((Time.time - startTime) > delayTime[i]) {
					delayTime[i] = 0.0f;
				} else
					continue;
			}

			if (programIsWaitingForActionToComplete[i])
				continue;

			string target;

			string[] op;

			if (programPointer >= currentProgram.theList.Count) { // program is finished

				if (programStacks [i].Count == 0) { // no programs stacked

					programs [i] = originalPrograms [i]; // we get back original program;
					isRecordingConversation = false; // stop recording conversations...
					isProgramRunning [i] = false;
					if (programIndexFromEventName ("onSpeak") == i) { // when we arrive at the end of an onSpeak event, de-interrupt
						CharacterGenerator chara = this.gameObject.GetComponent<CharacterGenerator> ();
						if (chara != null) {
							chara.interrupted = false;
						}
					}
					continue;

				} else { // we pop the program, and continue...
						programs [i] = (ListString2D) programStacks [i].Pop ();
						programPointer_ [i] = (int) programStacksPointers [i].Pop ();
						currentProgram = programs [i];
						programPointer = programPointer_ [i];
				}
			}

			if (programPointer >= currentProgram.theList.Count)
				continue;

			op = currentProgram.theList[programPointer].theList.ToArray ();


			if (op [0].Equals ("saveGame")) {
				mcRef.saveGame (false);
			}
			if (op [0].Equals ("deleteSaveGame")) {
				mcRef.deleteSaveGame ();
			}
			if (op [0].Equals ("setAutopilotMode")) {
				CharacterGenerator chara = this.GetComponent<CharacterGenerator> ();
				if (chara != null) {
					chara.refreshSpawnPosition ();
					chara.setAutopilotAbsolute (op [1].Equals ("Absolute"));
				}
			}

			if (op [0].Equals ("hide")) {
				_wm_hide ();
			}


			/*
		 	* sendMessage syntax:
		 	* sendMessage <target> <message> <params>*
		 	* 
		 	* This is, essentially, calling a execute-at-once method
		 	* 
		 	*/

			if (op [0].Equals ("setWalkingState")) { // should be in CharacterGenerator.cs

				CharacterGenerator charac;
				charac = this.GetComponent<CharacterGenerator> ();
				if (charac != null) {
					if(op[1].Equals("Standing")) 
						charac.setWalkingState (CharacterWalkingState.standing);
					if (op [1].Equals ("Walking"))
						charac.setWalkingState (CharacterWalkingState.walking);

					if (op [2].Equals ("Front"))
						charac.setWalkingDirection (CharacterDirection.front);
					if (op [2].Equals ("Left front"))
						charac.setWalkingDirection (CharacterDirection.frontLeft);
					if (op [2].Equals ("Left"))
						charac.setWalkingDirection (CharacterDirection.left);
					if (op [2].Equals ("Left back"))
						charac.setWalkingDirection (CharacterDirection.backLeft);
					if (op [2].Equals ("Back"))
						charac.setWalkingDirection (CharacterDirection.back);
					if (op [2].Equals ("Right back"))
						charac.setWalkingDirection (CharacterDirection.backRight);
					if (op [2].Equals ("Right"))
						charac.setWalkingDirection (CharacterDirection.right);
					if (op [2].Equals ("Right front"))
						charac.setWalkingDirection (CharacterDirection.frontRight);
					
					
				}

			}

			if (op [0].Equals ("sendMessage")) { // sendMessage is non-blocking

				target = op [1];
				string methodName;
				methodName = op [2];
				GameObject targetRef;
				WisdominiObject targetScriptRef;
				if (target.Equals ("this")) {
					targetScriptRef = this;				
				} else {					
					targetRef = GameObject.Find (target);
					targetScriptRef = targetRef.GetComponent<WisdominiObject> ();
					WisdominiObject[] ws = targetRef.GetComponents<WisdominiObject> ();
					foreach (WisdominiObject w in ws) {
						if (w.GetType ().GetMethod ("_wm_" + methodName) != null)
							targetScriptRef = w;
					}
					//targetScriptRef = targetRef.GetComponent<WisdominiObject> ();

				}
				//targetScriptRef.sendMessage (op); // stop using the implemented invoker, use C# mechanisms



				MethodInfo m = targetScriptRef.GetType ().GetMethod ("_wm_" + methodName);

				List<object> nativeParamList = new List<object> ();

				ParameterInfo[] paramInfoList = m.GetParameters ();

				for (int k = 0; k < paramInfoList.Length; ++k) {
					System.Type t = paramInfoList [k].ParameterType;
					if (t.Name.Equals ("Int32")) {
						int iParam;
						int.TryParse (op [3 + k], out iParam); // parse into integer
						nativeParamList.Add (iParam);
					} else if (t.Name.Equals ("Single")) {
						float fParam;
						float.TryParse (op [3 + k], out fParam);
						nativeParamList.Add (fParam);
					} else if (t.Name.Equals ("String")) {
						nativeParamList.Add (op [3 + k]); // no parsing needed
					} else if (t.Name.Equals ("Boolean")) {
						bool bParam;
						bool.TryParse (op [3 + k], out bParam);
						nativeParamList.Add (bParam);

					}
				}

				m.Invoke (targetScriptRef, nativeParamList.ToArray ());


			}

			/* run event
			 * 
			 * runEvent <nameOfEventToRun>
			 * 
			 */
			else if (op [0].Equals ("runEvent")) {

				int eventToRun = programIndexFromEventName (op [1]);
				if (eventToRun != -1) {
					startProgram (eventToRun);
				}
			}

			/* stop event
			 * 
			 * stopEvent <nameOfEventToRun>
			 * 
			 */
			else if (op [0].Equals ("stopEvent")) {

				int eventToRun = programIndexFromEventName (op [1]);
				if (eventToRun != -1) {
					isProgramRunning [eventToRun] = false;
				}
			} else if (op [0].Equals ("resumeEvent")) {

				int eventToRun = programIndexFromEventName (op [1]);
				if (eventToRun != -1) {
					isProgramRunning [eventToRun] = true;
				}
			}

			/*
		 	* startAction syntax:
		 	* startAction <target> <action> <blocking> <params>
		 	* 
		 	*/
			else if (op [0].Equals ("startAction")) { // startAction could be blocking
				/*
				target = op [1];
				GameObject targetRef;
				WisdominiObject targetScriptRef;
				if (target.Equals ("this")) {
					targetScriptRef = this;				
				} else {
					targetRef = GameObject.Find (target);
					targetScriptRef = targetRef.GetComponent<WisdominiObject> ();
				}
				targetScriptRef.startAction (op);
				if (op [2].Equals ("blocking")) { // if we want to block...
					waitForActionToComplete (targetScriptRef); // begin waiting
				}*/
				target = op [1];
				string methodName;
				GameObject targetRef;
				WisdominiObject targetScriptRef;
				if (target.Equals ("this")) {
					targetScriptRef = this;				
				} else {
					targetRef = GameObject.Find (target);
					targetScriptRef = targetRef.GetComponent<WisdominiObject> ();
				}
				//targetScriptRef.sendMessage (op); // stop using the implemented invoker, use C# mechanisms

				methodName = op [2];

				MethodInfo m = targetScriptRef.GetType ().GetMethod ("_wa_" + methodName);

				List<object> nativeParamList = new List<object> ();

				ParameterInfo[] paramInfoList = m.GetParameters ();

				nativeParamList.Add (this);
				for (int k = 1; k < paramInfoList.Length; ++k) {
					System.Type t = paramInfoList [k].ParameterType;
					if (t.Name.Equals ("Int32")) {
						int iParam;
						int.TryParse (op [3 + k], out iParam); // parse into integer
						nativeParamList.Add (iParam);
					} else if (t.Name.Equals ("Single")) {
						float fParam;
						float.TryParse (op [3 + k], out fParam);
						nativeParamList.Add (fParam);
					} else if (t.Name.Equals ("String")) {
						nativeParamList.Add (op [3 + k]); // no parsing needed
					} else if (t.Name.Equals ("Boolean")) {
						bool bParam;
						bool.TryParse (op [3 + k], out bParam);
						nativeParamList.Add (bParam);

					}
				}

				m.Invoke (targetScriptRef, nativeParamList.ToArray ());

				if (op [3].Equals ("blocking")) {
					targetScriptRef.registerWaitingObject (this, i);
					this.programIsWaitingForActionToComplete [i] = true;
				}


			} else if (op [0].Equals ("walkTo")) {

			

				CharacterGenerator charac;
				charac = this.GetComponent<CharacterGenerator> ();
				if (charac != null) {
					
					float targetx, targetz;
					float.TryParse (op [1], out targetx);
					float.TryParse (op [2], out targetz);

					charac.autopilotTo (targetx, targetz);

				}

				if (op [3].Equals ("blocking")) {
					registerWaitingObject (this, i); // self wait
					this.programIsWaitingForActionToComplete [i] = true;
				}

			} else if (op [0].Equals ("blockPlayerControls")) {
				
				GameObject.Find ("Player").GetComponent<PlayerScript> ().blockControls ();
			} else if (op [0].Equals ("unblockPlayerControls")) {
				GameObject.Find ("Player").GetComponent<PlayerScript> ().unblockControls ();
			} else if (op [0].Equals ("fullscreenImage")) {
				int imageN;
				FullScreenImageController controller = GameObject.Find ("FullScreenImageController").GetComponent<FullScreenImageController> ();
				controller.registerWaitingObject (this, i);
				this.programIsWaitingForActionToComplete [i] = true;
				if (int.TryParse (op [1], out imageN)) {
					controller.startItem (imageN);
				} else
					controller.startItem (op [1]);

			}

			/* 
		 	* delay syntax:
		 	* delay <number of seconds>
		 	* 
		 	* delay is necessarily blocking
		 	* 
		 	*/
			else if (op [0].Equals ("delay")) {

				startTime = Time.time;
				float t;
				float.TryParse (op [1], out t);
				if (t < 0.0f)
					delayTime [i] = 0.0f;
				else if (t > MAXDELAY)
					delayTime [i] = 0.0f;
				else {
					delayTime [i] = t;
				}

			} else if (op [0].Equals ("playSound")) {

				AudioSource audio = GameObject.Find ("LevelController").GetComponent<AudioSource > ();
                /*AudioClip prrr;
				prrr = (AudioClip)Resources.Load ("FinalAssets/Audio/SFX/Programs/" + op [1]);*/
                AudioClip Chosen = null;
                ObjectGeneratorScript og = this.gameObject.GetComponent<ObjectGeneratorScript>();
                if(og != null)
                {
                    foreach(AudioClip clip in og.soundEffects)
                    {
                        if (clip.name == op[1])
                        {
                            Chosen = clip;
                            break;
                        }
                    }
                }
                CharacterGenerator cg = this.gameObject.GetComponent<CharacterGenerator>();
                if (cg != null)
                {
                    foreach (AudioClip clip in cg.soundEffects)
                    {
                        if (clip.name == op[1])
                        {
                            Chosen = clip;
                            break;
                        }
                    }
                }
                if (Chosen != null) audio.PlayOneShot (Chosen);

			} else if (op [0].Equals ("playMusic")) {

				GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ().selectMixer (0);
				GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ().attachMusic (op [1]);
				GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ().setVolume (1.0f);
				GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ().playMusic ();
				//AudioSource audio = GameObject.Find ("MasterController").GetComponent<AudioSource > ();

				//AudioClip prrr;
				//prrr = (AudioClip)Resources.Load (op [1]);

				//audio.clip = prrr;
				//audio.Play ();

			} else if (op [0].Equals ("destroy")) {

				GameObject plyr = GameObject.Find ("Player");
				PlayerScript player = plyr.GetComponent<PlayerScript> ();

				if (player.interactingWith ((Interactor)this)) {
					player.OnTriggerExit (this.GetComponent<Collider> ());
				}
				Destroy (this.gameObject);

			} else if (op [0].Equals ("setLocation")) {
				//BoxCollider col;
				//col = this.GetComponent<BoxCollider> ();
				//GameObject plyr = GameObject.Find ("Player");
				//Vector3 spawnPos;
				// see if this is a "vertical" or "horizontal" collider

				//float colWidth = Mathf.Abs (col.bounds.max.x - col.bounds.min.x);
				//float colDepth = Mathf.Abs (col.bounds.max.z - col.bounds.min.z);
				/* COMENTADO POR CARRASCO
				if (colWidth > colDepth) { // "horizontal" trigger
					
					if ((col.bounds.center.z + col.transform.position.z) < plyr.transform.position.z) {  // player enter from bottom to up

						spawnPos = new Vector3 (plyr.transform.position.x, plyr.transform.position.y,
							plyr.transform.position.z + 3.0f);

					} else { // player enter from top to bottom

						spawnPos = new Vector3 (plyr.transform.position.x, plyr.transform.position.y,
							plyr.transform.position.z - 3.0f);
					}

				} else { // "vertical" trigger

					spawnPos = new Vector3 (plyr.transform.position.x - 3.0f, plyr.transform.position.y,
						plyr.transform.position.z);
				} */
//				string loc = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ().locationName;
//				float x;
//				float y;
//				float z;
//				x = mcRef.getStorage ().retrieveFloatValue ("Coords" + loc + "X");
//				y = mcRef.getStorage ().retrieveFloatValue ("Coords" + loc + "Y");
//				z = mcRef.getStorage ().retrieveFloatValue ("Coords" + loc + "Z");
//				spawnPos = new Vector3 (x, y, z);////////////////////////////
				//mcRef.registerPlayerSpawnLocation (spawnPos);
//				if ((x != 0.0f) && (y != 0.0f) && (z != 0.0f)) {
//					Vector3 newPos = new Vector3 (x, y, z);
//					this.transform.position = newPos;
//				}

				//mcRef.changeLocation (op [1]);
				SceneManager.LoadScene (op [1]);
			} else if (op [0].Equals ("loop")) {
				programPointer_ [i] = -1; // loop program from the beginning
			} else if (op [0].Equals ("makeLabel")) {
				// do nothing
			} else if (op [0].Equals ("branchTo")) {


				string targetLabel = op [1];
				// if targetLabel has replacement syntax...
				if (hasCorrectLinkSyntax (targetLabel)) {
					GameObject nextObject;
					WisdominiObject nextProgram;
					int dash = targetLabel.IndexOf ('/');
					string nextProgramObjectName = targetLabel.Substring (0, dash);
					string nextInstrStr = targetLabel.Substring (dash + 1);
					int nextInstr;
					int.TryParse (nextInstrStr, out nextInstr);
					nextObject = GameObject.Find (nextProgramObjectName);//.GetComponent<WisdominiObject> ();
					if (nextObject == null) { // meaning, it is not on the scene
						string folderName = ConversationFolderName;
						if (folderName.Equals (""))
							folderName = gameObject.name;
						string loadString = "Prefabs/NodeEditorObjects/" + folderName + "/" + nextProgramObjectName;
						nextObject = Instantiate (Resources.Load (loadString, typeof(GameObject))) as GameObject;

					}
					nextProgram = nextObject.GetComponent<WisdominiObject> ();
					ListString2D newProgram;
					newProgram = nextProgram.programs [0];
					programs [i] = newProgram;
					programPointer_ [i] = nextInstr - 1;
					Destroy (nextObject);
					hotLinkProgram = true;
				} else {
					int targetInstruction = getIndexOfLabel (i, op [1]);
					if (targetInstruction != -1)
						programPointer_ [i] = targetInstruction;
				}

			} 

			/*
			 * 
			 * play custom animation
			 * 
			 * playCustomAnim <anim>
			 * 
			 */
			else if (op [0].Equals ("playCustomAnim")) {

				CharacterGenerator chara = this.GetComponent<CharacterGenerator> ();
				if (chara != null) {
					int animNum;
					int.TryParse (op [1], out animNum);
					chara.customAnimationPlaying = animNum;
				}

			} else if (op [0].Equals ("stopCustomAnim")) {

				CharacterGenerator chara = this.GetComponent<CharacterGenerator> ();
				if (chara != null) {
					chara.customAnimationPlaying = -1;
				}

			}

			/*
			 * sendMail syntax:
			 * 
			 * sendMail <contents id>
			 * 
			 */
			else if (op [0].Equals ("sendMail")) {

				string uuid = SystemInfo.deviceUniqueIdentifier;

				/*WWWForm form = new WWWForm ();
				form.AddField ("uuid", uuid);
				form.AddField ("contid", op [1]);

				string serverURL = Utils.WisdominiServer + "/requestMail";
				wwwres = new WWW (serverURL, form);
				serverURL = "";

				string chops;

				chops = op [1];
				chops += "carroza";*/
				GameObject MailQueueGO = new GameObject ();
				MailQueueGO.name = "MailQueueAgent";
				MailQueueGO.AddComponent<QueueMailAgent> ().initialize (uuid, op [1]);
				DontDestroyOnLoad (MailQueueGO);

			}



			/*
		 	* if syntax:   (a query can only return a simple type:  int, float, string, etc...)
		 	* if <type of comparison> <true target> <false target> <op1 is constant?> <op2 is constant?>
		 	*              (<o1constant> | <target> <msg> <params>* \) (<o2constant> | <target> <msg> <params>*)
		 	*/
			else if (op [0].Equals ("if")) {

				string destinationLabel = op [2];
				int typeOfOp1;
				int op1_int;
				bool op1_bool;
				float op1_float;
				string op1_string;
				DataType op1_type;

				int typeOfOp2;
				int op2_int;
				float op2_float;
				bool op2_bool;
				string op2_string;
				DataType op2_type;

				int indexOfOperand2 = 7;

				Comparator compType;

				typeOfOp1 = typeOfOp2 = 1; // query until proven otherwise

				if (op [4].Equals ("Constant"))
					typeOfOp1 = 0;
				if (op [5].Equals ("Constant"))
					typeOfOp2 = 0;

				if (op [4].Equals ("Retrieve data"))
					typeOfOp1 = 2;
				if (op [5].Equals ("Retrieve data"))
					typeOfOp2 = 2;

				op1_type = DataType.Int; // integers unless proven otherwise
				op2_type = DataType.Int;

				op1_int = op2_int = 0;
				op1_float = op2_float = 0.0f;
				op1_string = op2_string = "";
				op1_bool = op2_bool = false;

				compType = Comparator.Equals;
				if (op [1].Equals ("="))
					compType = Comparator.Equals;
				if (op [1].Equals (">"))
					compType = Comparator.GreaterThan;
				if (op [1].Equals ("<"))
					compType = Comparator.LesserThan;
				if (op [1].Equals (">="))
					compType = Comparator.GreaterOrEqual;
				if (op [1].Equals ("<="))
					compType = Comparator.LesserOrEqual;
				if (op [1].Equals ("!="))
					compType = Comparator.NotEqual;
				if (op [1].Equals ("InRange"))
					compType = Comparator.InRange;
				if (op [1].Equals ("NotInRange"))
					compType = Comparator.NotInRange;
				if (op [1].Equals ("IsTrue"))
					compType = Comparator.IsTrue;
				if (op [1].Equals ("IsNotTrue"))
					compType = Comparator.IsNotTrue;

				if (typeOfOp1 == 0) { // constant

					// try to parse
					DataType guessedType;
					guessedType = DatatypeHelper.detectType (op [6]); // detect type of string rep of constant1
					switch (guessedType) {
					case DataType.Int: // int
						op1_type = DataType.Int;
						int.TryParse (op [6], out op1_int);
						break;
					case DataType.Float: // float
						op1_type = DataType.Float;
						float.TryParse (op [6], out op1_float);
						break;
					case DataType.Bool:
						bool.TryParse (op [6], out op1_bool);
						break;
					default: // string
						op1_type = DataType.String;
						op1_string = op [6];
						break;
					}

				} else if (typeOfOp1 == 2) { // Data retrieval
					string type;
					type = op [7];
					if (type.Equals ("Integer")) {
						op1_int = mcRef.getStorage ().retrieveIntValue (op [6]);
						op1_type = DataType.Int;
					}
					if (type.Equals ("Float")) {
						op1_float = mcRef.getStorage ().retrieveFloatValue (op [6]);
						op1_type = DataType.Float;

					}
					if (type.Equals ("Boolean")) {
						op1_type = DataType.Bool;
						op1_type = DataType.Bool;
						op1_type = DataType.Bool;
						op1_type = DataType.Bool;
						op1_type = DataType.Bool;
						MasterControllerScript mc = mcRef;
						//mc._storage = new DataStorage ();
						op1_type = DataType.Bool;
						DataStorage ds = mcRef.getStorage ();
						op1_bool = ds.retrieveBoolValue (op [6]);
					}
					if (type.Equals ("String")) {
						op1_type = DataType.String;
						op1_string = mcRef.getStorage ().retrieveStringValue (op [6]);
					}
					indexOfOperand2 = 8;

				} else { // Golly! A query!


					target = op [6];
					string methodName;
					GameObject targetRef;
					WisdominiObject targetScriptRef;
					if (target.Equals ("this")) {
						targetScriptRef = this;				
					} else {
						targetRef = GameObject.Find (target);
						targetScriptRef = targetRef.GetComponent<WisdominiObject> ();
					}
					//targetScriptRef.sendMessage (op); // stop using the implemented invoker, use C# mechanisms

					methodName = op [7];

					MethodInfo m = targetScriptRef.GetType ().GetMethod ("_wm_" + methodName);

					List<object> nativeParamList = new List<object> ();

					ParameterInfo[] paramInfoList = m.GetParameters ();

					for (int k = 0; k < paramInfoList.Length; ++k) {
						System.Type t = paramInfoList [k].ParameterType;
						if (t.Name.Equals ("Int32")) {
							int iParam;
							int.TryParse (op [8 + k], out iParam); // parse into integer
							nativeParamList.Add (iParam);
						} else if (t.Name.Equals ("Single")) {
							float fParam;
							float.TryParse (op [8 + k], out fParam);
							nativeParamList.Add (fParam);
						} else if (t.Name.Equals ("String")) {
							nativeParamList.Add (op [8 + k]); // no parsing needed
						}
					}

					indexOfOperand2 = 8 + 1 + paramInfoList.Length;

					object res = m.Invoke (targetScriptRef, nativeParamList.ToArray ());

					System.Type retType = m.ReturnType;

					if (retType.Name.Equals ("Int32")) {
						
						op1_int = (int)res;
						op1_type = DataType.Int;

					} else if (retType.Name.Equals ("Single")) {

						op1_float = (float)res;
						op1_type = DataType.Float;

					} else if (retType.Name.Equals ("String")) {

						op1_string = (string)res;
						op1_type = DataType.String;

					} else if (retType.Name.Equals ("Boolean")) {
						op1_bool = (bool)res;
						op1_type = DataType.Bool;
					}

				} // end of Golly! A query!


				// At this point, we have operand1 value & type

				if ((compType == Comparator.Equals) ||
				    (compType == Comparator.GreaterOrEqual) ||
				    (compType == Comparator.GreaterThan) ||
				    (compType == Comparator.LesserOrEqual) ||
				    (compType == Comparator.LesserThan) ||
				    (compType == Comparator.NotEqual)) { // binary comparator


					// Let's extract op2 value & type...


					if (typeOfOp2 == 0) { // constant

						// try to parse
						DataType guessedType;
						guessedType = DatatypeHelper.detectType (op [indexOfOperand2]); // detect type of string rep of constant1
						switch (guessedType) {
						case DataType.Int: // int
							op2_type = DataType.Int;
							int.TryParse (op [indexOfOperand2], out op2_int);
							break;
						case DataType.Float: // float
							op2_type = DataType.Float;
							float.TryParse (op [indexOfOperand2], out op2_float);
							break;
						case DataType.Bool:
							op2_type = DataType.Bool;
							bool.TryParse (op [indexOfOperand2], out op2_bool);
							break;
						default: // string
							op2_type = DataType.String;
							op2_string = op [indexOfOperand2];
							break;
						}


					} // end of constant
					else if (typeOfOp2 == 2) { // Data retrieval
						string type;
						type = op [indexOfOperand2 + 1];
						if (type.Equals ("Integer")) {
							op2_int = mcRef.getStorage ().retrieveIntValue (op [indexOfOperand2]);
							op2_type = DataType.Int;
						}
						if (type.Equals ("Float")) {
							op2_float = mcRef.getStorage ().retrieveFloatValue (op [indexOfOperand2]);
							op2_type = DataType.Float;

						}
						if (type.Equals ("Boolean")) {
							op2_type = DataType.Bool;
							op2_bool = mcRef.getStorage ().retrieveBoolValue (op [indexOfOperand2]);
						}
						if (type.Equals ("String")) {
							op2_type = DataType.String;
							op2_string = mcRef.getStorage ().retrieveStringValue (op [indexOfOperand2]);
						}

					} else { // Golly! A query! WARNING MISSING

					} // end of Golly a Query

					// now we have the value & type of operand 2...

					if ((compType == Comparator.Equals) ||
					    (compType == Comparator.NotEqual)) { // string admitted

						// determine joint type

						DataType jointType;

						if ((op1_type == DataType.String) ||
						    (op2_type == DataType.String)) { // if there is a string

							jointType = DataType.String;
							string finalop1, finalop2;

							finalop1 = finalop2 = "";

							// convert all operands into joint datatype
							if (op1_type == DataType.Int)
								finalop1 = op1_int.ToString ();
							if (op1_type == DataType.Float)
								finalop1 = op1_float.ToString ();
							if (op1_type == DataType.String)
								finalop1 = op1_string;
							if (op1_type == DataType.Bool)
								finalop1 = op1_bool.ToString ();

							if (op2_type == DataType.Int)
								finalop2 = op2_int.ToString ();
							if (op2_type == DataType.Float)
								finalop2 = op2_float.ToString ();
							if (op2_type == DataType.String)
								finalop2 = op2_string;
							if (op2_type == DataType.Bool)
								finalop2 = op1_bool.ToString ();
							
							// do the actual comparison
							if (compType == Comparator.Equals) {
							
								if (finalop1.Equals (finalop2))
									destinationLabel = op [2]; // TRUE branch
								else
									destinationLabel = op [3]; // FALSE branch

							} else {

								if (finalop1.Equals (finalop2))
									destinationLabel = op [3]; // FALSE branch
								else
									destinationLabel = op [2]; // TRUE branch
							
							}

						} // end of are there strings? 
						else if ((op1_type == DataType.Float) ||
						         (op2_type == DataType.Float)) { // there are no strings. Are there floats?

							jointType = DataType.Float;

							float finalop1, finalop2;

							finalop1 = finalop2 = 0.0f;

							// convert all operands into joint datatype
							if (op1_type == DataType.Int)
								finalop1 = (float)op1_int;
							if (op1_type == DataType.Float)
								finalop1 = op1_float;
							if (op1_type == DataType.Bool) {
								if (op1_bool)
									finalop1 = 1.0f;
								else
									finalop1 = 0.0f;
							}
								
						

							if (op2_type == DataType.Int)
								finalop2 = (float)op2_int;
							if (op2_type == DataType.Float)
								finalop2 = op2_float;
							if (op2_type == DataType.Bool) {
								if (op2_bool)
									finalop2 = 1.0f;
								else
									finalop2 = 0.0f;
							}

							if (compType == Comparator.Equals) {

								if (finalop1 == finalop2)
									destinationLabel = op [2];
								else
									destinationLabel = op [3];

							} else {

								if (finalop1 != finalop2)
									destinationLabel = op [2];
								else
									destinationLabel = op [3];

							}
							


						} // end of are there floats? 
						else if ((op1_type == DataType.Bool) || // are there any bools?
						         (op2_type == DataType.Bool)) {

							jointType = DataType.Bool;
							bool finalop1, finalop2;

							if (op1_type == DataType.Int) {
								if (op1_int != 0) {
									finalop1 = true;
								} else
									finalop1 = false;
							} else
								finalop1 = op1_bool;

							if (op2_type == DataType.Int) {
								if (op2_int != 0) {
									finalop2 = true;
								} else
									finalop2 = false;
							} else
								finalop2 = op2_bool;

							if (compType == Comparator.Equals) {

								if (op1_bool == op2_bool)
									destinationLabel = op [2];
								else
									destinationLabel = op [3];

							} else {

								if (op1_bool != op2_bool)
									destinationLabel = op [2];
								else
									destinationLabel = op [3];

							}


						} else { // just ints

							jointType = DataType.Int;

							// no need to convert anything


							if (compType == Comparator.Equals) {

								if (op1_int == op2_int)
									destinationLabel = op [2];
								else
									destinationLabel = op [3];

							} else {

								if (op1_int != op2_int)
									destinationLabel = op [2];
								else
									destinationLabel = op [3];

							}

						} // end of just ints
					} // end of comp == equals || notEqual


					if ((compType == Comparator.LesserThan) ||
					    (compType == Comparator.LesserOrEqual) ||
					    (compType == Comparator.GreaterOrEqual) ||
					    (compType == Comparator.GreaterThan)) { // strings not admitted

						if ((op1_type == DataType.String) ||
						    (op2_type == DataType.String)) { // if there is a string

							// make comparison fail
							destinationLabel = op [3]; // FAIL

						} // end of are there any strings?

						else if ((op1_type == DataType.String) ||
						         (op2_type == DataType.String)) { // are there any floats?

							float finalop1, finalop2;

							finalop1 = finalop2 = 0.0f;

							// convert all operands into joint datatype
							if (op1_type == DataType.Int)
								finalop1 = (float)op1_int;
							if (op1_type == DataType.Float)
								finalop1 = op1_float;


							if (op2_type == DataType.Int)
								finalop2 = (float)op2_int;
							if (op2_type == DataType.Float)
								finalop2 = op2_float;

							if (compType == Comparator.GreaterThan) {

								if (finalop1 > finalop2)
									destinationLabel = op [2];
								else
									destinationLabel = op [3];

							} else if (compType == Comparator.GreaterOrEqual) {

								if (finalop1 >= finalop2)
									destinationLabel = op [2];
								else
									destinationLabel = op [3];

							} else if (compType == Comparator.LesserThan) {

								if (finalop1 < finalop2)
									destinationLabel = op [2];
								else
									destinationLabel = op [3];

							} else if (compType == Comparator.LesserOrEqual) {

								if (finalop1 <= finalop2)
									destinationLabel = op [2];
								else
									destinationLabel = op [3];

							}

						} // end of are there any floats
						else { // just ints

							if (compType == Comparator.GreaterThan) {
							
								if (op1_int > op2_int)
									destinationLabel = op [2];
								else
									destinationLabel = op [3];
							
							} else if (compType == Comparator.GreaterOrEqual) {
							
								if (op1_int >= op2_int)
									destinationLabel = op [2];
								else
									destinationLabel = op [3];
							
							} else if (compType == Comparator.LesserThan) {
							
								if (op1_int < op2_int)
									destinationLabel = op [2];
								else
									destinationLabel = op [3];
							
							} else if (compType == Comparator.LesserOrEqual) {
							
								if (op1_int <= op2_int)
									destinationLabel = op [2];
								else
									destinationLabel = op [3];
							
							}

						} // end of just ints

					} // end of non-strings comparators


				} // end of binary comparator


				else if ((compType == Comparator.InRange) ||
				         (compType == Comparator.NotInRange)) { // range comparator



				} // end of range comparator


				else { // unary comparator

					if (compType == Comparator.IsTrue) { // integer op1 expected...

						if (op1_type == DataType.Int) {
							if (op1_int != 0)
								destinationLabel = op [2]; // TRUE result
							else
								destinationLabel = op [3]; // FALSE result
						} else
							destinationLabel = op [3]; // FALSE result
							

					}
					if (compType == Comparator.IsNotTrue) { // integer op1 expected...

						if (op1_type == DataType.Int) {
							if (op1_int == 0)
								destinationLabel = op [2]; // TRUE result
							else
								destinationLabel = op [3]; // FALSE result
						} else
							destinationLabel = op [3]; // FALSE result
						
					}

				} // end of unary comparator


				// finally, alter flow of program
				if (hasCorrectLinkSyntax (destinationLabel)) {
					GameObject nextObject;
					WisdominiObject nextProgram;
					int dash = destinationLabel.IndexOf ('/');
					string nextProgramObjectName = destinationLabel.Substring (0, dash);
					string nextInstrStr = destinationLabel.Substring (dash + 1);
					int nextInstr;
					int.TryParse (nextInstrStr, out nextInstr);
					nextObject = GameObject.Find (nextProgramObjectName);//.GetComponent<WisdominiObject> ();
					if (nextObject == null) { // meaning, it is not on the scene
						string folderName = ConversationFolderName;
						if (folderName.Equals (""))
							folderName = gameObject.name;
						string loadString = "Prefabs/NodeEditorObjects/" + folderName + "/" + nextProgramObjectName;
						nextObject = Instantiate (Resources.Load (loadString, typeof(GameObject))) as GameObject;

					}
					nextProgram = nextObject.GetComponent<WisdominiObject> ();
					ListString2D newProgram;
					newProgram = nextProgram.programs [0];
					programs [i] = newProgram;
					programPointer_ [i] = nextInstr - 1;
					Destroy (nextObject);
					hotLinkProgram = true;
				} else {
					int targetInstruction = getIndexOfLabel (i, destinationLabel);
					if (targetInstruction != -1)
						programPointer_ [i] = targetInstruction; // no need to -1, becuse we just skip a label
				}

			} // end of if(op[0]=="if")

			else if (op [0].Equals ("chispAlert")) {

				UIChispAlert calert = GameObject.Find ("ChispAlert").GetComponent<UIChispAlert> ();
				if (rosetta == null)
					rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();
				string text = rosetta.retrieveString (op [3]);
				calert._wa_alert (this, text);
				calert.registerWaitingObject (this, i);
				this.programIsWaitingForActionToComplete [i] = true;

			} else if (op [0].Equals ("closeChispAlert")) {

				UIChispAlert calert = GameObject.Find ("ChispAlert").GetComponent<UIChispAlert> ();
				calert.close ();

			} else if (op [0].Equals ("sayFromStringBank")) {



			} else if (op [0].Equals ("resetAnswers")) {

				DialogueObject dobj = GameObject.Find ("DialogueObject").GetComponent<DialogueObject> ();

				dobj.resetAnswers ();

			} else if (op [0].Equals ("disableInteraction")) {

				Interactor inter = this.GetComponent<Interactor> ();
				if (inter != null) {
					inter._wm_disable ();
				} 

			} else if (op [0].Equals ("enableInteraction")) {

				Interactor inter = this.GetComponent<Interactor> ();
				if (inter != null) {
					inter._wm_enable ();
				} 

			} else if (op [0].Equals ("disableAnswer")) {
				
				int answ;
				DialogueObject dobj = GameObject.Find ("DialogueObject").GetComponent<DialogueObject> ();

				bool allAnswers;
				bool.TryParse (op [2], out allAnswers);
				if (allAnswers) {
					dobj.disableAllAnswers ();
				} else {

					bool isStoredValue;
					bool.TryParse (op [3], out isStoredValue);

					if (isStoredValue) {
						answ = ds.retrieveIntValue (op [1]);

					} else {
						int.TryParse (op [1], out answ);
					}

					dobj.disableAnswer (answ);

				}

			} else if (op [0].Equals ("enableAnswer")) {

				int answ;
				DialogueObject dobj = GameObject.Find ("DialogueObject").GetComponent<DialogueObject> ();


				bool allAnswers;
				bool.TryParse (op [2], out allAnswers);
				if (allAnswers) {
					dobj.enableAllAnswers ();
				} else {
					bool isStoredValue;
					bool.TryParse (op [3], out isStoredValue);

					if (isStoredValue) {
						answ = ds.retrieveIntValue (op [1]);

					} else {
						int.TryParse (op [1], out answ);
					}

					dobj.enableAnswer (answ);
				}
			


			} else if (op [0].Equals ("saveGame")) {

				mcRef.saveGame (false);

			} else if (op [0].Equals ("replaceProgram")) {

				WisdominiObject nextProgram;
				GameObject nextObject;

				bool instantiatedObject = false;

				string nextProgramObjectName = op [1];
				int nextProgramFirstInstruction = 0;

				int slashPos = op [1].IndexOf ('/');
				if (slashPos != -1) {
					nextProgramObjectName = op [1].Substring (0, slashPos);
					string nextProgramFirstInstructionStr = op [1].Substring (slashPos + 1);
					int.TryParse (nextProgramFirstInstructionStr, out nextProgramFirstInstruction);
				}

				nextObject = GameObject.Find (nextProgramObjectName);//.GetComponent<WisdominiObject> ();
				if (nextObject == null) { // meaning, it is not on the scene
					string folderName = ConversationFolderName;
					if (folderName.Equals (""))
						folderName = gameObject.name;
					nextObject = Instantiate (Resources.Load ("Prefabs/NodeEditorObjects/" + folderName + "/" + op [1], typeof(GameObject))) as GameObject;
					instantiatedObject = true;
				}
				nextProgram = nextObject.GetComponent<WisdominiObject> ();

				int nextEvent = nextProgram.programIndexFromEventName (op [2]);
				if (nextEvent < 0)
					nextEvent = 0;
				ListString2D newProgram;
				newProgram = nextProgram.programs [nextEvent];

				bool continueExec;
				bool.TryParse (op [3], out continueExec);

				if (continueExec) {
					programStacks [i].Push (programs [i]); // push onto stack if we need to resume execution later
					programStacksPointers [i].Push (programPointer_ [i] + 1); // save program pointer (next instr)
				}

				/* replace program */
				programs [i] = newProgram;
				currentProgram = newProgram;
				programPointer_ [i] = nextProgramFirstInstruction - 1;

				if (instantiatedObject)
					Destroy (nextObject);
				hotLinkProgram = true;


			} else if (op [0].Equals ("say")) {
				DialogueObject dobj = GameObject.Find ("DialogueObject").GetComponent<DialogueObject> ();
				CharacterGenerator chara = this.GetComponent<CharacterGenerator> ();
				//string text = op [1];
				if (rosetta == null)
					rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();

				dobj.setMiniature (null, null, 0);
				dobj.setMiniature (null, null, 1);
				if ((chara.leftMiniatureN != null) && (chara.leftMiniatureB != null)) {
					dobj.setMiniature (chara.leftMiniatureN, chara.leftMiniatureB, 0);
				}
				if (chara.centerMiniatureN != null) {
					dobj.setMiniature (chara.centerMiniatureN, chara.centerMiniatureB, 1);
				}

                //string text = rosetta.retrieveString (op [4]);
                string text = op[1];
				bool wait;
				bool.TryParse (op [2], out wait);

				if (op [3].Equals ("Player"))
					dobj._wm_setSpeaker ("right");
				if (op [3].Equals ("NPC1"))
					dobj._wm_setSpeaker ("left");
				if (op [3].Equals ("NPC2"))
					dobj._wm_setSpeaker ("center");

				dobj._wa_say (this, text, true);
				dobj.registerWaitingObject (this, i);
				this.programIsWaitingForActionToComplete [i] = true;

				if (isRecordingConversation) {
					ds.storeStringValue ("Conversation" + convIndex + "_" + strIndex, op [4]);
					int cl = ds.retrieveIntValue ("LengthOfConversation" + convIndex);
					ds.storeIntValue ("LengthOfConversation" + convIndex, cl + 1);
					ds.storeStringValue ("Conversation" + convIndex + "_" + strIndex + "Speaker", op [3]);
					strIndex++;
				}
			} else if (op [0].Equals ("sayImage")) {
				DialogueObject dobj = GameObject.Find ("DialogueObject").GetComponent<DialogueObject> ();
				//string text = op [1];

				string subs = op [1].Substring ("Assets/Resources/".Length);
				subs = subs.Substring (0, subs.Length - 4);
				Sprite img = Resources.Load<Sprite> (subs);
				bool wait;
				bool.TryParse (op [2], out wait);

				if (op [3].Equals ("Player"))
					dobj._wm_setSpeaker ("right");
				if (op [3].Equals ("NPC1"))
					dobj._wm_setSpeaker ("left");
				if (op [3].Equals ("NPC2"))
					dobj._wm_setSpeaker ("center");

				dobj._wa_say (this, img, true);
				dobj.registerWaitingObject (this, i);
				this.programIsWaitingForActionToComplete [i] = true;

				if (isRecordingConversation) {
					ds.storeStringValue ("Conversation" + convIndex + "_" + strIndex, "<image>" + op [1]);
					int cl = ds.retrieveIntValue ("LengthOfConversation" + convIndex);
					ds.storeIntValue ("LengthOfConversation" + convIndex, cl + 1);
					ds.storeStringValue ("Conversation" + convIndex + "_" + strIndex + "Speaker", op [3]);
					strIndex++;
				}
			} else if (op [0].Equals ("setMiniature")) {
				DialogueObject dobj = GameObject.Find ("DialogueObject").GetComponent<DialogueObject> ();
				CharacterGenerator chara = this.GetComponent<CharacterGenerator> ();

				string filename = "";
				if (op [1].Equals ("Neutral")) {
					filename = chara.neutralMiniature.name;
				}
				if (op [1].Equals ("Sad")) {
					filename = chara.sadMiniature.name;
				}



				if (op [2].Equals ("Player"))
					dobj._wm_setMiniature ("right", filename);
				if (op [2].Equals ("NPC1"))
					dobj._wm_setMiniature ("left", filename);
				if (op [2].Equals ("NPC2"))
					dobj._wm_setMiniature ("center", filename);


			} else if (op [0].Equals ("ask")) {

				//i = i + 2;
				//i = i - 2;



				if (waitingForAnswer [i] == false) { // first pass of the instruction (selecting answer)

					DialogueObject dobj = GameObject.Find ("DialogueObject").GetComponent<DialogueObject> ();
					int nAnswers;
					int.TryParse (op [1], out nAnswers);
					List<string> ansList = new List<string> ();
					for (int k = 0; k < nAnswers; ++k) {
						ansList.Add (op [2 + k]);
					}
					dobj._wa_ask (this, ansList.ToArray ());
					dobj.registerWaitingObject (this, i);
					programIsWaitingForActionToComplete [i] = true;
					waitingForAnswer [i] = true;
					programPointer_ [i]--; // prevent programPointer from advancing

				} else { // second pass (branching)


					DialogueObject dobj = GameObject.Find ("DialogueObject").GetComponent<DialogueObject> ();
					int nAnswers;
					int.TryParse (op [1], out nAnswers);
					int slot = 2 + nAnswers + dobj.selectedAnswer;
					string targetLabel = op [2 + nAnswers + dobj.selectedAnswer - 1];
					// if targetLabel has replacement syntax...
					if (hasCorrectLinkSyntax (targetLabel)) {
						GameObject nextObject;
						WisdominiObject nextProgram;
						int dash = targetLabel.IndexOf ('/');
						string nextProgramObjectName = targetLabel.Substring (0, dash);
						string nextInstrStr = targetLabel.Substring (dash + 1);
						int nextInstr;
						int.TryParse (nextInstrStr, out nextInstr);
						nextObject = GameObject.Find (nextProgramObjectName);//.GetComponent<WisdominiObject> ();
						if (nextObject == null) { // meaning, it is not on the scene
							string folderName = ConversationFolderName;
							if (folderName.Equals (""))
								folderName = gameObject.name;
							string loadString = "Prefabs/NodeEditorObjects/" + folderName + "/" + nextProgramObjectName;
							nextObject = Instantiate (Resources.Load (loadString, typeof(GameObject))) as GameObject;

						}
						nextProgram = nextObject.GetComponent<WisdominiObject> ();
						ListString2D newProgram;
						newProgram = nextProgram.programs [0];
						programs [i] = newProgram;
						programPointer_ [i] = nextInstr - 1;
						Destroy (nextObject);
						hotLinkProgram = true;
					} else {
						int targetInstruction = getIndexOfLabel (i, op [2 + nAnswers + dobj.selectedAnswer - 1]);
						if (targetInstruction != -1)
							programPointer_ [i] = targetInstruction;
					}
					waitingForAnswer [i] = false;

					if (isRecordingConversation) {
						string rosId = op [2 + 2 * nAnswers + dobj.selectedAnswer - 1];
						ds.storeStringValue ("Conversation" + convIndex + "_" + strIndex, rosId);
						int cl = ds.retrieveIntValue ("LengthOfConversation" + convIndex);
						ds.storeIntValue ("LengthOfConversation" + convIndex, cl + 1);
						ds.storeStringValue ("Conversation" + convIndex + "_" + strIndex + "Speaker", "Player");
						strIndex++;
					}

				}
				
			} else if (op [0].Equals ("clearDialogue")) {
				DialogueObject dobj = GameObject.Find ("DialogueObject").GetComponent<DialogueObject> ();

				dobj._wm_clear ();

			} else if (op [0].Equals ("addToInventory")) {


				
				mcRef.registerPickedUpObject (this.name);
				ObjectGeneratorScript obj = this.gameObject.GetComponent<ObjectGeneratorScript> ();
				if (obj != null) { // if this wisdominiobject is a Item
					if (obj.FlameHeroClass != 0) {

                        //string lvl = GameObject.Find("LevelController").GetComponent<LevelControllerScript>().locationName.Substring(0, 6);
                        string lvl = SceneManager.GetActiveScene().name.Substring(0, 6);
							mcRef.getStorage ().storeStringValue ("CurrentLevelFlame", lvl);
							mcRef.getStorage ().storeIntValue ("CurrentFlameIndex", obj.FlameHeroClass-1);
							mcRef.getStorage ().storeIntValue ("QAHeroClass" + lvl + (obj.FlameHeroClass-1), obj.FlameHeroClass-1); // a little ad-hoc solution
							// store flame related data in case we have to resurrect the flame
							mcRef.getStorage().storeStringValue("FlameResurrectionName" + lvl + (obj.FlameHeroClass-1), this.name);
							mcRef.getStorage ().storeStringValue ("FlameResurrectionLocation" + lvl + (obj.FlameHeroClass-1), lvl);
					}
						
				}

			} else if (op [0].Equals ("followPlayer")) {

				CharacterGenerator chara = this.GetComponent<CharacterGenerator> ();

				chara.startFollowingPlayer ();


			} else if (op [0].Equals ("stopFollowingPlayer")) {

				CharacterGenerator chara = this.GetComponent<CharacterGenerator> ();

				chara.stopFollowingPlayer ();


			} else if (op [0].Equals ("fadeIn")) {

				Renderer rend = this.GetComponentInChildren<Renderer> ();
				material = rend.material;
				targetOpacity = 1.0f;
//				rend.material.color = new Color (1, 1, 1, 0);
//
//				do {
//					iTween.ColorUpdate (this.gameObject, new Color (1, 1, 1, 1), 4);
//
//				} while (rend.material.color.a < 1);
			} else if (op [0].Equals ("fadeOut")) {

				Renderer rend = this.GetComponentInParent<Renderer> ();
				if (rend == null)
					rend = this.GetComponentInChildren<Renderer> ();
				material = rend.material;
				targetOpacity = 0.0f;
				//rend.material.color = new Color (1, 1, 1, 1);

//				do {
//					iTween.ColorUpdate (rend.gameObject, new Color (1, 1, 1, 0), 4);
//
//				} while (rend.material.color.a > 0);
			} else if (op [0].Equals ("retrievePlayerPos")) {

				Vector3 posPlayer = GameObject.Find ("Player").transform.position;

				this.transform.position = posPlayer;
			} else if (op [0].Equals ("recordConversation")) {

				//DataStorage ds = mcRef.getStorage ();
				isRecordingConversation = true;
				/* find out if conversation has been previously recorded */
				/* Note: should NOT be necessary, but for testing purposes */
				/* we'll do the check. Can be removed for Release */

				/* we store rosettaid as TitleOfConversation<n> */



				int noc = ds.retrieveIntValue ("NumberOfStoredConversations");
				convIndex = noc;
				for (int index = 0; index < noc; ++index) {
					string convTitle = ds.retrieveStringValue ("TitleOfConversation" + index);
					if (convTitle.Equals (op [2])) { 
						convIndex = index; 
						continue; 
					}
				}

				if (convIndex == noc) {
					ds.storeIntValue ("NumberOfStoredConversations", noc + 1);
				}
				ds.storeIntValue ("LengthOfConversation" + convIndex, 0);
				ds.storeStringValue ("TitleOfConversation" + convIndex, op [2]);
				
				strIndex = 0;


			} else if (op [0].Equals ("dipmusic")) {
				LevelControllerScript lvl = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
				float time;
				float.TryParse (op [1], out time);
				lvl.dipMusic (time);
			}

			else if(op[0].Equals("alert")) {

				alertImageController alert = GameObject.Find ("AlertImage").GetComponent<alertImageController> ();
				if(rosetta == null) rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();
                string text = op[1];//rosetta.retrieveString (op [4]);
				float delay;
				float.TryParse (op [2], out delay);
				alert._wa_setAlertMessageWithTimeout(this, text, delay);
				if (op [3].Equals ("wait")) {
					alert.registerWaitingObject (this, i);
					this.programIsWaitingForActionToComplete [i] = true;
				}

			}

			else if(op[0].Equals("enabled")) 
			{
				_wm_Enabled ();
			}

			else if(op[0].Equals("disabled")) 
			{
				_wm_Disabled ();
			}


			else if (op [0].Equals ("updateData")) {

				if (op [1].Equals ("Integer")) {

					int op1, op2;
					op1 = mcRef.getStorage ().retrieveIntValue (op [2]);

					if (op [4].Equals ("Constant")) {
						int.TryParse (op [5], out op2);
					} else {
						op2 = mcRef.getStorage ().retrieveIntValue (op [5]);
					}

					if(op[3].Equals("Add")) {
						op1 += op2;
						mcRef.getStorage ().storeIntValue (op [2], op1);
					}
					if(op[3].Equals("Substract")) {
						op1 -= op2;
						mcRef.getStorage ().storeIntValue (op [2], op1);
					}
					if(op[3].Equals("Maximum")) {
						if(op2 > op1) op1 = op2;
						mcRef.getStorage ().storeIntValue (op [2], op1);
					}
					if(op[3].Equals("Minimum")) {
						if(op2 < op1) op1 = op2;
						mcRef.getStorage ().storeIntValue (op [2], op1);
					}
					if(op[3].Equals("Multiply")) {
						op1 *= op2;
						mcRef.getStorage ().storeIntValue (op [2], op1);
					}
					if(op[3].Equals("Divide")) {
						op1 /= op2;
						mcRef.getStorage ().storeIntValue (op [2], op1);
					}
					if(op[3].Equals("Set")) {
						op1 = op2;
						mcRef.getStorage ().storeIntValue (op [2], op1);
					}

				}
				if (op [1].Equals ("Float")) {

					float op1, op2;
					op1 = mcRef.getStorage ().retrieveFloatValue (op [2]);

					if (op [4].Equals ("Constant")) {
						float.TryParse (op [5], out op2);
					} else {
						op2 = mcRef.getStorage ().retrieveFloatValue (op [5]);
					}

					if(op[3].Equals("Add")) {
						op1 += op2;
						mcRef.getStorage ().storeFloatValue (op [2], op1);
					}
					if(op[3].Equals("Substract")) {
						op1 -= op2;
						mcRef.getStorage ().storeFloatValue (op [2], op1);
					}
					if(op[3].Equals("Maximum")) {
						if(op2 > op1) op1 = op2;
						mcRef.getStorage ().storeFloatValue (op [2], op1);
					}
					if(op[3].Equals("Minimum")) {
						if(op2 < op1) op1 = op2;
						mcRef.getStorage ().storeFloatValue (op [2], op1);
					}
					if(op[3].Equals("Multiply")) {
						op1 *= op2;
						mcRef.getStorage ().storeFloatValue (op [2], op1);
					}
					if(op[3].Equals("Divide")) {
						op1 /= op2;
						mcRef.getStorage ().storeFloatValue (op [2], op1);
					}
					if(op[3].Equals("Set")) {
						op1 = op2;
						mcRef.getStorage ().storeFloatValue (op [2], op1);
					}

				}
				if (op [1].Equals ("Boolean")) {

					bool op1, op2;
					op1 = mcRef.getStorage ().retrieveBoolValue (op [2]);

					if (op [4].Equals ("Constant")) {
						bool.TryParse (op [5], out op2);
					} else {
						op2 = mcRef.getStorage ().retrieveBoolValue (op [5]);
					}


					if(op[3].Equals("Set")) {
						op1 = op2;
						mcRef.getStorage ().storeBoolValue (op [2], op1);
					}
					if(op[3].Equals("Invert")) {
						op1 = !op1;
						mcRef.getStorage ().storeBoolValue (op [2], op1);
					}

				}
				if (op [1].Equals ("String")) {

					string op1, op2;
					op1 = mcRef.getStorage ().retrieveStringValue (op [2]);

					if (op [4].Equals ("Constant")) {
						op2 = op [5];
					} else {
						op2 = mcRef.getStorage ().retrieveStringValue (op [5]);
					}

					if (op [3].Equals ("Concatenate")) {
						op1 += op2;
						mcRef.getStorage ().storeStringValue (op [2], op1);
					}
					if(op[3].Equals("Set")) {
						op1 = op2;
						mcRef.getStorage ().storeStringValue (op [2], op1);
					}

				}



			} // end of updateData
			else if (op [0].Equals("storeData")) {

				if (op [1].Equals ("Integer")) {

					int iValue;
					int.TryParse (op [3], out iValue);
					if (mcRef == null)
						mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
					mcRef.getStorage ().storeIntValue (op [2], iValue);

				}
				if (op [1].Equals ("Float")) {

					float fValue;
					float.TryParse (op [3], out fValue);
					mcRef.getStorage ().storeFloatValue (op [2], fValue);

				}
				if (op [1].Equals ("Boolean")) {

					bool bValue;
					bool.TryParse (op [3], out bValue);
					mcRef.getStorage ().storeBoolValue (op [2], bValue);

				}
				if (op [1].Equals ("String")) {

					mcRef.getStorage ().storeStringValue (op [2], op[3]);

				}

			}


			// Then there will come the game-specific ones:
			//  addToInventory
			//  addToEnergyCounter... etc...

			++programPointer_[i]; // proceed to next command
			if(hotLinkProgram) { --i; hotLinkProgram = false; }
		
		}



	}

	public virtual void sendMessage(params string[] p) {

	}

	public virtual void startAction(params string[] p) {

	}

	public string[] getAllLabels(int programIndex) {

		ListString2D program;
		List<string> labels;

		labels = new List<string> ();

		labels.Add ("By name...");

		program = programs [programIndex];


		for (int i = 0; i < program.theList.Count; ++i) {

			if (program.theList [i].theList [0].Equals ("makeLabel")) { 	// if op == makeLabel
				labels.Add (program.theList [i].theList [1]); 			// remember first parameter
			}

		}

		string[] result;
		result = labels.ToArray (); 
		return result;

	}

	public int getIndexOfLabel(int programIndex, string label) {

		ListString2D program;

		program = programs [programIndex];

		for (int i = 0; i < program.theList.Count; ++i) {

			if (program.theList [i].theList [0].Equals ("makeLabel")) { // if op == makeLabel
				if(program.theList[i].theList[1].Equals(label)) return i;
			}

		}

		return -1;

	}

	public void setOpacity(float op) {
		Renderer rend = this.GetComponentInChildren<Renderer> ();
		material = rend.material;
		targetOpacity = op;
		opacity = op;
		Color c = material.color;
		c.a = op;
		material.color = c;
	}

	public float getOpacity() {
		return opacity;
	}
	

	public void Update () {

		// fadein and fadeout
		if (opacity < targetOpacity) {
			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > targetOpacity) {
				opacity = targetOpacity;
			}
			if (material != null) {
				Color c = material.color;
				c.a = opacity;
				material.color = c;
			}
		}
		if (opacity > targetOpacity) {
			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < targetOpacity) {
				opacity = targetOpacity;
			}
			if (material != null) {
				Color c = material.color;
				c.a = opacity;
				material.color = c;
			}
		}

		if (mcRef == null)
			mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();

		if (firstUpdate) {
			int i;
			for (i = 0; i < eventNamesInOrder.Count; ++i) {
				if (eventNamesInOrder [i].Equals ("onAwake") && (!preventAwake)) {
					this.startProgram (i);
				}
			}
			firstUpdate = false;
		}
		UpdateProgram ();
	
	}

	public string[] eventNames() {

		return eventNamesInOrder.ToArray ();

	}

	// WARNING mover a Utils
	/* This check is a tad simplistic, but will do */
	bool hasCorrectLinkSyntax(string s) {

		int middle = s.IndexOf ('/');
		if (middle != -1)
			return true;
		else
			return false;

	}

	public void _wm_hide() {
		this.transform.position = new Vector3 (0, -10000, 0);
		Rigidbody r = this.GetComponent<Rigidbody> ();
		if (r != null) { // prevent hidden object to fall to Y = minus infinity
			r.isKinematic = true;
			r.useGravity = false; 
		}
	}

	public void _wm_Destroy() {
		Destroy (this.gameObject);
	}

	public void stopAllEvents() {
		for (int i = 0; i < programs.Count; ++i) {
			stopProgram (i);
		}
	}

	public void _stopAllEvents() {
		stopAllEvents ();
	}

	public void _wm_startEventByName(string ename) {

		int eventToRun = programIndexFromEventName (ename);
		if (eventToRun != -1) {
			startProgram (eventToRun);
		}

	}

	public void _wm_Enabled()
	{
		Renderer[] rs = GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in rs)
			r.enabled = true;
		Collider[] cs = GetComponentsInChildren<Collider> ();
		foreach (Collider c in cs)
			c.enabled = true;
	}

	public void _wm_Disabled()
	{
		Renderer[] rs = GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in rs)
			r.enabled = false;
		Collider[] cs = GetComponentsInChildren<Collider> ();
		foreach (Collider c in cs)
			c.enabled = false;
	}

}
