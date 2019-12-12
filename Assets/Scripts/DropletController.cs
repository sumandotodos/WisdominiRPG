using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropletController : WisdominiObject {

	public float followerYOffset;
	public GameObject[] waterDroplets;
	public GameObject noAnswerQuestionPrefab;
	public StringBankCollection stringBanks;
	public LevelControllerScript level;
	public string variableName = "NTimesLevel1Heaven";
	public string whirlpoolName;
	HeavyLevitatingFollower [] floater;
	public GameObject floaterPrefab;
	List<bool> dropletAlive;
	public Whirlpool wpool;

	int visitNumber;

	int actualNumberOfDroplets; // waterDroplets holds a MAXIMUM
								// this is the ACTUAL number of dropelts
	int usedQuestions = 0;

	StringBank currentStringBank;

	new void Start () 
	{

		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		dropletAlive = new List<bool> ();
		visitNumber = level.retrieveIntValue (variableName);

		visitNumber = (visitNumber % stringBanks.bank.Length);

		currentStringBank = stringBanks.bank [visitNumber];
	
		actualNumberOfDroplets = currentStringBank.nItems ();

		for (int i = 0; i < actualNumberOfDroplets; ++i) 
		{
			dropletAlive.Add (true);
		}
		for (int i = actualNumberOfDroplets; i < waterDroplets.Length; ++i) 
		{
			dropletAlive.Add (false);
		}

		for (int i = actualNumberOfDroplets; i < waterDroplets.Length; ++i) 
		{
			//Destroy (waterDroplets [i]); // get rid of unused droplets
			waterDroplets[i].SetActive(false);
		}

		currentStringBank.rosetta = level.rosetta;
		currentStringBank.reset ();

		for (int i = 0; i < waterDroplets.Length; ++i) 
		{
			bool b = level.retrieveBoolValue ("PickedUpDroplet" + (i + 1));
			if (b)
				dropletAlive [i] = false;
		}

		int nFloaters;

		nFloaters = currentStringBank.nItems ();
		floater = new HeavyLevitatingFollower[nFloaters];

		int pickedUpDroplets = level.retrieveIntValue ("Droplets");

		for (int i = 0; i < nFloaters; ++i) {

			Vector3 newPos = level.player.transform.position - Vector3.right * 100;
			newPos.x -= 5 * i;
			GameObject newFloater = (GameObject)Instantiate (floaterPrefab, newPos, Quaternion.Euler (0, 0, 0));
			floater [i] = newFloater.GetComponent<HeavyLevitatingFollower> ();
			floater [i].player = level.player;
			floater [i].nearRadius = 2.0f + i * 2.0f;
			floater [i].initialize ();
			floater [i].yOffset = followerYOffset;
			if (i < pickedUpDroplets)
				floater [i].immediateBreak ();
		}
		usedQuestions = pickedUpDroplets;
	}

	public void indexAndDistanceToClosestDroplet(Vector3 loc, out int index, out float distance) 
	{
		if (usedQuestions == actualNumberOfDroplets) 
		{
			index = -1;
			distance = 0.0f;
			return;
		}
			
		int firstDroplet = 0;
		int minIndex = 0;
		float minDist;

		while (!dropletAlive [firstDroplet])
			++firstDroplet;

		minDist = (waterDroplets [firstDroplet].transform.position - loc).magnitude;
		minIndex = firstDroplet;

		for (int i = firstDroplet+1; i < waterDroplets.Length; ++i) {

			if (dropletAlive [i]) 
			{
				float dist = (waterDroplets [i].transform.position - loc).magnitude;
				if (dist < minDist) {
					minDist = dist;
					minIndex = i;
				}
			}
		}
		index = minIndex;
		distance = minDist;
	}

	public int indexOfClosestDroplet(Vector3 loc) 
	{
		if (usedQuestions == actualNumberOfDroplets) 
		{
			return -1;
		}

		int firstDroplet = 0;
		int minIndex = 0;
		float minDist;

		while (!dropletAlive [firstDroplet])
			++firstDroplet;

		minDist = (waterDroplets [firstDroplet].transform.position - loc).magnitude;
		minIndex = firstDroplet;

		for (int i = firstDroplet+1; i < waterDroplets.Length; ++i) 
		{
			if (dropletAlive [i]) 
			{
				float dist = (waterDroplets [i].transform.position - loc).magnitude;
				if (dist < minDist) {
					minDist = dist;
					minIndex = i;
				}
			}
		}
		return minIndex;
	}

	public float distanceToClosestDroplet(Vector3 loc)
	{
		if (usedQuestions == actualNumberOfDroplets) 
		{
			return 0.0f;
		}

		int firstDroplet = 0;
		int minIndex = 0;
		float minDist;

		while (!dropletAlive [firstDroplet])
			++firstDroplet;

		minDist = (waterDroplets [firstDroplet].transform.position - loc).magnitude;
		minIndex = firstDroplet;

		for (int i = firstDroplet+1; i < waterDroplets.Length; ++i) {

			if (dropletAlive [i]) {

				float dist = (waterDroplets [i].transform.position - loc).magnitude;
				if (dist < minDist) {
					minDist = dist;
					minIndex = i;
				}
			}
		}
		return minDist;
	}

	public Vector2 droplet2DPosition(int index) 
	{
		return new Vector2 (waterDroplets [index].transform.position.x, waterDroplets [index].transform.position.z);
	}

	public bool isDropletAlive(int n)
	{
		if (n >= dropletAlive.Count)
			return false;
		return dropletAlive [n];
	}

	public void _wm_spawnNoAnswerQuestion(int dropNum) 
	{
		level.storeBoolValue ("PickedUpDroplet" + dropNum, true);
		++usedQuestions;
		int drplt = level.retrieveIntValue ("Droplets");
		level.storeIntValue ("Droplets", drplt + 1);

		dropletAlive[dropNum-1] = false;

		Vector3 playerPos = level.player.transform.position;

		playerPos.y += 2.5f;

		GameObject newNAQGO = (GameObject)Instantiate (noAnswerQuestionPrefab, playerPos+new Vector3(-1, 0.3f, -1.3f), Quaternion.Euler (50.0f, 0, 0));
		NoAnswerQuestion newNAQ = newNAQGO.GetComponent<NoAnswerQuestion> ();
		string nextStr = currentStringBank.getStringId (drplt);
		nextStr = level.rosetta.retrieveString (nextStr);
		newNAQ.initialize ();
        newNAQ.transform.localScale = 0.2f * Vector2.one;
        newNAQ.setText (StringUtils.chopLines(nextStr, 25));
		newNAQ.setAutoTransitionOut (3.0f + nextStr.Length * 0.05f);
		newNAQ.transitionIn ();

		if (usedQuestions == actualNumberOfDroplets) 
		{
			level.storeBoolValue (whirlpoolName, true);
			//if (level.retrieveBoolValue ("HasAlphabet")) {
				wpool.enable ();
			//}
		}
		if (usedQuestions <= floater.Length) {
			floater [usedQuestions - 1].Break ();
		}
	}

	public bool _wm_allDroplets() {
		return usedQuestions == actualNumberOfDroplets;
	}
}
