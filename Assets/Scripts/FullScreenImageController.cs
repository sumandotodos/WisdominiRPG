using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FitType { horizontal, vertical };

[System.Serializable]
public class FSItem {

	public Sprite[] image;
	public string name;
	public FitType fit;
	public float fps = 0;
	public Vector3 initialPosition;
	public float initialScale = 1;
	public Vector3 finalPosition;
	public float finalScale = 1;
	public Vector3 aspectScale;
	public float duration = 5;
	public float transitionTime = 1;

}

public class FullScreenImageController : WisdominiObject {

	Image canvas;
	UIImageFader fader;

	public FSItem[] items;

	bool cancellable = true;
	int playingItem;
	float fadeTime;
	float frameTime;
	float elapsedTime;
	float frameElapsedTime;
	int currentFrame;

	float scale;
	Vector3 position;
	Vector3 screenCenter;
	Vector3 screenAspect;

	float deltaScalePerSecond;
	Vector3 deltaPositionPerSecond;

	const int IDLE = 0;
	const int FADINGIN = 1;
	const int SHOWING = 2;
	const int FADINGOUT = 3;
	const float TIMETHRESHOLD = 0.01f;

	// Use this for initialization
	void Start () {
		canvas = GameObject.Find ("FullScreenCanvas").GetComponent<Image> ();
		fader = canvas.gameObject.GetComponent<UIImageFader> ();
		state = 0;
	}

	int state;

	// Update is called once per frame
	void Update () {
		
		if (state == IDLE) {

		}

		if (state == SHOWING) {
			elapsedTime += Time.deltaTime;
			if(elapsedTime > (items[playingItem].duration - items[playingItem].transitionTime)) {
				state = FADINGOUT;
				if (fadeTime > 0.0f) {
					fader.fadeOut ();
				} else
					fader.setOpacity (0.0f);
			}

		}

		if (state == FADINGOUT) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > (items [playingItem].duration)) {
				state = IDLE;
				notifyFinishAction ();
			}
		}


		if (state != IDLE) {
			scale += deltaScalePerSecond * Time.deltaTime;
			canvas.transform.localScale = screenAspect * scale;
			position += deltaPositionPerSecond * Time.deltaTime;
			canvas.transform.position = screenCenter + position;

			frameElapsedTime += Time.deltaTime;
			if (frameElapsedTime > frameTime) {
				frameElapsedTime = 0.0f;
				currentFrame = (currentFrame + 1)%items[playingItem].image.Length; // wrap around
				canvas.sprite = items [playingItem].image [currentFrame];
			}

		}

	}

	public void startItem(string name) {
		bool somethingStarted = false;
		for (int i = 0; i < items.Length; ++i) {
			if (items [i].name.Equals (name)) {
				startItem (i);
				somethingStarted = true;
				break;
			}
		}
		if (!somethingStarted) {
			Debug.Log ("<color=red>FullScreenController: could not start '" + name + "'</color>");
		}
	}

	public void startItem(int n, bool c) {

		cancellable = c;
		startItem (n);

	}

	public void startItem(int n) {

		float localAspect = ((float)Screen.width) / ((float)Screen.height);
		screenAspect = new Vector3 (items [playingItem].aspectScale.x / localAspect, items [playingItem].aspectScale.y, items [playingItem].aspectScale.z);
		playingItem = n;
		canvas.sprite = items [playingItem].image [0];
		if (items [playingItem].transitionTime > TIMETHRESHOLD) {
			fadeTime = (1.0f / items [playingItem].transitionTime);
			fader.fadeInSpeed = fader.fadeOutSpeed = fadeTime;
			fader.fadeIn ();
		} else {
			fadeTime = 0.0f;
			fader.setOpacity (1.0f);
		}
		if (items [playingItem].fps > 0.0f) {
			frameTime = (1.0f / items [playingItem].fps);
		} else
			frameTime = items[playingItem].duration * 2.0f; // just impossibly high
		state = SHOWING;
		elapsedTime = 0.0f;
		frameElapsedTime = 0.0f;
		currentFrame = 0;
		screenCenter = new Vector3 (Screen.width / 4, Screen.height / 4, 0);
		scale = items [playingItem].initialScale;
		canvas.transform.localScale = screenAspect * scale;
		position = canvas.transform.position = screenCenter + items [playingItem].initialPosition;
		deltaScalePerSecond = (items [playingItem].finalScale - items [playingItem].initialScale) / items [playingItem].duration;
		deltaPositionPerSecond = (items [playingItem].finalPosition - items [playingItem].initialPosition) / items [playingItem].duration;
	}

	public void _wm_startItem(string s) {
		startItem (s);
	}

//	public void _wa_startItem(WisdominiObject w, int n) {
//		w.isWaitingForActionToComplete = true;
//		waitingRef = w;
//		startItem (n);
//	}

	public void _wa_startItem(WisdominiObject w, string s) {
		w.isWaitingForActionToComplete = true;
		waitingRef = w;
		startItem (s);
	}
}
