using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(FastDialogueImporter))]
public class FastDialogueImporterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FastDialogueImporter importer = (FastDialogueImporter)target;

        if (GUILayout.Button("Choose folder"))
        {
            string path = EditorUtility.OpenFolderPanel("Folder", "", "");
            importer.Folder = path;
            // add event
        }

        if(importer.Folder!="")
        {
            DirectoryInfo dir = new DirectoryInfo(importer.Folder);
            FileInfo[] info = dir.GetFiles("*.fdl");
            int nFiles = 0;
            foreach (FileInfo f in info)
            {
                ++nFiles;
            }
            if (nFiles > 0)
            {
                if (GUILayout.Button("Import dialogues in folder"))
                {
                    int nImports = 0;
                    foreach (FileInfo f in info)
                    {
                        string filename = f.Name;
                        string objectName = filename.Substring(0, filename.Length - 4);
                        Debug.Log("Finding object named: " + objectName);
                        CharacterGenerator gen = GameObject.Find(objectName).GetComponent<CharacterGenerator>();
                        if (gen != null)
                        {
                            string contents = File.ReadAllText(f.FullName);
                            gen.importFastDialogue(contents);   
                            ++nImports;
                        }
                    }
                    Debug.Log("<color=green>Number of imported dialogues: " + nImports + "</color>");
                }
            }
            else
            {
                EditorGUILayout.LabelField("No dialogue files in folder");
            }
        }
    }
}