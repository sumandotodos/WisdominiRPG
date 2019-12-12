using UnityEngine;
using System.Collections;

public class Freezer : WisdominiObject  {

	public bool frozen;

	// Use this for initialization
	new void Start () {
	
		frozen = false;

	}
	
	public void _wm_freeze() {

		frozen = true;

	}

	public void _wm_unfreeze() {

		frozen = false;
	}
}
