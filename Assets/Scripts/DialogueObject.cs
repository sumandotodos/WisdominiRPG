using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum BarStatus { showing, appearing, hidden, disappearing };

public enum SpeakerPosition { left, center, right };

public class DialogueObject : WisdominiObject {

	bool panelMustNotifyFinishAction = false;

	[HideInInspector]
	public List<int> disabledAnswers;


	/* Constants */

	const float bubbleWidth = 430.0f;
	const float bubbleHeight = 71.0f;
	const float MaxBubbleWidth = 820.0f;
	const float MinBubbleWidth = 160.0f;
	const float DeployedBottomY = -220.0f;

	const int maxAnswers = 16;

	Color askBubbleColor = new Color (0.7f, 0.7f, 0.7f, 0.7f);

	const float HiddenTopY = 265.0f + 120.0f;
	const float ShowingTopY = 130.0f + 125.0f;

	const float ShowingBottomY = 50.0f - 725.0f;
	const float HiddenBottomY = -30.0f - 725.0f;

	const float openBottomY = 150.0f;

	const float panelYSpeed = 900.0f;//900.0f;

	public float thisBubbleHeight;

	const float strangeMagicConversionFactor = 0.50f;


	/* Properties */

	public BarStatus topBarStatus = BarStatus.hidden;
	public BarStatus bottomPanelStatus = BarStatus.hidden;

	float targetTopY = HiddenTopY;
	float topY = HiddenTopY;
	float targetBottomY = HiddenBottomY;
	float bottomY = HiddenBottomY;

	public float maxScroll = 0;
	public float minScroll = 0;

	private Canvas theCanvas;
	public Color blobColor;

	Color leftColor = new Color (1.0f, 1.0f, 1.0f, 0.7f);
	Color centerColor = new Color (1.0f, 0.8f, 0.8f, 0.7f);
	Color rightColor = new Color (0.4f, 0.85f, 0.7f, 0.7f);

	private int talkerPosition;

	SpeakerPosition speakerPosition = SpeakerPosition.left;
	bool positionChanged = true;

	private float verticalScroll;
	private float targetScroll;

	private float targetTypeHereTransparency;
	private float typeHereTransparency;

	Vector3 originalPosition;

	const float SCROLLSPEED = 6.0f;

	public int selectedAnswer;

	//float nextBubbleY;
	public float nextTextY;
	public float prevBubbleHeight = 0.0f;

	public bool readerMode = false;

    public 


	//List<DialogueElement> elements;

	List<GameObject> elements;
	List<GameObject> answerElements;
	List<GameObject> sayElements;
	List<GameObject> bottomPanelElements;
	List<GameObject> bubbleContainerElements;

	SpeakerPosition currentSpeaker;

	GameObject currentParent;

	bool isAnswerBlob = false;
	int globalAnswerNumber = 0;

	List<string> conversation; // for storing the conversation


	string[] answerArray;

	public bool autoscroll = true;

	float globalTextSpeed = 1.75f;

	bool usingAlienFont = false;


	/* References  */

	public GameObject theDialogueObject;

	public Image top;
	public Image middle;
	public Image bottom;

	public GameObject topBarRef;
	public GameObject bottomPanelRef;
	public GameObject bubbleContainerRef;

	//public Image leftImageRef;
	//public Image rightImageRef;

	public Image TypeHereRef;
	public Text TypeHereTextRef;

	public Sprite[] pressToContinue;

	MasterControllerScript masterController;
	DataStorage ds;

	public UIBlinker npc0Blinker;
	public UIBlinker npc1Blinker;

    public Sprite TopLeft;
    public Sprite TopCenter;
    public Sprite TopRight;
    public Sprite MiddleLeft;
    public Sprite MiddleCenter;
    public Sprite MiddleRight;
    public Sprite BottomLeft;
    public Sprite BottomCenter;
    public Sprite BottomRight;
    public Sprite LeftPoint;
    public Sprite RightPoint;

    public Font NormalFont;
    public Font AlienFont;


	/* methods */


	public void _wm_showTopBar() {

		if ((topBarStatus == BarStatus.hidden) || (topBarStatus == BarStatus.disappearing)) {
			topBarStatus = BarStatus.appearing;
			targetTopY = ShowingTopY;
			panelMustNotifyFinishAction = false;
		}
	

	}

