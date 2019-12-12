using UnityEngine;
using System.Collections;
//using UnityEditor;
using UnityEngine.UI;

public class HUDController : WisdominiObject {

	/* references */

	public Text alertTextRef;
	public Image alertImageRef;
	GameObject cameraPivotY;
	bool responseAvailable;
	int response;
	public RawImage YESIconRef;
	public RawImage NOIconRef;
	public Text Response1Text;
	public Text Response2Text;
	public Text Response3Text;
	public Text Response4Text;
	public Text Response5Text;
	public Text Response6Text;
	public alertImageController alertController;
	public LevelControllerScript level;
	public AudioClip userInputSound;

	Vector3 touchPos;
	bool touching;

	bool firstFrame = true;
	float cameraAngle;

	const float ATOMARPORCULO = 10000.0f;

	public PlayerScript playerRef;

	int multiDigitResponse = 0;
	int multiDigitResponsePosition = 0;

	//alertImageController imCtrl;

	new void Start () {
	
		cameraPivotY = GameObject.Find ("CameraPivotY");
		//imCtrl = alertImageRef.GetComponent<alertImageController> ();
		//imCtrl.reset ();
		//imCtrl.setAlertMessage ("Esto no está nada bien...");
		//imCtrl.setSelfTimeout (15.0f);
		touching = false;
		disableResponseIcons ();
		disableNumericResponseIcons ();

	}

	public void select1() {
		response = 1;
		multiDigitResponse += response * ((int)Mathf.Pow (10, (float)multiDigitResponsePosition));
		if(multiDigitResponsePosition > 0)
		--multiDigitResponsePosition;
		responseAvailable = true;
		if(alertController != null) {
			alertController.close ();
		}
		if ((level != null) && (userInputSound != null)) {
			level.playSound (userInputSound);
		}
	}

	public void select2() {
		response = 2;
		multiDigitResponse += response * ((int)Mathf.Pow (10, (float)multiDigitResponsePosition));
		if(multiDigitResponsePosition > 0)
			--multiDigitResponsePosition;
		responseAvailable = true;
		if(alertController != null) {
			alertController.close ();
		}
		if ((level != null) && (userInputSound != null)) {
			level.playSound (userInputSound);
		}
	}

	public void select3() {
		response = 3;
		multiDigitResponse += response * ((int)Mathf.Pow (10, (float)multiDigitResponsePosition));
		if(multiDigitResponsePosition > 0)
			--multiDigitResponsePosition;
		responseAvailable = true;
		if(alertController != null) {
			alertController.close ();
		}
		if ((level != null) && (userInputSound != null)) {
			level.playSound (userInputSound);
		}
	}

	public void select4() {
		response = 4;
		multiDigitResponse += response * ((int)Mathf.Pow (10, (float)multiDigitResponsePosition));
		if(multiDigitResponsePosition > 0)
			--multiDigitResponsePosition;
		responseAvailable = true;
		if(alertController != null) {
			alertController.close ();
		}
		if ((level != null) && (userInputSound != null)) {
			level.playSound (userInputSound);
		}
	}

	public void select5() {
		response = 5;
		multiDigitResponse += response * ((int)Mathf.Pow (10, (float)multiDigitResponsePosition));
		if(multiDigitResponsePosition > 0)
			--multiDigitResponsePosition;
		responseAvailable = true;
		if(alertController != null) {
			alertController.close ();
		}
		if ((level != null) && (userInputSound != null)) {
			level.playSound (userInputSound);
		}
	}

	public void select6() {
		response = 6;
		multiDigitResponse += response * ((int)Mathf.Pow (10, (float)multiDigitResponsePosition));
		if(multiDigitResponsePosition > 0)
			--multiDigitResponsePosition;
		responseAvailable = true;
		if(alertController != null) {
			alertController.close ();
		}
		if ((level != null) && (userInputSound != null)) {
			level.playSound (userInputSound);
		}
	}

	public void selectYES() {

		response = 1;
		responseAvailable = true;
		if(alertController != null) {
			alertController.close ();
		}
		if ((level != null) && (userInputSound != null)) {
			level.playSound (userInputSound);
		}

	}

	public void selectNO() {

		response = -1;
		responseAvailable = true;
		if(alertController != null) {
			alertController.close ();
		}
		if ((level != null) && (userInputSound != null)) {
			level.playSound (userInputSound);
		}

	}

	public int _wm_getResponse() {
		return getResponse ();
	}

	public int _wm_getMultiDigitResponse() {
		return multiDigitResponse;
	}

