using UnityEngine;
using System.Collections;

public class TabController : MonoBehaviour {

	public float targetScale;
	public float scale;

	const float scaleSpeed = 3.0f;

	Vector3 pickUpPosition;
	Transform p;

	// Use this for initialization
	void Start () {

		p = null;
		scale = 0.0f;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (scale < targetScale) {
			scale += scaleSpeed * Time.deltaTime;
			if (scale > targetScale)
				scale = targetScale;
		}

		if (scale > targetScale) {
			scale -= scaleSpeed * Time.deltaTime;
			if (scale < targetScale)
				scale = targetScale;
		}

		this.transform.localScale = new Vector3 (scale, scale, scale);

		if (p != null) {
			this.transform.position = p.position;
		}

	}

	public void attachTo(Transform tr) {

		p = tr;

	}

	public void setPickupPosition(Vector3 pos) {

		pickUpPosition = pos;
		this.transform.position = pos;

	}

	public void touch() {

		targetScale = 1.0f;

	}

	public void release() {

		p = null;
		targetScale = 0.0f;

	}
}
