using UnityEngine;
using System.Collections;

public class Stopcock : WisdominiObject {

	public AudioClip[] sounds;
	public LevelControllerScript level;

	public FakePanelAxis axis;

	float angle;
	float targetAngle;
	const float angleSpeed = 30.0f;
	public float angleDelta;

	int soundIndex;

	// Use this for initialization
	new void Start () {
	
		angle = targetAngle = 0.0f;
		updateAngle ();
		soundIndex = 0;

	}

	public void _wm_twist() {
		twist ();
	}

	public void twist() {
		targetAngle += angleDelta;
		if ((level != null) && (sounds.Length > 0) && (soundIndex < sounds.Length) && (sounds [soundIndex] != null)) {
			level.playSound (sounds [soundIndex]);
			soundIndex = (soundIndex + 1) % sounds.Length;
		}
	}

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
	
	// Update is called once per frame
	new void Update () {
	
		bool change = Utils.updateSoftVariable (ref angle, targetAngle, angleSpeed);
		if (change) {
			updateAngle ();
		}

	}
}
