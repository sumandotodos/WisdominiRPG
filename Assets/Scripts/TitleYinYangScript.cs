using UnityEngine;
using System.Collections;

public class TitleYinYangScript : MonoBehaviour {

	public Vector3 angulo;

	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
	
		this.transform.Rotate(angulo);

	}
}
