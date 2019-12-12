using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ANGLEEEE { up, down, left, right };

public class UIDirectionIndicator : WisdominiObject {

	/* references */

	public GameObject sunTarget = null;
	//public GameObject [] waterDroplets;
	//public int actualNumberOfDroplets;
	public PlayerScript player;
	public float angleSpeed = 300.0f;
	public float plainSightRadius = 10.0f;
	public DropletController dropletController;

	float overallOpacity = 1.0f;
	float targetOverallOpacity = 1.0f;
	const float overallOpacitySpeed = 1.0f;

	RectTransform rect;

	public ANGLEEEE angulito;

	int delayFrames = 2;

	/* properties */

	public float angle;
	public float targetAngle;
	float ta;
	float opacity;

	float upAngle, leftAngle, downAngle, rightAngle;
	public float upDegrees, leftDegrees, downDegrees, rightDegrees;

	public float angleDegrees;

	float screenW, screenH;

	new void Start () 
	{
		angle = 0.0f;
		targetAngle = 0.0f;
		ta = 0.0f;
		opacity = 1.0f;
		rect = this.GetComponent<RectTransform> ();
		screenW = Screen.width;
		screenH = Screen.height;

		// tan(angle) == screenW/screenH
		//angle = arctan (screenW/screenH

		upAngle = Mathf.Atan ((screenW - rect.rect.width)/ (screenH - rect.rect.height));
		leftAngle = upAngle + 2.0f * Mathf.Atan ((screenH - rect.rect.height) / (screenW - rect.rect.width));
		downAngle = leftAngle + 2.0f * upAngle;
		rightAngle = downAngle + 2.0f * Mathf.Atan ((screenH - rect.rect.height) / (screenW - rect.rect.width));

		upDegrees = Mathf.Rad2Deg * upAngle;
		leftDegrees = Mathf.Rad2Deg * leftAngle;
		downDegrees = Mathf.Rad2Deg * downAngle;
		rightDegrees = Mathf.Rad2Deg * rightAngle;

		overallOpacity = 0.0f;
		fadeIn ();	
	}
	
