using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameChamanaController : CharacterGenerator {

	public string[] grupo01;
	public string[] grupo02;
	public string[] grupo03;

	bool clave01 = true;
	bool clave02 = true;
	bool clave03 = true;

	public WisdominiObject conver01;
	public WisdominiObject conver02;
	public WisdominiObject conver03;
	public WisdominiObject conver04;

	public WisdominiObject currentConver;

	DataStorage ds;
	GameObject Chamana;

	new void Start()
	{
		ds = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ().getStorage ();
		Chamana = GameObject.Find ("Chamana");

		for (int i = 0; i < grupo01.Length; i++) 
		{
			if(!ds.retrieveBoolValue(grupo01[i]))
			{
				clave01 = false;
			}
		}

		for (int i = 0; i < grupo02.Length; i++) 
		{
			if(!ds.retrieveBoolValue(grupo02[i]))
			{
				clave02 = false;
			}
		}

		for (int i = 0; i < grupo03.Length; i++) 
		{
			if(!ds.retrieveBoolValue(grupo03[i]))
			{
				clave03 = false;
			}
		}

		// Faltaria el for de Siaomi -> clave04

		if (!clave01) {
			currentConver = conver01;
		} else {
			if (!clave02) {
				currentConver = conver02;
			} else {
				if (!clave03) {
					currentConver = conver03;
				} else {
					currentConver = conver04;
				}
			}
		}



		//Deshabilitar chamana si clave04 = true -> desactivar chamana, activa logger y portal y abrir puerta.
		// Faltarian referencias arriba.
	}

	public void _wm_GetConver()
	{
		WisdominiObject wisObj = currentConver.GetComponent<WisdominiObject> ();
		ListString2D newProg = wisObj.programs [0];
		programs = new List<ListString2D> ();
		currentProgram = null;
		programPointer_ = new List<int> ();
		programPointer_.Add (0);
		waitingForAnswer = new List<bool> ();
		eventNamesInOrder = new List<string> ();
		delayTime = new List<float> ();
		delayTime.Add (0.0f);
		isProgramRunning = new List<bool> ();
		isProgramRunning.Add (false);
		waitingForAnswer = new List<bool> ();
		waitingForAnswer.Add (false);
		programStacks = new List<Stack> ();
		programStacksPointers = new List<Stack> ();
		instructionColor = new List<ListInt1D> (); // a color for each instruction of each program
		programIsWaitingForActionToComplete = new List<bool> ();
		programIsWaitingForActionToComplete.Add (false);
		programPointer = 0;
		programs.Add (newProg);
		Stack newStack = new Stack ();
		programStacks.Add (newStack);
		Stack newProgramIndexStack = new Stack ();
		programStacksPointers.Add (newProgramIndexStack);
		originalPrograms.Add (programs[0]);
		this.startProgram (0);
	}

}
