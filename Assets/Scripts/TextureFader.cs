using UnityEngine;
using System.Collections;

public class TextureFader : WisdominiObject {

	/* references */

	/* public properties */

	public float maxOpacity = 1.0f;
	public float minOpacity = 0.0f;
	public float opacitySpeed = 0.3f;
	public string ShaderParameter;
	public Color tint;

	/* properties */

	Material mat;
	float opacity;
	float targetOpacity;

	// Use this for initialization
	void Start () {
	
		mat = this.GetComponent<Renderer> ().material;
		opacity = targetOpacity = 0.0f;
		updateMat ();

	}

	void updateMat() {

		tint.a = opacity;
		if (ShaderParameter.Equals ("")) {
			mat.color = tint;
		} else {
			mat.SetColor (ShaderParameter, tint);
		}

	}

	// Update is called once per frame
	void Update () {
	
		if (opacity < targetOpacity) {
			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > targetOpacity) {
				opacity = targetOpacity;
			}
			updateMat ();
		}

		if (opacity > targetOpacity) {
			opacity -= opacity * Time.deltaTime;
			if (opacity < targetOpacity) {
				opacity = targetOpacity;
			}
			updateMat ();
		}

	}

	public void fadeIn() {

		targetOpacity = maxOpacity;

	}

	public void fadeOut() {

		targetOpacity = minOpacity;

	}

	public void _wm_fadeIn() {

		fadeIn ();

	}

	public void _wm_fadeOut() {

		fadeOut ();

	}
}
