using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum warriorIconState { folded, unfolded, folding, unfolding };

public class WarriorIcon : MonoBehaviour {

	/* public properties */

	public int Index;
	public string CategoryName;
	public int numberOfElements = 7;
	MasterControllerScript mc;
	DataStorage ds;


	/* properties */

	float deployedScale;
	float radius;
	float angle;
	const float minRadius = 0.0f;
	const float maxRadius = 190.0f;
	const float radiusSpeed = 280.0f;
	float angleSpeed = 3.0f;
	float slowAngleSpeed = 0.05f;
	float opacity;
	float targetOpacity;
	[HideInInspector]
	public float globalOpacity = 0.4f;
	[HideInInspector]
	public warriorIconState status;


	/* constants */
	public string[] HeroType = {
		"Warrior",
		"Master",
		"Philosopher",
		"Sage",
		"Explorer",
		"Wizard",
		"Yogi"
	};

	string levelName;

	void Start () 
	{
		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = mc.getStorage ();

		levelName = ds.retrieveStringValue ("CurrentLevel");

		setLevel (0, -1);

		deployedScale = this.transform.localScale.x;
		this.transform.localPosition = new Vector3 (0, 0, 0);
		radius = minRadius;
		status = warriorIconState.unfolding;
		angle = Index * 2.0f * 3.1416f / ((float)numberOfElements);
		opacity = targetOpacity = 1.0f;
		this.gameObject.GetComponent<Image>().color = new Vector4 (1, 1, 1, opacity*globalOpacity);
	}

	public void setNumberOfElements(int ne) 
	{
		numberOfElements = ne;
		angle = Index * 2.0f * 3.1416f / ((float)numberOfElements);
	}
	
	void Update () 
	{
		if (opacity < targetOpacity) 
		{
			opacity += Time.deltaTime;
			if (opacity > targetOpacity)
				opacity = targetOpacity;
			this.gameObject.GetComponent<Image>().color = new Vector4 (1, 1, 1, opacity*globalOpacity);
		}

		if (opacity > targetOpacity)
		{
			opacity -= Time.deltaTime;
			if (opacity < targetOpacity)
				opacity = targetOpacity;
			this.gameObject.GetComponent<Image>().color = new Vector4 (1, 1, 1, opacity*globalOpacity);
		}

		float scale = (radius - minRadius) / (maxRadius - minRadius);
		this.transform.localScale = new Vector3 (deployedScale * scale, deployedScale * scale, deployedScale * scale);


		if (status == warriorIconState.unfolding) 
		{
			radius += radiusSpeed * Time.deltaTime;
			angle += angleSpeed * Time.deltaTime;
			if (radius > maxRadius) 
			{
				radius = maxRadius;
				status = warriorIconState.unfolded;
			}

			float x, y;
			x = radius * Mathf.Cos (angle);
			y = radius * Mathf.Sin (angle);
			Vector3 newPos = new Vector3 (x, y, 0);
			this.transform.localPosition = newPos;
		} 

		else if (status == warriorIconState.folding) 
		{
			radius -= radiusSpeed * Time.deltaTime;
			angle += angleSpeed * Time.deltaTime;
			if (radius < minRadius) {

				radius = minRadius;
				status = warriorIconState.folded;
			}

			float x, y;
			x = radius * Mathf.Cos (angle);
			y = radius * Mathf.Sin (angle);
			Vector3 newPos = new Vector3 (x, y, 0);
			this.transform.localPosition = newPos;
		} 

		else 
		{
			angle += slowAngleSpeed * Time.deltaTime;
			float x, y;
			x = radius * Mathf.Cos (angle);
			y = radius * Mathf.Sin (angle);
			Vector3 newPos = new Vector3 (x, y, 0);
			this.transform.localPosition = newPos;
		}	
	}


	/*
	 * 
	 * Set globalOpacity
	 * 
	 * Level = 0   :   every icon is a hero category, check "CurrentLevelHasHero" + TypeOfHero
	 * 
	 * Level = 1   :   every icon is a individual hero, check "HasHero" + TypeOfHero + Index
	 * 
	 */
	public void setLevel(int l, int heroClass) 
	{
		if (l == 0) {
			globalOpacity = 0.4f;
			//if(ds.retrieveBoolValue("CurrentLevelHasHero" + HeroType[Index])) globalOpacity = 1.0f;
			if(ds.retrieveBoolValue("Has" + levelName + HeroType[Index])) globalOpacity = 1.0f;
			this.gameObject.GetComponent<Image>().color = new Vector4 (1, 1, 1, opacity*globalOpacity);
		} else if (l == 1) {
			globalOpacity = 0.4f;
			string test = "Has" + levelName + HeroType [heroClass] + (Index);
			if(ds.retrieveBoolValue("Has" + levelName + HeroType[heroClass] + (Index))) globalOpacity = 1.0f;
			this.gameObject.GetComponent<Image>().color = new Vector4 (1, 1, 1, opacity*globalOpacity);
		}
	}

	public void fadeout() 
	{
		targetOpacity = 0.0f;
	}

	public void fadein() 
	{
		targetOpacity = 1.0f;
	}
}
