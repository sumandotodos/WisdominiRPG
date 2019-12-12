using UnityEngine;
using System.Collections;

public enum DoorAxis { x, y, z };


/*
 * 
 * 
 * BetterDoor2's are self-reentrant.  Use instead of Door's!!!
 * 
 * (Make sure the GameObject has a unique name, in the fashion
 * 	
 * 		Door<Level><DoorDescr> )
 * 
 * 
 */


public class BetterDoor2 : Interactor {

	/* references */

	public AudioClip openSound;
	public AudioClip closeSound;
	public AudioClip cannotOpenSound;
	public LevelControllerScript level;
	//AudioSource aSource;
	public bool interactable;
	string intIcon;
	bool isOpen = false;
	public string permissionVariable;
	public BetterDoor2 secondaryDoor;

	/* public properties */

	public float closedAngle;
	public float openAngle;
	public float angleSpeed;
	public float maxDist;
	public bool calculatePos;
	float angle;
	float targetAngle;
	public DoorAxis axis = DoorAxis.y;

	override public string interactIcon() 
	{
		return intIcon;
	}

	override public void effect() 
	{
		if (isOpen)
			_wm_close ();
		else 
			_wm_open ();
	}

	new void Start () 
	{
		if (interactable)
			intIcon = "Hand";
		else
			intIcon = "";

		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		angle = closedAngle;
		bool open = level.retrieveBoolValue ("Is" + this.name + "Open");
		if (open) {
			angle = targetAngle = openAngle;
			isOpen = true;
		} else {
			angle = targetAngle = closedAngle;
			isOpen = false;
		}
		switch(axis) {
		case DoorAxis.x:
			this.transform.localRotation = Quaternion.Euler (angle, 0, 0);
			break;
		case DoorAxis.y:
			this.transform.localRotation = Quaternion.Euler (0, angle, 0);
			break;
		case DoorAxis.z:
			this.transform.localRotation = Quaternion.Euler (0, 0, angle);
			break;
		}
	}

	public void _wm_immediatelyOpen() {
		level.storeBoolValue ("Is" + this.name + "Open", true);
		isOpen = true;
		angle = targetAngle = openAngle;
		switch(axis) 
		{
		case DoorAxis.x:
			this.transform.localRotation = Quaternion.Euler (angle, 0, 0);
			break;
		case DoorAxis.y:
			this.transform.localRotation = Quaternion.Euler (0, angle, 0);
			break;
		case DoorAxis.z:
			this.transform.localRotation = Quaternion.Euler (0, 0, angle);
			break;
		}
	}

	public void _wm_open() 
	{
		bool canOpen = true;
		if (!permissionVariable.Equals ("")) 
		{
			canOpen = level.retrieveBoolValue (permissionVariable);
		}

		if (!canOpen) 
		{
			if (cannotOpenSound != null) 
			{
				level.playSound (cannotOpenSound);
			}
			return;
		}

		level.storeBoolValue ("Is" + this.name + "Open", true);
		isOpen = true;
		targetAngle = openAngle;

		if (calculatePos && CalculatePlayerPos() || !calculatePos) {
			if (openSound != null) {
				level.playSound (openSound);
			}
		}

		if (secondaryDoor != null)
			secondaryDoor._wm_open ();
	}

	public void _wm_close() 
	{
		level.storeBoolValue ("Is" + this.name + "Open", false);
		isOpen = false;
		targetAngle = closedAngle;

		if (calculatePos && CalculatePlayerPos () || !calculatePos) {
			if (openSound != null) {
				level.playSound (closeSound);
			}
		}

		if (secondaryDoor != null)
			secondaryDoor._wm_close ();
	}

	bool CalculatePlayerPos()
	{
		float dist = Mathf.Abs (Vector3.Distance (GameObject.Find ("Player").transform.position, this.transform.position));

		if (dist > maxDist)
			return false;
		else
			return true;
	}

	new void Update () 
	{
		if (angle < targetAngle) 
		{
			angle += angleSpeed * Time.deltaTime;
			if (angle > targetAngle) 
			{
				angle = targetAngle;

			}
			switch(axis) 
			{
			case DoorAxis.x:
				this.transform.localRotation = Quaternion.Euler (angle, 0, 0);
				break;
			case DoorAxis.y:
				this.transform.localRotation = Quaternion.Euler (0, angle, 0);
				break;
			case DoorAxis.z:
				this.transform.localRotation = Quaternion.Euler (0, 0, angle);
				break;
			}
		}

		if (angle > targetAngle) 
		{
			angle -= angleSpeed * Time.deltaTime;
			if (angle < targetAngle) 
			{
				angle = targetAngle;
			}

			switch(axis) 
			{
			case DoorAxis.x:
				this.transform.localRotation = Quaternion.Euler (angle, 0, 0);
				break;
			case DoorAxis.y:
				this.transform.localRotation = Quaternion.Euler (0, angle, 0);
				break;
			case DoorAxis.z:
				this.transform.localRotation = Quaternion.Euler (0, 0, angle);
				break;
			}
		}
	}
}
