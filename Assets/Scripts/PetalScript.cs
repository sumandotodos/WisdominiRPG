using UnityEngine;
using System.Collections;

public class PetalScript : MonoBehaviour {

	public float initialPhase;
	public float phaseSpeed;

	float phase;

	// Use this for initialization
	void Start () {
	
		phase = initialPhase;

	}
	
	// Update is called once per frame
	void Update () {

		Vector3 v = new Vector3();
		v.y = phaseSpeed;
		v.x = v.z = 0.0f;
		this.transform.Rotate(v);

	}
}
