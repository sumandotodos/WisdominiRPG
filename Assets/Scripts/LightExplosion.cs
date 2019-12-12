using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightExplosion : WisdominiObject {

	new public Light light;
	SoftFloat intensity;
	public float maxIntensity;
	public float minIntensity;
	public float speed;
	public int nCycles;
	int leftCycles;

	// Use this for initialization
	new void Start () {
		intensity = new SoftFloat ();
		intensity.setSpeed (speed);
		light.intensity = minIntensity;
	}

	public void explode() {
		state = 1;
		leftCycles = nCycles;
	}

	public void _wm_explode() {
		explode ();
	}
		
	int state = 0;

	// Update is called once per frame
	new void Update () {
		if (state == 0) {

		}
		if (state == 1) {
			intensity.setValue (maxIntensity);
			state = 2;
		}
		if (state == 2) {
			light.intensity = intensity.getValue ();
			if (!intensity.update ()) {
				state = 3;
				intensity.setValue (minIntensity);
			}
		}
		if (state == 3) {
			light.intensity = intensity.getValue ();
			if (!intensity.update ()) {
				--leftCycles;
				light.intensity = minIntensity;
				if (leftCycles == 0) {
					state = 0;
				} else {
					state = 1;
				}
			}
		}
	}


}
