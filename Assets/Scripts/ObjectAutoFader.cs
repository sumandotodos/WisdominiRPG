using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OpacityMode { transparent, opaque };

[System.Serializable]
public struct objectWithVariable {

	public WisdominiObject obj;
	public string StorageVariable;

}

public class ObjectAutoFader : MonoBehaviour {

	public objectWithVariable[] objVars;

	public LevelControllerScript levelController;

	public OpacityMode opacityMode;

	// Use this for initialization
	void Start () {
		if (levelController == null) {
			levelController = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		}
		for (int i = 0; i < objVars.Length; ++i) {
			if (!levelController.retrieveBoolValue (objVars [i].StorageVariable)) {
				if (objVars [i].obj != null) {
					if (opacityMode == OpacityMode.opaque)
						objVars [i].obj.setOpacity (1.0f);
					else
						objVars [i].obj.setOpacity (0.0f);
				}
			}
		}
	}
	

}
