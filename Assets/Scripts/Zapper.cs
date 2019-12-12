using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zapper : MonoBehaviour {

	MasterControllerScript mc;
	public GameObject[] objectToZap;
	public string zapVariable;
	public bool zapOnTrue;

	// Use this for initialization
	void Start () {
		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		bool zap = mc.getStorage ().retrieveBoolValue (zapVariable);
		bool performZap = zap;
		if (zapOnTrue == false)
			performZap = !performZap;
		if (performZap) {
			for (int i = 0; i < objectToZap.Length; ++i) {
				Destroy (objectToZap [i]);
			}
		}
	}

}
