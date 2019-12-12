using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : WisdominiObject {

	public List<CameraUtils> cams;
	int curCamera;

	public bool autoAssemble = true;
	public int dontDisableIndex = -1;

	int mainCameraIndex = 0;

	// Use this for initialization
	void Start () {
		if(autoAssemble) {
			mainCameraIndex = 0;
			CameraUtils[] camsInScene = GameObject.FindObjectsOfType<CameraUtils> ();
			CameraUtils mainCamera = GameObject.Find ("Main Camera").GetComponent<CameraUtils>();
			cams = new List<CameraUtils> ();
			cams.Add (mainCamera);
			for (int i = 0; i < camsInScene.Length; ++i) {
				if (camsInScene [i] != mainCamera) {
					cams.Add (camsInScene [i]);
					camsInScene [i].gameObject.GetComponent<Camera> ().enabled = false;
				}
			}
		}
		else {
			mainCameraIndex = dontDisableIndex;
			for (int i = 0; i < cams.Count; ++i) {
				if((i != dontDisableIndex) && (cams[i] != null))
				cams [i].GetComponent<Camera> ().enabled = false;
			}
		}
		curCamera = 0;
	}
	
	public void _wm_switchToCameraIndex(int n) {
		cams [n]._wm_setRenderingEnabled (true);
		Mover camMover = cams [n].GetComponent<Mover> ();
		if (camMover != null)
			camMover._wm_restartPosition ();
		if (curCamera != n) {
			cams [curCamera]._wm_setRenderingEnabled (false);
			curCamera = n;
		}
	}

	public void _wm_switchToCameraName(string n) {
		for (int i = 0; i < cams.Count; ++i) {
			if (cams [i] != null) {
				if (cams [i].gameObject.name.Equals (n)) {
					_wm_switchToCameraIndex (i);
				}
			}
		}
	}

	public void _wm_switchToMainCamera() {
		cams [mainCameraIndex]._wm_setRenderingEnabled (true);
		if (curCamera != mainCameraIndex) {
			cams [curCamera]._wm_setRenderingEnabled (false);
		}
		curCamera = mainCameraIndex;
	}

}
