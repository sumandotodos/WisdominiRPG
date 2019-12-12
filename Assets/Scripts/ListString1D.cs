using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Wrapper class for serialization */

[System.Serializable]
public class ListString1D {

	public List<string> theList;

	public ListString1D () {

		theList = new List<string>();

	}

}