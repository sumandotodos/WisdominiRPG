using UnityEngine;
using System.Collections;

public class SecretRamp : WisdominiObject {

	public FakePanelAxis axis;

	float angle;
	float targetAngle;

	public AudioClip sound;
	public LevelControllerScript level;
	public float angleSpeed = 6.0f;
	public float closeAngle = 0.0f;
	public float openAngle = -40.0f;

	void updateAngle() {
		switch(axis) {

		case FakePanelAxis.x:
			this.transform.localRotation = Quaternion.Euler (angle, 0, 0);
			break;
		case FakePanelAxis.y:
			this.transform.localRotation = Quaternion.Euler (0, angle, 0);
			break;
		case FakePanelAxis.z:
			this.transform.localRotation = Quaternion.Euler (0, 0, angle);
			break;

		}
	}

	// Use this for initialization
	new void Start () {

		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		//level.storeBoolValue (this.name, true);
		bool isOpen = level.retrieveBoolValue (this.name);
		if (isOpen) {
			angle = targetAngle = openAngle;
		}
		else angle = targetAngle = closeAngle;
		updateAngle ();
	
	}
	
	// Update is called once per frame
	new void Update () {

		bool change = Utils.updateSoftVariable (ref angle, targetAngle, angleSpeed);
		if (change)
			updateAngle ();
	
	}

	public void _wm_open() {
		open ();
	}

	public void open() {
		if ((level != null) && (sound != null))
			level.playSound (sound);
		targetAngle = openAngle;
		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		level.storeBoolValue (this.name, true);
	}

	public void _wm_close() {
		close ();
	}

	public void close() {
		if ((level != null) && (sound != null))
			level.playSound (sound);
		targetAngle = closeAngle;
		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		level.storeBoolValue (this.name, false);
	}
}
