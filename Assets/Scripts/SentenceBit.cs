using UnityEngine;
using System.Collections;

enum SentenceBitState { Interact, Wait1, Assembling, Wait2, Fading, Finished };

public class SentenceBit : MonoBehaviour {

	Vector3 smallDelta = new Vector3 (0.1f, -0.1f, 0.1f);
	SoftFloat adjustX, adjustZ;
	float pickXdelta, pickZdelta;

	public string text;

	public DisorderedSentence parent;

	bool correctlyAligned = false;

	Camera cam;

	public float damp;
	public float maxSpeed;
	public float minSpeed;

	TextMesh theMesh;
	TextMesh theShadow;
	Plane thePlane;
	BoxCollider theCollider;

	GameObject textMeshGO;
	GameObject textShadowGO;
	GameObject planeGO;

	public Vector3 location;
	public Vector3 speed;

	float volumeRadius;

	SentenceBitState state;

	bool isTouchingThis;

	public Vector3 lastPos;
	public Vector3 position;

	Vector3 targetPosition;
	Vector3 targetSpeedDirection;

	public float width;

	public int order;

	string chainedText;
	string ownText;

	public int lines;
	public int lineNumber;

	public float targetSpeed;

	float elapsedTime;

	int isNew;
	bool hasTargetCoords;
	bool isMoving;
	bool isWaiting;
	bool isFading;

	const float WAIT1TIME = 1.0f;
	const float WAIT2TIME = 3.0f;

	public void Start() {

		isNew = 0;
		lines = 1;
		lineNumber = 0;
		width = 0.0f;
		hasTargetCoords = false;

		if (targetSpeed == 0.0f)
			targetSpeed = 1.0f;

		state = SentenceBitState.Interact;
	}


	public void Initialize () {

		textMeshGO = new GameObject ();
		textShadowGO = new GameObject ();

		textMeshGO.name = "TextMeshObject";

		theMesh = textMeshGO.AddComponent<TextMesh> ();
		theShadow = textShadowGO.AddComponent<TextMesh> ();
		theShadow.color = Color.black;



		theMesh.transform.rotation = (Quaternion.Euler (90, 90, 0));
		theShadow.transform.rotation = (Quaternion.Euler (90, 90, 0));

		theMesh.anchor = TextAnchor.MiddleCenter;
		theShadow.anchor = TextAnchor.MiddleCenter;

		theCollider = textMeshGO.AddComponent<BoxCollider> ();
		theCollider.center = Vector3.zero;

		maxSpeed = 0.2f;
		minSpeed = 0.005f;

		isTouchingThis = false;

		adjustX = new SoftFloat (0.0f);
		adjustX.setSpeed (3.0f);
		adjustX.setTransformation (TweenTransforms.cubicOut);
		adjustZ = new SoftFloat (0.0f);
		adjustZ.setSpeed (6.0f);
		adjustX.setTransformation (TweenTransforms.cubicOut);

		//selfPositioning = false;

	}

	public void randomizePosition(float volumeRadius_) {

		float randRadius;
		float randPhi;

		randRadius = Random.value * volumeRadius_;
		randPhi = Random.value * 6.28f;

		Vector3 pos = new Vector3( randRadius * Mathf.Cos(randPhi), 0.0f , randRadius * Mathf.Sin(randPhi));


		theMesh.transform.position = pos;
		theShadow.transform.position = pos + smallDelta;

		volumeRadius = volumeRadius_;

	}

	public void setCamera(Camera c) {

		cam = c;

	}

	public void randomizeSpeed() {

		speed = new Vector3 (Random.value, 0.0f, Random.value);
			speed.Normalize();
		speed = speed / 8.0f;


	}

	public void setText(string txt) {
		
		text = txt;
		theMesh.text = text;
		theShadow.text = text;

		//theMesh.GetComponent<Renderer> ().bounds;

		chainedText = ownText = txt;

	}

	public void setPosition(Vector3 pos) {
		/*
		textMeshGO.transform.position = pos;
		//planeGO.transform.position = pos;
*/
	}

	bool checkRectCollision(float xmin1, float ymin1, float xmax1, float ymax1, 
		float xmin2, float ymin2, float xmax2, float ymax2) {

		if (xmax1 < xmin2)
			return false;
		if (ymax1 < ymin2)
			return false;
		if (xmax2 < xmin1)
			return false;
		if (ymax2 < ymin1)
			return false;

		return true;
	}

	public void setCorrectlyAligned(bool c) {
		correctlyAligned = c;
	}

