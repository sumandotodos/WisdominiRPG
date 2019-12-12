using UnityEngine;
using System.Collections;

public class LightPillar : MonoBehaviour {

	/* references */

	public GameObject[] petal;


	/* properties */

	float[] phaseSpeed;
	float[] angle;

	// Use this for initialization
	void Start () {
	
		phaseSpeed = new float[petal.Length];
		angle = new float[petal.Length];

		for (int i = 0; i < petal.Length; ++i) {

			phaseSpeed [i] = (float)(Random.Range (10, 40)) / 20.0f;
			angle [i] = (float)(Random.Range (0, 70))/10.0f;


		}

	}
	
	// Update is called once per frame
	void Update () {
	
		for (int i = 0; i < petal.Length; ++i) {

			angle [i] += phaseSpeed [i] * Time.deltaTime;
			petal [i].GetComponent<Renderer> ().material.color = new Vector4 (1.0f, 1.0f, 1.0f, (1.0f + Mathf.Cos (angle [i]))/2.0f);

		}

	}
}
