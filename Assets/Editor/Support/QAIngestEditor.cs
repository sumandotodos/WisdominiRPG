using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(QAIngest))]
public class QAIngestEditor : Editor  {
	

	public override void OnInspectorGUI() 
	{
		DrawDefaultInspector ();

		QAIngest ingestRef = (QAIngest)target;

		if (GUILayout.Button ("Load file")) 
		{
			string path = EditorUtility.OpenFilePanel ("Choose file", "", "txt");
			StreamReader fileIn = new StreamReader (path);
			string contents = fileIn.ReadToEnd ();
			ingestRef.loadFile (contents);
		}
		if (ingestRef.fileLoaded) {
			if (GUILayout.Button ("Process"))
			{
				parse (ingestRef);
			}
		}
	}

	public void parse(QAIngest ingestRef)
	{
		List<string> questionList = new List<string> ();
		List<string> setNameList = new List<string> ();
		List<int> numberOfAnswers = new List<int> ();
		List<int> correctAnswerList = new List<int> ();
		List<string> answersList = new List<string>();
		int correctAnswer;
		List<StringBank> sbList = new List<StringBank> ();
		List<GameObject> prefabList = new List<GameObject> ();

		// Structure is:

		//  +
		//	<name of the set>
		//	-
		//	<question 1>
		//	/
		//	<answer 1 to question 1>
		//	/	
		//	<answer 2 to question 1>
		//	...etc
		//	-
		//	<question 2>
		//	/
		//	<answer 1 to question 2>
		//	...etc

		int answerNumber;
		int questionNumber;

		AllTerrainParser parser = new AllTerrainParser (ingestRef.fileContents);
		parser.setParserMode (ParserMode.begin);

		GameObject newStringBankGO;
		GameObject GO;
		StringBank newStringBank;
		Object prefab;

		int ojojo;
		ojojo = parser.dataSize ();

		while (parser.charAtHead () == '+') 
		{
			questionList = new List<string> ();
			correctAnswerList = new List<int> ();
			sbList = new List<StringBank> ();
			prefabList = new List<GameObject> ();

			parser.scanToChar ('+'); // go past '+'
			parser.scanToNextLine (); // onto next line
			parser.setParserMode (ParserMode.end); // move end head until the end of the line
			parser.scanWhileNotNextLine ();
			string setName = parser.extract ();
			setNameList.Add (setName);
			parser.setParserMode (ParserMode.begin);
			parser.scanToNextLine ();
			while (parser.charAtHead () == '-') 
			{
				numberOfAnswers = new List<int> ();
				answersList = new List<string>();
				answerNumber = 0;
				correctAnswer = -1;
				
				parser.scanToChar ('-'); // scan past '-'
				parser.scanToNextLine (); // move onto next line
				parser.setParserMode (ParserMode.end); // end head until the end of line
				parser.scanWhileNotNextLine ();
				string question = parser.extract ();
				questionList.Add (question);
				parser.setParserMode (ParserMode.begin);
				parser.scanToNextLine (); // onto next line
				string link = "";
				if (parser.charAtHead () == '$') 
				{ // link found
					parser.scanToChar ('$');
					parser.setParserMode (ParserMode.end);
					parser.scanWhileNotNextLine ();
					link = parser.extract ();
					parser.setParserMode (ParserMode.begin);
					parser.scanToNextLine ();
				}

				while (parser.charAtHead () == '/') 
				{
					parser.scanToChar ('/');
					parser.scanToNextLine ();
					parser.setParserMode (ParserMode.end);
					parser.scanWhileNotNextLine ();
					string answer = parser.extract ();
					if (answer.ToCharArray () [0] == '*') 
					{
						answer = answer.Substring (1);
						correctAnswer = answerNumber;
					}
					answersList.Add (answer);
					parser.setParserMode (ParserMode.begin);
					parser.scanToNextLine ();
					++answerNumber;
				}

				// if found, store link in server
				if (!link.Equals ("")) 
				{
					WWWForm form = new WWWForm ();
					form.AddField ("setId", setName);
					form.AddField ("questionId", questionList.Count);
					form.AddField ("link", link);

					string script = Utils.WisdominiServer + "/registerLink";
					WWW www = new WWW (script, form);

					if (www != null) 
					{
						while (!www.isDone) 
						{
							// wait?!?
						}
					}
				}

				numberOfAnswers.Add (answerNumber);
				correctAnswerList.Add (correctAnswer);
			
				// at this point we have gathered: set name, question, and all answers
				// we can produce a StringBank(Respuestas... )
				newStringBankGO = new GameObject();
				newStringBank = newStringBankGO.AddComponent<StringBank> ();
				newStringBank.reset ();
				newStringBank.phrase = new string[answerNumber];
				for (int i = 0; i < answerNumber; ++i) 
				{
					newStringBank.phrase [i] = answersList [i];
				}
				newStringBank.extra = "Respuestas" + setName + questionList.Count;
                GO = PrefabUtility.SaveAsPrefabAssetAndConnect(newStringBankGO, "Assets/Prefabs/StringBanks/" + ingestRef.outputFolder + "/StringBank(Respuestas" + setName + "_" + questionList.Count + ").prefab", InteractionMode.AutomatedAction);
				prefabList.Add (GO);
				sbList.Add (newStringBank);

			} // end of while '-'

			// at this point we have gathered all questions in the set, 
			// we can produce a StringBank(Preguntas... )
			newStringBankGO = new GameObject();
			newStringBank = newStringBankGO.AddComponent<StringBank>();
			newStringBank.reset();
			newStringBank.phrase = new string[questionList.Count];
			newStringBank.correntAnswer = new int[questionList.Count];
			for(int i = 0; i < questionList.Count; ++i) {
				newStringBank.phrase[i] = questionList[i];
				newStringBank.correntAnswer[i] = correctAnswerList[i] + 1;
			}
			newStringBank.extra = "Preguntas" + setName;
            GO = PrefabUtility.SaveAsPrefabAssetAndConnect(newStringBankGO, "Assets/Prefabs/StringBanks/" + ingestRef.outputFolder + "/StringBank(Preguntas" + setName + ").prefab", InteractionMode.AutomatedAction);
			prefabList.Add (GO);
			sbList.Add (newStringBank);

			// and finally, we can create the StringBankCollection prefab

			GameObject newStringBankColGO = new GameObject ();
			StringBankCollection newStringBankCol = newStringBankColGO.AddComponent<StringBankCollection> ();
			newStringBankCol.bank = new StringBank[questionList.Count + 1];
			newStringBankCol.bank [0] = prefabList [prefabList.Count - 1].GetComponent<StringBank> ();
			for (int i = 0; i < sbList.Count-1; ++i) {
				newStringBankCol.bank [i + 1] = (StringBank)prefabList [i].GetComponent<StringBank> ();
			}
			PrefabUtility.SaveAsPrefabAssetAndConnect(newStringBankColGO, "Assets/Prefabs/StringBanks/" + ingestRef.outputFolder + "/StringBank(Preguntas" + setName + ").prefab", InteractionMode.AutomatedAction);

			DestroyImmediate (newStringBankColGO);
			for (int i = 0; i < sbList.Count; ++i) {
				DestroyImmediate (sbList [i].gameObject);
			}

		} // end of while '+'
			

	}

}
