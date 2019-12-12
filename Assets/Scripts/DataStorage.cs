using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum DataType { Int, Float, String, Bool };

[Serializable]
public class DataStorage : MonoBehaviour {

	public SerializableDictionary serial;

	public Dictionary<string, int> iDict;
	public Dictionary<string, float> fDict;
	public Dictionary<string, string> sDict;
	public Dictionary<string, bool> bDict;

	const int DEFAULTINT = 0;
	const float DEFAULTFLOAT = 0.0f;
	const string DEFAULTSTRING = "";
	const bool DEFAULTBOOL = false;

	bool initialized = false;

	// Use this for initialization
	void Start () {
	
		if (!initialized)
			initialize ();


	}

	public void initialize() {

		iDict = new Dictionary<string, int>();
		fDict = new Dictionary<string, float> ();
		sDict = new Dictionary<string, string> ();
		bDict = new Dictionary<string, bool> ();

		serial = new SerializableDictionary ();

		initialized = true;

	}

	public int retrieveIntValue(string key) {

		int result;
		if(iDict.TryGetValue(key, out result)) {
			return result;
		}
		else {
			return DEFAULTINT;
		}



	}

	public void storeIntValue(string key, int value) {

		iDict[key] = value;

		serial.iKey.Add (key);
		serial.iValue.Add (value);

	}

	public float retrieveFloatValue(string key) {

		float result;
		if(fDict.TryGetValue(key, out result)) {
			return result;
		}
		else {
			return DEFAULTFLOAT;
		}



	}

	public void storeFloatValue(string key, float value) {

		fDict[key] = value;

		serial.fKey.Add (key);
		serial.fValue.Add (value);

	}

	public string retrieveStringValue(string key) {

		string result;
		if(sDict.TryGetValue(key, out result)) {
			return result;
		}
		else {
			return DEFAULTSTRING;
		}



	}

	public void storeStringValue(string key, string value) {

		sDict[key] = value;

		serial.sKey.Add (key);
		serial.sValue.Add (value);

	}

	public bool retrieveBoolValue(string key) {

		bool result;
		if(bDict.TryGetValue(key, out result)) {
			return result;
		}
		else {
			return DEFAULTBOOL;
		}



	}

	public void storeBoolValue(string key, bool value) {

		bDict[key] = value;

		serial.bKey.Add (key);
		serial.bValue.Add (value);

	}
}
