using UnityEngine;
using System.Collections;

public class CameraLookAt : MonoBehaviour {

	/* references */

	public GameObject target;

	/* public properties */

	public Vector3 upVector;

	Camera cam;

	new public bool enabled;

	// Use this for initialization
	void Start () {
	
		cam = this.GetComponent<Camera> ();
		enabled = true;

	}
	
	// Update is called once per frame
	void Update () {

		if (enabled) {
			//cam.transform.LookAt (target.transform.position);
			cam.transform.LookAt(new Vector3(this.transform.position.x, target.transform.position.y, target.transform.position.z));
		}
	
	}

	public void enable() {

		enabled = true;

	}

	public void disable() {

		enabled = false;

	}
}
