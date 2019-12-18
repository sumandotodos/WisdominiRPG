using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

enum QAControllerState { idle, idle2, exitting1, exitting2, printingQuestion, waitingForAnswer, 
	waitingForAnswer2, answered, 
	preparingNextQuestion, preparingNextQuestion2, exitting3, exitting4 };

public class QAController : WisdominiObject {
	
	/* references */

	public StringBank obtainTexts;
	public ImageSwitch imageSwitch;
	public ParticleSystem particles;
//	public int FirstSetIndex;
//	public int LastSetIndex;
	StringBankCollection qa; // the full collection of sets
	public ArrayCollectionNoRepeat QAHelper;
	public GameObject QAnswerPrefab;
	public GameObject answerPanel;
	public int[] subsetSize;
	MasterControllerScript mcRef;
	new public Rosetta rosetta;
	public burner[] burn;
	public Text questionText;
	StringBank qb;
	StringBank ab;
	GameObject [] answerGO;
	public UIFaderScript fader;
	public UIFaderScript secondFader;
	public GameObject flower;
	public GameObject spark1;
	public GameObject spark2;
	public GameObject spark3;
	public GameObject flowerRenderQuad;
	public GameObject keyImage;
	public GameObject youGotAKeyText;
	UIImageFader keyImageFader;
	UITextFader youGotAKeyTextFader;
	Animator flowerAnim;
	Animator spark1Anim;
	Animator spark2Anim;
	Animator spark3Anim;
	DataStorage ds;
	public LevelControllerScript levelRef;
	public AudioClip hitSound;
	public AudioClip missSound;
	public AudioClip fireSound;
	public AudioClip winSound;
	public AudioClip sparksSound;

	/* properties */

	new bool[] enabled;
	int [] header;
	QAControllerState state;
	float questionLetterOutput;
	string question;
	float elapsedTime;
	int correctAnswer;
	int selectedAnswer;
	int currentQuestion;
	int nMisses;
	public int keyChannel;
	int nLevel;


	/* constants */

	float letterOutputSpeed = 30.0f;
	const float slowletterOutputSpeed = 30.0f;
	const float fastletterOutputSpeed = 400.0f;
	const float initialDelay = 2.0f;
	const int maxAnswers = 8;
	const float postAnswerTimeOut = 1.0f;
	const int numQuestions = 5;
	const string color1 = "#FFFFFFFF";
	const string color2 = "#FFFFFF99";
	const string color3 = "#FFFFFF44";
	const string color4 = "#FFFFFF00";

	float sparkScale = 0.0f;
	float sparkTargetScale = 0.0f;

	int bankIndex;

	const float QuestionLettersPerLine = 60;
	const float InitialY = 250.0f;
	const float InterAnswerDistance = 65.0f;
	const float QuestionLineHeight = 32.0f;
	const float AnswerLineHeight = 24.0f;
	const float YAdjust = +360.0f;

	new void Start () 
	{
		rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();
		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = mcRef.getStorage ();

		mcRef.playMusic ("QAMusic");

		/* retrieve status from MasterController general storage */
		/*enabled = new bool[qa.Length];
		for (int i = 0; i < qa.Length; ++i) {

			enabled [i] = mcRef.getStorage ().retrieveBoolValue ("QuestionSetIsEnabled" + i);

		}

		header = new int[qa.Length];
		for (int i = 0; i < qa.Length; ++i) {

			header [i] = mcRef.getStorage ().retrieveIntValue ("QuestionSetHeader" + i);

		}*/
		keyChannel = ds.retrieveIntValue ("CurrentFlameIndex");
		nLevel = ds.retrieveIntValue ("Level");
		if (nLevel == 0)
			nLevel = 1;
		imageSwitch.setChannel (keyChannel);
		keyImage.GetComponent<Image> ().sprite = imageSwitch.getSprite ();

		string sss = obtainTexts.getStringId (keyChannel);
		youGotAKeyText.GetComponent<Text> ().text = rosetta.retrieveString (sss);

		state = QAControllerState.idle;
		questionLetterOutput = 0.0f;
		elapsedTime = 0.0f;
		answerGO = new GameObject[maxAnswers];
		currentQuestion = 0;

		//string curLev = ds.retrieveStringValue ("CurrentLevel");
		//int n = ds.retrieveIntValue (curLev + "NumberOfKeys");
//		int n = ds.retrieveIntValue ("GlobalQACounter");
//		ds.storeIntValue ("GlobalQACounter", n + 1);
//
//		n = FirstSetIndex + (n % (LastSetIndex-FirstSetIndex+1)) - 1;
//
//		bankIndex = n;

		qa = QAHelper.getNextBank ();

		qa.reset();
		qb = qa.getNextBank ();
		qb.reset ();

		nMisses = 0;

		flowerAnim = flower.GetComponent<Animator> ();
		spark1Anim = spark1.GetComponent<Animator> ();
		spark2Anim = spark2.GetComponent<Animator> ();
		spark3Anim = spark3.GetComponent<Animator> ();

		keyImageFader = keyImage.GetComponent<UIImageFader> ();
		youGotAKeyTextFader = youGotAKeyText.GetComponent<UITextFader> ();

		particles.Stop ();
		spark1.transform.localScale = Vector3.zero;
		spark2.transform.localScale = Vector3.zero;
		spark3.transform.localScale = Vector3.zero;
	}
	
