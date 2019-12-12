using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraFollowDirectionGate : WisdominiObject {

	public string prevTrigger;
	public GameObject target;
	public float maxDist;
	public float angleYin; 
	public float angleYout;

	CameraManager cam;
	float currentDist; 
	//public float saveY;
	Vector3 colPoint;
	bool turnable = false;
	DataStorage ds;
	LevelControllerScript level;
	ListCameraFollowDirection controller;
	bool enter;

	new void Start () 
	{
		cam = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		ds = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ().getStorage();
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		controller = GameObject.Find ("ControllerDirection").GetComponent<ListCameraFollowDirection> ();

		bool getTarget = ds.retrieveBoolValue (this.gameObject.name + level.locationName + "HaveTarget");
		if (getTarget) 
		{
			target = GameObject.Find ("Player").gameObject;
		}

		string getLast = ds.retrieveStringValue("LastTriggerIn" + level.locationName);
		prevTrigger = getLast;
		if (getLast == this.transform.name) 
		{
			turnable = true;
		}

		//float _saveY = ds.retrieveFloatValue (this.gameObject.name + level.locationName + "SaveY");
		//saveY = _saveY;

		enter = ds.retrieveBoolValue (this.gameObject.name + level.locationName + "Enter");

		float x, y, z;
		x = ds.retrieveFloatValue (this.gameObject.name + level.locationName + "pointX");
		y = ds.retrieveFloatValue (this.gameObject.name + level.locationName + "pointY");
		z = ds.retrieveFloatValue (this.gameObject.name + level.locationName + "pointZ");
		if ((x != 0.0f) && (y != 0.0f) && (z != 0.0f))
		{
			Vector3 newPos = new Vector3 (x, y, z);
			colPoint = newPos;
		}
	}

	new void Update () 
	{
		if (target == null) 
		{
			return;
		} 

		else 
		{
			if (enter) 
			{
				Vector2 targetPos = new Vector2 (target.transform.position.x, target.transform.position.z);
				Vector2 thisPos = new Vector2 (colPoint.x, colPoint.z);
				currentDist = Vector2.Distance (thisPos, targetPos);

				if (currentDist > maxDist) 
				{
					if (turnable) 
					{
						//cam.setTargetY (saveY + angleY, 0.2f); 
						cam.setTargetY (angleYin, 0.2f);
						turnable = false;
					}
				}
				else 
				{
					turnable = true;
					float y = (currentDist * angleYin) / maxDist;
					//cam.setTargetY (saveY + y, 0.5f);
					cam.setTargetY (y, 0.5f);
				}
			} 
			else 
			{
				Vector2 targetPos = new Vector2 (target.transform.position.x, target.transform.position.z);
				Vector2 thisPos = new Vector2 (colPoint.x, colPoint.z);
				currentDist = Vector2.Distance (thisPos, targetPos);

				if (currentDist > maxDist) 
				{
					if (turnable) 
					{
						//cam.setTargetY (saveY - angleY, 0.2f);
						cam.setTargetY (angleYin, 0.2f);
						turnable = false;
						target = null;
						controller.NextLastTrigger (level.locationName);
					}
				} 
				else 
				{
					turnable = true;
					float y = (currentDist * angleYin) / maxDist;
					//cam.setTargetY (saveY - y, 0.5f);
					cam.setTargetY (y, 0.5f);
				}
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player") 
		{
			if (target == null) 
			{
				target = other.gameObject;
				//saveY = cam.pivotY.transform.localEulerAngles.y;
				enter = true;
				ds.storeBoolValue (this.gameObject.name + level.locationName + "Enter", enter);
				colPoint = other.gameObject.transform.position;
				//ds.storeFloatValue (this.gameObject.name + level.locationName + "SaveY", saveY);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointX", colPoint.x);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointY", colPoint.y);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointZ", colPoint.z);
				ds.storeBoolValue (this.gameObject.name + level.locationName + "HaveTarget", true);
				controller.SaveLastTrigger (this.gameObject, level.locationName);
			} 
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player") 
		{
			if (target != null) 
			{
				target = other.gameObject;
				enter = false;		
				ds.storeBoolValue (this.gameObject.name + level.locationName + "Enter", enter);
				//saveY = cam.pivotY.transform.localEulerAngles.y;
				colPoint = other.gameObject.transform.position;
				//ds.storeFloatValue (this.gameObject.name + level.locationName + "SaveY", saveY);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointX", colPoint.x);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointY", colPoint.y);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointZ", colPoint.z);
				ds.storeBoolValue (this.gameObject.name + level.locationName + "HaveTarget", true);
				controller.SaveLastTrigger (this.gameObject, level.locationName);
			}
		}
	}
}
