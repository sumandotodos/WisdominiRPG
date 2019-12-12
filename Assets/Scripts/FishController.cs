using UnityEngine;
using System.Collections;

public class FishController : MonoBehaviour {

	float bearing;
	float targetBearing;
	float bearingSpeed;

	Vector3 speed;
	Vector3 speedDirection;

	float fwdSpeed;
	float targetFwdSpeed;

	float timeToSlightChange;
	float timeToBigChange;

	const float minSlightChangeTime = 0.2f;
	const float maxSlightChangeTime = 2.0f;

	const float minBigChangeTime = 2.0f;
	const float maxBigChangeTime = 8.0f;

	const float minSlightChange = 0.2f;
	const float maxSlightChange = 10.0f;

	const float minBigChange = 15.0f;
	const float maxBigChange = 160.0f;

	float elapseTimeOfLastSlightChange = 0.0f;
	float elapseTimeOfLastBigChange = 0.0f;

	const float granularity = 10000.0f;

	public DynaMesh refractiveMesh;

	bool turningAround;

	public float volumeRadius;

	// Use this for initialization
	void Start () {
	
		fwdSpeed = targetFwdSpeed = 0.03f;
		speedDirection = new Vector3 (1, 0, 0);
		bearing = Random.Range (0.0f * granularity, 360.0f * granularity) / granularity;
		targetBearing = bearing;
		bearingSpeed = 0.25f;

		turningAround = false;

		timeToSlightChange = Random.Range (minSlightChangeTime * granularity, maxSlightChangeTime * granularity) / granularity;

		timeToBigChange = Random.Range (minBigChangeTime * granularity, maxBigChangeTime * granularity) / granularity;
	}
	
	// Update is called once per frame
	void Update () {

		if (bearing > targetBearing) {

			if (turningAround)
				bearing -= bearingSpeed * 4.0f;
			else 
				bearing -= bearingSpeed;
			if (bearing < targetBearing) {
				bearing = targetBearing;
				turningAround = false;
			}

		} else if (bearing < targetBearing) {

			if (turningAround)
				bearing += bearingSpeed * 4.0f;
			else 
				bearing += bearingSpeed;
			if (bearing > targetBearing) {
				bearing = targetBearing;
				turningAround = false;
			}

		}

		elapseTimeOfLastSlightChange += Time.deltaTime;
		elapseTimeOfLastBigChange += Time.deltaTime;

		if ((elapseTimeOfLastBigChange > timeToBigChange) && !turningAround) {

			elapseTimeOfLastBigChange = 0.0f;

			timeToBigChange = Random.Range (minBigChangeTime * granularity, maxBigChangeTime * granularity) / granularity;

			targetBearing = Random.Range (minSlightChange * granularity, maxSlightChange * granularity) / granularity;

		}

		if ((elapseTimeOfLastSlightChange > timeToSlightChange) && !turningAround ) {

			elapseTimeOfLastSlightChange = 0.0f;

			timeToSlightChange = Random.Range (minSlightChangeTime * granularity, maxSlightChangeTime * granularity) / granularity;

			targetBearing = Random.Range (minBigChange * granularity, maxBigChange * granularity) / granularity;

		}

		//bearing += 2f;

		this.transform.rotation = Quaternion.Euler (0, -bearing+180, 0);

		speed = new Vector3 (Mathf.Cos (bearing*2.0f*3.1416f/360.0f), 0, Mathf.Sin (bearing*2.0f*3.1416f/360.0f)); // unit vector
		speedDirection = new Vector3 (Mathf.Cos (bearing*2.0f*3.1416f/360.0f), 0, Mathf.Sin (bearing*2.0f*3.1416f/360.0f));
		speed *= fwdSpeed;

		Vector3 pos = this.transform.position;
		pos += speed;
		pos.y = 0;
		this.transform.position = pos;



		float radius;

		float x, z;

		x = this.transform.position.x-100.0f;
		z = this.transform.position.z;

		//refractiveMesh.disturb (-z, x, 0.5f, 1.3f);

		radius = Mathf.Sqrt (x*x + z*z);

		/*if ((radius > volumeRadius * 0.75) && !turningAround) { // out of bounds, calculate new bearing

			Vector3 yPrimeAxis = new Vector3 (x, 0, z);
			yPrimeAxis.Normalize ();
			Vector3 xPrimeAxis = new Vector3 (-z, 0, x);
			xPrimeAxis.Normalize ();

			float primeY = Vector3.Dot (speedDirection, yPrimeAxis);
			float primeX = Vector3.Dot (speedDirection, xPrimeAxis);

			primeY = -primeY; // invert radial direction

			Vector3 newSpeed = primeY * yPrimeAxis + primeX * xPrimeAxis;

			//speed = newSpeed;
			//speed /= fwdSpeed;
			targetBearing = Mathf.Acos ((float)newSpeed.x) * 360.0f / (2.0f * 3.1416f);
			if(newSpeed.z < 0) {
				targetBearing = 180.0f + (180.0f - targetBearing);
			}

			turningAround = true;

		}

		if (radius < volumeRadius * 0.65)
			turningAround = false;
		*/

	
	}


	void OnCollisionEnter(Collision col) {

		targetBearing += 180.0f;
		turningAround = true;

	}
}
