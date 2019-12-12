using UnityEngine;
using System.Collections;

public class SetCameraHold : MonoBehaviour {

	public bool disableCameraLookAt;
	public CameraManager cam;

	void Start () {

		cam = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
    }

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player") 
		{
			cam.moveToPoint (cam.transform.position);
			if (disableCameraLookAt) 
			{
				cam._wm_disableLookAt ();
			}
		}
	}

	void OnTriggerExit(Collider other) {

		if (other.tag == "Player") 
		{
			cam.moveToOriginalPosition ();
			if (disableCameraLookAt)
			{
				cam._wm_enableLookAt ();
			}
		}
	}
}
