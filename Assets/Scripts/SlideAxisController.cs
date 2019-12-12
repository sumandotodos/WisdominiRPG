using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideAxisController : WisdominiObject {

	MasterControllerScript mc;

	public SetCameraSlideAxis[] slide;
	public SetCameraHoldByPoints[] holds;

	void Start() {
		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		for (int i = 0; i < slide.Length; ++i) {
			bool clear = mc.getStorage ().retrieveBoolValue ("N4Guardia"+(i+1)+"Clear");
			if (clear) {
				slide [i].enabled = true;
				if (holds [i] != null) {
					holds [i].enabled = true;
				}
			} else {
				slide [i].enabled = false;
				if (holds [i] != null) {
					holds [i].enabled = false;
				}
			}
		}
	}

	public void _wm_enableSlideAxis(int n) {
		slide [n].enabled = true;
		if (holds [n] != null) {
			holds [n].enabled = true;
		}
	}

}
