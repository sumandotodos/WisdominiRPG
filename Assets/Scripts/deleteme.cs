using UnityEngine;
using System.Collections;

public class deleteme : MonoBehaviour {

	float angle;
	float anglespeed = 0.3f;

	// Use this for initialization
	void Start () {

		angle = 0.0f;
	
	}
	
	// Update is called once per frame
	void Update () {

		angle += anglespeed;
		this.transform.rotation = Quaternion.Euler (angle, angle, angle);
	
	}
}
