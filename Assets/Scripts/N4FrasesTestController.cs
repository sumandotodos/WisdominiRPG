using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N4FrasesTestController : WisdominiObject {

	public PlayerScript player;
	alertImageController alert;

	public N4Puente puente;

	public AudioClip errorSound_N;
	public AudioClip hitSound_N;
	public AudioClip clearSound_N;

	public GameObject bloqueoTrasero1;
	public GameObject bloqueoTrasero2;
	public GameObject bloqueoFrontal;

	public int nFrasesCiencia = 9;
	public int nFrasesMente = 5;

	public FGTable cienciaTabla;
	public FGTable espirituTabla;

	public ParticleSystem cienciaPart;
	public ParticleSystem espirituPart;
	public N4LightSignal cienciaSignal;
	public N4LightSignal espirituSignal;

	public CameraFollowAux cameraFollow;
	public CameraTremor cameraTremor;

	public int frasesTotales;

	public string currentFrase;

	public int state = 0;

	public bool currentIsScience;
	bool playerVotesScience;

	public int frasesCorrectas;

	// Use this for initialization
	void Start () {
		reset ();	
	}

	public void reset() {
		frasesTotales = nFrasesMente + nFrasesCiencia;
		frasesCorrectas = 0;
		state = 0;
		cienciaPart.Stop ();
		espirituPart.Stop ();
		this.programIsWaitingForActionToComplete = new List<bool> ();
		this.programIsWaitingForActionToComplete.Add (false);
		alert = GameObject.Find ("AlertImage").GetComponent<alertImageController> ();
		nFrasesMente = espirituTabla.nRows ();
		nFrasesCiencia = cienciaTabla.nRows ();
	}

	public void _wm_reset() {
		reset ();

	}

	public void _wm_startTest() {
		bloqueoFrontal.SetActive (true);
		bloqueoTrasero1.SetActive (true);
		bloqueoTrasero2.SetActive (true);
		state = 1;
	}

	public void _wm_chooseCiencia() {

	}

	public void _wm_chooseMeditador() {

	}

	public void _wm_prepareNextFrase() {

	}
	
	// Update is called once per frame
	void Update () {
		if (state == 0) {
		  // idle
		}

		if (state == 1) {
			player.blockControls ();
			int row;
			// choose a sentence
			if ((nFrasesMente > 0) && (nFrasesCiencia > 0)) {
				int r = Random.Range (0, 2);
				if (r == 0) {
					currentIsScience = true;
					row = cienciaTabla.getNextRowIndex ();
					currentFrase = (string)cienciaTabla.getElement ("FRASE", row);
					--nFrasesCiencia;
				} else {
					currentIsScience = false;
					row = espirituTabla.getNextRowIndex ();
					currentFrase = (string)espirituTabla.getElement ("FRASE", row);
					--nFrasesMente;
				}
			} else {
				if ((nFrasesCiencia > 0)) {
					currentIsScience = true;
					row = cienciaTabla.getNextRowIndex ();
					currentFrase = (string)cienciaTabla.getElement ("FRASE", row);
					--nFrasesCiencia;
				} else if ((nFrasesMente > 0)) {
					currentIsScience = false;
					row = espirituTabla.getNextRowIndex ();
					currentFrase = (string)espirituTabla.getElement ("FRASE", row);
					--nFrasesMente;
				} else {
					puente._wm_resetBridge();

					reset ();
				}
			}
			state = 2;
			alert._wa_setAlertMessage (this, currentFrase);
			alert.registerWaitingObject (this, 0);
			this.programIsWaitingForActionToComplete [0] = true;
		}

		if (state == 2) {
			if (!this.programIsWaitingForActionToComplete [0]) {
				player.unblockControls ();
				state = 3;
			}
		}

		if (state == 3) { // waiting for player input... state will change to 4

		}

		if (state == 4) {
			if (playerVotesScience == currentIsScience) { // correct
				if (currentIsScience) {
					cienciaPart.Play ();
					cienciaSignal._wm_signal ();
					Invoke ("stopAllParticles", 3.75f);

					puente._wm_lowerBridge ();
				} else {
					espirituPart.Play ();
					espirituSignal._wm_signal ();
					Invoke ("stopAllParticles", 3.75f);

					puente._wm_lowerBridge ();
				}
			} else { // incorrect
				puente._wm_resetBridge();

				reset ();
			}
			if ((nFrasesMente == 0) && (nFrasesCiencia == 0)) { // fin del test
				state = 0;
				cameraFollow.clearIntermediateLocations (); // libera la cámara
				GameObject.Find("LevelController").GetComponent<LevelControllerScript>().storeBoolValue("N4TestPuentePassed", true);
				bloqueoFrontal.SetActive (false);
				bloqueoTrasero1.SetActive (false);
				bloqueoTrasero2.SetActive (false);
				player.unblockControls ();
			} else {
				player._wa_autopilotTo (this, -56.96f, 59.25f, -223.0f);
				this.isWaitingForActionToComplete = true;
				state = 5;
			}
		}

		if (state == 5) {
			if (!this.isWaitingForActionToComplete) {
				player._wa_autopilotTo (this, -56.96f, 59.25f, -222.55f);
				this.isWaitingForActionToComplete = true;
				state = 6;
			}
		}

		if (state == 6) {
			if (!this.isWaitingForActionToComplete) {
				player.idlePose ();
				state = 1;
			}
		}

	}

	public void _wm_voteScience() {
		if (state == 3) {
			Debug.Log ("VoteScience");
			playerVotesScience = true;
			state = 4;
		}
	}

	public void _wm_voteSpirit() {
		if (state == 3) {
			Debug.Log ("VoteSpirit");
			playerVotesScience = false;
			state = 4;
		}
	}

	public bool _wm_hasFinished() {
		return (frasesTotales == frasesCorrectas);
	}

	public void stopAllParticles() {
		cienciaPart.Stop ();
		espirituPart.Stop ();
	}
}
