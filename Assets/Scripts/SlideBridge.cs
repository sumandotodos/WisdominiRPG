using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBridge : WisdominiObject {

	MasterControllerScript mc;
	DataStorage ds;

	public GameObject barrier_N;

	public float totalDisplacement;
	public int numberOfSteps;
	public bool autoreentrant;
	Vector3 basePos;

	public int step;
	public float posValue;

	SoftFloat pos;

	FakePanelAxis axis = FakePanelAxis.x;

	// Use this for initialization
	void Start () {
		basePos = this.transform.position;
		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = mc.getStorage ();
		pos = new SoftFloat (0.0f);
		pos.setSpeed (2.0f);
		if (autoreentrant) {
			int bs = ds.retrieveIntValue (this.name + "BridgeStep");
			step = bs;
			setStepImmediate (bs);
			if (step == numberOfSteps) {
				if (barrier_N != null) {
					barrier_N.SetActive (false);
				}
			}
		} 
	}

	public void setStep(int s) {
		if (s > numberOfSteps)
			return;
		if (s == numberOfSteps) {
			if (barrier_N != null) {
				barrier_N.SetActive (false);
			}
		}
		pos.setValue (totalDisplacement * ((float)(s)) / ((float)(numberOfSteps)));
		if (autoreentrant) {
			ds.storeIntValue (this.name + "BridgeStep", s);
		} 
	}

	public void setStepImmediate(int s) {
		pos.setValueImmediate (totalDisplacement * ((float)(s)) / ((float)(numberOfSteps)));
		if (autoreentrant) {
			ds.storeIntValue (this.name + "BridgeStep", s);
		} 
	}

	public void stepUp() {
		if (step < numberOfSteps) {
			setStep (++step);
			if (step == numberOfSteps) {
				if (barrier_N != null) {
					barrier_N.SetActive (false);
				}
			}
		}
	}

	public void _wm_stepUp() {
		stepUp();
	}
	
	// Update is called once per frame
	void Update () {
		pos.update ();	
		posValue = pos.getValue ();
		if (axis == FakePanelAxis.x) {
			Vector3 newPos = basePos + new Vector3 (pos.getValue (), 0, 0);
			this.transform.position = newPos;
		} else if (axis == FakePanelAxis.z) {
			Vector3 newPos = basePos + new Vector3 (0, 0, pos.getValue ());
			this.transform.position = newPos;
		}
	}
}
