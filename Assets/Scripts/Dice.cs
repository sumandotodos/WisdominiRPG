using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {

	/* references */

	Animator anim;

	/* properties */

	float elapsedTime;
	float delay;
	int state; 
	int value;

	// Use this for initialization
	void Start () {
	
		float randomYAngle = FloatRandom.floatRandomRange (0, 360.0f);
		this.transform.parent.rotation = Quaternion.Euler (0, randomYAngle, 0);
		delay = FloatRandom.floatRandomRange (0.2f, 0.8f);
		anim = this.GetComponent<Animator> ();
		state = 0;
		elapsedTime = 0.0f;
		//roll ();

	}
	
	// Update is called once per frame
	void Update () {

		if (state == 0) {

		}

		if (state == 1) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > delay)
				++state;
		}
	
		if (state == 2) {
			anim.SetInteger ("rolling", translate(value));
			state = 0;
		}



	}

	public void reset() {
		state = 0;
		elapsedTime = 0.0f;
	}

	public void resetAnimation() {
		anim.SetTrigger ("reset");
		anim.SetInteger ("rolling", 0);
		anim.Play ("DiceIdle");
	}

	private int translate(int v) {
		switch (v) {
		case 1:
			return 1;
		case 2:
			return 4;
		case 3:
			return 6;
		case 4:
			return 3;
		case 5:
			return 2;
		case 6:
			return 5;
		}
		return v;
	}

	public int roll() {

		int v = Random.Range (1, 6);

		value = v;

		state = 1;

		return v;

	}

	public int roll(int whatResult) {
		Debug.Log ("<color=orange>WhatResult : " + whatResult + "</color>");
		value = whatResult;
		state = 1;
		return whatResult;
	}
}
