using UnityEngine;
using System.Collections;

public class QACamera : MonoBehaviour {


	/* properties */

	Vector3 initialPosition;
	float angle;
	float roll;

	float initialRoll;


	/* constants */

	const float angleSpeed = 0.7f;
	const float amplitude = 17.0f;

	const float rollSpeed = 0.45f;
	const float rollAmplitude = 6.0f;


	/* methods */

	// Use this for initialization
	void Start () {

		initialPosition = this.transform.position;
		initialRoll = this.transform.rotation.z;
		angle = 0.0f;
		roll = 0.0f;


	
	}



	// Update is called once per frame
	void Update () {

		Vector3 newPosition;

		newPosition.x = initialPosition.x;
		newPosition.y = initialPosition.y;
		newPosition.z = initialPosition.z + amplitude * Mathf.Sin (angle);

		angle += angleSpeed * Time.deltaTime;

		float newRoll = initialRoll + rollAmplitude * Mathf.Sin (roll);

		roll += rollSpeed * Time.deltaTime;

		this.transform.position = newPosition;
		this.transform.rotation = Quaternion.Euler (0, 0, newRoll);
	
	}


}
