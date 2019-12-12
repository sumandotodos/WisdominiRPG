using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMontanaMago : WisdominiObject {

	string destination;
	public int numMago;

	LevelControllerScript level;
	VignetteScript vignette;
	int state = 0;

	new void Start ()
	{
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		vignette = GameObject.FindGameObjectWithTag ("HUD").GetComponentInChildren<VignetteScript> ();
		destination = "Level2Plane0_interior_MontanasMagos";
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
			level.loadScene (destination);
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag != "Player")
			return;

		level.storeIntValue ("MagoElegido", numMago);
		this.isWaitingForActionToComplete = true;
		if(vignette != null) vignette._wa_close (this);
		state = 1;
	}
}
