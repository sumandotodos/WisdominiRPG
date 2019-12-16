using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum CameraManagerState { idle, moving }
public enum CameraSituation { interior, exterior }

public class CameraManager : WisdominiObject {

	public float DistanceZ = 0;

	public Vector3 offset;

	/* references */

	public GameObject[] marker;
	CameraUtils cameraTremor;
	CameraFollowAux cameraFollowAux;

	/* properties */

	public Vector3 destination;
	Vector3 lerpPosition;
	Vector3 posBase;

	public CameraManagerState state;

	LevelControllerScript level;
	public GameObject player_A;
	public string estado;

	public GameObject currentTarget_A;
	public GameObject target;
	public bool lookingTarget = true;
	public float timeToTarget;

	//[HideInInspector]
	public GameObject pivotY;
	//[HideInInspector]
	public GameObject pivotX;
	//[HideInInspector]
	public GameObject pivotZ;
	//[HideInInspector]
	public GameObject mainCamera;

	public GameObject whereTheCameraShouldBe;

	float rotY;
	float rotX;
	float movZ;
	float movM;

	public float targetY;
	public float y;
	public SoftFloat softY;
	public SoftFloat softX;
	public SoftFloat softZ;

	//public float timeRotY;
	public float speedRotY;
	float timeRotX;
	float timeMoveZ;
	float timeMoveM;

	public bool snap;
	public Vector3 snapPoint;

	//[HideInInspector]
	public EnumFreeAxis freeAxis;

	//[HideInInspector]
	public CameraSituation camSit;

	DataStorage ds;


	void Awake()
	{
		GameObject lgo = GameObject.Find ("LevelController");
		if (lgo != null) {
			level = lgo.GetComponent<LevelControllerScript> ();
		}
		player_A = GameObject.Find ("Player");
		//warpToOriginalPosition ();

		softY = new SoftFloat ();
		softY.setTransformation (TweenTransforms.cubicOut);
		softX = new SoftFloat ();
		softX.setTransformation (TweenTransforms.cubicOut);
		softZ = new SoftFloat ();
		softZ.setTransformation (TweenTransforms.cubicOut);

		if (player_A != null) {
			if (player_A.GetComponent<PlayerScript> ().targetLookAt != null)
				target = player_A.GetComponent<PlayerScript> ().targetLookAt;
		}
		//		setTargetX (25, 0.1f);
		//		setTargetY (0, 0.1f);
		//		setDistanceZ (10, 0.1f);

		lgo = GameObject.Find ("PhysicalCameraFollow");
		if (lgo != null) {
			cameraTremor = lgo.GetComponentInChildren<CameraUtils> (); 
			cameraFollowAux = lgo.GetComponent<CameraFollowAux> ();
		}

        iTween.Init(this.gameObject);

        /*if(SceneManager.GetActiveScene().name.ToLower().Contains("interior"))
        {
            setDistanceZ(8.0f);
        }

        if (SceneManager.GetActiveScene().name.ToLower().Contains("exterior"))
        {
            setDistanceZ(10.0f);
        }*/

    }

    public void StoreCameraAngles()
    {
        float rx = FGUtils.normalizeAngleBalanced(pivotX.transform.eulerAngles.x);
        float ry = FGUtils.normalizeAngleBalanced(pivotY.transform.eulerAngles.y);
        Debug.Log("<color=red>Storing = rx: " + rx + ", ry: " + ry + "</color>");
        ds.storeFloatValue("Rot" + level.locationName + "Y", ry);
        ds.storeFloatValue("Rot" + level.locationName + "X", rx);
    }

