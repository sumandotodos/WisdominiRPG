using UnityEngine;
using System.Collections;

public class UINervous : MonoBehaviour {

	public float maxDisplacement;
	public float ratio;
	float currentDisplacement;

	Vector3 displacement;
	Vector3 originalPosition;

	float elapsedTime;
	float nextPeak;
	float peakDuration;

	const float meanPeakDuration = 1.0f;
	const float peakDurationSigma = 0.35f;

	const float meanTroughDuration = 1.5f;
	const float troughDurationSigma = 0.25f;

	int state = 0;

	// Use this for initialization
	void Start () {

		elapsedTime = 0.0f;
		nextPeak = Random.Range (meanTroughDuration - troughDurationSigma, meanTroughDuration + troughDurationSigma);
		originalPosition = this.transform.localPosition;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (state == 0) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > nextPeak) {
				elapsedTime = 0.0f;
				peakDuration = Random.Range (meanPeakDuration - peakDurationSigma, meanPeakDuration + peakDurationSigma);
				state = 1;
				currentDisplacement = maxDisplacement;
			}
		}

		if (state == 1) {
			elapsedTime += Time.deltaTime;
			if(elapsedTime > peakDuration) {
				elapsedTime = 0.0f;
				nextPeak = Random.Range (meanTroughDuration - troughDurationSigma, meanTroughDuration + troughDurationSigma);
				state = 0;
				currentDisplacement = maxDisplacement / ratio;
			}
		}

		displacement = new Vector3 (Random.Range (-currentDisplacement, currentDisplacement), 
			Random.Range (-currentDisplacement, currentDisplacement),
			Random.Range (-currentDisplacement, currentDisplacement));

		this.transform.localPosition = originalPosition + displacement;
	}
}
