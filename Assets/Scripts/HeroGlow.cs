using UnityEngine;
using System.Collections;

enum HeroGlowState { hidden, growing, shrinking };

public class HeroGlow : MonoBehaviour {

	HeroGlowState state;
	Material mat;
	float opacity;
	public float speed = 1.0f;
	public float scale = 20.0f;

	// Use this for initialization
	void Start () {

		state = HeroGlowState.hidden;
		mat = this.GetComponent<Renderer> ().material;
		mat.SetColor ("_Tint", new Color (0, 0, 0, 0));
		opacity = 0.0f;
	
	}

	public void glow() {

		state = HeroGlowState.growing;

	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == HeroGlowState.growing) {

			opacity += speed * Time.deltaTime;
			if (opacity > 1.0f) {
				opacity = 1.0f;
				state = HeroGlowState.shrinking;
			}

		} else if (state == HeroGlowState.shrinking) {

			opacity -= speed * Time.deltaTime;
			if (opacity < 0.0f) {
				opacity = 0.0f;
				state = HeroGlowState.hidden;
			}

		}

		this.transform.localScale = new Vector3 (opacity*scale, opacity*scale, opacity*scale);
		mat.SetColor ("_Tint", new Color (0, 0, 0, opacity));

	}
}
