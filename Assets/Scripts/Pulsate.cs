using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pulsate : MonoBehaviour {

	bool isEnabled;

	public float frequency1 = 20.0f;
	public float frequency2 = 27.3f;
	public float minOpacity = 0.6f;
	public float maxOpacity = 1.0f;
	float phase1;
	float phase2;

	RawImage image;

	// Use this for initialization
	void Start () {
	
		isEnabled = false;
		image = this.GetComponent<RawImage> ();
		image.enabled = false;
		phase1 = Random.Range (0, 2 * Mathf.PI);
		phase2 = Random.Range (0, 2 * Mathf.PI);


	}
	
	// Update is called once per frame
	void Update () {

		if (isEnabled) {
			phase1 += (frequency1 * 2*Mathf.PI) * Time.deltaTime;
			phase2 += (frequency2 * 2*Mathf.PI) * Time.deltaTime;
			/*float f1 = Mathf.Cos (phase1);
			float f2 = Mathf.Cos (phase2);
			float f3 = f1 + f2;
			float f4 = f3 / 4.0f;
			float f5 = (maxOpacity - minOpacity);
			float f6 = f4 * f5;
			float f7 = f5 / 2.0f;
			float f8 = f7 + f6;*/
			float opacity = minOpacity + (maxOpacity - minOpacity) / 2.0f + ((Mathf.Cos (phase1) + Mathf.Cos (phase2)) / 4.0f) * (maxOpacity - minOpacity);
			image.color = new Color(1, 1, 1, opacity);
		}
	
	}

	public void setEnable(bool e) {
		if (e)
			enable ();
		else
			disable ();
	}

	public void enable() {

		isEnabled = true;
		image.enabled = isEnabled;

	}

	public void disable() {

		isEnabled = false;
		if (image != null) {
			image.enabled = isEnabled;
		}

	}
}
