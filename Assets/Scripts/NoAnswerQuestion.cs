using UnityEngine;
using System.Collections;

public enum NoAnswerQuestionState { idle, transitioningIn, transitioningOut };

public class NoAnswerQuestion : MonoBehaviour {

	/* references */

	TextMesh theText;


	/* public properties */

	public float transitionInSpeed = 3.0f;
	public float transitionOutSpeed = 3.0f;

	public string testString = "";


	/* properties */

	string text;
	float shownLength;
	string[] charColor;
	string[] reverseCharColor;
	float transitionSpeed;
	float timeToTransitionOut = 0.0f;
	float elapsedTime = 0.0f;
	NoAnswerQuestionState state;
	float scale;
	float xAngle;


	/* public properties */

	public float scaleSpeed = 0.1f;
	public float xAngleSpeed = 2.0f;


	/* constants */

	const string c0 = "#000000ff";
	const string c1 = "#000000bb";
	const string c2 = "#00000088";
	const string c3 = "#00000055";
	const string c4 = "#00000022";
	const string c5 = "#00000000";
	public float minScale = 0.09f;

	const int nColors = 6;


	void Start() {

		initialize ();

	}

	// Use this for initialization
	public void initialize () {
	
		theText = this.GetComponent<TextMesh> ();
		theText.text = "";
		charColor = new string[nColors];
		reverseCharColor = new string[nColors];
		charColor [0] = c0;
		charColor [1] = c1;
		charColor [2] = c2;
		charColor [3] = c3;
		charColor [4] = c4;
		charColor [5] = c5;
		reverseCharColor [0] = c5;
		reverseCharColor [1] = c4;
		reverseCharColor [2] = c3;
		reverseCharColor [3] = c2;
		reverseCharColor [4] = c1;
		reverseCharColor [5] = c0;
		scale = minScale;
		xAngle = 45.0f;
		shownLength = 0.0f;



		state = NoAnswerQuestionState.transitioningIn;

		/*if (!testString.Equals ("")) {
			setText (testString);
			transitionIn();
		}*/

	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 newScale = new Vector3 (scale, scale, scale);

		scale += scaleSpeed * Time.deltaTime;

		xAngle += xAngleSpeed * Time.deltaTime;

		this.transform.localScale = newScale;
		this.transform.rotation = Quaternion.Euler (xAngle, 0, 0);

		if (state == NoAnswerQuestionState.transitioningIn) {

			int nLetters = (int)shownLength;

			if (nLetters <= text.Length) {
				shownLength += transitionInSpeed * Time.deltaTime;
				nLetters = (int)shownLength;
				if (nLetters > text.Length) {
					nLetters = text.Length; // end transition
					shownLength = 0.0f;
					state = NoAnswerQuestionState.idle;
				}
			}


			if (nLetters < nColors) { // starting case


				theText.text = "";

				for (int i = 0; i < nLetters; ++i) {

					theText.text += "<color=" + charColor [nColors - nLetters + i] + ">" + text.Substring (i, 1) +
							"</color>";

				}

				theText.text += "<color=#00000000>" + text.Substring (nLetters, text.Length - nLetters) +
						"</color>";

			}

			if ((nLetters >= nColors) && (nLetters < (text.Length - nColors))) { // middle case

				theText.text = "<color=#000000ff>" + text.Substring (0, nLetters - nColors) + "</color>";
				for (int i = 0; i < nColors; ++i) {

					theText.text += "<color=" + charColor [i] + ">" + text.Substring (nLetters-nColors + i, 1) +
						"</color>";

				}
				theText.text += "<color=#00000000>" + text.Substring (nLetters, text.Length - nLetters) +
					"</color>";

			}

			if(nLetters >= (text.Length - nColors)) { // ending case

				theText.text = "<color=#000000ff>" + text.Substring (0, nLetters) + "</color>";
				for (int i = 0; i < text.Length-nLetters; ++i) {

					theText.text += "<color=" + charColor [i] + ">" + text.Substring (nLetters + i, 1) +
						"</color>";

				}

			}


		}

		if (state == NoAnswerQuestionState.transitioningOut) {


			int nLetters = (int)shownLength;

			if (nLetters <= text.Length) {
				shownLength += transitionInSpeed * Time.deltaTime;
				nLetters = (int)shownLength;
				if (nLetters > text.Length) {
					nLetters = text.Length; // end transition
					state = NoAnswerQuestionState.idle;
					Destroy (this.gameObject);
				}
			}


			if (nLetters < nColors) { // starting case


				theText.text = "";

				for (int i = 0; i < nLetters; ++i) {

					theText.text += "<color=" + reverseCharColor [nColors - nLetters + i] + ">" + text.Substring (i, 1) +
						"</color>";

				}

				theText.text += "<color=#000000ff>" + text.Substring (nLetters, text.Length - nLetters) +
					"</color>";

			}

			if ((nLetters >= nColors) && (nLetters < (text.Length - nColors))) { // middle case

				theText.text = "<color=#00000000>" + text.Substring (0, nLetters - nColors) + "</color>";
				for (int i = 0; i < nColors; ++i) {

					theText.text += "<color=" + reverseCharColor [i] + ">" + text.Substring (nLetters-nColors + i, 1) +
						"</color>";

				}
				theText.text += "<color=#000000ff>" + text.Substring (nLetters, text.Length - nLetters) +
					"</color>";

			}

			if(nLetters >= (text.Length - nColors)) { // ending case

				theText.text = "<color=#00000000>" + text.Substring (0, nLetters) + "</color>";
				for (int i = 0; i < text.Length-nLetters; ++i) {

					theText.text += "<color=" + reverseCharColor [i] + ">" + text.Substring (nLetters + i, 1) +
						"</color>";

				}

			}

		}

		if (state == NoAnswerQuestionState.idle) {

			if (timeToTransitionOut > 0.0f) {

				elapsedTime += Time.deltaTime;
				if (elapsedTime > timeToTransitionOut) {

					state = NoAnswerQuestionState.transitioningOut;

				}

			}

		}

	}

	public void setText(string txt) {

		text = txt;
	
	}


	public void transitionIn() {
	
		state = NoAnswerQuestionState.transitioningIn;


	}

	public void setAutoTransitionOut(float t) {

		timeToTransitionOut = t;

	}




	public void transitionOut() {

		state = NoAnswerQuestionState.transitioningOut;

	}
}
