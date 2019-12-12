using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextureAnimator : MonoBehaviour {

	public Texture[] image;
	public float animationSpeed;
	int frame;

	public int offset;

	Material mat;

	float elapsedTime;

	public bool useSharedMat = true;

	// Use this for initialization
	void Start () {

		elapsedTime = 0.0f;
		frame = 0;

		if (!useSharedMat) {
			mat = this.GetComponent<Renderer> ().material;
		} else {
			mat = this.GetComponent<Renderer> ().sharedMaterial;
		}

	}

	void OnDrawGizmos() {

		if (image.Length > 0) {
			//this.GetComponent<Renderer> ().material.mainTexture = image [0];
			this.GetComponent<Renderer> ().sharedMaterial.mainTexture = image [0];

		}

	}

	public void resetAnimation() {

		frame = 0;
		mat.mainTexture = image [frame];

	}

	// Update is called once per frame
	void Update () {

		elapsedTime += Time.deltaTime;
		if (elapsedTime > (1.0 / animationSpeed)) {
			elapsedTime = 0.0f;
			if (image.Length > 0) {
				++frame;
				mat.mainTexture = image [(frame + offset)%image.Length];
			}
		}

	}
}
