using UnityEngine;
using System.Collections;

enum WFYingYangState { idle, growing, spinning, trembling, homing };

public class WordFightYinYang : WisdominiObject {

	/* references */

	public GameObject wordBlobPrefab;
	public Camera cam;
	public WordFightController controller;

	/* properties */

	WFYingYangState state;
	float elapsedTime;
	float scale;
	float angle;
	float angleSpeed;
	bool oddFrame;
	bool nextGood;
	string nextString;


	/* public properties */

	[HideInInspector]
	public float difficulty;

	/* constants */

	const float firstDelay = 0.5f;
	const float scaleSpeed = 3.0f;
	const float maxScale = 1.9f;
	const float fastAngleSpeed = 4400.0f;
	const float slowAngleSpeed = 26.0f;
	const float tremblingTime = 0.1f;
	const float zOffset = 0.1f;

	new void Start ()
	{
		state = WFYingYangState.idle;
		elapsedTime = 0.0f;
		scale = 0.0f;
		angle = 0.0f;
		angleSpeed = fastAngleSpeed;
		this.transform.localScale = Vector3.zero;
		oddFrame = true;
		difficulty = controller.difficulty;
	
	}
	
	void Update ()
	{
		if (state == WFYingYangState.idle) 
		{

		}

		if (state == WFYingYangState.homing)
		{
			int nTurns = (int)(angle / 360.0f);
			float normalAngle = angle - 360.0f * nTurns;
			angle = normalAngle;
			angle += angleSpeed * Time.deltaTime;
			angleSpeed += Time.deltaTime * 20.0f;
			if (angle > 360.0f) {
				angle = 360.0f;
				state = WFYingYangState.idle;
			}
			this.transform.rotation = Quaternion.Euler (0, 0, angle);
		}

		if (state == WFYingYangState.growing) 
		{			
			scale += scaleSpeed * Time.deltaTime;

			if (scale > maxScale) 
			{
				scale = maxScale;
				state = WFYingYangState.spinning;
				notifyFinishAction ();
			}

			angle += angleSpeed * Time.deltaTime;

			this.transform.localScale = new Vector3(scale, scale, scale);
			this.transform.rotation = Quaternion.Euler (0, 0, angle);

			if (angleSpeed > slowAngleSpeed) 
			{ 
				angleSpeed = angleSpeed * 0.97f;
			}
		}

		if (state == WFYingYangState.spinning) 
		{
			if (angleSpeed > slowAngleSpeed) { 
				angleSpeed = angleSpeed * 0.97f;
			}
			angle += angleSpeed * Time.deltaTime;
			this.transform.rotation = Quaternion.Euler (0, 0, angle);
		}

		if (state == WFYingYangState.trembling) 
		{
			angle += angleSpeed * Time.deltaTime;
			this.transform.rotation = Quaternion.Euler (0, 0, angle);

			Vector3 newPos = this.transform.position;

			if (oddFrame) {
				newPos.y += 0.05f;
			} else
				newPos.y -= 0.05f;

			this.transform.position = newPos;

			oddFrame = !oddFrame;

			elapsedTime += Time.deltaTime;
			if (elapsedTime > tremblingTime) {
				elapsedTime = 0.0f;
				newPos = this.transform.position;

				if (oddFrame) {
					newPos.y += 0.05f;
				} else
					newPos.y -= 0.05f;

				this.transform.position = newPos;
				Vector3 spawnPos = this.transform.position;
				spawnPos.z += zOffset;
				GameObject newBlobGO = (GameObject)Instantiate (wordBlobPrefab, spawnPos, Quaternion.Euler (Vector3.zero));
				WordFightWordBlob newBlob = newBlobGO.GetComponent<WordFightWordBlob> ();
				newBlobGO.name = "WFBlob";
				newBlob.angle = FloatRandom.floatRandomRange (0.0f, 6.28f);
				newBlob.initialize ();
				newBlob.good = nextGood;
				newBlob.controller = controller;
				newBlob.yinYang = this;
				newBlob.cam = cam;
				newBlob.difficulty = difficulty;
				if (nextString.Equals ("")) 
				{
					nextString = "";
				}
				newBlob.setText (nextString);
				state = WFYingYangState.spinning;
			}
		}	
	}

	public void home() 
	{
		state = WFYingYangState.homing;
	}

	public void grow() 
	{
		state = WFYingYangState.growing;
	}

	public void _wa_grow(WisdominiObject waiter) 
	{
		waitingRef = waiter;
		waiter.isWaitingForActionToComplete = true;
		grow ();
	}

	public bool isFinishedHoming() 
	{
		return state == WFYingYangState.idle;
	}

	public void emitBlob(string text, bool good) 
	{
		elapsedTime = 0.0f;
		nextString = text;
		nextGood = good;
		state = WFYingYangState.trembling;
	}
}
