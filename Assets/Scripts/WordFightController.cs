using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum WordFightControllerState { delay0, delay, delay2, delay3, zoomToPlayer, zoomToPlayer0, fightStart, fightEndWin, fightEndLose,
					zoomToPlayer1, zoomToPlayer2, loseFromTheBeginning, delay4, delay5, delay6, delay10, delay11,
					delay7, delay8, delay9, tutorial1, tutorial2, tutorial3 };

public class WordFightController : WisdominiObject {

	//Inspector
	public int substate;
	public WordFightControllerState state;

	/* references */

	public CameraManager cam;
	public HeroFan heroFan;
	public WordFightYinYang yinYang;
	public GameObject justYin;
	public GameObject justYang;
	public GameObject faderWhite;
	public GameObject blackSquare;
	new public Rosetta rosetta;
	public StringBank GenericGoodStuffBank;
	public StringBank GenericNaughtyStuffBank;
	public StringBank WarriorGoodStuff;
	public StringBank WarriorNaughtyStuff;
	public StringBank PhilosopherGoodStuff;
	public StringBank PhilosopherNaughtyStuff;
	public StringBank WizardGoodStuff;
	public StringBank WizardNaughtyStuff;
	public StringBank SageGoodStuff;
	public StringBank SageNaughtyStuff;
	public StringBank ExplorerGoodStuff;
	public StringBank ExplorerNaughtyStuff;
	public StringBank MasterGoodStuff;
	public StringBank MasterNaughtyStuff;
	public StringBank YogiGoodStuff;
	public StringBank YogiNaughtyStuff;

	public StringBank chispaStuff;

	const int AHORAAPRENDERAS = 0;
	const int DEBESELEGIR = 1;
	const int JUZGALOS = 2;
	const int NOTIENESNINGUN = 3;
	const int NOTIENESELADECUADO = 4;
	const int CONQUEHEROE = 5;
	const int NOHASELEGIDO = 6;
	const int HASELEGIDOSABIA = 7;
	const int TANTOTUSOMBRA = 8;
	const int TUAURAESLUM = 9;
	const int QUETIENESQUEHACER = 10;
	const int CADAPALABRASERA = 11;
	const int DEBESTOCARLOS = 12;
	const int YINYANGEMITIDOBUENO = 13;
	const int HASILUMINADO = 14;
	const int YINYANGEMITIDOMALO = 15;
	const int CONTINUALABATALLA = 16;
	const int HASPERDIDOLABATALLA = 18;

	StringBank specificGoodStuff;
	StringBank specificNaughtyStuff;


	public WordFightHealthBar shadowBar;
	public WordFightHealthBar playerBar;
	public MasterControllerScript mcRef;
	public UIFaderScript fader;
	public UIChispAlert chispAlert;
	public HeroGlow heroGlow;

	public WordFighter shadow;
	public WordFighter hero;

	public GameObject playerLightMat;
	public GameObject playerDarkMat;
	public GameObject shadowLightMat;
	public GameObject shadowDarkMat;
	DataStorage ds;

	/* properties */


	float elapsedTime;
	float timeToNextBlob;
	public float difficulty = 1.0f;
	public string requiredWisdom;
	bool tutorialMode;

	public float shadowEnergy;
	public float playerEnergy;



	WordFightWordBlob emittedBlob;

	/* constants */

	const float initialDelay = 3.6f;
	const float minTimeToNextBlob = 2.0f;
	const float maxTimeToNextBlob = 6.0f;
	const float hitDamage = 0.1f;

	const int Reward = 6; // should be 6

	string chosenHero = "";

