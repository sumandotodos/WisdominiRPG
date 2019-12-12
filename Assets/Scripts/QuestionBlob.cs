using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestionBlob : MonoBehaviour {

	public string[] answer;

	int answerNumber;

	string text;
	public float textSpeed;

	public Color theColor;

	public DialogueObject theParent;

	public Image top;
	public Image middle;
	public Image bottom;

	public Sprite touchImage_0, touchImage_1;
	public Image touch;

	public bool touchToContinue;

	public Text theText;
	public Text[] textArray;
	bool finished = false;

	private float scale;
	private float shownStringLength;
	bool finishOnce = false;

	const float INITIALSCALE = 0.5f;
	const float SCALESPEED = 0.05f;

	float elapsedTime = 0.0f;
	int fNumber = 0;

	int nAnswer;
	int curAnswer = 0;

	// Use this for initialization
	void Start () {

		answerNumber = 0;

		scale = INITIALSCALE;
		shownStringLength = 0;
		this.transform.localScale = new Vector3 (scale, scale, scale);
		top.color = new Vector4 (theColor.r, theColor.g, theColor.b, scale*0.85f);
		middle.color = new Vector4 (theColor.r, theColor.g, theColor.b, scale*0.85f);
		bottom.color = new Vector4 (theColor.r, theColor.g, theColor.b, scale*0.85f);
		touch =	this.gameObject.AddComponent<Image> ();
		touch.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);

		touch.sprite = touchImage_0;
		touch.enabled = false;

		touchToContinue = true;

		text = answer [0];
		theText = textArray [0];

		nAnswer = answer.Length;


	}

	// Update is called once per frame
	void Update () {

		if (scale < 1.0f) {

			scale += SCALESPEED;
			if (scale >= 1.0f)
				scale = 1.0f;
			this.transform.localScale = new Vector3 (scale, scale, scale);
			top.color = new Vector4 (theColor.r, theColor.g, theColor.b, scale*0.85f);
			middle.color = new Vector4 (theColor.r, theColor.g, theColor.b, scale*0.85f);
			bottom.color = new Vector4 (theColor.r, theColor.g, theColor.b, scale*0.85f);


		}

		int numberOfCharactersToShow;

		shownStringLength += textSpeed;

		numberOfCharactersToShow = (int)shownStringLength;

		if (numberOfCharactersToShow >= text.Length) {
			numberOfCharactersToShow = text.Length;
			theText.text = text;

			++curAnswer;
			if (curAnswer == nAnswer) {
				finished = true;
			

				if ((theParent != null) && !touchToContinue) {


					theParent.textHasFinishedRendering (); // notify end of action

					theParent = null;
				}
				if (touchToContinue && !finishOnce) {
					touch.enabled = true;
					elapsedTime = 0.0f;
					fNumber = 0;
					finishOnce = true;
				}
			} else { // game on, game on...
				theText = textArray[curAnswer];
				text = answer [curAnswer];
				shownStringLength = 0.0f;
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
				theParent.textHasFinishedRendering ();
				theParent = null;
				touch.enabled = false;
			}
		}

	}
}
