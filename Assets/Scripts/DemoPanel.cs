using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DemoPanel : WisdominiObject {

	/* references */
	RawImage img;
	public Text theText;
	//public Text otherText;
	public UIFaderScript fader;

	/* public properties */
	public float maxY;
	public float minY;
	public float speed;
	public string loopScene;

	public SoftFloat yPos;


	/* properties */
	float Y;
	Vector3 originalPos;
	int state; // slot 0 state
	float elapsedTime;
	float opacity;


	/* constants */
	const float INITIALDELAY = 3.0f;
	const float DELAY2 = 2.0f;
	const float targetOpacity = 1.0f;
	const float opacitySpeed = 0.4f;


	/* methods */
	// Use this for initialization
	new void Start () {
		img = this.GetComponent<RawImage> ();
		Y = minY;
		yPos = new SoftFloat (Y);
		yPos.setSpeed (30.3f);
		yPos.setTransformation (TweenTransforms.tanh);
		originalPos = img.transform.localPosition;
		img.transform.localPosition = originalPos + new Vector3(0, Y, 0);
		state = 0;
		theText.color = new Color (1, 1, 1, 0);
		//otherText.color = new Color (0, 0, 0, 0);
		state = 1;
	}
	
	// Update is called once per frame
	new void Update () {
	
		if (state == 0) { // idle state

		}

		if (state == 1) { // initial delay
			elapsedTime += Time.deltaTime;
			if (elapsedTime > INITIALDELAY) {
				yPos.setValue (maxY);
				++state;
			}
		}

		if (state == 2) { // scrolling the rawimage

			bool change = yPos.update ();//Utils.updateSoftVariable (ref Y, maxY, speed);

			if (change) {
				img.transform.localPosition = originalPos + new Vector3 (0, yPos.getValue(), 0);
			} else {
				++state;
				elapsedTime = 0.0f;
			}
		}

		if (state == 3) { // a second delay
			elapsedTime += Time.deltaTime;
			if (elapsedTime > DELAY2) {
				++state;
			}
		}

		if (state == 4) { // fading the text in
			bool change = Utils.updateSoftVariable(ref opacity, targetOpacity, opacitySpeed);
			if (change) {
				theText.color = new Color (1, 1, 1, opacity);
				//otherText.color = new Color (0, 0, 0, opacity);
			} else {
				elapsedTime = 0.0f;
				++state;
			}
		}

		if (state == 5) { // waiting for touch event
			if (Input.GetMouseButtonDown (0)) {
				++state;
				fader._wa_fadeOut (this);
				this.isWaitingForActionToComplete = true;
			}
		}

		if (state == 6) { // fading out
			if (!isWaitingForActionToComplete) 
			{				
				System.IO.File.Delete(Application.persistentDataPath + "/save000.dat");
				++state;
			}
		}

		if (state == 7) {
			SceneManager.LoadScene (loopScene);
		}

	}
}
