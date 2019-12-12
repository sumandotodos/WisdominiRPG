using UnityEngine;
using System.Collections;

/*
 * 
 *  Specific purpose portal, no need to create
 *   lots and lots of WisdominiObjects
 * 
 */


public class BetterPortal : WisdominiObject {

	LevelControllerScript level;
	public string destination;
	public VignetteScript vignette;
	int state = 0;

	public Vector3 forceSpawnCoordinates;

	new void Start ()
	{
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		vignette = GameObject.FindGameObjectWithTag ("HUD").GetComponentInChildren<VignetteScript> ();
	}
	
	new void Update () {
	
		if (state == 0) 
		{
			return;
		}

		if (state == 1) 
		{
			if (isWaitingForActionToComplete)
				return;
			level.storeStringValue ("GroundType", "");
			level.loadScene (destination);
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag != "Player")
			return;

		this.isWaitingForActionToComplete = true;
		level.vignette._wa_close (this);
		state = 1;

		if(forceSpawnCoordinates.sqrMagnitude > 0) {
			level.storeFloatValue ("Coords" + destination + "X", forceSpawnCoordinates.x);
			level.storeFloatValue ("Coords" + destination + "Y", forceSpawnCoordinates.y);
			level.storeFloatValue ("Coords" + destination + "Z", forceSpawnCoordinates.z);
		}
		int or = other.GetComponent<PlayerScript> ().orientation ();
		level.storeIntValue ("Orientation", or);
	}
}