using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum InterconNetworkState { busy, relaxed, driftingAway, away }

public class interconNetwork : MonoBehaviour {

	//Inspector watch
	public float angle;
	public float targetAngle;

	/* references */

	MasterControllerScript master;
	public Freezer freezer;
	public GameObject shadowPrefab;
	public GameObject whirlwindPrefab;
	public GameObject player;
	public Rosetta rosetta;
	Rigidbody rigidBody;
	public GameObject thoughtTextPrefab;
	public MasterControllerScript mcRef;
	public LevelControllerScript level;
	[HideInInspector]
	public DataStorage ds;

	public Sprite FamilyIcon;
	public Sprite CoupleIcon;
	public Sprite FriendsIcon;
	public Sprite WorkIcon;

	/* string banks */

	public StringBank places;
	public StringBank dates;

	/* string banks collections */

	public StringBankCollection[] HeroShadows;
	public StringBankCollection[] HeroRedeyes;

	public ShadowSpawnController shadowSpawnController;

	/* public properties */

	public bool frozen;

	/* properties */

	float timeToWhirl;
	float elapsedTime;
	float whirlDifficulty = 1.0f;

	float linearSpeed;
	float targetLinearSpeed;
	float timeToChasePlayer;
	float elapsedChasingTime;
	float elapsedVeeringTime;
	float timeToVeer;
	float elapsedTimeToVeer;
	public InterconNetworkState state;

	bool deletedWhirlwinds = false;

	/* constants */

	const float minTimeToWhirl = 0.5f;
	const float maxTimeToWhirl = 8.0f;
	public float minLinearSpeed = 1.0f;
	public float maxLinearSpeed = 3.0f;
	const float angleSpeed = 8.0f;
	const float minTimeToChasePlayer = 1.0f;
	const float maxTimeToChasePlayer = 4.0f;
	const int percentChanceOfChasing = 40;

	public float height = 3.3f;

	/* methods */

	public void initialize()
	{
		if (level == null) {
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		}
		if (player == null) {
			player = GameObject.Find ("Player");
		}
		whirlDifficulty = 1.0f;
		elapsedTime = 0.0f;
		targetAngle = angle = (floatRandomRange(0.0f, 360.0f) / 360.0f) * 2.0f * 3.1416f;
		timeToWhirl = floatRandomRange (minTimeToWhirl/whirlDifficulty, maxTimeToWhirl/whirlDifficulty);
		timeToChasePlayer = floatRandomRange (minTimeToChasePlayer, maxTimeToChasePlayer);
		timeToVeer = floatRandomRange (minTimeToChasePlayer, maxTimeToChasePlayer);
		frozen = false;
		targetLinearSpeed = linearSpeed = maxLinearSpeed;
		rigidBody = this.GetComponent<Rigidbody> ();
		elapsedChasingTime = 0.0f;
		elapsedVeeringTime = 0.0f;
		rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();
		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = mcRef.getStorage ();

		string lvl = level.locationName.Substring (0, 6);
		string key = lvl + "SpawnedShadows";
		ds.storeIntValue (key, 0);

		int waveNumber = ds.retrieveIntValue (lvl + "ShadowWaveNumber");
		if (waveNumber > shadowSpawnController.waveType.Length) 
		{
			Destroy (this.gameObject);
		}

		state = InterconNetworkState.busy;
		if (freezer == null) {
			freezer = GameObject.Find ("Freezer").GetComponent<Freezer> ();
		}

	}

	void Start () 
	{
		initialize ();
		whirlDifficulty = 4.0f;
	}
		
