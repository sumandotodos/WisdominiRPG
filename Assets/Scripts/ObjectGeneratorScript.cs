using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ObjectGeneratorScript : Interactor {

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

	public GameObject spriteHolder;
	public string nameMat;
	public bool commonMat;

	private Material matRef;
	private SpriteRenderer rendRef;
	private int showingFrame;
	private float frameTime;

	[HideInInspector]
	public string namer;

	[HideInInspector]
	public int nEvents;

	public bool interactable;
	public bool pickable;
	//public string WOname;
	const int maxEvents = 20;

	/*Lis of textures to use
	 */
	public Texture[] images;
	public Sprite[] imagesSprite;

	[HideInInspector]
	public RGBColorItem rgb;
	[HideInInspector]
	public RGBColorItem ready;

	[HideInInspector]
	public int[] editorSelected;

	[HideInInspector]
	public List<int> selection;

	[HideInInspector]
	//public List<List<int>> action;
	public List<ListInt1D> action;

	public float animationSpeed;
	float animationDelay;

	public WisdominiObject[] NextProgram;

	/* list of gameobjects to message 
	 */
	public GameObject[] messageTargets;

	public AudioClip[] soundEffects;

	public AudioClip[] music;

	public string interactIconName;

	public int FlameHeroClass;
	//public int FlameNumber;


	void Reset() 
	{
		//Awake ();
	}

	void OnDrawGizmos() 
	{

	}

	public override void effect() 
	{
		int prg = programIndexFromEventName ("onInteract");
		if (prg != -1) {
			startProgram (prg);
		}
	}

	public override string interactIcon() 
	{
		return interactIconName;
	}


	void OnTriggerEnter(Collider other) 
	{
		// Only the player can pick up things and stuff...
		if (other.tag != "Player")
			return;
		if (other.name == "Main Camera") {
			return;
		}

		//Debug.Log ("<color=purple>" + other.name + " entered " + this.name + "</color>");

		if (pickable) 
		{
			MasterControllerScript mcRef = GameObject.Find("MasterController").GetComponent<MasterControllerScript>();

			// search for program index for event "onPickup"
			int i = programIndexFromEventName ("onPickup"); 
			if (i != -1) { // if successfully found...				

				startProgram(i); // start the program
			}

			if (FlameHeroClass > 0) { // if it is a flame, a little ad-hoc solution
				//string lvl = GameObject.Find("LevelController").GetComponent<LevelControllerScript>().locationName.Substring(0, 6);
                string lvl = SceneManager.GetActiveScene().name;
                mcRef.getStorage ().storeStringValue ("CurrentLevelFlame", lvl);
				mcRef.getStorage ().storeIntValue ("CurrentFlameIndex", FlameHeroClass-1);
				mcRef.getStorage ().storeIntValue ("QAHeroClass" + lvl + (FlameHeroClass-1), FlameHeroClass-1); // a little ad-hoc solution
				// store flame related data in case we have to resurrect the flame
				mcRef.getStorage().storeStringValue("FlameResurrectionName" + lvl + (FlameHeroClass-1), this.name);
                mcRef.getStorage().storeStringValue("FlameResurrectionLocation" + lvl + (FlameHeroClass - 1), lvl);

			}
			mcRef.registerPickedUpObject (this.name);
		}

		if (!interactEnabled)
			return;

		int j = programIndexFromEventName ("onOverlap");
		if (j != -1) {
			startProgram (j);

		}
	}


	/* Manage collisions/overlaps */

	void OnTriggerExit(Collider other) 
	{
		Debug.Log ("<color=purple>" + other.name + " exitted " + this.name + "</color>");
		int j = programIndexFromEventName ("onEndOverlap");
		if (j != -1) {
			startProgram (j);
			Debug.Log ("<color=red>" + other.name + " exitted " + this.name + "and started program " + j + "</color>");
		}
	}

	new void Start () 
	{	
		base.Start ();
		if (spriteHolder != null) 
		{
			rendRef = spriteHolder.GetComponent<SpriteRenderer> ();
		} 

		else
			rendRef = null;

		if (rendRef != null) 
		{
			//matRef = rendRef.material;
			matRef = rendRef.material;
			//			if (matRef != null) {
			//				matRef.SetFloat("_Mode", 2);
			//				matRef.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
			//				matRef.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
			//				matRef.SetInt("_ZWrite", 1);
			//				matRef.EnableKeyword("_ALPHATEST_ON");
			//				matRef.DisableKeyword("_ALPHABLEND_ON");
			//				matRef.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			//				matRef.renderQueue = 3000;
			//
			//			}
		}

		if (animationSpeed > 0.0f)
		{
			animationDelay = 1.0f / animationSpeed;
		} 
		else {
			animationDelay = 0.0f;
		}
		showingFrame = 0;
		frameTime = 0.0f;


	}

	public void initialize() {

		pickable = false;
		nEvents = 0;
		editorSelected = new int[maxEvents];
		for (int i = 0; i < maxEvents; ++i) {
			editorSelected[i] = 0;
		}
		//rgb = new RGBColorItem ();
		//ready = new RGBColorItem ();
		selection = new List<int> ();
		action = new List<ListInt1D> ();


		//tex = null;

		reset ();


	}

	new void Update () 
	{
		/* Refresh sprite face
		 */
		if (imagesSprite.Length > 0) { // if we have at least one image
			if (animationDelay == 0.0f) {
				showingFrame = 0;
			} else if ((Time.time - frameTime) > animationDelay) {
				showingFrame = (showingFrame + 1) % imagesSprite.Length;
				frameTime = Time.time;
			}
			if (spriteHolder != null)
				rendRef.sprite = imagesSprite [showingFrame];
		}
		/* Update base object
		 */
		base.Update ();	
	}

	public void addEvent() 
	{
		//++nEvents;
		selection.Add (0);
		action.Add (new ListInt1D ());
	}

	public void removeEvent(int index) 
	{
		if (selection.Count > 0) {
			--nEvents;
			selection.RemoveAt (index);
			action.RemoveAt (index); // Leave list to garbage collector
		}
	}

	public int getEvents() 
	{
		return selection.Count;
	}



	public int getTypeOfEvent(int index) 
	{
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
		char[] spaceSeparators = { '|', '\n' };

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

	public Sprite getTex() {

		return imagesSprite[0];

	}
}