	new void Update () 
	{
		bool change = Utils.updateSoftVariable (ref sparkScale, sparkTargetScale, 30.0f);
		if (change) {
			spark1.transform.localScale = new Vector3 (sparkScale, sparkScale, sparkScale);
			spark2.transform.localScale = new Vector3 (sparkScale, sparkScale, sparkScale);
			spark3.transform.localScale = new Vector3 (sparkScale, sparkScale, sparkScale);
		}

		if (state == QAControllerState.idle)
		{
			elapsedTime += Time.deltaTime;


			if (elapsedTime > initialDelay)
			{
				letterOutputSpeed = slowletterOutputSpeed;
				state = QAControllerState.idle2;
			}
		}

		if (state == QAControllerState.idle2) 
		{
			/* extract question and answers */

			ab = qa.getNextBank ();
			correctAnswer = qb.correntAnswer [currentQuestion];
			ab.rosetta = rosetta;
			qb.rosetta = rosetta;

			ab.reset ();

			burn [currentQuestion].lightUp ();

			question = qb.getNextString ();

			state = QAControllerState.printingQuestion;

			questionLetterOutput = 0.0f;
		}

		if (state == QAControllerState.printingQuestion) 
		{
			int letters = (int)questionLetterOutput;

			if (Input.GetMouseButtonDown (0))
				letterOutputSpeed = fastletterOutputSpeed;

			if ((letters > 2) && (letters < question.Length-2)) {
				questionText.text = "<color=" + color1 + ">" + question.Substring (0, letters) + "</color>" +
					"<color=" + color2 + ">" + question.Substring (letters, 1) + "</color>" +
					"<color=" + color3 + ">" + question.Substring (letters+1, 1) + "</color>" +
					 "<color=" + color4 + ">" + question.Substring(letters+2, question.Length-1-letters-2) +
					"</color>";
			}
			if (letters == question.Length) {
				questionText.text = question;
			}
			questionLetterOutput += letterOutputSpeed * Time.deltaTime;

			if (letters > question.Length-1) {
				letters = question.Length-1;
				questionText.text = "" + question;
				state = QAControllerState.waitingForAnswer;

				for (int i = 0; i < maxAnswers; ++i)
					answerGO [i] = null;

				float yPos = YAdjust - (question.Length / QuestionLettersPerLine) * QuestionLineHeight;
				for(int i = 0; i<ab.nItems(); ++i) 
				{
					GameObject newAnswer;
					newAnswer = Instantiate (QAnswerPrefab);
					newAnswer.transform.SetParent(answerPanel.transform);
					newAnswer.transform.localScale = Vector3.one;            
					newAnswer.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 45);
					newAnswer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					newAnswer.GetComponent<Text> ().alignment = TextAnchor.UpperCenter;
					newAnswer.GetComponent<QAAnswer> ().initialize (yPos);
					newAnswer.GetComponent<QAAnswer> ().answerNumber = i + 1;
					int nLines;
					newAnswer.GetComponent<QAAnswer> ().setText (StringUtils.chopLines (ab.getNextString (), 85, out nLines));
					newAnswer.GetComponent<QAAnswer> ().setDelay (i * 0.3f);
					yPos -= (InterAnswerDistance + AnswerLineHeight * (nLines-1));
					answerGO [i] = newAnswer;
				}
			}
		}

		if (state == QAControllerState.waitingForAnswer) 
		{
			bool waiting = true;
			for (int i = 0; i < ab.nItems(); i++) 
			{
				if (answerGO [i].GetComponent<QAAnswer> ().state != QAAnswerState.idle)
					waiting = false;
				else {
					waiting = true;
				}
			}

			if (waiting) {
				for (int i = 0; i < ab.nItems(); i++) {
					answerGO [i].GetComponent<QAAnswer> ().state = QAAnswerState.ready;
					state = QAControllerState.waitingForAnswer2;
				}
			}			
		}

		if (state == QAControllerState.waitingForAnswer2) 
		{

		}

		if (state == QAControllerState.answered) 
		{

			letterOutputSpeed = slowletterOutputSpeed;
			for (int i = 0; i < ab.nItems (); ++i) 
			{
				if (answerGO [i].GetComponent<QAAnswer> ().answerNumber != selectedAnswer) {
					answerGO [i].GetComponent<QAAnswer> ().dispose ();
				} else {					
						answerGO [i].GetComponent<QAAnswer> ().blink (selectedAnswer == correctAnswer);
					if (selectedAnswer == correctAnswer)
						levelRef.playSound (hitSound);
					else
						levelRef.playSound (missSound);
				}
			}

			elapsedTime = 0.0f;
			state = QAControllerState.preparingNextQuestion;
		}

