using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepButton : MonoBehaviour {

	public GameObject litPiece;
	public GameObject unlitPiece;

	float initialZ;
	public float depressZDelta;
	float depressedZ;
	float z;
	float targetz;

	public int arrayPosition;
	public StepButtonController buttonController;

	bool lit = false;

	int state = 0;

	public bool steppable = true;

	// Use this for initialization
	void Start () {
		depressedZ = initialZ - depressZDelta;
		z = initialZ;
		litPiece.SetActive (false);
	}

	public void toggle() {
		if (buttonController.stopOnSuccess && buttonController.succeded)
			return;
		if (state == 0) {
			state = 1;
			targetz = depressedZ;
			buttonController.toggleData (arrayPosition);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 1) {
			if (!Utils.updateSoftVariable (ref z, targetz, 2.0f)) {
				state = 2;
				targetz = initialZ;
				lit = !lit;
				litPiece.SetActive (lit);
				unlitPiece.SetActive (!lit);
			} else
				litPiece.transform.localPosition = new Vector3 (0, 0, z);
				unlitPiece.transform.localPosition = new Vector3 (0, 0, z);
		}
		if (state == 2) {
			if (!Utils.updateSoftVariable (ref z, targetz, 2.0f)) {
				state = 0;
				targetz = initialZ;

				litPiece.transform.localPosition = new Vector3 (0, 0, initialZ);
				unlitPiece.transform.localPosition = new Vector3 (0, 0, initialZ);
			} else
				litPiece.transform.localPosition = new Vector3 (0, 0, z);
				unlitPiece.transform.localPosition = new Vector3 (0, 0, z);
		}
	}

	public void OnTriggerEnter(Collider Other) {
		if (Other.tag == "Player") {
			toggle ();
		}
	}
}
