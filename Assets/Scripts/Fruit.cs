using UnityEngine;
using System.Collections;

enum FruitState  { invisible, idle, growing, appearing };

public class Fruit : WisdominiObject {

	/* public properties */

	[HideInInspector]
	public float timeToCompletion;

	/* properties */

	float fruitScale;
	float clampedScale;
	Material mat;
	Color immatureColor = Color.green;
	Color ripeColor = Color.red;
	public float elapsedTime;
	FruitState state;


	/* constants */

	const float minFruitScale = 0.3f;
	const float maxFruitScale = 1.0f;

	void Awake() {
		mat = this.GetComponentInChildren<Renderer> ().material;
	}

	// Use this for initialization
	new void Start () {
	

		reset ();
	
	}

	new public void reset() {

		clampedScale = 0.0f;
		fruitScale = Mathf.Lerp(minFruitScale, maxFruitScale, clampedScale);
		this.transform.localScale = new Vector3 (fruitScale, fruitScale, fruitScale);
		Color curColor = Color.Lerp (immatureColor, ripeColor, clampedScale);
		mat.color = curColor;
		elapsedTime = 0.0f;
		state = FruitState.idle;

	}
	
	// Update is called once per frame
	new void Update () {
	
		switch (state) {

			case FruitState.invisible:
				break;

			case FruitState.idle:
				break;

			case FruitState.growing:
				elapsedTime += Time.deltaTime;
				float tParam = elapsedTime / timeToCompletion;
				fruitScale = Mathf.Lerp (minFruitScale, maxFruitScale, tParam);
				this.transform.localScale = new Vector3 (fruitScale, fruitScale, fruitScale);
				Color curColor = Color.Lerp (immatureColor, ripeColor, tParam);
				mat.color = curColor;
				if (tParam > 1) {
					state = FruitState.idle;
					notifyFinishAction ();
				}
				break;

			case FruitState.appearing:
				elapsedTime += Time.deltaTime;
				tParam = elapsedTime / timeToCompletion;
				if (tParam > 1) {
					tParam = 1;
					state = FruitState.idle;
					notifyFinishAction();
				}
				fruitScale = Mathf.Lerp (0.0f, minFruitScale, tParam);
				this.transform.localScale = new Vector3 (fruitScale, fruitScale, fruitScale);
				
				break;

		}



	}

	public void _wm_appear() {

		timeToCompletion = 3.0f;
		state = FruitState.appearing;
		mat.color = immatureColor;

	}

	public void hide() {

		this.transform.localScale = Vector3.zero;

	}

	public void _wa_grow(WisdominiObject waiter, float totalTime) {

		this.waitingRef = waiter;
		waiter.isWaitingForActionToComplete = true;
		timeToCompletion = totalTime;
		state = FruitState.growing;

	}
}
