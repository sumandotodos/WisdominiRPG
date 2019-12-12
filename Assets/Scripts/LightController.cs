using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : WisdominiObject {

	MasterControllerScript mc;
	public Light light;
	public GameObject[] hiddenThing;
	public Color offColor;
	Color originalColor;
	Color ambientColor;
	float originalIntensity;
	bool state;

	void Start() {
		originalColor = light.color;
		originalIntensity = light.intensity;
		ambientColor = RenderSettings.ambientLight;
		state = true;
		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		bool storedState = !mc.getStorage ().retrieveBoolValue (this.name);
		if (storedState) {
			_wm_turnOnLight ();
		} else
			_wm_turnOffLight ();
	}

	public void _wm_turnOnLight() {
		for(int i = 0; i < hiddenThing.Length; ++i) 
			hiddenThing[i].SetActive (false);
		light.intensity = originalIntensity;
		light.color = originalColor;
		RenderSettings.ambientLight = ambientColor;
		state = true;
		mc.getStorage ().storeBoolValue (this.name, false);
	}

	public void _wm_turnOffLight() {
		for(int i = 0; i < hiddenThing.Length; ++i) 
			hiddenThing[i].SetActive (true);
		light.intensity = 0.1f;
		light.color = offColor;
		RenderSettings.ambientLight = offColor;
		state = false;
		mc.getStorage ().storeBoolValue (this.name, true);
	}

	public void _wm_switch() {
		if (state)
			_wm_turnOffLight ();
		else
			_wm_turnOnLight ();
	}
}
