using UnityEngine;
using System.Collections;

public class HeroEffectWarrior : HeroEffect {

	public ParticleSystem particles;

	public Texture[] image;
	public BigHero bigHero;

	float opacity;
	float targetOpacity;
	public float opacitySpeed = 1.0f;

	Material mat;

	// Use this for initialization
	void Start () {

		opacity = targetOpacity = 0.0f;
		mat = particles.GetComponent<Renderer> ().material;
		mat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
		particles.Stop ();
	
	}
	
	// Update is called once per frame
	void Update () {

		bool changed = Utils.updateSoftVariable (ref opacity, targetOpacity, opacitySpeed);

		if(changed) {
			mat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
		}
	
	}

	public override void reset() {

		particles.Stop ();
		targetOpacity = 0.0f;

	}

	public override void effect() {

		bigHero.GetComponent<Renderer> ().material.mainTexture = image [0];
		particles.Play ();
		targetOpacity = 1.0f;

	}
}