	public void _wm_hideTopBar() {

		if ((topBarStatus == BarStatus.showing) || (topBarStatus == BarStatus.appearing)) {
			topBarStatus = BarStatus.disappearing;
			targetTopY = HiddenTopY;
			panelMustNotifyFinishAction = false;
		}

	}

	public void _wm_deployBottomPanel() {

			
			targetTypeHereTransparency = 1.0f;
			
			bottomPanelStatus = BarStatus.appearing;
			targetBottomY = DeployedBottomY;
			panelMustNotifyFinishAction = false;

	}

	public void setAutoScroll(bool autos) {

		autoscroll = autos;

	}

	public void _wm_showBottomPanel() {

		if ((bottomPanelStatus == BarStatus.hidden) || (bottomPanelStatus == BarStatus.disappearing)) {
			bottomPanelStatus = BarStatus.appearing;
			targetBottomY = ShowingBottomY;
			TypeHereTextRef.color = new Vector4 (0.0f, 0.0f, 0.0f, 1.0f);
			TypeHereRef.color = new Vector4 (1.0f, 1.0f, 1.0f, 1.0f);
			panelMustNotifyFinishAction = false;
		}

	}

	public void _wm_hideBottomPanel() {

		if ((bottomPanelStatus == BarStatus.showing) || (bottomPanelStatus == BarStatus.appearing)) {
			bottomPanelStatus = BarStatus.disappearing;
			targetBottomY = HiddenBottomY;
			panelMustNotifyFinishAction = false;
		}

	}

	public void _wa_hideBottomPanel(WisdominiObject waitRef, int prg) {

		this.registerWaitingObject (waitRef, prg);
		_wm_hideBottomPanel ();
		panelMustNotifyFinishAction = true;

	}

	public void _wm_setSpeaker(string position) {

		if(position.ToLower().Equals("left")) {
			if (currentSpeaker != SpeakerPosition.left)
				positionChanged = true;
			currentSpeaker = SpeakerPosition.left;
		}
		if(position.ToLower().Equals("center")) {
			if (currentSpeaker != SpeakerPosition.center)
				positionChanged = true;
			currentSpeaker = SpeakerPosition.center;
		}
		if(position.ToLower().Equals("right")) {
			if (currentSpeaker != SpeakerPosition.right)
				positionChanged = true;
			currentSpeaker = SpeakerPosition.right;
		}



	}

	public void _wm_useAlienFont() {

		usingAlienFont = true;

	}

	public void _wm_useNormalFont() {

		usingAlienFont = false;

	}

	// Use this for initialization
	new void Start () {

		disabledAnswers = new List<int> ();

		currentParent = bubbleContainerRef;

		currentSpeaker = SpeakerPosition.right; // NPC #1

		positionChanged = true;

		topBarStatus = BarStatus.hidden;

		typeHereTransparency = 0.0f;
		targetTypeHereTransparency = 0.0f;

		//leftImageRef.enabled = false;
		//rightImageRef.enabled = false;
		setMiniature(null, null, 0);
		setMiniature (null, null, 1);

		bottomPanelStatus = BarStatus.hidden;

		topBarRef.GetComponent<RectTransform> ().transform.localPosition = new Vector3 (0, HiddenTopY, 0);
		bottomPanelRef.GetComponent<RectTransform> ().transform.localPosition = new Vector3 (0, HiddenBottomY, 0);

		//nextBubbleY = 20.0f;
		nextTextY = 180.0f;
		verticalScroll = 0.0f;
		targetScroll = 0.0f;
		maxScroll = 0.0f;

		blobColor = new Vector4 (1.0f, 1.0f, 1.0f, 1.0f);

		originalPosition = this.transform.position;


		answerElements = new List<GameObject> ();
		sayElements = new List<GameObject> ();
		elements = sayElements;

		clear();

		masterController = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = masterController.getStorage ();

		base.Start ();
	
	}

	public void disableAnswer(int n) {

		disabledAnswers.Add (n);

	}

	public void enableAnswer(int n) {

		disabledAnswers.Remove (n);

	}

	public void disableAllAnswers() {

		for (int i = 0; i < maxAnswers; ++i) {
			disabledAnswers.Add (i);
		}

	}
		
	public void resetAnswers() {

		disabledAnswers.Clear();

	}

	public void enableAllAnswers() {

		disabledAnswers.Clear ();

	}


