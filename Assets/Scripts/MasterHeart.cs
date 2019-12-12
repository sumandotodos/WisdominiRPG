using UnityEngine;
using System.Collections;

public class MasterHeart : MonoBehaviour {

	Vector3 initialPosition;

	public float phase;
	public float phaseSpeed;
	public float windSpeed;
	public float initialPhase;
	Vector3 displacement;
	Vector3 position;
	float scale;
	Vector3 initialLocalScale;

	// Use this for initialization
	void Start () {

		position = initialPosition = this.transform.localPosition;
		initialLocalScale = this.transform.localScale;
		displacement = new Vector3 (windSpeed, 0.5f, 0);
		phase = initialPhase;
		scale = phase / 90.0f;
		if (scale > 1.0f)
			scale = 1.0f;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		phase += phaseSpeed * Time.deltaTime;
		if (phase > 360.0f)
			phase -= 360.0f;

		scale = phase / 90.0f;
		if (scale > 1.0f)
			scale = 1.0f;

		position = initialPosition + displacement * phase/360.0f + new Vector3(Mathf.Sin(phase/4.0f), 0, 0)/120;

		this.transform.localPosition = position;
		this.transform.localScale = initialLocalScale * scale;

	}
}
