using UnityEngine;
using System.Collections;

public class Fence : WisdominiObject {

	/* references */
	MasterControllerScript masterController;

	DoorState state;

	const float angleSpeed = 230.0f;

	/* public properties */

	public float closedAngle;
	public float openAngle;
	float targetAngle;

	float angle;
	public string nameKey;

	new void Start () 
	{
		state = DoorState.Closed;
		isWaitingForActionToComplete = false;
		angle = closedAngle;
		targetAngle = closedAngle;
		masterController = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();

		if (masterController.getStorage ().retrieveBoolValue (nameKey)) 
		{
			state = DoorState.Open;
			angle = openAngle;
		}
		this.transform.localRotation = Quaternion.Euler (0, angle, 0);
	}

	new void Update () 
	{
		if (state == DoorState.Opening || state == DoorState.Closing) 
		{
			if (angle < targetAngle) 
			{
				angle += angleSpeed * Time.deltaTime;

				if (angle > targetAngle) 
				{
					angle = targetAngle;
					if (state == DoorState.Opening)
						state = DoorState.Open;
					else if (state == DoorState.Closing)
						state = DoorState.Closed;
				}
				this.transform.localRotation = Quaternion.Euler (0, angle, 0);
			}

			if (angle > targetAngle) 
			{
				angle -= angleSpeed * Time.deltaTime;

				if (angle < targetAngle) 
				{
					angle = targetAngle;
					if (state == DoorState.Opening)
						state = DoorState.Open;
					else if (state == DoorState.Closing)
						state = DoorState.Closed;
				}
				this.transform.localRotation = Quaternion.Euler (0, angle, 0);
			}
		}
	}

	public void _wm_open() 
	{
		if (state == DoorState.Closed) 
		{
			state = DoorState.Opening;
			targetAngle = openAngle;
		}
	}

	public void _wm_close() 
	{
		if (state == DoorState.Open) 
		{
			state = DoorState.Closing;
			targetAngle = closedAngle;
		}
	}

	public void _wm_switch() 
	{
		if (state == DoorState.Closed)
			_wm_open ();
		else
			_wm_close ();
	}
}
