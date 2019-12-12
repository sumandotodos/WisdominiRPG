using UnityEngine;
using System.Collections;

public class CameraPivot : MonoBehaviour {

	public float pitchAmplitude;
	public float pitchSpeed;

	public float yawAmplitude;
	public float yawSpeed;

	float elapsedTime;

	// Use this for initialization
	void Start () {
	
		elapsedTime = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {

		float pangle = pitchAmplitude * Mathf.Sin (pitchSpeed * elapsedTime);
		float yangle = yawAmplitude * Mathf.Cos (yawSpeed * elapsedTime);

		this.transform.rotation = Quaternion.Euler (pangle, yangle, 0);

		elapsedTime += Time.deltaTime;

	}
}
