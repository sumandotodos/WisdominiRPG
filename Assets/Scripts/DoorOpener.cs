using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour {

	public BetterDoor door;

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player") {

			door._wm_open ();

		}

	}
}
