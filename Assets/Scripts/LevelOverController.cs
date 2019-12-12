using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelOverController : MonoBehaviour {

	/* references */

	public Flash flasher;
	public PieceSlide[] piece;
	public UITextFader text1;
	public UITextFader text2;
	public Text t1, t2;
	public UIImageScaler planetScaler;
	public UIRawImageFader planetFader;
	public UIFaderScript fader;
	public StringBank tryAgainBank;
	public AudioClip breakSound;
	AudioSource aSource;
	MasterControllerScript mc;
	DataStorage ds;


	/* properties */

	float elapsedTime; // slot 0 elapsed time
	int state = 0; // slot 0 state



	/* constants */
	const float initialDelay = 2.2f;


	Rosetta rosetta;


	// Use this for initialization
	void Start () {

		elapsedTime = 0.0f;
		rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();
		tryAgainBank.rosetta = rosetta;
		tryAgainBank.reset ();
		t1.text = tryAgainBank.getString (0);
		t2.text = tryAgainBank.getString (0);
		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = mc.getStorage ();

		aSource = this.GetComponent<AudioSource> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == 0) { // initial delay
		
			elapsedTime += Time.deltaTime;
			if (elapsedTime > initialDelay) {

				flasher.go ();
				state = 1;
				elapsedTime = 0.0f;
			
			}
		}

		if (state == 1) { // flashing

			elapsedTime += Time.deltaTime;
			if (elapsedTime > 0.25f) {
				for (int i = 0; i < piece.Length; ++i) {
					piece [i].go ();
				}
				if ((aSource != null) && (breakSound != null)) {
					aSource.PlayOneShot (breakSound);
				}
				elapsedTime = 0.0f;
				++state;
			}

		}

		if (state == 2) { // pieces separating
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1.0f) {
				text1.fadeIn ();
				text2.fadeIn ();
				elapsedTime = 0.0f;
				++state;
			}
		}

		if (state == 3) { // showing text
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1.0f) {
				planetFader.fadeIn ();
				planetScaler.go ();
				elapsedTime = 0.0f;
				++state;
			}
		}

		if (state == 4) { // showing planet, and final delay
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 6.0f) {
				fader.fadeOut ();
				elapsedTime = 0.0f;
				++state;
			}
		}

		if (state == 5) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 3.0f) {
				killLevel (1);
			}
		}

	}

	public void killLevel(int l) {

        mc.loadGame(true);
        mc.saveGame(false);
        SceneManager.LoadScene("root");
        //ds.storeBoolValue ("PlayerMustMaterialize", true);
        //ds.storeStringValue ("ReentryCondition", "");

        // NO, CHAVAL, SISTEMA DE CHECKPOINTS
        /*
		switch (l) {
		// Cambiar todo esto para no tener que inicializar tantos valores, es un pestazo
		case 1: // clear all stored values from Level1
			ds.storeBoolValue ("IsDoorbrownDoorGeoOpen", false);
			ds.storeBoolValue ("IsDoorgreyDoorGeoOpen", false);
			ds.storeBoolValue ("IsDoorblueDoorGeoOpen", false);
			ds.storeBoolValue ("IsDoorgreenDoorGeoOpen", false);
			ds.storeBoolValue ("IsDoorgreyDoorGeo (1)Open", false);
			ds.storeBoolValue ("IsDoorgreyDoorGeo (2)Open", false);
			ds.storeBoolValue ("IsDoorgreyGeoCasa2", false);
			ds.storeBoolValue ("IsDoorgreyGeoCasa3", false);
			ds.storeBoolValue ("IsDoorgreyGeoCasaInsultador", false);
			ds.storeBoolValue ("IsDoorgreyGeoCasaTraficante", false);
			ds.storeBoolValue ("IsDoorgreyGeoCasa4", false);
			ds.storeBoolValue ("IsDoorgreyGeoCasa5", false);
			ds.storeBoolValue ("IsLeverOn", false);
			ds.storeBoolValue ("IsLeverOn2", false); 
			ds.storeFloatValue ("CoordsLevel1/Plane0_exteriorX", 0.0f);
			ds.storeFloatValue ("CoordsLevel1/Plane0_exteriorY", 0.0f);
			ds.storeFloatValue ("CoordsLevel1/Plane0_exteriorZ", 0.0f);
			ds.storeIntValue ("Orientation", 0);
			ds.storeIntValue ("Level1ShadowWaveNumber", 0);
			ds.storeBoolValue ("Level1BlueMirrorDepleted", false);
			ds.storeBoolValue ("Level1BlueMirrorActivated", false);
			ds.storeBoolValue ("Level1BrownMirrorDepleted", false);
			ds.storeBoolValue ("Level1BrownMirrorActivated", false);
			ds.storeBoolValue ("Level1YellowMirrorDepleted", false);
			ds.storeBoolValue ("Level1YellowMirrorActivated", false);
			ds.storeBoolValue ("Level1GreenMirrorDepleted", false);
			ds.storeBoolValue ("Level1GreenMirrorActivated", false);
			SceneManager.LoadScene ("Scenes/Level1/Level1Plane0_exterior");
			break;

		default:
			
			break;
		}*/

    }
}
