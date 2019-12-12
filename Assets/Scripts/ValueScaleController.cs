using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum VSCStatus { picking, idle, exitting };

public class ValueScaleController : WisdominiObject {

	/* reference */
	public ValuesScale theScale;
	public Camera theCamera;
	public UIFaderScript fader;
	public GameObject Text1;
	public GameObject Text2;
	public GameObject Text3;
	public GameObject Text4;
	public GameObject Text5;
	ValueScaleItem[] item;
	public MasterControllerScript mcRef;

	public float physicalHeight;

	public Text textRef;

	ValueScaleItem pickedItem;

	/* properties */
	VSCStatus status;
	int[] floorContents;


	/* constants */
	const float ratioConstant = 21;
	float floorHeight;
	int lastFloor;
	int floor;
	const float m = -0.071f;
	const float b = 0.48f;


	int[] maxCharacPerFloor;

	// Use this for initialization
	new void Start () {

		float aspect;
		aspect = Screen.width / Screen.height;

		theCamera.fieldOfView = 60.0f*(Screen.height/600.0f) - (aspect - 1.777f) * ratioConstant;

		floorHeight = physicalHeight / 5.0f;
		status = VSCStatus.idle;

		item = new ValueScaleItem[5];
		item [0] = Text1.GetComponent<ValueScaleItem> ();
		item [1] = Text2.GetComponent<ValueScaleItem> ();
		item [2] = Text3.GetComponent<ValueScaleItem> ();
		item [3] = Text4.GetComponent<ValueScaleItem> ();
		item [4] = Text5.GetComponent<ValueScaleItem> ();

		for (int i = 0; i < 5; ++i) {
			item [i].initialize ();
		}

		maxCharacPerFloor = new int[5];
		maxCharacPerFloor [0] = 150;
		maxCharacPerFloor [1] = 130;
		maxCharacPerFloor [2] = 110;
		maxCharacPerFloor [3] = 95;
		maxCharacPerFloor [4] = 70;

		string s;
		float xPos;
		int maxWidth;
		s = adjustString (item [0].getText (), maxCharacPerFloor[0]);
		item [0].setText (s);
		maxWidth = maxLineWidth (s);
		xPos = m * maxWidth + b;
		Text1.transform.position = new Vector3 (xPos, 0, 0);
		s = adjustString (item [1].getText (), maxCharacPerFloor[1]);
		item [1].setText (s);
		maxWidth = maxLineWidth (s);
		xPos = m * maxWidth + b;
		Text2.transform.position = new Vector3 (xPos, 0, 0);
		s = adjustString (item [2].getText (), maxCharacPerFloor[2]);
		item [2].setText (s);
		maxWidth = maxLineWidth (s);
		xPos = m * maxWidth + b;
		Text3.transform.position = new Vector3 (xPos, 0, 0);
		s = adjustString (item [3].getText (), maxCharacPerFloor[3]);
		item [3].setText (s);
		maxWidth = maxLineWidth (s);
		xPos = m * maxWidth + b;
		Text4.transform.position = new Vector3 (xPos, 0, 0);
		s = adjustString (item [4].getText (), maxCharacPerFloor[4]);
		item [4].setText (s);
		maxWidth = maxLineWidth (s);
		xPos = m * maxWidth + b;
		Text5.transform.position = new Vector3 (xPos, 0, 0);


		for (int i = 0; i < 5; ++i) {
			item [i].setY (i * floorHeight + 2.0f);
		}

		floorContents = new int[5];
		for (int i = 0; i < 5; ++i)
			floorContents [i] = i; // this should be load from MCs Database


		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();

	
	}

	int maxLineWidth(string s) {

		int max = 0;
		int count = 0;
		int head = 0;

		while (head < s.Length) {
			if (s [head++] != '\n')
				++count;
			else {
				if (count > max)
					max = count;
				count = 0;
			}
		}

		if (count > max)
			max = count;

		return max;

	}

	string adjustString(string s, int maxCharaPerLine) {

		if (s.Length < maxCharaPerLine)
			return s;

		int head = 0;
		char[] array = s.ToCharArray ();

		while ((head + (maxCharaPerLine - 1)) < s.Length) {
		
			head += (maxCharaPerLine - 1);

			while ((array [head] != ' ') && (head > 0))
				--head;

			array [head] = '\n';

		}

		return new string(array);

	}

