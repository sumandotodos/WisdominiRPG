using UnityEngine;
using System.Collections;

public class HeavyLevitatingFollower : MonoBehaviour {

	/* references */

	public AudioClip sonido;

	LevelControllerScript level;

	public GameObject roca;
	public GameObject gema;

	public PlayerScript player;
	public float meanHeight = 4.0f;
	public float angleSpeed = 6.0f;
	public float amplitude = 4.0f;
	public float k = 1.0f;
	public float nearRadius = 2.0f;
	public float speed = 6.0f;
	Animator anim;

	public float yOffset = 0.0f;

	Rigidbody r;

	bool markedForHiding = false;
	const float hidingDelay = 0.5f;
	float hidingTimer;

	/* properties */

	float targetY;
	float angle = 0;


	public void initialize () 
	{
		anim = roca.GetComponent<Animator> ();

		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
	
		r = this.GetComponent<Rigidbody> ();
		angle = FloatRandom.floatRandomRange (0.0f, 6.28f);

	}

	public void immediateBreak() 
	{
		//roca.GetComponent<Renderer> ().enabled = false;
		anim.SetBool ("Roto", true);

		//Destroy (roca.gameObject, tiempoDesaparece);
		markedForHiding = true;
		hidingTimer = 2.0f;

		level.playSound (sonido);
	}

	public void Break ()
	{
		anim.SetBool ("Roto", true);
		//Destroy (roca.gameObject, tiempoDesaparece);
		markedForHiding = true;
		hidingTimer = 0.0f;

		level.playSound (sonido);
	}

	void Start() 
	{
		initialize ();
	}
	
	void Update () 
	{
		//this.transform.LookAt(new Vector3(player.targetLookAt.transform.position.x, this.transform.position.y, player.targetLookAt.transform.position.z));
		angle += angleSpeed * Time.deltaTime;

		targetY = yOffset + meanHeight + amplitude * Mathf.Sin (angle);

		if (markedForHiding) {
			hidingTimer += Time.deltaTime;
			if (hidingTimer > hidingDelay) {
				markedForHiding = false;
				roca.SetActive (false);
				gema.SetActive (true);
			}
		}
	}

	void FixedUpdate() 
	{
		float xForce, zForce;

		if (player == null)
			return;
		Vector3 playerPos = player.transform.position;
		playerPos.y = 0;

		Vector3 thisPos = this.transform.position;
		thisPos.y = 0;

		Vector3 deltaVec = playerPos - thisPos;
		if (deltaVec.magnitude < nearRadius) {
			xForce = zForce = 0;
		} else {
			float m = deltaVec.magnitude;
			deltaVec.Normalize ();
			deltaVec *= (m - nearRadius);
			deltaVec *= speed;
			xForce = deltaVec.x;
			zForce = deltaVec.z;
		}

		float yForce = k * (targetY - this.transform.position.y);
		r.velocity = new Vector3 (xForce, yForce, zForce);
	}
}
