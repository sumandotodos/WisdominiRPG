#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;

public abstract class GenericNode : ScriptableObject {

	public Rect nodeRect;
	public bool hasInputs = false;
	public string nodeTitle = "";

	public virtual void DrawNode() {

		nodeTitle = EditorGUILayout.TextField ("Title", nodeTitle, GUILayout.Width(200));

	}

	public abstract void DrawCurves();

}

#endif