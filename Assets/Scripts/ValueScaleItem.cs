using UnityEngine;
using System.Collections;

public enum ValueScaleItemStatus { idle, picked };

public class ValueScaleItem : MonoBehaviour {

	/* constants */

	const float floorHeight = 2.5f;


	/* references */
	public ValueScaleController controller;



	/* properties */
	float x, y, z;
	float targetX, targetY, targetZ;
	float speed;
	int floor;

	bool isPicked;

	TextMesh theText;
	string originalText;

	public string getText() {

		return originalText;
	
	}

	public void setText(string t) {

		theText.text = t;

	}

	// Use this for initialization
	public void initialize () {

		theText = this.GetComponent<TextMesh> ();
		originalText = theText.text;
		targetY = y = this.transform.position.y;
		speed = 4.0f;
	
	}

	int returnFloor() {

		float y = this.transform.position.y;
		return (int)(y / floorHeight);

	}

	// Update is called once per frame
	void Update () {
	
		/* update soft variables */
		if (x < targetX) {

			x += speed * Time.deltaTime;
			if (x > targetX) {

				x = targetX;

			}

		}
		if (x > targetX) {

			x -= speed * Time.deltaTime;
			if (x < targetX) {

				x = targetX;

			}

		}
		if (z < targetZ) {

			z += speed * Time.deltaTime;
			if (z > targetZ) {

				z = targetZ;

			}

		}
		if (z > targetZ) {

			z -= speed * Time.deltaTime;
			if (z < targetZ) {

				z = targetZ;

			}

		}
		if (y < targetY) {

			y += speed * Time.deltaTime;
			if (y > targetY) {

				y = targetY;

			}

		}
		if (y > targetY) {

			y -= speed * Time.deltaTime;
			if (y < targetY) {

				y = targetY;

			}

		}


		Vector3 newPos = new Vector3 (this.transform.position.x, y, z);
		this.transform.position = newPos;
	
	}

	public void setTargetY(float ty) {

		targetY = ty;

	}

	public void setY(float ny) {

		y = ny; targetY = ny;
		Vector3 newPos = new Vector3 (this.transform.position.x, y, z);
		this.transform.position = newPos;

	}

	public void pick() {

		targetZ = -1.0f;
		isPicked = true;

	}

	public void release() {

		targetZ = 0.0f;
		isPicked = false;

	}

	public void foo() {

		theText.text = "En pelotas!";

	}


}
