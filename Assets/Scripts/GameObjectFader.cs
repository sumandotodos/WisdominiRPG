using UnityEngine;
using System.Collections;

enum GameObjectFaderState {  transparent, fadingIn, opaque, fadingOut };

public class GameObjectFader : MonoBehaviour {


	/* references */

	Material mat;


	/* properties */

	float opacity;
	GameObjectFaderState state;


	/* public properties */

	public float fadeInSpeed = 6.0f;
	public float fadeOutSpeed = 6.0f;
	public float minOpacity = 0.0f;
	public float maxOpacity = 1.0f;
	public bool isDesktopShader = true;



	// Use this for initialization
	void Start () {
	
		mat = this.GetComponent<Renderer> ().material;
		opacity = minOpacity;
		state = GameObjectFaderState.transparent;
		updateMaterial ();

	}

	public void setOpacity(float op) {

		opacity = op;
		updateMaterial ();

	}



	void updateMaterial() {

		if (isDesktopShader) {
			Color newColor = mat.color;
			newColor.a = opacity;
			mat.color = newColor;
		} else {
			mat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
		}

	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == GameObjectFaderState.transparent) {


		}

		if (state == GameObjectFaderState.fadingIn) {

			opacity += fadeInSpeed * Time.deltaTime;
			if (opacity > maxOpacity) {
				opacity = maxOpacity;
				state = GameObjectFaderState.opaque;
			}
			updateMaterial ();

		}

		if (state == GameObjectFaderState.fadingOut) {

			opacity -= fadeOutSpeed * Time.deltaTime;
			if (opacity < minOpacity) {
				opacity = minOpacity;
				state = GameObjectFaderState.transparent;
			}
			updateMaterial ();

		}

		if (state == GameObjectFaderState.opaque) {

		}

	}

	public void fadeIn() {

		state = GameObjectFaderState.fadingIn;

	}

	public void fadeOut() {

		state = GameObjectFaderState.fadingOut;

	}
}
