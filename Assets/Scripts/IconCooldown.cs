using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum coolDownState { flashingIn, flashingOut, idle };

public class IconCooldown : MonoBehaviour {



	/* references */

	public UIFaderScript fader;




	/* public properties */

	public float percent;
	public float fullHeight = 100.0f;
	public float cooldownTime = 20.0f;



	/* properties */

	float elapsedTime;
	RectTransform rect;
	RawImage raw;
	coolDownState state;
	float flashValue;
	bool dispel;

	/* constants */

	const float flashSpeed = 6.0f;


	// Use this for initialization
	void Start () {
	
		rect = this.GetComponent<RectTransform> ();
		raw = this.GetComponent<RawImage> ();
		elapsedTime = 0.0f;
		Color newColor = new Color (8, 8, 8, 8);
		raw.color = newColor;
		state = coolDownState.idle;
		dispel = false;

	}


	// Update is called once per frame
	void Update () {

		elapsedTime += Time.deltaTime;
		if (elapsedTime < cooldownTime)
			setFraction (elapsedTime / cooldownTime);
		else
			setFraction (1.0f);
	
		if (state == coolDownState.flashingIn) {

			fader.imageRef.enabled = true;
			fader.setFadeColor (1, 1, 1);
			fader.setFadeValue (flashValue);
			flashValue -= flashSpeed * Time.deltaTime;
			if (flashValue < 0.0) {
				flashValue = 0.0f;
				state = coolDownState.flashingOut;
			}

		}

		if (state == coolDownState.flashingOut) {

			fader.setFadeColor (1, 1, 1);
			fader.setFadeValue (flashValue);
			flashValue += flashSpeed * Time.deltaTime;
			if (flashValue > 1.0) {
				flashValue = 1.0f;
				setFraction (0);
				fader.setFadeValue (1.0f);
				fader.setFadeColor (0, 0, 0);
				dispel = false;
				state = coolDownState.idle;
				fader.imageRef.enabled = false;
			}

		}

		if (state == coolDownState.idle) {



		}

	}

	/*
	public bool useItem() {

		if (elapsedTime > cooldownTime) {
			elapsedTime = 0.0f;
			return true;
		}

		return false;

	}*/


	void setFraction(float p) {

		Rect r;
		//rect.transform.localScale = new Vector2(1, p);
		r = rect.rect;
		r.height = fullHeight * p;
		Vector2 sd = rect.sizeDelta;
		sd.y = fullHeight * p;
		rect.sizeDelta = sd;
		r = new Rect ();
		r.x = 0.0f;
		r.y = 0.0f;
		r.width = 1.0f;
		r.height = p;
		raw.uvRect = r;

	}


	public void dispelEyes() {

		if (elapsedTime > cooldownTime) {
			dispel = true;
			elapsedTime = 0.0f;
			flashValue = 0.0f;
			fader.setFadeValue (1.0f);
			state = coolDownState.flashingIn;
		}

	}

	public bool mustDispel() {

		return dispel;

	}


}

