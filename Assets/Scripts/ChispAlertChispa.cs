using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ChispaState { leftToRight, rightToLeft, idle };

public class ChispAlertChispa : MonoBehaviour {
	
	/* references */

	public UIChispAlert controller;
	public GameObject dustPrefab;
	//public Image theImage;

	/* properties */

	float elapsedTime;
	float angle;
	public ChispaState state; 
	float maxX, maxY, minX;
	bool showText;

	float xSpeed;
	float xAccel;

	/* public properties */
	public float yPos, xPos;	

	/* constants */

	const float timeToEmitDust = 0.09f;
	const float swipeSpeed = 180.0f;
	const float swayAmplitude = 5.0f;
	const float angleSpeed = 20.0f;
	const float swipeAccel = 200.0f;

	void Start () 
	{	
		initialize ();
	}

	void initialize() 
	{
		elapsedTime = 0.0f;
		state = ChispaState.idle;
		minX = xPos = -Screen.width / 3.0f;
		maxX = Screen.width + Screen.width / 3.0f;
		maxY = Screen.height;
		xSpeed = swipeSpeed;
		this.transform.position = new Vector2 (xPos, yPos);
		showText = false;
	}
	
	void Update () 
	{
		float scaleFactor = Screen.height / 600.0f;

		if (showText) {
			controller.theText.color = new Color (1, 1, 1, 1);
			showText = false;
		}

		if (state == ChispaState.leftToRight || state == ChispaState.rightToLeft) 
		{
			xPos += xSpeed * scaleFactor * Time.deltaTime;
			xSpeed += xAccel * scaleFactor * Time.deltaTime;

			// we are in absolute coordinates: 0 is left, Screen.width is right	
			yPos = (maxY / 2.0f) + swayAmplitude * Mathf.Sin (angle);

			angle += angleSpeed * Time.deltaTime;

			this.transform.position = new Vector2 (xPos, yPos);

			if (xPos > maxX || xPos < minX) {
				state = ChispaState.idle;
				xSpeed = swipeSpeed;
			}

			elapsedTime += Time.deltaTime;
			if (elapsedTime > timeToEmitDust) 
			{
				elapsedTime = 0.0f;
				GameObject newDustGO = (GameObject) Instantiate (dustPrefab, this.transform.position, 
				Quaternion.Euler (0, 0, FloatRandom.floatRandomRange(0, 180.0f)));
				newDustGO.transform.parent = this.transform.parent;
				ChispaAlertDustParticle newDust = newDustGO.GetComponent<ChispaAlertDustParticle> ();
				newDust.initialize ();
			}
		}

		if (state == ChispaState.leftToRight) 
		{
			if (Input.GetMouseButtonDown (0)) { // finish at once

				xSpeed = xSpeed * scaleFactor * 10.0f;
			}
		}

		if (state == ChispaState.rightToLeft) 
		{
			if (Input.GetMouseButtonDown (0)) { // finish at once

				xSpeed = xSpeed * scaleFactor * 10.0f;
			}
		}

		if (state == ChispaState.idle) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				controller.dismiss ();
			}
		}	
	}

	public void swipeLeftToRight() 
	{
		xSpeed = swipeSpeed;
		xPos = -Screen.width/20.0f;
		xAccel = swipeAccel;
	
		this.transform.localScale = new Vector3 (1, 1, 1);

		state = ChispaState.leftToRight;

		showText = true;
	}

	public void swipeRightToLeft() 
	{
		xSpeed = -swipeSpeed;
		xPos = Screen.width + Screen.width/20.0f;
		xAccel = -swipeAccel;

		this.transform.localScale = new Vector3 (-1, 1, 1);

		state = ChispaState.rightToLeft;

		showText = true;
	}
}
