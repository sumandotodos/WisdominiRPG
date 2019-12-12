using UnityEngine;
using System.Collections;

public enum InventoryState { Closed, Open, Opening, Closing };

public class InventoryObject : WisdominiObject {

	Animator anim;

	public UIInventory uiInvRef;

	public LevelControllerScript level;

	const float hidingY = 0.4f;
	const float showingY = 0.1f;
	const float ySpeed = 1.0f;

	const float DELAY = 0.3f;

	float delay = 0.0f;
	float elapsedTime = 0.0f;



	bool isOpen;

	float y, targetY;

	// Use this for initialization
	new void Start () {

	}


	public void initialize () {
	
		base.Start ();

		anim = this.GetComponent<Animator> ();
		isOpen = false;

		y = targetY = hidingY;
		Vector3 newPos = this.transform.localPosition;
		newPos.y = y;
		this.transform.localPosition = newPos;

		//_wm_open ();

	}
	
	// Update is called once per frame
	new void Update () {
	
		if (y > targetY) {
			y -= ySpeed * Time.deltaTime;
			if (y < targetY) {
				y = targetY;
				if(waitingRef != null) 
					notifyFinishAction ();
			}
			Vector3 newPos = this.transform.localPosition;
			newPos.y = y;
			this.transform.localPosition = newPos;
		}

		if (y < targetY) {
			y += ySpeed * Time.deltaTime;
			if (y > targetY) {
				y = targetY;
				if(waitingRef != null) 
					notifyFinishAction ();
			}
			Vector3 newPos = this.transform.localPosition;
			newPos.y = y;
			this.transform.localPosition = newPos;
		}

		if (delay > 0.0f) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > delay) {
				delay = 0.0f;

				targetY = hidingY;
				isOpen = false;

			}
		}

	}

	public void reenter() {

		this.GetComponent<Animator> ().SetTrigger ("InventoryReentry");
		//this.GetComponent<Animator> ().SetBool ("InventoryOpen", true);
		targetY = y = showingY;
		Vector3 newPos = this.transform.localPosition;
		newPos.y = y;
		this.transform.localPosition = newPos;
		isOpen = true;
		delay = 0.0f;
		uiInvRef.show ();
		level.storeStringValue ("ReentryCondition", "Inventory");

		level.storePlayerCoordinates ();

	}

	public void _wm_open() {

		anim.SetTrigger ("InventoryOpen");
		targetY = showingY;
		isOpen = true;
		delay = 0.0f;
		if(uiInvRef != null)
		uiInvRef.show ();
		level.storeStringValue ("ReentryCondition", "Inventory");

		level.storePlayerCoordinates ();


	}

	public void _wm_close() {


		anim.SetTrigger ("InventoryClose");
		//isOpen = false;
		delay = DELAY;
		elapsedTime = 0.0f;
		if (uiInvRef != null)
			uiInvRef.hide ();
		level.storeStringValue ("ReentryCondition", "");


	}

	public void _wa_open(WisdominiObject waiter) {

		waitingRef = waiter;
		_wm_open ();

	}

	public void _wa_close(WisdominiObject waiter) {

		waitingRef = waiter;
		_wm_close ();

	}

	public void _wm_invertState() {

		if (isOpen) {
			_wm_close ();
		} else
			_wm_open ();

	}


}
