using UnityEngine;
using System.Collections;

enum ShadowState { spawning1, spawning2, spawning3, idle, walking, seeking, enteringMirror, entered, summoned };
enum ShadowStateSlot2 { disabled, cannotShoot, idling, bursting, blinking };

/* WARNING factorize all common attributes with Player or not?? */
public class Shadow : WisdominiObject {

	/* references */

	public Freezer freezer;
	public Sprite[] WalkingFront;
	public Sprite[] WalkingFrontLeft;
	public Sprite[] WalkingLeft;
	public Sprite[] WalkingLeftBack;
	public Sprite[] WalkingBack;
	public Sprite[] WalkingBackRight;
	public Sprite[] WalkingRight;
	public Sprite[] WalkingRightFront;
	public GameObject eyesPrefab;
	public float spawnDirection = 1.0f;
	Sprite[] images;
	Rigidbody rigidBody;
	public GameObject SpriteQuad;
	SpriteRenderer spriteRend;
	Material SpriteMat;
	public StringBank redEyesStringBank;
	new public Rosetta rosetta;
	public GameObject thoughtTextPrefab;
	//[HideInInspector]
	//public NavMeshAgent navAgent;

	/* properties */

	float timeToStateChange;
	float animElapsedTime;
	float stateElapsedTime;
	public float animationSpeed = 6.0f;
	float angle;
	float targetAngle;
	float elapsedTime;
	float linearSpeed;
	ShadowState state;
	int substate = 0;
	ShadowStateSlot2 state2;
	int frame;
	bool initialized = false;
	bool blink;
	float timeToBurst;
	float burstElapsedTime;
	float eyeShotElapsedTime;
	float timeToEyeShot;
	float canShootRedEyesTime;
	public int id; // shadow id
	Vector3 mirrorPos;
	Vector3 startPos;
	float lerp;
	bool frozen;
	bool moving;
	float y;
	Vector3 summonInitialPosition;
	Vector3 summonIntermediatePosition;
	Vector3 summonFinalPosition;
	Vector3 summonFinalPosition2;
	Vector3 summonMirrorPosition;
	float summonDelay;
	[HideInInspector]
	public int control, strength;
	public int shadowWaveInt;
	public string shadowWaveStr;

	/* constants */

	const float minTimeToStateChange = 1.0f;
	const float maxTimeToStateChange = 8.0f;
	const float angleSpeed = 10.0f;
	const float blinkSpeed = 30.0f;
	const float maxLinearSpeed = 5.0f;
	const float lerpSpeed = 0.4f;
	const float minBurstDuration = 0.5f;
	const float maxBurstDuration = 1.0f;
	const float minTimeToBurst = 3.0f;
	const float maxTimeToBurst = 8.0f;
	const float maxEyeShotInterval = 0.2f;
	const float minEyeShotInterval = 0.06f;

	const float doNotShootRedEyesTime = 5.0f;
	const float distThreshold = 0.75f;

	Vector3 autopilotDestination;
	Vector3 autopilotOrigin;
	bool autopilot = false;

	/* methods */

	new void Start() 
	{
		if (!initialized)
			initialize ();
	}

	public void initialize () 
	{	
		frame = 0;
		linearSpeed = maxLinearSpeed;
		elapsedTime = 0.0f;
		state = ShadowState.spawning1;
		state2 = ShadowStateSlot2.cannotShoot;
		timeToStateChange = floatRandomRange (minTimeToStateChange, maxTimeToStateChange);
		targetAngle = angle = (floatRandomRange (0.0f, 360.0f) / 360.0f) * 2.0f * 3.1416f;
		rigidBody = this.GetComponent<Rigidbody> ();
		spriteRend = SpriteQuad.GetComponent<SpriteRenderer> ();
		SpriteMat = spriteRend.material;
		/*SpriteMat.SetFloat("_Mode", 2);
		SpriteMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
		SpriteMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
		SpriteMat.SetInt("_ZWrite", 1);
		SpriteMat.EnableKeyword("_ALPHATEST_ON");
		SpriteMat.DisableKeyword("_ALPHABLEND_ON");
		SpriteMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		//SpriteMat.renderQueue = 2450;*/
		directionFromAngle (angle);
		spriteRend.sprite = images [0];
		stateElapsedTime = 0.0f;
		canShootRedEyesTime = 0.0f;
		burstElapsedTime = 0.0f;
		timeToBurst = floatRandomRange (minBurstDuration, maxBurstDuration);
		initialized = true;
		blink = false;
		rigidBody.velocity = new Vector3 (0, 0, 0);
		rigidBody.detectCollisions = false;
		rigidBody.isKinematic = true;
		if (freezer == null) 
		{
			freezer = GameObject.Find ("Freezer").GetComponent<Freezer> ();
		}
		//navAgent = this.GetComponent<NavMeshAgent> ();
		this.frozen = false;

		rigidBody.useGravity = true;
		moving = true;
	}
	
