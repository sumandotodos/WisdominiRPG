using UnityEngine;
using System.Collections;

public class CandleFlame : MonoBehaviour {

	public float minScaleX = 0.8f;
	public float maxScaleX = 1.0f;
	public float minScaleY = 1.1f;
	public float maxScaleY = 1.32f;

	public float flickerSpeed = 1.0f;

	float elapsedTime;

	Vector3 originalScale;

	float seedX;
	float seedY;

	// Use this for initialization
	void Start () {
	
		seedX = FloatRandom.floatRandomRange (0.0f, 6.28f);
		seedY = FloatRandom.floatRandomRange (0.0f, 6.28f);

		originalScale = this.transform.localScale;

	}
	
	// Update is called once per frame
	void Update () {

		float sx, sy;

		elapsedTime += Time.deltaTime * flickerSpeed;

		sx = minScaleX + ((maxScaleX - minScaleX) / 2.0f) + ((maxScaleX - minScaleX) / 2.0f) * Mathf.Cos (seedX + elapsedTime);
		sy = minScaleY + ((maxScaleY - minScaleY) / 2.0f) + ((maxScaleY - minScaleY) / 2.0f) * Mathf.Cos (seedY + elapsedTime);

		Vector3 newScale = new Vector3 (sx * originalScale.x, sy * originalScale.y, originalScale.z);
		this.transform.localScale = newScale;
	
	}
}
