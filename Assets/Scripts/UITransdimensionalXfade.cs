using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITransdimensionalXfade : WisdominiObject {

	RawImage img;
	float opacity;
	int state;
	public float opacitySpeed = 0.2f;

	// Use this for initialization
	new void Start () {
	


		opacity = 1.0f;
		img = this.GetComponent<RawImage> ();
		img.enabled = false;
		state = 0;

		MasterControllerScript mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		DataStorage ds = mc.getStorage ();
		if (ds.retrieveBoolValue ("IsChangingPlanes")) {
			ds.storeBoolValue ("IsChangingPlanes", false);
			_wm_startXfade ();
		}

	}

	public void _wa_startXfade(WisdominiObject waiter) {

		waitingRef = waiter;
		waiter.isWaitingForActionToComplete = true;
		_wm_startXfade ();

	}

	public void _wm_startXfade() {

		opacity = 1.0f;
		img.color = new Color (1, 1, 1, opacity);
		img.enabled = true;
		state = 1;

	}
	
	// Update is called once per frame
	new void Update () {
	
		if (state == 0)
			return;

		if(state == 1) {

			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < 0.0f) {
				opacity = 0.0f;
				img.enabled = false;
				notifyFinishAction ();
				state = 0;
			}

			img.color = new Color (1, 1, 1, opacity);

		}

	}
}
