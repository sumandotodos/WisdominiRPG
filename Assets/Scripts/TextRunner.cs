using UnityEngine;
using System.Collections;

public class TextRunner : MonoBehaviour {

	public ThoughtText target;
	public int index;
	public MatrixCorridorController controller;

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player") {
			target.autorun = true;
		}

		string msg = "nivel1-" + index;
		//Utils.queueMessage (msg);
		Utils.queueMessage(msg);

	}
}
