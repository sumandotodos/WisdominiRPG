using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMontanaMago : MonoBehaviour {

	public GameObject puerta;
	public GameObject portal;
	public GameObject logger;
	public string mapa;

	LevelControllerScript lvl;

	void Start () 
	{
		lvl = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		if (lvl.retrieveBoolValue (mapa)) {
			puerta.SetActive (false);
			portal.SetActive (true);
			logger.SetActive (true);
		} else {
			puerta.SetActive (true);
			portal.SetActive (false);
			logger.SetActive (false);
		}
	}
}
