using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrembler : MonoBehaviour {

	new Light light;

	public float minIntensity;
	public float maxIntensity;
	public float noiseWeight;
	public float angleSpeed;
	float time;

	// Use this for initialization
	void Start () {
		light = this.GetComponent<Light> ();
		time = Random.Range (0.0f, 1000.0f);
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime * angleSpeed;
		float val = minIntensity + ((Mathf.Cos (time) + 1) / 2.0f) * maxIntensity;
		float rnd = Random.Range (0, noiseWeight);
		light.intensity = val + rnd;
	}
}
