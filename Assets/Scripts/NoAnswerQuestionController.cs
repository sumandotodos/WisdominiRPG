using UnityEngine;
using System.Collections;

public class NoAnswerQuestionController : WisdominiObject {

	/* references */

	public MasterControllerScript mcRef;
	new public Rosetta rosetta;
	public PlayerScript player;
	public StringBankCollection stringBankCol;
	public GameObject noAnswerQuestionPrefab;
	public GameObject floaterPrefab;
	DataStorage ds;
	StringBank bank;
	HeavyLevitatingFollower [] floater;
	int questionNumber;

	/* properties */

	int visitNumber;

	// Use this for initialization
	new void Start () {
	
		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();
		ds = mcRef.getStorage ();
		visitNumber = ds.retrieveIntValue ("TopFloorVisitNumber");

		bank = stringBankCol.getBank (visitNumber);
		bank.rosetta = rosetta;
		bank.reset ();

		int nFloaters;

		nFloaters = bank.nItems ();
		floater = new HeavyLevitatingFollower[nFloaters];

		for (int i = 0; i < nFloaters; ++i) {

			Vector3 newPos = player.transform.position;
			newPos.x -= 5 * i;
			GameObject newFloater = (GameObject)Instantiate (floaterPrefab, newPos, Quaternion.Euler (0, 0, 0));
			floater [i] = newFloater.GetComponent<HeavyLevitatingFollower> ();
			floater [i].player = player;
			floater [i].nearRadius = 2.0f + i * 1.0f;
			floater [i].initialize ();

		}

		questionNumber = 0;

	}
	
	// Update is called once per frame
	new void Update () {
	
	}

	public void _wm_spawnNoAnswerQuestion() {

		Vector3 playerPos = player.transform.position;

		playerPos.y = 2.5f;

		GameObject newNAQGO = (GameObject)Instantiate (noAnswerQuestionPrefab, playerPos+new Vector3(-1, 0.3f, -1.3f), Quaternion.Euler (50.0f, 0, 0));
		NoAnswerQuestion newNAQ = newNAQGO.GetComponent<NoAnswerQuestion> ();
		string nextStr = bank.getNextStringId ();
		nextStr = rosetta.retrieveString (nextStr);
        newNAQ.transform.localScale = 0.2f * Vector2.one;
		newNAQ.initialize ();
		newNAQ.setText (nextStr);
		newNAQ.setAutoTransitionOut (3.0f + nextStr.Length * 0.05f);
		newNAQ.transitionIn ();


		floater [questionNumber++].GetComponent<MeshRenderer> ().enabled = false;

	}
}
