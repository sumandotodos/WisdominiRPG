using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public enum Interaction { None, Something };
public enum PlayerDirection { front, left, right, back, frontright, frontleft, backright, backleft };
public enum PlayerState { walking, idling, meditating, talking, ascending, ascending2, spinning, spinning2, spinning3,
	spawningShadow1, spawningShadow2, spawningShadow3, spawningShadow4, spawningShadow5, enteringMirror,
	dematerialized, materializing, seekingMirrorPoint1, seekingMirrorPoint2 };

public enum PlayerWalkType { onGround, floating };

public class PlayerScript : WisdominiObject {

	/* references */

	//public Text interactext;
	//public Text enterexittext;

	public float DEJAMEVERESADISTANCIA;
	//public SoftCameraPivot cameraPivotY;
	public CameraManager cam;
	public MirrorObject mirrorObject;
	GameObject masterControllerRef;
	DataStorage ds;
	MasterControllerScript mcsRef;
	public bool isLevitating;
	public interconNetwork iNetRef;
	public AudioClip matSound;
	public GameObject auraPrefab;
	public GameObject spriteQuadRef;
	public GameObject spriteQuadBehindRef;
	//public GameObject spriteMaterialize;
	public GameObject interactionQuadRef;
	public LevelControllerScript levelRef;
	public MirroredPlayer mirroredPlayer;
	public Camera camRef;
	//[HideInInspector]
	public UIInventoryV2 inventory;
	public bool canOpenInventory = true;
	public Sprite[] IdleSprite;
	public float IdleAnimationSpeed = 1.0f;
	public Sprite[] WalkingFront;
	public Sprite[] WalkingFrontLeft;
	public Sprite[] WalkingLeft;
	public Sprite[] WalkingLeftBack;
	public Sprite[] WalkingBack;
	public Sprite[] WalkingBackRight;
	public Sprite[] WalkingRight;
	public Sprite[] WalkingRightFront;
	public Sprite[] floatingFront;
	public Sprite[] floatingFrontLeft;
	public Sprite[] floatingLeft;
	public Sprite[] floatingBackLeft;
	public Sprite[] floatingBack;
	public Sprite[] floatingBackRight;
	public Sprite[] floatingRight;
	public Sprite[] floatingFrontRight;
	public Sprite[] Meditating;
	public Sprite[] Ascending;
	public int[] footstepFrame;
	public float ascendingAnimationSpeed;
	public Sprite[] Descending;
	public float descendingAnimationSpeed;
	public Texture neutralMiniature;
	public Texture happyMiniature;
	public Texture sadMiniature;
	public Texture worriedMiniature;
	public HUDController hud;
	Sprite[] images;
	[HideInInspector]
	public Rigidbody r;
	[HideInInspector]
	public UnityEngine.AI.NavMeshAgent agent;
	public StringBank recoveryStrings;
	public Sprite HeroReflection;
	bool usingHeroReflection = false;
	Vector3 externalForce;
    float remainingBlinkTime = 0.0f;
    bool BlinkShow = true;
    int remainingBlinks = 0;

	int mustSnapToPlayerStart = 4;

	[HideInInspector]
	public float modulusSpeed;

	public float equilibriumHeight = 3.0f;
	public float floatingAcceleration = 2.0f;
	public float equilibriumStrength = 2.0f;
	public float height;
	public float heightSpeed;
	public float maxHeightSpeed = 1.0f;

	Vector3 autopilotDestination;
	Vector3 autopilotOrigin;
	bool autopilot = false;

	/* properties */

	int dematDelayFrames = 0;
	float elapsedTime; // elapsedTime for slot 0 (state)
	float blockedElapsedTime; // elapsed for slot 1 (state2)
	float blinkRemainingTime;
	bool blink = false;
	int frame;
	int meditationFrame;
	PlayerDirection direction;
	public PlayerWalkType walkType = PlayerWalkType.onGround;
	public float WalkingAnimationSpeed = 1.0f;
	public PlayerState state; // idling walking or materializing
	public PlayerState state2; // rest
	//bool ascendingUp;
	Vector3 ascendStartPos;
	Vector3 enterMirrorStartPosition;
	Vector3 enterMirrorPosition;
	float ascendPos;
	float ascendSpeed;
	float ascendAccel;
	float angle;
	float lerp;
	float angleSpeed;
	float angleAccel;
	float lastHorizontal, lastVertical;
	Interactor backupInteractee;
	bool grabbedFrame = false;
	bool mirrorActivationDelaying = false;
	public float spawningShadowElapsedTime;

	/* constants */

	const float maxAngleSpeed = 3400.0f;
	const float angleThreshold = 5000.0f;
	const float maxAngleAccel = 2000.0f;
	const float shadowBlinkAnimationSpeed = 30.0f;
	const float dematerializationDelay = 0.75f;
	const float materializationDuration = 4.0f;
	const float walkMultiplicator = 10.0f;
	const float mirrorActivationDelay = 6.0f;
	const float blinkingTime = 5.0f;
	const float noSpawningShadowTime = 7.5f;

	/*^comienzo de borrame
	 * */
	bool msgHecho = false;
	float myTimer = 0.0f;
	/* fin de borrame */

	//GameObject interactee;
	Interactor interactee;

	Animator sprAnimator;
	SpriteRenderer playerRendRef;
	Material playerMatRef;
	SpriteRenderer otherPlayerRendRef;
	Material otherPlayerMat;

	bool canInteract = false;

	bool started = false;

	public bool blocked; // true if the Player is not allowed to receive movement input from Human

	public float speed; // speed of movement
	const float PLAYERSPEED = 8.0f;

    public Sprite HandIcon;
    public Sprite MouthIcon;
    public Sprite EyeIcon;

	//float opacity; 


	Interaction interaction;
	//bool callbackAction;
	[HideInInspector]
	public GameObject currentHUD;
	[HideInInspector]
	public GameObject[] disabledHUD;

	float contador;
	public GameObject targetLookAt;
	bool materializated = false;

	public void _wm_levitate()
	{
		walkType = PlayerWalkType.floating;
	}

	public void _wm_unlevitate() 
	{
		walkType = PlayerWalkType.onGround;
	}
//	bool awake = false;
	bool HUDSelected = false;

    bool moving = false;

    float meditatingRemaining;

    public bool IsMoving()
    {
        return moving;
    }

