using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class SerializableDictionary {

	public List<string> bKey;
	public List<bool> bValue;

	public List<string> iKey;
	public List<int> iValue;

	public List<string> fKey;
	public List<float> fValue;

	public List<string> sKey;
	public List<string> sValue;

	public SerializableDictionary() {

		bKey = new List<string>();
		bValue = new List<bool>();

		iKey = new List<string>();
		iValue = new List<int>();

		fKey = new List<string>();
		fValue = new List<float>();

		sKey = new List<string>();
		sValue = new List<string>();

	}

}
