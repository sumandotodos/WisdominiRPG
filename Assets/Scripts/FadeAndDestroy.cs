using UnityEngine;
using System.Collections;

public class FadeAndDestroy : MonoBehaviour {

	public float initialDelay;
	public float fadeDuration;

	int state = 0;
	float elapsedTime;

	float opacity;
	float targetOpacity;

	Material mat;

	// Use this for initialization
	void Start () {
	
		mat = this.GetComponent<Renderer> ().material;
		opacity = 1.0f;
		targetOpacity = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == 0) { // initial delay

			elapsedTime += Time.deltaTime;
			if (elapsedTime > initialDelay) {
				elapsedTime = 0.0f;
				++state;
			}

		}

		if (state == 1) {

			bool change = Utils.updateSoftVariable (ref opacity, targetOpacity, 1.0f / fadeDuration);
			if (change) {
				mat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
			} else
				++state;

		}

		if (state == 2) {
			Destroy (this.gameObject);
		}

	}
}