    new void Start () 
	{
		WalkingMode ();

		SelectHUD ();

		GameObject inetGO = GameObject.Find ("RedIntercon");
		if (inetGO != null) {
			iNetRef = inetGO.GetComponent<interconNetwork> ();
		}

		speed = PLAYERSPEED;

		GameObject mgo = GameObject.Find ("MirrorObject");
		if(mgo != null) {
			mirrorObject = mgo.GetComponent<MirrorObject> ();
		}

		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		if (agent != null) {
			agent.enabled = false;
			agent.updateRotation = false;
		}

		r = GetComponent<Rigidbody> ();
		unblockControls ();

		cam = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		masterControllerRef = GameObject.Find ("MasterController");
		mcsRef = masterControllerRef.GetComponent<MasterControllerScript> ();
		rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();
		ds = mcsRef.getStorage ();

		// check music
		if (!ds.retrieveBoolValue ("Is" + levelRef.locationName + "MusicPlaying")) {
			mcsRef.selectMixer (0);
			string lvl = levelRef.locationName.Substring (5, 1);
			//mcsRef.playMusic (int.Parse(lvl));
			mcsRef.setVolume (1.0f);
			ds.storeBoolValue ("Is" + levelRef.locationName + "MusicPlaying", true);
		}

		spawningShadowElapsedTime = noSpawningShadowTime;

		state2 = PlayerState.idling;

		interactionQuadRef.GetComponent<SpriteRenderer> ().enabled = false;

		interaction = Interaction.None;
		interactee = null;
		backupInteractee = null;

		direction = PlayerDirection.front;

		playerRendRef = spriteQuadRef.GetComponent<SpriteRenderer>();
		playerMatRef = playerRendRef.material;
		otherPlayerRendRef = spriteQuadBehindRef.GetComponent<SpriteRenderer> ();
		otherPlayerMat = otherPlayerRendRef.material;

		ascendPos = 0.0f;
		ascendSpeed = 0.0f;
		ascendAccel = 45.0f;

		elapsedTime = 0.0f;

		if (ds.retrieveBoolValue ("PlayerMustMaterialize")) 
		{
			state = PlayerState.dematerialized;
			playerMatRef.color = new Color (1, 1, 1, 0);

			blocked = true;
		} else {
			state = PlayerState.idling;
			//opacity = 1.0f;
			playerMatRef.color = new Color (1, 1, 1, 1);
		}

		float x, y, z;
		x = ds.retrieveFloatValue ("Coords" + levelRef.locationName + "X");
		y = ds.retrieveFloatValue ("Coords" + levelRef.locationName + "Y");
		z = ds.retrieveFloatValue ("Coords" + levelRef.locationName + "Z");
		Debug.Log ("<color=red>Player coords retrieved: (" + x + "," + y + "," + z + ") on "+levelRef.locationName+" </color>");
		int orientation;
		orientation = ds.retrieveIntValue ("Orientation");
		Debug.Log ("<color=green>Player orient. stored: (" + orientation + ") on "+levelRef.locationName+"</color>");
		setOrientation (orientation);
		if ((x == 0) && (y == 0) && (z == 0)) 
		{
			if (GameObject.Find ("PlayerStart")) 
			{
				Vector3 newPos = GameObject.Find ("PlayerStart").transform.position;
				this.transform.position = newPos;
			}
			mustSnapToPlayerStart = 60;
		}
		if ((x != 0.0f) && (y != 0.0f) && (z != 0.0f)) {
			Vector3 newPos = new Vector3 (x, y, z);
			this.transform.position = newPos;
			mustSnapToPlayerStart = 0;
		}
		levelRef.loadPhysicalCameraPosition ();
		cam.Initialize ();
		string nameOfFollowingCharacter = ds.retrieveStringValue ("FollowingChar");
				if (!nameOfFollowingCharacter.Equals ("")) {
					GameObject charaGO = GameObject.Find (nameOfFollowingCharacter);
					if (charaGO != null) {
						float xc = ds.retrieveFloatValue ("FollowerRelativeCoordsX");
						float xy = ds.retrieveFloatValue ("FollowerRelativeCoordsY");
						float xz = ds.retrieveFloatValue ("FollowerRelativeCoordsZ");
						CharacterGenerator chara = charaGO.GetComponent<CharacterGenerator> ();
						if (chara != null) {
							chara.warpToRelativeToPlayer (new Vector3 (xc, xy, xz));
							chara.setAutopilotAbsolute (true);
							chara.preventAwake = true;
							chara.stopAllEvents ();
							chara.startFollowingPlayer ();
							chara.markedAsStartFollowing = true;
						}
					}
				}


		levelRef.Start ();
		string retAct = ds.retrieveStringValue ("ReturnFromActivity");
		if (retAct.Equals ("Well")) {
			int res = ds.retrieveIntValue ("ActivityResult");
			if (res > 0) {
				int red = ds.retrieveIntValue ("RedManaObtained");
				int blue = ds.retrieveIntValue ("BlueManaObtained");
				if (red > 0) {
					string msg = rosetta.retrieveString (recoveryStrings.getStringId (0)) + red;

					levelRef._wm_alert (msg);
				} else {
					//string msg = rosetta.retrieveString (recoveryStrings.getStringId (1)) + blue;
					string s1 = recoveryStrings.getStringId (1);
					string s2 = rosetta.retrieveString (s1);
					string s3 = s2 + blue;
					levelRef._wm_alert (s3);
				}
			} else {
				//levelRef._wm_alert (rosetta.retrieveString (recoveryStrings.getStringId (2)));
				string s1 = recoveryStrings.getStringId (2);
				string s2 = rosetta.retrieveString (s1);
				levelRef._wm_alert (s2);
			}
		} else if (retAct.Equals ("Main-1")) {
			string lvl = levelRef.locationName.Substring (0, 6);
			ds.storeIntValue (lvl + "SpawnedShadows", 0);
		}

		if (levelRef.floor == 0)
			ds.storeStringValue ("ReturnFromActivity", "Main");
		else if (levelRef.floor == -1)
			ds.storeStringValue ("ReturnFromActivity", "Main-1");
		else if (levelRef.floor == 1)
			ds.storeStringValue ("ReturnFromActivity", "Main+1");

		height = this.transform.position.y - 6;

		externalForce = Vector3.zero;


		mcsRef.saveGame (false); // save every time the player is spawned (change location)

		/*
		 * 
		 * Check reentrancy
		 * 
		 */

		string reentryCondition = ds.retrieveStringValue ("ReentryCondition");

		if (reentryCondition.Equals ("MirrorActivation")) {
			mirrorActivationDelaying = true;
			//camRef.GetComponent<CameraLookAt> ().enabled = false;
			cam._wm_disableLookAt ();

		} else
			mirrorActivationDelaying = false;


		if (reentryCondition.Equals ("Inventory")) {
			ds.storeStringValue ("ReentryCondition", ""); // consume condition
			int _num = ds.retrieveIntValue ("InventoryLevel");
			inventory.reenter (_num);
		}

        if(levelRef.retrieveBoolValue("Meditating"))
        {
            levelRef.storeBoolValue("Meditating", false);
            meditatingRemaining = 4.5f;
            playerRendRef.sprite = Meditating[0];
            otherPlayerRendRef.sprite = Meditating[0];
        }
        else
        {
            meditatingRemaining = 0.0f;
        }

        /*
		 * 
		 * Reset reentry condition
		 * 
		 */

        started = true;
	}

