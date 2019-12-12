using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationGenerator : WisdominiObject {

	/*
	 * A medida que se estabilice la funcionalidad, hay que cambiar los nombres
	 * de algunas variables, que son poco claros
	 * 
	 * de momento: selection: array que contiene ints, uno por evento, con la selección
	 *      del popup de evento  CAMBIAR NOMBREEEE
	 * 
	 * 
	 * 
	 * 
	 */




	private int showingFrame;
	private float frameTime;

	[HideInInspector]
	public string namer;

	[HideInInspector]
	public int nEvents;

	[HideInInspector]
	//public bool pickable;
	//public string WOname;
	const int maxEvents = 20;

	public bool canSpeak;
	//public ConversationNode conversation;

	/*Lis of textures to use
	 */
	//public Texture[] images;


	public WisdominiObject[] NextProgram;

	public float WalkingAnimationSpeed = 1.0f;

	public Texture[] otherImages;


	[HideInInspector]
	public int[] editorSelected;

	[HideInInspector]
	public List<int> selection;

	[HideInInspector]
	//public List<List<int>> action;
	public List<ListInt1D> action;

	public Sprite[] sayImages;

	public StringBank[] stringBank;

	public Texture neutralMiniature;
	public Texture happyMiniature;
	public Texture sadMiniature;
	public Texture worriedMiniature;

	public float animationSpeed;
	float animationDelay;

	/* list of gameobjects to message 
	 */
	public GameObject[] messageTargets;

	public AudioClip[] soundEffects;

	public AudioClip[] music;

	/* stuff for the node editor */
	public bool isRoot;
	public float windowX, windowY;


	void Awake() {

		/*if (spriteHolder != null) {

			if (images.Length > 0) {

				rendRef = spriteHolder.GetComponent<Renderer> ();
				rendRef.material.mainTexture = images [0];

			}

		}
		*/

		//images = Idle;

	}

	void Reset() {

		Awake ();

	}




	/* Manage collisions/overlaps */

	void OnTriggerEnter(Collider other) {
		/*
		if (pickable) {
			// search for program index for event "onPickup"
			int i = programIndexFromEventName ("onPickup"); 
			if (i != -1) { // if successfully found...
				startProgram(i); // start the program
				MasterControllerScript mcRef = GameObject.Find("MasterController").GetComponent<MasterControllerScript>();
				mcRef.registerPickedUpObject (this.name);
			}
		}
		*/
		int j = programIndexFromEventName ("onOverlap");
		if (j != -1) {
			startProgram (j);
		}

	}

	// Use this for initialization
	new void Start () {




		base.Start ();
	}

	public void initialize() {

		//pickable = false;
		nEvents = 0;
		editorSelected = new int[maxEvents];
		for (int i = 0; i < maxEvents; ++i) {
			editorSelected[i] = 0;
		}

		selection = new List<int> ();
		action = new List<ListInt1D> ();


		//tex = null;

		reset ();


	}

	// Update is called once per frame
	new void Update () {


		/* Update base object
		 */
		base.Update ();


	}

	public void addEvent() {

		//++nEvents;
		selection.Add (0);
		action.Add (new ListInt1D ());

	}

	public void removeEvent(int index) {

		if (selection.Count > 0) {
			--nEvents;
			selection.RemoveAt (index);
			action.RemoveAt (index); // Leave list to garbage collector
		}

	}

	public int getEvents() {

		return selection.Count;

	}



	public int getTypeOfEvent(int index) {

		if (index < selection.Count ) {
			return selection.ToArray () [index];
		} else
			return 0;

	}

	public void setTypeOfEvent(int index, int type) {

		if (index < selection.Count ) {
			selection[index] = type;
		}

	}

	public void addAction(int eIndex) {

		action [eIndex].theList.Add (0);

	}

	public void setAction(int eIndex, int aIndex, int act) {

		if((eIndex < action.Count) && (aIndex < action[eIndex].theList.Count)) {
			action[eIndex].theList[aIndex] = act;
		}

	}

	public void removeAction(int eIndex, int aIndex) {

		action [eIndex].theList.RemoveAt (aIndex);

	}

	public void insertAction(int eIndex, int aIndex) {

		action [eIndex].theList.Insert (aIndex, 0);

	}

	int indexFromString(string [] list, string item) {

		for(int i = 0; i<list.Length; ++i) {
			if (list [i].Equals (item))
				return i;
		}

		return -1;

	}

	public void parseProgram(int eIndex, string prg, string [] actions) {

		int nActs = 0;

		char[] lineDelimiters = { '\n' };
		char[] spaceSeparators = { ' ', '\n' };

		string[] instructions = prg.Split (lineDelimiters);

		nActs = (instructions.Length) - 1;

		programs [eIndex] = new ListString2D ();
		action [eIndex] = new ListInt1D ();

		for (int i = 0; i < nActs; ++i) {

			ListString1D newInstruction = new ListString1D ();
			string[] instrParams = instructions [i].Split (spaceSeparators);

			newInstruction.theList.AddRange (instrParams);

			programs [eIndex].theList.Add (newInstruction);

			addAction (eIndex);
			setAction (eIndex, i, indexFromString (actions, instrParams [0]));

		}


	}

	public int nActions(int eIndex) {

		if(eIndex < action.Count) {

			return action[eIndex].theList.Count;

		}
		return 0;

	}

	public int getTypeOfAction(int eIndex, int aIndex) {

		if ((eIndex < action.Count) && (aIndex < action [eIndex].theList.Count)) {
			return action [eIndex].theList[aIndex];
		} else
			return 0;

	}

	public void setTypeOfAction(int eIndex, int aIndex, int type) {

		if ((eIndex < action.Count) && (aIndex < action [eIndex].theList.Count)) {
			action [eIndex].theList[aIndex] = type;
		} else
			return;

	}


}
