using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

enum AlphabetState { initial, listing, reading, exitting };

public class AlphabetController : WisdominiObject {

	/* references */
	new public Rosetta rosetta;
	public GameObject container;
	public Scrollbar scrollbar;
	public UIFaderScript fader;
	public DialogueObject dialogueController;
	MasterControllerScript mcRef;
	DataStorage ds;

	float lastScroll;
	float deltaY;

	Vector2 clickPosition;

	List<StoredConversation> conversationList;
	List<AlphabetEntry> entryList;

	/* properties */
	int nConversations;
	List<GameObject> conversationBarGO;
	AlphabetState status;


	// Use this for initialization
	new void Start () {

		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();
		ds = mcRef.getStorage ();

		status = AlphabetState.initial;
		scrollbar.gameObject.SetActive (false);
		conversationList = new List<StoredConversation> ();
		entryList = new List<AlphabetEntry> ();

		lastScroll = 0.0f;



		StoredConversation con;
		int numberOfConversations = ds.retrieveIntValue ("NumberOfStoredConversations");
		for (int index = 0; index < numberOfConversations; ++index) {
			con = new StoredConversation();
			con.initialize ();
			string title;
			title = ds.retrieveStringValue ("TitleOfConversation" + index);
			con.setTitle(rosetta.retrieveString(title));
			int nBubbles = ds.retrieveIntValue ("LengthOfConversation" + index);
			for (int jindex = 0; jindex < nBubbles; ++jindex) {
					string contents = ds.retrieveStringValue("Conversation" + index + "_" + jindex);
					StoredConversationSpeaker convSpeak = StoredConversationSpeaker.player;
					string speaker = ds.retrieveStringValue ("Conversation" + index + "_" + jindex + "Speaker");
					if (speaker.Equals ("Player"))
						convSpeak = StoredConversationSpeaker.player;
					if (speaker.Equals ("NPC1"))
						convSpeak = StoredConversationSpeaker.npc0;
					if (speaker.Equals ("NPC2"))
						convSpeak = StoredConversationSpeaker.npc1;
				con.addBubble(rosetta.retrieveString(contents), convSpeak);
			}
			conversationList.Add(con);
		}


		conversationBarGO = new List<GameObject> ();

		for (int i = 0; i < conversationList.Count; ++i) {
			GameObject newBar;
			newBar = new GameObject ();
			AlphabetEntry entry = newBar.AddComponent<AlphabetEntry> ();
			entry.theParent = container;
			entry.yPos = 253 - 106 - i * 100;
			entry.timeDelay = i * 0.25f;
			entry.theConversation = conversationList [i];
			entry.initialize ();
			entry.index = i;
			entry.dialogueController = dialogueController;
			entry.theController = this.GetComponent<AlphabetController> ();
			conversationBarGO.Add (newBar);
			entryList.Add (entry);

			// if the list is too long, enable scrollbar
			if (entry.yPos < (-400 + 100)) {
				//scrollbar.GetComponent<Image>().enabled = true;
				scrollbar.gameObject.SetActive (true);
			}

		}

		this.isWaitingForActionToComplete = false;



	}

	public void clickNotification(int idx) {

		for (int i = 0; i < entryList.Count; ++i) {

			if (entryList [i].index != idx) {
				entryList [i].targetOpacity = 0.0f;
			} else
				entryList [i].blink ();

		}

		status = AlphabetState.reading;

	}
	
	// Update is called once per frame
	new void Update () {

		if (Input.GetMouseButtonDown (0)) {

			clickPosition = Input.mousePosition;

		}

		if (Input.GetMouseButton (0)) {

			Vector2 currentPos = Input.mousePosition;

			deltaY = currentPos.y - clickPosition.y;

			dialogueController.setScroll (lastScroll + deltaY);

		}

		if (Input.GetMouseButtonUp (0)) {

			lastScroll += deltaY;
			if (lastScroll > dialogueController.getMaxScroll())
				lastScroll = dialogueController.getMaxScroll ();
			if (lastScroll < dialogueController.getMinScroll ())
				lastScroll = dialogueController.getMinScroll ();

		}

		if (status == AlphabetState.exitting) {

			if (!isWaitingForActionToComplete) {

				if (mcRef == null)
					return;

				DataStorage ds = mcRef.getStorage ();
				string returnLevel = ds.retrieveStringValue ("ReturnLocation");
				if (!returnLevel.Equals ("")) {
					SceneManager.LoadScene (returnLevel);
				}

			}

		}

		if (scrollbar.gameObject.activeSelf) {


			float viewportHeight = container.GetComponent<RectTransform> ().rect.height - 106;
			float maxHeight = conversationList.Count * 100;
			float scrollvalue = scrollbar.GetComponent<Scrollbar> ().value;
			float yPos = scrollvalue * (maxHeight - viewportHeight);
			Vector3 pos = container.transform.position;
			pos.y = Screen.height/2 + yPos;
			container.transform.position = pos;

		}
	
	}

	public void goBack() {

		if ((status == AlphabetState.initial) || (status == AlphabetState.listing)) {

			fader._wa_fadeOut (this);
			this.isWaitingForActionToComplete = true;
			status = AlphabetState.exitting;

		}

		if (status == AlphabetState.reading) {

			for (int i = 0; i < entryList.Count; ++i) {
				entryList [i].reset ();
			}
			status = AlphabetState.listing;
			dialogueController._wm_clear ();

		}

	}
}
