using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulse : MonoBehaviour {

	new public Light light;
	public float frequency = 15.0f;
	public float min = 0.5f;
	public float max = 1.0f;
	public float initialPhase = 0.0f;
	float phase;

	// Use this for initialization
	void Start () {
		phase = 0;
	}
	
	// Update is called once per frame
	void Update () {
		light.intensity = (min + ((max - min) / 2.0f) * (Mathf.Sin (phase) + 1));
		phase += (frequency * Time.deltaTime);
	}
}
