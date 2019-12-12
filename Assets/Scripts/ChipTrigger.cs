using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipTrigger : MonoBehaviour {


	// PASAR AL CONTROLLER FERFUFLO A QUIEN HE TOCADO
	string currentCollider;

	void Start () 
	{
		
	}
	
	void Update () 
	{
		
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		currentCollider = other.name;
	}

	void OnTriggerExit2D (Collider2D other)
	{
		currentCollider = "ninguno";
	}

	public string SayName()
	{
		return currentCollider;
	}
}
