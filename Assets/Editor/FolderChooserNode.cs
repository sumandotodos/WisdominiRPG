#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FolderChooserNode : BaseNode {

	int selection = 0;

	NodeEditor nodeEditor;

	string newFolder;

	public override void DrawWindow() {

		List<string> dirs = new List<string>();

		string path = Application.dataPath + "/Resources/Prefabs/NodeEditorObjects/";

		DirectoryInfo info = new DirectoryInfo(path);

		DirectoryInfo[] dirInfo = info.GetDirectories ();

		//DirectoryInfo[] fileInfo = info.GetDirectories ();

		dirs.Add (".");
		foreach (DirectoryInfo dir in dirInfo) {

			dirs.Add (dir.Name);

		}

		newFolder = EditorGUILayout.TextField (newFolder);
		if (GUILayout.Button ("Create new folder")) {
			if (!newFolder.Equals ("")) {
				AssetDatabase.CreateFolder ("Assets/Resources/Prefabs/NodeEditorObjects", newFolder);
				newFolder = "";
			}
		}

		int selection2 = EditorGUILayout.Popup (selection, dirs.ToArray());
		if (selection2 != selection) {
			nodeEditor.updateFolder (dirs[selection2]);
		}

		selection = selection2;
	
	}

	public void setNodeEditor(NodeEditor w) {

		nodeEditor = w;

	}

	public override void DrawCurves() {



	}
}

#endif