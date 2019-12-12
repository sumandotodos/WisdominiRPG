using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N4Puente : WisdominiObject {

	public LevelControllerScript level_N;
	public string StorageVariable;
	public float openAngle;
	public float deltaAngle;
	float currentAngle;
	SoftFloat angle;

	public CameraTremor cameraTremor;

	public float angleOutput;

	// Use this for initialization
	void Start () {
		if (level_N == null) {
			level_N = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		}
		angle = new SoftFloat ();
		angle.setSpeed (3.0f);
		angle.setTransformation (TweenTransforms.tanh);
		currentAngle = level_N.retrieveFloatValue (StorageVariable);
		angle.setValueImmediate (openAngle - currentAngle);
		this.transform.rotation = Quaternion.Euler (-angle.getValue(), 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		angle.update ();
		angleOutput = angle.getValue ();
		this.transform.rotation = Quaternion.Euler (-angle.getValue(), 0, 0);
	}

	public void _wm_lowerBridge() {
		currentAngle += deltaAngle;
		if (currentAngle > openAngle)
			currentAngle = openAngle;
		level_N.storeFloatValue (StorageVariable, currentAngle);
		angle.setValue (openAngle - currentAngle);
	}

	public void _wm_resetBridge() {
		currentAngle = 0;
		level_N.storeFloatValue (StorageVariable, currentAngle);
		angle.setValue (openAngle);
	}
}
