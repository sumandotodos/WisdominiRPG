using UnityEngine;
using System.Collections;

public class Pinpoint : MonoBehaviour {

	/* properties */

	float angle;
	public float angleSpeed = 30.0f;
	public float amplitude = 10.0f;
	float y;
	float x;

	// Use this for initialization
	void Start () {

		angle = 0.0f;
		y = this.transform.localPosition.y;
		x = this.transform.localPosition.x;

	}
	
	// Update is called once per frame
	void Update () {
	
		angle += angleSpeed * Time.deltaTime;
		Vector3 newPos = new Vector3 (x, y + amplitude*Mathf.Sin(angle), 0);
		this.transform.localPosition = newPos;


	}
}
