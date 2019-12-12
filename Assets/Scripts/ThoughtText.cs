using UnityEngine;
using System.Collections;

enum ThoughtTextState {  scaling1, scaling2, idle, fading };

public class ThoughtText : MonoBehaviour {

	/* properties */

	public bool autorun = true;

	TextMesh theTextMesh;
	public float timeToFadeOut;
	public float elapsedTime;
	ThoughtTextState state;
	float opacity;
	float scale;

	/* constants */

	const float fadeOutSpeed = 0.5f;
	const float scaleInSpeed = 2.75f;
	public float secondsPerChar = 0.4f;
	const float minimumDuration = 2.0f;

	bool started = false;

	// Use this for initialization
	void Start () {
	
		if (started)
			return;
		started = true;
		theTextMesh = this.GetComponent<TextMesh> ();
		elapsedTime = 0.0f;
		state = ThoughtTextState.scaling1;
		opacity = 1.0f;
		scale = 0.0f;
		theTextMesh.transform.localScale = new Vector3 (0, 0, 0);

	}

	public void setText(string txt) {

		Start ();
		theTextMesh.text = txt;
		timeToFadeOut = secondsPerChar * txt.Length;
		if (timeToFadeOut < minimumDuration)
			timeToFadeOut = minimumDuration;
		elapsedTime = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {

		if (!autorun)
			return;
	


		if (state == ThoughtTextState.scaling1) {

			scale += scaleInSpeed * Time.deltaTime;
			if (scale > 1.15f) {

				state = ThoughtTextState.scaling2;

			}
			theTextMesh.transform.localScale = new Vector3 (scale, scale, scale);

		}

		if (state == ThoughtTextState.scaling2) {

			scale -= (scaleInSpeed/1.5f) * Time.deltaTime;
			if (scale < 1.0f) {
				scale = 1.0f;
				state = ThoughtTextState.idle;
			}

			theTextMesh.transform.localScale = new Vector3 (scale, scale, scale);

		}

		if (state == ThoughtTextState.idle) {

			elapsedTime += Time.deltaTime;

			if (elapsedTime > timeToFadeOut) {


				state = ThoughtTextState.fading;
			}

		}

		if (state == ThoughtTextState.fading) {

			opacity -= fadeOutSpeed * Time.deltaTime;
			if (opacity < 0) {

				Destroy (this.gameObject);

			} else {

				Vector4 newCol = new Vector4 (1, 1, 1, opacity);
				theTextMesh.color = newCol;

			}

		}

	}
}
