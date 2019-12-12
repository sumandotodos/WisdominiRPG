using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CharacterDirection { front, frontLeft, left, backLeft, back, backRight, right, frontRight, free };
public enum CharacterWalkingState { walking, standing, following };
public enum FollowState { follow, noFollow, rendervous };

public class CharacterGenerator : Interactor {
	
	// borrame luego
	public float pivotYRotation;

	[HideInInspector]
	Vector3 direction;

	//[HideInInspector]
	public float distanceToPlayer;
	//[HideInInspector]
	public int followingState;
	//[HideInInspector]
	public FollowState followState = FollowState.noFollow;
	[HideInInspector]
	public bool markedAsStartFollowing = false;

	int NFramesNeededToChangeDirection = 5;
	int RemainingFramesToChangeDirection;
	int RemainingFramesToReachEndpoint;


	public float relativeCoordinatesScale = 1.0f;

	//[HideInInspector]
	public int customAnimationPlaying = -1;
	float elapsedTime = 0.0f;
	/*
	 * A medida que se estabilice la funcionalidad, hay que cambiar los nombres
	 * de algunas variables, que son poco claros (ME PARECE QUE SE VAN A QUEDAR ASÍ)
	 * 
	 * de momento: selection: array que contiene ints, uno por evento, con la selección
	 *      del popup de evento  CAMBIAR NOMBREEEE
	 * 
	 * 
	 * 
	 * 
	 */
	const float DISTANCETHRESHOLD=0.2f;

	[HideInInspector]
	public bool walkingToTarget = false;
	[HideInInspector]
	public Vector3 walkingInitialVector;

	//[HideInInspector]
	public float targetX = 0.0f;
	//[HideInInspector]
	public float targetZ = 0.0f;

	//[HideInInspector]
	public GameObject cameraPivotY;

	public GameObject spriteHolder;

	private Material matRef;
	private SpriteRenderer rendRef;
	public int showingFrame;




	[HideInInspector]
	public string namer;

	[HideInInspector]
	public int nEvents;

	public float nearRadius = 2.0f;
	public float speed = 6.0f;

	[HideInInspector]
	//public bool pickable;
	//public string WOname;
	const int maxEvents = 20;

	public PlayerScript player;

	public bool canSpeak;
	//public ConversationNode conversation;

	/*Lis of textures to use
	 */
	//public Texture[] images;
	public float IdleAnimationSpeed = 1.0f;
	public Texture[] Idle;
	public Sprite[] IdleSprite;
	public Texture[] WalkingFront;
	public Sprite[] WalkingFrontSprite;
	public Texture[] WalkingFrontLeft;
	public Sprite[] WalkingFrontLeftSprite;
	public Texture[] WalkingLeft;
	public Sprite[] WalkingLeftSprite;
	public Texture[] WalkingLeftBack;
	public Sprite[] WalkingLeftBackSprite;
	public Texture[] WalkingBack;
	public Sprite[] WalkingBackSprite;
	public Texture[] WalkingBackRight;
	public Sprite[] WalkingBackRightSprite;
	public Texture[] WalkingRight;
	public Sprite[] WalkingRightSprite;
	public Texture[] WalkingRightFront;
	public Sprite[] WalkingRightFrontSprite;

	public WisdominiObject[] NextProgram;

	public Texture leftMiniatureN;
	public Texture leftMiniatureB;
	public Texture centerMiniatureN;
	public Texture centerMiniatureB;

	public Texture neutralMiniature;
	public Texture blinkMiniature;
	public Texture sadMiniature;
	public Texture worriedMiniature;

	public Sprite[] images;

	public Sprite[] sayImages;

	public float WalkingAnimationSpeed = 1.0f;

	public Texture[] otherImages;

	[HideInInspector]
	public int[] editorSelected;

	[HideInInspector]
	public List<int> selection;

	[HideInInspector]
	//public List<List<int>> action;
	public List<ListInt1D> action;

	public float animationSpeed;
	public float animationDelay;
	public float frameTime;

