using UnityEngine;
using System.Collections;
using UnityEngine.UI;

enum VignetteState { open, opening, close, closing };

public class VignetteScript : WisdominiObject {

	public Image circle;
	public Image padLeft;
	public Image padRight;
	public Image padTop;
	public Image padBottom;

	bool mustFinishAction = false;

	const float OPENSCALE = 2.5f;
	const float CLOSESCALE = 0.001f;

	public float scaleSpeed;

	private float currentScale;
	private float targetScale;

	VignetteState state;

	private bool visibilty;

	// Use this for initialization
	new void Start () {

		state = VignetteState.open;
		currentScale = OPENSCALE;
		setVisibility (false);
		currentScale = OPENSCALE;
		//this.transform.localScale = new Vector3 (currentScale, currentScale, currentScale);
		setVignetteScale(currentScale);

	}

	public void setVignetteScale(float sc) {

		circle.transform.localScale = new Vector3 (sc, sc, sc);
		padLeft.transform.localScale = padRight.transform.localScale = new Vector3 (1.5f - sc/2.0f, 1, 1);
		padTop.transform.localScale = padBottom.transform.localScale = new Vector3 (1, 1.5f - sc/2.0f, 1);

	}

	private void setVisibility(bool vis) {

		//this.GetComponent<Renderer> ().enabled = vis;
		circle.enabled = vis;
		padBottom.enabled = vis;
		padLeft.enabled = vis;
		padRight.enabled = vis;
		padTop.enabled = vis;

	}
	
	// Update is called once per frame
	new void Update () {

		if (mustFinishAction) {
			mustFinishAction = false;
			this.notifyFinishAction ();
		}

		//this.transform.localScale = new Vector3 (currentScale, currentScale, currentScale);


		switch (state) {
		case VignetteState.close:
			break;
		case VignetteState.closing:
			currentScale -= scaleSpeed;
			if (currentScale < CLOSESCALE) {
				currentScale = 0.0f;
				state = VignetteState.close;
				mustFinishAction = true;
			}
			break;
		case VignetteState.open:
			break;
		case VignetteState.opening:
			currentScale += scaleSpeed;
			if (currentScale > OPENSCALE) {
				currentScale = OPENSCALE;
				state = VignetteState.open;
				this.notifyFinishAction ();
				this.setVisibility (false);
			}
			break;
		}

		setVignetteScale(currentScale);
	
	}

	public void _wm_close() {

		this.setVisibility (true);
		currentScale = OPENSCALE;
		state = VignetteState.closing;

	}

	public void _wm_open() {

		this.setVisibility (true);
		currentScale = 0.0f;
		state = VignetteState.opening;

	}

	public void _wa_close(WisdominiObject waiter) {

		this.registerWaitingObject (waiter);
		mustFinishAction = false;
		_wm_close ();

	}

	public void _wa_open(WisdominiObject waiter) {

		this.registerWaitingObject (waiter);
		mustFinishAction = false;
		_wm_open ();

	}
}
