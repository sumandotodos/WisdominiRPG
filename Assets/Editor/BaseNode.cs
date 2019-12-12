#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public struct NippleRef {

	public int instruction;
	public int answer;

}

public abstract class BaseNode : ScriptableObject {
	
	public Rect windowRect;

	public bool hasInputs = false;

	public string windowTitle = "";

	public List<Rect> outputNipples;
	public List<NippleRef> nipples;
	public List<Rect> inputNipples;
	public List<int> instructionOfOutNipple;
	public List<int> instructionOfInNipple;
	public List<int> opOfNipple;

	public virtual void DrawWindow() {

		//windowTitle = EditorGUILayout.TextField("Title", windowTitle);

	}

	public abstract void DrawCurves();

	/*public virtual void SetInput(BaseInputNode input, Vector2 clickPos) {



	}


	public virtual void NodeDeleted(BaseNode node) {


	}

	public virtual BaseInputNode ClickedOnInput(Vector2 pos) {

		return null;

	}*/
	

}

#endif