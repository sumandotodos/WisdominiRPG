using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerMode { onlyPlayer, cameraAndPlayer, playerOnlyInCameraIn, onlyCamera };

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

	public bool cameraIn;
	public bool playerIn;

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

	}

	void OnTriggerEnter (Collider other)
	{
        Debug.Log("Entering: " + other.tag);
		if (!enabled)
			return;
				
		if (other.tag == "PhysicalCamera") {
			cameraHasEntered = true;
			cameraIn = true;
		}
		if (other.tag == "Player") {
			playerIn = true;
		}
        if (mode == TriggerMode.onlyPlayer && (playerIn == false))
        {
            return;
        }
        if (mode == TriggerMode.onlyCamera && (cameraIn == false))
        {
            return;
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
			
		} else {
			cam.setTargetX (angleXin);
			
		}
			
	}

	void OnTriggerExit (Collider other)
	{
        Debug.Log("Exitting: " + other.tag);
        if (!enabled)
			return;
		
		if(other.tag == "Player") 
			playerIn = false;

		if (other.tag == "PhysicalCamera")
			cameraIn = false;

        if (mode == TriggerMode.onlyPlayer && (playerIn == true))
        {
            return;
        }
        if (mode == TriggerMode.onlyCamera && (cameraIn == true))
        {
            return;
        }
        //if (((!playerIn) && (!cameraIn) && mode == TriggerMode.cameraAndPlayer) || ((!playerIn) && mode == TriggerMode.onlyPlayer))
		//{			
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

			
		//}
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