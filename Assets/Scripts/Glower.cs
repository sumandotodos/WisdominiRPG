using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Glower : MonoBehaviour {

	/* public properties */

	public float speed = 6.0f;
	public int nOfTimes = 0; // 0 means loop forever


	/* properties */

	int times;
	int state;
	float opacity;

	Image theImage;


	// Use this for initialization
	void Start () {
	
		times = 0;
		state = 0;
		opacity = 0.0f;
		theImage = this.GetComponent<Image> ();
		theImage.color = new Color (1, 1, 1, 0);
		state = 0;

	}
	
	// Update is called once per frame
	void Update () {
	
		/* idle */
		if (state == 0) {

		}


		/* opacity up */
		if (state == 1) {

			opacity += speed * Time.deltaTime;
			if (opacity > 1.0f) {
				opacity = 1.0f;
				++state;
			}
			theImage.color = new Color (1, 1, 1, opacity);

		}


		/* opacity down */
		if (state == 2) {

			opacity -= speed * Time.deltaTime;
			if (opacity < 0.0f) {
				opacity = 0.0f;
				++times;
				if (times == nOfTimes)
					state = 0; 
				else
					state = 1;
			}
			theImage.color = new Color (1, 1, 1, opacity);

		}




	}

	public void glow() {

		state = 1;

	}

	public void reglow()
	{
		state = 2;
	}
}
