using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pivot { back, front };

public class SetCameraBacktracker : WisdominiObject {

	public GameObject frontIntermediate;
	public GameObject backIntermediate;
	LevelControllerScript level;
    MasterControllerScript mcRef;

	public float backTrackDistance;

	public bool showDistance = false;
	public float backToPlayerDistance;

	public bool enabled = true;
	public bool reentrant = false;

	public bool goToBackAtGoingIn = true;
	public bool goToBackAtComingOut = true;

    float reentryDelay = 1.0f;

	public int instanceNest;

	public static int nest = 0;

	public bool stickToFrontAtGoingIn = false;

	public CameraFollowAux cameraFollow;

	GameObject player = null;

	public Pivot pivotAt = Pivot.front;

	new void Start() {
		if (cameraFollow == null) {
			cameraFollow = GameObject.Find ("PhysicalCameraFollow").GetComponent<CameraFollowAux> ();
		}
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
        mcRef = FindObjectOfType<MasterControllerScript>();
		if (reentrant) {
			enabled = level.retrieveBoolValue ("is" + this.name + "Enabled");
		}
        reentryDelay = 1.0f;
	}

	void OnTriggerEnter(Collider other) {
        Debug.Log("<color=red>SetCameraBacktrack</color>");
        if (reentryDelay > 0.0f)
        {
            cameraFollow.intermediateTarget = mcRef.intermediateTarget;
            return;
        }
        else
        {
            mcRef.intermediateTarget = cameraFollow.intermediateTarget;
        }
        if (!enabled)
			return;
		if (other.tag == "Player") {
			if (nest == 0) { //level.retrieveIntValue("BacktrackNest") == 0) {
				cameraFollow.clearIntermediateLocations ();
			}
            //level.storeIntValue("BacktrackNest", level.retrieveIntValue("BacktrackNest") + 1);
            ++nest;
			float dist = getFlatDistance(backIntermediate.transform.position, other.transform.position);
			if (pivotAt == Pivot.front) {
				if (dist > backTrackDistance) { // if back entering
					Debug.Log ("<color=orange>BackTrack enter: back(" + dist + ")</color>");
					cameraFollow.addIntermediateLocation (frontIntermediate.transform.position, false);
					if(goToBackAtGoingIn)
					cameraFollow.addIntermediateLocation (backIntermediate.transform.position, true);
				} else { // front entering
					Debug.Log ("<color=orange>BackTrack enter: front(" + dist + ")</color>");
					if (stickToFrontAtGoingIn) {
						cameraFollow.addIntermediateLocation (frontIntermediate.transform.position, false);
					}

				}
			} else {
				if (dist < backTrackDistance) { // if front entering
					Debug.Log ("<color=orange>BackTrack enter: back(" + dist + ")</color>");
					cameraFollow.addIntermediateLocation (frontIntermediate.transform.position, false);
					if(goToBackAtGoingIn)
					cameraFollow.addIntermediateLocation (backIntermediate.transform.position, true);
				} else { // front entering
					Debug.Log ("<color=orange>BackTrack enter: front(" + dist + ")</color>");
					if (stickToFrontAtGoingIn) {
						cameraFollow.addIntermediateLocation (frontIntermediate.transform.position, false);
					}
				}
			}
		}




	}

	public float getFlatDistance(Vector3 pos1, Vector3 pos2) {
		Vector3 p1 = pos1;
		p1.y = 0;
		Vector3 p2 = pos2;
		p2.y = 0;
		return (p1 - p2).magnitude;
	}

	void Update() {
        instanceNest = nest;//.retrieveIntValue("BacktrackNest");
		if (showDistance) {
			if (player == null)
				player = GameObject.Find ("Player");
			backToPlayerDistance = getFlatDistance(player.transform.position, backIntermediate.transform.position);
		}
        if(reentryDelay > 0.0f)
        {
            reentryDelay -= Time.deltaTime;
        }
    }

	void OnTriggerExit(Collider other) {
        if (reentryDelay > 0.0f) return;
        if (!enabled)
			return;
		if (other.tag == "Player") {
            --nest; //level.storeIntValue("BacktrackNest", level.retrieveIntValue("BacktrackNest") - 1);
            if (nest==0) {//level.retrieveIntValue("BacktrackNest") == 0) { 
				cameraFollow.clearIntermediateLocations ();
			}
			// where are you exitting, front or back??
			float dist = getFlatDistance(backIntermediate.transform.position, other.transform.position);
			if (pivotAt == Pivot.front) {
				if (dist > backTrackDistance) { // if back exitting
					Debug.Log ("<color=grey>BackTrack exit: back (" + dist + ")</color>");
					if(goToBackAtComingOut)
					cameraFollow.addIntermediateLocation (backIntermediate.transform.position, true);
				} else {
					Debug.Log ("<color=grey>BackTrack exit: front (" + dist + ")</color>");
				}
			} else {
				if (dist < backTrackDistance) { // if back exitting
					Debug.Log ("<color=grey>BackTrack exit: back (" + dist + ")</color>");
					if(goToBackAtComingOut)
					cameraFollow.addIntermediateLocation (backIntermediate.transform.position, true);
				} else {
					Debug.Log ("<color=grey>BackTrack exit: front (" + dist + ")</color>");
				}
			}
		}
	}

	public void _wm_enable() 
	{
		if (reentrant) {
			level.storeBoolValue ("is" + this.name + "Enabled", true);
		}
		enabled = true;
	}

	public void _wm_disable() 
	{
		if (reentrant) {
			level.storeBoolValue ("is" + this.name + "Enabled", false);
		}
		enabled = false;
	}

}