	/*
	 * 
	 * Clear all bubbles and elements
	 * 
	 */
	void clear() {

		_wm_hideTopBar ();
		_wm_hideBottomPanel ();

		verticalScroll = 0.0f;
		targetScroll = 0.0f;
		Vector3 pos = new Vector3 (originalPosition.x, originalPosition.y + verticalScroll, originalPosition.z);
		bubbleContainerRef.transform.position = pos;

		for (int i = 0; i < elements.Count; ++i) {
			Destroy (elements [i]);
		}

		//nextBubbleY = 20.0f;
		nextTextY = 180.0f;
		verticalScroll = 0.0f;
		targetScroll = 0.0f;
		maxScroll = 0.0f;
		prevBubbleHeight = 0.0f;

		originalPosition = this.transform.position;

		positionChanged = true;

		sayElements = new List<GameObject> ();
		elements = sayElements;

	}
	
	// Update is called once per frame
	new void Update () {

		float conversionFactor = Screen.height / 600.0f;
	
		if (verticalScroll < targetScroll) {

			verticalScroll += SCROLLSPEED;
			if (verticalScroll > targetScroll) {
				verticalScroll = targetScroll;
			}
			Vector3 pos = new Vector3 (originalPosition.x, originalPosition.y + verticalScroll*conversionFactor, originalPosition.z);
			bubbleContainerRef.transform.position = pos;

		}
		if (verticalScroll > targetScroll) {

			verticalScroll -= SCROLLSPEED;
			if (verticalScroll < targetScroll) {
				verticalScroll = targetScroll;
			}
			Vector3 pos = new Vector3 (originalPosition.x, originalPosition.y + verticalScroll*conversionFactor, originalPosition.z);
			bubbleContainerRef.transform.position = pos;

		}
		Vector3 newPos;
		if (topY > targetTopY) {

			topY -= panelYSpeed * Time.deltaTime;
			if (topY < targetTopY) {
				topBarStatus = BarStatus.showing;
				topY = targetTopY;

				newPos = topBarRef.GetComponent<RectTransform> ().transform.localPosition;
				newPos.y = topY;
				topBarRef.GetComponent<RectTransform> ().transform.localPosition = newPos;
			} else {

				newPos = topBarRef.GetComponent<RectTransform> ().transform.localPosition;
				newPos.y = 
					topY;
				topBarRef.GetComponent<RectTransform> ().transform.localPosition = newPos;
			}

		}

		if (topY < targetTopY) {

			topY += panelYSpeed * Time.deltaTime;
			if (topY > targetTopY) {
				topBarStatus = BarStatus.hidden;
				topY = targetTopY;
			}


			newPos = topBarRef.GetComponent<RectTransform> ().transform.localPosition;
			newPos.y = topY;
			topBarRef.GetComponent<RectTransform> ().transform.localPosition = newPos;

		}
			

		if (bottomY > targetBottomY) {

			bottomY -= panelYSpeed * Time.deltaTime;
			if (bottomY < targetBottomY) {
				bottomPanelStatus = BarStatus.hidden;
				bottomY = targetBottomY;

				newPos = bottomPanelRef.GetComponent<RectTransform> ().transform.localPosition;
				newPos.y = bottomY;
				bottomPanelRef.GetComponent<RectTransform> ().transform.localPosition = newPos;
				if(panelMustNotifyFinishAction) notifyFinishAction ();
				clearAnswers ();

			} else {

				newPos = bottomPanelRef.GetComponent<RectTransform> ().transform.localPosition;
				newPos.y = 
					bottomY;
				bottomPanelRef.GetComponent<RectTransform> ().transform.localPosition = newPos;
			}

		}


		if (bottomY < targetBottomY) {

			bottomY += panelYSpeed * Time.deltaTime;
			if (bottomY > targetBottomY) {
				bottomPanelStatus = BarStatus.showing;
				bottomY = targetBottomY;

				newPos = bottomPanelRef.GetComponent<RectTransform> ().transform.localPosition;
				newPos.y = bottomY;
				bottomPanelRef.GetComponent<RectTransform> ().transform.localPosition = newPos;
			} else {

				newPos = bottomPanelRef.GetComponent<RectTransform> ().transform.localPosition;
				newPos.y = 
					bottomY;
				bottomPanelRef.GetComponent<RectTransform> ().transform.localPosition = newPos;
			}

		}


		if (typeHereTransparency > targetTypeHereTransparency) {

			typeHereTransparency -= (panelYSpeed/100.0f) * Time.deltaTime;
			if (typeHereTransparency < targetTypeHereTransparency) {
				typeHereTransparency = targetTypeHereTransparency;
			}
			Color col = new Vector4(1.0f, 1.0f, 1.0f, 1.0f-typeHereTransparency);
			TypeHereRef.color = col;
			TypeHereTextRef.color = col;

		}


		if (typeHereTransparency < targetTypeHereTransparency) {

			typeHereTransparency += (panelYSpeed/100.0f) * Time.deltaTime;
			if (typeHereTransparency > targetTypeHereTransparency) {
				typeHereTransparency = targetTypeHereTransparency;
			}
			Color col = new Vector4(1.0f, 1.0f, 1.0f, 1.0f-typeHereTransparency);
			TypeHereRef.color = col;
			TypeHereTextRef.color = col;

		}

	}

