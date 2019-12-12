using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VersionCheckController : MonoBehaviour {

	public StringBank stringBank;
	public Text text1, text2, text3;
	public UITextFader fader1, fader2, fader3;
	int state = 0;
	MasterControllerScript mcRef;
	float elapsedTime;

	// Use this for initialization
	void Start () {
		
		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		text1.color = new Color (1, 1, 1, 0);
		text2.color = new Color (1, 1, 1, 0);
		text3.color = new Color (1, 1, 1, 0);
		state = 1;
		//text1.text = stringBank.getStr
	
	}

	// Update is called once per frame
	void Update () {
	
		if (state == 0) { // idle

		}
		if (state == 1) {
			bool newv = Version.newVersionAvailable ();
			if (newv) {
				++state;
			} else {
				mcRef.setActivityFinished ();
			}
		}
		if (state == 2) {
			fader1.fadeIn ();
			fader2.fadeIn ();
			fader3.fadeIn ();
			++state;
		}
		if (state == 3) {
			if (Input.GetMouseButtonDown (0)) {
				elapsedTime = 0.0f;
				fader1.fadeOut ();
				fader2.fadeOut ();
				fader3.fadeOut ();
				++state;
			}
		}
		if (state == 4) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 2.0f) {
				++state;
			}
		}
		if (state == 5) {
			mcRef.setActivityFinished ();
		}

	}
}
