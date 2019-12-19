using UnityEngine;
using System.Collections;

public class GoundLeverScript : WisdominiObject {

	/* references */

	MasterControllerScript mcRef;
	public GameObject lever;
	public AudioClip switchOnSound;
	public AudioClip switchOffSound;
	AudioSource source;
	public string leverName;
	bool done = true;
	public string nameKey;

	/* properties */

	bool isOn;
	public float targetAngle;
	public float angle;

	/* constants */

	const float offAngle = -40.0f;
	const float onAngle = 40.0f;
	const float angleSpeed = 270.0f;

	new void Start () 
	{
		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		done = mcRef.getStorage ().retrieveBoolValue (nameKey);
		isOn = false;
		angle = targetAngle = offAngle;
		source = this.GetComponent<AudioSource> ();
		lever.transform.localRotation = Quaternion.Euler (angle, 0, 0);	

	}

	public void _wa_switchLever(WisdominiObject waiter) 
	{
		waiter.isWaitingForActionToComplete = true;
		waitingRef = waiter;
		switchLever ();
	}

	public void _wm_switchLever() 
	{
		switchLever ();
	}

	public void switchLever() 
	{
		//done = true;
		if(isOn)
		{
			targetAngle = offAngle;
		}
		else 
		{
			targetAngle = onAngle;
		}
	}

	public void _wm_InstaOpen()
	{
		InstaOpen ();
	}

	public void InstaOpen()
	{
		angle = targetAngle;
		lever.transform.localRotation = Quaternion.Euler (angle, 0, 0);
		//done = false;
	}
	
	new void Update () 
	{
		if (angle < targetAngle) 
		{
			angle += angleSpeed * Time.deltaTime;
			if (angle > targetAngle) {
				angle = targetAngle;
				isOn = true;
				if (switchOnSound!=null) 
				{
					source.PlayOneShot (switchOnSound);
				}
				notifyFinishAction ();
			}
			lever.transform.localRotation = Quaternion.Euler (angle, 0, 0);
		}

		if (angle > targetAngle) 
		{
			angle -= angleSpeed * Time.deltaTime;
			if (angle < targetAngle) {
				angle = targetAngle;
				isOn = false;
				if (switchOffSound!=null) {
					source.PlayOneShot (switchOffSound);
				}
				notifyFinishAction ();
			}
			lever.transform.localRotation = Quaternion.Euler (angle, 0, 0);
		}	
	}
}
