using UnityEngine;
using System.Collections;

public class FootstepTrigger : WisdominiObject {

	FootstepSoundManager footsoundMgr;

	public string groundType = "";
	public string exitGroundType = "";

	bool enabled = true;

	// Use this for initialization
	void Start () {

		footsoundMgr = GameObject.Find ("FootstepSoundManager").GetComponent<FootstepSoundManager>();
		enabled = true;
	
	}

	public void _wm_setEnable(bool en) {
		enabled = en;
	}
	
	void OnTriggerEnter(Collider other) {

		if (!enabled)
			return;
		if (other.tag == "Player") {

			if (!groundType.Equals ("")) {
				footsoundMgr.setGroundType (groundType);
			}

		}

	}

	void OnTriggerExit(Collider other) {

		if (!enabled)
			return;
		if (other.tag == "Player") {
			if (!exitGroundType.Equals ("")) {
				footsoundMgr.setGroundType (exitGroundType);
			}
		}

	}

}
