using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformReader : WisdominiObject {


	public bool _wm_axisWithinBounds(int axis, float min, float max) {

		float angleValue = 0;
		if (axis == 0)
			angleValue = this.transform.rotation.eulerAngles.x;
		if (axis == 1)
			angleValue = this.transform.rotation.eulerAngles.y;
		if (axis == 2)
			angleValue = this.transform.rotation.eulerAngles.z;

		return ((min <= angleValue) && (angleValue <= max));

	}

	public float _wm_axisRead(int axis) {
		float angleValue = 0;
		if (axis == 0)
			angleValue = this.transform.rotation.eulerAngles.x;
		if (axis == 1)
			angleValue = this.transform.rotation.eulerAngles.y;
		if (axis == 2)
			angleValue = this.transform.rotation.eulerAngles.z;

		return angleValue;
	}
}
