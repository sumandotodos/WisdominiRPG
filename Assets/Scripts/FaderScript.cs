using UnityEngine;
using System.Collections;

public class FaderScript : MonoBehaviour {

	float fade; // 0.0 is totally black; 1.0 is totally transparent
	float targetFade;
	public Material matRef;

	public float fadeSpeed;

	bool isFading; 
	bool initialized;

	Vector4 col;

	// Use this for initialization
	void Start () {
	
		if (!initialized)
			Initialize ();

	}

	public void Initialize() {

		setFadeValue(0.0f);

		col = new Vector4 ();

		isFading = false;

		initialized = true;

	}

	void updateMaterial() {

		col = matRef.color;
		col.w = 1.0f-fade;
		matRef.color = col;
	}

	public void setFadeValue(float f) {

		fade = f;
		updateMaterial ();

	}
	
	// Update is called once per frame
	void Update () {
	
		if (!isFading)
			return;


		if (fade > targetFade) {
			fade -= fadeSpeed;
			if (fade < targetFade) {
				fade = targetFade;
				isFading = false;
			}
		}
		else {
			fade += fadeSpeed;
			if (fade > targetFade) {
				fade = targetFade;
				isFading = false;
			}
		}
		

		updateMaterial ();
	}

	public void fadeOut() {

		targetFade = 0.0f;
		isFading = true;

	}

	public void fadeIn() {

		targetFade = 1.0f;
		isFading = true;

	}

}
