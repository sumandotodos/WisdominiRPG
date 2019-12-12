using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FerfufloController : WisdominiObject {

	/* references */

	new public Rosetta rosetta;
	public DiceRollController rollController;
	public GameObject[] alphaCurtains;
	public TextMesh assertion;
	public TextMesh transmission;
	public TextMesh answer;
	public GameObject ads;
	public GameObject chip;
	Animator adsAnim;
	DataStorage ds;
	GameObject currentAlphaCurtain;
	//public StringBankCollection[] stringBanks;
	public StringBank[] sabias;
	public StringBank[] ignorantes;
	public StringBank transmissionBank;
	public StringBank propositionBank;
	//public Text t;
	public Image whiteHalo;
	public Image blackHalo;
	public GameObject whiteCard, blackCard;
	public UIFaderScript fader;
	public GameObject TV;

	List<string> thisBatchVariables;

	/* properties */

	float elapsedTime;
	bool chipPicked;
	bool chipInserted;

	float currentCurtainPosition;
	float targetCurtainPosition;

	Vector3 chipCapturePos;
	Vector3 chipRelaxPos;
	Vector3 chipCurrentPos;

	//StringBank propositionBank;
	//StringBank wiseAnswers;
	StringBank lameAnswers;

	float chipScale;

	public int state;

	/* public properties */

	public float curtainSpeed = 6.0f;
	[HideInInspector]
	public bool chipOnWhite, chipOnBlack;
	float opacity;

	/* constants */

	const float leftClosedCurtain = 19.0f;
	const float rightClosedCurtain = 90.0f;
	const float openCurtain = 45.0f;
	const float smallDelay = 0.3f;
	const float opacitySpeed = 6.0f;

	MasterControllerScript mc;

	public void reset() {
		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();
		currentAlphaCurtain = alphaCurtains [0];
		currentCurtainPosition = targetCurtainPosition = rightClosedCurtain;
		currentAlphaCurtain.transform.position = new Vector3 (currentCurtainPosition, 
			currentAlphaCurtain.transform.position.y, 
			currentAlphaCurtain.transform.position.z);

		amountRequired = mc.getStorage ().retrieveIntValue ("TVFerfuflosAmount");

		adsAnim = ads.GetComponent<Animator> ();
		// do not always start at the beginning...
		adsAnim.SetFloat("offset", FloatRandom.floatRandomRange (0.0f, 1.0f));

		answer.text = "";

		state = 0;
		chipScale = 1.0f;
		opacity = 0.0f;

		whiteCard.GetComponent<Image>().color = new Color (1, 1, 1, 0);
		blackCard.GetComponent<Image> ().color = new Color (1, 1, 1, 0);
		chip.GetComponent<Image> ().color = new Color (1, 1, 1, 0);
		whiteHalo.color = new Color (1, 1, 1, 0);
		blackHalo.color = new Color (1, 1, 1, 0);
		answer.GetComponent<Renderer> ().material.SetColor ("_Tint", new Color (1, 1, 1, 0));
		assertion.GetComponent<Renderer> ().material.SetColor ("_Tint", new Color (1, 1, 1, 0));
		transmission.GetComponent<Renderer> ().material.SetColor ("_Tint", new Color (1, 1, 1, 0));


		chipOnWhite = chipOnBlack = false;

		//
		//propositionBank = stringBanks [0].bank [0];

		//wiseAnswers = stringBanks [0].bank [2];
		/* just in case */
		propositionBank.rosetta = rosetta;
		propositionBank.reset ();

		//wiseAnswers.rosetta = rosetta;

		transmissionBank.rosetta = rosetta;
		transmissionBank.reset ();

		//propositionBank.reset ();

		//wiseAnswers.reset ();

		chipInserted = false;

		// update number of screen id usage


		foreach (StringBank sb in sabias) {
			sb.rosetta = rosetta;
			sb.reset ();
		}

		foreach (StringBank sb in ignorantes) {
			sb.rosetta = rosetta;
			sb.reset ();
		}

		chip.transform.position = chipRelaxPos;
		chipCurrentPos = chipRelaxPos;
		chip.transform.localScale = Vector3.one;
	}

	new void Start () 
	{
		
		chipRelaxPos = chipCurrentPos = chip.transform.position;
		thisBatchVariables = new List<string> ();
		reset ();
		string id = mc.getStorage ().retrieveStringValue ("TVFerfuflosId");
		mc.getStorage ().storeIntValue ("TVFerfuflosTimes" + id, 0);

	}

	int diceResult;
	int sabiaOIgnorante; // los "tres números cuánticos" de cada test
	int cual;

	int nTimes;
	int amountRequired;

	new void Update ()
	{
		float relX;//, relY;
		relX = chipCurrentPos.x / Screen.width;
		//relY = chipCurrentPos.y / Screen.height;
		string currentCard;
		currentCard = chip.GetComponent<ChipTrigger>().SayName();

		if (currentCard == "WhiteCard") {
			whiteHalo.GetComponent<Glower> ().glow ();
			chipOnWhite = true;
		} else {
			whiteHalo.GetComponent<Glower> ().reglow ();
			chipOnWhite = false;
		}
		if (currentCard == "BlackCard") {
			blackHalo.GetComponent<Glower> ().glow ();
			chipOnBlack = true;
		} else {
			blackHalo.GetComponent<Glower> ().reglow ();
			chipOnBlack = false;
		}

		/*if ((relX > 0.89f) && (relX <= 0.965f) &&!chipInserted) {
			whiteHalo.color = new Color (1, 1, 1, 1);
			chipOnWhite = true;
		} else {
			whiteHalo.color = new Color (1, 1, 1, 0);
			chipOnWhite = false;
		}

		if ((relX > 0.04f) && (relX <= 0.10f) &&!chipInserted) {
			blackHalo.color = new Color (1, 1, 1, 1);
			chipOnBlack = true;
		} else {
			blackHalo.color = new Color (1, 1, 1, 0);
			chipOnBlack = false;
		}*/

		if (chipPicked) 
		{
			chip.transform.position = chipCurrentPos = chipRelaxPos + (Input.mousePosition - chipCapturePos);
		} else {
			if (chipCurrentPos != chipRelaxPos) {
				Vector3 dir = chipRelaxPos - chipCurrentPos;
				dir.Normalize ();
				dir *= 36.0f;
				chipCurrentPos += dir;
				// check if we overpast the target
				Vector3 checkDir = chipRelaxPos - chipCurrentPos;
				if (Vector3.Dot (checkDir, dir) < 0.0f) {
					chipCurrentPos = chipRelaxPos;
				}
				chip.transform.position = chipCurrentPos;
			}
		}

		if (chipInserted) 
		{
			chipScale -= 6.0f * Time.deltaTime;
			if (chipScale < 0.0f)
				chipScale = 0.0f;
			chip.transform.localScale = new Vector3 (chipScale, chipScale, chipScale);
		}

		/* wait for touch */
		if (state == 0) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > smallDelay) {
				
				++state;

				string id = mc.getStorage ().retrieveStringValue ("TVFerfuflosId");
				nTimes = mc.getStorage ().retrieveIntValue ("TVFerfuflosTimes" + id);
				mc.getStorage ().storeIntValue ("TVFerfuflosTimes" + id, nTimes + 1);

			}
			//if (Input.GetMouseButtonDown (0))
			//	++state;
		}	
		if (state == 1) 
		{
			int testStatus = 0;
			rollController.reset ();
			do {
				diceResult = Random.Range (3, 18 + 1);
				sabiaOIgnorante = Random.Range (0, 2);
				if (sabiaOIgnorante == 0)
					sabiaOIgnorante--;
				if (sabiaOIgnorante == 1) {
					cual = Random.Range (0, sabias [diceResult - 3].nItems());
				} else {
					cual = Random.Range (0, ignorantes [diceResult - 3].nItems());
				}
				if(sabiaOIgnorante == 1) {
					testStatus = mc.getStorage().retrieveIntValue("Ferfuflo"+diceResult+"+1"+cual);
				}
				else {
					testStatus = mc.getStorage().retrieveIntValue("Ferfuflo"+diceResult+"-1"+cual);
				}
			} while(testStatus != 0);
			//thisBatchVariables.Add ("Ferfuflo" + diceResult + "" + sabiaOIgnorante + "" + cual);
			if (sabiaOIgnorante == 1) {
				thisBatchVariables.Add  ("Ferfuflo" + diceResult + "+1" + cual);
			} else {
				thisBatchVariables.Add  ("Ferfuflo" + diceResult + "-1" + cual);
			}
			rollController.play (diceResult);
			++state;
		}

		if (state == 2) 
		{
			if (rollController.resultReady)
			{
				++state;
				transmission.text = transmissionBank.getString (rollController.rollResult);
				string sss= propositionBank.getString(rollController.rollResult);
				assertion.text = StringUtils.chopLines(sss, 30);
				if (sabiaOIgnorante == 1) {
					lameAnswers = sabias [rollController.rollResult - 3];
				}
				else {
					lameAnswers = ignorantes [rollController.rollResult - 3];
				}
				lameAnswers.rosetta = rosetta;
				lameAnswers.reset ();
				//answer.text = StringUtils.chopLines(lameAnswers.getNextString (), 25);
				answer.GetComponent<Renderer> ().material.SetColor ("_Tint", new Color (1, 1, 1, 0));
				curtainOpenLeft ();
				adsAnim.SetBool ("whiteNoise", true);
				elapsedTime = 0.0f;
				//ads.transform.position = new Vector3 (0, 0, 10);
				// ACTIVAR LAS ??? Y EL OFFSET DE LA IMAGEN TVCRACKED
				TV.GetComponent<TVController>().ShowParticles();
			}
		}

		if (state == 3) 
		{
			if (elapsedTime <= 0.35f) {
				elapsedTime += Time.deltaTime;
				if (elapsedTime >= 0.35f) {
					ads.transform.position = new Vector3 (0, -20, 10);
				}
			}
			if (currentCurtainPosition == targetCurtainPosition) {
				++state;
			}
		}

		if (state == 4) 
		{
			if (Input.GetMouseButtonDown (0)) {
				opacity = 1.0f;	
				++state;
			}
		}

		if (state == 5) 
		{			
			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < 0.0f) {
				opacity = 0.0f;
				answer.text = StringUtils.chopLines(lameAnswers.getNextString (), 25);
				assertion.text = "";
				transmission.text = "";
				TV.GetComponent<TVController> ().ChangeTexture ();
				++state;
			}

			assertion.GetComponent<Renderer> ().material.SetColor ("_Tint", new Color (1, 1, 1, opacity));
			transmission.GetComponent<Renderer> ().material.SetColor ("_Tint", new Color (1, 1, 1, opacity));
		}

		if (state == 6) 
		{
			opacity += opacitySpeed * Time.deltaTime;
			if (opacity >= 1.0f) {
				opacity = 1.0f;
				elapsedTime = 0.0f;
				answer.GetComponent<Renderer> ().material.SetColor ("_Tint", new Color (1, 1, 1, opacity));
				//opacity = 0.0f;
				//if (opacity >= 1.0f) {  QUIZAS SEA MEJOR OPCION
				if (Input.GetMouseButtonDown (0)) {
					opacity = 0.0f;
					++state;
				}
			} else {
				answer.GetComponent<Renderer> ().material.SetColor ("_Tint", new Color (1, 1, 1, opacity));
			}
		}

		if (state == 7) {
			opacity += 6.0f * Time.deltaTime;
			if (opacity > 1.0f) {
				opacity = 1.0f;
				++state;
			}
			whiteCard.GetComponent<Image>().color = new Color (1, 1, 1, opacity);
			blackCard.GetComponent<Image> ().color = new Color (1, 1, 1, opacity);
			chip.GetComponent<Image> ().color = new Color (1, 1, 1, opacity);
		}


		if (state == 9) {
			if (amountRequired == (nTimes+1)) // exit! trance
				state = 10;
			else
				state = 12; // nos quedamos
		}

		if (state == 10) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 3.0f) {
				if (batchCorrect ()) {
					mc.getStorage ().storeIntValue ("TVBatchResult", 1);
				} else {
					resetBatchFerfuflosAnswers ();
					mc.getStorage ().storeIntValue ("TVBatchResult", -1);
				}
				fader._wa_fadeOut (this);
				this.isWaitingForActionToComplete = true;
				++state;
			}
		}

		if (state == 11) {
			
			if (isWaitingForActionToComplete)
				return;		

			countFerfuflosAnswers ();

			string returnLoc = mc.getStorage ().retrieveStringValue ("ReturnLocation");

			UnityEngine.SceneManagement.SceneManager.LoadScene (returnLoc);
			
		}

		if (state == 12) {
			opacity -= 6.0f * Time.deltaTime;
			if (opacity <= 0.0f) {
				opacity = 0.0f;
				++state;
				rollController.resetAnimation ();
			}
			answer.GetComponent<Renderer> ().material.SetColor ("_Tint", new Color (1, 1, 1, opacity));
			whiteCard.GetComponent<Image>().color = new Color (1, 1, 1, opacity);
			blackCard.GetComponent<Image> ().color = new Color (1, 1, 1, opacity);
			chip.GetComponent<Image> ().color = new Color (1, 1, 1, opacity);
		}

		if (state == 13) { // loop back
			reset();

			state = 0;
			elapsedTime = 0;
		}
	
		if (currentCurtainPosition < targetCurtainPosition) {

			currentCurtainPosition += curtainSpeed * Time.deltaTime;
			if (currentCurtainPosition > targetCurtainPosition) {
				currentCurtainPosition = targetCurtainPosition;
			}

			currentAlphaCurtain.transform.position = new Vector3 (currentCurtainPosition, 
				currentAlphaCurtain.transform.position.y, 
				currentAlphaCurtain.transform.position.z);
		}

		if (currentCurtainPosition > targetCurtainPosition) {

			currentCurtainPosition -= curtainSpeed * Time.deltaTime;
			if (currentCurtainPosition < targetCurtainPosition) {
				currentCurtainPosition = targetCurtainPosition;
			}

			currentAlphaCurtain.transform.position = new Vector3 (currentCurtainPosition, 
				currentAlphaCurtain.transform.position.y, 
				currentAlphaCurtain.transform.position.z);
		}
	}

	public void curtainCloseLeft() 
	{
		currentCurtainPosition = openCurtain;
		targetCurtainPosition = leftClosedCurtain;
		currentAlphaCurtain.transform.position = new Vector3 (currentCurtainPosition, 
			currentAlphaCurtain.transform.position.y, 
			currentAlphaCurtain.transform.position.z);
	}

	public void curtainOpenLeft() 
	{
		currentCurtainPosition = leftClosedCurtain;
		targetCurtainPosition = openCurtain;
		currentAlphaCurtain.transform.position = new Vector3 (currentCurtainPosition, 
			currentAlphaCurtain.transform.position.y, 
			currentAlphaCurtain.transform.position.z);
	}

	public void curtainCloseRight() 
	{
		currentCurtainPosition = openCurtain;
		targetCurtainPosition = rightClosedCurtain;
		currentAlphaCurtain.transform.position = new Vector3 (currentCurtainPosition, 
			currentAlphaCurtain.transform.position.y, 
			currentAlphaCurtain.transform.position.z);
	}

	public void curtainOpenRight() 
	{
		currentCurtainPosition = rightClosedCurtain;
		targetCurtainPosition = openCurtain;
		currentAlphaCurtain.transform.position = new Vector3 (currentCurtainPosition, 
			currentAlphaCurtain.transform.position.y, 
			currentAlphaCurtain.transform.position.z);
	}

	public void pickChip() 
	{
		chipCapturePos = Input.mousePosition;
		chipPicked = true;
	}

	public void releaseChip()
	{
		if (chipOnBlack || chipOnWhite)
		{
			insertChip ();
		
			if (chipOnBlack) 
			{
				if (sabiaOIgnorante == 1) {
					mc.getStorage ().storeIntValue ("Ferfuflo" + diceResult + "+1" + cual, -1);
				} else {
					mc.getStorage ().storeIntValue ("Ferfuflo" + diceResult + "-1" + cual, 1);
				}
				blackCard.GetComponent<Animator> ().SetTrigger ("configuring");
			}
			if (chipOnWhite) 
			{
				if (sabiaOIgnorante == 1) {
					mc.getStorage ().storeIntValue ("Ferfuflo" + diceResult + "+1" + cual, 1);
				} else {
					mc.getStorage ().storeIntValue ("Ferfuflo" + diceResult + "-1" + cual, -1);
				}
				whiteCard.GetComponent<Animator> ().SetTrigger ("configuring");
			}
		}
		else 
			chipPicked = false;
	}

	public void  insertChip() 
	{
		chipInserted = true;
		state = 9;
	}

	static int[] sabiasNItems     = { 4, 3, 3, 3, 3, 3, 5, 3, 4, 3, 2, 1, 4, 3, 3, 3 };
	static int[] ignorantesNItems = { 3, 3, 3, 2, 2, 2, 2, 2, 3, 2, 2, 2, 2, 1, 3, 3 };
	static string[] ids = {
		"casa1",
		"casa2",
		"casa3",
		"casa4",
		"casa5",
		"casa6",
		"casa7",
		"amarilla",
		"marron",
		"verde",
		"azul",
		"naranja"
	};

	public static void resetFerfufloAnswers(MasterControllerScript mc) {
		int completed = 0;
		int correct = 0;

		for (int i = 3; i <= 18; ++i) {
			for (int j = 0; j < sabiasNItems[i-3]; ++j) {
				mc.getStorage ().storeIntValue ("Ferfuflo" + i + "+1" + j, 0);

			}
			for (int j = 0; j < ignorantesNItems [i - 3]; ++j) {
				mc.getStorage ().storeIntValue ("Ferfuflo" + i + "-1" + j, 0);

			}
		}

		for (int i = 0; i < ids.Length; ++i) {
			mc.getStorage().storeIntValue("TVFerfuflosTimes" + ids[i], 0);
		}

		mc.getStorage ().storeIntValue ("FerfufloCompleted", 0);
		mc.getStorage ().storeIntValue ("FerfufloCorrect", 0);
	}

	public void resetBatchFerfuflosAnswers() {
		for (int i = 0; i < thisBatchVariables.Count; ++i) {
			mc.getStorage ().storeIntValue (thisBatchVariables [i], 0);
		}
	}

	public bool batchCorrect() {
		bool ok = true;
		for (int i = 0; i < thisBatchVariables.Count; ++i) {
			if (mc.getStorage ().retrieveIntValue (thisBatchVariables [i]) != 1) {
				ok = false;
				break;
			}
		}
		return ok;
	}

	public void countFerfuflosAnswers() {
		int completed = 0;
		int correct = 0;

		for (int i = 3; i <= 18; ++i) {
			for (int j = 0; j < sabias [i - 3].nItems(); ++j) {
				int result = mc.getStorage ().retrieveIntValue ("Ferfuflo" + i + "+1" + j);
				if (result != 0)
					++completed;
				if (result == 1)
					++correct;
			}
			for (int j = 0; j < ignorantes [i - 3].nItems(); ++j) {
				int result = mc.getStorage ().retrieveIntValue ("Ferfuflo" + i + "-1" + j);
				if (result != 0)
					++completed;
				if (result == 1)
					++correct;
			}
		}
		mc.N3FerHechas = completed;
		mc.N3FerCorrectas = correct;
		mc.getStorage ().storeIntValue ("FerfufloCompleted", completed);
		mc.getStorage ().storeIntValue ("FerfufloCorrect", correct);
	}
}
