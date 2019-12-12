using UnityEngine;
using System.Collections;

enum ChispaWorldState { idle, comingOut, Out, returning };

public class ChispaWorld : WisdominiObject {

	/* references */

	public Sprite[] images;
	public GameObject dustPrefab;
	Animator anim;

	/* public properties */

	public float animationSpeed;

	public float dustFrequency;

	public bool transitioning = false;

	/* properties */

	ChispaWorldState state;
	int frame;
	float elapsedTime;
	float timeToEmit;
	SpriteRenderer mat;

	void OnDrawGizmos() {

		if (images.Length > 0 && images [0] != null) {
			//this.GetComponent<Renderer> ().material.mainTexture = images [0];
			this.GetComponent<SpriteRenderer>().sprite = images[0];
		}

	}

	// Use this for initialization
	new void Start () {

		frame = 0;
		timeToEmit = 0.0f;
		elapsedTime = 0.0f;

		//mat = this.GetComponent<Renderer> ().material;
		mat = this.GetComponent<SpriteRenderer> ();
		anim = this.GetComponent<Animator> ();

		state = ChispaWorldState.idle;
		mat.color = new Color (0, 0, 0, 0);

		transitioning = false;
	
	}
	
	// Update is called once per frame
	new void Update () {

		elapsedTime += Time.deltaTime;
		if (elapsedTime > (1.0f / animationSpeed)) {

			frame = (frame + 1) % images.Length;

			mat.sprite = images [frame];

		}

		if (state != ChispaWorldState.idle) {

			timeToEmit += Time.deltaTime;
			if (timeToEmit > (1.0f / dustFrequency)) {

				timeToEmit = 0.0f;
				float newAngle = FloatRandom.floatRandomRange (0, 6.28f);
				GameObject newDustGO = (GameObject)Instantiate (dustPrefab, this.transform.position,
					                      Quaternion.Euler (0, 0, newAngle));
			


			}

		}



		if (state == ChispaWorldState.comingOut) {

			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("ChispaRemainsOut")) {
				notifyFinishAction();
				state = ChispaWorldState.Out;
				transitioning = false;
			}

			/*AnimatorStateInfo inf = anim.GetCurrentAnimatorStateInfo (0);
			float nt = inf.normalizedTime;


			if ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
				&& !anim.IsInTransition(0)) {

				notifyFinishAction();
				state = ChispaWorldState.Out;
			}*/

		}



		if (state == ChispaWorldState.returning) {

			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {

				mat.color = new Color (0, 0, 0, 0);
				notifyFinishAction ();
				state = ChispaWorldState.idle;
				transitioning = false;
			}

		}


	
	}

	public void appear() {

		mat.color = new Color (1, 1, 1, 1);
		state = ChispaWorldState.comingOut;
		anim.SetBool ("isOut", true);
		transitioning = true;

	}

	public void disappear() {

		state = ChispaWorldState.returning;
		anim.SetBool ("isOut", false);
		transitioning = true;

	}

	public void _wa_appear(WisdominiObject waiter) {

		waitingRef = waiter;
		waiter.isWaitingForActionToComplete = true;
		appear ();

	}

	public void _wa_disappear(WisdominiObject waiter) {

		waitingRef = waiter;
		waiter.isWaitingForActionToComplete = true;
		disappear ();

	}

}
