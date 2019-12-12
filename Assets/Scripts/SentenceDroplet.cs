using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceDroplet : MonoBehaviour {

	public float YAccel;
	public float ZAccel;
	public float xAngleSpeed;
	public float ZSpeed;
	float YSpeed;
	float xAngle;
	float y;
	float z;
	float dz;
	float dy;
	public float killDeltaY = 20;
	public float killDeltaZAccel = 6;
	Vector3 position;
	bool started = false;
	public TextMesh theText;

	public void setText(string t) {
		theText.text = t;
	}

	// Use this for initialization
	public void Start () {
		
		if (started)
			return;
		started = true;

		xAngle = 90.0f;

		position = this.transform.position;
		y = position.y;
		z = position.z;
	}
	
	// Update is called once per frame
	void Update () {

		if (xAngle > 0.0f) {
			xAngle -= xAngleSpeed * Time.deltaTime;
		}

		YSpeed += YAccel * Time.deltaTime;

		y -= YSpeed * Time.deltaTime;
		dy += YSpeed * Time.deltaTime;

		z -= ZSpeed * Time.deltaTime;

		if (dz < killDeltaZAccel) {
			ZSpeed += ZAccel * Time.deltaTime;

		}
		dz += ZSpeed * Time.deltaTime;

		if (dz > killDeltaZAccel) {
			ZSpeed = (ZSpeed * 0.96f);
		}

		if (dy > killDeltaY) {
			Destroy (theText.gameObject);
			Destroy (this.gameObject);
		}

		position.y = y;
		position.z = z;
		this.transform.rotation = Quaternion.Euler (xAngle, 0, 0);
		this.transform.position = position;

	}
}
