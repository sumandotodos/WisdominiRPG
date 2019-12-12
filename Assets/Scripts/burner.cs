using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum BurnerState { idle, lightingUp, litUp, appearing };

public class burner : MonoBehaviour {


	/* references */

	Image image;
	public Flame flame;


	/* properties */

	float opacity;
	BurnerState state;
	int substate = 0;
	float elapsedTime = 0.0f;

	/* constants */

	const float minOpacity = 0.3f;
	const float opacitySpeed = 0.6f;


	// Use this for initialization
	void Start () {

		opacity = minOpacity;
		state = BurnerState.appearing;
		image = this.GetComponent<Image> ();
		image.color = new Color (1, 1, 1, opacity);
	
	}
	
	// Update is called once per frame
	void Update () {

		if(state == BurnerState.appearing && substate == 0) {

			elapsedTime += Time.deltaTime;
			if (elapsedTime > 2.0f)
				++substate;

		}
		if(state == BurnerState.appearing && substate == 1) {

			bool change = Utils.updateSoftVariable (ref opacity, minOpacity, opacitySpeed/2.0f);
			if (change) {
				image.color = new Color (1, 1, 1, opacity);
			} else
				state = BurnerState.idle;

		}


		if (state == BurnerState.idle) {


		}

		if (state == BurnerState.lightingUp) {

			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > 1.0f) {
				opacity = 1.0f;
				state = BurnerState.litUp;
			}
			image.color = new Color (1, 1, 1, opacity);

		}

		if (state == BurnerState.litUp) {



		}
	
	}

	public void lightUp() {

		state = BurnerState.lightingUp;

	}

	public void burstInFlames() {

		flame.lightUp ();

	}
}
