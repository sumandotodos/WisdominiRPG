using UnityEngine;
using System.Collections;

enum WhirlwindState { starting, moving, dying };

public class Whirlwind : WisdominiObject {

	/* references */

	public Freezer freezer;
	public GameObject shadowPrefab;
	public GameObject redeyesPrefab;
	public interconNetwork parent;
	public GameObject SpriteQuad;
	public Sprite[] start;
	public Sprite[] main;
	public Sprite[] end;
	SpriteRenderer sr;
	Material SpriteMat;
	Rigidbody rigidBody;
	new public Rosetta rosetta;
	public GameObject thoughtTextPrefab;

	public StringBank places;
	public StringBank dates;
	public StringBank placesStringBank;
	public StringBank datesStringBank;
	public StringBankCollection shadowStringBanks;
	public StringBankCollection redEyesStringBanks;
	StringBank shadowStringBank;
	StringBank redEyesStringBank;

	public Sprite FamilyIcon;
	public Sprite CoupleIcon;
	public Sprite FriendsIcon;
	public Sprite WorkIcon;

	public ShadowSpawnController shadowSpawnController;

	/* public properties */

	public float animationSpeed = 12.0f;

	const int iconFamily = 2;
	const int iconCouple = 0;
	const int iconWork = 1;
	const int iconFriends = 3;

	/* properties */

	float angle; // angle is soft
	float targetAngle;
	float linearSpeed;
	float elapsedTime;
	float veerElapsedTime;
	float ttlElapsedTime;
	float timeToLive;
	float timeToVeer;
	WhirlwindState state = WhirlwindState.starting;
	int frame;
	bool initialized = false;

	/* constants */

	const float minTimeToVeer = 1.0f;
	const float maxTimeToVeer = 5.0f;
	const float minLinearSpeed = 8.0f;
	const float maxLinearSpeed = 12.0f;
	const float minTTL = 5.0f;
	const float maxTTL = 10.0f;
	const float angleSpeed = 8.0f;


	/* methods */

	public void initialize() 
	{
		/* initial angle is random */
		targetAngle = angle = (floatRandomRange(0.0f, 360.0f) / 360.0f) * 2.0f * 3.1416f;

		/* time to live */
		timeToLive = floatRandomRange (minTTL, maxTTL);

		linearSpeed = floatRandomRange (minLinearSpeed, maxLinearSpeed);
		timeToVeer = floatRandomRange (minTimeToVeer, maxTimeToVeer);
		elapsedTime = 0.0f;
		ttlElapsedTime = 0.0f;
		state = WhirlwindState.starting;
		frame = 0;
		veerElapsedTime = 0.0f;
		SpriteMat = SpriteQuad.GetComponent<Renderer> ().material;
		sr = SpriteQuad.GetComponent<SpriteRenderer> ();
		/*SpriteMat.SetFloat("_Mode", 2);
		SpriteMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
		SpriteMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
		SpriteMat.SetInt("_ZWrite", 1);
		SpriteMat.EnableKeyword("_ALPHATEST_ON");
		SpriteMat.DisableKeyword("_ALPHABLEND_ON");
		SpriteMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		//SpriteMat.renderQueue = 2450;*/
		//SpriteMat.mainTexture = start [0];
		sr.sprite = start [0];
		rigidBody = this.GetComponent<Rigidbody> ();
		initialized = true;
		if (freezer == null) {
			freezer = GameObject.Find ("Freezer").GetComponent<Freezer> ();
		}
	}

	public void Start ()
	{
		if (!initialized)
			initialize ();	
	}
	
