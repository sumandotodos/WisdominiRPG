using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepButtonController : MonoBehaviour {

	public int key;
	public bool[] data;

	public WisdominiObject successProgram_N;

	public bool watchMeSucceed = false;
	public int watchMeCompute = 0;

	public bool stopOnSuccess = true;
	[HideInInspector]
	public bool succeded;

	// Use this for initialization
	void Start () {
		succeded = false;
	}

	private bool checkKey() {
		int total = 0;
		for (int i = 0; i < data.Length; ++i) {
			int addThis = 1;
			for (int j = 0; j < i; ++j) {
				addThis = (addThis) << 1;
			}
			if (data [i])
				total += addThis;
		}
		watchMeCompute = total;
		if (total == key) {
			succeded = true;
			return true;
		}
		else
			return false;
	}

	public void toggleData(int pos) {
		if (stopOnSuccess && succeded)
			return;
		data [pos] = !data [pos];
		if (checkKey()) {
			watchMeSucceed = true;
			if (successProgram_N != null) {
				successProgram_N.startProgram (0);
			}
		}
	}
}
