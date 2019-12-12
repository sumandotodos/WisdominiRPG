using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserDetails : MonoBehaviour {

	/* references */

	public Text theField;

	MasterControllerScript mcRef;

	WWW www;

	int state;

	/* constants */



	// Use this for initialization
	void Start () {
	
		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		if (mcRef.getStorage ().retrieveBoolValue ("UserIsConfirmed")) {
			finishActivity ();
		}
		string uuid = SystemInfo.deviceUniqueIdentifier;

		WWWForm form = new WWWForm ();
		form.AddField ("uuid", uuid);

		string script = Utils.WisdominiServer + "/checkConfirmation";
		www = new WWW (script, form);


		state = 0;

	}
	
	// Update is called once per frame
	void Update () {

		if (www != null) {
			if (www.isDone) {
				if (state == 0) {
					bool res;
					bool.TryParse (www.text, out res);
					if (res == true) {
						mcRef.getStorage ().storeBoolValue ("UserIsConfirmed", true);
						mcRef.saveGame (false);
						mcRef.setActivityFinished ();
					} else {

					}
					www = null;
					state = 1;
				} else { 
					
					finishActivity ();
					www = null;
				}
			}
			
		}
	
	}

	public void registerUser() {

		string uuid = SystemInfo.deviceUniqueIdentifier;

		WWWForm form = new WWWForm ();
		form.AddField ("uuid", uuid);
		form.AddField ("email", theField.text);

		string script = Utils.WisdominiServer + "/newUser";
		www = new WWW (script, form);
		// enough??

		//mcRef.setActivityFinished ();

		return;

	}

	public void finishActivity() {

		GameObject checkerGO = new GameObject();
		RegistrationChecker checker;
		checkerGO.name = "Registrarion Checker";
		checker = checkerGO.AddComponent<RegistrationChecker> ();
		checker.initialize ();
		//checkerGO.transform.parent = mcRef.gameObject.transform;
		DontDestroyOnLoad (checkerGO);

		mcRef.setActivityFinished ();

	}

}
