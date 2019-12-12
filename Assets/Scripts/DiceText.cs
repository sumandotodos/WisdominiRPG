using UnityEngine;
using System.Collections;

public class DiceText : MonoBehaviour {

	int state;

	float opacity;
	//float delay;
	float elapsedTime;
	float y;
	string text;

	Material mat;

	Vector3 initialPosition;

	const float opacitySpeed = 10.0f;
	const float ySpeed = 1.5f;
	const float delay = 0.8f;

	// Use this for initialization
	void Start () {

		initialPosition = this.transform.position;
		mat = this.GetComponent<Renderer> ().material;
		opacity = 0.0f;
		y = 0.0f;
		state = 0;
		mat.color = new Color (1, 1, 1, opacity);
		elapsedTime = 0.0f;



	}
	
	// Update is called once per frame
	void Update () {

		if (state == 0) {

		}

		if (state == 1) {

			elapsedTime += Time.deltaTime;
			y += ySpeed * Time.deltaTime;

			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > 1.0f) {
				opacity = 1.0f;

			}
			mat.color = new Color (1, 1, 1, opacity);

			this.transform.position = new Vector3 (0, y, 0) + initialPosition;

			if (elapsedTime > delay) {
				++state;
			}

		}

		if (state == 2) {

			y += ySpeed * Time.deltaTime;
			this.transform.position = new Vector3 (0, y, 0) + initialPosition;
			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < 0.0f) {
				opacity = 0.0f;
				++state;
			}
			mat.color = new Color (1, 1, 1, opacity);

		}
	
	}

	public void go() {

		state = 1;

	}

	public void reset() {

		elapsedTime = 0.0f;
		state = 0;
		this.transform.position = initialPosition;
		mat.color = new Color (1, 1, 1, 0);
		opacity = 0.0f;
		y = 0.0f;
		state = 0;

	}
}