	new void Update () 
	{	
		if (freezer.frozen)
			return;

		if (state == ShadowState.seeking) 
		{

		}

		if (state == ShadowState.entered) 
		{			
			
		}

		if (state == ShadowState.enteringMirror)
		{
			lerp += lerpSpeed * Time.deltaTime;
			if (lerp > 1.0f) {
				SpriteMat.color = new Color (1, 1, 1, 0);
				state = ShadowState.entered;
				state2 = ShadowStateSlot2.disabled;
			}
		}

		if (state2 == ShadowStateSlot2.disabled) 
		{

		}

		if (state2 == ShadowStateSlot2.blinking) 
		{
			blink = !blink;
			if (blink) {
				SpriteMat.color = new Color (1, 1, 1, 0);
			} else {
				SpriteMat.color = new Color (1, 1, 1, 1);
			}
		}

		if (state2 == ShadowStateSlot2.cannotShoot)
		{
			canShootRedEyesTime += Time.deltaTime;
			if (canShootRedEyesTime > doNotShootRedEyesTime) 
			{
				state2 = ShadowStateSlot2.idling;
			}
		}

		if (state2 == ShadowStateSlot2.idling) 
		{
			burstElapsedTime += Time.deltaTime;
		
			if (burstElapsedTime > timeToBurst) 
			{
				burstElapsedTime = 0.0f;
				eyeShotElapsedTime = 0.0f;
				timeToEyeShot = floatRandomRange (minEyeShotInterval, maxEyeShotInterval);
				timeToBurst = floatRandomRange (minBurstDuration, maxBurstDuration);
				state2 = ShadowStateSlot2.bursting;
			}		
		}

		if (state2 == ShadowStateSlot2.bursting) 
		{
			eyeShotElapsedTime += Time.deltaTime;
			burstElapsedTime += Time.deltaTime;
			if (eyeShotElapsedTime > timeToEyeShot) {
				eyeShotElapsedTime = 0.0f;
				timeToEyeShot = floatRandomRange (minEyeShotInterval, maxEyeShotInterval);
				GameObject newEyes = (GameObject) Instantiate (eyesPrefab, this.transform.position + new Vector3(0, 2.0f, 0), Quaternion.Euler (0, 0, 0));
				newEyes.GetComponent<Redeyes> ().thoughtTextPrefab = thoughtTextPrefab;
				IconCooldown coolDown = GameObject.Find ("StarCooldown").GetComponent<IconCooldown> ();
				newEyes.GetComponent<Redeyes> ().eyeDispelController = coolDown;
				GameObject p = GameObject.Find ("Player");
				newEyes.GetComponent<Redeyes> ().player = p;
				Letter letter = GameObject.Find ("Letter").GetComponent<Letter> ();
				newEyes.GetComponent<Redeyes> ().letter = letter;
				newEyes.GetComponent<Redeyes> ().initialize ();
				redEyesStringBank.rosetta = rosetta;
				newEyes.GetComponent<Redeyes> ().stringBank = redEyesStringBank;
				newEyes.GetComponent<Redeyes> ().rosetta = rosetta;
			}

			if (burstElapsedTime > timeToBurst)
			{
				burstElapsedTime = 0.0f;
				timeToBurst = floatRandomRange (minTimeToBurst, maxTimeToBurst);
				state2 = ShadowStateSlot2.idling;
			}
		}

		if (state == ShadowState.spawning1) 
		{
			stateElapsedTime += Time.deltaTime;
			Vector3 newPos = this.transform.position;
			newPos.x += Time.deltaTime * spawnDirection * 2.0f;
			this.transform.position = newPos;
			if (stateElapsedTime > 1.0f) {
				state = ShadowState.spawning2;
				rigidBody.detectCollisions = true;
				rigidBody.isKinematic = false;
			}		
		}

		if (state == ShadowState.spawning2) 
		{
			stateElapsedTime += Time.deltaTime;
			if (stateElapsedTime > 2.0f) {
				state = ShadowState.spawning3;
				rigidBody.velocity = new Vector3(0, 0, 0);
			}
			rigidBody.velocity = new Vector3(1.0f, 0.0f, 0.0f);
		}

		if (state == ShadowState.spawning3) 
		{
			stateElapsedTime += Time.deltaTime;
			if (stateElapsedTime > 2.0f) {
				state = ShadowState.idle;
				SpriteMat.color = new Vector4 (1, 1, 1, 1);
			}
		}

		if (state == ShadowState.spawning1 ||
		   state == ShadowState.spawning2 ||
		   state == ShadowState.spawning3) {

			elapsedTime += Time.deltaTime;
			if (elapsedTime > (1.0f / blinkSpeed))
			{
				elapsedTime = 0.0f;
				blink = !blink;
				if (blink) {
					SpriteMat.color = new Vector4 (1, 1, 1, 0);
				} else {
					SpriteMat.color = new Vector4 (1, 1, 1, 1);
				}
			}

			return;
		}

		elapsedTime += Time.deltaTime;
		animElapsedTime += Time.deltaTime;

		if (elapsedTime > timeToStateChange) {
			timeToStateChange = floatRandomRange (minTimeToStateChange, maxTimeToStateChange);
			targetAngle = (floatRandomRange (0.0f, 360.0f) / 360.0f) * 2.0f * 3.1416f;
			elapsedTime = 0.0f;
		}

		if (angle < targetAngle) 
		{
			angle += angleSpeed * Time.deltaTime;
			if (angle > targetAngle) {
				angle = targetAngle;
			}
		}

		if (angle > targetAngle)
		{
			angle -= angleSpeed * Time.deltaTime;
			if (angle < targetAngle) {
				angle = targetAngle;
			}
		}

		if (state == ShadowState.walking) 
		{
			
		}

		if (state == ShadowState.summoned) 
		{
			if (substate == 0) { // waiting for shadow to reach Intermediate position

				if (!isWaitingForActionToComplete) 
				{					
					autopilotOrigin = summonIntermediatePosition;
					autopilotDestination = summonFinalPosition;
					autopilot = true;
					substate = 1;
					elapsedTime = 0.0f;
					this.isWaitingForActionToComplete = true;
					waitingRef = this;
				}
			}

			if (substate == 1) { // waiting for shadow to reach final position 2

				if (!isWaitingForActionToComplete) 
				{
					autopilotOrigin = summonFinalPosition;
					autopilotDestination = summonFinalPosition2;
					autopilot = true;
					substate = 2;
					elapsedTime = 0.0f;
					this.isWaitingForActionToComplete = true;
					waitingRef = this;
				}
			}

			if (substate == 2) { // waiting for shadow to reach mirror

				if (!isWaitingForActionToComplete) 
				{
					moving = false;
					elapsedTime += Time.deltaTime;
					if (elapsedTime > (1.0f + summonDelay))
					{
						rigidBody.isKinematic = true;
						rigidBody.useGravity = false;
						lerp = 0.0f;
						summonFinalPosition2 = this.transform.position;
						substate = 3;
					}
				}
			}

			if (substate == 3)
			{
				lerp += Time.deltaTime;
				if (lerp > 1.0f) {
					lerp = 1.0f;
					substate = 4;
				}
				if (blink)
					spriteRend.enabled = false;
				else
					spriteRend.enabled = true;
				blink = !blink;
				Vector3 pos = Vector3.Lerp (summonFinalPosition2, summonMirrorPosition, lerp);
				this.transform.position = pos;
				this.transform.localScale = new Vector3 (1.0f - lerp, 1.0f - lerp, 1.0f - lerp);
			}
		}

		if (moving) {
			directionFromAngle (angle);
			spriteRend.sprite = images [frame % images.Length];
			if (animElapsedTime > (1.0f / animationSpeed)) {
				animElapsedTime = 0.0f;
				frame = (frame + 1) % images.Length;
			}
		} else {
			spriteRend.sprite = images [0];
		}
	}

