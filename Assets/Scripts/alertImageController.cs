using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class alertImageController : WisdominiObject {


	public float yScaleSpeed;
	public Text textRef;

	[HideInInspector]
	public bool closeOnClick = true;

	public enum State { Closed, Opening, Open, Closing };
	public State state;
	float yScale;
	float startTime;
	float duration;
	bool unblock = false;

	bool started = false;

	public void _wm_preventCloseOnClick() {
		closeOnClick = false;
	}

	public void _wm_allowCloseOnClick() {
		closeOnClick = true;
	}

	public void Start () {

		if (started)
			return;
		started = true;

		base.Start ();
		state = State.Closed;
		duration = 0.0f;
		waitingRef = null;
		reset ();
	}
	
	void Update () {

		UpdateProgram (); // so is update program

		Vector2 sc;

		if (state == State.Open) {
			if (Input.GetMouseButtonDown (0)) {
				if(closeOnClick) state = State.Closing;
			}
		}
	
		if (state == State.Open) {
			if (duration > 0.0f) {

				if ((Time.time - startTime) > duration) {
					state = State.Closing;
				}

			}
		}

		switch (state) {
		case State.Closed:
			if (unblock == true) {
				GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ().unblockPlayerControls ();
				unblock = false;
			}
			break;	
		case State.Opening:
			if (yScale < 2.0f) {
				yScale += yScaleSpeed * Time.deltaTime;
			} else {
				yScale = 2.0f;
				state = State.Open;
			}
			sc = this.transform.localScale;
			sc.y = yScale;
			this.transform.localScale = sc;
			break;
		case State.Open:
			break;
		case State.Closing:
			if (yScale > 0.0f) {
				yScale -= yScaleSpeed * Time.deltaTime;
			} else {
				yScale = 0.0f;
				state = State.Closed;
				notifyFinishAction ();
			}
			sc = this.transform.localScale;
			sc.y = yScale;
			this.transform.localScale = sc;
			break;
		}
		/*
		 * 
		 * Si esto fuera UGLI:
		 * 
		 * Closed:
		 * 
		 * Opening:
		 * 	while(yScale<1.0f) yScale += 0.01f;
		 *  yScale = 1.0f;
		 *  next;
		 * 
		 * Open:
		 *   texter.startAction(action: "sfsdfs", blocking: yes);
		 *   texter.startAction("sdfsdf");
		 * *
		 * 
		 * /
		*/

	}

	public void close() {
		state = State.Closing;
	}

	public void _wm_setAlertMessage(string msg) {

		yScale = 0.0f;
		state = State.Opening;
		duration = 0.0f;
		textRef.text = msg;

	}

	public void _wm_setAlertMessageAndClose(string msg) {

		yScale = 0.0f;
		state = State.Opening;
		duration = 0.0f;
		textRef.text = msg;
		unblock = true;

	}

	public void _wa_setAlertMessage(WisdominiObject waiter, string msg) {

		waitingRef = waiter;
		_wm_setAlertMessage(msg);

	}

	public void _wa_setAlertMessageWithTimeout(WisdominiObject waiter, string msg, float time) {

		waitingRef = waiter;
		waiter.isWaitingForActionToComplete = true;
		_wm_setAlertMessage(msg);
		_wm_setSelfTimeout(time);

	}

	public void reset() {

		state = State.Closed;
		yScale = 0.0f;
		duration = 0.0f; // indefinitely open
		Vector2 sc = this.transform.localScale;
		sc.y = yScale;
		this.transform.localScale = sc;
	}

	public void _wm_reset() {

		reset();

	}

	public bool _wm_isOpen() {

		return (yScale > 0);
	
	}

	public void _wm_setSelfTimeout(float Seconds) {

		startTime = Time.time;
		duration = Seconds;
		

	}

	

	
}
