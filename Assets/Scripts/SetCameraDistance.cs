using UnityEngine;
using System.Collections;

public class SetCameraDistance : MonoBehaviour {

	public bool disableCameraLookAt;
	GameObject pivotZ;
	CameraManager cam;
	public float targetDist;
	public float cameraDistanceSpeed;
	float originalDistance;
	public bool gate;

	void Start () 
	{
		cam = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		pivotZ = cam.pivotZ;
		originalDistance = pivotZ.transform.localPosition.z;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Player") {
			originalDistance = pivotZ.transform.localPosition.z;
			cam.setDistanceZ (targetDist, cameraDistanceSpeed);
		}
	}

	void OnTriggerExit(Collider other) {
		if (gate) {
			return;
		}

		if (other.tag == "Player") 
		{
			cam.setDistanceZ (originalDistance, cameraDistanceSpeed);
			if (disableCameraLookAt) {
				cam._wm_enableLookAt ();
			}
		}
	}
}

