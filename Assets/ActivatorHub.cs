using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ActivatorChannel {

	public GameObject theObject;
	public bool startActive;

}

public class ActivatorHub : WisdominiObject {

	LevelControllerScript level;

	public bool reentrant = true;

	public ActivatorChannel[] channels;

	// Use this for initialization
	void Start () {
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		foreach (ActivatorChannel c in channels) {
			int status = level.retrieveIntValue (c.theObject.name + "isActive");
			if (status == 0) {
				c.theObject.SetActive (c.startActive);
			} else if (status == 1) {
				c.theObject.SetActive (true);
			} else
				c.theObject.SetActive (false);
		}
	}

	public void _wm_setActiveByIndex(int channel, bool en) {

		if (reentrant) {
			if (en)
				level.storeIntValue (channels [channel].theObject.name + "isActive", 1);
			else
				level.storeIntValue (channels [channel].theObject.name + "isActive", -1);
		}
		channels [channel].theObject.SetActive (en);

	}

	public void _wm_setActiveByString(string name, bool en) {
		int theIndex = -1;
		for (int i = 0; i < channels.Length; ++i) {
			if (channels [i].theObject.name.Equals (name)) {
				theIndex = i;
				break;
			}
		}
		if (theIndex != -1)
			_wm_setActiveByIndex (theIndex, en);

	}

}
