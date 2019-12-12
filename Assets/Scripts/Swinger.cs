using UnityEngine;
using System.Collections;

public class Swinger : MonoBehaviour {

	public float amplitude = 1.5f;
	public float speed = 10.0f;
	Vector3 initialPosition;
	public bool relative = false;
	float angle;

	// Use this for initialization
	void Start () {

		angle = 0.0f;
		if (!relative)
			initialPosition = this.transform.position;
		else
			initialPosition = this.transform.localPosition;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		angle += speed * Time.deltaTime;

		if(!relative) 
		this.transform.position = new Vector3 (initialPosition.x, initialPosition.y 
			+ amplitude * Mathf.Sin (angle), initialPosition.z);
		else
			this.transform.localPosition = new Vector3 (initialPosition.x, initialPosition.y 
				+ amplitude * Mathf.Sin (angle), initialPosition.z);

	}
}
