using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class BackgroundOffsetter : MonoBehaviour {

	public RawImage bbb;

	void Start() {

		float u, v;
		Vector2 off;
		off = bbb.uvRect.position;
		u = off.x;
		v = off.y;



	}

	void Update() {

		float u, v;
		Vector2 off;
		off = bbb.uvRect.position;
		Rect r;

		u = bbb.uvRect.x;
		v = bbb.uvRect.y;

		u += (0.2f * Time.deltaTime);
		v += (0.1f * Time.deltaTime);

		r = bbb.uvRect;

		r.x = u;
		r.y = v;
		//off = new Vector2 (u, v);
		bbb.uvRect = r;

	}

}
