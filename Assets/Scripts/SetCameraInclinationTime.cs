using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerMode { onlyPlayer, cameraAndPlayer, playerOnlyInCameraIn };

public class SetCameraInclinationTime : WisdominiObject {

	public GameObject target;
	public float timeToTurn;
	public float angleXin; 
	public float angleXout;

	CameraManager cam;
	float currentDist; 
	public float saveY;
	Vector3 colPoint;
	float direction;
	bool turnable = false;
	DataStorage ds;
	LevelControllerScript level;
	public bool cameraHasEntered = false;

	public bool gateMode = false;
	public bool relative = true;
	public bool directed = false;

	public bool enabled = true;
	public bool reentrant = false;

	public TriggerMode mode = TriggerMode.cameraAndPlayer;

	ListCameraFollowDirection controller;

	bool cameraIn;
	bool playerIn;

	new void Start () 
	{
		//enabled = true;
		cam = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		ds = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ().getStorage();
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		controller = GameObject.Find ("ControllerDirection").GetComponent<ListCameraFollowDirection> ();

		cameraIn = false;
		playerIn = false;

		if (reentrant) {
			enabled = level.retrieveBoolValue ("is" + this.name + "Enabled");
		}

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
		//Debug.Log ("I-T ENTER: " + other.name);
		if (!enabled)
			return;
		if (other.tag == "Player" || (other.tag == "MainCamera" && mode == TriggerMode.cameraAndPlayer)) 
		{			
			if (other.tag == "MainCamera") {
				cameraHasEntered = true;
				cameraIn = true;
			}
			if (other.tag == "Player") {
				playerIn = true;
			}
			if (directed || gateMode) {
				// valorar return sin hacer nada
				Vector3 centerVector = (this.transform.position - other.transform.position);
				if (Vector3.Dot (this.transform.forward, centerVector) > 0.0f)
					return;
			}

			saveY = cam.pivotY.transform.localEulerAngles.y;
			if (relative) {
				
				cam.setTargetX (saveY + angleXin, timeToTurn);
				//ds.storeFloatValue (this.gameObject.name + level.locationName + "SaveY", saveY + angleY);
			} else {
				cam.setTargetX (angleXin);
				//ds.storeFloatValue (this.gameObject.name + level.locationName + "SaveY", angleY);
			}
			//controller.SaveLastTrigger (this.gameObject, level.locationName);
		}
	}

	void OnTriggerExit (Collider other)
	{
		//Debug.Log ("I-T EXIT: " + other.name);
		if (!enabled)
			return;
		//if ((other.tag == "MainCamera" && cameraHasEntered == true && mode == TriggerMode.cameraAndPlayer) || (other.tag == "Player" && cameraHasEntered == false)) 
		if(other.tag == "Player") 
			playerIn = false;
		if (other.tag == "MainCamera")
			cameraIn = false;
		if(((!playerIn) && (!cameraIn) && mode == TriggerMode.cameraAndPlayer) || ((!playerIn) && mode == TriggerMode.onlyPlayer))
		{			
			cameraHasEntered = false;
			if (directed || gateMode) {
				
				// valorar return sin hacer nada
				Vector3 centerVector = (this.transform.position - other.transform.position);
				if (Vector3.Dot (this.transform.forward, centerVector) > 0.0f)
					return;
			}

			if (relative) {
				cam.setTargetX (saveY - angleXout, timeToTurn);
			} else {
				cam.setTargetX (angleXout, timeToTurn);
				Debug.Log ("restoring angle X");
			}

			//controller.NextLastTrigger (level.locationName);
		}
	}

	public void _wm_enable() 
	{
		if (reentrant) {
			level.storeBoolValue ("is" + this.name + "Enabled", true);
		}
		enabled = true;
	}

	public void _wm_disable() 
	{
		if (reentrant) {
			level.storeBoolValue ("is" + this.name + "Enabled", false);
		}
		enabled = false;
	}
}