	public void _wm_setColor(float r, float g, float b) {

		blobColor = new Vector4 (r, g, b, 1.0f);

	}

	public void setColor(Color c) {

		blobColor = c;

	}


	public void _wm_setMiniature(string miniatureLocation, string sprName) {

		//GameObject newMiniGO = new GameObject ();
		//Image newImage = newMiniGO.AddComponent<Image> ();
		Sprite spr = Resources.Load<Sprite> ("Miniatures/"+sprName);
		//newImage.sprite = spr;
		//newMiniGO.transform.parent = currentParent.transform;
		if (miniatureLocation.ToLower ().Equals ("left")) {
			//leftImageRef.sprite = spr;
			//leftImageRef.enabled = true;
		}
		if (miniatureLocation.ToLower ().Equals ("right")) {
			//rightImageRef.sprite = spr;
			//rightImageRef.enabled = true;
		}
		
		

		//DialogueMiniature minRef = newMiniGO.AddComponent<DialogueMiniature> ();
		//minRef.theImage = newImage;

		//elements.Add (newMiniGO);

		//talkerPosition = miniatureLocation;

	}

	public void _wm_clear() {

		clear();
		if(topBarStatus != BarStatus.hidden) _wm_hideTopBar ();

	}

	/*
	 * Spawns several Text objects, one per answer
	 * 
	 */

	public void _wm_ask(params string[] answer) {

		if (speakerPosition != SpeakerPosition.right) {
			_wm_setSpeaker ("right");
			positionChanged = false;
		}

		Color prevColor = blobColor;
		setColor (askBubbleColor);

		_wm_showBottomPanel ();

		answerArray = answer;

		int nAnswers = answer.Length;
		bool b;

		float backupNextY;
		float backupPrevBubbleHeight;
		backupPrevBubbleHeight = prevBubbleHeight;
		prevBubbleHeight = 0.0f;
		backupNextY = nextTextY;
		nextTextY = 350.0f;

		elements = answerElements;

		isAnswerBlob = true;

		for (int i = 0; i < nAnswers; ++i) {
			currentParent = bottomPanelRef;

			//if (b == false) {
				globalAnswerNumber = i+1;

				if (!disabledAnswers.Contains (globalAnswerNumber)) {
					_wm_say (answer [i], true);
				}
			//}

		}

		elements = sayElements;

		isAnswerBlob = false;
		globalAnswerNumber = 0;

		nextTextY = backupNextY;
		prevBubbleHeight = backupPrevBubbleHeight;

		currentParent = bubbleContainerRef;
		setColor (prevColor);

		positionChanged = true;


	}

	public void clearAnswers() {

		int l = answerElements.Count;
		for (int i = 0; i < l; ++i) {
			Destroy (answerElements [i]);
		}

	}

	public void showAnswers() {

		_wm_deployBottomPanel ();

	}



	public void selectAnswer(int a) {

		selectedAnswer = a;
		isAnswerBlob = false;
		_wm_say (answerArray [selectedAnswer - 1], true);
		_wm_hideBottomPanel();

	}



	public void selectVoidAnswer() {

	}



	public void _wm_say(string text, bool touchToContinue, float textSpeed) {

		globalTextSpeed = textSpeed;
		_wm_say (text, touchToContinue);

	}

	public void _wm_say(string text, bool touchToContinue) {

		_wm_say (text, null, touchToContinue);

	}

