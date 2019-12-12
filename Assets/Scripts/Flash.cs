using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Flash : MonoBehaviour {

	public float flashDuration;
	public float flashTime;
	float elapsedTime;
	float flashElapsedTime;
	public float initialDelay;

	RawImage image;

	bool blink;

	int state = 0;

	// Use this for initialization
	void Start () {
	
		state = -1;
		elapsedTime = 0.0f;
		flashElapsedTime = 0.0f;
		image = this.GetComponent<RawImage> ();
		image.enabled = false;
		blink = false;

	}

	public void go() {

		state = 0;

	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == 0) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > initialDelay) {
				state = 1;
				elapsedTime = 0.0f;
				image.enabled = true;
			}
		}
		if (state == 1) {
			flashElapsedTime += Time.deltaTime;
			elapsedTime += Time.deltaTime;
			if (elapsedTime > flashDuration) {
				elapsedTime = 0.0f;
				if (blink) {
					image.color = new Color (1, 1, 1, 1);
				} else {
					image.color = new Color (0, 0, 0, 0);
				}
				blink = !blink;
			}
			if (flashElapsedTime > flashTime) {
				state = 2;
			}
		}
		if (state == 2) {
			image.enabled = false;
		}

	}
}
