using UnityEngine;
using System.Collections;

public class AvoidCameraInterpenetration : MonoBehaviour {

	public Camera cam;
	public AvoidCameraInterpenetration2 avoid;
	public PlayerScript player;
	public float restDist = 30.0f;
	public float targetDist = 30.0f;
	public float adjustSpeed = 120.0f;
	public float restY = 0.83f;

	// Use this for initialization
	void Start () {

		//cam.transform.localPosition = new Vector3 (0, 0, -restDist);

	}
	
	// Update is called once per frame
	void Update () {

		if (restDist < targetDist) {
			restDist += adjustSpeed * Time.deltaTime;
			if (restDist > targetDist) {
				restDist = targetDist;
			}

		}

		if (restDist > targetDist) {
			restDist -= adjustSpeed * Time.deltaTime;
			if (restDist < targetDist) {
				restDist = targetDist;
			}

		}

		//cam.transform.localPosition = new Vector3 (0, 0, -restDist);
	
	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Wall") {
			avoid.unlock ();
		}

	}

	void OnTriggerExit(Collider other) {

		avoid.reset ();

	}
}
