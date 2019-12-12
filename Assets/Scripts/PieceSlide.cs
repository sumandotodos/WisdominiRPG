using UnityEngine;
using System.Collections;

public class PieceSlide : MonoBehaviour {

	public Vector3 displacement;
	public float time;
	public Vector3 chorro;

	Vector3 original;

	float t;

	bool locked;

	public void go() {

		locked = false;

	}

	// Use this for initialization
	void Start () {
	
		original = this.transform.localPosition;
		locked = true;
		t = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {
	
		if (locked)
			return;

		t += (1.0f / time) * Time.deltaTime;
		if (t > 0.8f) {
			t = 0.8f;
		}
		Vector3 pos = Vector3.Lerp (original + displacement, original, linToSoft (t));
		chorro = pos;
		this.transform.localPosition = pos;

	}

	float linToSoft(float l) {

		return Mathf.Exp (-(3.0f * l));

	}
}
