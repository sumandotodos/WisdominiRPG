using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaderItween : MonoBehaviour {

	public Color initial;
	public Color final;
	public float timeTo;
	public bool action;

	Renderer rend;

	void Start () 
	{
		rend = this.GetComponent<Renderer> ();
		rend.material.color = initial;
	}
	
	void Update () 
	{
		if (action)
		iTween.ColorUpdate (this.gameObject, final, timeTo);
	}
}
