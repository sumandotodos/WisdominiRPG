#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ObjectNode : BaseNode {

	public List<string> events;
	string newEvent = "";
	public string currentFolder;
	public GameObject theObject;
	public ConversationGenerator theWO;

	public virtual string getResult() {

		return "None";

	}

	public override void DrawWindow() {

		//base.DrawCurves ();

		EditorGUILayout.LabelField ("Name: ");
		windowTitle = EditorGUILayout.TextField (windowTitle, GUILayout.Width(120));


		if (GUILayout.Button ("Edit")) {
			ConversationNodeEditor window = CreateInstance<ConversationNodeEditor>();
			window.title = "Editing " + windowTitle;
			window.root = theWO;
			theObject.name = windowTitle;
			window.rootGO = theObject;
			window.folder = currentFolder;
			window.initialize ();

			window.Show();
		}

		if (GUILayout.Button ("Update prefab")) {
			//GameObject newGO = new GameObject ();
			//newGO.name = windowTitle;
			//WisdominiObject newlwo = newGO.AddComponent<WisdominiObject> ();
			//newlwo.initialize ();
			/*for (int i = 0; i < events.Count; ++i) {
				newlwo.addEvent (events [i]);
			}*/
                
            PrefabUtility.SaveAsPrefabAssetAndConnect(theObject, "Assets/Resources/Prefabs/NodeEditorObjects/" + currentFolder + "/" + windowTitle + ".prefab", InteractionMode.AutomatedAction);
		}

	}

	public void initialize() {

		//events = new List<string>();

	}

	public void addEvent(string e) {

		//events.Add (e);

	}

	public override void DrawCurves() {



	}

}

#endif