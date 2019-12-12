using UnityEngine;
using System.Collections;

public class WallLeverPuzzleController : MonoBehaviour {

	public WisdominiObject prizeProgram;
	public WallLever[] levers;
	public LevelControllerScript level;
	public AudioClip prizeSound;

	public void action() {

		int leverOK = 0;
		for (int i = 0; i < levers.Length; ++i) {
			if (levers [i].isActivated ())
				++leverOK;
		}

		if (leverOK == levers.Length) {
			if ((level != null) && (prizeSound != null)) {
				level.playSound (prizeSound);
				if (prizeProgram != null) {
					prizeProgram.startProgram (0);
				}
			}
		}

	}

	// Use this for initialization
	void Start () {
		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