    public void Initialize()
	{
        //state = CameraManagerState.idle;
        iTween.Init(this.gameObject);
        ds = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ().getStorage ();
		// No inicializarlo aquí por problemas al recargar niveles en ciertas posiciones y/o rotaciones

		float x, y, z;

		x = ds.retrieveFloatValue ("Pos" + level.locationName + "X");
		y = ds.retrieveFloatValue ("Pos" + level.locationName + "Y");
		z = ds.retrieveFloatValue ("Pos" + level.locationName + "Z");

		if ((x != 0.0f) && (y != 0.0f) && (z != 0.0f)) {
			Vector3 newPos = new Vector3 (x, y, z);
			//warpToPosition (x, y, z);
			//destination = newPos;
		} else {
			//warpToOriginalPosition ();
		}

		float rx, ry, pz, pm;
		ry = ds.retrieveFloatValue ("Rot" + level.locationName + "Y");
		rx = ds.retrieveFloatValue ("Rot" + level.locationName + "X");
		pz = ds.retrieveFloatValue ("PosIn" + level.locationName + "Z");

		Debug.Log ("<color=green>Camera state load: P(" + x +
			"," + y + "," + z +
			")  R(" + ry + "," + rx +
			"," + pz + ")</color>");

        //pm = ds.retrieveFloatValue ("PosIn" + level.locationName + "M");

        float actualRx = Mathf.Abs(rx) < 0.01f ? 25.0f : rx;
        setImmediateX(actualRx);
		setImmediateY (ry);
//		setImmediateZ (pz);
	
		if (level.CameraXAngleOverride != 0) {
			setTargetX (level.CameraXAngleOverride, 0.1f);
		}
		else
		    setTargetX (actualRx, 0.1f);
		


		if (pz != 0) 
		{
			setDistanceZ (Mathf.Abs(pz), 0.1f);
		}
		else {
			if (level.CameraZDistOverride != 0) {
				setDistanceZ (level.CameraZDistOverride, 0.1f);
			}
			else
			setDistanceZ (10, 0.1f);
		}

//		if (pm = !0) 
//		{
//			SetDistanceM (pm, 0.1f);
//		}
        

		if (level.locationName.Contains ("exterior")) 
		{
			camSit = CameraSituation.exterior;
		} 
		else 
		{
			camSit = CameraSituation.interior;
		}
	}

