using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeleccionarMago : MonoBehaviour {

	public GameObject[] magos;
	int numMago;
	LevelControllerScript lvl;

	void Start () 
	{
		for (int i = 0; i < magos.Length; i++) 
		{
			magos [i].SetActive(false);
		}

		lvl = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		numMago = lvl.retrieveIntValue ("MagoElegido");

		magos [numMago].SetActive (true);
	}
}
