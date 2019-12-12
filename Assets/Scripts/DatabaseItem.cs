using UnityEngine;
using System.Collections;

public class DatabaseItem {

	const int MAXINTDATA = 20;
	const int MAXFLOATDATA = 20;

	int[] intData;
	float[] floatData;

	void Start() {

		intData = new int[20];

	}

	void setIntData(int index, int data) {

		intData [index] = data;

	}

	int getIntData(int index) {

		return intData [index];

	}

	void setFloatData(int index, float data) {

		floatData [index] = data;

	}

	float getFloatData(int index) {

		return floatData [index];

	}
		

}
