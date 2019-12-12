using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class UITextFader : MonoBehaviour {


	/* references */

	Text txt;


	/* properties */

	float opacity;
	GameObjectFaderState state;


	/* public properties */

	public float fadeInSpeed = 6.0f;
	public float fadeOutSpeed = 6.0f;
	public float minOpacity = 0.0f;
	public float maxOpacity = 1.0f;



	// Use this for initialization
	void Start () {

		txt = this.GetComponent<Text> ();
		opacity = minOpacity;
		state = GameObjectFaderState.transparent;
		updateMaterial ();

	}

	public void setOpacity(float op) {

		opacity = op;
		updateMaterial ();

	}



	void updateMaterial() {

		Color newColor = txt.color;
		newColor.a = opacity;
		txt.color = newColor;

	}

	// Update is called once per frame
	void Update () {

		if (state == GameObjectFaderState.transparent) {


		}

		if (state == GameObjectFaderState.fadingIn) {

			opacity += fadeInSpeed * Time.deltaTime;
			if (opacity > maxOpacity) {
				opacity = maxOpacity;
				state = GameObjectFaderState.opaque;
			}
			updateMaterial ();

		}

		if (state == GameObjectFaderState.fadingOut) {

			opacity -= fadeOutSpeed * Time.deltaTime;
			if (opacity < minOpacity) {
				opacity = minOpacity;
				state = GameObjectFaderState.transparent;
			}
			updateMaterial ();

		}

		if (state == GameObjectFaderState.opaque) {

		}

	}

	public void fadeIn() {

		state = GameObjectFaderState.fadingIn;

	}

	public void fadeOut() {

		state = GameObjectFaderState.fadingOut;

	}
}
