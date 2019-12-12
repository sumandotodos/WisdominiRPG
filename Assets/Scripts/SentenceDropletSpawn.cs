using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceDropletSpawn : MonoBehaviour {

	public GameObject spawnUnen;
	public GameObject spawnSeparan;

	public GameObject dropletPrefab;

	public FGTable frasesQueNosUnen;
	public FGTable frasesQueNosSeparan;

	public float timeToSpawnUnen;
	public float timeToSpawnSeparan;

	const float minTimeToSpawn = 1.0f;
	const float maxTimeToSpawn = 2.0f;

	public float initialZSpeed;
	public float ZAccel;
	public float YAccel;
	public float killZSpeed;

	int state = 1;

	// Use this for initialization
	void Start () {
		float time = Random.Range (minTimeToSpawn, maxTimeToSpawn);
		timeToSpawnUnen = time;
		time = Random.Range (minTimeToSpawn, maxTimeToSpawn);
		timeToSpawnSeparan = time;
	}
	
	// Update is called once per frame
	void Update () {

		// modelo de lonchas
		if (state == 0) {
			// idle
		}

		if (state == 1) {
			timeToSpawnUnen -= Time.deltaTime;
			if (timeToSpawnUnen <= 0.0f) {
				spawn (0);
			}
			timeToSpawnSeparan -= Time.deltaTime;
			if (timeToSpawnSeparan <= 0.0f) {
				spawn (1);
			}
		}

	}

	public void spawn(int type) {

		GameObject newGO = (GameObject)Instantiate (dropletPrefab);

		newGO.GetComponent<SentenceDroplet> ().YAccel = YAccel;
		newGO.GetComponent<SentenceDroplet> ().ZAccel = YAccel;
		newGO.GetComponent<SentenceDroplet> ().ZSpeed = initialZSpeed;
		newGO.GetComponent<SentenceDroplet> ().killDeltaZAccel = killZSpeed;

		if (type == 0) {
			int r = Random.Range(0, frasesQueNosUnen.nRows ());
			string frase = (string)frasesQueNosUnen.getElement (0, r);
			frase = StringUtils.chopLines (frase, 10);
			newGO.transform.position = spawnUnen.transform.position;
			newGO.GetComponent<SentenceDroplet> ().setText (frase);
			newGO.GetComponent<SentenceDroplet> ().Start ();
			timeToSpawnUnen = Random.Range (minTimeToSpawn, maxTimeToSpawn);
		}

		if (type == 1) {
			int r = Random.Range(0, frasesQueNosSeparan.nRows ());
			string frase = (string)frasesQueNosSeparan.getElement (0, r);
			frase = StringUtils.chopLines (frase, 10);
			newGO.transform.position = spawnSeparan.transform.position;
			newGO.GetComponent<SentenceDroplet> ().setText (frase);
			newGO.GetComponent<SentenceDroplet> ().Start ();
			timeToSpawnSeparan = Random.Range (minTimeToSpawn, maxTimeToSpawn);
		}

	}
}
