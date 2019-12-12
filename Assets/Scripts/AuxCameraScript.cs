using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxCameraScript : WisdominiObject {

	Camera cam;

	void Start() {
		cam = this.GetComponent<Camera> ();
	}

	public void _wm_setEnabled(bool en) {
		cam.enabled = en;
	}

}
