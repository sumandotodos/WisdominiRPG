using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBlinker : MonoBehaviour {

	public Texture normal;
	public Texture blink;

	const float minBlinkTime = 1.0f;
	const float maxBlinkTime = 5.0f;

	float timeToNextBlink;

	const float blinkTime = 0.25f;

	RawImage img;

	float elapsedTime;

	int state; // 0 stopped
				// 1 open eye
				// 2 closed eye

	// Use this for initialization
	void Start () {
	
		state = 0;
		timeToNextBlink = Random.Range (minBlinkTime, maxBlinkTime);
		img = this.GetComponent<RawImage> ();
		reload ();

	}

	public void reload() {
		if (img == null)
			return;
		if (normal != null) {
			img.enabled = true;
			img.texture = normal;
			state = 1;
		} else {
			img.enabled = false;
			state = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (state > 0) {
			elapsedTime += Time.deltaTime;
		}

		if (state == 1) {
			if(elapsedTime > timeToNextBlink) {
				elapsedTime = 0.0f;
				timeToNextBlink = Random.Range (minBlinkTime, maxBlinkTime);
				img.texture = blink;
			}
		}
		if (state == 2) {
			if (elapsedTime > blinkTime) {
				elapsedTime = 0.0f;
				state = 1;
				img.texture = normal;
			}
		}
	

	}
}
