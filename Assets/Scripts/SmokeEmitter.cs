using UnityEngine;
using System.Collections;

public class SmokeEmitter : MonoBehaviour {

	public float density;
	public Vector3 wind;
	public Color tint;
	public float turbulenceStrength;
	public float turbulenceFrequency;
	public float timeToLive;

	public GameObject SmokePuffPrefab;

	float timeToNextSpawn;

	float elapsedTime;

	// Use this for initialization
	void Start () {
	
		timeToNextSpawn = 1.0f/density;

	}
	
	// Update is called once per frame
	void Update () {

		elapsedTime += Time.deltaTime;

		if (elapsedTime > timeToNextSpawn) {

			elapsedTime = 0.0f; /*
			GameObject newGO = new GameObject ();
			SmokePuff smokeRef;
			smokeRef = newGO.AddComponent<SmokePuff> ();
			smokeRef.turbulenceStregth = turbulenceStrength;
			smokeRef.turbulenceFrequency = turbulenceFrequency;
			smokeRef.wind = wind;
			smokeRef.tintColor = tint;
			smokeRef.initialPosition = this.transform.position;
			*/
			SmokePuff smokeRef = Instantiate (SmokePuffPrefab).GetComponent<SmokePuff>();
			smokeRef.turbulenceStregth = turbulenceStrength;
			smokeRef.turbulenceFrequency = turbulenceFrequency;
			smokeRef.wind = wind;
			smokeRef.tintColor = tint;
			smokeRef.initialPosition = this.transform.position;
			smokeRef.timeToLive = timeToLive;


		}
	
	}
}
