using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Wrapper class for serialization */

[System.Serializable]
public class ListString2D {

	public List<ListString1D> theList;

	public ListString2D () {

		theList = new List<ListString1D>();

	}

}