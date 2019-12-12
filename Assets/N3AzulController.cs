using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N3AzulController : WisdominiObject {

	public bool[] accomplished;
	public FakePanel fakePanel;

	// Use this for initialization
	void Start () {
		accomplished = new bool[3];
	}
	
	public void _wm_toggleAccomplishment(int position) {
		accomplished [position] = !accomplished [position];
		bool allDone = true;
		foreach (bool c in accomplished) {
			if (c == false) {
				allDone = false;
				break;
			}
		}
		if (allDone) {
			fakePanel._wm_open ();
		}
	}
}
