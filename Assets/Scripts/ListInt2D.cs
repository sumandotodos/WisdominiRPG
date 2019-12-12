using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Wrapper class for serialization */

[System.Serializable]
public class ListInt2D {

	public List<ListInt1D> theList;

	public ListInt2D () {

		theList = new List<ListInt1D>();

	}

}