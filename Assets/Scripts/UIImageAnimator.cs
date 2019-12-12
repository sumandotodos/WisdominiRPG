using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIImageAnimator : MonoBehaviour {

	public Sprite[] image;
	public float animationSpeed;
	int frame;
	public bool enableControl = true;

	Image img;

	public bool loop = false;
	public bool autostart = true;

	float elapsedTime;

	// Use this for initialization
	void Start () {

		elapsedTime = 0.0f;
		frame = 0;
	
		img = this.GetComponent<Image> ();

		if (!autostart)
		 if(enableControl) img.enabled = false;

	}

	public void resetAnimation() {

		frame = 0;
		img.sprite = image [frame];
		elapsedTime = 0.0f;
		if(enableControl) img.enabled = true;
		autostart = true;

	}
	
	// Update is called once per frame
	void Update () {

		if (!autostart)
			return;
	
		elapsedTime += Time.deltaTime;
		if (elapsedTime > (1.0f / animationSpeed)) {
			elapsedTime = 0.0f;
			if (frame < (image.Length - 1)) {
				++frame;
				img.sprite = image [frame];
			} else {
				if (loop) {
					frame = 0;
					img.sprite = image [frame];
				} else {
					if(enableControl) img.enabled = false;
				}
			}
		}

	}
}
