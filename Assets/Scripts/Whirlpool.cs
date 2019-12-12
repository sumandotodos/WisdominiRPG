using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

enum WhirlpoolState { disabled, enabled, finishing, finished };

public class Whirlpool : WisdominiObject {

	/* references */

	public LevelControllerScript level;
	public GameObject spriteQuad;
	public string reentryVariable;

	/* constants */

	const float maxSpinSpeed = 12.0f;
	const float minSpinSpeed = 2.0f;
	const float maxAccel = 20.0f;

	/* properties */

	float angle;
	float spinSpeed;
	float spinAccel;
	public string heavenVariableName = "";
	WhirlpoolState state;
	public bool requireAlphabet = true;

	public bool autoenable = true;

	/* methods */

	new void Start () 
	{
		state = WhirlpoolState.disabled;

		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		if (!reentryVariable.Equals ("")) {
			bool en = level.retrieveBoolValue (reentryVariable);
			if (en) {
				enable ();
			}
		}

		if(autoenable) enable ();
	}
	
	new void Update () 
	{
		angle += spinSpeed;
		spinSpeed += spinAccel;
		if (spinSpeed > maxSpinSpeed) 
		{
			spinSpeed = maxSpinSpeed;
		}
		if (spinSpeed < 0.0f) 
		{
			spinSpeed = 0.0f;
		}
		spriteQuad.transform.rotation = Quaternion.Euler (90, angle, 0);

		if (state == WhirlpoolState.finishing) 
		{
			if (isWaitingForActionToComplete)
				return;

			level.fadeout (this);
			isWaitingForActionToComplete = true;
			state = WhirlpoolState.finished;
		}

		if (state == WhirlpoolState.finished) 
		{
			if (isWaitingForActionToComplete)
				return;
			//SceneManager.LoadScene ( + level.lowerFloorName);
		}
	}

	public void _wm_enable() 
	{
		enable();
	}

	public void enable()
	{
		state = WhirlpoolState.enabled;
		spinSpeed = minSpinSpeed;
		spinAccel = 0.0f;
		level.storeBoolValue (reentryVariable, true);
	}

	public void activate() 
	{
		spinAccel = maxAccel;
	}

	public void disable() 
	{
		state = WhirlpoolState.disabled;
		spinAccel = -maxAccel;
	}

	public void OnTriggerEnter(Collider other) {

		if (state == WhirlpoolState.disabled)
			return;

		if (requireAlphabet && (!level.retrieveBoolValue ("HasAlphabet"))) {
			return;
		}

		if(other.tag == "Player")
		{
			other.gameObject.GetComponent<PlayerScript> ().land ();
			other.gameObject.GetComponent<PlayerScript> ().spin(this);
			this.isWaitingForActionToComplete = true;
			state = WhirlpoolState.finishing;
			if (!heavenVariableName.Equals ("")) {
				// reset reentry related global variables
				level.storeIntValue (heavenVariableName, level.retrieveIntValue (heavenVariableName) + 1);
				level.storeBoolValue ("Droplets", false);
				level.storeIntValue("Droplets", 0);
				for (int i = 0; i <= 6; ++i) {
					level.storeBoolValue ("PickedUpDroplet" + i, false);
				}
				level.storeBoolValue ("HeaveSpeak", false);
				level.storeBoolValue ("SignGameReentry", false);
			}
		}
	}
}
