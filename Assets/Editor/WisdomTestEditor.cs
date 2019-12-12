#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(WisdomTest))]
public class WisdomTestEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		WisdomTest wisRef = (WisdomTest)target;
		//MyTest.myTextArea = EditorGUILayout.TextArea(MyTest.myTextArea);
		EditorGUILayout.TextArea("" + wisRef.mecagoentodo);
		if (GUILayout.Button ("Generate")) {
			wisRef.mecagoentodo = Random.Range (0, 512);
			EditorUtility.SetDirty(wisRef);
			EditorSceneManager.MarkSceneDirty (wisRef.gameObject.scene);
		}
		if (GUILayout.Button ("Generate 2")) {
			wisRef.otracosilla = Random.Range (0, 512);
			EditorUtility.SetDirty(wisRef);
			EditorSceneManager.MarkSceneDirty (wisRef.gameObject.scene);
		}
		if (GUI.changed) { EditorUtility.SetDirty(wisRef); }
	}
}

#endif