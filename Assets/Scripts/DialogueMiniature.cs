using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueMiniature : MonoBehaviour {

	public float scaleSpeed;
	public Image theImage;

	const float SCALESPEED = 0.04f;
	const float STARTSCALE = 0.5f;
	private float scale;

	// Use this for initialization
	void Start () {

		scale = STARTSCALE;
		scaleSpeed = SCALESPEED;
		this.transform.localScale = new Vector3 (scale, scale, scale);
		theImage.color = new Vector4 (1.0f, 1.0f, 1.0f, scale);

	}
	
	// Update is called once per frame
	void Update () {

		if (scale < 1.0f) {

			scale += scaleSpeed;
			if (scale >= 1.0f) {

				scale = 1.0f;

			}

			this.transform.localScale = new Vector3 (scale, scale, scale);
			theImage.color = new Vector4 (1.0f, 1.0f, 1.0f, scale);

		}
	
	}
}
