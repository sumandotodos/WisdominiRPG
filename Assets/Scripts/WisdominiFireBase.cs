using UnityEngine;
using System.Collections;

public class WisdominiFireBase : WisdominiObject {

	MasterControllerScript masterController;

	DigitalRuby.PyroParticles.FireConstantBaseScript fire;

	// Use this for initialization
	new void Start () {

		fire = this.GetComponent<DigitalRuby.PyroParticles.FireConstantBaseScript> ();
		masterController = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		if(masterController.getStorage ().retrieveBoolValue ("HasFireBeenExtinguised")) {
			fire.Stop ();
		}
	
	}
	


	public void _wm_stop() {

		masterController.getStorage ().storeBoolValue ("HasFireBeenExtinguised", true);
		fire.Stop ();

	}

}
