using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftMover : MonoBehaviour {

	SoftFloat pos;

	public float speed;
	public float initialPos;
	public float finalPos;
	public FakePanelAxis axis;

	public Vector3 initialLocalPos;

	public bool autoMove = false;
	public bool autoUnmove = false;

	public bool reentrant = false;

	bool atFinalPos = false;

	bool started = false;

	private void updatePosition() {
		switch (axis) {
		case FakePanelAxis.x:
			this.transform.localPosition = new Vector3 (pos.getValue (), initialLocalPos.y, initialLocalPos.z);
			break;
		case FakePanelAxis.y:
			this.transform.localPosition = new Vector3 (initialLocalPos.x, pos.getValue (), initialLocalPos.z);
			break;
		case FakePanelAxis.z:
			this.transform.localPosition = new Vector3 (initialLocalPos.x, initialLocalPos.y, pos.getValue ());
			break;
		}
	}

	public void reset() {
		if (!started)
			Start ();
		pos.setSpeed (speed);
		pos.setTransformation (TweenTransforms.linear);
		updatePosition ();
	}

	// Use this for initialization
	void Start () {
		if (started)
			return;
		initialLocalPos = this.transform.localPosition;
		started = true;
		pos = new SoftFloat (initialPos);
		reset ();
	}

	// Update is called once per frame
	void Update () {
		if (pos.update ()) {
			updatePosition ();
		} else {
			if (atFinalPos && autoUnmove) {
				_wm_unmove ();
			}
			if ((!atFinalPos) && autoMove) {
				_wm_move ();
			}
		}
	}

	public void _wm_move() {
		pos.setValue (finalPos);
		atFinalPos = true;
	}

	public void _wm_unmove() {
		pos.setValue (initialPos);
		atFinalPos = false;
	}

	public void _wm_toggle() {
		if (atFinalPos)
			_wm_unmove ();
		else
			_wm_move ();
	}
}