	new void Update () 
	{
		if (freezer == null)
			freezer = GameObject.Find ("Freezer").GetComponent<Freezer> ();
		if (freezer.frozen)
			return;

		elapsedTime += Time.deltaTime;

		if (state == WhirlwindState.starting) 
		{
			if (elapsedTime > (1.0f / animationSpeed)) 
			{
				if (frame < start.Length) {
					sr.sprite = start [frame++];
				} else {
					state = WhirlwindState.moving;
					frame = 0;
				}
				elapsedTime = 0.0f;
			}
		}

		if (state == WhirlwindState.moving) 
		{
			veerElapsedTime += Time.deltaTime;
			if (veerElapsedTime > timeToVeer) 
			{
				veerElapsedTime = 0.0f;
				timeToVeer = floatRandomRange (minTimeToVeer, maxTimeToVeer);
				targetAngle = (floatRandomRange(0.0f, 360.0f) / 360.0f) * 2.0f * 3.1416f;
			}

			if (elapsedTime > (1.0f / animationSpeed)) 
			{
				elapsedTime = 0.0f;
				if (frame < main.Length) 				
				{
					sr.sprite = main [frame];
					frame = (frame + 1) % main.Length;
				}
			}

			if(!parent.frozen) ttlElapsedTime += Time.deltaTime;
			if (ttlElapsedTime > timeToLive) 
			{
				state = WhirlwindState.dying;
				frame = 0;
			}
		}

		if (state == WhirlwindState.dying) 
		{
			if (elapsedTime > (1.0f / animationSpeed)) 
			{
				elapsedTime = 0.0f;
				if (frame < end.Length) 
				{
					sr.sprite = end [frame++];
				}
				else 
				{
					Destroy (this.gameObject);
				}
			}
		}

		if (angle < targetAngle) 
		{
			angle += angleSpeed * Time.deltaTime;
			if (angle > targetAngle) {
				angle = targetAngle;
			}
		}

		if (angle > targetAngle) {

			angle -= angleSpeed * Time.deltaTime;
			if (angle < targetAngle) {
				angle = targetAngle;
			}
		}
	}

	void FixedUpdate() 
	{
		if (freezer == null)
			freezer = GameObject.Find ("Freezer").GetComponent<Freezer> ();
		if (freezer.frozen) 
		{
			rigidBody.velocity = new Vector3 (0, 0, 0);
		} else {
			Vector3 velocity = new Vector3 (Mathf.Cos (angle), 0, Mathf.Sin (angle));
			velocity *= linearSpeed;
			rigidBody.velocity = velocity;
		}
	}

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

