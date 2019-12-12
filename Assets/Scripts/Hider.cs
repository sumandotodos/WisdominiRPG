using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : WisdominiObject {

	public bool StartHidden;

	Vector3 initialPosition;
	Vector3 aTomarPorCuloPosition;

	bool fromOutside = false;

	public void hide() {
		
			this.transform.position = aTomarPorCuloPosition;

	}

	public void unhide() {
		
			this.transform.position = initialPosition;

	}

	public void _wm_hide() {
		fromOutside = true;
		hide ();
	}

	public void _wm_unhide() {
		fromOutside = true;
		unhide();
	}

	// Use this for initialization
	new void Start () {
		initialPosition = this.transform.position;
		aTomarPorCuloPosition = new Vector3 (0, 10000, 0);
		if (StartHidden) {
			if(fromOutside == false) this.transform.position = aTomarPorCuloPosition;
		} else {
			if(fromOutside == false) this.transform.position = initialPosition;
		}
	}
	

}
