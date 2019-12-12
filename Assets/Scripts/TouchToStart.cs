using UnityEngine;
using System.Collections;

public class TouchToStart : MonoBehaviour {

	float elapsedTime;
	float scale;
	int state;
	public bool wait = true;

	const float scaleSpeed = 0.15f;

	// Use this for initialization
	void Start () {
	
		elapsedTime = 0.0f;
		state = 0;
		scale = 0.0f;
		this.transform.localScale = Vector3.zero;

	}

	public void reset() {

		state = 0;
		elapsedTime = 0.0f;
		scale = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {

		elapsedTime += Time.deltaTime;

		if (state == 0) {

			this.transform.localScale = new Vector3 (scale, scale, scale);
			scale += scaleSpeed * Time.deltaTime * 10.0f;

			if (scale > 0.7)
				++state;

			elapsedTime = 0.0f;

		}

		if (state == 1) {

			wait = false;
			this.transform.localScale = new Vector3 (scale, scale, scale);
			scale -= scaleSpeed * Time.deltaTime;

			if (scale < 0.45)
				++state;

			elapsedTime = 0.0f;

		}

		if (state == 2) {

			scale = 0.5f - 0.05f * Mathf.Cos (elapsedTime * 2.0f);
			this.transform.localScale = new Vector3 (scale, scale, scale);

		}
	
	}
}
