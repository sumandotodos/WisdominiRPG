using UnityEngine;
using System.Collections;

public class QueueMailAgent : MonoBehaviour {

	WWW www;
	int nullFrames;

	string uuid, contents;

	const int MaxNullFrames = 6000;

	// Use this for initialization
	void Start () {



	}

	public void initialize(string newuuid, string newcontents) {

		uuid = newuuid;
		contents = newcontents;

		WWWForm form = new WWWForm ();
		form.AddField ("uuid", uuid);
		form.AddField ("contid", contents);

		string script = Utils.WisdominiServer + "/queueMail";
		www = new WWW (script, form);

		nullFrames = 0;

	}

	// Update is called once per frame
	void Update () {

		if (www != null) {
			if (www.isDone) {

				www.Dispose ();
				www = null;
				Destroy (this.gameObject);
			}

		} else {
			++nullFrames;
		}

		if (nullFrames >= MaxNullFrames)
			Destroy (this.gameObject);

	}
}
