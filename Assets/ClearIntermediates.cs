using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearIntermediates : MonoBehaviour {

	public bool directed = true;

	CameraFollowAux cameraFollow;

	void Start() {
		cameraFollow = GameObject.Find ("PhysicalCameraFollow").GetComponent<CameraFollowAux> ();
	}

	void OnTriggerEnter(Collider other) {

		if (other.tag != "Player")
			return;

		if (directed) {
			
			Vector3 centerVector = (this.transform.position - other.transform.position);
			if (Vector3.Dot (this.transform.forward, centerVector) > 0.0f) {
				
				return;
			}

		}

		cameraFollow.clearIntermediateLocations ();

	}
}
