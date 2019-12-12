using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuendeKiller : MonoBehaviour {

	MasterControllerScript mc;
	DataStorage ds;

	// Use this for initialization
	void Start () {
		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = mc.getStorage ();
		if (ds.retrieveStringValue ("FollowingChar").StartsWith ("Duende")) {
			ds.storeStringValue ("FollowingChar", "");
			ds.storeIntValue ("N4DuendeElegido", 0);
		}
	}
	

}
