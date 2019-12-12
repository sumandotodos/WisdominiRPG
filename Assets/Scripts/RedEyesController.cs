using UnityEngine;
using System.Collections;

enum RedEyesControllerState { canMakeText, cannotMakeText };

public class RedEyesController : MonoBehaviour {

	/* properties */

	bool canMakeTextFlag;
	float coolDownElapsedTime;

	/* constants */

	const float coolDownTime = 4.0f;

	void Start () 
	{	
		canMakeTextFlag = true;
		coolDownElapsedTime = 0.0f;
	}
	
	void Update () 
	{
		if (!canMakeTextFlag) 
		{
			coolDownElapsedTime += Time.deltaTime;
			if (coolDownElapsedTime > coolDownTime) 
			{
				canMakeTextFlag = true;
				coolDownElapsedTime = 0.0f;
			}
		}	
	}

	public bool canMakeText() 
	{
		return canMakeTextFlag;
	}

	public void textMade() 
	{
		canMakeTextFlag = false;
	}
}
