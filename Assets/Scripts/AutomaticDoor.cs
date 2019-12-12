using UnityEngine;
using System.Collections;

public class AutomaticDoor : WisdominiObject {

	/* referencias */

	public WisdominiObject noPermissionProgram_N;
	public FakePanelAxis axis;
	public GameObject leftDoor;
	public GameObject rightDoor;
	public AudioClip openSound;
	public AudioClip closeSound;
	public LevelControllerScript level;
	public string permissionVariable = "";
	public float autoCloseTime = 0.0f;

	public bool reentrant = false;

	/* properties */

	public float closedDisplacement;
	public float openDisplacement;
	public float speed;
	float displacement;
	float targetDisplacement;
	Vector3 leftDoorOriginalPosition;
	Vector3 rightDoorOriginalPosition;
	public float elapsedTime;
	int state = 0; // 0 is closed     1 is open
	public bool autocloseEnable = false;
	public bool reenableAutocloseOnTrigger = true;
	bool autocloseHijack = false;
	/* constantes */


	public void setAutocloseHijack(bool hi) {
		autocloseHijack = hi;
	}

	new void Start () 
	{

		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		leftDoorOriginalPosition = leftDoor.transform.localPosition;
		rightDoorOriginalPosition = rightDoor.transform.localPosition;
		displacement = 0;
		autocloseHijack = false;
		autocloseEnable = false;
		targetDisplacement = 0;

		if (reentrant) {

			bool isOpen = level.retrieveBoolValue (this.name + "Open");
			if (isOpen) {
				openImmediately ();
			}

		}
	
	}

	public void _wm_setAutocloseEnabled(bool en) 
	{
		autocloseEnable = en;
	}
	
	new void Update ()
	{
		if (state == 1) { // door is open

			if (autocloseEnable) {
				if (!autocloseHijack) {
					if (autoCloseTime > 0.0f) { // if we want the door to autoclose after some time...
						elapsedTime += Time.deltaTime;
						if (elapsedTime > autoCloseTime) {
							close ();
							elapsedTime = 0.0f;
						}
					}
				}
			}
		}

		bool change = Utils.updateSoftVariable (ref displacement, targetDisplacement, speed);

		if (change) 
		{
			Vector3 newLeftPosition = Vector3.zero;
			Vector3 newRightPosition = Vector3.zero;

			switch(axis) 
			{
			case FakePanelAxis.x:
				newLeftPosition = leftDoorOriginalPosition + new Vector3 (-displacement, 0, 0);
				newRightPosition = rightDoorOriginalPosition + new Vector3 (displacement, 0, 0);
				break;
			case FakePanelAxis.y:
				newLeftPosition = leftDoorOriginalPosition + new Vector3 (0, -displacement, 0);
				newRightPosition = rightDoorOriginalPosition + new Vector3 (0, displacement, 0);
				break;
			case FakePanelAxis.z:
				newLeftPosition = leftDoorOriginalPosition + new Vector3 (0, 0, -displacement);
				newRightPosition = rightDoorOriginalPosition + new Vector3 (0, 0, displacement);
				break;
			}
			leftDoor.transform.localPosition = newLeftPosition;
			rightDoor.transform.localPosition = newRightPosition;
		}	
	}

	public void _wm_open() 
	{
		open ();
	}

	public void _wm_close() 
	{
		close ();
	}

	public void openImmediately() {
		targetDisplacement = displacement = openDisplacement;
		Vector3 newLeftPosition = Vector3.zero, newRightPosition = Vector3.zero;
		switch(axis) {
		case FakePanelAxis.x:
			newLeftPosition = leftDoorOriginalPosition + new Vector3 (-displacement, 0, 0);
			newRightPosition = rightDoorOriginalPosition + new Vector3 (displacement, 0, 0);
			break;
		case FakePanelAxis.y:
			newLeftPosition = leftDoorOriginalPosition + new Vector3 (0, -displacement, 0);
			newRightPosition = rightDoorOriginalPosition + new Vector3 (0, displacement, 0);
			break;
		case FakePanelAxis.z:
			newLeftPosition = leftDoorOriginalPosition + new Vector3 (0, 0, -displacement);
			newRightPosition = rightDoorOriginalPosition + new Vector3 (0, 0, displacement);
			break;
		}
		leftDoor.transform.localPosition = newLeftPosition;
		rightDoor.transform.localPosition = newRightPosition;
	}

	public void open() {

		targetDisplacement = openDisplacement;
		if ((level != null) && (closeSound != null)) 
		{
			if(state == 0) level.playSound (openSound, this.gameObject);
		}
		state = 1;
		if(reentrant)
		level.storeBoolValue (this.name + "Open", true);
	}

	public bool isClosing() {
		return state == 0;
	}

	public void close() 
	{
		targetDisplacement = closedDisplacement;

		if ((level != null) && (openSound != null)) 
		{
			if(state == 1) level.playSound (closeSound, this.gameObject);
		}
		state = 0;
		if(reentrant)
		level.storeBoolValue (this.name + "Open", false);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Player") 
		{
			if (reenableAutocloseOnTrigger)
				autocloseEnable = true; // Carlos

			if (!permissionVariable.Equals ("")) 
			{
				bool willOpen = false;
				if (!permissionVariable.Equals (""))
					willOpen = level.retrieveBoolValue (permissionVariable);
				else
					willOpen = true;
				if (willOpen) {
					open ();
				} else { // no permission !!
					if (noPermissionProgram_N != null) {
						noPermissionProgram_N.startProgram (0); // igual para better door 2!!!
					}
				}
			} else  { 
				open ();
			}
		} 
		else if (other.tag == "NPCAuto")
		{
			open ();
		}
	}

	void OnTriggerExit(Collider other) 
	{
		if (other.tag == "Player") 
			if (reenableAutocloseOnTrigger)
				autocloseEnable = true;
	}
}
