using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flotador : MonoBehaviour {

	public float amplitude;

	public float frecuencia;

	Vector3 initialPos;

	float angle;

	// Use this for initialization
	void Start () {

		initialPos = this.transform.position;	
	}
	
	// Update is called once per frame
	void Update () {

		angle += frecuencia * Time.deltaTime;

		this.transform.localPosition = new Vector3 (0, amplitude * Mathf.Sin (angle), 0);
	}
}
