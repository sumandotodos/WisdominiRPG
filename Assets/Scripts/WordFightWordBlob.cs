using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum WFWordBlobState {  idle, drifting, pickedUp, sinking, paused };

public class WordFightWordBlob : MonoBehaviour {



	/* references */

	public WordFightYinYang yinYang; // to know the origin
	public WordFightController controller;
	public TextMesh shadowText;
	public TextMesh theText;
	public GameObject theQuad;
	public Camera cam;
	public int test;


	float targetDeltaY;
	float deltaY;




	/* properties */

	Vector3 pos;
	Vector3 pickUpWorldPos;
	WFWordBlobState state;
	Vector3 prevPosition;
	float saveZ;
	float scale;
	public bool good;
	public float difficulty;
	int nTurns = 1;
	WFWordBlobState savedState;
	Vector3 worldCoords;

	/* public properties */

	public float angle = 0.0f;
	public float speed = 1.5f;
	public int preferredMaxLineLength = 16;



	/* constants */

	const float killdist = 10.0f;
	const float turnDist = 5.5f;
	const float insertDist = 1.0f;
	const float SpeedFactor = 4.0f;

	public void initialize() {

		state = WFWordBlobState.drifting;
		saveZ = this.transform.position.z;
		//deltaY = targetDeltaY = 0.0f;
		//tab = null;

	}



	// Use this for initialization
	void Start () {


	
	}

	public void pause() {

		savedState = state;
		state = WFWordBlobState.paused;

	}

	public void unpause() {

		state = savedState;

	}
	
	// Update is called once per frame
	void Update () {

		if (state == WFWordBlobState.paused) {

		}
	
		if (state == WFWordBlobState.drifting) {

			Vector3 displacement = Vector3.zero;

			displacement.x = speed * difficulty * Mathf.Cos (angle) * Time.deltaTime;
			displacement.y = speed * difficulty * Mathf.Sin (angle) * Time.deltaTime;

			Vector3 newPos = this.transform.position + displacement;
			this.transform.position = newPos;

			Vector3 diff = newPos - yinYang.transform.position;

			if (!good && (diff.magnitude > turnDist)) {
				if (nTurns > 0) {
					--nTurns;
					state = WFWordBlobState.sinking;
				}
			}

			if (diff.magnitude > killdist)
				Destroy (this.gameObject);

		}

		if (state == WFWordBlobState.sinking) {

			Vector3 direction = yinYang.transform.position - this.transform.position;
			direction.Normalize ();

			Vector3 displacement = Vector3.zero;

			displacement.x = speed * difficulty * direction.x * Time.deltaTime;
			displacement.y = speed * difficulty * direction.y * Time.deltaTime;

			Vector3 newPos = this.transform.position + displacement;
			this.transform.position = newPos;

			Vector3 diff = newPos - yinYang.transform.position;
			if (diff.magnitude < insertDist) {
				
				if (!good) {
					controller.hurtPlayer ();
				}
				Destroy (this.gameObject);
			}

		}

		if (state == WFWordBlobState.pickedUp) {

			if (deltaY < targetDeltaY) {
				deltaY += 2.0f * Time.deltaTime;
			}
				
			test = 0;

			RaycastHit hit;
			RaycastHit[] allHits;
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);

			allHits = Physics.RaycastAll(ray);
			for(int i = 0; i<allHits.Length; ++i) {
				GameObject objectHit = allHits[i].transform.gameObject;

				if (objectHit.tag == "RayCastBillboard") {

					prevPosition = worldCoords;
					worldCoords = allHits[i].point;
					test = 1;
					worldCoords.z = saveZ;
					this.transform.position = worldCoords + new Vector3(0, deltaY, 0);


				}


			} /*else {
				worldCoords = prevPosition;
			}*/


			Vector3 diff = worldCoords - yinYang.transform.position;
			if (diff.magnitude < insertDist) {
				if (good) {
					controller.hurtShadow ();
				} else
					controller.hurtPlayer ();
				Destroy (this.gameObject);
			}



			if (Input.GetMouseButtonUp (0)) {

				/* new speed and angle */
				state = WFWordBlobState.drifting;
				Vector3 releaseVelocity = worldCoords - prevPosition;
				speed = releaseVelocity.magnitude * SpeedFactor;// / Time.deltaTime;
				angle = Mathf.Acos (releaseVelocity.normalized.x);
				if (releaseVelocity.y < 0.0)
					angle = Mathf.Deg2Rad * 360.0f - angle;

				/* reenable collider */
				this.GetComponent<Collider> ().enabled = true;

				targetDeltaY = 0.0f;
				//if(tab != null) tab.release ();
			}


			//prevPosition = worldCoords;

		}

	}


	public void setText(string txt) {

		//theText.text = txt;
		//shadowText.text = txt;
		//return;

		int maxLine;
		char[] data;
		List<int> indexes = new List<int>();
		data = txt.ToCharArray ();
		//theText.text = txt; // add \n's if text is too long
		maxLine = txt.Length;

		/* get list of all spaces */
		for (int i = 0; i < txt.Length; ++i) {
			if (data [i] == ' ')
				indexes.Add (i);
		}

		int offset = preferredMaxLineLength;
		while (offset < txt.Length) {
			

			/* find closest space */
			int closest = -1;
			int minDistance;
			minDistance = txt.Length *  txt.Length;
			for (int i = 0; i < indexes.Count; ++i) {
				if((indexes[i]-offset)*(indexes[i]-offset) < minDistance) {
					minDistance = (indexes[i]-offset)*(indexes[i]-offset);
					closest = i;
				}
			}

			int prevOffset = offset;

			if (closest != -1) {
				data [indexes[closest]] = '\n';
				offset = indexes[closest];
			}

			offset += preferredMaxLineLength;

			int postOffset = offset;

			// prevent stucking
			if (postOffset <= prevOffset) 
				offset = prevOffset + preferredMaxLineLength;

		}

		theText.text = new string (data);
		shadowText.text = theText.text;

		//theQuad.transform.localScale = new Vector3(1.0f + txt.Length/10.0f, 1, 1);
			
	}



	public void pickUp(Vector3 screenPos) {

		pickUpWorldPos = screenPos;
		this.GetComponent<Collider> ().enabled = false;
		state = WFWordBlobState.pickedUp;
		nTurns = 0; // it is VERY difficult if otherwise
		//targetDeltaY = 0.5f;


	}


}
