using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum FlameState { idle, lightingUp, litUp };

public class Flame : MonoBehaviour {



	/* references */

	Image image;



	/* properties */

	float opacity;
	FlameState state;




	/* constants */

	const float opacitySpeed = 15.0f;



	// Use this for initialization
	void Start () {

		opacity = 0.0f;
		image = this.GetComponent<Image> ();
		state = FlameState.idle;
		image.color = new Color (1, 1, 1, opacity);
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == FlameState.idle) {
			

		}

		if (state == FlameState.lightingUp) {

			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > 1.0f) {
				opacity = 1.0f;
				state = FlameState.litUp;
			}

			image.color = new Color (1, 1, 1, opacity);

		}

		if (state == FlameState.litUp) {
			

		}

	}

	public void lightUp() {

		state = FlameState.lightingUp;

	}
}
