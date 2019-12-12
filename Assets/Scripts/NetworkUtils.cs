using UnityEngine;
using System.Collections;

public class NetworkUtils : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void queueMessage(string msg) {

		string uuid = SystemInfo.deviceUniqueIdentifier;
		GameObject MailQueueGO = new GameObject ();
		MailQueueGO.name = "MailQueueAgent";
		MailQueueGO.AddComponent<QueueMailAgent> ().initialize (uuid, msg);
		DontDestroyOnLoad (MailQueueGO);


	}
}
