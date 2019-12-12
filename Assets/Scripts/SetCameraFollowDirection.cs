using UnityEngine;
using System.Collections;

public class SetCameraFollowDirection : WisdominiObject {

	public string prevTrigger;
	public GameObject target;
	public float maxDist;
	public float angleY; 

	public float activateEntryAngle;
	public bool activated;
	public bool requireEntryAngle = false;
	const float THRESHOLD = 0.1f;

	CameraManager cam;
	public float currentDist; 
	public float saveY;
	Vector3 colPoint;
	public float direction;
	bool turnable = false;
	DataStorage ds;
	LevelControllerScript level;

	ListCameraFollowDirection controller;

	public bool killTheNoise = false;

	new void Start () 
	{
		cam = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		ds = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ().getStorage();
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		controller = GameObject.Find ("ControllerDirection").GetComponent<ListCameraFollowDirection> ();

		bool getTarget = ds.retrieveBoolValue (this.gameObject.name + level.locationName + "HaveTarget");

		if (killTheNoise) {
			getTarget = false;
			ds.storeBoolValue (this.gameObject.name + level.locationName + "HaveTarget", false);
		}

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

		float _saveY = ds.retrieveFloatValue (this.gameObject.name + level.locationName + "SaveY");
		saveY = _saveY;

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
			Vector2 targetPos = new Vector2 (target.transform.position.x, target.transform.position.z);
			Vector2 thisPos = new Vector2 (colPoint.x, colPoint.z);
			currentDist = Vector2.Distance (thisPos, targetPos);
			if (currentDist > maxDist) 
			{
//				if (turnable) 
//				{
//					cam.setTargetY (saveY + angleY, 0.2f); 
//					turnable = false;
//				}
			} 
			else 
			{
				turnable = true;
				float y = (currentDist * angleY) / maxDist;
				cam.setTargetY (saveY + y, 0.5f);
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
        Debug.Log("<color=yellow>SetCameraFollowDirection</color>");
        if (other.tag == "Player") 
		{
			

			CalculatePos (other.transform.position);

			if (direction > 0 && target == null) 
			{

				float eulerY = cam.pivotY.transform.localEulerAngles.y;
				while (eulerY > 180.0f)
					eulerY -= 360.0f;

				activated = true;
				if (requireEntryAngle && (Mathf.Abs(activateEntryAngle-eulerY)>THRESHOLD)) {
					activated = false;
					return;
				}

				target = other.gameObject;
				saveY = eulerY;
				colPoint = other.gameObject.transform.position;
				ds.storeFloatValue (this.gameObject.name + level.locationName + "SaveY", saveY);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointX", colPoint.x);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointY", colPoint.y);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointZ", colPoint.z);
				ds.storeBoolValue (this.gameObject.name + level.locationName + "HaveTarget", true);
				controller.SaveLastTrigger (this.gameObject, level.locationName);
			} 

			if (direction < 0 && target != null) 
			{
				cam.setTargetY (saveY, 1);
				target = null;
				ds.storeBoolValue (this.gameObject.name + level.locationName + "HaveTarget", false);
				controller.NextLastTrigger (level.locationName);
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player") 
		{
			if (!activated)
				return;
			CalculatePos (other.transform.position);

			if (direction > 0 && target != null) {
				cam.setTargetY (saveY, 1);
				target = null;
				ds.storeBoolValue (this.gameObject.name + level.locationName + "HaveTarget", false);
				controller.NextLastTrigger (level.locationName);
			}

			if (direction < 0 && target == null) 
			{

				float eulerY = cam.pivotY.transform.localEulerAngles.y;
				while (eulerY > 180.0f)
					eulerY -= 360.0f;

				target = other.gameObject;
				saveY = eulerY;
				colPoint = other.gameObject.transform.position;
				ds.storeFloatValue (this.gameObject.name + level.locationName + "SaveY", saveY);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointX", colPoint.x);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointY", colPoint.y);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointZ", colPoint.z);
				ds.storeBoolValue (this.gameObject.name + level.locationName + "HaveTarget", true);
				controller.SaveLastTrigger (this.gameObject, level.locationName);
			}
		}
	}

	void CalculatePos(Vector3 v)
	{
		Vector3 dir = (v - this.transform.position).normalized;
		direction = Vector3.Dot (dir, this.transform.forward);
	}
}
