using UnityEngine;
using System.Collections;

public class HeroEffectWizard : HeroEffect {

	public Texture[] image;
	public BigHero bigHero;

	public GameObject glow;

	Material mat;

	float opacity, targetOpacity;
	public float opacitySpeed = 6.0f;

	// Use this for initialization
	void Start () {

		mat = glow.GetComponent<Renderer> ().material;
		opacity = targetOpacity = 0.0f;
		mat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));

	}

	// Update is called once per frame
	void Update () {

		bool changed = Utils.updateSoftVariable (ref opacity, targetOpacity, opacitySpeed);

		if (changed) {
			mat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
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

