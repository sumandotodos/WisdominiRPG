using UnityEngine;
using System.Collections;

public class HeroEffectPhilosopher : HeroEffect {

	float opacity;
	float targetOpacity;
	public float opacitySpeed = 6.0f;

	public Texture[] image;
	public BigHero bigHero;

	public Material[] butterflyMat;

	// Use this for initialization
	void Start () {

		foreach (Material m in butterflyMat) {
			m.SetColor("_TintColor", new Color(0, 0, 0, 0));
		}
		opacity = 0.0f;

	}

	// Update is called once per frame
	void Update () {

		bool changed = Utils.updateSoftVariable (ref opacity, targetOpacity, opacitySpeed);
		if (changed) {
			foreach (Material m in butterflyMat) {
				m.SetColor("_TintColor", new Color(1, 1, 1, opacity));
			}
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
