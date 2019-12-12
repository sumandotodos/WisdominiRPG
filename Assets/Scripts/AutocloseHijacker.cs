using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutocloseHijacker : WisdominiObject {

	public WisdominiObject onLiftProgram;
	public AutomaticDoor[] autoDoor;
	public int genteInside;
	public int elementsNeededForLift = 2;
	public int extraNeed = 0;
	MasterControllerScript mc;

	bool playerInside, characterInside, cameraInside;

	// Use this for initialization
	void Start () {
		genteInside = 0;
		extraNeed = 0;
		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		playerInside = characterInside = cameraInside = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (mc.getStorage ().retrieveStringValue ("FollowingChar").Equals (""))
			extraNeed = 0;
		else
			extraNeed = 1;
		if ((genteInside > 0) && (genteInside < (elementsNeededForLift + extraNeed))) {
			for (int i = 0; i < autoDoor.Length; ++i) {
				autoDoor [i].setAutocloseHijack (true);
//				if (autoDoor [i].isClosing ())
//					autoDoor [i].open ();
			}
		} else {
			for (int i = 0; i < autoDoor.Length; ++i) {
				autoDoor [i].setAutocloseHijack (false);
			}
		}
	}

	void OnTriggerEnter(Collider other) {

		Debug.Log ("<color=blue>AHJ Entering: " + other.name + " with tag: " + other.tag + "</color>");
		if((other.tag != "Player") && (other.tag != "Character") && (other.tag != "PhysicalCamera")) return;

		int genteAntes = genteInside;
		if (other.tag == "Player") {
			if (!playerInside) {
				playerInside = true;
				++genteInside;
				Debug.Log ("<color=blue>Increment by tag 'Player'</color>");
			}
		}
		if (other.tag == "Character") {
			if (!characterInside) {
				characterInside = true;
				++genteInside;
				Debug.Log ("<color=blue>Increment by tag 'Character'</color>");
			}
		}
		if (other.tag == "PhysicalCamera") {
			if (!cameraInside) {
				cameraInside = true;
				++genteInside;
				Debug.Log ("<color=blue>Increment by tag 'MainCamera'</color>");
			}
		}
		if(genteInside == (elementsNeededForLift + extraNeed)) {
			onLiftProgram._wm_startEventByName("customEvent1");
		}
		if (genteInside != genteAntes) {
			Debug.Log ("---->>> Enter: " + other.gameObject.name);
		}
	}

	void OnTriggerExit(Collider other) {

		Debug.Log ("<color=yellow>AHJ Exitting: " + other.name + " with tag: " + other.tag + "</color>");
		if((other.tag != "Player") && (other.tag != "Character") && (other.tag != "PhysicalCamera")) return;

		int genteAntes = genteInside;
		if (other.tag == "Player") {
			if (playerInside) {
				playerInside = false;
				--genteInside;
				Debug.Log ("<color=yellow>Decrement by tag 'Player'</color>");
			}
		}
		if (other.tag == "PhysicalCamera") {
			if (cameraInside) {
				cameraInside = false;
				--genteInside;
				Debug.Log ("<color=yellow>Decrement by tag 'MainCamera'</color>");
			}
		}
		if (other.tag == "Character") {
			if (characterInside) {
				characterInside = false;
				--genteInside;
				Debug.Log ("<color=yellow>Decrement by tag 'Character'</color>");
			}
		}
		if (genteInside != genteAntes) {
			Debug.Log ("<<<---- Exit: " + other.gameObject.name);
		}
	}
}
