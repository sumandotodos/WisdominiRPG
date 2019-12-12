using UnityEngine;
using System.Collections;

public class HeroEffectSage : HeroEffect {

	public GameObject pajaro;
	public Material pajaroMat;

	public Texture[] image;
	public BigHero bigHero;

	public float pajaroDelay;
	float elapsedTime;

	float opacity;
	float targetOpacity;
	public float opacitySpeed = 1.0f;

	bool fadingOut;

	Animator anim;

	// Use this for initialization
	void Start () {

		opacity = targetOpacity = 0.0f;
		pajaroMat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
		anim = pajaro.GetComponent<Animator> ();
		fadingOut = false;

	}

	// Update is called once per frame
	void Update () {

		bool changed = Utils.updateSoftVariable (ref opacity, targetOpacity, opacitySpeed);

		if (changed) {
			pajaroMat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
		} else {
			if (fadingOut == true) {
				anim.SetBool ("isFlying", false);
				fadingOut = false;
			}
		}


		if (elapsedTime < pajaroDelay) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= pajaroDelay) {
				anim.SetBool ("isFlying", true);
			}
		}

	}

	public override void reset() {

		fadingOut = true;
		targetOpacity = 0.0f;

	}

	public override void effect() {

		bigHero.GetComponent<Renderer> ().material.mainTexture = image [0];
		elapsedTime = 0.0f;
		targetOpacity = 1.0f;
		fadingOut = false;

	}
}

