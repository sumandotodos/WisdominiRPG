using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N3FerfufloTV : WisdominiObject {

	public string id;
	public int amount;

	LevelControllerScript level;

	public TVChannelSwitch tvChannelSwitch;

	alertImageController alert;

	int currentAmount;

	// Use this for initialization
	void Start () {
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		currentAmount = level.retrieveIntValue ("TVFerfuflosTimes" + id);
		if (currentAmount == amount) {
			tvChannelSwitch._wm_switchChannel (1);
			level.storeIntValue ("TVFerfuflosClearFirstTime", 1);
		}
		_wm_storeTVid ();
		alert = GameObject.Find ("AlertImage").GetComponent<alertImageController> ();
	}

	public void _wa_showPercentage(WisdominiObject waiter) {
		this.waitingRef = waiter;
		alert._wa_setAlertMessage (this, "Tarjetas configuradas al " + calculatePercetage () + "%");
		alert.programNotification = -1;
		isWaitingForActionToComplete = true;
		state = 1;
	}

	int state = 0;

	void Update() {

		if (state == 1) {
			if (!isWaitingForActionToComplete) {
				state = 0;
				notifyFinishAction ();
			}
		}

	}

	public bool _wm_isClear() {
		return currentAmount == amount;
	}
	
	public void _wm_storeTVid() {
		level.storeStringValue ("TVFerfuflosId", id);
		level.storeIntValue ("TVFerfuflosAmount", amount);
	}

	public int calculatePercetage() {
		int percent = (int)(((float)level.retrieveIntValue("FerfufloCompleted") / 41.0f) * 100.0f);
        percent = percent > 100 ? 100 : percent;
        return percent;
	}

	public void doAlert() {

	}
		

}
