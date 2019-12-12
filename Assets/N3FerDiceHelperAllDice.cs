using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N3FerDiceHelperAllDice : MonoBehaviour {

	LevelControllerScript level;

	// Use this for initialization
	void Start () {
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		bool d1 = level.retrieveBoolValue ("HasFerfufloDice1");
		bool d2 = level.retrieveBoolValue ("HasFerfufloDice2");
		bool d3 = level.retrieveBoolValue ("HasFerfufloDice3");
		if (d1 && d2 && d3)
			level.storeBoolValue ("HasAllFerfufloDice", true);
	}


}
