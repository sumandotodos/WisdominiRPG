using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardaController : MonoBehaviour {

	MasterControllerScript mc;

	public CharacterGenerator[] guarda;
	public Vector3[] solvedPosition;
	public Vector3 smallOffset;

	public string prefix;

	// Use this for initialization
	void Start () {
		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		for (int i = 0; i < guarda.Length; ++i) {
			bool clear = mc.getStorage ().retrieveBoolValue (prefix+(i+1)+"Clear");
			if (clear)
				solveGuardia (i);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void solveGuardia(int n) {

		guarda [n].transform.position = solvedPosition [n] + smallOffset;
		guarda [n]._wm_startEventByName ("customEvent1");

	}
}
