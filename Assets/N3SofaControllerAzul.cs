using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N3SofaControllerAzul : MonoBehaviour {

	public TransformReader tReader;
	public RigidBodyManipulator rbManip;
	public N3AzulController controller;

	public float targetAngle;
	public float error;

	public float whatAngle;

	// Update is called once per frame
	void Update () {

		whatAngle = tReader._wm_axisRead (1);
		if (tReader._wm_axisWithinBounds (1, targetAngle - error, targetAngle + error)) {
			rbManip._wm_setRigidBodyEnabled (false);
			controller._wm_toggleAccomplishment (2);
			this.enabled = false;
		}

	}
}
