using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum TriggerMode { onlyPlayer, cameraAndPlayer, playerOnlyInCameraIn };

public class SetCameraFollowDirectionTime : WisdominiObject {

	public GameObject target;
	public float timeToTurn;
	public float angleYin; 
	public float angleYout;

    const float recoilTime = 1.0f;
    float recoilRemain = 1.0f;

	bool playerInside = false;

	CameraManager cam;
	float currentDist; 
	
	Vector3 colPoint;
	float direction;
	bool turnable = false;
	DataStorage ds;
	LevelControllerScript level;

    public bool gateMode = false;
	public bool directed = false;
	public bool relative = true;
	public bool setAtExit = true;

	ListCameraFollowDirection controller;


    // reentrant state
    float saveY;


    new void Start () 
	{
		cam = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		ds = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ().getStorage();
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		controller = GameObject.Find ("ControllerDirection").GetComponent<ListCameraFollowDirection> ();
        recoilRemain = recoilTime;

//		string getLast = ds.retrieveStringValue("LastTriggerIn" + level.locationName);
//		if (getLast == this.transform.name) 
//		{
//			turnable = true;
//		}

		//float _saveY = ds.retrieveFloatValue (this.gameObject.name + level.locationName + "SaveY");
		//saveY = _saveY;
		//cam.setTargetY (saveY, 0.0f);
	}

    private void Update()
    {
        if(recoilRemain > 0.0f)
        {
            recoilRemain -= Time.deltaTime;
        }
    }

    void OnTriggerEnter (Collider other)
	{
        if (recoilRemain > 0.0f)
        {
            saveY = cam.pivotY.transform.localEulerAngles.y;
            Debug.Log("Trigger rejected");
            return;
        }
		if (other.tag == "Player") 
		{			

			playerInside = true;

			if ((directed) || gateMode) {
				// valorar return sin hacer nada
				Vector3 centerVector = (this.transform.position - other.transform.position);
				if (Vector3.Dot (this.transform.forward, centerVector) > 0.0f) {
					Debug.Log ("<color=red>Entrando al revés</color>");
					return;
				}
				Debug.Log ("<color=green>Entrando al derecho</color>");
			}

			saveY = cam.pivotY.transform.localEulerAngles.y;
			if (relative) {
				cam.setTargetY (saveY + angleYin, timeToTurn);
				//ds.storeFloatValue (this.gameObject.name + level.locationName + "SaveY", saveY + angleY);
			} else {
				cam.setTargetY (angleYin, timeToTurn);
				//ds.storeFloatValue (this.gameObject.name + level.locationName + "SaveY", angleY);
			}
			//controller.SaveLastTrigger (this.gameObject, level.locationName);
		}
	}

	void OnTriggerExit (Collider other)
	{
        if (recoilRemain > 0.0f) return;
        if (!setAtExit)
			return;
		
		//Debug.Log ("Exitting trigger: " + other.name);
		if (other.tag == "Player") 
		{			
			playerInside = false;

			if (directed || gateMode) {
				//return;
				// valorar return sin hacer nada
				Vector3 centerVector = (this.transform.position - other.transform.position);
				if (Vector3.Dot (this.transform.forward, centerVector) > 0.0f) {
					Debug.Log ("<color=red>Saliendo al revés</color>");
					return;
				}
				Debug.Log ("<color=green>Saliendo al derecho</color>");
			}

			if (relative) {
				cam.setTargetY (saveY - angleYout, timeToTurn);
			} else {
				cam.setTargetY (angleYout, timeToTurn);
				Debug.Log ("restoring angle Y");
			}
			
			//controller.NextLastTrigger (level.locationName);
		}
	}
}