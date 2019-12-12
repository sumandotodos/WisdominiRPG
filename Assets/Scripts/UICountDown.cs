using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICountDown : WisdominiObject {

	public Texture[] images;

	Vector3 originalScale;

	RawImage image;

	public int currentDigit;

	public float scale;
	float targetScale;

	public float opacity;
	float targetOpacity;

	public float scaleSpeed = 0.8f;
	public float opacitySpeed = 2.0f;

	public float elapsedTime;

	public float delayTime = 0.5f;

	int state; // 0: deactivated
			   // 1: running
			   // 2: delay
			   // 3: finished

	// Use this for initialization
	void Start () {

		image = this.GetComponent<RawImage> ();
		image.enabled = false;
		image.color = new Color (1, 1, 1, 0);
		opacity = 0.0f;
		targetOpacity = 0.0f;
		scale = 0.75f;
		targetScale = 1.0f;
		currentDigit = images.Length - 1;
		image.texture = images [currentDigit];
		originalScale = image.transform.localScale;
		this.transform.localScale = originalScale * scale;
	
	}

	public void _wm_launch() {
		launch ();
	}

	public void launch() {

		opacity = 1.0f;
		image.color = new Color(1, 1, 1, opacity);
		image.enabled = true;
		state = 1;

	}
	
	// Update is called once per frame
	void Update () {

		if (state == 1) {

			scale += scaleSpeed * Time.deltaTime;

			this.image.transform.localScale = originalScale * scale;
				
			bool changed = Utils.updateSoftVariable (ref opacity, targetOpacity, opacitySpeed);

			if (changed) {
				image.color = new Color (1, 1, 1, opacity);
			}
			else {
				state = 2;
				elapsedTime = 0.0f;
			}

		}

		if (state == 2) {

			elapsedTime += Time.deltaTime;
			if (elapsedTime > delayTime) {
				state = 1;
				scale = 0.75f;
				opacity = 1.0f;
				--currentDigit;
				if(currentDigit < 0) {
					state = 3;
					image.enabled = false;
				}
				else {
					image.transform.localScale =  originalScale * scale;
					image.texture = images[currentDigit];
					image.color = new Color(1, 1, 1, opacity);
				}
			}

		}
	
	}
}
