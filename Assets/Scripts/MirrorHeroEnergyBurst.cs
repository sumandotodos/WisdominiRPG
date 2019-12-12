using UnityEngine;
using System.Collections;

public class MirrorHeroEnergyBurst : WisdominiObject {

	/* references */

	public GameObject mirrorGO;
	public GameObject playerGO;
	PlayerScript player;
	ParticleSystem particles;

	/* properties */

	float t; // lerp space parameter [0.0f..1.0f]
	float tSpeed;
	Vector3 origin;
	Vector3 destination;
	[HideInInspector]
	public int state;
	float elapsedTime;
	float lifeTime;

	public float hAmplitude = 3.0f;
	public float freq = 4.0f;

	/* constants */

	const float startLifetime = 0.8f;
	const float lifetimeSpeed = 0.5f;
	const float acceleration = 1.2f;


	public void initialize() {

		player = playerGO.GetComponent<PlayerScript> ();
		origin = mirrorGO.transform.position;
		destination = playerGO.transform.position;
		t = 0;
		tSpeed = 0;
		state = 0;
		elapsedTime = 0.0f;
		lifeTime = 0.0f;
		particles = this.GetComponent<ParticleSystem> ();


	}

	// Use this for initialization
	new void Start () {

		initialize ();

	}
	
	// Update is called once per frame
	new void Update () {



		if (state == 0) {

			particles.startLifetime = lifeTime;
			lifeTime += lifetimeSpeed * Time.deltaTime;
			if (lifeTime > startLifetime) {
				++state;
			}

		}

		if (state == 1) {

			elapsedTime += Time.deltaTime;
			Vector3 pos = Vector3.Lerp (origin, destination, t);
			if (t >= 1.0f) {
				t = 1.0f;
				tSpeed = 0.0f;
				++state;
			} else {
				t += tSpeed * Time.deltaTime;
				tSpeed += acceleration * Time.deltaTime;
			}
			Vector3 deviation = new Vector3 ((t*(1-t)) * hAmplitude * Mathf.Sin (elapsedTime * freq), 0, 0);

			this.transform.position = (pos + deviation);

		}

		if (state == 2) {

			if(lifeTime > 0.0f) particles.startLifetime = lifeTime;
			lifeTime -= lifetimeSpeed * Time.deltaTime;
			if (lifeTime < 0.00f) {
				++state;
			}

		}
	
	}

	public void _wm_go() {

		state = 0;

	}
}
