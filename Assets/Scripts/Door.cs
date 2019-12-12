using UnityEngine;
using System.Collections;

enum DoorState { Closed, Closing, Open, Opening };

public class Door : Interactor {

	/* references */

	MasterControllerScript masterController;
	public ObjectGeneratorScript interactProgram = null;
	public AudioClip openSound;
	public AudioClip closeSound;
	new AudioSource audio;
	LevelControllerScript level;

	/* public properties */
	public bool slide = false;


	DoorState state;

	const float angleSpeed = 240.0f;

	float angle;

	new void Start () 
	{
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		state = DoorState.Closed;
		isWaitingForActionToComplete = false;
		angle = -180.0f;
		audio = this.GetComponent<AudioSource>();
		masterController = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		if (masterController.getStorage ().retrieveBoolValue ("IsDoor" + this.name + "Open")) {
			_wm_immediateOpen ();
		}
	}
	
	new void Update () 
	{
		if (isWaitingForActionToComplete)
			return;

		switch (state) 
		{
		case DoorState.Closed:
			break;

		case DoorState.Opening:
			angle -= angleSpeed * Time.deltaTime;
			if (angle <= -270.0f) {
				angle = -270.0f;
				state = DoorState.Open;
			}
			this.transform.rotation = Quaternion.Euler (0, angle, 0);
			break;

		case DoorState.Open:
			break;

		case DoorState.Closing:
			angle += angleSpeed * Time.deltaTime;
			if (angle >= -180.0f) {
				angle = -180.0f;
				state = DoorState.Closed;
			}
			this.transform.rotation = Quaternion.Euler (0, angle, 0);
			break;
		}	
	}

	public override void effect() {

		if (interactProgram != null)
		{
			int prgindex = interactProgram.programIndexFromEventName ("onInteract");
			interactProgram.startProgram (prgindex);

		} else { // default behaviour

			if (state == DoorState.Closed) {
				_wm_open ();
			}
			//this.gameObject.GetComponent<BoxCollider> ().enabled = false;
			if (state == DoorState.Open)
				_wm_close ();
		}
	}

	public void _wm_immediateOpen() 
	{
		state = DoorState.Open;
		angle = -270.0f;
		this.transform.rotation = Quaternion.Euler (0, angle, 0);
	}

	public void _wm_immediateClose()
	{
		state = DoorState.Closed;
		angle = -180.0f;
		this.transform.rotation = Quaternion.Euler (0, angle, 0);
	}

	public void _wm_open()
	{
		if (state == DoorState.Closed)
			state = DoorState.Opening;

		if((openSound != null) && (level!=null)){
			level.playSound (openSound);
		}
		masterController.getStorage ().storeBoolValue ("IsDoor" + this.name + "Open", true);
	}

	public void _wm_close()
	{
		if (state == DoorState.Open)
			state = DoorState.Closing;

		if ((closeSound != null) && (level != null)) {
			level.playSound (closeSound);
		}
		masterController.getStorage ().storeBoolValue ("IsDoor" + this.name + "Open", false);
	}

	public void _wm_switch() 
	{
		if (state == DoorState.Closed)
			_wm_open ();
		else
			_wm_close ();
	}

	public void _wm_unlinkInteractProgram()
	{
		interactProgram = null;
	}

	public override string interactIcon() 
	{
		return "Hand";
	}
}
