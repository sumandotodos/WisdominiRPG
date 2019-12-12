using UnityEngine;
using System.Collections;

public class UIImageScaler : MonoBehaviour {

	float scale;
	Vector3 initialScaleVec;

	public bool linear;

	public float time;

	public float finalScale;
	public float initialScale;
	float t;

	bool locked;

	// Use this for initialization
	void Start () {

		initialScaleVec = this.transform.localScale;
		scale = 1.0f;
		t = 0.0f;
		locked = true;
	
	}

	public void go() {

		locked = false;

	}
	
	// Update is called once per frame
	void Update () {
	
		if (locked)
			return;

		t += (1.0f / time) * Time.deltaTime;
		if (t > 1.0f)
			t = 1.0f;

		float param;
		if (linear)
			param = t;
		else
			param = 1.0f - linToSoft (t);
		float s = Mathf.Lerp (initialScale, finalScale, param);

		this.transform.localScale = initialScaleVec * s;

	}

	float linToSoft(float l) {

		return Mathf.Exp(-3.0f * l);

	}
}
