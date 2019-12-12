using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyEnabler : WisdominiObject {

	public float minDelay = 0.0f;
	public float maxDelay = 0.0f;
	Rigidbody r;
	float remainingTime;

	// Use this for initialization
	new void Start () {
		remainingTime = 0.0f;
		r = this.GetComponent<Rigidbody> ();
		r.isKinematic = true;
	}
	
	// Update is called once per frame
	new void Update () {
		if (remainingTime > 0.0f) {
			remainingTime -= Time.deltaTime;
			if (remainingTime < 0.0f) {
				r.isKinematic = false;
			}
		}
	}

	public void _wm_go() {
		remainingTime = Random.Range (minDelay, maxDelay);
	}
}
