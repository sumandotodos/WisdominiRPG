using UnityEngine;
using System.Collections;

public class EvenOddChecker : WisdominiObject {

	/* references */
	MasterControllerScript mc;
	DataStorage ds;

	public string variable;

	// Use this for initialization
	new void Start () {

		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = mc.getStorage ();
	
	}
	
	// Update is called once per frame
	new void Update () {
	
	}

	public bool _wm_isEven() {

		return (ds.retrieveIntValue (variable) % 2) == 0;

	}
}
