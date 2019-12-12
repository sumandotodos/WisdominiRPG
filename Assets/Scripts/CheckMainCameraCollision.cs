using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMainCameraCollision : MonoBehaviour {

	CameraManager cam;
	GameObject player;
	float saveM;

	public bool mobile = true;

	public string state;

	//[HideInInspector]
	public bool touching = false;
	[HideInInspector]
	public bool moving = false;


	void Start()
	{
		cam = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		player = GameObject.Find ("Player");
		state = "idle";
		saveM = this.transform.localPosition.z;
	}

	void Update () 
	{
		switch (state) 
		{
		case "idle":
			break;

		case "ir":
			Alejar ();
			break;

		case "volver":
			Acercar ();
			break;

		case "moviendo":
			if (this.transform.localPosition == Vector3.zero) 
			{
				state = "idle";
			}
			break;

		case "stop":
			mobile = false;
			cam.SetDistanceM (this.transform.position.z, 0.1f);
			break;
		}
		//if (cam.camSit == CameraSituation.interior) {
		//	float dir = Vector3.Distance (this.transform.position, player.transform.position);

		//	RaycastHit[] hitMainCam;
		//	hitMainCam = Physics.RaycastAll (this.transform.position, this.transform.forward, dir);
		//	for (int i = 0; i < hitMainCam.Length; ++i) {
		//		if (hitMainCam [i].collider.tag == "Wall") {
		//			if (hitMainCam [i].distance < dir) {
		//				// SE MUEVE
		//				cam.SetDistanceM (hitMainCam [i].distance, 0.5f);
		//				moving = true;
		//			} else {
		//				moving = false;
		//			}
		//		}
		//	}
		//}
	}

	void Alejar()
	{
		cam.SetDistanceM (this.transform.localPosition.z - 0.5f, 0.5f);
		float newPos = this.transform.localPosition.z - 0.5f;
		state = "moviendo";
		CalculatePos (newPos);

	}

	void Acercar()
	{
		cam.SetDistanceM (saveM, 1);
		//state = "idle";
		state = "moviendo";
		CalculatePos (saveM);
	}

	void CalculatePos(float _newPos)
	{
		if (this.transform.localPosition.z == _newPos) 
		{
			state = "idle";
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Wall") 
		{
			if (state != "moviendo") { // Quizas cambiar por localPos == 0 ?
				state = "ir";
			}
		}
	}

	//void OnTriggerEnter(Collider other)
	//{
	//	if (cam.camSit == CameraSituation.exterior) {
	//		if (other.tag == "Wall") {
	//			touching = true;
	//		}
	//	}
	//}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Wall") 
		{
			if (state != "moviendo")
				state = "stop";
		}
		//if (cam.camSit == CameraSituation.exterior) {
		//	if (other.tag == "Wall") {
		//		touching = false;
		//	}
		//}
	}
}
