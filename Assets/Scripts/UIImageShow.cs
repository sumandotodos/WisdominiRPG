using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIImageShow : WisdominiObject {

	RawImage img;
	bool isEnabled;

	// Use this for initialization
	void Start () {
	
		img = this.GetComponent<RawImage> ();
		float scale = img.rectTransform.sizeDelta.x;
		img.transform.localScale = new Vector3 (Screen.width / Screen.height, 1, 1);
		img.enabled = false;
		isEnabled = false;

	}

	public void _wa_show(WisdominiObject waitRef) {
		waitRef.isWaitingForActionToComplete = true;
		_wm_show ();
	}

	public void _wm_show() {
		img.enabled = true;
		isEnabled = true;
	}

	public void _wm_hide() {
		img.enabled = false;
		isEnabled = false;
	}

	// Update is called once per frame
	void Update () {
		if (!isEnabled)
			return;
		if (Input.GetMouseButtonDown (0)) {
			_wm_hide ();
			notifyFinishAction ();
		}
	}
}
