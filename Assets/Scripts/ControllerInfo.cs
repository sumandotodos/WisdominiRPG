using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInfo : WisdominiObject {

	alertImageController aic;
	public StringBank frases;
	int lastNum;

	new void Start () 
	{
		aic = GameObject.Find ("AlertImage").GetComponent<alertImageController> ();
		lastNum = -1;
	}

	public void _wm_CallAlert()
	{
		int currentNum;

		do {
			currentNum = Random.Range (0, frases.phrase.Length);
		} while (currentNum == lastNum);

		lastNum = currentNum;

		aic._wm_setAlertMessageAndClose (frases.phrase [currentNum]);
	}
}