	Color dialogueColor;
	Texture miniature;

	public float walkingSpeed;

	public Texture[] customAnimation1;
	public Sprite[] customAnimation1Sprite;
	public float customAnimation1Speed;
	public Texture[] customAnimation2;
	public Sprite[] customAnimation2Sprite;
	public float customAnimation2Speed;
	public Texture[] customAnimation3;
	public Sprite[] customAnimation3Sprite;
	public float customAnimation3Speed;


	[HideInInspector]
	public CharacterWalkingState walkingState;


	[HideInInspector]
	public CharacterDirection walkingDirection;

	/* list of gameobjects to message 
	 */
	public GameObject[] messageTargets;

	public AudioClip[] soundEffects;

	public AudioClip[] music;

	public StringBank[] stringBank;

	Rigidbody r;

	float moveVertical, moveHorizontal;

	[HideInInspector]
	public bool interrupted = false;
	[HideInInspector]
	public bool isRoot;

	public string interactIconName;

	const float FollowPathRecalculationInterval = 0.1f;
	float followElapsedTime;
	public float followDistance = 4.0f;
	const float SpeedThreshold = 0.05f;

	const float directionJitterTime = 0.05f;
	float voteDirectionElapsedTime;
	CharacterDirection voteDirection;
	CharacterDirection currentDirection;

	public bool autopilotAbsolute = true;
	Vector3 spawnPosition; 
	/*
	 * 
	 * if autopilot is not absolute,
	 * autopilotTo coordinates will be spawnPosition + autopilotDestination
	 * 
	 * if autopilot is absolute
	 * autopilotTo coordinates will be autopilotDestination only
	 * 
	 */

	int state3 = 0; // slot 3, for direction voting
					// 0 : idling
					// 1 : new direction voted
					// 2 : setting new direction

	float previousWalkSpeed;

	MasterControllerScript mcRef;

	public override void effect() 
	{
		if (interactEnabled == false)
			return;

		//if (interactIconName.Equals ("Mouth") || (interactIconName.Equals("Eye"))) {

			interrupted = true;
			int prg = programIndexFromEventName ("onSpeak");
			if (prg != -1) {
				startProgram (prg);
			}
		//}
	}

	public override string interactIcon() 
	{
		return interactIconName;
	}

	void Awake()
	{
		/*if (spriteHolder != null) {
			if (images.Length > 0) {
				rendRef = spriteHolder.GetComponent<Renderer> ();
				rendRef.material.mainTexture = images [0];
			}
		}
		*/
		walkingState = CharacterWalkingState.standing;
		walkingDirection = CharacterDirection.front;

		images = IdleSprite;
	}

	void Reset() 
	{
		Awake ();
	}

	void OnDrawGizmos() 
	{		
		if (spriteHolder != null) 
		{
			// CAMBIAR A SPRITE
			if (IdleSprite.Length > 0) 
			{
				SpriteRenderer myRend;
				myRend = spriteHolder.GetComponent<SpriteRenderer> ();
				myRend.sprite = IdleSprite [0];
				//myRend.sharedMaterial.mainTexture = Idle [0];
				//myRend.material.mainTexture = Walking [0].texture; 
				//spriteHolder.GetComponent<Mesh> ().uv = Walking [0].uv;//Idle [0];
			}
		}
	}

