using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

enum WellState { WaitingForVignette, ReadyToChangeLocation, Finished };

public class WellScript : Interactor {

	/* references */

	public GameObject lidPrefab;
	public StringBank wellStrings;
	//public StringBank phrase;
	public StringBankCollectionNoRepeat bank;
	public string wellID;
	public int redMana = 0;
	public int blueMana = 0;

	LevelControllerScript levelRef;
	MasterControllerScript mcRef;
	DataStorage ds;

	/* properties */

	GameObject lid;
	bool active = true;
	WellState state;

	//public string[] Sentence;

	new void Start () 
	{
		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = mcRef.getStorage ();
		state = WellState.WaitingForVignette;
		isWaitingForActionToComplete = true;
		rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();

		if (ds.retrieveBoolValue (wellID)) 
		{
			closeWell ();
		}

		levelRef = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
	}

	public void closeWell() 
	{
		lid = Instantiate (lidPrefab);
		lid.transform.position = this.transform.position + new Vector3 (0, -1.1f, 0);;
		lid.transform.rotation = Quaternion.Euler (0, this.transform.eulerAngles.y + 90, 0);
		lid.transform.localScale = new Vector3 (111, 111, 111);
		active = false;

		this.GetComponent<SphereCollider> ().enabled = false;
		interactEnabled = false;
	}
	
	new void Update () 
	{
		switch (state) 
		{

		case WellState.WaitingForVignette:
			if (!isWaitingForActionToComplete)
				state = WellState.ReadyToChangeLocation;
			break;

		case WellState.ReadyToChangeLocation:
			string loc = levelRef.locationName;
			GameObject player = GameObject.Find ("Player");
			ds.storeFloatValue ("Coords" + loc + "X", player.transform.position.x);
			ds.storeFloatValue ("Coords" + loc + "Y", player.transform.position.y);
			ds.storeFloatValue ("Coords" + loc + "Z", player.transform.position.z);
			ds.storeIntValue ("Orientation" + loc, player.GetComponent<PlayerScript> ().orientation ());
			ds.storeIntValue ("BlueManaObtained", blueMana);
			ds.storeIntValue ("RedManaObtained", redMana);
			int blue = ds.retrieveIntValue ("BlueMana");
			blue += blueMana;
			ds.storeIntValue ("BlueMana", blue);
			int red = ds.retrieveIntValue ("RedMana");
			red += redMana;
			ds.storeIntValue ("RedMana", red);
//			mcRef.selectMixer (0);
//			mcRef.setVolume (0.0f, 5);
//			mcRef.selectMixer (2);
//			mcRef.playMusic (2);
//			mcRef.setVolume (1.0f, 5);
			SceneManager.LoadScene ("Scenes/SentenceOrdering");
			state = WellState.Finished;
			break;

		default:
			break;
		}
	}

	public void _wm_pushSentenceBitsToMasterController() 
	{
		mcRef.resetSentencePiecesList ();
		StringBank sb = bank.getNextBank();
		for (int i = 0; i < sb.nItems(); ++i) {

			//mcRef.addSentencePiece (rosetta.retrieveString(phrase.getStringId(i)));
			mcRef.addSentencePiece (rosetta.retrieveString (sb.getStringId(i)));
		}
	}

	override public void effect() 
	{
		if (active) 
		{
			this._wm_pushSentenceBitsToMasterController ();		   // código esté en el 'other'
			mcRef.getStorage().storeStringValue("CurrentWell", wellID);
			levelRef.mcRef.saveLocation (levelRef.locationName);   // script
			VignetteScript vignetteObject = GameObject.Find ("Vignette").GetComponent<VignetteScript> ();
			this.isWaitingForActionToComplete = true;
			vignetteObject._wa_close (this);
		} else {
			//levelRef._wm_alert (rosetta.retrieveString(wellStrings.getStringId(0)));
		}
	}

	override public string interactIcon() 
	{
		return "Eye";
	}
}