		if (state == QAControllerState.preparingNextQuestion) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > postAnswerTimeOut) { 
				elapsedTime = 0.0f;
				state = QAControllerState.preparingNextQuestion2;
				if (selectedAnswer == correctAnswer) {
					burn [currentQuestion].burstInFlames ();

				} else {
					++nMisses;
					if (nMisses > 1) 
					{
						fader._wa_fadeOut (this);
						keyChannel = -1;
						// mark this flame for resurrection, please
						string lvl = ds.retrieveStringValue("CurrentLevelFlame");
						int FlameIndex = ds.retrieveIntValue("CurrentFlameIndex");
						string FlameName = ds.retrieveStringValue ("FlameResurrectionName" + lvl + FlameIndex);
						string FlameLocation = ds.retrieveStringValue ("FlameResurrectionLocation" + lvl + FlameIndex);
						ds.storeIntValue ("Flame" + (FlameIndex) + "Resurrect" + FlameLocation, 3);
						ds.storeStringValue ("Flame" + (FlameIndex) + "Resurrect" + FlameLocation, FlameName);
						this.isWaitingForActionToComplete = true;
						state = QAControllerState.exitting3;
					}
				}
				if (currentQuestion == numQuestions - 1) 
				{
					//fader._wa_fadeOut (this);
					//this.isWaitingForActionToComplete = true;
					/* do the flower thing */
					flowerRenderQuad.GetComponent<GameObjectFader> ().fadeIn ();
					flowerAnim.SetBool("open", true);
					spark1Anim.SetBool ("open", true);
					spark2Anim.SetBool ("open", true);
					spark3Anim.SetBool ("open", true);
					levelRef.dipMusic (4);
					levelRef.playSound (sparksSound);
					sparkTargetScale = 60.0f;
					elapsedTime = 0.0f;
					state = QAControllerState.exitting1;
				}
			}		
		}

		if (state == QAControllerState.preparingNextQuestion2) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > postAnswerTimeOut) {
				questionText.text = "";
				++currentQuestion;
				state = QAControllerState.idle;
			}
		}

		if (state == QAControllerState.exitting1) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 5.6f) {
				fader.setFadeColor (1, 1, 1);
				fader._wa_fadeOut (this);
				keyImageFader.fadeIn ();
				youGotAKeyTextFader.fadeIn ();
				this.isWaitingForActionToComplete = true;
				state = QAControllerState.exitting2;
			}
		}

		if (state == QAControllerState.exitting2) 
		{
			if (!isWaitingForActionToComplete) 
			{
				levelRef.playSound (winSound);
				elapsedTime = 0.0f;
				state = QAControllerState.exitting3;
			}
		}

		if (state == QAControllerState.exitting3) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 2.0f) {
				secondFader.setFadeValue(1.0f);
				secondFader.setFadeColor (0, 0, 0);
				secondFader._wa_fadeOut (this);
				this.isWaitingForActionToComplete = true;
				state = QAControllerState.exitting4;
			}
		}

		if (state == QAControllerState.exitting4)
		{
			string rLoc = mcRef.getStorage ().retrieveStringValue ("ReturnLocation");
			string lvl = rLoc.Substring (0, 6);

			if (isWaitingForActionToComplete) 
			{

			}
			else 
			{
				switch (keyChannel) {
				case 0:	
					mcRef.getStorage ().storeBoolValue ("Has" + lvl + "RedKey", true);
					break;
				case 1:
					mcRef.getStorage ().storeBoolValue ("Has" + lvl + "OrangeKey", true);
					break;
				case 2:
					mcRef.getStorage ().storeBoolValue ("Has" + lvl + "BlueKey", true);
					break;
				case 3:
					mcRef.getStorage ().storeBoolValue ("Has" + lvl + "GreenKey", true);
					break;
				case 4:
					mcRef.getStorage ().storeBoolValue ("Has" + lvl + "YellowKey", true);
					break;
				case 5:
					mcRef.getStorage ().storeBoolValue ("Has" + lvl + "PurpleKey", true);
					break;
				case 6:
					mcRef.getStorage ().storeBoolValue ("Has" + lvl + "BrownKey", true);
					break;
				}
				mcRef.getStorage ().storeBoolValue ("IsHUDCracked", false); // return HUD crack to non-crcked

				// increment number of keys
				string curLev = mcRef.getStorage ().retrieveStringValue ("CurrentLevel");
				int n = mcRef.getStorage ().retrieveIntValue (curLev + "NumberOfKeys");
				++n;
				mcRef.getStorage ().storeIntValue (curLev + "NumberOfKeys", n);
				SceneManager.LoadScene (rLoc);
			}
		}	
	}

	public void answerSelected(int a) 
	{
		selectedAnswer = a;
		state = QAControllerState.answered;
	}
}
