using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SuperDialogoHelper : WisdominiObject {

	public LevelControllerScript level;
	public DialogueObject dialogue;
	bool evenVisit;
	public string character;
	public int totalAnswers;
	int disabledAnswers;

	void Start() {
		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		if (dialogue == null)
			dialogue = GameObject.Find ("DialogueObject").GetComponent<DialogueObject> ();
	}

	public void _wm_selectAnswers () {
	
		string key = character + "SuperAnswer";
		string lKey = character + "SuperAnswerLevel";

		int lastAnswer = level.retrieveIntValue (key);
		int answerLevel = level.retrieveIntValue (lKey);

		List<bool> answerEnabled = new List<bool> ();

		dialogue.resetAnswers ();

		answerEnabled.Add (true); // we add an extra true so that indexes fall in [1 .. totalAnswers]
		for (int i = 0; i < totalAnswers; ++i) {
			answerEnabled.Add (true);
		}
		// disable all previously chosen answers
		disabledAnswers = 0;
		int answerChosen = level.retrieveIntValue (key + disabledAnswers);
		while (answerChosen != 0) {
			dialogue.disableAnswer (answerChosen);
			answerEnabled [answerChosen] = false;
			++disabledAnswers;
			answerChosen = level.retrieveIntValue (key + disabledAnswers);
		}

		// disable the answer we have just chosed
		if (lastAnswer != 0) {
			dialogue.disableAnswer (lastAnswer);
			answerEnabled [lastAnswer] = false;
			level.storeIntValue (key + disabledAnswers, lastAnswer);
			++disabledAnswers;
		}

		if ((totalAnswers-1) == disabledAnswers) {
			level.storeBoolValue (key + "Depleted", true);
		}

		int k = totalAnswers;
		// keep disabling answers until at most two remain
		while ((totalAnswers - disabledAnswers) > 2) {
			if (answerEnabled [k]) {
				dialogue.disableAnswer (k);
				answerEnabled [k] = false;
				++disabledAnswers;
			}
			--k;
		}
	

	}
		

}
