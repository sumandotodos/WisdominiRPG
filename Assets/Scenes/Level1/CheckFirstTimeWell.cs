using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFirstTimeWell : MonoBehaviour {

	alertImageController alert;
	CharacterGenerator charGen;
	Renderer rend;
	MasterControllerScript mcref;
	string estado = "";
	bool started;
	bool finished;
	float contador;

	void Awake()
	{
		rend = this.GetComponentInChildren<Renderer> ();
		//rend.material.color = new Color (1, 1, 1, 0);
		rend.sharedMaterial.color = new Color (1, 1, 1, 0);
	}

	void Start()
	{
		alert = GameObject.Find ("AlertImage").GetComponent<alertImageController> ();
		charGen = this.GetComponent<CharacterGenerator> ();
		mcref = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();

	}

	void Initializate () 
	{
		estado = "start";
	}
	
	void Update () 
	{		
		contador += Time.deltaTime;

		if (contador > 3 && estado == "") 
		{
			Initializate ();
		} 

		if (mcref != null) {
			started = mcref.getStorage ().retrieveBoolValue ("ConversationManaDone");
			finished = mcref.getStorage ().retrieveBoolValue ("PozoSabiduriaFinished");
		}
			
		if ((alert.state == alertImageController.State.Closed) && (estado == "start")) 
		{
			charGen.startProgram (0);
			estado = "checked";
		}

		if (!finished && started && estado == "checked") 
		{
			FadeIn ();
		}			

		if (finished) 
		{
			FadeOut ();
		}
	}

	void FadeIn()
	{
		iTween.ColorUpdate(this.transform.GetChild(0).gameObject, new Color (1, 1, 1, 1), 3);
	}

	void FadeOut()
	{
		iTween.ColorUpdate(this.transform.GetChild(0).gameObject, new Color (1, 1, 1, 0), 3);
	}
}
