using UnityEngine;
using System.Collections;

public class ActivarHud : MonoBehaviour {

	public GameObject hud;

	// Use this for initialization
	void Start () {
        hud = GameObject.Find("HUD");

		hud.gameObject.SetActive (true);
	
	}
	

}
