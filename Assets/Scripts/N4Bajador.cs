using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N4Bajador : WisdominiObject {

	public float initialY;
	public float finalY;

	public string StorageVariable;

	public LevelControllerScript levelController;

	SoftFloat y;

	// Use this for initialization
	void Start () {
		y = new SoftFloat (initialY);
		y.setSpeed (10.0f);
		y.setTransformation (TweenTransforms.cubicOut);
		Vector3 pos = this.transform.position;
		pos.y = initialY;
		this.transform.position = pos;
		if (levelController == null) {
			levelController = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		}
		if (levelController.retrieveBoolValue (StorageVariable)) {
			y.setValueImmediate (finalY);
			Vector3 temp = this.transform.position;
			temp.y = y.getValue ();
			this.transform.position = temp;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (y.update ()) {
			Vector3 pos = this.transform.position;
			pos.y = y.getValue ();
			this.transform.position = pos;
		}
	}

	public void _wm_baja() {
		y.setValue (finalY);
		levelController.storeBoolValue (StorageVariable, true);
	}
}
