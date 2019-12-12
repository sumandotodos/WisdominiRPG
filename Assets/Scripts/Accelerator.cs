using UnityEngine;
using System.Collections;

public class Accelerator : WisdominiObject {

	public FakePanelAxis axis;
	public VectorDirection direction;
	public float maxSpeed;
	public float acceleration;
	public float initialSpeed = 0.0f;
	public Vector3 destination;
	public float time;

	float speed;
	int state = 0; // 0 :   idle
				   // 1 :   accelerating
				   // 2 :   decelerating

	public void _wm_accelerate() {
		state = 1;
	}

	public void _wm_stop() {
		state = 2;
	}

	public void _wm_setSpeed(float s) {
		speed = s;
		if (speed > maxSpeed)
			speed = maxSpeed;
	}

	new void Start() {
		speed = initialSpeed;
		if (speed > 0.0f)
			state = 1;
	}

	public void _wm_GoLift () 
	{
		iTween.MoveTo (this.gameObject, destination, time);	
	}
	
	// Update is called once per frame
	new void Update () {
	
		if (state == 0) {
			return;
		}

		if (state == 1) {
			speed += acceleration * Time.deltaTime;
			if (speed > maxSpeed)
				speed = maxSpeed; // limit that speed!
		} 

		else if (state == 2) {
			speed -= acceleration * Time.deltaTime;
			if (speed < 0.0f) {
				speed = 0.0f;
				state = 0;
			}
		}

		if ((state == 1) || (state == 2)) {
			float displacement = speed * Time.deltaTime;
			if (direction == VectorDirection.negative)
				displacement *= -1.0f;
			Vector3 vDispl = Vector3.zero;
			switch (axis) {
			case FakePanelAxis.x:
				vDispl = new Vector3 (displacement, 0, 0);
				break;
			case FakePanelAxis.y:
				vDispl = new Vector3 (0, displacement, 0);
				break;
			case FakePanelAxis.z:
				vDispl = new Vector3 (0, 0, displacement);
				break;
			}
			this.transform.position += vDispl;
		}

	}


}
