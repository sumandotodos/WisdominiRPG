using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAroundAuto : MonoBehaviour {

	public Vector3 axis;
	public float speed = 2f;


	void Start () 
	{
		axis = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
	}

	void Update () 
	{
		this.transform.Rotate (axis, speed);

	}
}
