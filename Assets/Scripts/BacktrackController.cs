using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacktrackController : WisdominiObject {

	public LevelControllerScript level;
	public SetCameraBacktracker[] backtrack;
	public AutomaticDoor[] autoDoor;

	public void setEnabled(int i, bool en) {
		backtrack [i].enabled = en;
	}
		
	public void _wm_setEnabled(int i, bool en) {
		setEnabled (i, en);
	}

	void Start() {
		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		for (int i = 0; i < autoDoor.Length; ++i) {
			bool isOpen = level.retrieveBoolValue (autoDoor[i].gameObject.name + "Open");

			if (backtrack [i] != null)
				backtrack [i].enabled = isOpen;
			
		}
	}

}
