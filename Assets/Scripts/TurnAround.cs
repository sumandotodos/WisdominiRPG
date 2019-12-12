using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAround : MonoBehaviour {

	public Vector3 axis;
	public float speed;


	void Start () {
		
	}
	
	void Update () {
		this.transform.Rotate (axis, speed);
		
	}
}
