using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

	public GameObject planet;
	public Vector3 axis;
	public float speed;

	void Start () 
	{
		
	}
	
	void Update ()
	{
		if (planet == null) 
		{
			this.gameObject.transform.Rotate (axis, speed);
		}
		else 
		{
			this.gameObject.transform.RotateAround (planet.transform.position, axis, speed);
		}
	}
}
