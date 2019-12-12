using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogSpriter : MonoBehaviour {

	SpriteRenderer[] srs;
	public float distMin;
	public float distMax;
	float currentDist;
	GameObject target;
	float a;

	void Update () 
	{
		currentDist = Mathf.Abs (Vector3.Distance (this.gameObject.transform.position, target.transform.position));

		if (currentDist < distMin) {
			a = 1;
		} else if (currentDist > distMax) {
			a = 0;
		} else {
			a = 1 - ((currentDist - distMin) / (distMax - distMin));
		}

		foreach (SpriteRenderer sr in srs) 
		{
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, a);

			if (a <= 0) {
				sr.enabled = false;
			} else {
				sr.enabled = true;
			}
		}
	}

	void Start()
	{
		target = GameObject.Find ("CameraLerp");
		srs = GetComponentsInChildren<SpriteRenderer> ();
	}

}
