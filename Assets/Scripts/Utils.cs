using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour {

	/* constants */

	public const string WisdominiServer = "http://server.wisdomini.org";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void mecagoentodo() {

	}



	public static bool updateSoftVariable(ref float val, float target, float speed) {

		bool hasChanged = false;

		if (val < target) {
			val += speed * Time.deltaTime;
			hasChanged = true;
			if (val > target)
				val = target;
		}

		if (val > target) {
			val -= speed * Time.deltaTime;
			hasChanged = true;
			if (val < target)
				val = target;
		}


		return hasChanged;

	}

	public static void queueMessage(string msg) {

		string uuid = SystemInfo.deviceUniqueIdentifier;
		GameObject MailQueueGO = new GameObject ();
		MailQueueGO.name = "MailQueueAgent";
		MailQueueGO.AddComponent<QueueMailAgent> ().initialize (uuid, msg);
		DontDestroyOnLoad (MailQueueGO);


	}

	public static int indexOfStringInList(string[] list, string str) {

		for (int i = 0; i < list.Length; ++i) {
			if (str.Equals (list [i]))
				return i;
		}
		return 0;

	}


}
