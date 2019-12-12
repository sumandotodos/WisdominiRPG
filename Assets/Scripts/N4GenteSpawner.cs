using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N4GenteSpawner : WisdominiObject {

	public LevelControllerScript levelController;

	public string StorageVariable;

	public GameObject[] thingToSpawn;

	public float minInterval;
	public float maxInterval;

	float timeRemaining;

	bool singleShot = false;

	public bool going = true;

	// Use this for initialization
	void Start () {
		if (levelController == null)
			levelController = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		going = false;
		if (levelController.retrieveBoolValue (StorageVariable)) {
			going = true;
		}
		timeRemaining = -1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!going)
			return;

		if (timeRemaining < 0.0f) {
			int r = Random.Range (0, thingToSpawn.Length);
			GameObject newGO = (GameObject)Instantiate (thingToSpawn [r]);
			newGO.transform.position = this.transform.position;
			timeRemaining = Random.Range (minInterval, maxInterval);
			if (singleShot) {
				singleShot = false;
				going = false;
			}
		} else
			timeRemaining -= Time.deltaTime;

	}

	public void _wm_spawn() {
		singleShot = true;
		going = true;
		timeRemaining = -1.0f;
	}
}
