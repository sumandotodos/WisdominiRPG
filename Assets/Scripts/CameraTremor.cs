using UnityEngine;
using System.Collections;

public class CameraTremor : WisdominiObject {


	public RenderTexture target;
	Camera cam;


	public float amplitude;
	public float frequency;
	public bool X, Y, Z;
	public Vector3 baseLocalPosition;

	int frameGrabStatus;
	float camAspect;

	float elapsedTime;


	float phase;

	bool active = false;

	// Use this for initialization
	new void Start () {
	
		active = false;
		phase = 0.0f;

		frameGrabStatus = 0;
		cam = this.GetComponent<Camera> ();

		elapsedTime = 0.0f;

	}
	
	// Update is called once per frame
	new void Update () {

		if (!active)
			return;

		phase += Time.deltaTime * frequency;
		Vector3 noiseVector = amplitude * new Vector3 (pseudorandom (phase), pseudorandom (phase + 1.21f), 
			                      pseudorandom (phase - 0.95f));
		if (!X)
			noiseVector.x = 0;
		if (!Y)
			noiseVector.y = 0;
		if (!Z)
			noiseVector.z = 0;
		this.transform.localPosition = baseLocalPosition + noiseVector;
	
	}

	public float pseudorandom(float t) {

		return Mathf.Cos (t) + Mathf.Sin (t + 32) + Mathf.Cos (t - 21) * Mathf.Sin (t + 2) * Mathf.Cos(t * 1.25f) 
			- Mathf.Sin(t * 0.85f);

	}

	public void _wm_setActive(bool act) {
		active = act;
		if (active == true) {
			baseLocalPosition = this.transform.localPosition;
		}
		if (active == false) {
			this.transform.localPosition = baseLocalPosition;
		}
	}

	public void _wm_setAmplitude(float a) {
		amplitude = a;
	}

	public void _wm_setFrequency(float f) {
		frequency = f;
	}


	public void _wm_grab() {

		grab();

	}

	public void grab() {

		frameGrabStatus = 1;

	}

	void OnPostRender() {

		if (frameGrabStatus == 1) {

			camAspect = cam.aspect;

			cam.targetTexture = target;
			cam.aspect = camAspect;
			++frameGrabStatus;
			return;
		}

		if (frameGrabStatus == 2) {

			cam.targetTexture = null;
			frameGrabStatus = 0;
			cam.aspect = camAspect;
		}
	}


}
