using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuendesCocoController : MonoBehaviour {

	MasterControllerScript mc;
	public GameObject[] duende;
	// si está solucionado, matarlo

	void Start() {
		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		DataStorage ds = mc.getStorage ();
		for (int i = 0; i < duende.Length; ++i) {
			bool clear = ds.retrieveBoolValue ("N4Guardia" + (i + 1) + "Clear");
			if (clear)
				Destroy (duende [i]);
		}
	}
}
