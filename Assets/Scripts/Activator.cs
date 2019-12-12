using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : WisdominiObject {

	public GameObject whatToActivate;

	public bool startActivated;

	new void Start() {
		if (startActivated)
			whatToActivate.SetActive (true);
		else
			whatToActivate.SetActive (false);
	}

	public void _wm_activate() {
		whatToActivate.SetActive (true);
	}

	public void _wm_deactivate() {
		whatToActivate.SetActive (false);
	}

}
