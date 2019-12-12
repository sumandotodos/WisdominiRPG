using UnityEngine;
using System.Collections;

public class FloatRandom {

	public static float floatRandomRange(float min, float max) {

		int iMax, iMin;

		const float granularity = 10000.0f;

		iMax = (int)(max * granularity);
		iMin = (int)(min * granularity);

		int iRes = Random.Range(iMin, iMax);

		float fRes = ((float)iRes) / granularity;

		return fRes;

	}
}
