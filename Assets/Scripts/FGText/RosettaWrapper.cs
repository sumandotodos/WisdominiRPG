using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosettaWrapper : MonoBehaviour {

	public Rosetta rosetta;

	void Start() {
		if (rosetta == null)
			rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();
	}

}
