using UnityEngine;
using System.Collections;

public abstract class Interactor : WisdominiObject {

	public bool interactEnabled = true;

	abstract public void effect ();

	abstract public string interactIcon ();

	public void _wm_enable() {

		interactEnabled = true;
		//PlayerScript player = GameObject.Find ("Player").GetComponent<PlayerScript> ();
		//player.OnTriggerEnter (this.GetComponent<Collider>());

	}

	public void _wm_disable() {

		interactEnabled = false;
		PlayerScript player = GameObject.Find ("Player").GetComponent<PlayerScript> ();
		player.OnTriggerExit (this.GetComponent<Collider>());

	}
}
