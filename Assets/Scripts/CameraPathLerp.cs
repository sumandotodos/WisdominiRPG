using UnityEngine;
using System.Collections;

enum CameraPathLerpState2 { idle2, moving2 }

public class CameraPathLerp : WisdominiObject {
	
	/* references */

	public GameObject[] marker;

	/* properties */

	Vector3 destination;
	Vector3 lerpPosition;
	Vector3 posBase;

	CameraPathLerpState2 state;

	GameObject player;
	string estado;
	GameObject currentTarget;
	public GameObject target;
	public bool lookingTarget = true;


	void Awake()
	{
		player = GameObject.Find ("Player");
		this.transform.position = player.transform.position;
	}

	new void Start () {

		destination = player.transform.position;
		state = CameraPathLerpState2.idle2;
	}
	
	new void Update () {

		if (lookingTarget) 
		{
			currentTarget = target;
		} 
		else 
		{
			currentTarget = this.gameObject;
		}

		iTween.MoveUpdate(this.gameObject, iTween.Hash("position", destination, "lookTarget", currentTarget, "time", 2f));

		if (state == CameraPathLerpState2.idle2) {

			destination = player.transform.position;
			posBase = this.transform.position;
		}

		if (state == CameraPathLerpState2.moving2) 
		{
			if (estado == "volver") 
			{
				state = CameraPathLerpState2.idle2;
			}
			notifyFinishAction ();
		}
	}

	public void moveRelative(Vector3 rel) {

		destination = rel;
		state = CameraPathLerpState2.moving2;
		estado = "volver";
	}

	public void moveToMarker(int m) {

		destination = marker [m].transform.position;
		estado = "ir";
		state = CameraPathLerpState2.moving2;
	}

	public void warpToMarker(int m) {

		destination = marker [m].transform.position;
		estado = "ir";
		state = CameraPathLerpState2.moving2;
	}

	public void warpToOriginalPosition() {

		destination = player.transform.position;
		estado = "volver";
		state = CameraPathLerpState2.moving2;
	}


	public void moveToOriginalPosition() {

		destination = player.transform.position;
		estado = "volver";
		state = CameraPathLerpState2.moving2;
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

	public void _wm_enableLookAt() 
	{
		lookingTarget = true;
	}

	public void _wm_disableLookAt() 
	{
		lookingTarget = false;
	}
}
