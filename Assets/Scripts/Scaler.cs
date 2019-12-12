using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour {

	public float scaleSpeed = 0.001f;

	float scale = 16.0f;
	float initialScale;

	// Use this for initialization
	void Start () {
	
		initialScale = scale = this.transform.localScale.x;

	}
	
	// Update is called once per frame
	void Update () {

		scale += scaleSpeed * Time.deltaTime;

		if (scale > 1.5f * initialScale)
			scale = 1.5f * initialScale;

		this.transform.localScale = new Vector3 (scale, scale, scale);
	
	}
}