	/* Manage collisions/overlaps */

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag != "Player")
			return;
		if (other.name == "Main Camera") {
			return;
		}

		if (!this.name.StartsWith ("Duende")) {
			//Debug.Log ("<color=purple>other.name" + " entered " + this.name + "</color>");
		}

		if (!interactEnabled)
			return;

		int j = programIndexFromEventName ("onOverlap");
		if (j != -1) {
			startProgram (j);
		}
	}

	new void Start () 
	{

		base.Start ();

		RemainingFramesToChangeDirection = NFramesNeededToChangeDirection;
		RemainingFramesToReachEndpoint = NFramesNeededToChangeDirection;

		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		cameraPivotY = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ().pivotY;

		spawnPosition = this.transform.position;
		// autopiloting in relative mode is
		// relative to spawnPosition, so keep it

		if(spriteHolder != null) {

			rendRef = spriteHolder.GetComponent<SpriteRenderer> ();

			matRef = spriteHolder.GetComponent<SpriteRenderer> ().material;

		}
		
		if (IdleAnimationSpeed > 0.0f) 
		{
			animationDelay = 1.0f / IdleAnimationSpeed;
		} else {
			animationDelay = 0.0f;
		}

		showingFrame = 0;
		frameTime = 0.0f;
		followElapsedTime = 0.0f;

		followState = FollowState.noFollow;

		player = GameObject.Find ("Player").GetComponent<PlayerScript> ();

		r = GetComponent<Rigidbody> ();

		if (markedAsStartFollowing)
			startFollowingPlayer ();
		
	}

	public void initialize() 
	{
		//pickable = false;
		nEvents = 0;
		editorSelected = new int[maxEvents];
		for (int i = 0; i < maxEvents; ++i) 
		{
			editorSelected[i] = 0;
		}

		selection = new List<int> ();
		action = new List<ListInt1D> ();
		//tex = null;
		reset ();
	}

	public void setWalkingState(CharacterWalkingState s) {

		walkingState = s;
		switch (s) {
		case CharacterWalkingState.standing:
			if (IdleAnimationSpeed > 0.0f)
				animationDelay = 1.0f / IdleAnimationSpeed;
			else
				animationDelay = 0.0f;
			break;
		case CharacterWalkingState.walking:
			state3 = 0;
			if (animationSpeed > 0.0f)
				animationDelay = 1.0f / animationSpeed;
			else
				animationDelay = 0.0f;
			break;
		}
	}

	public void voteForWalkingDirection(CharacterDirection dir) 
	{
		if (state3 == 0) {
			voteDirectionElapsedTime = 0.0f;
			voteDirection = dir;
		}

		if(dir != voteDirection) { state3 = 0; return; } // fall back to idle

		state3 = 1;
	}

	public void setWalkingDirection(int orientation) 
	{		
		switch (orientation) 
		{
		case 0: // front
			setWalkingDirection (CharacterDirection.front);
			break;
		case 1: // front left
			setWalkingDirection (CharacterDirection.frontLeft);
			break;
		case 2: // left
			setWalkingDirection (CharacterDirection.left);
			break;
		case 3: // back left
			setWalkingDirection (CharacterDirection.backLeft);
			break;
		case 4: // back
			setWalkingDirection (CharacterDirection.back);
			break;
		case 5: // back right
			setWalkingDirection (CharacterDirection.backRight);
			break;
		case 6: // right
			setWalkingDirection (CharacterDirection.right);
			break;
		case 7: // front right
			setWalkingDirection (CharacterDirection.frontRight);
			break;
		}
	}

	public void setWalkingDirection(CharacterDirection d) 
	{        

		if (RemainingFramesToChangeDirection == NFramesNeededToChangeDirection) {
			voteDirection = d;
			--RemainingFramesToChangeDirection;
			return;
		} else {
			if ((RemainingFramesToChangeDirection > 0) && (d != voteDirection)) { // reset
				RemainingFramesToChangeDirection = NFramesNeededToChangeDirection; 
				return;
			}
			--RemainingFramesToChangeDirection;
		}

		currentDirection = d;
		walkingDirection = d;

		switch (d) 
		{
		case CharacterDirection.back:
			if (WalkingBackSprite.Length != 0)
            {
				images = WalkingBackSprite;               
		    }
            else
            {
				images = WalkingFrontSprite;
            }
			//if(this.name.Equals("DuendeCoco1")) Debug.Log (" <<< back ");
			showingFrame = showingFrame % images.Length;
	    	break;

		case CharacterDirection.front:
			if ( WalkingFrontSprite.Length != 0) 
			{
				images = WalkingFrontSprite;
                showingFrame = showingFrame % images.Length;
	    	}
			//if(this.name.Equals("DuendeCoco1")) Debug.Log (" <<< front ");
		    break;

		case CharacterDirection.left:
			if (WalkingLeftSprite.Length != 0) 
			{
				images = WalkingLeftSprite;               
	    	}
            else
            {
				images = WalkingFrontSprite;
            }
			showingFrame = showingFrame % images.Length;
			//if(this.name.Equals("DuendeCoco1")) Debug.Log (" <<< left ");
            break;

		case CharacterDirection.right:
			if (WalkingRightSprite.Length != 0) 
			{
				images = WalkingRightSprite;     
		    }
            else
            {
				images = WalkingFrontSprite;
            }
			showingFrame = showingFrame % images.Length;
			//if(this.name.Equals("DuendeCoco1")) Debug.Log (" <<< right ");
            break;

		case CharacterDirection.backLeft:
			if (WalkingLeftBackSprite.Length != 0) 
			{
				images = WalkingLeftBackSprite;
			}
            else
            {
				images = WalkingBackSprite;
            }
			showingFrame = showingFrame % images.Length;
			//if(this.name.Equals("DuendeCoco1")) Debug.Log (" <<< left back ");
            break;

		case CharacterDirection.backRight:
			if (WalkingBackRightSprite.Length != 0)
			{
				images = WalkingBackRightSprite;
		    }
            else
            {
				images = WalkingBackSprite;
            }
			showingFrame = showingFrame % images.Length;
			//if(this.name.Equals("DuendeCoco1")) Debug.Log (" <<< right back ");
            break;

		case CharacterDirection.frontLeft:
			if (WalkingFrontLeftSprite.Length != 0) 
			{
				images = WalkingFrontLeftSprite;
		    }
            else
            {
				images = WalkingFrontSprite;
            }
			showingFrame = showingFrame % images.Length;
			//if(this.name.Equals("DuendeCoco1")) Debug.Log (" <<< left front ");
            break;

		case CharacterDirection.frontRight:
			if (WalkingRightFrontSprite.Length != 0) 
			{
				images = WalkingRightFrontSprite;               
		    }
            else
            {
				images = WalkingFrontSprite;
            }
			showingFrame = showingFrame % images.Length;
			//if(this.name.Equals("DuendeCoco1")) Debug.Log (" <<< right front ");
            break;
		}

		RemainingFramesToChangeDirection = NFramesNeededToChangeDirection;

	}		

	public void refreshPlayerFollowDestination()
	{
		if (spriteHolder == null)
			return;
		walkingSpeed = player.modulusSpeed;
		if (walkingSpeed < SpeedThreshold)
			walkingSpeed = player.speed;
		Vector3 selfToPlayerVector = player.transform.position - this.transform.position;
		distanceToPlayer = selfToPlayerVector.magnitude;
		if (selfToPlayerVector.magnitude <= (followDistance)) {
			walkingState = CharacterWalkingState.standing;
			followingState = 0;
			showingFrame = 0;
			return;
		}
		followingState = 1;
		if (selfToPlayerVector.magnitude >= (followDistance * 4)) { // salto bruto
			//followingState = 2;
			//followState = FollowState.rendervous;
			Vector3 compensation = -selfToPlayerVector;
			compensation.y = 0;
			compensation.Normalize ();
			compensation *= followDistance;
			this.transform.position = player.transform.position + compensation;
		}


		Vector3 unitSelfToPlayerVector = selfToPlayerVector.normalized;
		// target will be followDistance units before reaching player
		Vector3 destination = player.transform.position - (followDistance) * unitSelfToPlayerVector;
		autopilotAbsolute = true;
		autopilotTo (destination.x, destination.z);
	}

	public void setAutopilotAbsolute(bool a) 
	{
		autopilotAbsolute = a;
	}

	public void refreshSpawnPosition() {
		spawnPosition = this.transform.position;
	}

	public void autopilotTo(float x, float z) 
	{
		if (autopilotAbsolute) {
			targetX = x;
			targetZ = z;
		} else {
			targetX = spawnPosition.x + x*relativeCoordinatesScale;
			targetZ = spawnPosition.z + z*relativeCoordinatesScale;
		}
		walkingToTarget = true;
		walkingInitialVector = new Vector3 (targetX - this.transform.position.x, 0, targetZ - this.transform.position.z);
	}

	public void autopilotToTrueRelative(float x, float z) 
	{
		targetX = x + this.transform.position.x;
		targetZ = z + this.transform.position.z;

		walkingToTarget = true;
		walkingInitialVector = new Vector3 (targetX - this.transform.position.x, 0, targetZ - this.transform.position.z);
	}

	public void showFrame(int f) {
//		if (this.name.Equals ("DuendeCoco1"))
//			Debug.Log ("ShowFrame called: " + f); 
		if (rendRef != null) {
			if (f < images.Length) {
				rendRef.sprite = images [f];
			}
		}
	}

	new void Update () 
	{
		moveVertical = 0.0f;
		moveHorizontal = 0.0f;

		base.Update ();

		if (interrupted) 
			 return;

		if (spriteHolder == null)
			return;

		/* Refresh sprite face
		 */
		if (customAnimationPlaying == -1) 
		{
			if (walkingState != CharacterWalkingState.standing) {
				elapsedTime += Time.deltaTime;

				if (animationDelay == 0.0f) {
					showingFrame = 0;
				} else if (elapsedTime > animationDelay) {
					elapsedTime = 0.0f;
					if (images.Length > 0) {
						showingFrame = (showingFrame + 1) % images.Length;
					} else
						showingFrame = 0;

					if ((showingFrame == 0) && (images.Length > 1))
						++showingFrame;				
				}
				if ((matRef != null) && images.Length > 0) {
					if (images.Length > 0) {
						//rendRef.sprite = images [showingFrame];
						showFrame (showingFrame);
					}
				}
			} else {
				showFrame (0);
				//if(this.name.Equals("DuendeCoco1")) Debug.Log ("Pilleti!");
			}

		} else {
			elapsedTime += Time.deltaTime;

			switch (customAnimationPlaying) 
			{
			case 1:
				if (elapsedTime > (1.0f / customAnimation1Speed)) 
				{
					elapsedTime = 0.0f;
					showingFrame = (showingFrame + 1) % customAnimation1Sprite.Length;
					if (matRef != null) {
						rendRef.sprite = customAnimation1Sprite [showingFrame];
					}
				}
				break;

			case 2:
				if (elapsedTime > (1.0f / customAnimation2Speed)) {
					elapsedTime = 0.0f;
					showingFrame = (showingFrame + 1) % customAnimation2Sprite.Length;
					if (matRef != null) {
						rendRef.sprite = customAnimation2Sprite [showingFrame];
					}
				}
				break;

			case 3:
				if (elapsedTime > (1.0f / customAnimation3Speed)) {
					elapsedTime = 0.0f;
					showingFrame = (showingFrame + 1) % customAnimation3Sprite.Length;
					if (matRef != null) {
						rendRef.sprite = customAnimation3Sprite [showingFrame];
					}
				}
				break;					
			}
		}

		moveVertical = 0.0f;
		moveHorizontal = 0.0f;
		if (walkingState == CharacterWalkingState.walking) 
		{ // move the character around

		} 

		if (walkingToTarget) 
		{
			Vector3 currentPos = new Vector3 (this.transform.position.x, 0, this.transform.position.z);
			Vector3 targetPos = new Vector3 (targetX, 0, targetZ);

			direction = targetPos - currentPos;

			/*if (direction.magnitude > 1.0f) {
				direction.Normalize ();
			}*/

			float dot = Vector3.Dot (walkingInitialVector, direction);

			if (dot < 0.0f) {
				//Vector3 newPos = new Vector3 (targetX, this.transform.position.y, targetZ);
				//this.transform.position = newPos;
				RemainingFramesToReachEndpoint = NFramesNeededToChangeDirection;
				state3 = 6;
				//setWalkingState (CharacterWalkingState.standing);
				walkingToTarget = false;
				moveHorizontal = 0.0f;
				moveVertical = 0.0f;
				notifyFinishAction ();

			} else {
				setWalkingDirection (Direction4FromVector (new Vector2(direction.x, direction.z)));
				setWalkingState (CharacterWalkingState.walking);
				moveHorizontal = walkingInitialVector.normalized.x;
				moveVertical = walkingInitialVector.normalized.z;
			}
		}

		// update slot 3

		if (state3 == 0) 
		{
			// do nothing
		}

		if (state3 == 1) 
		{
			voteDirectionElapsedTime += Time.deltaTime;
			if (voteDirectionElapsedTime > directionJitterTime) 
			{
				state3 = 0;
				voteDirectionElapsedTime = 0.0f;
				setWalkingDirection (voteDirection);
			}
		}
		if (state3 == 6) {
			--RemainingFramesToReachEndpoint;
			if (RemainingFramesToReachEndpoint <= 0) {
				setWalkingState (CharacterWalkingState.standing);

				state3 = 0;

			}
		}
		// end of slot 3


	}

	/* Add camera Y rotation offset */
	public CharacterDirection Direction4FromVector(Vector2 v) { // WARNING duplicate in Player
		/*
		 * 
		 * Solution: create WalkableCharacter class and let Player and CharacterGenerator inherit from it
		 * 
		 */
		float width;//, depth;
		const float firstOctantCos = 0.92f;
		const float thirdOctantCos = 0.38f;

		if (cameraPivotY != null) {
			v = Quaternion.AngleAxis (cameraPivotY.transform.rotation.eulerAngles.y, Vector3.forward) * v; 
			// es Vector3.forward (0, 0, 1) porque las coordenadas de V.x === World.x pero V.y === World.z
		}

		v.Normalize ();

		width = Mathf.Abs (v.x);
		//depth = Mathf.Abs (v.y);

		if (width > firstOctantCos) { // left or right

			if (v.x > 0.0)
				return CharacterDirection.right;
			else
				return CharacterDirection.left;

		} else if (width > thirdOctantCos) { // diagonals

			if (v.y > 0.0f && v.x > 0.0f)
				return CharacterDirection.backRight;
			else if (v.y > 0.0f && v.x < 0.0f)
				return CharacterDirection.backLeft;
			else if (v.y < 0.0f && v.x > 0.0f)
				return CharacterDirection.frontRight;
			else
				return CharacterDirection.frontLeft;


		} else if (width <= thirdOctantCos) { // up or down

			if (v.y > 0.0f)
				return CharacterDirection.back;
			else
				return CharacterDirection.front;
		}

		return CharacterDirection.front;
	}
		

	public void FixedUpdate() 
	{
		if (spriteHolder == null)
			return;
		if (cameraPivotY != null) {
			pivotYRotation = cameraPivotY.transform.rotation.eulerAngles.y;
		}

		if (followState == FollowState.follow) 
		{
			followElapsedTime += Time.deltaTime; // we could also use a coroutine
			if (followElapsedTime > FollowPathRecalculationInterval) {
				followElapsedTime = 0.0f;
				refreshPlayerFollowDestination ();
			}
		} 

		if (r != null) 
		{		
			Vector3 movement = new Vector3 (moveHorizontal * walkingSpeed, r.velocity.y, moveVertical * walkingSpeed); // WARNING magic number
			r.velocity = movement;
		}
	}

	public void addEvent()
	{
		//++nEvents;
		selection.Add (0);
		action.Add (new ListInt1D ());
	}

	public void removeEvent(int index) 
	{
		if (selection.Count > 0) 
		{
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

	public void setTypeOfEvent(int index, int type) 
	{
		if (index < selection.Count ) 
		{
			selection[index] = type;
		}
	}

	public void addAction(int eIndex) 
	{
		action [eIndex].theList.Add (0);
	}

	public void setAction(int eIndex, int aIndex, int act) 
	{
		if((eIndex < action.Count) && (aIndex < action[eIndex].theList.Count)) 
		{
			action[eIndex].theList[aIndex] = act;
		}
	}

	public void removeAction(int eIndex, int aIndex) 
	{
		action [eIndex].theList.RemoveAt (aIndex);
	}

	public void insertAction(int eIndex, int aIndex) 
	{
		action [eIndex].theList.Insert (aIndex, 0);
	}

	int indexFromString(string [] list, string item) 
	{
		for(int i = 0; i<list.Length; ++i) 
		{
			if (list [i].Equals (item))
				return i;
		}
		return -1;
	}

	public void parseProgram(int eIndex, string prg, string [] actions) 
	{
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

			if((instructionColor != null) && (eIndex < instructionColor.Count)) 
			{
				instructionColor [eIndex].theList.Add (-1);
			}

			addAction (eIndex);
			setAction (eIndex, i, indexFromString (actions, instrParams [0]));
		}
	}

	public int nActions(int eIndex) 
	{
		if(eIndex < action.Count)
		{
			return action[eIndex].theList.Count;
		}
		return 0;
	}

	public int getTypeOfAction(int eIndex, int aIndex) 
	{
		if ((eIndex < action.Count) && (aIndex < action [eIndex].theList.Count))
		{
			return action [eIndex].theList[aIndex];
		} else
			return 0;
	}

	public void setTypeOfAction(int eIndex, int aIndex, int type) 
	{
		if ((eIndex < action.Count) && (aIndex < action [eIndex].theList.Count)) 
		{
			action [eIndex].theList[aIndex] = type;
		} else
			return;
	}

	public void _wm_warpTo(float x, float y, float z) 
	{
		warpTo(new Vector3(x, y, z));
	}

	public void warpTo(Vector3 newPos) 
	{
		this.transform.position = newPos;
	}

	public void warpToRelativeToPlayer(Vector3 newPos)
	{
		if (player == null)
			player = GameObject.Find ("Player").GetComponent<PlayerScript> ();
		this.transform.position = player.transform.position + newPos;
	}

	public void followPlayer() 
	{
		startFollowingPlayer ();
	}

	public void startFollowingPlayer()
	{
		autopilotAbsolute = true;
		followElapsedTime = 0.0f;
		followState = FollowState.follow;
		previousWalkSpeed = walkingSpeed;
		walkingSpeed = player.modulusSpeed;
		if (walkingSpeed == 0.0f)
			walkingSpeed = player.speed;
		if (mcRef == null)
			mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		mcRef.getStorage ().storeStringValue ("FollowingChar", this.name);
		mcRef.getStorage ().storeFloatValue ("FollowerRelativeCoordsX", 1.0f);
		mcRef.getStorage ().storeFloatValue ("FollowerRelativeCoordsY", 0.0f);
		mcRef.getStorage ().storeFloatValue ("FollowerRelativeCoordsZ", -1.0f);
	}

	public void stopFollowingPlayer() 
	{
		followState = FollowState.noFollow;
		walkingSpeed = previousWalkSpeed;

		mcRef.getStorage ().storeStringValue ("FollowingChar", "");
	}

	/* WisdominiScript wrappers */

	public void _wm_followPlayer() 
	{
		followPlayer ();
	}

	public void _wa_autoPilotTo(WisdominiObject waiter, float x, float z) {
		setAutopilotAbsolute (true);
		waiter.isWaitingForActionToComplete = true;
		programNotification = -1;
		waitingRef = waiter;
		autopilotTo (x, z);
	}

	public void _wa_autoPilotToRelative(WisdominiObject waiter, float x, float z) {
		
		waiter.isWaitingForActionToComplete = true;
		programNotification = -1;
		waitingRef = waiter;
		autopilotToTrueRelative (x, z);
	}

	public void _wm_stopFollowingPlayer() 
	{
		stopFollowingPlayer();
	}

	public Sprite getTex() 
	{
		return images[0];
	}
}
