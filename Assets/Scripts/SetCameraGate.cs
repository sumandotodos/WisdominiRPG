using UnityEngine;
using System.Collections;

/*
 * Camera gate:
 * 
 * 	Similar to camera hold, but...
 * 
 * 
 */

public class SetCameraGate : MonoBehaviour {

	public bool disableCameraLookAt;
	public LevelControllerScript level_A;
	public CameraManager cam_A;
	public string reentryVariable;
	Vector3 storedPosition;
	public float exitRadius;

	void Start () {

		cam_A = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		level_A = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		storedPosition = Vector3.zero;

		// check reentry
		float x, y, z;
		x = level_A.retrieveFloatValue (reentryVariable + "_X");
		y = level_A.retrieveFloatValue (reentryVariable + "_Y");
		z = level_A.retrieveFloatValue (reentryVariable + "_Z");

		if ((x != 0.0f) || (y != 0.0f) || (z != 0.0f)) 
		{
			storedPosition = new Vector3 (x, y, z);
		}

	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player") 
		{
			if (storedPosition == Vector3.zero) 
			{ // if gate is empty

				// fill the gate
				storedPosition = other.gameObject.transform.position; // store player entry position
				level_A.storeFloatValue(reentryVariable + "_X", storedPosition.x);
				level_A.storeFloatValue(reentryVariable + "_Y", storedPosition.y);
				level_A.storeFloatValue(reentryVariable + "_Z", storedPosition.z);
			}

			cam_A.moveToPoint (storedPosition);

			if (disableCameraLookAt) 
			{
				cam_A._wm_disableLookAt ();
			}
		}

	}

	void OnTriggerExit(Collider other) {

		if (other.tag == "Player") {
			// check if we must vacate the gate
			float distance = Mathf.Abs((other.transform.position - storedPosition).magnitude);
			if (distance <= exitRadius) { // vacate
				storedPosition = Vector3.zero;
			}
			cam_A._wm_moveToOriginalPosition();
			if (disableCameraLookAt) 
			{
				cam_A._wm_enableLookAt ();
			}
		}

	}


}
