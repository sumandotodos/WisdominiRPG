using UnityEngine;
using System.Collections;

public class WallLever : Interactor {

	/* references */

	public LevelControllerScript level_N;
	public GameObject levelPiece;
	public AudioClip activateSound_N;
	public AudioClip deactivateSound_N;
	bool activated;
	public bool interactable;
	public WallLeverPuzzleController controller_N;

	public WallLever[] switchOthers;

	/* public properties */

	public float minHeight;
	public float maxHeight;
	float height;
	float targetHeight;
	public float slideSpeed;
	Vector3 initialPos;


	// Use this for initialization
	new void Start () {

		height = minHeight;
		activated = false;
		targetHeight = minHeight;
		initialPos = levelPiece.transform.localPosition;
		levelPiece.transform.localPosition = new Vector3 (initialPos.x, height, initialPos.z);
		if (level_N == null)
			level_N = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

	
	}

	public void switchLeverNoPropagation() {



		if (activated == false) {
			targetHeight = maxHeight;
			activated = true;
			if(level_N != null) {
				if (activateSound_N != null) {
					level_N.playSound (activateSound_N);
				}
			}
		} else {
			targetHeight = minHeight;
			activated = false;
			if(level_N != null) {
				if (activateSound_N != null) {
					level_N.playSound (deactivateSound_N);
				}
			}
		}



	}

	public void switchLever() {

		switchLeverNoPropagation ();

		for (int i = 0; i < switchOthers.Length; ++i) {
			switchOthers [i].switchLeverNoPropagation ();
		}

		if (controller_N != null)
			controller_N.action ();

	}

	public bool isActivated() {
		return activated;
	}

	public void _wm_switchLever() {

		switchLever ();

	}
	
	// Update is called once per frame
	new void Update () {

		if (height < targetHeight) {
			height += slideSpeed * Time.deltaTime;
			if (height > maxHeight) {
				height = maxHeight;
			}
			levelPiece.transform.localPosition = new Vector3 (initialPos.x, height, initialPos.z);
		}

		if (height > targetHeight) {
			height -= slideSpeed * Time.deltaTime;
			if (height < minHeight) {
				height = minHeight;
			}
			levelPiece.transform.localPosition = new Vector3 (initialPos.x, height, initialPos.z);
		}
	
	}

	public override string interactIcon() {
		if (interactable) {
			return "Hand";
		}
		return "";
	}

	public override void effect() {
		switchLever ();
	}
}
