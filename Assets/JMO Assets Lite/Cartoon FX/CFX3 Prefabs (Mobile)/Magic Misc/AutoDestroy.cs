using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

	public float delay;
	float elapsedTime;

	// Use this for initialization
	void Start () {

		elapsedTime = 0.0f;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		elapsedTime += Time.deltaTime;
		if (elapsedTime > delay) {
			Destroy (this.gameObject);
		}

	}
}
