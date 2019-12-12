using UnityEngine;
using System.Collections;

public class BetterDoor : WisdominiObject {

	/* references */

	public GameObject geometry;
	public AudioClip openSound;
	public AudioClip closeSound;
	public LevelControllerScript level;
	//AudioSource aSource;



	/* public properties */

	public float closedAngle;
	public float openAngle;
	public float angleSpeed;
	float angle;
	float targetAngle;
	public DoorAxis axis = DoorAxis.y;

	// Use this for initialization
	new void Start () {

		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
	
		angle = closedAngle;
		bool open = level.retrieveBoolValue ("Is" + this.name + "Open");
		if (open) {
			targetAngle = openAngle;
		} else {
			targetAngle = closedAngle;
		}

		//aSource = this.GetComponent<AudioSource> ();

	}

	public void _wm_open() {

		level.storeBoolValue ("Is" + this.name + "Open", true);
		targetAngle = openAngle;

			if (openSound != null) {
				level.playSound (openSound);
			}


	}

	public void _wm_close() {

		level.storeBoolValue ("Is" + this.name + "Open", false);
		targetAngle = closedAngle;

			if (openSound != null) {
				level.playSound (openSound);
			}

	}
	
	// Update is called once per frame
	new void Update () {

		if (angle < targetAngle) {
			angle += angleSpeed * Time.deltaTime;
			if (angle > targetAngle) {
				angle = targetAngle;

			}
			switch(axis) {
			case DoorAxis.x:
				geometry.transform.Rotate (angleSpeed * Time.deltaTime, 0, 0, Space.Self);
				break;
			case DoorAxis.y:
				geometry.transform.Rotate (0, angleSpeed * Time.deltaTime, 0, Space.Self);
				break;
			case DoorAxis.z:
				geometry.transform.Rotate (0, 0, angleSpeed * Time.deltaTime, Space.Self);
				break;
			}
		}
		if (angle > targetAngle) {
			angle -= angleSpeed * Time.deltaTime;
			if (angle < targetAngle) {
				angle = targetAngle;
			}
			switch(axis) {
			case DoorAxis.x:
				geometry.transform.Rotate (-angleSpeed * Time.deltaTime, 0, 0, Space.Self);
				break;
			case DoorAxis.y:
				geometry.transform.Rotate (0, -angleSpeed * Time.deltaTime, 0, Space.Self);
				break;
			case DoorAxis.z:
				geometry.transform.Rotate (0, 0, -angleSpeed * Time.deltaTime, Space.Self);
				break;
			}
		}
	
	}
}
