using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class SerializableVector3 {

	float x, y, z;

	public SerializableVector3(Vector3 v) {

		x = v.x;
		y = v.y;
		z = v.z;

	}

}
