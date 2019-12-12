//using UnityEngine;
//using System.Collections;
//
//public class SetCameraInclination : WisdominiObject {
//
//	public GameObject target;
//	public float maxDist;
//
//	float currentDist; 
//	public float saveX;
//	Vector3 colPoint;
//	float direction;
//
//	CameraManager cam;
//	GameObject pivotX;
//	public float angleX;
//	public bool autoenable = true;
//	bool enable;
//	public bool gate = false;
//	bool turnable = true;
//
//
//
//	void Start () 
//	{
//		cam = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
//		pivotX = cam.pivotX;
//		enable = autoenable;
//	}	
//
//	void Update()
//	{
//		if (target == null) 
//		{
//			return;
//		} 
//
//		else 
//		{
//			Vector2 targetPos = new Vector2 (target.transform.position.y, target.transform.position.z);
//			Vector2 thisPos = new Vector2 (colPoint.y, colPoint.z);
//			currentDist = Vector2.Distance (thisPos, targetPos);
//
//			if (currentDist > maxDist) 
//			{
//				if (turnable) {
//					turnable = false;
//					cam.setTargetX (saveX, 0.2f);
//				}
//			} 
//			else 
//			{
//				turnable = true;
//				float x = (currentDist * angleX) / maxDist;
//				cam.setTargetX (saveX + x, 0.5f);
//			}
//		}
//	}
//
//	void OnTriggerEnter(Collider other) 
//	{
//		if (!enable)
//			return;
//
//		if (other.tag == "Player") 
//		{
//			if (!gate) {
//				CalculatePos (other.transform.position);
//
//				if (direction > 0 && target == null) {
//					target = other.gameObject;
//					saveX = pivotX.transform.localEulerAngles.x;
//					colPoint = other.gameObject.transform.position;
//				} 
//
//				if (direction < 0 && target != null) {
//					cam.setTargetX (saveX, 1);
//					target = null;
//				}
//			}
//		}
//	}
//
//	void OnTriggerExit(Collider other) 
//	{
//		if (!enable)
//			return;
//
//		if (other.tag == "Player")
//		{
//			if (gate) {
//				cam.setTargetX (saveX, 1);
//				return;
//			} else {
//				CalculatePos (other.transform.position);
//
//				if (direction > 0 && target != null) {
//					cam.setTargetX(saveX, 1);
//					target = null;
//				}
//
//				if (direction < 0 && target == null) 
//				{
//					target = other.gameObject;
//					saveX = pivotX.transform.localEulerAngles.y;
//					colPoint = other.gameObject.transform.position;
//				}
//			}
//		}
//	}
//
//	void CalculatePos(Vector3 v)
//	{
//		Vector3 dir = (v - this.transform.position).normalized;
//		direction = Vector3.Dot (dir, this.transform.forward);
//	}
//
//	public void _wm_enable() 
//	{
//		enable = true;
//	}
//
//	public void _wm_disable() 
//	{
//		enable = false;
//	}
//
//}

using UnityEngine;
using System.Collections;

public class SetCameraInclination : WisdominiObject {

	public string prevTrigger;
	public GameObject target;
	public float maxDist;
	public float angleX; 

	public float activateEntryAngle;
	public bool activated;
	public bool requireEntryAngle = false;
	const float THRESHOLD = 0.1f;

	public bool enable = true;

	CameraManager cam;
	public float currentDist; 
	public float saveX;
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

		float _saveX = ds.retrieveFloatValue (this.gameObject.name + level.locationName + "SaveX");
		saveX = _saveX;

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
				float x = (currentDist * angleX) / maxDist;
				cam.setTargetX (saveX + x, 0.5f);
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{

		if (!enable)
			return;

		if (other.tag == "Player") 
		{


			CalculatePos (other.transform.position);

			if (direction > 0 && target == null) 
			{

				float eulerX = cam.pivotX.transform.localEulerAngles.x;
				while (eulerX > 180.0f)
					eulerX -= 360.0f;

				activated = true;
				if (requireEntryAngle && (Mathf.Abs(activateEntryAngle-eulerX)>THRESHOLD)) {
					activated = false;
					return;
				}

				target = other.gameObject;
				saveX = eulerX;
				colPoint = other.gameObject.transform.position;
				ds.storeFloatValue (this.gameObject.name + level.locationName + "SaveX", saveX);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointX", colPoint.x);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointY", colPoint.y);
				ds.storeFloatValue (this.gameObject.name + level.locationName + "pointZ", colPoint.z);
				ds.storeBoolValue (this.gameObject.name + level.locationName + "HaveTarget", true);
				controller.SaveLastTrigger (this.gameObject, level.locationName);
			} 

			if (direction < 0 && target != null) 
			{
				cam.setTargetX (saveX, 1);
				target = null;
				ds.storeBoolValue (this.gameObject.name + level.locationName + "HaveTarget", false);
				controller.NextLastTrigger (level.locationName);
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (!enable)
			return;

		if (other.tag == "Player") 
		{
			if (!activated)
				return;
			CalculatePos (other.transform.position);

			if (direction > 0 && target != null) {
				cam.setTargetX (saveX, 1);
				target = null;
				ds.storeBoolValue (this.gameObject.name + level.locationName + "HaveTarget", false);
				controller.NextLastTrigger (level.locationName);
			}

			if (direction < 0 && target == null) 
			{

				float eulerX = cam.pivotX.transform.localEulerAngles.x;
				while (eulerX > 180.0f)
					eulerX -= 360.0f;

				target = other.gameObject;
				saveX = eulerX;
				colPoint = other.gameObject.transform.position;
				ds.storeFloatValue (this.gameObject.name + level.locationName + "SaveX", saveX);
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

	public void _wm_enable() 
	{
		enable = true;
	}

	public void _wm_disable() 
	{
		enable = false;
	}
}

