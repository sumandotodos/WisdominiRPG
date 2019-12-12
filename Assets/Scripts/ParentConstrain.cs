using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentConstrain : MonoBehaviour {

	public GameObject papa;


	
	// Update is called once per frame
	void Update () {
		this.transform.position = papa.transform.position;
	}
}
