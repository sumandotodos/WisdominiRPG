using UnityEngine;
using System.Collections;

public class CameraFrameGrab : WisdominiObject {

	/* references */

	public RenderTexture target;
	Camera cam;

	/* properties */

	int frameGrabStatus;
	float camAspect;

	float elapsedTime;


	// Use this for initialization
	new void Start () {

		frameGrabStatus = 0;
		cam = this.GetComponent<Camera> ();

		elapsedTime = 0.0f;
	
	}

	public void _wm_grab() {

		grab();

	}
	
	public void grab() {

		frameGrabStatus = 1;

	}

	new void Update() {

		/*elapsedTime += Time.deltaTime;
		if (elapsedTime > 5.0f) {
			elapsedTime = 0.0f;
			grab ();
		}*/

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
