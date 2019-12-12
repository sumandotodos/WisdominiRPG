using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVChannelSwitch : WisdominiObject {

	public bool reentrant = true;

	public string screenId;

	public GameObject[] channel;

	int curChannel;

	LevelControllerScript level;

	void Start() {
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		if (reentrant) {
			curChannel = level.retrieveIntValue ("TVChannel" + screenId);
		}
		_wm_switchChannel (curChannel);
	}
		
	public void _wm_switchChannel(int ch) {

		for (int i = 0; i < channel.Length; ++i) {
			if (i == ch)
				channel [i].SetActive (true);
			else
				channel [i].SetActive (false);
		}

		if (reentrant) {
			if(level == null) level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
			if(level != null) level.storeIntValue ("TVChannel" + screenId, ch);
		}


	}
}