	new void Start ()
	{
		rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta>();
		state = WordFightControllerState.delay0;
		chispaStuff.rosetta = rosetta;
		chispaStuff.reset ();
		elapsedTime = 0.0f;
		//difficulty = 1.0f;
		GenericGoodStuffBank.rosetta = rosetta;
		GenericNaughtyStuffBank.rosetta = rosetta;
		GenericGoodStuffBank.reset ();
		GenericNaughtyStuffBank.reset ();

		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();

		ds = mcRef.getStorage ();
		cam = GameObject.Find ("Main Camera").GetComponent<CameraManager> ();
		cam.lookingTarget = false;
		string wis = ds.retrieveStringValue ("WordFightRequiredWisdom");
		//if (!wis.Equals ("")) {
		//	requiredWisdom = wis;
		//}
		requiredWisdom = wis;

		if (requiredWisdom.Equals ("Warrior")) {
			specificGoodStuff = WarriorGoodStuff;
			specificNaughtyStuff = WarriorNaughtyStuff;
		}
		else if (requiredWisdom.Equals ("Philosopher")) {
			specificGoodStuff = PhilosopherGoodStuff;
			specificNaughtyStuff = PhilosopherNaughtyStuff;
		}
		else if (requiredWisdom.Equals ("Wizard")) {
			specificGoodStuff = WizardGoodStuff;
			specificNaughtyStuff = WizardNaughtyStuff;
		}
		else if (requiredWisdom.Equals ("Sage")) {
			specificGoodStuff = SageGoodStuff;
			specificNaughtyStuff = SageNaughtyStuff;
		}
		else if (requiredWisdom.Equals ("Explorer")) {
			specificGoodStuff = ExplorerGoodStuff;
			specificNaughtyStuff = ExplorerNaughtyStuff;
		}
		else if (requiredWisdom.Equals ("Master")) {
			specificGoodStuff = MasterGoodStuff;
			specificNaughtyStuff = MasterNaughtyStuff;
		}
		else if (requiredWisdom.Equals ("Yogi")) {
			specificGoodStuff = YogiGoodStuff;
			specificNaughtyStuff = YogiNaughtyStuff;
		}

		tutorialMode = !ds.retrieveBoolValue ("WordFightTutorialDone");
					//ds.storeBoolValue ("CurrentLevelHasHeroWarrior", true);
					//ds.storeBoolValue ("CurrentLevelHasHeroPhilosopher", true);
					//ds.storeBoolValue ("CurrentLevelHasHeroWizard", true);

		//tutorialMode = true;

		justYin.GetComponent<Renderer> ().enabled = false;
		justYang.GetComponent<Renderer> ().enabled = false;
		//blackSquare.GetComponent<Renderer> ().enabled = false;

		//playerLightMat.GetComponent<Renderer>().material.SetColor ("_TintColor", new Color (1, 1, 1, 1));
		//playerDarkMat.GetComponent<Renderer>().material.SetColor ("_TintColor", new Color (1, 1, 1, 0));
		hero.setHealth(1.0f);

		//shadowLightMat.GetComponent<Renderer>().material.SetColor ("_TintColor", new Color (1, 1, 1, 0));
		//shadowDarkMat.GetComponent<Renderer>().material.SetColor ("_TintColor", new Color (1, 1, 1, 1));
		shadow.setHealth(1.0f);

		string darkMirror = ds.retrieveStringValue ("CurrentMirror");
		ds.storeBoolValue (darkMirror, false);

		substate = -5;	
	}
	
	new void Update () 
	{
		if (state == WordFightControllerState.delay0 && substate == -5)
		{
			cam._wa_warpToMarker(this, 4);
			substate = -4;
		}
	
		if (state == WordFightControllerState.delay0 && substate == -4) { // everything black

			elapsedTime += Time.deltaTime;
			if (elapsedTime > 2.6f) {
				elapsedTime = 0.0f;
				substate = -3;
				shadow.faded = false;
				hero.faded = false; // characters appear
			}
		}

		if (state == WordFightControllerState.delay0 && substate == -3) { // characters separate

			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1.6f) {
				elapsedTime = 0.0f;
				substate = -2;
				shadow.displaced = false;
				hero.displaced = false;
				cam._wa_moveToMarker(this, 3);
			}
		}

