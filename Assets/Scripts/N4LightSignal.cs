using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N4LightSignal : MonoBehaviour {

	public Light theLight;
	public float maxIntensity;
	public float speed;
	float intensity;
	float targetIntensity;
	public int nSignals;

	int state = 0;

	int cyclesRemaining;

	// Use this for initialization
	void Start () {
		intensity = 0;
		theLight.intensity = intensity;
		state = 0;
		cyclesRemaining = 0;
	}

	public void _wm_signal() {
		state = 1;
		targetIntensity = maxIntensity;
		cyclesRemaining = nSignals;
	}
	
	// Update is called once per frame
	void Update () {
		if(state != 0)
		theLight.intensity = intensity;

		if (state == 0) {
			// idle
		}

		if (state == 1) {
			intensity += speed * Time.deltaTime;
			if (intensity > maxIntensity) {
				intensity = maxIntensity;
				state = 2;
			}
		}

		if (state == 2) {
			intensity -= speed * Time.deltaTime;
			if (intensity < 0.0f) {
				intensity = 0.0f;
				theLight.intensity = 0;
				--cyclesRemaining;
				if (cyclesRemaining > 0)
					state = 1;
				else
					state = 0;
			}
		}
	}
}
