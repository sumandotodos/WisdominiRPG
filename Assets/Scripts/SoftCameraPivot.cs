using UnityEngine;
using System.Collections;

public class SoftCameraPivot : WisdominiObject {

	public LevelControllerScript level;

	public GameObject player;
	public GameObject pivotX;
	public GameObject cam;
	public GameObject cameraLerp;
	public float cameraDistance = 6.2f;
	public float cameraDistanceSpeed = 20.0f;
	[HideInInspector]
	public float defaultCameraDistanceSpeed = 20.0f;

	public float targetCameraDistance = 6.2f;

	public float targetY; //
	public float targetX; // angles
	public float targetZ; //

	public float targetInclination;

	public float originalInclination;

	public float speed;
	public float Xspeed, Yspeed;
	[HideInInspector]
	public float defaultSpeed = 100.0f;
	[HideInInspector]
	public float defaultXSpeed = 100.0f;

	public float X, Y, Z; // angles
	public float inclination;

	public float xHold, yHold, zHold;

	public float maxAxisCoordinate, minAxisCoordinate;

	bool enabled;

	bool held = false;
	int holdAxis;

	const float cameraZ = -6.2f;

	Vector3 originalCamPosition;

	float deltax, deltay, deltaz;
	float targetdx, targetdy, targetdz;
	float deltaSpeed = 6.0f;

	// Use this for initialization
	new void Start () {

		if (level == null) { // autoconnect level controller
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		}
		enabled = true;
		deltax = deltay = deltaz = targetdx = targetdy = targetdz = 0;
		X = targetX;
		Y = targetY;
		Z = targetZ;
		Xspeed = defaultXSpeed;
		Yspeed = defaultXSpeed;

		targetInclination = inclination = originalInclination = pivotX.transform.eulerAngles.x;
		originalCamPosition = cam.transform.position;
		/*
		// reentry
		string reentryCondition = level.retrieveStringValue ("ReentryCondition");
		if (!reentryCondition.Equals ("MirrorActivation")) {


			targetInclination = level.retrieveFloatValue (level.locationName + "CameraPivotTargetX");
			targetY = level.retrieveFloatValue (level.locationName + "CameraPivotTargetY");
			if (targetInclination != originalInclination) {
				inclination = targetInclination;
				pivotX.transform.localRotation = Quaternion.Euler (inclination, 0, 0);
			}
			if (targetY != 0.0f) {
				Y = targetY;
				this.transform.localRotation = Quaternion.Euler (0, Y, 0);
			}

		} else {
			inclination = targetInclination = 0.0f;
			Y = targetY = 0.0f;
			pivotX.transform.localRotation = Quaternion.Euler (inclination, 0, 0);
			this.transform.localRotation = Quaternion.Euler (0, Y, 0);
		}
		*/
		//cam.transform.localPosition = new Vector3 (0, 0, -cameraDistance);


	}
	/*
	public void _wm_setTargetY(float t) 
	{
		setTargetY (t);
	}
	public void _wm_setTargetX(float t) 
	{
		setTargetX (t);
	}
	public void _wm_setDistance(float d) 
	{
		setDistance (d);
	}

	public void setTargetY(float t) 
	{
		targetY = t;
		level.storeFloatValue (level.locationName + "CameraPivotTargetY", t);
	}

	public void setTargetX(float t) 
	{
		targetInclination = t;
		level.storeFloatValue (level.locationName + "CameraPivotTargetX", t);
	}

	public void setDistance(float d) 
	{
		targetCameraDistance = d;
		level.storeFloatValue (level.locationName + "CameraDistance", d);
	}
		
	public void setEnabled(bool en) {
		enabled = en;
	}
*/
	// Update is called once per frame
	new void Update () {

		if (!enabled)
			return;
		/*
		if (inclination < targetInclination) {
			inclination += Xspeed * Time.deltaTime;
			if (inclination > targetInclination) {
				inclination = targetInclination;
			}
		}
		if (inclination > targetInclination) {
			inclination -= Xspeed * Time.deltaTime;
			if (inclination < targetInclination) {
				inclination = targetInclination;
			}
		}

		if (X < targetX) {
			X += speed * Time.deltaTime;
			if (X > targetX)
				X = targetX;
		}
		if (Y < targetY) {
			Y += speed * Time.deltaTime;
			if (Y > targetY)
				Y = targetY;
		}
		if (Z < targetZ) {
			Z += speed * Time.deltaTime;
			if (Z > targetZ)
				Z = targetZ;
		}
		if (X > targetX) {
			X -= speed * Time.deltaTime;
			if (X < targetX)
				X = targetX;
		}
		if (Y > targetY) {
			Y -= speed * Time.deltaTime;
			if (Y < targetY)
				Y = targetY;
		}
		if (Z > targetZ) {
			Z -= speed * Time.deltaTime;
			if (Z < targetZ)
				Z = targetZ;
		}

		bool dChange = Utils.updateSoftVariable (ref cameraDistance, targetCameraDistance, cameraDistanceSpeed);
		//if (dChange) {
			cam.transform.localPosition = new Vector3 (0, 0, -cameraDistance);
		//}

		pivotX.transform.localRotation = Quaternion.Euler (inclination, 0, 0); // inclination is relative!
			// the reason of that being is:
			// when we reenter, the first time, inclination will be zero

		this.transform.localRotation = Quaternion.Euler (X, Y, Z);

		targetdx = 0; targetdy = 0; targetdz = 0;
		if (held) {
			// RESTRINGIR AXIS

			if(holdAxis!=0) targetdx = -(player.transform.position.x - xHold);
			if(holdAxis!=1) targetdy = -(player.transform.position.y - yHold);
			if(holdAxis!=2) targetdz = -(player.transform.position.z - zHold);


		} 

		if (deltax < targetdx) {
			deltax += deltaSpeed * Time.deltaTime;
			if (deltax > targetdx)
				deltax = targetdx;
		}
		if (deltax > targetdx) {
			deltax -= deltaSpeed * Time.deltaTime;
			if (deltax < targetdx)
				deltax = targetdx;
		}

		if (deltay < targetdy) {
			deltay += deltaSpeed * Time.deltaTime;
			if (deltay > targetdy)
				deltay = targetdy;
		}
		if (deltay > targetdy) {
			deltay -= deltaSpeed * Time.deltaTime;
			if (deltay < targetdy)
				deltay = targetdy;
		}

		if (deltaz < targetdz) {
			deltaz += deltaSpeed * Time.deltaTime;
			if (deltaz > targetdz)
				deltaz = targetdz;
		}
		if (deltaz > targetdz) {
			deltaz -= deltaSpeed * Time.deltaTime;
			if (deltaz < targetdz)
				deltaz = targetdz;
		}

		Vector3 newPos = new Vector3 (deltax, deltay, deltaz);
		this.transform.localPosition = newPos;
	*/
	}
	/*
	public void holdAt(Vector3 pos) {
		held = true;
		holdAxis = -1;
		xHold = pos.x;
		yHold = pos.y;
		zHold = pos.z;
	}

	public void hold() {

		held = true;
		holdAxis = -1;
		xHold = player.transform.position.x;
		yHold = player.transform.position.y;
		zHold = player.transform.position.z;

	}

	public void unhold() {

		held = false;

	}*/

