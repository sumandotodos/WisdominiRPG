using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QAIngest : MonoBehaviour {

	[HideInInspector]
	public bool fileLoaded = false;

	[HideInInspector]
	public string fileContents;

	public string outputFolder;

	public void loadFile(string contents) {
		fileContents = contents;
		fileLoaded = true;
	}



}
