using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N4CameraController : WisdominiObject {

	public CameraFollowAux cameraFollow;
	public GameObject testPosition;

	public void _wm_cameraToTestPosition() {

		cameraFollow.addIntermediateLocation (testPosition.transform.position, false);

	}

	public void _wm_releaseCamera() {

		cameraFollow.clearIntermediateLocations ();

	}
}