	new void Update ()
	{
		/* get location of closest dropAWater */
		//if (waterDroplets.Length > 0) {

		if (delayFrames > 0) 
		{
			--delayFrames;
			return;
		}

			int minIndex;
			float minDist;

			/*int minIndex = 0;
			float minDist = (waterDroplets [0].transform.position - player.transform.position).magnitude;

			for (int i = 1; i < waterDroplets.Length; ++i) {

				float dist = (waterDroplets [i].transform.position - player.transform.position).magnitude;
				if (dist < minDist) {
					minDist = dist;
					minIndex = i;
				}

			}*/
		Vector3 pos = player.transform.position;

			//dropletController.indexAndDistanceToClosestDroplet(pos, out minIndex, out minDist);
		minIndex = -1;
		if (sunTarget == null) 
		{
			minIndex = dropletController.indexOfClosestDroplet (pos);
			minDist = dropletController.distanceToClosestDroplet (pos);


			if (minIndex == -1) 
			{
				this.GetComponent<RawImage> ().color = new Color (1, 1, 1, 0);
				return;
			}
		} 
		else 
		{
			minDist = (sunTarget.transform.position - pos).magnitude;
		}

		opacity = 1.0f;

		if (minDist < plainSightRadius)
			opacity = 0.0f;
		
		if ((minDist < plainSightRadius * 1.3f) && (minDist >= plainSightRadius)) 
		{
			opacity = Mathf.Lerp (0.0f, 0.5f, ((minDist - plainSightRadius) / (plainSightRadius * 1.3f - plainSightRadius)));
		}

		if (minDist >= plainSightRadius * 1.3f) 
		{
			opacity = 1.0f / ((minDist - plainSightRadius*1.3f)/20.0f);

				if (opacity < 0.3f)
					opacity = 0.3f;

				if (opacity > 0.5f)
					opacity = 0.5f;
		}

		this.GetComponent<RawImage> ().color = new Color (1, 1, 1, opacity*overallOpacity);

			/* minIndex holds now the index to the closest dropAWater GO */

		Vector2 playerPos = new Vector2 (player.transform.position.x, player.transform.position.z);
		Vector2 waterPos;

		if (sunTarget == null) 
		{
			waterPos = dropletController.droplet2DPosition (minIndex);
		} 
		else 
		{
			waterPos = new Vector2 (sunTarget.transform.position.x, sunTarget.transform.position.z);
		}

		Vector2 diffVec = waterPos - playerPos;
		diffVec.Normalize ();

		targetAngle = Mathf.Acos (diffVec.y);
		if (diffVec.x > 0.0)
			targetAngle = Mathf.Deg2Rad * 360.0f - targetAngle;
			/* degrees, plees */
			targetAngle = Mathf.Rad2Deg * targetAngle;
			angleDegrees = targetAngle;
			//float scale = 0.75f + 0.25f * Mathf.Sin(Mathf.Deg2Rad * (2.0f* angle - 90));

			rect.rotation = Quaternion.Euler (0, 0, angle);
			//rect.localScale = new Vector3 (1, scale, 1);

			float xCoord, yCoord;

			float rScreenW = (screenW - rect.rect.width) / 2.0f;
			float rScreenH = (screenH - rect.rect.height) / 2.0f;

			float radAngle = angle;
			while (angle > 360.0f)
				angle -= 360.0f;
			while (angle < 0.0f)
				angle += 360.0f;
			radAngle = Mathf.Deg2Rad * radAngle;

		if (radAngle < upAngle) 
		{
			yCoord = rScreenH;
			xCoord = -rScreenH * Mathf.Tan (Mathf.Deg2Rad * angle);
			angulito = ANGLEEEE.up;

		} else if (radAngle < leftAngle) {

			xCoord = -rScreenW;
			yCoord = -rScreenW * Mathf.Tan (Mathf.Deg2Rad * angle - Mathf.PI / 2.0f);

			angulito = ANGLEEEE.left;

		} else if (radAngle < downAngle) {

			yCoord = -rScreenH;
			xCoord = rScreenH * Mathf.Tan (Mathf.Deg2Rad * angle - Mathf.PI);
			angulito = ANGLEEEE.down;

		} else if (radAngle < rightAngle) {

			xCoord = rScreenW;
			yCoord = xCoord * Mathf.Tan (Mathf.Deg2Rad * angle - 3.0f * Mathf.PI / 2.0f);
			angulito = ANGLEEEE.right;

		} else {

			yCoord = rScreenH;
			xCoord = -yCoord * Mathf.Tan (Mathf.Deg2Rad * angle);
			angulito = ANGLEEEE.up;
		}

		rect.position = new Vector2 (screenW/2 + xCoord, screenH/2 + yCoord); ///2 + xCoord, screenH/2 + yCoord);


		ta = targetAngle;
		//}

		float diff1 = Mathf.Abs (angle - targetAngle);
		float diff2 = 360.0f - diff1;
//		if (angle > targetAngle)
//			diff2 = Mathf.Abs (targetAngle - (angle - 360.0f));
//		else
//			diff2 = Mathf.Abs (angle - (targetAngle - 360.0f));

		if (diff2 < diff1) 
		{ // zero cross

			if (angle > ta) 
			{
				angle += angleSpeed * Time.deltaTime;
				if (angle > 360.0f) 
				{
					angle -= 360.0f;
					if (angle > ta) 
					{
						angle = ta;
					}
				}				
			}

			else if (angle < ta) 
			{
				angle -= angleSpeed * Time.deltaTime;
				if (angle < 0.0f) 
				{
					angle += 360.0f;
					if (angle < ta) 
					{
						angle = ta;
					}
				}
			}			
		} 
		else { // normal path
			if (angle > ta) 
			{
				angle -= angleSpeed * Time.deltaTime;
				if (angle < ta) 
				{					
					angle = ta;
				}
			}

			else if (angle < ta) 
			{
				angle += angleSpeed * Time.deltaTime;
				if (angle > ta) 
				{
					angle = ta;
				}
			}
		}

		bool change = Utils.updateSoftVariable (ref overallOpacity, targetOverallOpacity, overallOpacitySpeed);	
	}

	public void fadeIn() 
	{
		targetOverallOpacity = 1.0f;
	}

	public void fadeOut() 
	{
		targetOverallOpacity = 0.0f;
	}

	public void _wm_fadeIn() 
	{
		fadeIn ();
	}

	public void _wm_fadeOut() 
	{
		fadeOut ();
	}		
}


