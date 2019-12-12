using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N4AuxBlastEstudiante : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MasterControllerScript mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		if (mc.getStorage ().retrieveStringValue ("FollowingChar").Equals ("")) {
			Destroy (GameObject.Find ("Estudiante"));
		}
	}
	

}
