using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N4AuxCheckHas7Keys : WisdominiObject {

	public string prefix;
	public LevelControllerScript level;
	public int nKeys = 7;

	void Start() {
		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
	}

	public bool _wm_hasAllKeys() {

		bool hasAll = true;
		for (int i = 0; i < nKeys; ++i) {
			if (!level.retrieveBoolValue (prefix + (i + 1))) {
				hasAll = false;
				break;
			}
		}
		return hasAll;

	}
}
