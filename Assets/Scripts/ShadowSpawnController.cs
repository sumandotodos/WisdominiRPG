using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ShadowSpawnController : WisdominiObject {

	/* references */

	public Mirror mirror;

	MasterControllerScript master;
	DataStorage ds;

	List<Shadow> shadowList;

	public TextFader dates;
	public TextFader places;
	public TextFader phrase;
	public string level;
    public GameObject shadowPrefab;
	public LevelControllerScript levelRef;
	public ImageFader[] iconFader;
	public UIImageAnimator[] iconAnimation;
	public Freezer freezer;
	public PlayerScript player;
	public CameraManager camera;

	public string[] waveType;
	int[] waveIndexes;
	public int waveNumber;

	public GameObject csGaugePrefab;

	int control;
	int strength;

	const int minControl = 1;
	const int minStrength = 1;
	const int csRange = 3;
	const int levelCost = 2;
	const float LastShadowSpawnDelay = 6.0f;

	/* properties */

	public int state = 0; // state for slot 0
	public int state2 = 0; // state for slot 1 (lapse from last shadow spawn to lerping the camera)
	int shadowIndex;
	float elapsedTime; // elapsed time for slot 0
	float elapsedTime2; // elapsed time for slot 1

	Vector3 initialPos;
	Vector3 intermediatePos;
	Vector3 finalPos1;
	Vector3 finalPos2;
	Vector3 finalPos3;
	Vector3 finalPos4;
	Vector3 mirrorPos;

	/* constants */

	public const float offScreenRadius = 30.0f;
	const float onScreenRadius = 12.0f;
	public const int maxShadows = 4;
	const float enterTime = 0.75f;
	const float timeOut = 5.0f; // 5 seconds max!

	new void Start () 
	{
		shadowList = new List<Shadow> ();
		state = 0;

		waveIndexes = new int[waveType.Length];

		master = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = master.getStorage ();

		camera = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		levelRef = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		level = levelRef.locationName.Substring (0, 6);

        string lvl = levelRef.locationName.Substring(0, 6);
        string key = lvl + "SpawnedShadows";
        int nShadows = levelRef.retrieveIntValue(key);

        for(int i = 0; i < nShadows; ++i)
        {
            //spawn shadows here
            Debug.Log("<color=red>Should spawn " + nShadows + " shadows</color>");
        }

        waveNumber = ds.retrieveIntValue (level + "ShadowWaveNumber");
		if (waveNumber > waveType.Length) 
		{ // already cleared
			mirror.deplete();
		}

		else if (waveNumber == 0) { // first time here

			// randomize order of waves
			List<int> selectedIntegers = new List<int> ();
			for (int i = 0; i < waveType.Length; ++i) {
				int rand = Random.Range (0, waveType.Length);
				while (selectedIntegers.Contains (rand)) {
					rand = Random.Range (0, waveType.Length);
				}
				selectedIntegers.Add (rand);
				waveIndexes [i] = rand;
				ds.storeIntValue (level + "ShadowWaveIndex" + i, rand);
			}

			waveNumber = 1;
			ds.storeIntValue (level + "ShadowWaveNumber", 1);

		} else { 
			for (int i = 0; i < waveType.Length; ++i) 
			{
				waveIndexes [i] = ds.retrieveIntValue (level + "ShadowWaveIndex" + i);
			}
		}

		ds.storeStringValue ("WordFightRequiredWisdom", waveType[waveIndexes[waveNumber-1]]);

		control = Random.Range (minControl, minControl + csRange);
		strength = Random.Range (minStrength, minStrength + csRange);

		/*
		int reentryShadows = ds.retrieveIntValue ("ReentryShadows");
		for (int i = 0; i < reentryShadows; ++i) {
			float x, y, z;
			x = ds.retrieveFloatValue ("Shadow"+i+"Coords" + levelRef.locationName + "X");
			y = ds.retrieveFloatValue ("Shadow"+i+"Coords" + levelRef.locationName + "Y");
			z = ds.retrieveFloatValue ("Shadow"+i+"Coords" + levelRef.locationName + "Z");
			int orientation;
			orientation = ds.retrieveIntValue ("Shadow"+i+"Orientation");
			//setOrientation (orientation);
			if ((x != 0.0f) && (y != 0.0f) && (z != 0.0f)) {
				Vector3 newPos = new Vector3 (x, y, z);
				this.transform.position = newPos;
			}
		}*/
	}

	public int _wm_numberOfWaves() 
	{
		return waveType.Length;
	}

	public int getWaveIndex() 
	{
		return waveIndexes[waveNumber-1];
	}

	public void addShadow(Shadow s) 
	{
		s.shadowWaveInt = waveIndexes[waveNumber-1];
		s.shadowWaveStr = waveType [waveIndexes[waveNumber - 1]];
		if (shadowList == null)
			shadowList = new List<Shadow> ();
		shadowList.Add (s);
		if (shadowList.Count == maxShadows)
		{
			state2 = 1;
		}
	}

	public Shadow getShadow(int index)
	{
		if(index < shadowList.Count) {
			return shadowList[index];
		}
		else return null;
	}

	public int nShadows() 
	{
		return shadowList.Count;
	}

	public int getControl() 
	{
		return control;
	}

	public int getStrength() 
	{
		return strength;
	}

	public void callShadows(Vector3 mPos) 
	{
		/*
		Vector3 vec = new Vector3 (0, 0, -1);

		float arc = 45.0f;

		Vector3 startVec, stopVec;

		for (int i = 0; i < shadowList.Count; ++i) {
			startVec = Quaternion.AngleAxis (-arc/2.5f + i*arc/shadowList.Count, Vector3.up) * (vec * offScreenRadius);
			stopVec = Quaternion.AngleAxis (-arc/2.5f + i*arc/shadowList.Count, Vector3.up) * (vec * onScreenRadius);
			Vector3 shadowStartPos = mirrorPos + startVec;
			Vector3 shadowStopPos = mirrorPos + stopVec;
			shadowList [i].mirrorConverge (shadowStartPos, shadowStopPos, mirrorPos);
		}

		Vector3 playerTarget = vec * (onScreenRadius / 2.0f);
		// tell player to go in front of mirror, facing it
		player.blockControls ();
		player.r.isKinematic = true;
		player.agent.enabled = true;
		player.agent.destination = mirrorPos + playerTarget;
		player.directionFromVector (player.agent.destination, false);
		player.state = PlayerState.seekingMirrorPoint1;

		elapsedTime = 0.0f;

		state = 1;
		*/

		mirrorPos = mPos;
		initialPos = mirrorPos + new Vector3 (0, -2, -22);
		intermediatePos = mirrorPos + new Vector3 (0, 0, -11.5f);
		finalPos1 = mirrorPos + new Vector3 (-5, 0, -10);
		finalPos2 = mirrorPos + new Vector3 (-1.75f, 0, -10);
		finalPos3 = mirrorPos + new Vector3 (1.75f, 0, -10);
		finalPos4 = mirrorPos + new Vector3 (5, 0, -10);

		state = 7;

		elapsedTime = 0.0f;

		/*shadowList [0].summon (initialPos, intermediatePos, finalPos1, mirrorPos);
		shadowList [1].summon (initialPos, intermediatePos, finalPos2, mirrorPos);
		shadowList [2].summon (initialPos, intermediatePos, finalPos3, mirrorPos);
		shadowList [3].summon (initialPos, intermediatePos, finalPos4, mirrorPos);
*/
	}

	public void enterMirror() 
	{
		shadowIndex = 0;
		state = 2;
		elapsedTime = 0.0f;
	}


	new void Update() 
	{
		if (state2 == 0) { // slot 1, state 0: idling

		}
		if (state2 == 1) { // slot 1, state 1: initialize delay
			elapsedTime2 = 0.0f;
			++state2;
		}
		if (state2 == 2) { // slot 1, state 2: delaying
			elapsedTime2 += Time.deltaTime;
			if (elapsedTime2 > LastShadowSpawnDelay) {
				++state2;
			}
		}
		if (state2 == 3) { // slot 1, state 3: block the player and lerp the camera to the dark mirror
			freezer._wm_freeze();
			player.blockControls ();
			camera._wa_moveToMarker (this, 0);
			this.isWaitingForActionToComplete = true;
			this.state = 3;
			++state2;
		}

		if (state == 0) {
			//if (Input.GetMouseButtonDown (1))
			//	enterMirror (); // guarrería para probar

		}
		if (state == 1) { // waiting for shadows to approach

			elapsedTime += Time.deltaTime;

			int ok = 0;
			for (int i = 0; i < shadowList.Count; ++i) 
			{
				if (shadowList [i].hasConverged ())
					++ok;
			}
			if (ok == shadowList.Count) 
			{
				enterMirror ();
			}

			if (elapsedTime > timeOut) 
			{
				for (int i = 0; i < shadowList.Count; ++i)
				{
					//shadowList [i].navAgent.Warp (shadowList [i].navAgent.destination);
				}
				enterMirror ();
			}
		}

		if (state == 2) { // entering mirror
			elapsedTime += Time.deltaTime;
			if (elapsedTime > enterTime) {
				elapsedTime = 0.0f;
				shadowList [shadowIndex++].enterMirror ();
				if (shadowIndex == shadowList.Count) {
					state = 0;
					shadowIndex = 0;
					elapsedTime = 0.0f;
				}
			}
		}

		if (state == 3) { // waiting for camera to lerp to waypoint
			if(isWaitingForActionToComplete) return;
			elapsedTime = 0.0f;
			++state;
		}

		if (state == 4) { // waiting for mirror to activate
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 0.0f) {
				mirror.activate ();
				elapsedTime = 0.0f;
				++state;
			}
		}

		if (state == 5) { // waiting for camera to lerp back to player
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 4.0f) {
				camera._wa_moveToOriginalPosition (this);
				this.isWaitingForActionToComplete = true;
				++state;
			}
		}

		if (state == 6) {
			if (isWaitingForActionToComplete)
				return;
			freezer._wm_unfreeze ();
			player.unblockControls ();
			state = 0;
		}

		if (state == 7) 
		{
			shadowList[0].summon(initialPos, intermediatePos, finalPos1, mirrorPos, 0.0f);
			++state;
		}

		if (state == 8) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 0.75f) {
				elapsedTime = 0.0f;
				shadowList[1].summon(initialPos, intermediatePos, finalPos2, mirrorPos, 0.0f);
				++state;
			}
		}

		if (state == 9) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 0.75f) {
				elapsedTime = 0.0f;
				shadowList[2].summon(initialPos, intermediatePos, finalPos3, mirrorPos, 0.0f);
				++state;
			}
		}

		if (state == 10)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 0.75f) {
				elapsedTime = 0.0f;
				shadowList[3].summon(initialPos, intermediatePos, finalPos4, mirrorPos, 0.0f);
				++state;
			}
		}
	}
}
