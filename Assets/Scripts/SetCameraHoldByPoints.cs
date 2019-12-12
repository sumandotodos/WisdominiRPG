using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraHoldByPoints : MonoBehaviour {

	public Transform[] points;
	public bool disableCameraLookAt;
	public float speedPath;
	public float speedCam;
	public bool ignoreReturn;

	CameraManager cam;
	float travel = 0;
	GameObject target;
	bool go = false;
	float direction;

	public bool enabled = true;

	void Start () {

		cam = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		target = cam.gameObject;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (!enabled)
			return;
		if (other.tag == "Player") 
		{
			CalculatePos (other.transform.position);

			if (direction > 0) {
				cam.moveToPoint (cam.transform.position, 0);
				travel = 0;
				go = true;

				if (disableCameraLookAt) {
					cam._wm_disableLookAt ();
				}
			} else {
				if (!ignoreReturn) {
					cam.moveToPoint (points [points.Length - 1].position, 1);
				}
			}
		}
	}

	void Update()
	{
		if (go) 
		{
			travel += Time.deltaTime * speedPath;
			GoNextPoint ();
		}
	}

	void GoNextPoint()
	{
		Vector3 nextPoint = iTween.PointOnPath (points, travel);

		cam.moveToPoint (nextPoint, speedCam);

		if (travel >= 1)
			go = false;
	}

	void OnTriggerExit(Collider other) 
	{
		if (!enabled)
			return;
		if (other.tag == "Player") 
		{
			if (!ignoreReturn) 
			{
				go = false;
				cam.moveToOriginalPosition (2);
				if (disableCameraLookAt) {
					cam._wm_enableLookAt ();
				}
			}
		}
	}

	void CalculatePos(Vector3 v)
	{
		Vector3 dir = (v - this.transform.position).normalized;
		direction = Vector3.Dot (dir, this.transform.forward);
	}
}