	new void Update () {

		if (lookingTarget) 
		{
			currentTarget_A = (target == null) ? this.gameObject : target;
        } 
		else 
		{
			currentTarget_A = this.gameObject;
        }
               
        iTween.MoveUpdate(this.gameObject, iTween.Hash("position", destination, "lookTarget", currentTarget_A, "time", timeToTarget));

        //this.transform.position = Vector3.Lerp(this.transform.position, destination, 0.25f);
//		if (pivotY != null)
//		iTween.RotateUpdate (pivotY, iTween.Hash ("rotation", Vector3.up * rotY, "time", timeRotY, "islocal", true));
		if (pivotY != null) {
            //dT.text += "\n   >>>: pivoyy != null";
            //			if (y < targetY) {
            //				y += speedRotY * Time.deltaTime;
            //				if (y > targetY) {
            //					y = targetY;
            //				}
            //				pivotY.transform.rotation = Quaternion.Euler (new Vector3 (0, y, 0));
            //			}
            //			if (y > targetY) {
            //				y -= speedRotY * Time.deltaTime;
            //				if (y < targetY) {
            //					y = targetY;
            //				}
            //				pivotY.transform.rotation = Quaternion.Euler (new Vector3 (0, y, 0));
            //			}
            //			float yChangeSpeed = (targetY - y);
            //			if (yChangeSpeed > 8.0f)
            //				yChangeSpeed = 8.0f;
            //			if (yChangeSpeed < -8.0f)
            //				yChangeSpeed = -8.0f;
            //			y += yChangeSpeed * speedRotY * Time.deltaTime;
            //			pivotY.transform.rotation = Quaternion.Euler (new Vector3 (0, y, 0));
            softY.update();
			y = softY.getValue ();
			pivotY.transform.rotation = Quaternion.Euler (new Vector3 (0, softY.getValue (), 0));

		}
		if (pivotX != null)
		iTween.RotateUpdate(pivotX, iTween.Hash("rotation", Vector3.right * rotX, "time", timeRotX, "islocal", true));
		if (pivotZ != null)
		iTween.MoveUpdate(pivotZ, iTween.Hash("position", Vector3.back * movZ, "time", timeMoveZ, "islocal", true));
		if (mainCamera != null)
		iTween.MoveUpdate(mainCamera, iTween.Hash("position", Vector3.forward * movM, "time", timeMoveM, "islocal", true));
 
        if (state == CameraManagerState.idle) 
		{
            timeToTarget = 2;
			destination = player_A.transform.position + offset;
			posBase = this.transform.position;
		}

		if (state == CameraManagerState.moving) 
		{
            if (!snap) {
				switch (freeAxis) {
				case EnumFreeAxis.none:
				//destination = player.transform.position;
					break;

				case EnumFreeAxis.x:
					destination = new Vector3 (player_A.transform.position.x, this.transform.position.y, this.transform.position.z) + offset;
					break;

				case EnumFreeAxis.y:
					destination = new Vector3 (this.transform.position.x, player_A.transform.position.y, this.transform.position.z) + offset;
					break;

				case EnumFreeAxis.z:
					destination = new Vector3 (this.transform.position.x, this.transform.position.y, player_A.transform.position.z) + offset;
					break;
				}
			} else {
				switch (freeAxis) {
				case EnumFreeAxis.none:
					//destination = player.transform.position;
					break;

				case EnumFreeAxis.x:
					destination = new Vector3 (player_A.transform.position.x, player_A.transform.position.y, snapPoint.z) + offset;
					break;

				case EnumFreeAxis.y:
					destination = new Vector3 (snapPoint.x, player_A.transform.position.y, snapPoint.z) + offset;
					break;

				case EnumFreeAxis.z:
					destination = new Vector3 (snapPoint.x, player_A.transform.position.y, player_A.transform.position.z) + offset;
					break;
				}
			}

			if (estado == "ir") 
			{
				if (Mathf.Abs((this.transform.position - destination).magnitude) < 1)
				{
					notifyFinishAction ();
				}
			}

            if (estado == "volver") 
			{

				if (Mathf.Abs((this.transform.position - destination).magnitude) < 1)
				{
					state = CameraManagerState.idle;
					notifyFinishAction ();
					estado = "ir";
				}

            }

		}
	}

	public void setImmediateY(float angle) {
		pivotY.transform.rotation = Quaternion.Euler (new Vector3 (0, angle, 0));
		//targetY = y = angle;
		softY.setValueImmediate(angle);
	}
	public void setImmediateX(float angle) {
		pivotX.transform.rotation = Quaternion.Euler (new Vector3 (angle, 0, 0));
	}
	public void setImmediateZ(float dist) {
		pivotZ.transform.localPosition = new Vector3 (0, 0, dist);
	}

	public void moveRelative(Vector3 rel) {


		destination = rel;
		timeToTarget = 2;
		state = CameraManagerState.moving;
		estado = "volver";
	}

	public void moveToMarker(int m, float t = 3) {


        destination = marker [m].transform.position;
		timeToTarget = t;
		estado = "ir";
		state = CameraManagerState.moving;
	}

	public void moveToPoint(Vector3 p, float t = 3)
	{

        destination = p;
		timeToTarget = t;
		estado = "ir";
		state = CameraManagerState.moving;
	}

	public void warpToPosition(float _x, float _y, float _z)
	{

        this.transform.position = new Vector3 (_x, _y, _z);
		destination = new Vector3 (_x, _y, _z);
		timeToTarget = 0.01f;
		estado = "volver";
		state = CameraManagerState.moving;
	}

	public void warpToMarker(int m) {

        destination = marker [m].transform.position;
		timeToTarget = 0.01f;
		estado = "ir";
		state = CameraManagerState.moving;
	}

	public void warpToOriginalPosition() {

        this.transform.position = player_A.transform.position + offset;
		destination = player_A.transform.position + offset;
		timeToTarget = 0.01f;
		estado = "volver";
		state = CameraManagerState.moving;
	}

	public void _wm_warpToOriginalPosition() {
		warpToOriginalPosition();
	}