	public void _wm_say(string text, Sprite spr, bool touchToContinue) {


		if (topBarStatus == BarStatus.hidden) {
			_wm_showTopBar ();
		}

		DialogueElement newElement;

		Image top, middle, bottom;
		Image topLeft, topRight, middleLeft, middleRight, bottomLeft, bottomRight;

		newElement = new DialogueElement ();
		newElement.element = DialogueElementType.text;

		//nextTextY = -295.0f; //////////////// REMOVE

		GameObject newGO = new GameObject ();
		newGO.name = "Dialogue_bubble";


		GameObject newTopGO = new GameObject ();
		GameObject newTopLeftGO = new GameObject ();
		GameObject newTopRightGO = new GameObject ();

		GameObject newMiddleGO = new GameObject ();
		GameObject newMiddleLeftGO = new GameObject ();
		GameObject newMiddleRightGO = new GameObject ();

		GameObject newBottomGO = new GameObject ();
		GameObject newBottomLeftGO = new GameObject ();
		GameObject newBottomRightGO = new GameObject ();



		GameObject newTextGO = new GameObject ();

		GameObject newImageGO = new GameObject ();


		//newGO.transform.localPosition = new Vector3 (0, 0, 0);

		top = newTopGO.AddComponent<Image> ();
		middle = newMiddleGO.AddComponent<Image> ();
		bottom = newBottomGO.AddComponent<Image> ();

		topLeft = newTopLeftGO.AddComponent<Image> ();
		topRight = newTopRightGO.AddComponent<Image> ();

		middleLeft = newMiddleLeftGO.AddComponent<Image> ();
		middleRight = newMiddleRightGO.AddComponent<Image> ();

		bottomLeft = newBottomLeftGO.AddComponent<Image> ();
		bottomRight = newBottomRightGO.AddComponent<Image> ();

		EventTrigger t;
		t = newTopGO.AddComponent<EventTrigger> ();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		switch(globalAnswerNumber) {
		case 1:
			entry.callback.AddListener (delegate {
				selectAnswer (1);
			});
			break;
		case 2:
			entry.callback.AddListener (delegate {
				selectAnswer (2);
			});
			break;
		case 3:
			entry.callback.AddListener (delegate {
				selectAnswer (3);
			});
			break;
		case 4:
			entry.callback.AddListener (delegate {
				selectAnswer (4);
			});
			break;
		case 5:
			entry.callback.AddListener (delegate {
				selectAnswer (5);
			});
			break;
		}
		t.triggers.Add (entry);
		t = newMiddleGO.AddComponent<EventTrigger> ();
		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		switch(globalAnswerNumber) {
		case 1:
			entry.callback.AddListener (delegate {
				selectAnswer (1);
			});
			break;
		case 2:
			entry.callback.AddListener (delegate {
				selectAnswer (2);
			});
			break;
		case 3:
			entry.callback.AddListener (delegate {
				selectAnswer (3);
			});
			break;
		case 4:
			entry.callback.AddListener (delegate {
				selectAnswer (4);
			});
			break;
		case 5:
			entry.callback.AddListener (delegate {
				selectAnswer (5);
			});
			break;
		}
		t.triggers.Add (entry);
		t = newBottomGO.AddComponent<EventTrigger> ();
		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		switch(globalAnswerNumber) {
		case 1:
			entry.callback.AddListener (delegate {
				selectAnswer (1);
			});
			break;
		case 2:
			entry.callback.AddListener (delegate {
				selectAnswer (2);
			});
			break;
		case 3:
			entry.callback.AddListener (delegate {
				selectAnswer (3);
			});
			break;
		case 4:
			entry.callback.AddListener (delegate {
				selectAnswer (4);
			});
			break;
		case 5:
			entry.callback.AddListener (delegate {
				selectAnswer (5);
			});
			break;
		}
		t.triggers.Add (entry);


		newGO.transform.parent = currentParent.transform;

		if (positionChanged) {
			switch(currentSpeaker) {
			case SpeakerPosition.left:
                topLeft.sprite = LeftPoint; //Resources.Load<Sprite> ("Dialogue/ok/LeftTopPoint");
                topRight.sprite = TopRight;  //Resources.Load<Sprite> ("Dialogue/ok/RightTopCorner");
				break;
			case SpeakerPosition.right:
                topLeft.sprite = TopLeft;//Resources.Load<Sprite> ("Dialogue/ok/LeftTopCorner");
                topRight.sprite = RightPoint; //Resources.Load<Sprite> ("Dialogue/ok/RightTopPoint");
				break;
			case SpeakerPosition.center:
                topLeft.sprite = TopLeft; //.Load<Sprite> ("Dialogue/ok/LeftTopCorner");
                topRight.sprite = TopRight; //.Load<Sprite> ("Dialogue/ok/RightTopCorner");
				break;
			}

		} else {
            topLeft.sprite = TopLeft; //Resources.Load<Sprite> ("Dialogue/ok/LeftTopCorner");
            topRight.sprite = TopRight; //Resources.Load<Sprite> ("Dialogue/ok/RightTopCorner");
		}
        top.sprite = TopCenter; //Resources.Load<Sprite> ("Dialogue/ok/TopCenter");

        middle.sprite = MiddleCenter;  //Resources.Load<Sprite> ("Dialogue/ok/CenterCenter");
        middleLeft.sprite = MiddleLeft;  //Resources.Load<Sprite> ("Dialogue/ok/LeftCenter");
        middleRight.sprite = MiddleRight;  //Resources.Load<Sprite> ("Dialogue/ok/RightCenter");

        bottom.sprite = BottomCenter; //.Load<Sprite> ("Dialogue/ok/BottomCenter");

        bottomLeft.sprite = BottomLeft; //.Load<Sprite> ("Dialogue/ok/LeftBottomCorner");
        bottomRight.sprite = BottomRight;   //Resources.Load<Sprite> ("Dialogue/ok/RightBottomCorner");

		Image newImage = null;
		if (spr != null) {
			newImage = newImageGO.AddComponent<Image> ();
			newImage.sprite = spr;

		}

		Text newText = newTextGO.AddComponent<Text> ();
		newText.text = text;
		newText.fontSize = 26;

		t = newTextGO.AddComponent<EventTrigger> ();
		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		switch(globalAnswerNumber) {
		case 1:
			entry.callback.AddListener (delegate {
				selectAnswer (1);
			});
			break;
		case 2:
			entry.callback.AddListener (delegate {
				selectAnswer (2);
			});
			break;
		case 3:
			entry.callback.AddListener (delegate {
				selectAnswer (3);
			});
			break;
		case 4:
			entry.callback.AddListener (delegate {
				selectAnswer (4);
			});
			break;
		case 5:
			entry.callback.AddListener (delegate {
				selectAnswer (5);
			});
			break;
		}
		t.triggers.Add (entry);

		DialogueBlob newBlob = newGO.AddComponent<DialogueBlob> ();
		newBlob.dialogueObject = this;
		newBlob.readerMode = readerMode;

		switch(currentSpeaker) {
		case SpeakerPosition.left:
			newBlob.theColor = leftColor;
			break;
		case SpeakerPosition.right:
			newBlob.theColor = rightColor;
			break;
		default:
			newBlob.theColor = centerColor;
			break;

		}

		newBlob.touchToContinue = touchToContinue;

		newBlob.isAnswerBlob = isAnswerBlob;

		newBlob.touchImage_0 = pressToContinue [0];
		newBlob.touchImage_1 = pressToContinue [1];

		top.transform.parent = newGO.transform;
		middle.transform.parent = newGO.transform;
		bottom.transform.parent = newGO.transform;
		topLeft.transform.parent = newGO.transform;
		middleLeft.transform.parent = newGO.transform;
		bottomLeft.transform.parent = newGO.transform;
		topRight.transform.parent = newGO.transform;
		middleRight.transform.parent = newGO.transform;
		bottomRight.transform.parent = newGO.transform;

		newTextGO.transform.parent = newGO.transform;

		newBlob.theText = newText;
		newBlob.textSpeed = globalTextSpeed;
		newBlob.text = text;


		if (!usingAlienFont) { // Understandable
            newText.font = NormalFont;  //Resources.Load<Font> ("Fonts/GNUOLANE");
		} else { // non-understandable
            newText.font = AlienFont; //.Load<Font> ("Fonts/PreAlphabet");
		}
		newText.color = Color.black;

		newText.alignment = TextAnchor.MiddleCenter;
		//newText.transform.position = new Vector3 (0, 0, 0);

		newBlob.theText = newText;


		float thisBubbleWidth;

		const float avgFontWidth = 14.0f; // WARNING Magic??

		float textWidth = text.Length * avgFontWidth;

		thisBubbleWidth = textWidth;

		RectTransform rect = null;
		if (spr != null) {
			rect = newImage.GetComponent<RectTransform> ();
			thisBubbleWidth = rect.sizeDelta.x;
		}
		//float MaxBubbleWidth = 600.0f;
		int nLines = (int)(textWidth / MaxBubbleWidth);
		thisBubbleHeight = 24.0f + 26.0f * nLines;


		if (textWidth < MinBubbleWidth)
			thisBubbleWidth = MinBubbleWidth;

		if (textWidth > MaxBubbleWidth) {

			thisBubbleWidth = MaxBubbleWidth;
			//int nExtraLines = ((int)(textWidth / MaxBubbleWidth));
			//thisBubbleHeight += nExtraLines * avgFontWidth; // also height

		}

		if (spr != null) {
			
			thisBubbleHeight = 16.0f + rect.sizeDelta.y;
		}
			
		//nextTextY = 0.0f;
		//targetScroll = 200.0f * 0.55f;

		if (prevBubbleHeight > 0.0f) {
			nextTextY -= ((thisBubbleHeight + 50.0f) / 2.0f + (prevBubbleHeight + 50.0f) / 2.0f);
			if (!positionChanged) {
				nextTextY += 6.0f;
			}
			positionChanged = false;
		} else {
			nextTextY -= (thisBubbleHeight + 50.0f) / 2.0f;
			positionChanged = false;
		}

		while ((nextTextY - (thisBubbleHeight + 50.0f)/2.0f + maxScroll) < -225) {
			maxScroll += 4.0f;
		}
		if (autoscroll) {
			targetScroll = maxScroll;	
		}

		newBlob.centerYPos = nextTextY;
		newBlob.bubbleHeight = thisBubbleHeight + 50.0f;

		//nextTextY -= (thisBubbleHeight+50.0f)/2.0f; //(thisBubbleHeight + 24.0f - nLines*5.0f) / 2.0f; // compensate for centered coordinates

		//nextTextY = 0.0f;
		/*newGO.AddComponent<RectTransform> ();

		newGO.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		newGO.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		newGO.GetComponent<RectTransform> ().pivot = new Vector2 (0, 0);*/

		newGO.name = newGO.name + "(H=" + thisBubbleHeight + ")";

		//float www;
		//www = Screen.width;
		//newGO.transform.localPosition = new Vector3 (0, nextTextY, 0);
		//newGO.GetComponent<RectTransform> ().anchoredPosition = new Vector2(0, nextTextY);
		if (currentSpeaker == SpeakerPosition.left) {
			//newGO.transform.localPosition = new Vector3 (-((float)Screen.width), 0, 0);// + thisBubbleWidth/2.0f + 15.0f, 0, 0); // + thisBubbleWidth/2.0f
			newGO.transform.localPosition = new Vector3(-bubbleContainerRef.GetComponent<RectTransform>().rect.width/2.0f + thisBubbleWidth/2.0f + 15.0f, 0, 0);
		}
		if (currentSpeaker == SpeakerPosition.right) {
			newGO.transform.localPosition = new Vector3 (bubbleContainerRef.GetComponent<RectTransform>().rect.width/2.0f - thisBubbleWidth/2.0f - 86.0f, 0, 0);
		}
		if (currentSpeaker == SpeakerPosition.center) {
			newGO.transform.localPosition = new Vector3 (0, 0, 0);
		}
		newTextGO.transform.localPosition = new Vector3 (32, nextTextY, 0);
		newTopGO.transform.localPosition = new Vector3 (32, 24.0f/2.0f+thisBubbleHeight/2.0f+nextTextY, 0);
		newMiddleGO.transform.localPosition = new Vector3 (32, nextTextY, 0);
		newBottomGO.transform.localPosition = new Vector3 (32, -24.0f/2.0f-thisBubbleHeight/2.0f+nextTextY, 0);

		newTopLeftGO.transform.localPosition = new Vector3 (-thisBubbleWidth/2.0f+32.0f/2.0f, 24.0f/2.0f+thisBubbleHeight/2.0f+nextTextY, 0);
		newMiddleLeftGO.transform.localPosition = new Vector3 (-thisBubbleWidth/2.0f+32.0f/2.0f, nextTextY, 0);
		newBottomLeftGO.transform.localPosition = new Vector3 (-thisBubbleWidth/2.0f+32.0f/2.0f, -24.0f/2.0f-thisBubbleHeight/2.0f+nextTextY, 0);

		newTopRightGO.transform.localPosition = new Vector3 (32.0f+32.0f/2.0f+thisBubbleWidth/2.0f, 24.0f/2.0f+thisBubbleHeight/2.0f+nextTextY, 0);
		newMiddleRightGO.transform.localPosition = new Vector3 (32.0f+32.0f/2.0f+thisBubbleWidth/2.0f, nextTextY, 0);
		newBottomRightGO.transform.localPosition = new Vector3 (32.0f+32.0f/2.0f+thisBubbleWidth/2.0f, -24.0f/2.0f-thisBubbleHeight/2.0f+nextTextY, 0);

		newText.GetComponent<RectTransform> ().sizeDelta = new Vector2 (thisBubbleWidth * 0.9f, 90.0f+thisBubbleHeight);
		newTopGO.GetComponent<RectTransform> ().sizeDelta = new Vector2 (thisBubbleWidth, 24);
		newMiddleGO.GetComponent<RectTransform> ().sizeDelta = new Vector2 (thisBubbleWidth, thisBubbleHeight);
		newBottomGO.GetComponent<RectTransform> ().sizeDelta = new Vector2 (thisBubbleWidth, 24);

		newTopLeftGO.GetComponent<RectTransform> ().sizeDelta = new Vector2 (32, 24);
		newMiddleLeftGO.GetComponent<RectTransform> ().sizeDelta = new Vector2 (32, thisBubbleHeight);
		newBottomLeftGO.GetComponent<RectTransform> ().sizeDelta = new Vector2 (32, 24);

		newTopRightGO.GetComponent<RectTransform> ().sizeDelta = new Vector2 (32, 24);
		newMiddleRightGO.GetComponent<RectTransform> ().sizeDelta = new Vector2 (32, thisBubbleHeight);
		newBottomRightGO.GetComponent<RectTransform> ().sizeDelta = new Vector2 (32, 24);

		if (spr != null) {
			
			newImageGO.transform.parent = newGO.transform;
			newImageGO.transform.localPosition = new Vector3 (32, nextTextY, 0);
		}

		newBlob.top = top;
		newBlob.middle = middle;
		newBlob.bottom = bottom;
		newBlob.topLeft = topLeft;
		newBlob.middleLeft = middleLeft;
		newBlob.bottomLeft = bottomLeft;
		newBlob.topRight = topRight;
		newBlob.middleRight = middleRight;
		newBlob.bottomRight = bottomRight;
		newBlob.theText = newText;
		newBlob.theParent = this;

		newElement.data = text;

		elements.Add (newGO);
		elements.Add (newTopGO);
		elements.Add (newMiddleGO);
		elements.Add (newBottomGO);
		elements.Add (newTextGO);


		prevBubbleHeight = thisBubbleHeight;


	}