	void Update ()
	{
		if (freezer.frozen)
			return;

		if(!frozen) elapsedTime += Time.deltaTime;

		/* busy: spawning whilwinds */
		if (state == InterconNetworkState.busy) 
		{ // pursuing player		
			elapsedVeeringTime += Time.deltaTime;
			if (elapsedVeeringTime > timeToVeer) 
			{
				elapsedVeeringTime = 0.0f;
				//int r = Random.Range (0, 100);

				Vector3 pPos = player.transform.position;
				Vector3 tPos = this.transform.position;
				Vector3 delta = pPos - tPos;
				delta.Normalize ();
				targetAngle = Mathf.Acos (delta.x);
				if (delta.z < 0.0)
					targetAngle = (2.0f * 3.1416f) - targetAngle;

				linearSpeed = floatRandomRange (minLinearSpeed, maxLinearSpeed);

				Debug.Log ("<color=orange>Target Angle updated: " + targetAngle + "</color>");
			}

			if (elapsedTime > timeToWhirl) 
			{
				elapsedTime = 0.0f;
				GameObject newWhirl = (GameObject)Instantiate (whirlwindPrefab, this.transform.position, Quaternion.Euler (0, 0, 0));
				newWhirl.name = "Whirlwind";
				newWhirl.GetComponent<Whirlwind> ().thoughtTextPrefab = thoughtTextPrefab;
				newWhirl.GetComponent<Whirlwind> ().parent = this;
				newWhirl.GetComponent<Whirlwind> ().shadowPrefab = shadowPrefab;
				newWhirl.GetComponent<Whirlwind> ().rosetta = rosetta;
				newWhirl.GetComponent<Whirlwind> ().FamilyIcon = FamilyIcon;
				newWhirl.GetComponent<Whirlwind> ().CoupleIcon = CoupleIcon;
				newWhirl.GetComponent<Whirlwind> ().FriendsIcon = FriendsIcon;
				//newWhirl.GetComponent<Whirlpool> ().Start ();
				newWhirl.GetComponent<Whirlwind> ().WorkIcon = WorkIcon;
				int wave = shadowSpawnController.getWaveIndex ();

				newWhirl.GetComponent<Whirlwind> ().shadowStringBanks = HeroShadows[wave];
				newWhirl.GetComponent<Whirlwind> ().redEyesStringBanks = HeroRedeyes[wave];
				newWhirl.GetComponent<Whirlwind> ().shadowSpawnController = shadowSpawnController;
				newWhirl.GetComponent<Whirlwind> ().placesStringBank = places;
				newWhirl.GetComponent<Whirlwind> ().datesStringBank = dates;

				timeToWhirl = floatRandomRange (minTimeToWhirl / whirlDifficulty, maxTimeToWhirl / whirlDifficulty);
			}

			if (level == null) {
				level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
			}
			string lvl = level.locationName.Substring (0, 6);
			int nShadows = level.retrieveIntValue (lvl + "SpawnedShadows");

			if (nShadows >= ShadowSpawnController.maxShadows)
				state = InterconNetworkState.driftingAway;
		}

		if (state == InterconNetworkState.driftingAway) 
		{ // pursuing player
			if (!deletedWhirlwinds) 
			{
				if (GameObject.Find ("Whirlwind") != null) {
					Destroy (GameObject.Find ("Whirlwind"));
				} else {
					deletedWhirlwinds = true;
				}
			}

			elapsedVeeringTime += Time.deltaTime;
			if (elapsedVeeringTime > timeToVeer) 
			{
				elapsedVeeringTime = 0.0f;

				//int r = Random.Range (0, 100);

				Vector3 pPos = player.transform.position;
				Vector3 tPos = this.transform.position;
				Vector3 delta = -(pPos - tPos); // flee from player
				delta.Normalize ();
				targetAngle = Mathf.Acos (delta.x);
				if (delta.z < 0.0)
					targetAngle = (2.0f * 3.1416f) - targetAngle;

				linearSpeed = floatRandomRange (minLinearSpeed, maxLinearSpeed);
			}
		}

		if (state == InterconNetworkState.relaxed) 
		{ // just float by...
			elapsedVeeringTime += Time.deltaTime;
			if (elapsedVeeringTime > timeToVeer)
			{
				elapsedVeeringTime = 0.0f;

				//int r = Random.Range (0, 6.28f);
				float newAngle = FloatRandom.floatRandomRange(0, 6.28f);

				targetAngle = newAngle;

				linearSpeed = floatRandomRange (minLinearSpeed, maxLinearSpeed);
			}
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
	}

	void FixedUpdate()
	{
		if (frozen) 
		{
			//rigidBody.velocity = new Vector3 (0, 0, 0);
		}
		else 
		{
			// 
			float yCoord = height;
			RaycastHit[] hits = Physics.RaycastAll (this.transform.position, Vector3.down);
			for (int k = 0; k < hits.Length; ++k) {
				if (hits [k].collider.tag == "Ground") {
					yCoord = hits [k].point.y + height; // float above ground at constant height
				}
			}

			Vector3 velocity = new Vector3 (Mathf.Cos (angle), 0, Mathf.Sin (angle));
			velocity *= linearSpeed;
			Vector3 pos = this.transform.position;
			Vector3 newPos = pos + velocity * Time.deltaTime;
			newPos.y = yCoord;
			this.transform.position = newPos;
			//rigidBody.velocity = velocity;
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
}