	void FixedUpdate() {

		Vector3 dir = cam.transform.position - player.transform.position;

		RaycastHit hit;

		int layerMask = 1 << 8;
		layerMask = ~layerMask;


		float myZ = cameraZ;



		//cam.GetComponent<Camera>().fieldOfView = 55;


		/*
		if (Physics.Raycast (player.transform.position, dir, out hit, layerMask)) {
			if (hit.collider.tag == "Wall") {
				if (hit.distance < dir.magnitude) {
					cam.GetComponent<Camera> ().fieldOfView = 85.0f;
				} 
					
			}
		}*/


		/*

			Lets do this some other way

		*/
		RaycastHit[] hits;
		hits = Physics.RaycastAll (player.transform.position, dir, 100.0f);
		for (int i = 0; i < hits.Length; ++i) {
			if (hits [i].collider.tag == "Wall") {
				if (hits[i].distance < Mathf.Abs(cameraZ)) {
					myZ = -hits[i].distance;

					//cam.GetComponent<Camera>().fieldOfView = 85;
				} 
			}
		}

		cam.transform.localPosition = new Vector3 (0, 0, myZ);
			

	}
	/*
	public void lockAxis1D(int axis, Vector3 lockPoint) {

		switch (axis) {

			case 0: // X axis
				holdAxis = 0;
				yHold = lockPoint.y;
				zHold = lockPoint.z;
				break;

			case 1: // Y axis
				holdAxis = 1;
				xHold = lockPoint.x;
				zHold = lockPoint.z;
				break;

			case 2: // Z axis
				holdAxis = 2;
				xHold = lockPoint.x;
				yHold = lockPoint.y;
				break;

		}
		cam.GetComponent<CameraManager> ()._wm_disableLookAt ();
		held = true;
	}

	public void setlockAxisMinCoord(float m) 
	{
		minAxisCoordinate = m;
	}

	public void setlockAxisMaxCoord(float m) 
	{
		maxAxisCoordinate = m;
	}

	public void unlockAxis() {

		holdAxis = -1;
		cam.GetComponent<CameraManager> ()._wm_enableLookAt ();
		held = false;
	}

	public void enableCameraLookAt() 
	{
		cameraLerp.GetComponent<CameraManager> ()._wm_enableLookAt ();
	}

	public void disableCameraLookAt() 
	{
		cameraLerp.GetComponent<CameraManager> ()._wm_disableLookAt ();
	}

	public void _wm_enableCameraLookAt() 
	{
		cameraLerp.GetComponent<CameraManager> ()._wm_enableLookAt ();
	}

	public void _wm_disableCameraLookAt() 
	{
		cameraLerp.GetComponent<CameraManager> ()._wm_disableLookAt ();
	}

*/
}
