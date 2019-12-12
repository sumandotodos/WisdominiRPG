using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : WisdominiObject {

	public Vector3 speed;
	Vector3 initialPos;

	// Use this for initialization
	new void Start () {
		initialPos = this.transform.position;
	}
	
	// Update is called once per frame
	new void Update () {
		Vector3 pos = this.transform.position;
		pos += speed * Time.deltaTime;
		this.transform.position = pos;
	}

	public void _wm_restartPosition() {
		this.transform.position = initialPos;
	}

}
