using UnityEngine;
using System.Collections;

enum WFHealthBarState { idle, growing, growing2 };

public class WordFightHealthBar : WisdominiObject {

	/* references */

	public GameObject bar;
	public GameObject barQuad;
	Material barMat;

	/* public properties */

	public float targetBarFraction = 1.0f;
	public bool isShadow = true;

	/* properties */

	float barFraction;
	Color lowColor;// = Color.red;
	Color highColor;// = Color.green;
	bool blink;
	float elapsedTime;

	/* constants */

	const float scaleSpeed = 1.0f;
	const float barSpeed = 0.5f;
	const float blinkTime = 0.3f;

	bool healthBarEvent;

	float scale;
	WFHealthBarState state;


	// Use this for initialization
	new void Start () {
	
		scale = 0;
		barFraction = 0.0f;
		targetBarFraction = 1.0f;
		this.transform.localScale = new Vector3 (scale, scale, scale);
		bar.transform.localScale = new Vector3 (barFraction, 1, 1);
		state = WFHealthBarState.idle;
		barMat = barQuad.GetComponent<Renderer> ().material;
		if (isShadow) {
			highColor = Color.black;
			lowColor = Color.white;
		} else {
			highColor = Color.white;
			lowColor = Color.black;
		}

	}
	
	// Update is called once per frame
	void Update () {
	

		if (state == WFHealthBarState.idle) {

			elapsedTime += Time.deltaTime;
			if (elapsedTime > blinkTime) {
				blink = !blink;
				elapsedTime = 0.0f;
			}

			if (barFraction > targetBarFraction) {

				barFraction -= barSpeed * Time.deltaTime;

				if (barFraction < targetBarFraction) {
					barFraction = targetBarFraction;
					healthBarEvent = true;
				}

				Color newColor = RedGreenLerp (barFraction);
				bar.transform.localScale = new Vector3 (barFraction, 1, 1);
				barMat.SetColor ("_Tint", newColor);

					

			}

			if ((barFraction <= 0.25f) && blink) {
				bar.transform.localScale = new Vector3 (0, 1, 1);
			}
			if ((barFraction <= 0.25f) && !blink) {
				bar.transform.localScale = new Vector3 (barFraction, 1, 1);
			}

		}



		if (state == WFHealthBarState.growing) {

			scale += scaleSpeed * Time.deltaTime;
			if (scale >= 1.0f) {
				scale = 1.0f;
				state = WFHealthBarState.growing2;
			}
			this.transform.localScale = new Vector3 (scale, scale, scale);

		}

		if (state == WFHealthBarState.growing2) {

			barFraction += scaleSpeed * Time.deltaTime;
			if (barFraction > 1.0f) {
				scale = 1.0f;
				elapsedTime = 0.0f;
				state = WFHealthBarState.idle;
				notifyFinishAction ();
			}
			Color newColor = RedGreenLerp (barFraction);
			bar.transform.localScale = new Vector3 (barFraction, 1, 1);
			barMat.SetColor ("_Tint", newColor);

		}


	}

	Color RedGreenLerp(float t) {

		/*Color res = new Color ();

		if (t < 0.5) {

			res.r = 1.0f;
			res.g = t*2.0f;
			res.b = 0.0f;
			res.a = 1.0f;

		} else {

			res.r = 1.0f - 2.0f * (t - 0.5f);
			res.g = 1.0f;
			res.b = 0.0f;
			res.a = 1.0f;

		}

		return res;*/

		return Color.Lerp (lowColor, highColor, t);

	}


	public void grow() {

		state = WFHealthBarState.growing;

	}


	public void _wa_grow(WisdominiObject waiter) {

		waitingRef = waiter;
		waiter.isWaitingForActionToComplete = true;
		grow ();

	}

	public bool queryEvent(string channel) {

		if (channel.Equals ("HealthBarEvent")) {

			if (healthBarEvent) {
				healthBarEvent = false;
				return true;
			} else
				return false;

		}

		return false;

	}

	public void queryClear() {

		healthBarEvent = false;

	}


}
