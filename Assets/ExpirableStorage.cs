using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpirableStorage : WisdominiObject {

	// all containable data
	bool BooleanData = false;
	int IntegerData = 0;
//	float RealData;
//	string StringData;

	public void _wm_storeBool(bool b) {
		BooleanData = b;
	}

	public bool _wm_queryBool() {
		return BooleanData;
	}

	public void _wm_storeInt(int i) {
		IntegerData = i;
	}

	public bool _wm_queryInt(int i) {
		return IntegerData == i;
	}

}
