using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyManipulator : WisdominiObject {

	Rigidbody r;

	void Start() {
		r = this.GetComponent<Rigidbody> ();
	}

	public void _wm_setRigidBodyEnabled(bool en) {
		if (r != null) {
			r.isKinematic = !en;
		}
	}
}