	public void summon(Vector3 initialPosition, Vector3 intermediatePosition, Vector3 finalPosition, Vector3 mirrorPosition, float summonD) 
	{
		summonInitialPosition = initialPosition;
		summonIntermediatePosition = intermediatePosition;
		summonFinalPosition = finalPosition;
		summonFinalPosition2 = finalPosition + new Vector3 (0, 0, 1.6f);
		summonMirrorPosition = mirrorPosition + new Vector3(0, 3, 0); // ad-hoc correction: a tad higher
		summonDelay = summonD;

		this.transform.position = initialPosition;
		rigidBody.useGravity = true;
		rigidBody.isKinematic = false;

		autopilotOrigin = initialPosition;
		autopilotDestination = intermediatePosition;
		autopilot = true;

		state = ShadowState.summoned;
		state2 = ShadowStateSlot2.disabled;
		substate = 0;

		this.isWaitingForActionToComplete = true;
		waitingRef = this;

		linearSpeed = linearSpeed * 1.5f;
	}

	public void warpTo(Vector3 loc) 
	{
		this.transform.position = loc; // y que no duela
	}

	void FixedUpdate() {

		if (freezer.frozen && !this.frozen) 
		{
			this.frozen = true;
		}
		if (!freezer.frozen && this.frozen) 
		{
			this.frozen = false;
		}

		if (state == ShadowState.enteringMirror) 
		{
			Vector3 newPos = Vector3.Lerp (startPos, mirrorPos, lerp);
			this.transform.position = newPos;
			return;
		}

		if (state == ShadowState.spawning1 ||
		   state == ShadowState.spawning2 ||
		   state == ShadowState.spawning3)
			return;

		if (state != ShadowState.seeking) 
		{			
			Vector3 velocity = Vector3.zero;

			if (autopilot == true) 
			{
				if (Vector3.Dot ((autopilotDestination - this.transform.position).normalized, (autopilotDestination - autopilotOrigin).normalized) < 0.0f) {
					autopilot = false;
					notifyFinishAction ();
				} 
				else {
					angle = angleFromVector2 ((autopilotDestination - autopilotOrigin).normalized);
					directionFromAngle (angle);
					float moveHorizontal = (autopilotDestination - autopilotOrigin).normalized.x  * linearSpeed;
					float moveVertical = (autopilotDestination - autopilotOrigin).normalized.z  * linearSpeed;
					velocity = new Vector3 (moveHorizontal, rigidBody.velocity.y, moveVertical);
				}
			} 
			else { // no autopilot
				if (moving)
				{
					float yVel = rigidBody.velocity.y;
					if (yVel > 10.0f)
						yVel = 10.0f;
					if (yVel < -10.0f)
						yVel = -10.0f;

					velocity = new Vector3 (Mathf.Cos (angle) * linearSpeed, rigidBody.velocity.y, Mathf.Sin (angle) * linearSpeed);
				}
			}
			//velocity *= linearSpeed;
			rigidBody.velocity = velocity;
			//navAgent.Move (velocity);
		}
	}

