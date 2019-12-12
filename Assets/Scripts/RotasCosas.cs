using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotasCosas : MonoBehaviour {

	public Vector3 velocidad;

					
	
	// Update is called once per frame
	void Update () {

		this.transform.Rotate (velocidad*Time.deltaTime);
	}
}
