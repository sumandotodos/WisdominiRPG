using UnityEngine;
using System.Collections;

public class WordFightHero : MonoBehaviour {

	/* references */

	public MasterControllerScript mcRef;
	DataStorage ds;

	Material mat;

	public string wisdom;

	float targetOpacity;
	float opacity;
	public float opacitySpeed = 0.5f;
	//public float numberOfSecondsToVanish = 0.5f;

	void Start() 
	{
		mat = this.GetComponent<Renderer> ().material;

		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript>();
		ds = mcRef.getStorage ();

		string currentLvl = ds.retrieveStringValue ("CurrentLevel").Substring (0, 6);
//		ds.storeBoolValue (currentLvl + "HasHeroPhilosopher", true);
//		ds.storeBoolValue (currentLvl + "HasHeroMaster", true);
//		ds.storeBoolValue (currentLvl + "HasHeroSage", true);
//		ds.storeBoolValue (currentLvl + "HasHeroYogui", true);

		if (ds.retrieveBoolValue ("Has" + currentLvl + wisdom)) 
		{
			this.GetComponent<Collider> ().enabled = true;
			opacity = targetOpacity = 1.0f;
			opacitySpeed = opacitySpeed * 4;
		} else {
			this.GetComponent<Collider> ().enabled = false;
			opacity = targetOpacity = 0.25f;
		}

		mat.SetColor ("_Tint", new Color (1, 1, 1, opacity));
	}

	void Update()
	{
		if (opacity > targetOpacity) {
			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < targetOpacity)
				opacity = targetOpacity;
		}

		if (opacity < targetOpacity) {
			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > targetOpacity)
				opacity = targetOpacity;
		}
		mat.SetColor ("_Tint", new Color (1, 1, 1, opacity));
	}

	public void setTargetOpacity(float v) 
	{
		targetOpacity = v;
	}

}
