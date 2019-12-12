using UnityEngine;
using System.Collections;

public class EasyPlayer : MonoBehaviour {

	public Vector2 clickPoint;
	public Vector2 current;
	public Vector2 diff;
	Rigidbody r;
	int state;
	public float linearSpeed = 10.0f;

	// Use this for initialization
	void Start () {
		r = this.GetComponent<Rigidbody> ();
		state = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 0) { // idle
			current = Input.mousePosition;
			if (Input.GetMouseButtonDown (0)) {
				clickPoint = Input.mousePosition;
				state = 1;
			}
		}
		if (state == 1) { // friggin' moving
			current = Input.mousePosition;
			diff = current - clickPoint;
			r.velocity = new Vector3 (diff.x * linearSpeed, r.velocity.y, diff.y * linearSpeed);
			if (Input.GetMouseButtonUp (0)) {
				state = 0;
			}
		}
	}
}
