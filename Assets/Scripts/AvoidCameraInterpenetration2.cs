using UnityEngine;
using System.Collections;

public enum AvoidState { locked, idle, far, near, resetting };

public class AvoidCameraInterpenetration2 : MonoBehaviour {

	public int tocando;
	public string quiencojones;
	public AvoidState state;
	public float adjustSpeed = 120.0f;
	public float dist;
	public float targetDist;
	public float restDist = 30.0f;
	Camera cam;
	public int cols;

	// Use this for initialization
	void Start () {
	
		cam = this.GetComponent<Camera> ();
		state = AvoidState.locked;
		dist = targetDist = restDist;
		cols = 0;

	}
	
	// Update is called once per frame
	void Update () {



		this.transform.localPosition = new Vector3 (0, 0, -dist);
		if (dist < targetDist) {
			dist += adjustSpeed * Time.deltaTime;
			if (dist > targetDist) {
				dist = targetDist;
			}
		}
		if (dist > targetDist) {
			dist -= adjustSpeed * Time.deltaTime;
			if (dist < targetDist) {
				dist = targetDist;

			}
		}

		if (state == AvoidState.locked)
			return;

		if (state == AvoidState.far) {
			targetDist += adjustSpeed * Time.deltaTime;
			state = AvoidState.idle;
		}

		quiencojones = "";

		tocando = 0;

	
	}

	public void unlock() {

		state = AvoidState.far;

	}
	/*
	void OnCollisionEnter(Collision other) {

		++cols;
		state = AvoidState.far;

	}

	void OnCollisionExit(Collision other) {

		--cols;
		if (cols == 0)
			state = AvoidState.idle;

	}*/


	void OnTriggerStay(Collider col) {
	
		if(state != AvoidState.locked) {
			state = AvoidState.far; // as long as you are colliding with s/t, keep moving
		}
		tocando = 1;
		quiencojones = col.name;

	}


	public void reset() {

		targetDist = restDist;
		state = AvoidState.locked;

	}
}
