using UnityEngine;
using System.Collections;

[System.Serializable]
public class RGBColorItem : System.Object {

	public float R, G, B;

	float toGreyScale() {

		return (R + G + B) / 3.0f;

	}

}
