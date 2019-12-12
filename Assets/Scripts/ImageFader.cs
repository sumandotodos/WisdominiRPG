using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFader : MonoBehaviour {

	/* references */

	Image theImage;

	/* properties */

	public float opacity;
	float targetOpacity;
	float delay;
	float elapsedTime;

	/* public properties */

	public float opacitySpeed = 6.0f;

	// Use this for initialization
	void Start () {

		elapsedTime = 0.0f;
		theImage = this.GetComponent<Image> ();
		Color newColor = new Color (1, 1, 1, 0);
		theImage.color = newColor;
		delay = 0.0f;
		theImage.enabled = true; // save time!

	}

	// Update is called once per frame
	void Update () {

		if (opacity < targetOpacity) {

			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > targetOpacity) {
				opacity = targetOpacity;

			}

			Color newColor = new Color (1, 1, 1, opacity);
			theImage.color = newColor;

		}

		if (opacity > targetOpacity) {

			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < targetOpacity) {
				opacity = targetOpacity;
				if (opacity == 0.0f) {
					//theImage.enabled = false;
				}
			}

			Color newColor = new Color (1, 1, 1, opacity);
			theImage.color = newColor;

		}

		if (delay > 0.0f) {

			elapsedTime += Time.deltaTime;
			if (elapsedTime > delay) {
				delay = 0.0f;
				fadeOut ();
			}

		}

	}

	public void setSprite(Sprite spr) {

		theImage.sprite = spr;

	}


	public void fadeIn() {

		targetOpacity = 1.0f;
		//theImage.enabled = true;

	}

	public void fadeOut() {

		targetOpacity = 0.0f;

	}

	public void fadeOut(float d) {

		delay = d;
		elapsedTime = 0.0f;
	}

}
