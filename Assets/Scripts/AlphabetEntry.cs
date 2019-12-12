using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

enum AlphabetEntryState { delaying, deploying, idle, blinking }

public class AlphabetEntry : MonoBehaviour {

	/* references */

	public AlphabetController theController;
	public GameObject theParent;
	public DialogueObject dialogueController;

	/* public properties */

	public StoredConversation theConversation;
	public int index;
	public float timeDelay;
	public float yPos;
	public float targetOpacity;

	/* properties */

	AlphabetEntryState state;

	float elapsedTime;
	float xPos;
	GameObject barGO;
	GameObject titleGO;
	Image barImage;
	Text titleText;
	RectTransform rect;
	RectTransform textRect;
	float opacity;
	Vector4 colVec;
	Color textColor;

	/* constants */

	float startX;
	const float targetX = 0.0f;
	const float xSpeed = 35.0f;
	const float blinkTime = 0.66f;

	// Use this for initialization
	public void initialize () {
	
		targetOpacity = opacity = 1.0f;
		RectTransform parentRect = theParent.GetComponent<RectTransform> ();
		state = AlphabetEntryState.delaying;
		barGO = new GameObject ();
		barGO.transform.parent = theParent.transform;
		titleGO = new GameObject ();
		titleGO.transform.parent = barGO.transform;
		barImage = barGO.AddComponent<Image> ();
		barImage.color = new Color (0.2f, 0.8f, 0.7f, 0.5f);
		colVec = new Vector4 (barImage.color.r, barImage.color.g, barImage.color.b, barImage.color.a);
		titleText = titleGO.AddComponent<Text> ();
		titleText.text = theConversation.title;
		rect = barGO.GetComponent<RectTransform> ();
		textRect = titleGO.GetComponent<RectTransform> ();
		//rect.localPosition = new Vector2 (5, 5);
		rect.sizeDelta = new Vector2(Screen.width, 50);
		textRect.sizeDelta = new Vector2 (800, 50);
		titleText.font = Resources.Load<Font> ("Fonts/GNUOLANE");
		titleText.alignment = TextAnchor.MiddleCenter;
		titleText.fontSize = 18;
		textColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		titleText.color = textColor;
		elapsedTime = 0.0f;
		EventTrigger evt = barGO.AddComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback = new EventTrigger.TriggerEvent();
		entry.callback.AddListener((eventData) => conversationClickedOn());
		evt.triggers.Add (entry);
		//evt.OnPointerClick.AddListener(() =>  conversationClickedOn() );

		startX = -parentRect.rect.width;
		xPos = startX;
		Vector2 newPos = new Vector2 (xPos, yPos);
		rect.localPosition = newPos;

	}
	
	// Update is called once per frame
	void Update () {
	
		Vector2 newPos;

		if (state == AlphabetEntryState.delaying) {

			elapsedTime += Time.deltaTime;
			if (elapsedTime > timeDelay) {
				state = AlphabetEntryState.deploying;
				elapsedTime = 0.0f;
			}

		}

		if (state == AlphabetEntryState.deploying) {
			xPos += xSpeed;
			if (xPos > targetX) {
				xPos = targetX;
				state = AlphabetEntryState.idle;
			}

			newPos = new Vector2 (xPos, yPos);
		
			rect.localPosition = newPos;
		}
		if (state == AlphabetEntryState.idle) {



		}
		if (state == AlphabetEntryState.blinking) {

			int count;

			count = (int)(elapsedTime * 1000.0f);

			count = count % 80;

			if (count < 40) {
				barImage.enabled = true;
				titleText.enabled = true;
			} else {
				barImage.enabled = false;
				titleText.enabled = false;
			}

			elapsedTime += Time.deltaTime;

			if (elapsedTime > blinkTime) {
				barImage.enabled = false;
				titleText.enabled = false;
				elapsedTime = 0.0f;
				state = AlphabetEntryState.idle;


				/* pour out the conversation into the dialogueController */
				dialogueController.setScroll (-75.0f);
				dialogueController.autoscroll = false;
				for (int i = 0; i < theConversation.textList.Count; ++i) {
					if (theConversation.theSpeaker [i] == StoredConversationSpeaker.npc0) {
						dialogueController._wm_setSpeaker ("left");
					} else {
						dialogueController._wm_setSpeaker ("right");
					}
					dialogueController._wm_say (theConversation.textList [i], false, 0);
				}

			}

		}

		if (opacity < targetOpacity) {

			opacity += 1.6f * Time.deltaTime;
			if (opacity > targetOpacity) {
				opacity = targetOpacity;
			}
			barImage.color = new Vector4 (colVec.x, colVec.y, colVec.z,
				colVec.w * opacity);
			
			titleText.color = new Vector4 (textColor.r, textColor.g, textColor.b, textColor.a * opacity);

		}

		if (opacity > targetOpacity) {

			opacity -= 1.6f * Time.deltaTime;
			if (opacity < targetOpacity) {
				opacity = targetOpacity;
			}
			barImage.color = new Vector4 (colVec.x, colVec.y, colVec.z,
				colVec.w * opacity);

			titleText.color = new Vector4 (textColor.r, textColor.g, textColor.b, textColor.a * opacity);

		}


	}

	public void blink() {

		state = AlphabetEntryState.blinking;

	}

	public void reset() {

		initialize ();

	}

	public void conversationClickedOn() {

		int a;

		a = index;

		theController.clickNotification (index);
	}

}
