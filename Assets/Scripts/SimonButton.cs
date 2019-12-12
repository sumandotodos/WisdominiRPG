using UnityEngine;
using System.Collections;

public class SimonButton : Interactor {

	[HideInInspector]
	public Camera cam;
	[HideInInspector]
	public SimonButtonController controller;
	[HideInInspector]
	public bool playerInput;
	[HideInInspector]
	public int buttonId;
	public float litIntensity = 6.0f;
	public Light lightRef;
	public LevelControllerScript level;
	public GameObject litPiece;
	public GameObject unlitPiece;
	public AudioClip sound;
	public float litTime = 0.5f;
	float elapsedTime;
	int state = 0; // 0 is idle    1 is active    2 is displacing in     3 is displacing out
	Vector3 originalPosition;
	float displacement;
	public float maxDisplacement = 0.4f;
	bool displace;

	void unlightButton() {
		unlitPiece.GetComponent<Renderer> ().enabled = true;
		litPiece.GetComponent<Renderer> ().enabled = false;
		if (lightRef != null)
			lightRef.intensity = 0;
	}

	public void lightButton() {
		unlitPiece.GetComponent<Renderer> ().enabled = false;
		litPiece.GetComponent<Renderer> ().enabled = true;
		state = 1;
		elapsedTime = 0.0f;
		if (lightRef != null)
			lightRef.intensity = litIntensity;
		if ((level != null) && (sound != null)) {
			level.playSound (sound);
		}
	}

	// Use this for initialization
	new void Start () {

		if (lightRef != null)
			lightRef.intensity = 0;
		displacement = 0.0f;
		originalPosition = litPiece.transform.localPosition;
		unlitPiece.GetComponent<Renderer> ().enabled = true;
		litPiece.GetComponent<Renderer> ().enabled = false;
	
	}
		

	public void enableDisplacement() {
		displace = true;
	}

	public void disableDisplacement() {
		displace = false;
	}

	override public void effect() {
		lightButton();
		controller.playerPress (buttonId);
	}

	override public string interactIcon() {
		return "Hand";
	}

	// Update is called once per frame
	new void Update () {


		// slot 1
		if (Input.GetMouseButtonDown (0) && playerInput) {

			// raycast-test this
			RaycastHit[] hit;
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);

			hit = Physics.RaycastAll (ray);

			for (int i = 0; i < hit.Length; ++i) {
				if (hit [i].collider == this.GetComponent<Collider> ()) { // if we hit this button
					effect();
				}
			}

		}




		// slot 0
		if (state == 0) { // idle

		}
		if (state == 1) { // pressed time delay
			elapsedTime += Time.deltaTime;
			if (elapsedTime > litTime) {
				elapsedTime = 0.0f;
				state = 0;
				unlightButton ();
			}
		}
	}


}
