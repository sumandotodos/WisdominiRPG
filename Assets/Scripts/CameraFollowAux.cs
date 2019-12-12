using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// editado por Emilio el 6 de sep: agregar k a la ecuación, que no hace nada

[System.Serializable]
public struct IntermediateTarget {

	public Vector3 vector;
	public bool autoconsume;

	public IntermediateTarget(Vector3 v, bool b) {
		vector = v;
		autoconsume = b;
	}

}

public class CameraFollowAux : WisdominiObject {

	public GameObject target;
	public Vector3 targetCoords;
	public Camera cam;
	public float distToTarget;
	public float disableCollisionFarDistance = 8.0f;
	public float enableCollisionNearDistance = 2.0f;
	public bool collisionsEnabled;

	public List<IntermediateTarget> intermediateTarget;

	Rigidbody r;
	public float k = 1.0f;
	public float nearRadius = 0.05f;
	public float speed = 6.0f;
	public float xForce, yForce, zForce;
	public float maximumMagnitude = 80.0f;
    float recoilTime = 0.25f;
    float recoilRemain;

	float remainigTime;

    int frames = 0;

	// Use this for initialization
	void Start () {
        recoilRemain = recoilTime;
		intermediateTarget = new List<IntermediateTarget> ();
		collisionsEnabled = true;
		r = this.GetComponent<Rigidbody> ();
		r.velocity = Vector3.zero;
		GameObject player = GameObject.Find ("Player");
		this.transform.position = player.transform.position;
	}

	public void setCollisionsEnabled(bool en) {
		collisionsEnabled = en;
	}

	public void _wm_addIntermediateLocation (float x, float y, float z, bool consume) {
		Vector3 newVector = new Vector3 (x, y, z);
		addIntermediateLocation (newVector, consume);
	}

	public void _wm_stayWhereYouAre() {
		addIntermediateLocation (this.transform.position, false);
	}

	public void addIntermediateLocation(Vector3 thePosition, bool consume) {
		intermediateTarget.Add (new IntermediateTarget(thePosition, consume));
	}

	public void _wm_clearIntermediateLocations() {
		clearIntermediateLocations ();
	}

	public void clearIntermediateLocations() {
		intermediateTarget.Clear ();
	}

	void FixedUpdate() 
	{
        if(recoilRemain > 0.0f)
        {
            if(target!=null)
            {
                this.transform.position = target.transform.position;
            }
            recoilRemain -= Time.deltaTime;
            return;
        }
        if (target != null) {
			targetCoords = target.transform.position;
		}
		else 
			return;

		//Vector3 endPointCoords = targetCoords;

		if (intermediateTarget.Count > 0) {
			targetCoords = intermediateTarget [intermediateTarget.Count - 1].vector;
			//endPointCoords = intermediateTarget [0].vector;
		}

		Vector3 targetPos = targetCoords;
		//playerPos.y = 0;

		Vector3 thisPos = this.transform.position;
		//thisPos.y = 0;

		Vector3 deltaVec = targetPos - thisPos;

        //Vector3 endPointDelta = endPointCoords - thisPos;
        distToTarget = deltaVec.magnitude;
		if (collisionsEnabled) {
			if (deltaVec.magnitude > disableCollisionFarDistance) {
				r.detectCollisions = false;
			} else if (deltaVec.magnitude < enableCollisionNearDistance) {
				r.detectCollisions = true;
			}
		} else {
			r.detectCollisions = false;
		}

		if (deltaVec.magnitude < nearRadius) {

            xForce = zForce = yForce = 0;
			if (intermediateTarget.Count > 0) {
				if (intermediateTarget [intermediateTarget.Count - 1].autoconsume) {
					intermediateTarget.RemoveAt (intermediateTarget.Count - 1); // consume position
				}
			}
		} else {

            float m = deltaVec.magnitude;
			deltaVec.Normalize ();
			deltaVec *= (m - nearRadius/2);
			deltaVec *= speed;
			if (deltaVec.magnitude > maximumMagnitude) {
				deltaVec.Normalize ();
				deltaVec *= maximumMagnitude;
			}
			xForce = deltaVec.x;
			yForce = deltaVec.y;
			zForce = deltaVec.z;
		}

		//float yForce = k * (targetY - this.transform.position.y);
		r.velocity = new Vector3 (xForce, yForce, zForce);

		cam.transform.rotation = target.transform.rotation;

		if (remainigTime > 0.0f) {
			remainigTime -= Time.deltaTime;
			if (remainigTime <= 0.0f) {
				r.detectCollisions = true;
				collisionsEnabled = true;
			}
		}

    }

	public void disableCollision(float t) {
		remainigTime = t;
		r.detectCollisions = false;
		collisionsEnabled = false;
	}

	public void _wm_temporarilyDisableCollisions(float t) {
		disableCollision (t);
	}

	public void _wm_disableCollision() {
		collisionsEnabled = false;
		r.detectCollisions = false;
	}

	public void _wm_enableCollision() {
		collisionsEnabled = true;
		r.detectCollisions = true;
	}
}
