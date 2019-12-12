using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

enum HUDCrackGlassState { cracking, idle, cracked, changingActivity };

public class HUDCrackGlass : WisdominiObject {

	/* references */

	public Sprite [] crackedGlass;
	public AudioClip [] crackAudio;
	Image theImage;
	public Camera cam;
	MasterControllerScript master;
	DataStorage ds;
	LevelControllerScript levelController;

	/* properties */

	HUDCrackGlassState state;
	float elapsedTime;
	float nextTimeToCrack;
	int frame;
	int waitFrames;

	/* public properties */



	/* constant */

	const float minTimeToCrack = 0.25f;
	const float maxTimeToCrack = 0.75f;


	// Use this for initialization
	new void Start () {
	
		state = HUDCrackGlassState.idle;
		theImage = this.GetComponent<Image> ();
		theImage.color = new Color(0, 0, 0, 0);
		theImage.enabled = false;
		elapsedTime = 0.0f;
		waitFrames = 2;

		master = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = master.getStorage ();
		if (ds.retrieveBoolValue ("IsHUDCracked")) {
			state = HUDCrackGlassState.cracked;
			theImage.enabled = true;
			theImage.color = new Color(1, 1, 1, 1);
			theImage.sprite = crackedGlass [crackedGlass.Length-1];
		}
		levelController = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();

	}
	
	// Update is called once per frame
	new void Update () {
	
		if (state == HUDCrackGlassState.cracking) {

			elapsedTime += Time.deltaTime;
			if (elapsedTime > nextTimeToCrack) {
				elapsedTime = 0.0f;
				nextTimeToCrack = FloatRandom.floatRandomRange (minTimeToCrack, maxTimeToCrack);
				if (frame < crackedGlass.Length-1) {
					++frame;
					theImage.sprite = crackedGlass [frame];
					levelController.playSound (crackAudio [frame]);
				} else {
					state = HUDCrackGlassState.cracked;
				}
			}

		}

		if (state == HUDCrackGlassState.cracked) {


		}

		if (state == HUDCrackGlassState.changingActivity) {

			--waitFrames;
			if (waitFrames == 0) {

				ds.storeStringValue ("ReturnLocation", levelController.locationName);
				levelController.storeStringValue ("CurrentLevel", levelController.locationName);
				levelController.storePlayerCoordinates ();
				SceneManager.LoadScene ("Scenes/QA");

			}

		}


	}

	public void changeActivity() {

		if (state != HUDCrackGlassState.cracked) 
		{
			return;
		}
		waitFrames = 2;
		CameraUtils cgrab = cam.GetComponent<CameraUtils> ();
		cgrab.grab ();
		state = HUDCrackGlassState.changingActivity;

	}

	public void crack() {

		theImage.enabled = true;
		theImage.sprite = crackedGlass [0];
		levelController.playSound (crackAudio [0]);
		theImage.color = new Color (1, 1, 1, 1);
		elapsedTime = 0.0f;
		nextTimeToCrack = FloatRandom.floatRandomRange (minTimeToCrack, maxTimeToCrack);
		state = HUDCrackGlassState.cracking;
		ds.storeBoolValue ("IsHUDCracked", true);

	}

	public void _wm_crack() {

		crack ();

	}


}