	public void _wm_disableResponseIcons() {
		disableResponseIcons ();
		alertController._wm_allowCloseOnClick ();
	}
	public void _wm_enableResponseIcons() {
		enableResponseIcons ();

	}

	public void _wm_disableNumericResponseIcons() {
		disableNumericResponseIcons ();

	}
	public void _wm_enableNumericResponseIcons() {
		enableNumericResponseIcons ();

	}

	public void enableResponseIcons() {
		alertController._wm_preventCloseOnClick ();
		if (YESIconRef != null) {
			YESIconRef.enabled = true;
		}
		if (NOIconRef != null) {
			NOIconRef.enabled = true;
		}

	}

	public void enableNumericResponseIcons() {
		alertController._wm_preventCloseOnClick ();
		if (Response1Text != null) {
			Response1Text.enabled = true;
		}
		if (Response2Text != null) {
			Response2Text.enabled = true;
		}
		if (Response3Text != null) {
			Response3Text.enabled = true;
		}
		if (Response4Text != null) {
			Response4Text.enabled = true;
		}
		if (Response5Text != null) {
			Response5Text.enabled = true;
		}
		if (Response6Text != null) {
			Response6Text.enabled = true;
		}
		multiDigitResponse = 0;
		multiDigitResponsePosition = 3;

	}

	public void disableNumericResponseIcons() {
		alertController._wm_allowCloseOnClick ();
		if (Response1Text != null) {
			Response1Text.enabled = false;
		}
		if (Response2Text != null) {
			Response2Text.enabled = false;
		}
		if (Response3Text != null) {
			Response3Text.enabled = false;
		}
		if (Response4Text != null) {
			Response4Text.enabled = false;
		}
		if (Response5Text != null) {
			Response5Text.enabled = false;
		}
		if (Response6Text != null) {
			Response6Text.enabled = false;
		}
		multiDigitResponse = 0;
		multiDigitResponsePosition = 3;

	}

	public void _wm_setMultidigitResponseLength(int l) {
		multiDigitResponsePosition = 3;
	}

	public void disableResponseIcons() {
		alertController._wm_allowCloseOnClick ();
		if (YESIconRef != null) {
			YESIconRef.enabled = false;
		}
		if (NOIconRef != null) {
			NOIconRef.enabled = false;
		}

	}

	/*
	 * 
	 * This method will be pooled every frame to know if the user chose a selection:
	 * Responses:  1 = the user clicked on OK     -1 = the user clicked on NO_OK     0 = nothing yet
	 * 
	 */
	public int getResponse() {

		if (responseAvailable) {
			responseAvailable = false;
			return response;
		} else {
			return 0;
		}

	}

	public float touchHorizontal() {

		if (!touching) {
			firstFrame = true;
			return 0.0f;
		}
		if (firstFrame) {
			firstFrame = false;
			if (cameraPivotY != null)
				cameraAngle = -cameraPivotY.transform.localEulerAngles.y;
			else
				cameraAngle = 0.0f;
		}
		Vector3 newPos = Input.mousePosition;
		Vector3 displ = (newPos - touchPos)/((float)Screen.height);
		displ = Quaternion.AngleAxis (cameraAngle, Vector3.forward) * displ;
			
		if (displ.x > 0) { // positive
			if (displ.x < 0.03f)
				return 0.0f;
			if (displ.x > 0.1f)
				return 0.1f;
		} else { // negative
			if(displ.x > -0.03f) return 0.0f;
			if (displ.x < -0.1f)
				return -0.1f;
		}

		return displ.x;

	}

	public float touchVertical() {

		if (!touching) {
			firstFrame = true;
			return 0.0f;
		}
		if (firstFrame) {
			firstFrame = false;
			if (cameraPivotY != null)
				cameraAngle = -cameraPivotY.transform.localEulerAngles.y;
			else
				cameraAngle = 0.0f;
		}
		Vector3 newPos = Input.mousePosition;
		Vector3 displ = (newPos - touchPos)/((float)Screen.height);
		displ = Quaternion.AngleAxis (cameraAngle, Vector3.forward) * displ;

		if (displ.y > 0) { // positive
			if (displ.y < 0.03f)
				return 0.0f;
			if (displ.y > 0.1f)
				return 0.1f;
		} else { // negative
			if(displ.y > -0.03f) return 0.0f;
			if (displ.y < -0.1f)
				return -0.1f;
		}

		return displ.y;

	}
	new void Update () {

		if (Input.GetMouseButtonDown (0)) {
			touchPos = Input.mousePosition;
			touching = true;
		}

		if (Input.GetMouseButtonUp (0)) {

			touching = false;
		}
	}
}