	// Update is called once per frame
	new void Update () {

		if (Input.GetMouseButtonDown (0) && status == VSCStatus.idle) {


			RaycastHit hit;
			Ray ray = theCamera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit)) {
				
				GameObject go = hit.collider.gameObject;

				Vector2 coords = hit.textureCoord;
				textRef.text = "Coords: (" + coords.x + ", " + coords.y + ")";
				//ValueScaleItem item = go.GetComponent<ValueScaleItem> ();

				if (coords.x < 0.2)
					pickedItem = item [floorContents[0]];
				else if (coords.x < 0.4)
					pickedItem = item [floorContents[1]];
				else if (coords.x < 0.6)
					pickedItem = item [floorContents[2]];
				else if (coords.x < 0.8)
					pickedItem = item [floorContents[3]];
				else if (coords.x < 1.0)
					pickedItem = item [floorContents[4]];
		

				pickedItem.pick ();
				lastFloor = ((int)(coords.x * 5.0f));
				status = VSCStatus.picking;
				// Do something with the object that was hit by the raycast.
			}
		}

		if (status == VSCStatus.picking) {
			
			RaycastHit hit;
			Ray ray = theCamera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit)) {



				GameObject go = hit.collider.gameObject;

				Vector2 coords = hit.textureCoord;

				floor = ((int)(coords.x * 5.0f));

				textRef.text = "Coords: (" + coords.x + ", " + coords.y + ")";
				textRef.text += "\nFloor: " + floor + ",  Last Floor: " + lastFloor;
				textRef.text += "\nfloorContents[4] = " + floorContents[4];
				textRef.text += "\nfloorContents[3] = " + floorContents[3];
				textRef.text += "\nfloorContents[2] = " + floorContents[2];
				textRef.text += "\nfloorContents[1] = " + floorContents[1];
				textRef.text += "\nfloorContents[0] = " + floorContents[0];
				//ValueScaleItem item = go.GetComponent<ValueScaleItem> ();
				pickedItem.setY(coords.x * physicalHeight);

				if (floor == 0) {
					Debug.Log ("WTF");
				}

				if(floor != lastFloor) {
					

					float maxWidth;
					float xPos;
					string s;
					s = item [floorContents[floor]].getText ();
					s = adjustString (s, maxCharacPerFloor[lastFloor]);
					item [floorContents[floor]].setText (s);
					maxWidth = maxLineWidth (s);
					xPos = m * maxWidth + b;
					Vector3 curPos = item [floorContents [floor]].transform.position;
					item [floorContents[floor]].transform.position = new Vector3 (xPos, curPos.y, curPos.z);

					item [floorContents[floor]].setTargetY (lastFloor * floorHeight + 2.0f);

					/* interchange contents */
					int temp;
					temp = floorContents [lastFloor];
					floorContents [lastFloor] = floorContents [floor];
					floorContents [floor] = temp;
				}

				lastFloor = floor;

				// Do something with the object that was hit by the raycast.
			}
		}

		if (Input.GetMouseButtonUp (0) && status == VSCStatus.picking) {

			pickedItem.release ();
			float maxWidth;
			float xPos;
			string s;
			s = pickedItem.getText ();
			s = adjustString (s, maxCharacPerFloor[floor]);
			pickedItem.setText (s);
			maxWidth = maxLineWidth (s);
			xPos = m * maxWidth + b;
			pickedItem.transform.position = new Vector3 (xPos, 0, 0);
			pickedItem.setTargetY(floor * floorHeight + 2.0f);
			status = VSCStatus.idle;

		}

		if (status == VSCStatus.exitting && !isWaitingForActionToComplete) {

			// load level
			if (mcRef == null)
				return;

			DataStorage ds = mcRef.getStorage ();
			string returnLevel = ds.retrieveStringValue ("ReturnLocation");
			if (!returnLevel.Equals ("")) {
				SceneManager.LoadScene (returnLevel);
			}

		}
	
	}

	public void exit() {

		fader._wa_fadeOut (this);
		this.isWaitingForActionToComplete = true;
		status = VSCStatus.exitting;
	}

}

//				   level 5: 110
//				   leve  4: 120
// 				   level 3: 130
// Max characters: level 2: 140
//				   level 1: 150
