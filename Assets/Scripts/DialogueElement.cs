using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum MiniatureLocation { left, middle, right };

public enum DialogueElementType { miniature, bubbleStart, bubbleMiddle, text, image, bubbleEnd };

[System.Serializable]
public class DialogueElement : Object {

	public Text theText {

		get;		set;
	}

	public DialogueElementType element { 

		get; 		set;

	}

	public string data {

		get;		set;
	}



}