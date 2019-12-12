#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(WarriorsActivityController))]
public class WarriorsActivityControllerEditor : Editor  {

	public override void OnInspectorGUI() {

		int globalStrIndex = 0;

		Rosetta rosetta;

		DrawDefaultInspector ();

		WarriorsActivityController warRef = (WarriorsActivityController)target;

		EditorGUILayout.LabelField ("Number of descriptions: " + warRef.IndividualDescriptions.Length);

		rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();

		if (GUILayout.Button ("Update Rosetta")) {
			for (int i = 0; i < warRef.ClassDescription.Length; ++i) {

				rosetta.registerString ("WarriorsActContClassDesc" + i, warRef.ClassDescription [i]);
				globalStrIndex++;

			}
			for (int i = 0; i < warRef.IndividualDescriptions.Length; ++i) {

				rosetta.registerString ("WarriorsActContIndDesc" + i, warRef.IndividualDescriptions [i]);
				globalStrIndex++;

			}
			for (int i = 0; i < warRef.ClassNames.Length; ++i) {

				rosetta.registerString ("WarriorsActContClassNames" + i, warRef.ClassNames [i]);
				globalStrIndex++;

			}
			for (int i = 0; i < warRef.IndividualNames.Length; ++i) {

				rosetta.registerString ("WarriorsActContIndNames" + i, warRef.IndividualNames [i]);
				globalStrIndex++;

			}
		}


	}

}

#endif