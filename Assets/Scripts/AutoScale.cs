using UnityEngine;
using System.Collections;

public class AutoScale : MonoBehaviour {

	const float magic = 1.86f;

	// Use this for initialization
	void Start () {

		float w = Screen.width;
		float h = Screen.height;
		float aspect = w / h;
		this.transform.localScale = new Vector3 (aspect / magic, 1, 1);
	
	}
	

}
