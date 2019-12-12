using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum State { Idle, Finished };

public class DisorderedSentence : WisdominiObject {

	public string [] sentenceBits;
	string completeSentence;

	public float volumeRadius;

	public Camera cam;

	int nPieces;

	public bool finished;
	bool activityFinished = false;

	public int nLines;

	int actionState;

	GameObject []bitGO;

	const float SPACE = 1.5f;

	MasterControllerScript mcRef;

	public GameObject  UIFader;
	UIFaderScript fRef;

	bool globalCanPick = false;
	public bool canPick = true;
	bool lose;

	new void Start () 
	{
		lose = false;
		fRef = UIFader.GetComponent<UIFaderScript> ();

		fRef.setFadeValue (0.0f);
		fRef.Initialize ();
		fRef.fadeIn ();

		finished = false;

		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();

		if(mcRef != null) sentenceBits = mcRef.getSentencePieces ().ToArray ();
	
		nPieces = sentenceBits.Length;

		bitGO = new GameObject[nPieces];

		actionState = 0;

		for (int i = 0; i < nPieces; ++i) {

			bitGO[i] = new GameObject();
			SentenceBit newBit;
			newBit = bitGO [i].AddComponent<SentenceBit> ();
			newBit.Initialize ();

			newBit.setText (sentenceBits [i]);
			newBit.randomizePosition (volumeRadius);
			newBit.randomizeSpeed ();
			newBit.setCamera (cam);
			newBit.order = i;
			newBit.parent = this;

			newBit.setPosition (new Vector3 (0, 0, 0));
			bitGO [i].name = "SentenceBit" + i;
		}

		completeSentence = sentenceBits [0];
		for (int i = 1; i < sentenceBits.Length; ++i) {
			completeSentence = completeSentence + " " + sentenceBits [i];
		}

		setText ("");

		mcRef.getStorage ().storeStringValue ("ReturnFromActivity", "Well");
		mcRef.getStorage ().storeIntValue ("ActivityResult", 0);

		isWaitingForActionToComplete = true;

	}

	public SentenceBit getPiece(int number) {

		if (number < nPieces) {
			return bitGO [number].GetComponent<SentenceBit> ();
		} else
			return null;

	}

	public void setText(string txt) {

		if (txt.Equals (completeSentence) && !finished) 
		{
			int solved = mcRef.getStorage ().retrieveIntValue ("CorrectlySolvedWells");
			mcRef.getStorage ().storeIntValue ("CorrectlySolvedWells", solved + 1);
			mcRef.getStorage ().storeIntValue ("ActivityResult", 1);
			SentenceBit bitData;
			fRef.setFadeColor (1.0f, 1.0f, 1.0f);
			fRef.setFadeValue (0.0f);
			fRef.fadeIn ();
			// Calculate width of each line
			// X is vertical (+ up)
			// Z is horizontal (- right)
			float destX;
			destX = ((float)nLines * 3.0f) / 4.0f;
			for (int i = 0; i < nLines; ++i) {
				
				float lineWidth = 0.0f;
				bool first = true;
				for (int j = 0; j < bitGO.Length; ++j) {
					bitData = bitGO [j].GetComponent<SentenceBit> ();
					bitData.setCorrectlyAligned (true);
					if (bitData.lineNumber == i) {
						lineWidth += bitData.width;
						if (!first)
							lineWidth += SPACE;
						first = false;
					}
				}
				// now we know ith line width, set destination coordinates
				float lastWidth = 0.0f;
				float destZ;
				destZ = lineWidth / 4.0f;

				for (int j = 0; j < bitGO.Length; ++j) {
					bitData = bitGO [j].GetComponent<SentenceBit> ();
					if (bitData.lineNumber == i) {
						destZ -= lastWidth;
						bitData.setTargetPosition (destZ, destX);
						lastWidth = bitData.width + SPACE;
					}
				}
				destX -= 3.0f;

			}
			//fRef._wa_fadeOut (this);
			isWaitingForActionToComplete = true;
			//finished = true;
		}
	}

	public void setLines(int l) 
	{
		nLines = l;
	}
	
	new void Update () 
	{
		if (!Input.GetMouseButton (0) && !globalCanPick)
			canPick = true;

		if (isWaitingForActionToComplete)
			return;

		if (actionState == 0)
		{
			isWaitingForActionToComplete = true;
		} 

		else if (actionState == 1)
		{
			string returnToLocation = mcRef.getSavedLocation ();

			if (!lose)
			{
				string currentWell = mcRef.getStorage ().retrieveStringValue ("CurrentWell");
				mcRef.getStorage ().storeBoolValue (currentWell, true);
				int num = mcRef.getStorage ().retrieveIntValue ("FirstWellDone");
				mcRef.getStorage ().storeIntValue ("FirstWellDone", ++num);
			}

			int nWells;
			nWells = mcRef.getStorage ().retrieveIntValue ("TotalWells");
			mcRef.getStorage ().storeIntValue ("TotalWells", nWells + 1);
			mcRef.setVolume (0.0f, 1.5f);
			mcRef.selectMixer (0);
			mcRef.playMusic ();
			mcRef.setVolume (1.0f, 1.5f);
			SceneManager.LoadScene (returnToLocation);
		}
	}

	public void finishActivity() 
	{
		if (!activityFinished) 
		{
			activityFinished = true;
			fRef.setFadeColor (0.0f, 0.0f, 0.0f);
			fRef._wa_fadeOut (this);
			isWaitingForActionToComplete = true;
			actionState = 1;
		}
	}

	public void _wa_moveBitsToFinalPosition(WisdominiObject waiter)
	{
		waitingRef = waiter;
	}

	public void _wm_gameover() 
	{
		fRef.setFadeColor (0.0f, 0.0f, 0.0f);
		fRef._wa_fadeOut (this);
		string currentWell = mcRef.getStorage().retrieveStringValue("CurrentWell");
		mcRef.getStorage ().storeBoolValue (currentWell, false);
		isWaitingForActionToComplete = true;
		actionState = 1;
		lose = true;
	}
}
