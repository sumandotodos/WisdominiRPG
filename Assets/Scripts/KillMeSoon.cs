using UnityEngine;
using System.Collections;

public class KillMeSoon : MonoBehaviour {

	public MirrorAnimAux m;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			m.Break ();
		}
	
	}
}
