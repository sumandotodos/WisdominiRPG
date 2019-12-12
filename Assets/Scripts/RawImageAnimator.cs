using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RawImageAnimator : MonoBehaviour {

	public Texture[] image;
	public float animationSpeed;
	int frame;

	public int offset;

	RawImage img;

	float elapsedTime;

	// Use this for initialization
	void Start () {

		elapsedTime = 0.0f;
		frame = 0;

		img = this.GetComponent<RawImage> ();

	}

	void OnDrawGizmos() {

		if (image.Length > 0) {
			if (img != null) {
				img.texture = image [0];
			}
		}

	}

	public void resetAnimation() {

		frame = 0;
		if (img.enabled == true)
			img.texture = image [frame];

	}

	// Update is called once per frame
	void Update () {

		elapsedTime += Time.deltaTime;
		if (elapsedTime > (1.0f / animationSpeed)) {
			elapsedTime = 0.0f;
			if (image.Length > 0) {
				++frame;
				if(img.enabled == true) 
					img.texture = image [(frame + offset)%image.Length];
			}
		}

	}
}
