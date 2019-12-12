using UnityEngine;
using System.Collections;

public class AllWavesDoneAux : WisdominiObject {

	public LevelControllerScript level;
	public ShadowSpawnController shadowSpawnController;

	void Start() {
		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
	}

	public bool _wm_allWavesDone() 
	{
		string lvl = level.locationName.Substring (0, 6);
		int waveNumber = level.retrieveIntValue (lvl + "ShadowWaveNumber");
		if (waveNumber > shadowSpawnController.waveType.Length)
			return true;
		return false;
	}
}
