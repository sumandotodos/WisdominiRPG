using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N3AuxFerfufloChecker : WisdominiObject {

	public int numberOfFerfufloTests = 87;

	public BetterDoor2 door;

	LevelControllerScript level;

	public AudioClip openSound;
	public AudioClip noOpenSound;

	alertImageController alert;

	void Start() {
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		alert = GameObject.Find ("AlertImage").GetComponent<alertImageController> ();
	}

	bool waiting;

	public int state = 0;

	int percent;

	new void Update() {

		if (state == 1) {
			if (!alert._wm_isOpen ()) {
				state = 4;
			}

		}

		if (state == 2) {
			if (!alert._wm_isOpen ()) {
				state = 5;
			}

		}

		if (state == 3) {
			if (!alert._wm_isOpen ()) {
				state = 6;
			}

		}

		if (state == 4) {
			level.player.unblockControls ();
			door._wm_open ();
			state = 0;
		}

		if (state == 5) {
			level.player.unblockControls ();
			FerfufloController.resetFerfufloAnswers (level.mcRef);
			state = 0;
		}

		if (state == 6) {
			
			alert._wa_setAlertMessage (this, "Tarjetas configuradas al " + percent + "%  Es necesario que las tarjetas estén configuradas al 100%");
			this.isWaitingForActionToComplete = true;
			alert.programNotification = -1;

			state = 7;
		}

		if(state == 7) {
		if(!isWaitingForActionToComplete) {
				level.player.unblockControls ();
				state = 0;
			}
		}

	}

	public void _wm_checkFerfufloAccess() {

		if (level.retrieveIntValue ("FerfufloCorrect") == numberOfFerfufloTests) {
			level.playSound (openSound);
			level.player.blockControls ();
			level._wm_alert ("Puerta abierta");
			state = 1;
//			level.player.unblockControls ();
//			door._wm_open ();
		} else {
			if (level.retrieveIntValue ("FerfufloCompleted") == numberOfFerfufloTests) {
				level.playSound (noOpenSound);
				level.player.blockControls ();
				level._wm_alert ("El código configurado en las tarjetas no es válido. Como medida de seguridad, se han reiniciado las tarjetas.");
				state = 2;
//				level.player.unblockControls ();
//				FerfufloController.resetFerfufloAnswers (level.mcRef);
			} else {
				percent = (int)(((float)level.retrieveIntValue("FerfufloCompleted") / 87.0f) * 100.0f);
				level.player.blockControls ();
				level.playSound (noOpenSound);
				state = 3;
//				level._wm_alert ("Tarjetas configuradas al " + percent + "%  Es necesario que las tarjetas estén configuradas al 100%");
//				level.player.unblockControls ();
			}
		}

	}

}
