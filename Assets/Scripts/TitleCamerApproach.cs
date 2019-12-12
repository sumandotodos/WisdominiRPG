using UnityEngine;
using System.Collections;

public class TitleCamerApproach : MonoBehaviour {

	Vector3 initialPosition;
	float zDisplacement;
	public float targetZ;
	float zSpeed;

	// Use this for initialization
	void Start () {

		initialPosition = this.transform.position;
		zDisplacement = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {

		zSpeed = targetZ - this.transform.position.z;
		if (zSpeed > 1.0f)
			zSpeed = 1.0f;
	
		zDisplacement += zSpeed * Time.deltaTime;

		this.transform.position = initialPosition + new Vector3 (0, 0, zDisplacement);

	}
}
