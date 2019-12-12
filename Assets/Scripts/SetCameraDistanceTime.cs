using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraDistanceTime : WisdominiObject {

	public GameObject target;
	public float timeToTurn;
	public float distZin; 
	public float distZout;

	CameraManager cam;
	float currentDist; 
	public float saveY;
	Vector3 colPoint;
	float direction;
	bool turnable = false;
	DataStorage ds;
	LevelControllerScript level;

	public bool gateMode = false;
	public bool relative = true;


	ListCameraFollowDirection controller;

	new void Start () 
	{
		cam = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		ds = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ().getStorage();
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		controller = GameObject.Find ("ControllerDirection").GetComponent<ListCameraFollowDirection> ();

		//		string getLast = ds.retrieveStringValue("LastTriggerIn" + level.locationName);
		//		if (getLast == this.transform.name) 
		//		{
		//			turnable = true;
		//		}

		//float _saveY = ds.retrieveFloatValue (this.gameObject.name + level.locationName + "SaveY");
		//saveY = _saveY;
		//cam.setTargetY (saveY, 0.0f);
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player") 
		{			
			if (gateMode) {
				// valorar return sin hacer nada
				Vector3 centerVector = (this.transform.position - other.transform.position);
				if (Vector3.Dot (this.transform.forward, centerVector) > 0.0f)
					return;
			}

			//saveY = cam.pivotY.transform.localEulerAngles.y;
			if (relative) {
                float Z = cam.GetDistanceZ();
                saveY = Z;
				cam.setDistanceZ (Z + distZout, timeToTurn);
				//ds.storeFloatValue (this.gameObject.name + level.locationName + "SaveY", saveY + angleY);
			} else {
				cam.setDistanceZ (distZin);
				//ds.storeFloatValue (this.gameObject.name + level.locationName + "SaveY", angleY);
			}
			//controller.SaveLastTrigger (this.gameObject, level.locationName);
		}
	}

	void OnTriggerExit (Collider other)
	{
		Debug.Log ("Exitting trigger: " + other.name);
		if (other.tag == "Player") 
		{			
			if (gateMode) {
				return;
				// valorar return sin hacer nada
				Vector3 centerVector = (this.transform.position - other.transform.position);
				if (Vector3.Dot (this.transform.forward, centerVector) > 0.0f)
					return;
			}

			if (relative) {
				cam.setDistanceZ (saveY, timeToTurn);
			} else {
				cam.setDistanceZ (distZout, timeToTurn);
			}

			//controller.NextLastTrigger (level.locationName);
		}
	}
}