	public void moveToOriginalPosition(float t = 3) {

        destination = player_A.transform.position + offset;
		timeToTarget = t;
		estado = "volver";
		state = CameraManagerState.moving;
	}

	public void _wm_warpToMarker(int m) 
	{
		warpToMarker (m);
	}

	public void _wa_warpToMarker(WisdominiObject w, int m) {

		w.isWaitingForActionToComplete = true;
		waitingRef = w;
		warpToMarker (m);
	}

	public void _wa_moveToMarker(WisdominiObject w, int m) {

		w.isWaitingForActionToComplete = true;
		waitingRef = w;
		moveToMarker (m);
	}

	public void _wa_moveRelative(WisdominiObject w, Vector3 rel) {

		w.isWaitingForActionToComplete = true;
		waitingRef = w;
		moveRelative (rel);
	}

	public void _wa_moveToOriginalPosition(WisdominiObject w) 
	{
		w.isWaitingForActionToComplete = true;
		waitingRef = w;
		moveToOriginalPosition ();
	}

	public void _wm_moveToMarker(int m)
	{
		moveToMarker (m);
	}

	public void _wm_moveToOriginalPosition()
	{
		moveToOriginalPosition ();
	}

	public void _wm_setTargetY(float y, float t) 
	{
		setTargetY (y, t);
	}
	public void _wm_setTargetX(float x, float t) 
	{
		setTargetX (x, t);
	}
	public void _wm_setDistanceZ(float z, float t) 
	{
		setDistanceZ (z, t);
	}

	public void setTargetY(float ny = 0, float time = 2) 
	{
//		rotY = y;
		speedRotY = (1.0f / time) * Mathf.Abs(y-ny);
//		level.storeFloatValue (level.locationName + "CameraPivotTargetY", y);
		//targetY = ny;
		softY.setValue(ny);
		softY.setSpeed (speedRotY);
		targetY = ny;

	}

	public void setTargetX(float x = 0, float time = 2) 
	{
		rotX = x;
		timeRotX = time;
		level.storeFloatValue (level.locationName + "CameraPivotTargetX", x);
	}

    public float GetDistanceZ()
    {
        return DistanceZ;
    }

    public void setDistanceZ(float z = 0, float time = 2) 
	{
		if (z == 0) {
			PlayerScript pl = null;
			pl.blockControls (); // make it fail
		}
		DistanceZ = z;
		movZ = z;
		timeMoveZ = time;
		level.storeFloatValue (level.locationName + "CameraDistance", z);
	}

	public void SetDistanceM (float m = 0, float time = 2)
	{
		movM = m;
		timeMoveM = time;
		level.storeFloatValue (level.locationName + "CameraPivotDistanceMain", m);
	}

	public void Block2Axis(EnumFreeAxis _axis, float _snapPointX = 0, float _snapPointY = 0, float _snapPointZ = 0)
	{

        state = CameraManagerState.moving;
		lookingTarget = false;
		freeAxis = _axis;

		snapPoint = new Vector3 (_snapPointX, _snapPointY, _snapPointZ);

		if (snapPoint == Vector3.zero) {
			snap = false;
		} else {
			snap = true;
		}
	}

	public void UnBlockAllAxis()
	{
		freeAxis = EnumFreeAxis.none;
		state = CameraManagerState.idle;
		lookingTarget = true;
	}

	public void _wm_enableLookAt() 
	{
		lookingTarget = true;
	}

	public void _wm_disableLookAt() 
	{
		lookingTarget = false;
	}

	public void _wm_startTremble(float freq, float amp) {
		if (cameraTremor != null) {
			cameraTremor.frequency = freq;
			cameraTremor.amplitude = amp;
			cameraTremor._wm_setActive (true);
		}
	}

	public void _wm_stopTremble() {
		if (cameraTremor != null)
			cameraTremor._wm_setActive (false);
	}

	public void setCollisionsEnabled(bool en) {
		cameraFollowAux.setCollisionsEnabled (en);
	}
}
