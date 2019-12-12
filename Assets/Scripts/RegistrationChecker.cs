using UnityEngine;
using System.Collections;

public class RegistrationChecker : MonoBehaviour {

	WWW www;
	int nullFrames;

	MasterControllerScript masterController;

	const int MaxNullFrames = 6000;

	// Use this for initialization
	void Start () {


	
	}

	public void initialize() {

		masterController = GameObject.Find("MasterController").GetComponent<MasterControllerScript>();

		string uuid = SystemInfo.deviceUniqueIdentifier;

		WWWForm form = new WWWForm ();
		form.AddField ("uuid", uuid);

		string script = Utils.WisdominiServer + "/checkConfirmation";
		www = new WWW (script, form);

		nullFrames = 0;

	}
	
	// Update is called once per frame
	void Update () {

		if (www != null) {
			if (www.isDone) {
				bool res;
				bool.TryParse (www.text, out res);
				if (res == true) {
					masterController.getStorage ().storeBoolValue ("UserIsConfirmed", true);
					masterController.saveGame (false);
				} else {
					// tough titty
				}
				Destroy (this.gameObject);
			}

		} else {
			++nullFrames;
		}

		if (nullFrames >= MaxNullFrames)
			Destroy (this.gameObject);
	
	}
}
