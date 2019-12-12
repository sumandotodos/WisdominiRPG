using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N4EliminadorDeBarreras : WisdominiObject {

	public GameObject[] barreritas;
	public string baseVariableName;
	LevelControllerScript level;

	public void _wm_deleteBarrier(int i) {
		Destroy (barreritas [i]);
	}

	// Use this for initialization
	new void Start () {
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		for (int i = 1; i <= barreritas.Length; ++i) { // reentrancy
			string variable = baseVariableName.Replace ("<1>", "" + i);
			if (level.retrieveBoolValue (variable))
				_wm_deleteBarrier (i - 1);
		}
	}
	

}
