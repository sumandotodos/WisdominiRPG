using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBreather : MonoBehaviour {

	RawImage image;
	public GameObject dip;
	UIImageAnimator dipAnim;

	float opacity = 0.0f;
	float targetOpacity = 0.0f;

	public float period = 4.0f; // 4 seconds is nice to begin with
	float pauseDelay = 1.0f;
	public float totalTime = 0.0f;

	public float elapsedTime;

	LevelControllerScript level;

	public float phase;

	public AudioClip[] droplet;
	public AudioClip waterSound;

	int state = 0;
		/*
		 * 0: idle
		 * 1: breathing
		 * 2: pause
		 * 
		 */

	// Use this for initialization
	void Start () {

		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		image = this.GetComponent<RawImage> ();

		image.enabled = false;
		image.color = new Color (1, 1, 1, 0.0f);
		image.raycastTarget = false;

		dipAnim = dip.GetComponent<UIImageAnimator> ();

		state = 0;

		elapsedTime = 0.0f;

		//activate ();

	}

	public void activate() {

		state = 1;
		image.raycastTarget = true;
		image.enabled = true;
		level.playSound (waterSound);

	}
	
	// Update is called once per frame
	void Update () {
	
		if (state > 0)
			elapsedTime += Time.deltaTime;

		if (state == 1) {

			if (opacity > 0.0f) {
				opacity -= (1.0f / pauseDelay) * 1.5f * Time.deltaTime;
				if (opacity < 0.0f)
					opacity = 0.0f;
				image.color = new Color (1, 1, 1, opacity);

			}
			if (elapsedTime > pauseDelay / 2.0f) {
				++state;
				elapsedTime = 0.0f;
				phase = 0.0f;
			}

		}

		if (state == 2) {


			opacity = ((1.0f - Mathf.Cos (phase))/2.0f) * 0.5f;
			phase = elapsedTime * 2.0f*Mathf.PI / period;
			image.color = new Color (1, 1, 1, opacity);
			if (phase > 2.0f * Mathf.PI) {
				opacity = 0.0f;
				image.color = new Color (1, 1, 1, 0.0f);
				++state;
			}
		}

		if (state == 3) {
			phase = elapsedTime * 2.0f*Mathf.PI / period;
			if (elapsedTime > 16.0f)
				elapsedTime = 16.0f; // oh, please!
		}

	}

	public void breath() {

		targetOpacity = 0.0f;

	}

	public void touch() {

		if (state == 0)
			return;

		float x = Input.mousePosition.x;
		float y = Input.mousePosition.y;

		dip.transform.localPosition = new Vector2 (2*x-Screen.width, 2*y-Screen.height);
		dipAnim.resetAnimation ();
		state = 1;

		totalTime = (phase * period / (2.0f * Mathf.PI));
		period = (period + totalTime) / 2.0f; // adjust period
		elapsedTime = 0.0f;
		phase = 0.0f;

		state = 1;

		int randomSound = Random.Range (0, droplet.Length - 1);
		level.playSound (droplet [randomSound]);

	}
		
}
