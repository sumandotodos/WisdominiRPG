using UnityEngine;
using System.Collections;

public class DuendeController : WisdominiObject {

	MasterControllerScript mc;

	public LevelControllerScript level;
	public string solvePrefix;
	public CharacterGenerator[] duendeCoco;
	public Vector3[] solvedPosition;
	public int selected = -1; // despublicar
	string retrieveVariable = "N4DuendeElegido";

	public Vector3 smallOffset;


	// Use this for initialization
	new void Start () {

		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		for (int i = 0; i < duendeCoco.Length; ++i) {
			if (duendeCoco [i] != null) { // WARNING CAMBIO EMILIO
				bool clear = mc.getStorage ().retrieveBoolValue (solvePrefix + (i + 1) + "Clear");
				if (clear)
					solveDuende (i);
			}
		}
		bool coco = level.retrieveBoolValue ("N4DuendeIsCocoroloco");
		if (!coco)
			retrieveVariable = "N4DuendeMatElegido";
		refresh ();




	}
	
	public void refresh() {


		//if (coco) {
			selected = level.retrieveIntValue (retrieveVariable);
			for (int i = 0; i < duendeCoco.Length; ++i) {
				if (duendeCoco [i] != null) {
					if ((selected - 1) == i)
						duendeCoco [i].interactEnabled = false;
				}
			}
		//} else {

		//}

	}

	public void _wm_refresh() {
		refresh ();
	}

	public void _wm_walkToRelative(float x, float z) {
		if (selected < 1)
			return;
		duendeCoco [selected - 1].stopFollowingPlayer ();
		duendeCoco [selected - 1].refreshSpawnPosition ();
		duendeCoco [selected - 1].setAutopilotAbsolute (false);
		duendeCoco [selected - 1].autopilotTo (x, z);
	}

	public void _wm_walkTo(float x, float z) {
		if (selected < 1)
			return;
		duendeCoco [selected - 1].stopFollowingPlayer ();
		duendeCoco [selected-1].setAutopilotAbsolute (true);
		duendeCoco [selected-1].autopilotTo (x, z);
	}

	public new void _wm_hide() {
		if (selected == -1)
			return;
		duendeCoco [selected-1]._wm_hide ();
		level.storeIntValue (retrieveVariable, 0);
	}

	public void _wm_stopFollowingPlayer() {
		if (selected < 1)
			return;
		duendeCoco [selected-1]._wm_stopFollowingPlayer ();
	}

	public void _wm_releaseDuende() {
		level.storeIntValue (retrieveVariable, 0);
	}

	public void solveDuende(int n) {


			duendeCoco [n].transform.position = solvedPosition [n] + smallOffset;
			duendeCoco [n]._wm_startEventByName ("customEvent1");


	}

	public void _wm_runEvent(string eventname) {
		duendeCoco [selected - 1]._wm_startEventByName (eventname);
	}
}
