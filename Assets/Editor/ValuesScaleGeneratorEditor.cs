#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ValuesScaleGenerator))]
public class ValuesScaleGeneratorEditor : Editor {



	public override void OnInspectorGUI() {

		ValuesScaleGenerator sRef = (ValuesScaleGenerator)target;

		DrawDefaultInspector ();

		if (GUILayout.Button ("Update prefab")) {

			sRef.rosetta.registerString ("ValuesScale" + name + "Item1", sRef.Item1);
			sRef.rosetta.registerString ("ValuesScale" + name + "Item2", sRef.Item2);
			sRef.rosetta.registerString ("ValuesScale" + name + "Item3", sRef.Item3);
			sRef.rosetta.registerString ("ValuesScale" + name + "Item4", sRef.Item4);
			sRef.rosetta.registerString ("ValuesScale" + name + "Item5", sRef.Item5);

		}

	}


}

#endif