		if (state == WordFightControllerState.delay0 && substate == -2) {

			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1.6f) {
				elapsedTime = 0.0f;
				substate = -1;
				shadow.setAnimation (0);
				hero.setAnimation (0);
			}
		}

		if (state == WordFightControllerState.delay0 && substate == -1) {

			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1.2f) {
				elapsedTime = 0.0f;
				substate = 0;
				shadow.setAnimation (1);
				hero.setAnimation (1);
			}
		}			

		if(state == WordFightControllerState.delay0 && substate == 0) {

			elapsedTime += Time.deltaTime;
			if(elapsedTime > 0.4f) 
			{
				if (tutorialMode) 
				{
					chispAlert._wa_alert(this, chispaStuff.getString(AHORAAPRENDERAS));
					this.isWaitingForActionToComplete = true;
				}
				//shadow.setAnimation (0);
				//hero.setAnimation (0);
				++substate;
				elapsedTime = 0.0f;
			}
		}

		if (state == WordFightControllerState.delay0 && substate == 1) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 0.2f) 
			{
				substate = 0;
				state = WordFightControllerState.delay;
				//cam.lerpSpeed = 1.6f;
				// CAMBIAR TIEMPO DEL ITWEEN ?
				//shadow.setAnimation (1);
				//hero.setAnimation (1);
			}
		}

		if (state == WordFightControllerState.delay) 
		{
			if (isWaitingForActionToComplete)
				return;

			chispAlert.close ();

			elapsedTime += Time.deltaTime;
			if (elapsedTime > 0.5f) 
			{
				state = WordFightControllerState.zoomToPlayer0;
				heroFan.open ();
				cam._wa_moveToMarker (this, 1);
			}
		}

		if (state == WordFightControllerState.zoomToPlayer0) 
		{
			if (isWaitingForActionToComplete)
				return;
			// else {
				string currentLvl = ds.retrieveStringValue ("CurrentLevel").Substring (0, 6);
		

				int nHeroes = 0;
				if (ds.retrieveBoolValue ("Has" + currentLvl + "Warrior"))
					++nHeroes;
				if (ds.retrieveBoolValue ("Has" + currentLvl + "Master"))
					++nHeroes;
				if (ds.retrieveBoolValue ("Has" + currentLvl + "Philosopher"))
					++nHeroes;
				if (ds.retrieveBoolValue ("Has" + currentLvl + "Explorer"))
					++nHeroes;
				if (ds.retrieveBoolValue ("Has" + currentLvl + "Sage"))
					++nHeroes;
				if (ds.retrieveBoolValue ("Has" + currentLvl + "Wizard"))
					++nHeroes;
				if (ds.retrieveBoolValue ("Has" + currentLvl + "Yogi"))
					++nHeroes;

			if (nHeroes == 0) {
				chispAlert._wa_alert (this, chispaStuff.getString (NOTIENESNINGUN));
				substate = 0;
				this.isWaitingForActionToComplete = true;
				state = WordFightControllerState.loseFromTheBeginning;
			} else if (tutorialMode) {
				chispAlert._wa_alert (this, chispaStuff.getString (DEBESELEGIR));
				this.isWaitingForActionToComplete = true;
				state = WordFightControllerState.zoomToPlayer1;
				substate = 0;
			} else {
				state = WordFightControllerState.zoomToPlayer;
			}
		}

		if (state == WordFightControllerState.zoomToPlayer1 && substate == 0) 
		{
			if (isWaitingForActionToComplete)
				return;
			if(tutorialMode) chispAlert._wa_alert (this, chispaStuff.getString(JUZGALOS));
			substate = 1;
		}
		if (state == WordFightControllerState.zoomToPlayer1 && substate == 1) 
		{
			if (isWaitingForActionToComplete)
				return;
			if(tutorialMode) chispAlert._wa_alert (this, chispaStuff.getString(CONQUEHEROE));
			state = WordFightControllerState.zoomToPlayer2;
		}

		if (state == WordFightControllerState.zoomToPlayer2) 
		{
			if (tutorialMode) 
			{
				if (isWaitingForActionToComplete)
					return;
				chispAlert.close ();
			}
			state = WordFightControllerState.zoomToPlayer;
		}

		if (state == WordFightControllerState.zoomToPlayer) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				RaycastHit hit;
				Ray ray = cam.gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit)) 
				{
					GameObject objectHit = hit.transform.gameObject;

					if (objectHit.tag == "Hero") 
					{
						WordFightHero h = objectHit.GetComponent<WordFightHero> ();

						chosenHero = h.wisdom;
						heroFan.keep (h.wisdom);
						//heroFan.close ();

						state = WordFightControllerState.delay2;
						substate = -2;
						elapsedTime = 0.0f;					
					}
				}
			}
		}

		if (state == WordFightControllerState.loseFromTheBeginning && substate==0) 
		{
			if (Input.GetMouseButtonDown(0)) 
			{		
				chispAlert.close ();
				fader._wa_fadeOut (this);
				this.isWaitingForActionToComplete = true;
				substate = 1;
			}
		}
		if (state == WordFightControllerState.loseFromTheBeginning && substate==1) 
		{
			if (!isWaitingForActionToComplete) {
				ds.storeIntValue ("AlphabetReward", -Reward*6);
				string ret = ds.retrieveStringValue ("ReturnLocation");
				SceneManager.LoadScene (ret);
			}
		}

		if (state == WordFightControllerState.delay2 && substate == -2) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 0.5f) 
			{
				++substate;
				heroFan.closeSlowly ();
			}
		}

		if (state == WordFightControllerState.delay2 && substate == -1) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1.5f) 
			{
				++substate;
				heroGlow.glow ();
				//heroFan.closeSlowly ();
			}
		}

		if (state == WordFightControllerState.delay2 && substate == 0) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 3.5f) {
				elapsedTime = 0.0f;
				//cam._wa_moveToMarker (this, 3);
				cam._wm_moveToMarker (3);
				++substate;
				if (!chosenHero.Equals (requiredWisdom)) 
				{					
					state = WordFightControllerState.loseFromTheBeginning;
					substate = 0;
					chispAlert._wa_alert (this, chispaStuff.getString (NOHASELEGIDO));
					fader.setFadeColor (0, 0, 0);
					//fader.fadeOut ();
				}
				else if (tutorialMode) 
				{					
					chispAlert._wa_alert (this, chispaStuff.getString (HASELEGIDOSABIA));
					++substate;
				}
			}
		}

		if (state == WordFightControllerState.delay2 && substate == 1) 
		{
			++substate;
		}
		if (state == WordFightControllerState.delay2 && substate == 2) 
		{
			if (tutorialMode)
			{
				if (isWaitingForActionToComplete)
					return;
				chispAlert.close ();
			}
			++substate;
		}

		if (state == WordFightControllerState.delay2 && substate == 3) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 0.75f) {
				elapsedTime = 0.0f;

				shadowBar.grow ();
				playerBar._wa_grow (this);
				//hero.fighting = true;
				//shadow.fighting = true;
				hero.showHalo ();
				shadow.showHalo ();
				//yingYang._wa_grow (this);

				timeToNextBlob = FloatRandom.floatRandomRange (minTimeToNextBlob/difficulty, maxTimeToNextBlob/difficulty);
				++substate;
				// Cambiar esto para jugar en serio
				shadowEnergy = 0.1f;
				playerEnergy = 1.0f;
			}
		}

		if (state == WordFightControllerState.delay2 && substate == 4) 
		{
			if (isWaitingForActionToComplete)
				return;

			if (tutorialMode) 
			{
				chispAlert._wa_alert (this, chispaStuff.getString (TANTOTUSOMBRA));
			} 
			++substate;
		}

		if (state == WordFightControllerState.delay2 && substate == 5) 
		{
			if (tutorialMode) 
			{
				if (isWaitingForActionToComplete)
					return;
			}

			if (tutorialMode) 
			{
				chispAlert._wa_alert (this, chispaStuff.getString (TUAURAESLUM));
			}
			++substate;
		}

		if (state == WordFightControllerState.delay2 && substate == 6) 
		{
			if (tutorialMode) 
			{
				if (isWaitingForActionToComplete)
					return;
				chispAlert.close ();
			}
			yinYang._wa_grow (this);

			++substate;
		}

		if (state == WordFightControllerState.delay2 && substate == 7) 
		{
			if (isWaitingForActionToComplete)
				return;

			hero.fighting = true;
			shadow.fighting = true;
			elapsedTime = 0.0f;
			if (tutorialMode) 
			{
				++substate;
			}
			else {
				state = WordFightControllerState.fightStart;
			}
		}

		if (state == WordFightControllerState.delay2 && substate == 8) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1.5f) {
				if (tutorialMode) {
					chispAlert._wa_alert (this, chispaStuff.getString (QUETIENESQUEHACER));
				}
				++substate;
			}
		}

		if (state == WordFightControllerState.delay2 && substate == 9) 
		{
			if (tutorialMode)
			{
				if (isWaitingForActionToComplete)
					return;
			}

			if (tutorialMode) 
			{
				chispAlert._wa_alert (this, chispaStuff.getString (CADAPALABRASERA));
			}
			++substate;
		}

		if (state == WordFightControllerState.delay2 && substate == 10) 
		{
			if (tutorialMode) 
			{
				if (isWaitingForActionToComplete)
					return;

				chispAlert._wa_alert (this, chispaStuff.getString (DEBESTOCARLOS));
			}
			elapsedTime = 0.0f;
			++substate;
		}

		if (state == WordFightControllerState.delay2 && substate == 11) 
		{
			if (tutorialMode) 
			{
				if (isWaitingForActionToComplete)
					return;
				chispAlert.close ();
			}
			elapsedTime = 0.0f;
			++substate;
		}

		if(state == WordFightControllerState.delay2 && substate == 12) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1.0f) {
				yinYang.emitBlob (GenericGoodStuffBank.getNextString (), true);
				elapsedTime = 0.0f;
				++substate;
			}
		}

		if(state == WordFightControllerState.delay2 && substate == 13) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1.0f) {
				emittedBlob = GameObject.Find ("WFBlob").GetComponent<WordFightWordBlob> ();
				emittedBlob.pause ();
				chispAlert._wa_alert (this, chispaStuff.getString (YINYANGEMITIDOBUENO));
				++substate;
			}
		}

		if (state == WordFightControllerState.delay2 && substate == 14)
		{
			if (isWaitingForActionToComplete)
				return;

			chispAlert.close ();
			emittedBlob.unpause ();
			shadowBar.queryClear ();
			++substate;
		}

		/* wait until player drags word onto yingyang */
		if (state == WordFightControllerState.delay2 && substate == 15) 
		{
			checkMouseClick ();

			if (shadowBar.queryEvent ("HealthBarEvent")) 
			{
				substate = 17;
				chispAlert._wa_alert (this, chispaStuff.getString (HASILUMINADO));
			}

			if (emittedBlob == null && shadowBar.targetBarFraction == 1) 
			{
				yinYang.emitBlob (GenericGoodStuffBank.getNextString (), true);
				++substate;
			}
		}

		if (state == WordFightControllerState.delay2 && substate == 16) 
		{
			GameObject g = GameObject.Find ("WFBlob");
			if (g != null) {
				emittedBlob = g.GetComponent<WordFightWordBlob> ();
				substate = 15;
			}
		}

		if (state == WordFightControllerState.delay2 && substate == 17) 
		{
			if (isWaitingForActionToComplete)
				return;
			chispAlert.close ();
			++substate;
			elapsedTime = 0.0f;
		}

		if (state == WordFightControllerState.delay2 && substate == 18) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1.0f) {
				yinYang.emitBlob (GenericNaughtyStuffBank.getNextString (), false);
				elapsedTime = 0.0f;
				++substate;
			}
		}

		if(state == WordFightControllerState.delay2 && substate == 19) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1.0f) {
				emittedBlob = GameObject.Find ("WFBlob").GetComponent<WordFightWordBlob> ();
				emittedBlob.pause ();
				chispAlert._wa_alert (this, chispaStuff.getString (YINYANGEMITIDOMALO));
				++substate;
			}
		}

		if (state == WordFightControllerState.delay2 && substate == 20) 
		{
			if (isWaitingForActionToComplete)
				return;

			chispAlert.close ();
			elapsedTime = 0.0f;
			++substate;
		}


		if (state == WordFightControllerState.delay2 && substate == 21) 
		{
			elapsedTime += Time.deltaTime;

			if (elapsedTime > 1.0f) {
				emittedBlob.unpause ();			
				++substate;
			}
		}

		/* wait until naughtyWord has gone away */
		if (state == WordFightControllerState.delay2 && substate == 22) 
		{
			checkMouseClick ();

			/* blob has hit player. Emit another */
			if (emittedBlob == null && playerBar.queryEvent("HealthBarEvent")) {
				yinYang.emitBlob (GenericNaughtyStuffBank.getNextString (), false);
				++substate;
				return;
			}

			if (playerBar.targetBarFraction < 0.5f)
				playerBar.targetBarFraction = 0.9f;

			/* blob has flown away. To next substate */
			if (emittedBlob == null && !playerBar.queryEvent("HealthBarEvent")) {
				substate = 24;
			}
		}

		if (state == WordFightControllerState.delay2 && substate == 23) 
		{
			/* spawn new blob */
			GameObject g = GameObject.Find ("WFBlob");
			if (g != null) {
				emittedBlob = g.GetComponent<WordFightWordBlob> ();
				substate = 22;
			}
		}

		if (state == WordFightControllerState.delay2 && substate == 24) 
		{
			chispAlert._wa_alert (this, chispaStuff.getString (CONTINUALABATALLA));
			++substate;
		}

		if (state == WordFightControllerState.delay2 && substate == 25) 
		{
			if (isWaitingForActionToComplete)
				return;
			chispAlert.close ();
			state = WordFightControllerState.fightStart;
		}

		if (state == WordFightControllerState.fightEndWin && substate == 0) 
		{
			cam.moveToMarker (2, 5);
			yinYang.home ();
			elapsedTime = 0.0f;
			++substate;
			ds.storeIntValue ("AlphabetReward", Reward);
			int defeated = ds.retrieveIntValue ("DefeatedShadows");
			++defeated;
			ds.storeIntValue ("DefeatedShadows", defeated);
			ds.storeBoolValue ("WordFightTutorialDone", true);
		}

		if (state == WordFightControllerState.fightEndWin && substate == 1)
		{
			if (yinYang.isFinishedHoming()) 
			{
//				yinYang.GetComponent<Renderer> ().enabled = false;
//				justYin.GetComponent<Renderer> ().enabled = true;
//				justYang.GetComponent<Renderer> ().enabled = true;
				//blackSquare.GetComponent<Renderer> ().enabled = true;
				++substate;
				elapsedTime = 0.0f;
			}
		}

		if (state == WordFightControllerState.fightEndWin && substate == 2) 
		{
			elapsedTime += Time.deltaTime;
//			Vector3 pos;
//			pos = justYin.transform.localPosition;
//			pos.x += Time.deltaTime;
//			justYin.transform.position = pos;
//			pos = justYang.transform.localPosition;
//			pos.x -= Time.deltaTime;
//			justYang.transform.position = pos;
//			pos = blackSquare.transform.localScale;
//			pos.x -= Time.deltaTime;
//			blackSquare.transform.localScale = pos;

			if (!faderWhite.GetComponent<FaderItween>().action)
			faderWhite.GetComponent<FaderItween> ().action = true;

			if (elapsedTime > faderWhite.GetComponent<FaderItween> ().timeTo) 
			{
				string levelName = ds.retrieveStringValue ("CurrentLevel").Substring(0,6);
				int nWave = ds.retrieveIntValue (levelName + "ShadowWaveNumber");
				++nWave;
				ds.storeIntValue (levelName + "ShadowWaveNumber", nWave); // increment wave number
				//Handheld.PlayFullScreenMovie ("4.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
				if (Version.versionType == VersionType.demo) 
				{
					string returnLocation = ds.retrieveStringValue ("ReturnLocation");
					SceneManager.LoadScene (returnLocation);
				} else {
					//string currentHero = ds.retrieveStringValue ();
					int numHero = ds.retrieveIntValue("Has" + requiredWisdom);
                    //ds.storeStringValue ("TheaterFolder", requiredWisdom + numHero);
                    //SceneManager.LoadScene ("Scenes/Theater");
                    int wins = ds.retrieveIntValue("WordFightWins");
                    ds.storeIntValue("WordFightWins", ++wins);
                    ds.storeBoolValue("JustWonFight", true);
                    ds.storeBoolValue("FadeWhite", true);
                    string returnLocation = ds.retrieveStringValue("ReturnLocation");
                    SceneManager.LoadScene(returnLocation);
                }
			}
		}

		if(state == WordFightControllerState.fightEndLose && substate == 0) 
		{
			chispAlert._wa_alert (this, chispaStuff.getString (HASPERDIDOLABATALLA));
			fader.setFadeColor (0, 0, 0);
			this.isWaitingForActionToComplete = true;
			++substate;
		}

		if (state == WordFightControllerState.fightEndLose && substate == 1) 
		{
			if (isWaitingForActionToComplete)
				return;
			
			chispAlert.close ();
			fader._wa_fadeOut(this);
			this.isWaitingForActionToComplete = true;
			++substate;
		}

		if (state == WordFightControllerState.fightEndLose && substate == 2) 
		{
			if (isWaitingForActionToComplete)
				return;
			
			this.isWaitingForActionToComplete = true;
			++substate;
		}

		if (state == WordFightControllerState.fightEndLose && substate == 3) 
		{
			if (!this.isWaitingForActionToComplete) {
				string ret = ds.retrieveStringValue ("ReturnLocation");
				ds.storeIntValue ("AlphabetReward", -Reward);
				SceneManager.LoadScene (ret);
			}
		}

		if (state == WordFightControllerState.fightStart) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > timeToNextBlob) 
			{
				elapsedTime = 0.0f;
				timeToNextBlob = FloatRandom.floatRandomRange (minTimeToNextBlob/difficulty, maxTimeToNextBlob/difficulty);

				float chance = FloatRandom.floatRandomRange (0.0f, 1.0f);

				if (chance < 0.5f) 
				{

					string ns = GenericGoodStuffBank.getNextString ();
					int a = ns.Length;
					yinYang.emitBlob (ns, true);

				} 
				else {
					string ns = GenericNaughtyStuffBank.getNextString ();
					int hhh = ns.Length;
					yinYang.emitBlob (ns, false);
				}
			}

			if (Input.GetMouseButtonDown (0)) 
			{
				RaycastHit[] hits;
				Ray ray = cam.gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

				hits = Physics.RaycastAll (ray);
				for(int i =0; i<hits.Length; ++i) {
					GameObject objectHit = hits[i].transform.gameObject;

					if (objectHit.tag == "TextBlob") 
					{
						WordFightWordBlob theBlob = objectHit.GetComponent<WordFightWordBlob> ();
						//theBlob.tab = tab;
						theBlob.pickUp (hits[i].point);

						//Destroy (objectHit);

					} else if (objectHit.tag == "RayCastBillboard") 
					{
						Vector3 worldCoords = hits[i].point;
					}
				}
			}
		}
	}

	void checkMouseClick() 
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			RaycastHit hit;
			Ray ray = cam.gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit)) 
			{
				GameObject objectHit = hit.transform.gameObject;

				if (objectHit.tag == "TextBlob") 
				{
					WordFightWordBlob theBlob = objectHit.GetComponent<WordFightWordBlob> ();
					//theBlob.tab = tab;
					theBlob.pickUp (hit.point);
					//Destroy (objectHit);

				} 
				else if (objectHit.tag == "RayCastBillboard") 
				{
					Vector3 worldCoords = hit.point;
				}
			}
		}
	}

	public void hurtShadow() 
	{		
		shadowEnergy -= hitDamage;
		if (shadowEnergy < 0.0f) 
		{
			shadowEnergy = 0.0f;
			state = WordFightControllerState.fightEndWin;
			substate = 0;
		}
		shadowBar.targetBarFraction = shadowEnergy;
		//shadowLightMat.GetComponent<Renderer>().material.SetColor ("_TintColor", new Color (1, 1, 1, 1.0f - shadowEnergy));
		//shadowDarkMat.GetComponent<Renderer>().material.SetColor ("_TintColor", new Color (1, 1, 1, shadowEnergy));
		shadow.setHealth (shadowEnergy);
		shadow.hit ();
	}

	public void hurtPlayer() 
	{
		playerEnergy -= hitDamage;

		if (playerEnergy < 0.0f) 
		{
			playerEnergy = 0.0f; // battle end
			state = WordFightControllerState.fightEndLose;
			substate = 0;
		}
		playerBar.targetBarFraction = playerEnergy;
		//playerLightMat.GetComponent<Renderer>().material.SetColor ("_TintColor", new Color (1, 1, 1, playerEnergy));
		//playerDarkMat.GetComponent<Renderer>().material.SetColor ("_TintColor", new Color (1, 1, 1, 1.0f - playerEnergy));
		hero.setHealth(playerEnergy);
		hero.hit ();
	}
}
