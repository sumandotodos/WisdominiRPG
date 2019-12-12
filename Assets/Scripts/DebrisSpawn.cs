using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawn : WisdominiObject {

	public GameObject[] thingToSpawn; // must have a RigidBody attached

	public float density;
	public float speed;
	public float duration;

	float spawnedItems;
	float totalTime;

	private void spawnSingle() {
		
		GameObject t;

		int which = Random.Range (0, thingToSpawn.Length);

		t = thingToSpawn [which];

		GameObject newGO = (GameObject)Instantiate (t);
		Vector3 minBounds = this.GetComponent<BoxCollider> ().bounds.min;
		Vector3 maxBounds = this.GetComponent<BoxCollider> ().bounds.max;
		Vector3 newPos = new Vector3 (Random.Range (minBounds.x, maxBounds.x), 
			                 Random.Range (minBounds.y, maxBounds.y), 
				             Random.Range (minBounds.z, maxBounds.z));
		newGO.transform.position = newPos;
		newGO.GetComponent<Rigidbody> ().AddExplosionForce (speed, this.transform.position, 20.0f, 0.0f, ForceMode.Impulse);
	}

	// Use this for initialization
	new void Start () {
		state = 0;
	}

	int state = 0;

	public void _wm_spawnDebris() {
		spawnedItems = 0.0f;
		totalTime = 0.0f;
		state = 1;

	}

	// Update is called once per frame
	new void Update () {
		if (state == 0) {

		}
		if (state == 1) {
			spawnedItems +=  (density * Time.deltaTime);
			totalTime += Time.deltaTime;
			while (spawnedItems > 1.0f) {
				spawnSingle ();
				spawnedItems -= 1.0f;
			}
			if (totalTime > duration)
				state = 0;
		}

	}
}
