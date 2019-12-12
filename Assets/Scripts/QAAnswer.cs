using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum QAAnswerState { delaying, poppingUp, poppingUp2, poppingUp3, idle, ready,
	changingColor, idle2, disappearing, blinkingTrue, blinkingFalse };

public class QAAnswer : MonoBehaviour {



	/* references */

	public QAController controller;
	Text theText;



	/* public properties */

	public int answerNumber;
	public QAAnswerState state;


	/* properties */

	float delay;
	float elapsedTime;
	float opacity;
	float scale;
	RectTransform rect;
	bool visible;
	Color targetColor;
	Color currentColor;
	float lerpParam;
	int nblinks;

	/* constants */

	const float minScale = 0.25f;
	const float opacitySpeed = 0.6f;
	const float scaleSpeed = 1.2f;
	const float blinkTime = 0.05f;
	const float lerpSpeed = 6.0f;
	const int maxblinks = 10;
	public float interAnswerDistance = 65.0f;
	public float vertAdjust = 250.0f;

	public void initialize(float yPos) {
		
		theText = this.GetComponent<Text> ();
		elapsedTime = 0.0f;
		opacity = 0.0f;
		theText.color = new Color (1, 1, 1, opacity);
		state = QAAnswerState.delaying;
		rect = this.GetComponent<RectTransform> ();
		rect.transform.position = new Vector2 (0, 0);
		rect.anchoredPosition = new Vector2 (0, yPos);
		rect.anchorMax = new Vector2 (1, 7.5f/600.0f);
		rect.anchorMin = new Vector2 (0, 0);
		if (controller == null) {
			controller = GameObject.Find ("QAController").GetComponent<QAController>();
		}
	}
	/*
	public void initialize() {

		theText = this.GetComponent<Text> ();
		elapsedTime = 0.0f;
		opacity = 0.0f;
		theText.color = new Color (1, 1, 1, opacity);
		state = QAAnswerState.delaying;
		rect = this.GetComponent<RectTransform> ();
		rect.transform.position = new Vector2 (0, 0);
		rect.anchoredPosition = new Vector2 (0, -answerNumber * interAnswerDistance + vertAdjust);
		rect.anchorMax = new Vector2 (1, 7.5f/600.0f);
		rect.anchorMin = new Vector2 (0, 0);
		if (controller == null) {
			controller = GameObject.Find ("QAController").GetComponent<QAController>();
		}


	}*/

	// Use this for initialization
	void Start () {
	
		//initialize ();

	}
	
	// Update is called once per frame
	void Update () {
	
		elapsedTime += Time.deltaTime;
	
		if (state == QAAnswerState.delaying) {

			if (elapsedTime > delay) {

				scale = minScale;
				state = QAAnswerState.poppingUp;

			}

		}

		if (state == QAAnswerState.poppingUp) {

			Vector3 newScale = new Vector3 (scale, scale, scale);
			theText.color = new Color (1, 1, 1, opacity);
			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > 1.0f)
				opacity = 1.0f;
			scale += scaleSpeed * Time.deltaTime;
			rect.transform.localScale = newScale;

			if (scale > 1.15) {
				state = QAAnswerState.poppingUp2;
			}

		}

		if (state == QAAnswerState.poppingUp2) {

			Vector3 newScale = new Vector3 (scale, scale, scale);
			rect.transform.localScale = newScale;
			theText.color = new Color (1, 1, 1, opacity);
			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > 1.0f)
				opacity = 1.0f;
			scale -= scaleSpeed/1.3f * Time.deltaTime;

			if (scale < 0.92f) {
				newScale = new Vector3 (scale, scale, scale);
				rect.transform.localScale = newScale;
				state = QAAnswerState.poppingUp3;
			}

		}

		if (state == QAAnswerState.poppingUp3) {

			Vector3 newScale = new Vector3 (scale, scale, scale);
			rect.transform.localScale = newScale;
			theText.color = new Color (1, 1, 1, opacity);
			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > 1.0f)
				opacity = 1.0f;
			scale += scaleSpeed/1.6f * Time.deltaTime;

			if (scale > 1.0f) {
				scale = 1.0f;
				newScale = new Vector3 (scale, scale, scale);
				rect.transform.localScale = newScale;
				state = QAAnswerState.idle;
			}

			nblinks = 0;

		}

		if (state == QAAnswerState.idle) {



		}

		if (state == QAAnswerState.disappearing) {

			Color c = theText.color;
			c.a = opacity;
			theText.color = c;
			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < 0.0f) {
				Destroy (this.gameObject);
			}

		}

		if (state == QAAnswerState.blinkingTrue) {

			lerpParam += lerpSpeed * Time.deltaTime;

			if (lerpParam > 1.0f) {
				lerpParam = 1.0f;

			}

			currentColor = Color.Lerp (Color.white, targetColor, lerpParam);
			if (!visible)
				currentColor.a = 0.0f;

			theText.color = currentColor;

			if (elapsedTime > blinkTime) {

				elapsedTime = 0.0f;
				visible = !visible;
				++nblinks;
				if (nblinks > maxblinks)
					state = QAAnswerState.disappearing;

			}

		}

		if (state == QAAnswerState.blinkingFalse) {

			lerpParam += lerpSpeed * Time.deltaTime;

			if (lerpParam > 1.0f) {
				lerpParam = 1.0f;
				//state = QAAnswerState.idle;
			}

			currentColor = Color.Lerp (Color.white, targetColor, lerpParam);
			if (!visible)
				currentColor.a = 0.0f;

			theText.color = currentColor;

			if (elapsedTime > blinkTime) {

				elapsedTime = 0.0f;
				visible = !visible;
				++nblinks;
				if (nblinks > maxblinks)
					state = QAAnswerState.disappearing;

			}
		}

	}

	public void setText(string t) {

		theText.text = t;

	}

	public void notifyAnswer() {

		if (state == QAAnswerState.ready)
		controller.answerSelected (answerNumber);

	}

	public void setDelay(float d) {

		delay = d;

	}

	public void dispose() {

		state = QAAnswerState.disappearing;

	}

	public void blink(bool correct) {


		elapsedTime = 0.0f;
		visible = true;
		lerpParam = 0.0f;
		if (correct) {
			state = QAAnswerState.blinkingTrue;
			targetColor = Color.green;
		} else {
			state = QAAnswerState.blinkingFalse;
			targetColor = Color.red;
		}

	}

}
