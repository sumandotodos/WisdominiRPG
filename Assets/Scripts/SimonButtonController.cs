using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimonButtonController : MonoBehaviour {

	public LevelControllerScript level;
	public SimonButton[] button;
	public int sequenceLength = 1;
	public Camera cam;
	/*
	 * 
	 * generate = true : this controller generates, stores and shows a pattern
	 * 
	 * generate = false : this controller receives input from the player, and checks
	 * 				      correct pattern inpu
	 * 
	 */
	public bool generate = true;
	public float interButtonTime = 4.0f;
	float elapsedTime;
	public int state = 0;
	/*
	 * State list:
	 * 
	 * 	0: idle
	 *  1: initial delay for generation
	 *  2, 3: playing back generated sequence
	 * 
	 *  4: waiting for player input
	 *  5: 
	 * 
	 */

	int matchedButtons;
	int seqIndex;
	public string simonSequenceVariable; // a variable name to store the generated sequence
	string generatedSequence = "";
	public WisdominiObject unlockProgram; // program to execute when correct code is intered


	const float InitialDelay = 1.0f;


	// Use this for initialization
	void Start () {

		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		cam = GameObject.Find ("Main Camera").GetComponent<Camera> ();

		matchedButtons = 0;
		state = 0;
		generatedSequence = level.retrieveStringValue (simonSequenceVariable);
        Debug.Log("Generated seq: " + generatedSequence);
		if (generatedSequence == null)
			generatedSequence = "";
		if (generate) {
			if (generatedSequence.Equals (""))
				generateSequence ();
			//state = 1;
		}
		for (int i = 0; i < button.Length; ++i) { // propagate level & controller & misc reference
			button [i].level = level;
			button [i].controller = this;
			button [i].buttonId = i;
			button [i].playerInput = !generate;
			button [i].cam = cam;
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (state == 0) { // idling

		}

		if (state == 1) { // initial delay
			elapsedTime += Time.deltaTime;
			if (elapsedTime > InitialDelay) {
				elapsedTime = 0.0f;
				seqIndex = 0;
				++state;
			}
		}

		if (state == 2) { 
			if (seqIndex < generatedSequence.Length) { // if there letters remaining in the sequence...
				char seqChar = generatedSequence.ToCharArray () [seqIndex];
				int seqNum = (int)(seqChar) - (int)('0'); // transform for char to integer
				button [seqNum].lightButton (); // light that button
				++state;
				++seqIndex;
			} else
				state = 0; // go to idle state
		}

		if (state == 3) { // inter-button delay
			elapsedTime += Time.deltaTime;
			if (elapsedTime > interButtonTime) {
				elapsedTime = 0.0f;
				--state;
			}
		}
	
	}

	public void playerPress(int buttonId) {

		if (!generatedSequence.Equals ("")) {

            Debug.Log("Generated seq: " + generatedSequence);
            char seqChar = generatedSequence.ToCharArray () [matchedButtons];
			int seqNum = (int)(seqChar) - (int)('0'); // transform for char to integer
            if (seqNum == buttonId)
            {
                ++matchedButtons;
                Debug.Log("     ---> match ok");
                if (matchedButtons == sequenceLength)
                {
                    if (unlockProgram != null)
                    {
                        unlockProgram.startProgram(0);
                        matchedButtons = 0;
                    }
                }
            }
            else
            {
                matchedButtons = 0;
                Debug.Log("     ---> match is SHIT");
            }

		}

	}

	public void generateSequence() {

		int nButtons = button.Length;

		if (sequenceLength < 1)
			return;

		if (nButtons < 1)
			return;

		string seq = "";
		int prevRandom = -1;
		for(int i = 0; i<sequenceLength; ++i) {
			// make sure the same button is not chosen two times in a row
			int currentRandom = (Random.Range (0, nButtons - 1));
			while(currentRandom == prevRandom) currentRandom = (Random.Range (0, nButtons - 1));
			seq = seq + currentRandom;
			prevRandom = currentRandom;
		}

		if (!simonSequenceVariable.Equals("")) {
			level.storeStringValue (simonSequenceVariable, seq);
		}

		generatedSequence = seq;

	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player") {
			if(generate)
				state = 1;
		}

	}
}
