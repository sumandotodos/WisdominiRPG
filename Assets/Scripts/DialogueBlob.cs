using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueBlob : MonoBehaviour {



	/* public properties */

	public string text;
	public float textSpeed;
	public Color theColor;
	public bool isAnswerBlob = false;
	public bool touchToContinue;

	public DialogueObject dialogueObject;

	public float centerYPos;
	public float bubbleHeight;

	/* properties */

	float elapsedTime = 0.0f;
	int fNumber = 0;
	bool finished = false;
	private float scale;
	private float shownStringLength;
	bool finishOnce = false;
	float textSpeedMultiplier = 1.0f;

	public float LOOKATME;

	public bool readerMode = false;

	/* references */

	public DialogueObject theParent;
	public Image top;
	public Image middle;
	public Image bottom;
	public Image topLeft;
	public Image middleLeft;
	public Image bottomLeft;
	public Image topRight;
	public Image middleRight;
	public Image bottomRight;
	public Sprite touchImage_0, touchImage_1;
	public Image touch;
	public Text theText;

	bool renderingEnabled = true;

	/* constants */

	const float INITIALSCALE = 0.5f;
	const float SCALESPEED = 0.05f;


	public void disableRendering() {

		top.enabled = false;
		middle.enabled = false;
		bottom.enabled = false;
		topLeft.enabled = false;
		middleLeft.enabled = false;
		bottomLeft.enabled = false;
		topRight.enabled = false;
		middleRight.enabled = false;
		bottomRight.enabled = false;
		touch.enabled = false;
		theText.enabled = false;
		renderingEnabled = false;


	}

	// Use this for initialization
	void Start () {

		renderingEnabled = true;

		scale = INITIALSCALE;
		shownStringLength = 0;
		this.transform.localScale = new Vector3 (scale, scale, scale);
		Vector4 col = new Vector4(theColor.r, theColor.g, theColor.b, scale*0.85f);

		top.color = col;
		middle.color = col;
		bottom.color = col;
		topLeft.color = col;
		middleLeft.color = col;
		bottomLeft.color = col;
		topRight.color = col;
		middleRight.color = col;
		bottomRight.color = col;
		touch =	this.gameObject.AddComponent<Image> ();
		touch.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);

		touch.sprite = touchImage_0;
		touch.enabled = false;

		textSpeedMultiplier = 1.0f;
	
	}

	
	// Update is called once per frame
	void Update () {

		if (!renderingEnabled)
			return;

		LOOKATME = centerYPos - bubbleHeight + dialogueObject.maxScroll;
		if ((centerYPos - bubbleHeight + dialogueObject.maxScroll > 300.0f) && !isAnswerBlob && !readerMode) {
			disableRendering ();
		}

		if (scale < 1.0f) {
			
			scale += SCALESPEED;
			if (scale >= 1.0f)
				scale = 1.0f;
			this.transform.localScale = new Vector3 (scale, scale, scale);
			Vector4 col = new Vector4(theColor.r, theColor.g, theColor.b, scale*0.85f);

			top.color = col;
			middle.color = col;
			bottom.color = col;
			topLeft.color = col;
			middleLeft.color = col;
			bottomLeft.color = col;
			topRight.color = col;
			middleRight.color = col;
			bottomRight.color = col;


		}
	
		int numberOfCharactersToShow;

		shownStringLength += (textSpeed * textSpeedMultiplier);
		if (textSpeed == 0.0f)
			shownStringLength = text.Length;

		numberOfCharactersToShow = (int)shownStringLength;

		if (Input.GetMouseButtonDown (0)) {
			textSpeedMultiplier = 4.0f;
		}

		if (numberOfCharactersToShow >= text.Length) {
			numberOfCharactersToShow = text.Length;
			theText.text = text;
			finished = true;

			if ((theParent != null) && !touchToContinue) {
				
				if (!isAnswerBlob) {
					theParent.textHasFinishedRendering (); // notify end of action
					theParent = null;
				}
			}
			if (touchToContinue && !finishOnce) {
				//touch.enabled = true;
				elapsedTime = 0.0f;
				fNumber = 0;
				finishOnce = true;
			}
		} else {

			theText.text = text.Substring (0, numberOfCharactersToShow);

		}


		if (finished && touchToContinue) {
			// show touch icon
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 0.33f) {
				fNumber = (fNumber + 1) % 2;
				elapsedTime = 0.0f;
				if (fNumber == 0) {
					touch.sprite = touchImage_0;
				}
				if (fNumber == 1) {
					touch.sprite = touchImage_1;
				}
			}

			if (Input.GetMouseButtonDown (0) && theParent != null) {
				if (!isAnswerBlob) {
					theParent.textHasFinishedRendering ();
					theParent = null;
					touch.enabled = false;
				}
			}
		}

	}
}
