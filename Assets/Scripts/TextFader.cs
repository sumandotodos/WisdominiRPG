using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextFader : MonoBehaviour {

	/* references */

	Text theText;

	/* properties */

	float opacity;
	float targetOpacity;
	float elapsedTime;
	float delay;

	/* public properties */

	public float opacitySpeed = 2.5f;

	// Use this for initialization
	void Start () {

		theText = this.GetComponent<Text> ();
		Color newColor = new Color (1, 1, 1, 0);
		opacity = 0.0f;
		theText.color = newColor;
		delay = 0.0f;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (opacity < targetOpacity) {

			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > targetOpacity) {
				opacity = targetOpacity;
			}

			Color newColor = new Color (1, 1, 1, opacity);
			theText.color = newColor;

		}

		if (opacity > targetOpacity) {

			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < targetOpacity) {
				opacity = targetOpacity;
			}

			Color newColor = new Color (1, 1, 1, opacity);
			theText.color = newColor;

		}

		if (delay > 0.0f) {

			delay -= Time.deltaTime;
			if (delay < 0.0f) {
				delay = 0.0f;
				fadeOut ();
			}

		}

	}

	public void setText(string txt) {

		theText.text = txt;

	}

	public void fadeIn() {

		targetOpacity = 1.0f;

	}

	public void fadeOut() {

		targetOpacity = 0.0f;

	}

	public void fadeOut(float d) {

		delay = d;

	}
}
