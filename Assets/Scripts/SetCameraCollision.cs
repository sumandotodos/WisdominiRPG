using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraCollision : MonoBehaviour {

	CameraManager cam; 
	GameObject player;
	float saveZ;
	RaycastHit hitCamZ;
	CheckMainCameraCollision cmcc;


	void Start () 
	{
		cam = GameObject.Find("CameraLerp").GetComponent<CameraManager> ();
		cmcc = cam.mainCamera.GetComponent<CheckMainCameraCollision> ();
		player = GameObject.Find ("Player");
		saveZ = this.transform.localPosition.z;
	}

	void Update()
	{
//		if (cam.camSit == CameraSituation.interior) 
//		{
//			float dir = Vector3.Distance(this.transform.position,  player.transform.position);
//			if (Physics.Raycast (this.transform.position, this.transform.forward, dir)) 
//			{
//				if (hitCamZ.collider.tag == "Camera") 
//				{
//					if (hitCamZ.distance < dir && !cam.mainCamera.GetComponent<CheckMainCameraCollision>().moving) 
//					{
//						//cam.SetDistanceM (0, 1);
//					} 
//				}
//			}
//		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (cam.camSit == CameraSituation.exterior) 
		{
			if (other.tag == "Wall") 
			{
				cmcc.state = "ir";
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (cam.camSit == CameraSituation.exterior) 
		{
			if (other.tag == "Wall")  
			{
				cmcc.state = "volver";
			}
		}
	}

}
