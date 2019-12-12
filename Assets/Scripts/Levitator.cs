using UnityEngine;
using System.Collections;

public enum LevitationState { floating, onGround };

public class Levitator : WisdominiObject {

	public float yOffset;

	float phase;
	public float phaseSpeed;

	public float smallScale = 1.0f;
	public float bigScale = 2.0f;

	float y;

	int state;

	public LevitationState levState;

	// Use this for initialization
	void Start () {
	
		phase = -Mathf.PI / 2.0f;
		state = -1;

	}

	public void _wm_takeOff() {
		takeOff ();
	}

	public void takeOff() {

		state = 0;
		levState = LevitationState.floating;

	}

	public void _wm_takeOn() {
		takeOn ();
	}

	public void takeOn() {

		if (phase > 1.0f * Mathf.PI)
			phase -= 2.0f * Mathf.PI;
		state = 2;
		levState = LevitationState.onGround;

	}
	
	// Update is called once per frame
	void Update () {

		if(state == -1) {
			
			if (levState == LevitationState.floating)
				takeOff ();

		}


			

	
		if (state == 0) {

			if (phase > 0.0f)
				++state;

			phase += phaseSpeed * Time.deltaTime / bigScale;

			this.transform.localPosition = new Vector3 
            (this.transform.localPosition.x, yOffset + bigScale * Mathf.Sin (phase), this.transform.localPosition.z);

		}

		if (state == 1 || state == 2) {

			this.transform.localPosition = new Vector3 
            (this.transform.localPosition.x, yOffset + smallScale * Mathf.Sin (phase),
                this.transform.localPosition.z);
			if (levState == LevitationState.onGround)
				takeOn ();

			phase += phaseSpeed * Time.deltaTime / smallScale;

		}

		if (state == 1) {

			if (phase > 2.0f * Mathf.PI)
				phase -= 2.0f * Mathf.PI;


		}

		if (state == 2) {

			if (phase > 1.0f * Mathf.PI) {
				
				++state; 
			}

		}

		if (state == 3) {

			this.transform.localPosition = new Vector3 
            (this.transform.localPosition.x, yOffset + bigScale * Mathf.Sin (phase),
                this.transform.localPosition.z);
			if (phase > 1.5f * Mathf.PI) {
				phase = 1.5f * Mathf.PI;
				this.transform.localPosition = new Vector3 
                (this.transform.localPosition.x, yOffset + bigScale * Mathf.Sin (phase),
                    this.transform.localPosition.z);
				state = -1;
				phase = -Mathf.PI / 2.0f;
			}

			phase += phaseSpeed * Time.deltaTime / bigScale;

		}



	}
}
