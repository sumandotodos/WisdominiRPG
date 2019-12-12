using UnityEngine;
using System.Collections;

public class LightLevitatingFollower : MonoBehaviour {

	/* references */

	/* properties */

	float yRotation;

	/* public properties */

	public float yRotationSpeed = 8.0f;

	// Use this for initialization
	void Start () {
	
		yRotation = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {

		yRotation += yRotationSpeed * Time.deltaTime;

		this.transform.rotation = Quaternion.Euler (0, yRotation, 0);
	
	}
}