    public void Blink(float BlinkTime)
    {
        remainingBlinks = Mathf.FloorToInt(0.08f * BlinkTime);
        remainingBlinkTime = 0.08f;
        Debug.Log("Remaining blinks: " + remainingBlinks);
    }

    public void _wm_teleport(string TargetName)
    {
        GameObject Target = GameObject.Find(TargetName);
        if(Target != null)
        {
            this.transform.position = Target.transform.position;
        }
    }

    public void teleport(Vector3 Loc)
    {
        this.transform.position = Loc;
    }

    public void setExternalForce(Vector3 eForce)
	{
		externalForce = eForce;
	}

	public void _wm_setOrientation(int o) 
	{
		setOrientation(o);
	}

	public void setOrientation(int o) 
	{
		setOrientation (directionFromOrientationIndex (o));
	}

	public void setOrientation(PlayerDirection dir) 
	{
		switch (dir) {
		case PlayerDirection.left:
			direction = PlayerDirection.left;
			if (walkType == PlayerWalkType.onGround) {
				playerRendRef.sprite = WalkingLeft [frame];
				otherPlayerRendRef.sprite = WalkingLeft [frame];
				images = WalkingLeft;
			} else {
				playerRendRef.sprite = floatingLeft [frame];
				otherPlayerRendRef.sprite = floatingLeft [frame];
				images = floatingLeft;
			}
			break;
		case PlayerDirection.right:
			direction = PlayerDirection.right;
			if (walkType == PlayerWalkType.onGround) {
				playerRendRef.sprite = WalkingRight [frame];
				otherPlayerRendRef.sprite = WalkingRight [frame];
				images = WalkingRight;
			}
			else {
				playerRendRef.sprite = floatingRight [frame];
				otherPlayerRendRef.sprite = floatingRight [frame];
				images = floatingRight;
			}
			break;
		case PlayerDirection.front:
			direction = PlayerDirection.front;
			if (walkType == PlayerWalkType.onGround) {
				playerRendRef.sprite = WalkingFront [frame];
				otherPlayerRendRef.sprite = WalkingFront [frame];
				images = WalkingFront;
			}
			else {
				playerRendRef.sprite = floatingFront [frame];
				otherPlayerRendRef.sprite = floatingFront [frame];
				images = floatingFront;
			}
			break;
		case PlayerDirection.back:
			direction = PlayerDirection.back;
			if (walkType == PlayerWalkType.onGround) {
				playerRendRef.sprite = WalkingBack [frame];
				otherPlayerRendRef.sprite = WalkingBack [frame];
				images = WalkingBack;
			}
			else {
				playerRendRef.sprite = floatingBack [frame];
				otherPlayerRendRef.sprite = floatingBack [frame];
				images = floatingBack;
			}
			break;
		case PlayerDirection.frontleft:
			direction = PlayerDirection.frontleft;
			if (walkType == PlayerWalkType.onGround) {
				playerRendRef.sprite = WalkingFrontLeft [frame];
				otherPlayerRendRef.sprite = WalkingFrontLeft [frame];
				images = WalkingFrontLeft;
			}
			else {
				playerRendRef.sprite = floatingFrontLeft [frame];
				otherPlayerRendRef.sprite = floatingFrontLeft [frame];
				images = floatingFrontLeft;
			}
			break;
		case PlayerDirection.frontright:
			direction = PlayerDirection.frontright;
			if (walkType == PlayerWalkType.onGround) {
				playerRendRef.sprite = WalkingRightFront [frame];
				otherPlayerRendRef.sprite = WalkingRightFront [frame];
				images = WalkingRightFront;
			}
			else {
				playerRendRef.sprite = floatingFrontRight [frame];
				otherPlayerRendRef.sprite = floatingFrontRight[frame];
				images = floatingFrontRight;
			}
			break;
		case PlayerDirection.backleft: 
			direction = PlayerDirection.backleft;
			if (walkType == PlayerWalkType.onGround) {
				playerRendRef.sprite = WalkingLeftBack [frame];
				otherPlayerRendRef.sprite = WalkingLeftBack [frame];
				images = WalkingLeftBack;
			}
			else {
				playerRendRef.sprite = floatingBackLeft [frame];
				otherPlayerRendRef.sprite = floatingBackLeft [frame];
				images = floatingBackLeft;
			}
			break;
		case PlayerDirection.backright:
			direction = PlayerDirection.backright;
			if (walkType == PlayerWalkType.onGround) {
				playerRendRef.sprite = WalkingBackRight [frame];
				otherPlayerRendRef.sprite = WalkingBackRight [frame];
				images = WalkingBackRight;
			}
			else {
				playerRendRef.sprite = floatingBackRight [frame];
				otherPlayerRendRef.sprite = floatingBackRight [frame];
				images = floatingBackRight;
			}
			break;
		}
	}

	public int orientation() 
	{
		return orientationFromDirection (direction);
	}

	public PlayerDirection directionFromOrientationIndex(int o) 
	{
		switch (o) {

		case 0:
			return PlayerDirection.front;

		case 1:
			return PlayerDirection.frontleft;

		case 2:
			return PlayerDirection.left;

		case 3:
			return PlayerDirection.backleft;

		case 4:
			return PlayerDirection.back;

		case 5:
			return PlayerDirection.backright;

		case 6:
			return PlayerDirection.right;

		case 7:
			return PlayerDirection.frontright;

		default:
			return PlayerDirection.front;
		}
	}

	public int orientationFromDirection(PlayerDirection pd) 
	{
		switch (pd) {

		case PlayerDirection.front:
			return 0;

		case PlayerDirection.frontleft:
			return 1;

		case PlayerDirection.left:
			return 2;

		case PlayerDirection.backleft:
			return 3;

		case PlayerDirection.back:
			return 4;

		case PlayerDirection.backright:
			return 5;

		case PlayerDirection.right:
			return 6;

		case PlayerDirection.frontright:
			return 7;

		default:
			return 0;
		}
	}

	public void setMirrorHeroTexture(Sprite t) 
	{
		HeroReflection = t;
		usingHeroReflection = true;
	}

	public void unsetMirrorHeroTexture() 
	{
		usingHeroReflection = false;
	}