	public float angleFromVector2(Vector2 dir) 
	{
		dir.Normalize ();
		float res = Mathf.Acos (dir.x);
		if (dir.y < 0.0f)
			res += Mathf.PI / 2.0f;

		return res;
	}

	/* WARNING put floatRangomRange somewhere sensible, do not repeat across classes */
	public float floatRandomRange(float min, float max) 
	{
		int iMax, iMin;

		const float granularity = 10000.0f;

		iMax = (int)(max * granularity);
		iMin = (int)(min * granularity);

		int iRes = Random.Range(iMin, iMax);

		float fRes = ((float)iRes) / granularity;

		return fRes;
	}

	public void mirrorConverge(Vector3 startPos, Vector3 stopPos, Vector3 mirrorPos) 
	{
		state = ShadowState.seeking;
		state2 = ShadowStateSlot2.disabled;
		//navAgent.speed = linearSpeed * 75.0f;
		float distToMirror = (mirrorPos - this.transform.position).magnitude;
		if (distToMirror > ShadowSpawnController.offScreenRadius) {
			//navAgent.Warp (startPos);
		}
		//navAgent.destination = stopPos;
		//navAgent.updateRotation = false;
		this.mirrorPos = mirrorPos;
		this.startPos = stopPos;
	}

	public bool hasConverged() 
	{
		//float distToTarget = (navAgent.destination - this.transform.position).magnitude;
		float distToTarget = 10000.0f;
		return distToTarget <= distThreshold;
	}

	public void enterMirror() 
	{
		y = this.transform.position.y;
		//navAgent.enabled = false;
		rigidBody.detectCollisions = false;
		rigidBody.useGravity = false;
		state = ShadowState.enteringMirror;
		lerp = 0.0f;
		state2 = ShadowStateSlot2.blinking;
		startPos.y = y;
		mirrorPos.y = y;
	}

	/* angle in degrees, plees  0 degrees is looking front */
	public void directionFromAngle(float angle) 
	{
		angle = (angle / (2.0f * 3.1416f)) * 360.0f;

		while (angle > 360.0f)
			angle -= 360.0f;
		
		if (angle < 22.5f) {
			images = WalkingRight;
			return;
		}
		if (angle < 67.5f) {
			images = WalkingBackRight; // WARNING frontleft, when the art is ready!!
			return;
		}
		if (angle < 112.5f) {
			images = WalkingBack;
			return;
		}
		if (angle < 157.5f) {
			images = WalkingLeftBack;
			return;
		}
		if (angle < 202.5f) {
			images = WalkingLeft;
			return;
		}
		if (angle < 247.5f) {
			images = WalkingFrontLeft;
			return;
		}
		if (angle < 292.5f) {
			images = WalkingFront;
			return;
		}
		if (angle < 337.5f) {
			images = WalkingRightFront;
			return;
		}
		if (angle < 360) {
			images = WalkingRight;
			return;
		}
	}
}
