using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum UIChispAlertState { waitingForAppearance, Idle, waitingForClose, dismissed };

public class UIChispAlert : WisdominiObject {

	/* references */

	//public GameObject chispa;

	public LevelControllerScript level;
	public AudioClip lToR;
	public AudioClip rToL;
	public AudioClip openSound;
	public AudioClip closeSound;
	public GameObject dustParticlePrefab;
	public UIFaderScript backdrop;
	public Text theText;
	public ChispAlertChispa chispa;
	public ChispaWorld chispaWorld;
	public GameObject alphaCurtain;
	string text;

	RectTransform alphaRect;

	/* properties */

	//List<string> msgHeap;
	bool ChispaLeftToRight = true;
	bool chispaIsOut;
	public UIChispAlertState state;

	new void Start () 
	{	
		//msgHeap = new List<string> ();
		alphaRect = alphaCurtain.GetComponent<RectTransform>();
		chispaIsOut = false;
		state = UIChispAlertState.dismissed;
	}
	
	new void Update () 
	{
		float scaleFactor = 600.0f / Screen.height;
		float aspect = Screen.width / Screen.height;
	
		if (!ChispaLeftToRight) {
			float x = chispa.xPos - (Screen.width / aspect);
			if (x > (Screen.width / 2))
				x = (Screen.width / 2);
			alphaRect.transform.position = new Vector3 (x, 0, 0);
		} else {
			float x = chispa.xPos + (Screen.width / aspect);
			if (x < (Screen.width / 2))
				x = (Screen.width / 2);
			alphaRect.transform.position = new Vector3 (x, 0, 0);
		}

		if (state == UIChispAlertState.waitingForAppearance) 
		{
			if (isWaitingForActionToComplete)
				return;

			alert (text);
			state = UIChispAlertState.Idle;
		}

		if (state == UIChispAlertState.waitingForClose) 
		{
			if (isWaitingForActionToComplete)
				return;

			close ();
			state = UIChispAlertState.Idle;
		}		
	}

	public void alert(string msg) 
	{
		text = msg;
		if (chispaIsOut == false) 
		{
			chispaIsOut = true;
			if (chispaWorld != null) 
			{
				chispaWorld._wa_appear (this);
				if ((level != null) && (openSound != null))
					level.playSound (openSound);
				this.isWaitingForActionToComplete = true;
			}
			else
				this.isWaitingForActionToComplete = false;
			state = UIChispAlertState.waitingForAppearance;
			return;			
		}

		alphaRect.transform.position = new Vector3(-Screen.width / 20.0f - 160.0f-Screen.width-600.0f, 0, 0);
		theText.text = text;
		theText.color = new Color (0, 0, 0, 0);
		backdrop.setFadeColor (1, 1, 1);
		backdrop.fadeOut ();
		backdrop.GetComponent<Image> ().raycastTarget = true;
		theText.raycastTarget = true;
		if (ChispaLeftToRight) {
			chispa.swipeLeftToRight ();
			if ((level != null) && (lToR != null))
				level.playSound (lToR);
		} else {
			chispa.swipeRightToLeft ();
			if ((level != null) && (rToL != null))
				level.playSound (rToL);
		}

		ChispaLeftToRight = !ChispaLeftToRight;
	}

	public void dismiss() 
	{
		if (state == UIChispAlertState.dismissed)
			return;

		bool canDismiss = true;
		if (chispaWorld != null) {
			canDismiss = !chispaWorld.transitioning;
		}

		if ((chispa.state == ChispaState.idle) && canDismiss) 
		{
			notifyFinishAction ();
			/* make sure text is not visible */
			alphaRect.transform.position = new Vector3(-Screen.width / 20.0f - 160.0f-Screen.width-600.0f, 0, 0);
		}
	}

	public void close()
	{
		if (chispaIsOut == true) 
		{
			chispaIsOut = false;
			if(chispaWorld != null) chispaWorld._wa_disappear (this);
			if ((level != null) && (closeSound != null))
				level.playSound (closeSound);
			this.isWaitingForActionToComplete = true;
			state = UIChispAlertState.waitingForClose;
			backdrop.GetComponent<Image> ().raycastTarget = false;
			theText.raycastTarget = false;
			ChispaLeftToRight = true;

			theText.text = ""; // cheap version

			backdrop.fadeIn ();

			state = UIChispAlertState.dismissed;

			return;
		}
	}

	public void _wa_alert(WisdominiObject wait, string msg) 
	{
		alert (msg);
		wait.isWaitingForActionToComplete = true;
		waitingRef = wait;
	}
}
