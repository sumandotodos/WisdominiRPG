using UnityEngine;
using System.Collections;

public class HeroEffectMaster : HeroEffect {

	public GameObject dollar;
	public GameObject heart1;
	public GameObject heart2;

	public Texture[] image;
	public BigHero bigHero;

	float opacity;
	float targetOpacity;
	public float opacitySpeed = 6.0f;

	Material h1Mat, h2Mat, dMat;

	// Use this for initialization
	void Start () {

		opacity = targetOpacity = 0.0f;
		dMat = dollar.GetComponent<Renderer> ().material;
		h1Mat = heart1.GetComponent<Renderer> ().material;
		h2Mat = heart2.GetComponent<Renderer> ().material;
		dMat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
		h1Mat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
		h2Mat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));

	}

	// Update is called once per frame
	void Update () {

		bool change = Utils.updateSoftVariable (ref opacity, targetOpacity, opacitySpeed);

		if (change) {
			dMat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
			h1Mat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
			h2Mat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
		}

	}

	public override void reset() {

		targetOpacity = 0.0f;

	}

	public override void effect() {

		bigHero.GetComponent<Renderer> ().material.mainTexture = image [0];
		targetOpacity = 1.0f;

	}
}

