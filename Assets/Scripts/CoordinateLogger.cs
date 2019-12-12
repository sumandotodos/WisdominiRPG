using UnityEngine;
using System.Collections;

public class CoordinateLogger : MonoBehaviour {

	/* references */


	public LevelControllerScript level;

	public string targetLocation = "";

	MasterControllerScript masterController;
	DataStorage ds;
	CameraManager cam;

	void Start () 
	{
		masterController = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = masterController.getStorage ();
		cam = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
	}

	void OnTriggerEnter(Collider other) 
	{
		if(level==null) level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		if (other.tag == "Player") 
		{
			string loc = level.locationName;
			if (!targetLocation.Equals ("")) 
			{
				loc = targetLocation;
			} 

			float x = this.transform.position.x;
			float z = this.transform.position.z;

			ds.storeFloatValue ("Coords" + loc + "X", x);
			ds.storeFloatValue ("Coords" + loc + "Y", other.transform.position.y);
			ds.storeFloatValue ("Coords" + loc + "Z", z);

			//level.storePhysicalCameraPosition ();

//			ds.storeFloatValue ("Pos" + loc + "X", cam.gameObject.transform.position.x);
//			ds.storeFloatValue ("Pos" + loc + "Y", cam.gameObject.transform.position.y);
//			ds.storeFloatValue ("Pos" + loc + "Z", cam.gameObject.transform.position.z);
//			ds.storeFloatValue ("Rot" + loc + "Y", cam.pivotY.transform.localEulerAngles.y);
//			ds.storeFloatValue ("Rot" + loc + "X", cam.pivotX.transform.localEulerAngles.x);
//			ds.storeFloatValue ("PosIn" + loc + "Z", cam.pivotZ.transform.localPosition.z);
			//ds.storeFloatValue ("PosIn" + loc + "M", cam.mainCamera.transform.localPosition.z);
		}
	}
}
