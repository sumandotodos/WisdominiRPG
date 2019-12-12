using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteAnimator : MonoBehaviour {

	public Sprite[] image;
	SpriteRenderer renderer;
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

		renderer = GetComponentInChildren<SpriteRenderer> ();



	}

	void OnDrawGizmos() {

		if (image.Length > 0) {
			
			GetComponentInChildren<SpriteRenderer> ().sprite = image[0];
		
		}

	}

	public void resetAnimation() {

		frame = 0;
		renderer.sprite = image [frame];

	}

	// Update is called once per frame
	void Update () {

		elapsedTime += Time.deltaTime;
		if (elapsedTime > (1.0 / animationSpeed)) {
			elapsedTime = 0.0f;
			if (image.Length > 0) {
				++frame;

				renderer.sprite = image[(frame + offset)%image.Length];
			}
		}

	}
}
