using UnityEngine;
using System.Collections;

public class HeroEffectYogi : HeroEffect {

	public Texture[] image;
	public BigHero bigHero;

	public float animationSpeed; 

	float elapsedTime = 0.0f;

	bool active;

	int frame;

	// Use this for initialization
	void Start () {

		frame = 0;
		elapsedTime = 0.0f;
		active = false;

	}

	// Update is called once per frame
	void Update () {

		if (!active)
			return;

		elapsedTime += Time.deltaTime;
		if(elapsedTime > (1.0f / animationSpeed)) {
			elapsedTime = 0.0f;
			++frame;
			if(frame == image.Length) frame = 0;
			bigHero.GetComponent<Renderer> ().material.mainTexture = image [frame];
		}

	}

	public override void reset() {

		active = false;

	}

	public override void effect() {

		bigHero.GetComponent<Renderer> ().material.mainTexture = image [0];
		active = true;

	}
}

