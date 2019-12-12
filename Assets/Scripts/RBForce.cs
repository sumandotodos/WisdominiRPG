using UnityEngine;
using System.Collections;

public class RBForce : MonoBehaviour {

	PlayerScript p; // the player that will enter our domain
	public Vector3 forceDirection;
	public float forceMagnitude;
	Vector3 nForceDirection; // normalized direction

	// Use this for initialization
	void Start () {
		nForceDirection = forceDirection;
		nForceDirection.Normalize ();
		p = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player") {
			p = other.gameObject.GetComponent<PlayerScript>();
			if (p != null) {
				p.setExternalForce( forceMagnitude * nForceDirection);
			}
		}

	}


	void OnTriggerExit(Collider other) {

		if (other.tag == "Player") {
			if (p != null) {
				p.setExternalForce( Vector3.zero);
			}
		}

	}

}