	void Update ()
	{

		adjustX.update ();
		adjustZ.update ();

		Vector3 touchPos;

		if (order == 0) {
			correctlyAligned = true;
		}
		if (correctlyAligned) {
			theMesh.color = Color.green;
		} else
			theMesh.color = Color.white;


		if (parent.finished) 
		{
			speed = Vector3.zero;
			//return;
		}

		if (state == SentenceBitState.Wait1) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > WAIT1TIME) 
			{
				elapsedTime = 0.0f;
				state = SentenceBitState.Assembling;
			}
		}

		if (state == SentenceBitState.Assembling) {
			
				Vector3 pos = this.theMesh.transform.position;
				pos += targetSpeedDirection / targetSpeed * Time.deltaTime;
				elapsedTime += Time.deltaTime;
				if (elapsedTime >= targetSpeed) {
					state = SentenceBitState.Wait2;
					pos = targetPosition;
					elapsedTime = 0.0f;
				}
				this.theMesh.transform.position = pos;
				this.theShadow.transform.position = pos + smallDelta;
			return;
		}

		if (state == SentenceBitState.Wait2) {
			parent.finished = true;
			elapsedTime += Time.deltaTime;
			if (elapsedTime > WAIT2TIME) {
				elapsedTime = 0.0f;
				state = SentenceBitState.Finished;
				parent.finishActivity ();
			}
		}

		if (state != SentenceBitState.Interact)
			return;

		if (isNew == 0) {
			Bounds bounds = theMesh.GetComponent<Renderer> ().bounds;

			theCollider.size = new Vector3 (bounds.size.z, bounds.size.x, 0.2f);
			//Vector3 cen = theCollider.center;
			//theCollider.center = new Vector3 (bounds.size.z / 2.0f, cen.y, cen.z);
			width = theCollider.size.x;

			isNew = 1;
		}
	
		bool touching;

		if (isTouchingThis && Input.GetMouseButtonUp (0)) { // release the finger
			isTouchingThis = false;
			BoxCollider interceptor;

			interceptor = GameObject.Find ("RaycastInterceptor").GetComponent<BoxCollider> ();
			Ray rayCastRay = cam.ScreenPointToRay (Input.mousePosition);
			RaycastHit info;
			interceptor.Raycast (rayCastRay, out info, 1000.0f);

			position = info.point;
			position.y = 0.0f;
			Vector3 drift = position - lastPos;
			while (drift.magnitude > maxSpeed)
				drift *= 0.8f;
			speed = drift;
			if (speed.magnitude < minSpeed)
				speed = Vector3.zero;

			adjustX.setValue (0.0f);
			adjustZ.setValue (0.0f);
		}

		if (!isTouchingThis && Input.GetMouseButtonDown (0)) // down finger
		{ // touching
			touchPos = Input.mousePosition;
			touching = true;
			float w = Screen.width;
			float h = Screen.height;

			const float PERSPMAGIC = 0.0617f;

			float localX = ((touchPos.x - w / 2.0f) / (w / 2.0f)) * volumeRadius * cam.fieldOfView * PERSPMAGIC;
			float localY = ((touchPos.y - h / 2.0f) / (h / 2.0f)) * volumeRadius * cam.fieldOfView * PERSPMAGIC;

			// check it collides with this bit
			position = theMesh.transform.position;

			Ray rayCastRay = cam.ScreenPointToRay (Input.mousePosition);
			RaycastHit info;
			theCollider.Raycast (rayCastRay, out info, 1000.0f);

			if ((info.collider == theCollider) && parent.canPick) {
				isTouchingThis = true;
				parent.canPick = false; // pick ONLY one!
				lastPos = new Vector3 (info.point.x, 0, info.point.z);
				pickXdelta = position.x - info.point.x;
				pickZdelta = position.z - info.point.z;

				adjustZ.setValue (0.0f);
				adjustX.setValue (2.0f);
			}

			if (isTouchingThis) 
			{
				//theMesh.GetComponent<Renderer> ().enabled = false;
			}

		} else {
			//theMesh.GetComponent<Renderer> ().enabled = true;
		}



		if (isTouchingThis) 
		{
			BoxCollider interceptor;

			interceptor = GameObject.Find ("RaycastInterceptor").GetComponent<BoxCollider> ();

			//pos = Input.mousePosition;

			Ray rayCastRay = cam.ScreenPointToRay (Input.mousePosition);
			RaycastHit info;
			interceptor.Raycast (rayCastRay, out info, 1000.0f);

			position = info.point + new Vector3 (pickXdelta, 0.0f, pickZdelta) + new Vector3 (adjustX.getValue (), 0.0f, adjustZ.getValue ());
			if (position.magnitude > volumeRadius) {
				position = volumeRadius * 0.95f * position.normalized;
			}

			lastPos = info.point;
			lastPos.y = 0.0f;

			while (speed.magnitude > maxSpeed) 
			{
				speed *= 0.8f;
			}

			//lastPos = new Vector3 (position.x, position.y, position.z);

		} else {
			position = theMesh.transform.position;
			position = position + speed;

			float spdModulus = speed.magnitude;

			if (spdModulus > maxSpeed)
				spdModulus *= 1.0f - (Time.deltaTime / 3.0f); // loose energy

			speed.Normalize ();
			speed = speed * spdModulus;

			float dist = position.magnitude;

			if (dist > volumeRadius) { // bounce back

				Vector3 yPrimeAxis = new Vector3 (position.x, 0, position.z);
				yPrimeAxis.Normalize ();
				Vector3 xPrimeAxis = new Vector3 (-position.z, 0, position.x);
				xPrimeAxis.Normalize ();

				float primeY = Vector3.Dot (speed, yPrimeAxis);
				float primeX = Vector3.Dot (speed, xPrimeAxis);

				primeY = -primeY; // invert radial direction

				Vector3 newSpeed = primeY * yPrimeAxis + primeX * xPrimeAxis;

				speed = newSpeed;
			}
		}
		theMesh.transform.position = position;
		theShadow.transform.position = position + smallDelta;

		// track following bit

		SentenceBit nextBit = parent.getPiece (this.order+1);
		if (nextBit == null)
			return;
		// find out if next piece is properly aligned

		float nextPieceCenterX;
		float nextPieceCenterY;
		float nextPieceWidth;

		Bounds nextPieceBounds = nextBit.theMesh.GetComponent<Renderer> ().bounds;
		BoxCollider nextBoxCollider = nextBit.theCollider;

		nextPieceCenterX = -nextPieceBounds.center.z;
		nextPieceCenterY = nextPieceBounds.center.x;
		nextPieceWidth = nextBoxCollider.size.x;

		float thisPieceCenterX;
		float thisPieceCenterY;
		float thisPieceWidth;

		Bounds thisPieceBounds = theMesh.GetComponent<Renderer> ().bounds;

		thisPieceCenterX = -thisPieceBounds.center.z;
		thisPieceCenterY = thisPieceBounds.center.x;
		thisPieceWidth = theCollider.size.x;

		float deltaX, deltaY;

		deltaX = nextPieceCenterX - thisPieceCenterX;
		deltaY = nextPieceCenterY - thisPieceCenterY;

		if ((Mathf.Abs(deltaY) < (theCollider.size.y / 3.0)) &&

			 (deltaX > (thisPieceWidth / 2.0 + nextPieceWidth / 2.0) &&
				deltaX < (thisPieceWidth / 2.0 + nextPieceWidth / 2.0) * 1.7)) { // also horizontally aligned

				if(correctlyAligned) nextBit.setCorrectlyAligned(true);

				chainedText = ownText + " " + nextBit.chainedText;
				lines = nextBit.lines;
				nextBit.lineNumber = lineNumber;

			if (order == 0) 
			{
				parent.setLines (lines);	
				parent.setText (chainedText);
			}			
		}
		else if ((deltaX < 0.0f) //-(thisPieceWidth + nextPieceWidth))
			&&  (deltaY < -(theCollider.size.y * 1.0)))
			{ // next line possibility
			if(correctlyAligned) nextBit.setCorrectlyAligned(true);
			chainedText = ownText + " " + nextBit.chainedText;
			nextBit.lineNumber = 1 + lineNumber;
			lines = 1 + nextBit.lines;
		}

		else { // no aligment of any sort
			nextBit.setCorrectlyAligned(false);
			chainedText = ownText;
			lines = 0;
			if(order == 0) parent.setText ("");
		}
	}

	public void setTargetPosition(float z, float x) 
	{
		Vector3 pos = new Vector3 (x, 0, z);
		targetPosition = pos;

		speed = Vector3.zero;
		pos = this.theMesh.transform.position;

		targetSpeedDirection = targetPosition - pos;
		//targetSpeedDirection.Normalize ();
		elapsedTime = 0.0f;

		isMoving = true;
		hasTargetCoords = true;

		state = SentenceBitState.Wait1;
		elapsedTime = 0.0f;
	}
}
