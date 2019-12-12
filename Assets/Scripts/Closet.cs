using UnityEngine;
using System.Collections;

public class Closet : Interactor {

	/* references */
	public GameObject leftDoor;
	public GameObject rightDoor;
	public LevelControllerScript level;

	bool isFull;

	public Interactor contents;

	float targetAngle, angle;
	const float angleSpeed = 160.0f;
	const float closedAngle = 0.0f;
	const float openAngle = 135.0f;

	public AudioClip openSound;
	public AudioClip closeSound;

	AudioSource theSource;

	bool isOpen;

	MasterControllerScript mc;
	DataStorage ds;
	LevelControllerScript lvl;

	new void Start () 
	{
		targetAngle = angle = closedAngle;
		isOpen = false;

		if (contents != null)
			isFull = true;

		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = mc.getStorage ();
		lvl = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		if (ds.retrieveBoolValue ("Is" + this.name + lvl.locationName + "Empty")) {
			isFull = false;
		}
	}

	public override string interactIcon() 
	{
		return "Hand";
	}

	new void Update () 
	{
		if (angle < targetAngle) {
			angle += angleSpeed * Time.deltaTime;
			if (angle > targetAngle) {
				angle = targetAngle;
				notifyFinishAction ();
			}
			leftDoor.transform.localRotation = Quaternion.Euler (-90, angle, 0);
			rightDoor.transform.localRotation = Quaternion.Euler (-90, -angle / 1.3f, 0);
		}

		if (angle > targetAngle) {
			angle -= angleSpeed * Time.deltaTime;
			if (angle < targetAngle) {
				angle = targetAngle;
				notifyFinishAction ();
			}
			leftDoor.transform.localRotation = Quaternion.Euler (-90, angle, 0);
			rightDoor.transform.localRotation = Quaternion.Euler (-90, -angle / 1.3f, 0);
		}
	}

	public void openCloset() 
	{
		targetAngle = openAngle;
		if((level!=null) && (openSound!=null))
			level.playSound (openSound);
		isOpen = true;
	}

	public void closeCloset() 
	{
		targetAngle = closedAngle;
		if((level!=null) && (closeSound!=null))
			level.playSound (closeSound);
		isOpen = false;
	}

	public void _wm_doEffect() {
		effect ();
	}

	override public void effect() 
	{
		if (!isOpen) {
			openCloset ();
		} else {
			if (isFull) 
			{
				contents.startProgram (0);//contents.programIndexFromEventName ("onInteract"));
				ds.storeBoolValue ("Is" + this.name + lvl.locationName + "Empty", true);
				isFull = false;
			} else {

				closeCloset ();
			}
		}
	}
}