	public Sprite getMirroredTexture() 
	{
		if (usingHeroReflection) {
			return HeroReflection;
		}

		if ((state == PlayerState.walking) || (state == PlayerState.idling)) {
			if (direction == PlayerDirection.front) {
				return WalkingBack [frame];
			}
			if (direction == PlayerDirection.back) {
				return WalkingFront [frame];
			}
			if (direction == PlayerDirection.left) {
				return WalkingLeft [frame];
			}
			if (direction == PlayerDirection.right) {
				return WalkingRight [frame];
			}
			if (direction == PlayerDirection.frontleft) {
				return WalkingLeftBack [frame];
			}
			if (direction == PlayerDirection.backleft) {
				return WalkingFrontLeft [frame];
			}
			if (direction == PlayerDirection.frontright) {
				return WalkingBackRight [frame];
			}
			if (direction == PlayerDirection.backright) {
				return WalkingRightFront [frame];
			}
		}
		return WalkingFront [0];
	}

	new void Update () 
	{
        if(meditatingRemaining > 0.0f)
        {
            meditatingRemaining -= Time.deltaTime;
            return;
        }

        if (remainingBlinks > 0) // orthogonal blinking state
        {
            remainingBlinkTime -= Time.deltaTime;
            if(remainingBlinkTime <= 0.0f)
            {
                BlinkShow = !BlinkShow;
                remainingBlinkTime = 0.08f;
                --remainingBlinks;
                if(remainingBlinks == 0)
                {
                    BlinkShow = true;
                }
                spriteQuadRef.GetComponent<SpriteRenderer>().enabled = BlinkShow;
            }
        }

        if (mustSnapToPlayerStart > 0) {
			if (GameObject.Find ("PlayerStart")) 
			{
				Vector3 newPos = GameObject.Find ("PlayerStart").transform.position;
				this.transform.position = newPos;
			}
			mustSnapToPlayerStart--;
		}

		if (!started)
			return;
		if (state == PlayerState.idling) 

		{
			materializated = true;
		}

		if (state2 == PlayerState.enteringMirror) 
		{			
			lerp += Time.deltaTime;
			if (lerp > 1.0f) 
			{
				lerp = 1.0f;					
			}
			if (blink) {
				spriteQuadRef.GetComponent<SpriteRenderer> ().enabled = false;
				mirroredPlayer.gameObject.SetActive(false);
			} else {
				spriteQuadRef.GetComponent<SpriteRenderer> ().enabled = true;
				mirroredPlayer.gameObject.SetActive(true);
			}

			blink = !blink;
			Vector3 pos = Vector3.Lerp (enterMirrorStartPosition, enterMirrorPosition, lerp);
			this.transform.position = pos;
			this.transform.localScale = new Vector3 (1.0f - lerp, 1.0f - lerp, 1.0f - lerp);
			mirroredPlayer.transform.localScale = this.transform.localScale;
		}
		if (mirrorActivationDelaying) 
		{ // Reentry condition: showing mirror activation
			if (elapsedTime > mirrorActivationDelay) {
				ds.storeStringValue ("ReentryCondition", "");
				string returnLocation = ds.retrieveStringValue ("ReturnLocation");
				SceneManager.LoadScene (returnLocation);
			}
			return;
		}

		if (state2 == PlayerState.seekingMirrorPoint1) 
		{
			if((agent.remainingDistance < 0.25f) && agent.velocity.magnitude == 0) 
			{	
				Vector3 target = agent.destination + new Vector3 (0, 0, 0.25f);
				agent.destination = target;
				elapsedTime = 0.0f;
				directionFromVector (target, false);
				state2 = PlayerState.seekingMirrorPoint2;
				camRef.GetComponent<CameraUtils>().grab();

			}
		}

		if (state2 == PlayerState.seekingMirrorPoint2) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 9.0f) {
				SceneManager.LoadScene("Scenes/WordFight");
			}
		}

		if (state == PlayerState.dematerialized) 
		{
			++dematDelayFrames;

			blockedElapsedTime += Time.deltaTime;

			if (blockedElapsedTime > dematerializationDelay) {
				blockedElapsedTime = 0.0f;
				state = PlayerState.materializing;
				GameObject AuraGO = (GameObject)Instantiate (auraPrefab, 
					this.transform.position + new Vector3(0, 0.16f, -0.5f), 
					Quaternion.Euler (0, 0, 0));
				levelRef.playSound (matSound);
			} else
				return;
		}

		if (state == PlayerState.materializing) 
		{
			blockedElapsedTime += Time.deltaTime;
			float newOpacity = elapsedTime / materializationDuration;
			if (newOpacity > 1.0f)
				newOpacity = 1.0f;
			iTween.ColorUpdate(spriteQuadRef, new Color (1, 1, 1, 1), 4);

			if (blockedElapsedTime > materializationDuration) 
			{
				blockedElapsedTime = 0.0f;
				playerMatRef.color = new Color (1, 1, 1, 1);
				state = PlayerState.idling;
				blocked = false;
				ds.storeBoolValue ("PlayerMustMaterialize", false);
				materializated = true;
			}
		}

		if (Input.GetMouseButtonDown(0) && interaction != Interaction.None) 
		{
			contador = 1;
		}

		if (Input.GetMouseButton (0)) 
		{
			contador += Time.deltaTime;
		}

		if (!blocked && Input.GetMouseButtonUp (0)) 
		{
			if (!canInteract) {
				if (canOpenInventory) {
					if (contador < 0.5f) {
						Ray ray = camRef.ScreenPointToRay (Input.mousePosition);
						//Physics.RaycastAll (ray, out hit);

						RaycastHit[] hits;
						hits = Physics.RaycastAll (ray, 2000);
						for (int i = 0; i < hits.Length; i++) {
							RaycastHit hit = hits [i];

							if (hit.collider != null) {
								if (hit.collider.tag == "Player") {
									blockControls ();
									//inventory._wm_open ();
									inventory.open (1);
								}
							}
						}
					}
				}
			}
			contador = 0;
		}

		if (state2 == PlayerState.spinning) 
		{
			angle += angleSpeed * Time.deltaTime;
			angleSpeed += angleAccel * Time.deltaTime;
			if (angleSpeed > maxAngleSpeed)
				angleSpeed = maxAngleSpeed;
			directionFromAngle (angle);
			playerRendRef.sprite = images [0];
			otherPlayerRendRef.sprite = images [0];
			if (angle > angleThreshold) {
				state2 = PlayerState.spinning2;
				elapsedTime = 0.0f;
				frame = 0;
			}
		}

		if (state2 == PlayerState.spinning2) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > (1.0f / descendingAnimationSpeed)) {

				if (frame < Descending.Length) {
					frame++;
					elapsedTime = 0.0f;
				} else {
					//levelRef.fadeout (this);
					//this.isWaitingForActionToComplete = true;
					camRef.GetComponent<CameraUtils>().grab();
					mcsRef.getStorage ().storeBoolValue ("IsChangingPlanes", true);
					setOrientation (0);
					idlePose ();
					state2 = PlayerState.spinning3;
					return;
				}
			}
		}

		if (state2 == PlayerState.spinning3)
		{
			//if (isWaitingForActionToComplete)
			//	return;
			string loc = levelRef.lowerFloorName;
			mcsRef.getStorage().storeBoolValue ("InhibitVignette", true);
			mcsRef.getStorage ().storeFloatValue ("Coords" + loc + "X", this.transform.position.x);
			mcsRef.getStorage ().storeFloatValue ("Coords" + loc + "Y", this.transform.position.y);
			mcsRef.getStorage ().storeFloatValue ("Coords" + loc + "Z", this.transform.position.z);
			mcsRef.getStorage ().storeFloatValue ("Coords" + levelRef.lowerFloorName + "X", this.transform.position.x);
			mcsRef.getStorage ().storeFloatValue ("Coords" + levelRef.lowerFloorName + "Y", this.transform.position.y);
			mcsRef.getStorage ().storeFloatValue ("Coords" + levelRef.lowerFloorName + "Z", this.transform.position.z);
			mcsRef.getStorage().storeIntValue ("Orientation", 0);
			SceneManager.LoadScene (levelRef.lowerFloorName);
		}

		if (state2 == PlayerState.ascending) 
		{
			elapsedTime += Time.deltaTime;
			state2 = PlayerState.ascending2;
//			if (elapsedTime > (1.0f / ascendingAnimationSpeed)) {
//				elapsedTime = 0.0f;
//				if (meditationFrame < Ascending.Length) {
//					//playerRendRef.sprite = Ascending [meditationFrame];
//					//otherPlayerRendRef.sprite = Ascending [meditationFrame++];
//				} else {
//					state2 = PlayerState.ascending2;
//					ascendPos = 0.0f;
//					ascendSpeed = 0.0f;
//					ascendStartPos = spriteQuadRef.transform.localPosition;
//				}
//			}
		}

		if (state2 == PlayerState.ascending2) 
		{
			//Vector3 newPos = new Vector3 (ascendStartPos.x, ascendStartPos.y + ascendPos, ascendStartPos.z);
			ascendPos += ascendSpeed * Time.deltaTime;
			ascendSpeed += ascendAccel * Time.deltaTime;
			//spriteQuadRef.transform.localPosition = newPos;
			if ((ascendPos > 90.0f) && !grabbedFrame) {
				camRef.GetComponent<CameraUtils> ().grab ();
				mcsRef.getStorage ().storeBoolValue ("IsChangingPlanes", true);
				mcsRef.getStorage().storeBoolValue ("InhibitVignette", true);
				grabbedFrame = true;
				//levelRef.fadeout ();
			}
			if(ascendPos > 100.0f) {
				string loc = levelRef.upperFloorName;
//				mcsRef.getStorage ().storeFloatValue ("Coords" + loc + "X", this.transform.position.x);
//				mcsRef.getStorage ().storeFloatValue ("Coords" + loc + "Y", this.transform.position.y);
//				mcsRef.getStorage ().storeFloatValue ("Coords" + loc + "Z", this.transform.position.z);
				mcsRef.getStorage ().storeFloatValue ("Coords" + levelRef.upperFloorName + "X", this.transform.position.x);
				mcsRef.getStorage ().storeFloatValue ("Coords" + levelRef.upperFloorName + "Y", this.transform.position.y);
				mcsRef.getStorage ().storeFloatValue ("Coords" + levelRef.upperFloorName + "Z", this.transform.position.z);

				//levelRef.storePhysicalCameraPosition (levelRef.upperFloorName);

//				ds.storeFloatValue ("Pos" + loc + "X", cam.gameObject.transform.position.x);
//				ds.storeFloatValue ("Pos" + loc + "Y", cam.gameObject.transform.position.y);
//				ds.storeFloatValue ("Pos" + loc + "Z", cam.gameObject.transform.position.z);
//				ds.storeFloatValue ("Rot" + loc + "Y", cam.pivotY.transform.localEulerAngles.y);
//				ds.storeFloatValue ("Rot" + loc + "X", cam.pivotX.transform.localEulerAngles.x);
//				ds.storeFloatValue ("PosIn" + loc + "Z", cam.pivotZ.transform.localPosition.z);

				//ds.storeFloatValue ("PosIn" + loc + "M", cam.mainCamera.transform.localPosition.z);
				//SceneManager.LoadScene (levelRef.upperFloorName);
				levelRef.loadScene(levelRef.upperFloorName);
			}
		}

		if (state2 == PlayerState.spawningShadow1)
		{
			blockedElapsedTime = 0.0f;
			state2 = PlayerState.spawningShadow2; 
		}

		if (state2 == PlayerState.spawningShadow2) 
		{
			blockedElapsedTime += Time.deltaTime;
			if(blockedElapsedTime > 1.0f) { // WARNING constantize
				blockedElapsedTime = 0.0f;
				blinkRemainingTime = 0.0f;
				state2 = PlayerState.spawningShadow3;
			}
		}

		if (state2 == PlayerState.spawningShadow3)
		{
			blockedElapsedTime += Time.deltaTime;

			if (blockedElapsedTime > 1f) 
			{
				if (blocked) 
				{
					unblockControls ();
				}
			}

			if (blockedElapsedTime > blinkingTime) {
				blockedElapsedTime = 0.0f;
				state2 = PlayerState.spawningShadow4;
				spriteQuadRef.GetComponent<SpriteRenderer> ().enabled = true;
			}
			blinkRemainingTime += Time.deltaTime;
			if (blinkRemainingTime > (1.0f / shadowBlinkAnimationSpeed)) {
				blinkRemainingTime = 0.0f;
				blink = !blink;
				if (blink)
					spriteQuadRef.GetComponent<SpriteRenderer> ().enabled = false;
				else
					spriteQuadRef.GetComponent<SpriteRenderer> ().enabled = true;
			}
		}

		if (state2 == PlayerState.spawningShadow4) 
		{
			spriteQuadRef.GetComponent<SpriteRenderer> ().enabled = true;
			state = PlayerState.idling;
			state2 = PlayerState.idling;
			iNetRef.frozen = false;
		}

		spawningShadowElapsedTime += Time.deltaTime;

		if (isWaitingForActionToComplete)
			return;
	}

	void FixedUpdate()
    { 
        if (mustSnapToPlayerStart > 0) {
			if (GameObject.Find ("PlayerStart")) 
			{
				Vector3 newPos = GameObject.Find ("PlayerStart").transform.position;
				this.transform.position = newPos;
			}
			mustSnapToPlayerStart--;
		}

		if (!started)
			return;
		if (mirrorActivationDelaying) 
		{ // Reentry condition: showing mirror activation
			if(elapsedTime == 0.0f) 
			{
				if (ds == null)
					ds = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ().getStorage ();
				int mirrorNumber = ds.retrieveIntValue ("ActivateMirror"); // the gates will be just the 'next' mirror

//				if(cam != null)
//				{
//					cam.warpToMarker (mirrorNumber);
//					GameObject newTarget = GameObject.Find (ds.retrieveStringValue ("ActivatedMirror"));
//					cam.target = newTarget;
//				}
				CameraSwitch cameraController = GameObject.Find("CameraController").GetComponent<CameraSwitch>();
				cameraController._wm_switchToCameraIndex (mirrorNumber);
				if (mirrorObject != null) 
				{
					mirrorObject.activate (mirrorNumber);
				}
			}

			elapsedTime += Time.deltaTime;
			if (elapsedTime > mirrorActivationDelay) {
				ds.storeStringValue ("ReentryCondition", "");
				string returnLocation = ds.retrieveStringValue ("ReturnLocation");
				SceneManager.LoadScene (returnLocation);
			}
			return;
		}

		if (agent.enabled == true) 
		{
			return;
		}

		string intern, backn;
		if (interactee != null)
			intern = interactee.name;
		else intern = "null";
		if (backupInteractee != null)
			backn = backupInteractee.name;
		else backn = "null";

        if (meditatingRemaining > 0.0f)
        {
            meditatingRemaining -= Time.deltaTime;
            return;
        }

        /* WARNING crazy statement, reformulate */
        if (blocked)
			return;
		//if ((state != PlayerState.idling) && (state != PlayerState.walking))
		//	return;

		moving = false;
		if((state != PlayerState.materializing) && (state != PlayerState.dematerialized)) state = PlayerState.idling;

		//float moveHorizontal = Input.GetAxis ("Horizontal");
		//float moveVertical = Input.GetAxis ("Vertical");
		float moveHorizontal = 0.0f;
		float moveVertical = 0.0f;

		/*if (walkType == PlayerWalkType.floating) {
			if (height < equilibriumHeight) {
				heightSpeed += (equilibriumHeight - height)*(equilibriumHeight - height) * floatingAcceleration * Time.deltaTime;
			} else if (height > equilibriumHeight) {
				heightSpeed -= (height - equilibriumHeight)*(height - equilibriumHeight) * floatingAcceleration * Time.deltaTime;
			}
			height += heightSpeed * Time.deltaTime * equilibriumStrength;
			if (heightSpeed > 0.0f) {
				if (heightSpeed > maxHeightSpeed)
					heightSpeed = maxHeightSpeed;
			} else {
				if (heightSpeed < -maxHeightSpeed)
					heightSpeed = -maxHeightSpeed;
			}
		}*/

		if (autopilot == false) {
			moveHorizontal = hud.touchHorizontal () * walkMultiplicator;
			moveVertical = hud.touchVertical () * walkMultiplicator;
		} else {
			Vector3 current = this.transform.position;
			current.y = 0.0f;
			Vector3 dest = autopilotDestination;
			dest.y = 0.0f;
			Vector3 origin = autopilotOrigin;
			origin.y = 0.0f;
			DEJAMEVERESADISTANCIA = Vector3.Dot ((dest - current).normalized, (dest - origin).normalized);
			if (Vector3.Dot ((dest - current).normalized, (dest - origin).normalized) < 0.0f) {
				autopilot = false;
				notifyFinishAction ();
			} else {
				moveHorizontal = (autopilotDestination - autopilotOrigin).normalized.x/10.0f * walkMultiplicator;
				moveVertical = (autopilotDestination - autopilotOrigin).normalized.z/10.0f * walkMultiplicator;
			}
		}

		if(lastHorizontal != 0.0f && lastVertical != 0.0f) 
			direction = Direction4FromVector (new Vector2 (lastHorizontal, lastVertical));
		if ((moveHorizontal != 0.0f) || (moveVertical != 0.0f)) {
			direction = Direction4FromVector (new Vector2 (moveHorizontal, moveVertical));
			lastHorizontal = moveHorizontal;
			lastVertical = moveVertical;
			moving = true;
			state = PlayerState.walking;
		}

		if (moving) {
			setOrientation(direction);

			elapsedTime += Time.deltaTime;
			if (elapsedTime > (1.0f / WalkingAnimationSpeed)) {
				elapsedTime = 0.0f;
				frame++;
				foreach (int f in footstepFrame) {
					if(f == frame) {
						levelRef.footstep();
					}
				}
				if (walkType == PlayerWalkType.onGround) {
					switch (direction) {
					case PlayerDirection.left:
						if (frame == WalkingLeft.Length)
							frame = 1;
						break;
					case PlayerDirection.right:
						if (frame == WalkingRight.Length)
							frame = 1;
						break;
					case PlayerDirection.front:
						if (frame == WalkingFront.Length)
							frame = 1;
						break;
					case PlayerDirection.back:
						if (frame == WalkingBack.Length)
							frame = 1;
						break;
					case PlayerDirection.backleft:
						if (frame == WalkingLeftBack.Length)
							frame = 1;
						break;
					case PlayerDirection.backright:
						if (frame == WalkingBackRight.Length)
							frame = 1;
						break;
					case PlayerDirection.frontleft:
						if (frame == WalkingFrontLeft.Length)
							frame = 1;
						break;
					case PlayerDirection.frontright:
						if (frame == WalkingRightFront.Length)
							frame = 1;
						break;
					}
				} else {
					switch (direction) {
					case PlayerDirection.left:
						if (frame == floatingLeft.Length)
							frame = 0;
						break;
					case PlayerDirection.right:
						if (frame == floatingRight.Length)
							frame = 0;
						break;
					case PlayerDirection.front:
						if (frame == floatingFront.Length)
							frame = 0;
						break;
					case PlayerDirection.back:
						if (frame == floatingBack.Length)
							frame = 0;
						break;
					case PlayerDirection.backleft:
						if (frame == floatingBackLeft.Length)
							frame = 0;
						break;
					case PlayerDirection.backright:
						if (frame == floatingBackRight.Length)
							frame = 0;
						break;
					case PlayerDirection.frontleft:
						if (frame == floatingFrontLeft.Length)
							frame = 0;
						break;
					case PlayerDirection.frontright:
						if (frame == floatingFrontRight.Length)
							frame = 0;
						break;
					}
				}
			}

		} else { // not moving
			if (walkType == PlayerWalkType.onGround) {
				switch (direction) {
				case PlayerDirection.left:
					playerRendRef.sprite = WalkingLeft [0];
					otherPlayerRendRef.sprite = WalkingLeft [0];
					break;
				case PlayerDirection.right:
					playerRendRef.sprite = WalkingRight [0];
					otherPlayerRendRef.sprite = WalkingRight [0];
					break;
				case PlayerDirection.front:
					playerRendRef.sprite = WalkingFront [0];
					otherPlayerRendRef.sprite = WalkingFront [0];
					break;
				case PlayerDirection.back:
					playerRendRef.sprite = WalkingBack [0];
					otherPlayerRendRef.sprite = WalkingBack [0];
					break;
				case PlayerDirection.backleft:
					playerRendRef.sprite = WalkingLeftBack [0];
					otherPlayerRendRef.sprite = WalkingLeftBack [0];
					break;
				case PlayerDirection.backright:
					playerRendRef.sprite = WalkingBackRight [0];
					otherPlayerRendRef.sprite = WalkingBackRight [0];
					break;
				case PlayerDirection.frontleft:
					playerRendRef.sprite = WalkingFrontLeft [0];
					otherPlayerRendRef.sprite = WalkingFrontLeft [0];
					break;
				case PlayerDirection.frontright:
					playerRendRef.sprite = WalkingRightFront [0];
					otherPlayerRendRef.sprite = WalkingRightFront [0];
					break;
				}
			}

			frame = 0;
			elapsedTime = 0.0f;
		}

		bool cannotWalk = false;

		float yVelocity = r.velocity.y;

		Vector3 movement = new Vector3 (externalForce.x + moveHorizontal*speed, externalForce.y + yVelocity, externalForce.z + moveVertical*speed); // WARNING magic number
		Vector3 flatMovement = movement;
		flatMovement.y = 0.0f;
		modulusSpeed = flatMovement.magnitude;
		if ((!blocked) && (!cannotWalk))
			r.velocity = movement;
		else
			r.velocity = new Vector3 (0.0f, 0.0f, 0.0f);

		if (Input.GetMouseButtonDown (0) && (moving == false)) 
		{
			canInteract = true;
		}

		if (moving == true)
			canInteract = false;

		if(Input.GetMouseButtonUp(0) && (canInteract == true)) {

			canInteract = false;

			switch (interaction) {

			case Interaction.None:
				break;

			default:
				if(interactee!=null) interactee.effect ();
				break;
			}
		}
	}

	public void autopilotTo(Vector3 dest) 
	{
		autopilotOrigin = this.transform.position;
		autopilotDestination = dest;
		autopilot = true;
	}

	public void _wa_autopilotTo(WisdominiObject waiter, float dx, float dy, float dz) 
	{
		waiter.isWaitingForActionToComplete = true;
		programNotification = -1;
		waitingRef = waiter;
		autopilotTo (new Vector3(dx, dy, dz));
	}

	public void _wa_autopilotToRelative(WisdominiObject waiter, float dx, float dy, float dz) 
	{
		waiter.isWaitingForActionToComplete = true;
		programNotification = -1;
		waitingRef = waiter;
		autopilotTo (new Vector3(this.transform.position.x + dx, this.transform.position.y + dy, this.transform.position.z + dz));
	}
	/*
	public bool getRightDPad() {

		bool res;
		res = hud.touchRight () || Input.GetButton ("Right");

		return res;
	}
*/
	/*
	public bool getLeftDPad() {

		bool res;
		res = hud.touchLeft () || Input.GetButton ("Left");

		return res;
	}

	public bool getUpDPad() {

		bool res;
		res = hud.touchUp () || Input.GetButton ("Up");

		return res;
	}

	public bool getDownDPad() {

		bool res;
		res = hud.touchDown () || Input.GetButton ("Down");

		return res;
	}
	*/
	public void blockControls() 
	{
		blocked = true;
		idlePose ();
		_wm_hideIcon ();
	}

	public void unblockControls() 
	{
		blocked = false;
		_wm_showIcon ();

	}

	public void _wm_callMe(int a, float b) 
	{ // our first exported method

	}

	public bool interactingWith(Interactor other) 
	{
		return interactee == other;
	}

	public void spin(WisdominiObject waiter) 
	{
		waitingRef = waiter;
		state2 = PlayerState.spinning;
		angle = 0.0f;
		angleSpeed = 0.0f;
		angleAccel = maxAngleAccel;
		walkType = PlayerWalkType.onGround;
		blockControls ();
	}

	public void meditate() 
	{
		state2 = PlayerState.meditating;
        setOrientation(0);
		elapsedTime = 0.0f;
        levelRef.storeBoolValue("Meditating", true);
        levelRef.storeIntValue("Orientation", 0);
        blockedElapsedTime = 0.0f;
		blocked = true;
		meditationFrame = 0;
		playerRendRef.sprite = Meditating [0];
		otherPlayerRendRef.sprite = Meditating [0];
	}

	public void directionFromVector(Vector3 v, bool isRelative) 
	{
		Vector3 rel;
		if (isRelative)
			rel = v;
		else {
			rel = v - this.transform.position;
		}
		if (Mathf.Abs (rel.x) > Mathf.Abs (rel.z)) { // horizontal

			if (rel.x > 0.0f) {
				images = WalkingRight;
				return;
			}

			images = WalkingLeft;
			return;

		} else { // vertical

			if (rel.z > 0.0f) {
				images = WalkingBack;
				return;
			}
			images = WalkingFront;
			return;
		}
	}

	/* angle in degrees, plees  0 degrees is looking front */
	public void directionFromAngle(float angle) 
	{
		while (angle > 360.0f)
			angle -= 360.0f;

		if (angle < 45) {
			images = WalkingFront;
			return;
		}
		if (angle < 90) {
			images = WalkingFrontLeft; // WARNING frontleft, when the art is ready!!
			return;
		}
		if (angle < 135) {
			images = WalkingLeft;
			return;
		}
		if (angle < 180) {
			images = WalkingLeftBack;
			return;
		}
		if (angle < 225) {
			images = WalkingBack;
			return;
		}
		if (angle < 270) {
			images = WalkingBackRight;
			return;
		}
		if (angle < 315) {
			images = WalkingRight;
			return;
		}
		if (angle < 360) {
			images = WalkingRightFront;
			return;
		}
	}

	public void ascend() 
	{
		state2 = PlayerState.ascending;
		frame = 0;
		elapsedTime = 0.0f;
	}

	public bool spawnShadow() 
	{
		if (spawningShadowElapsedTime < noSpawningShadowTime)
			return false;

		state2 = PlayerState.spawningShadow1;
		blockControls ();

		spawningShadowElapsedTime = 0.0f;

		return true;
	}

	public void OnTriggerEnter(Collider other) 
	{
		//enterexittext.text = enterexittext.text + "E";
		Interactor test = other.gameObject.GetComponent<Interactor> ();

		if (test != null) {
			if (test.getOpacity () == 0.0f) // can't interact with faded out things!
				return;
			if (interactee != null) { // if we are already tending to one interactee
				if(backupInteractee == null) 
					backupInteractee = test; // save for later
				return;
			}
		}
		if ((test != null) && test.interactEnabled) 
		{
			interactee = test;
			interaction = Interaction.None;
			string icon = interactee.interactIcon ();
			if (icon.Equals ("Hand")) {
				interactionQuadRef.GetComponent<SpriteRenderer> ().enabled = true;
                interactionQuadRef.GetComponent<SpriteRenderer>().sprite = HandIcon; 
				interaction = Interaction.Something;
			}
			if (icon.Equals ("Mouth")) {
				interactionQuadRef.GetComponent<SpriteRenderer> ().enabled = true;
                interactionQuadRef.GetComponent<SpriteRenderer>().sprite = MouthIcon;  
				interaction = Interaction.Something;
			}
			if (icon.Equals ("Eye")) {
				interactionQuadRef.GetComponent<SpriteRenderer> ().enabled = true;
                interactionQuadRef.GetComponent<SpriteRenderer>().sprite = EyeIcon;
				interaction = Interaction.Something;
			}
		}
	}

	public void _wm_getUnfollowed()
	{

	}

	public void enterMirror() 
	{
		lerp = 0.0f;
		enterMirrorStartPosition = this.transform.position;
		enterMirrorPosition = this.transform.position + new Vector3 (0, 3.0f, 3.0f);
		state2 = PlayerState.enteringMirror;
	}

	public void OnTriggerExit(Collider other) 
	{
		//enterexittext.text = enterexittext.text + "X";
		Interactor test = other.gameObject.GetComponent<Interactor> ();
		if (test != null) 
		{
			if (test == backupInteractee) { // exitting from backup
				backupInteractee = null; // no worries
			} else if (test == interactee) {
				interactee = null;
				interactionQuadRef.GetComponent<SpriteRenderer> ().enabled = false;
				interaction = Interaction.None;
				if (backupInteractee != null) { // if we have a backup interactee...
					// backupInteractee becomes main interactee
					Interactor temp = backupInteractee;
					interactee = null;
					backupInteractee = null;
					OnTriggerEnter (temp.GetComponent<Collider> ()); // chain OnTriggerEnter
				}
			}
		}
	}

	public void _wm_hideIcon() 
	{
		interactionQuadRef.GetComponent<SpriteRenderer> ().enabled = false;
	}

	public void _wm_showIcon() 
	{
		interactionQuadRef.GetComponent<SpriteRenderer> ().enabled = true;

		if (interaction == Interaction.None) 
		{
			_wm_hideIcon ();
		}
	}

	public void _wm_idlePose()
	{
		idlePose ();
	}

	public void idlePose() 
	{
		frame = 0;
		playerRendRef.sprite = images [frame];
		otherPlayerRendRef.sprite = images [frame];
	}

	/* Add camera Y rotation offset */
	public PlayerDirection Direction4FromVector(Vector2 v) 
	{ // WARNING duplicate

		float width;//, depth;
		const float firstOctantCos = 0.92f;
		const float thirdOctantCos = 0.38f;

		//if (cam.pivotY != null) {
		v = Quaternion.AngleAxis (cam.pivotY.transform.localEulerAngles.y, Vector3.forward) * v;
		//}
		v.Normalize ();

		width = Mathf.Abs (v.x);
		//depth = Mathf.Abs (v.y);

		if (width > firstOctantCos) { // left or right

			if (v.x > 0.0)
				return PlayerDirection.right;
			else
				return PlayerDirection.left;

		} else if (width > thirdOctantCos) { // diagonals

			if (v.y > 0.0f && v.x > 0.0f)
				return PlayerDirection.backright;
			else if (v.y > 0.0f && v.x < 0.0f)
				return PlayerDirection.backleft;
			else if (v.y < 0.0f && v.x > 0.0f)
				return PlayerDirection.frontright;
			else
				return PlayerDirection.frontleft;

		} else if (width <= thirdOctantCos) { // up or down

			if (v.y > 0.0f)
				return PlayerDirection.back;
			else
				return PlayerDirection.front;
		}

		return PlayerDirection.front;
	}

	public void land() 
	{
		Levitator lev;
		lev = spriteQuadRef.GetComponent<Levitator> ();
		if (lev != null)
			lev.takeOn ();

		lev = spriteQuadBehindRef.GetComponent<Levitator> ();
		if (lev != null)
			lev.takeOn ();
	}

	public void takeOff() {
		Levitator lev;
		lev = spriteQuadRef.GetComponent<Levitator> ();
		if (lev != null)
			lev.takeOff ();

		lev = spriteQuadBehindRef.GetComponent<Levitator> ();
		if (lev != null)
			lev.takeOff ();
	}

	public void WalkingMode()
	{
		string nombreNivel = levelRef.locationName;

		if (nombreNivel.Contains ("+")) {
			walkType = PlayerWalkType.floating;
		} else {
			walkType = PlayerWalkType.onGround;
		}
	}

	public void SelectHUD()
	{

		if (HUDSelected)
			return;
		HUDSelected = true;

		string nombreNivel = levelRef.locationName;
		GameObject lightHUD = GameObject.Find ("LightHUD");
		GameObject darkHUD = GameObject.Find ("DarkHUD");
		GameObject normalHUD = GameObject.Find ("HUD");

		//public GameObject[] disabledHUD;
		disabledHUD = GameObject.FindGameObjectsWithTag ("HUD");

		if (nombreNivel.Contains ("+")) 
		{
			currentHUD = lightHUD;
		} 

		else 
		{
			if (nombreNivel.Contains ("exterior")) 
			{
				if (nombreNivel.Contains ("-"))
				{
					currentHUD = darkHUD;
				} 

				else 
				{
					currentHUD = normalHUD;
				}
			} 

			else 
			{
				currentHUD = normalHUD;
			}
		}

		for (int i = 0; i < disabledHUD.Length; i++) 
		{
			if (currentHUD != disabledHUD [i].gameObject) 
			{
				disabledHUD [i].SetActive (false);
			}
		}

		hud = currentHUD.GetComponent<HUDController> ();
		inventory = hud.GetComponentInChildren<UIInventoryV2> ();
	}

	public bool _wm_ObtainState()
	{
		return materializated;
	}
}