	void OnTriggerEnter(Collider other) 
	{
		if (state == WhirlwindState.dying)
			return;

		if (other.tag != "Player")
			return;
		else
			parent.frozen = true;

		// tell the player to spawn shadow. Will return bool value
		// if player.spawnShadow() returns true, continue spawning the shadow
		//  otherwise, return doing nothing
		if (!other.gameObject.GetComponent<PlayerScript> ().spawnShadow ()) 
		{
			return;
		}

		string lvl = parent.level.locationName.Substring (0, 6);
		string key = lvl + "SpawnedShadows";
		int nShadows = parent.level.retrieveIntValue (key);

		int control, strength;
		control = shadowSpawnController.getControl ();
		strength = shadowSpawnController.getStrength ();

		if (nShadows < 4) {

			shadowStringBank = shadowStringBanks.bank [nShadows];
			redEyesStringBank = redEyesStringBanks.bank [nShadows];


			GameObject newShadow;
			newShadow = (GameObject)Instantiate (shadowPrefab, other.gameObject.transform.position + new Vector3(0, 0.0f, 0), Quaternion.Euler (0, 0, 0));

			// check spawn direction
			bool rightOK = false;
			bool leftOK = false;
			RaycastHit hit;
			rightOK = !Physics.Raycast (other.gameObject.transform.position, Vector3.right, 10.0f);
			leftOK = !Physics.Raycast (other.gameObject.transform.position, Vector3.left, 10.0f);

			if (leftOK && (!rightOK))
				newShadow.GetComponent<Shadow> ().spawnDirection = -1.0f;
			
			shadowStringBank.rosetta = rosetta;
			placesStringBank.rosetta = rosetta;
			datesStringBank.rosetta = rosetta;
			string sssId = shadowStringBank.getNextStringId ();
			string sss = shadowStringBank.getNextString ();
			string pl = placesStringBank.getNextStringId ();
			pl = placesStringBank.getNextString ();
			string dt = datesStringBank.getNextStringId ();
			dt = datesStringBank.getNextString ();

			shadowSpawnController.phrase.setText (sss);
			shadowSpawnController.phrase.fadeIn ();
			shadowSpawnController.phrase.fadeOut (5.0f);

			shadowSpawnController.dates.setText (dt);
			shadowSpawnController.dates.fadeIn ();
			shadowSpawnController.dates.fadeOut (5.0f);

			shadowSpawnController.places.setText (pl);
			shadowSpawnController.places.fadeIn ();
			shadowSpawnController.places.fadeOut (5.0f);


			if (sssId.Contains ("Familia")) {
				shadowSpawnController.iconAnimation [iconFamily].resetAnimation ();
				shadowSpawnController.iconFader[iconFamily].fadeIn ();
				shadowSpawnController.iconFader[iconFamily].fadeOut (5.0f);
			}
			if (sssId.Contains ("Amigos")) {
				shadowSpawnController.iconAnimation [iconFriends].resetAnimation ();
				shadowSpawnController.iconFader[iconFriends].fadeIn ();
				shadowSpawnController.iconFader[iconFriends].fadeOut (5.0f);
			}
			if (sssId.Contains ("Pareja")) {
				shadowSpawnController.iconAnimation [iconWork].resetAnimation ();
				shadowSpawnController.iconFader[iconCouple].fadeIn ();
				shadowSpawnController.iconFader[iconCouple].fadeOut (5.0f);
			}
			if (sssId.Contains ("Trabajo")) {
				shadowSpawnController.iconAnimation [iconWork].resetAnimation ();
				shadowSpawnController.iconFader[iconWork].fadeIn ();
				shadowSpawnController.iconFader[iconWork].fadeOut (5.0f);
			}
		

			newShadow.GetComponent<Shadow> ().thoughtTextPrefab = thoughtTextPrefab;
			newShadow.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
			newShadow.GetComponent<Shadow> ().initialize ();
			newShadow.GetComponent<Shadow> ().eyesPrefab = redeyesPrefab;
			redEyesStringBank.rosetta = rosetta;
			newShadow.GetComponent<Shadow> ().id = nShadows;
			newShadow.GetComponent<Shadow> ().redEyesStringBank = redEyesStringBank;
			newShadow.GetComponent<Shadow> ().rosetta = rosetta;

			shadowSpawnController.addShadow (newShadow.GetComponent<Shadow> ());

			parent.level.storeIntValue (key, ++nShadows);


			GameObject newCGaugeGO;
			GameObject newSGaugeGO;
			newCGaugeGO = (GameObject)Instantiate (shadowSpawnController.csGaugePrefab, other.gameObject.transform.position + new Vector3(-2.0f, 5.8f, 0), Quaternion.Euler (0, 0, 0));
			newSGaugeGO = (GameObject)Instantiate (shadowSpawnController.csGaugePrefab, other.gameObject.transform.position + new Vector3(2.0f, 5.8f, 0), Quaternion.Euler (0, 0, 0));
			ShadowPowerIndicator con = newCGaugeGO.GetComponent<ShadowPowerIndicator> ();
			ShadowPowerIndicator str = newSGaugeGO.GetComponent<ShadowPowerIndicator> ();
			con.initialize (true, control);
			str.initialize (false, strength);
			con.transform.SetParent(newShadow.transform);
			str.transform.SetParent (newShadow.transform);
			con.transform.localScale = new Vector3 (2, 2, 2);
			str.transform.localScale = new Vector3 (2, 2, 2);
		}

		this.state = WhirlwindState.dying;
	}
}