	public void _wa_ask(WisdominiObject waiter, string[] answers) {

		this.waitingRef = waiter;
		_wm_ask (answers);

	}

	public void _wa_say(WisdominiObject waiter, string text, bool touchToContinue) {

		this.waitingRef = waiter;
		_wm_say(text, touchToContinue);

	}

	public void _wa_say(WisdominiObject waiter, Sprite spr, bool touchToContinue) {

		this.waitingRef = waiter;
		_wm_say("", spr, touchToContinue);

	}

	/*public void _wa_setMiniature(WisdominiObject waiter, int miniatureLocation, string sprName) {

		this.waitingRef = waiter;
		_wm_setMiniature(miniatureLocation, sprName);

	}*/
		

	public void textHasFinishedRendering() {


		notifyFinishAction ();

	}

	public float getMaxScroll() {
		return maxScroll;
	}

	public float getMinScroll() {
		return 0.0f;
	}

	public void setScroll(float scroll) {

		targetScroll = scroll;
		if (targetScroll > maxScroll)
			targetScroll = maxScroll;
		if (targetScroll < minScroll)
			targetScroll = minScroll;

	}

	public void setMiniature(Texture normal, Texture blink, int pos) {

		if (pos == 0) {

			npc0Blinker.normal = normal;
			npc0Blinker.blink = blink;
			npc0Blinker.reload ();

		} else if (pos == 1) {
			
			npc1Blinker.normal = normal;
			npc1Blinker.blink = blink;
			npc1Blinker.reload ();

		}

	}



	
}
