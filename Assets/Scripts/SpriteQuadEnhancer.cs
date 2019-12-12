using UnityEngine;
using System.Collections;

public class SpriteQuadEnhancer : MonoBehaviour {

	/* references */

	public Texture tex;



	/* properties */

	public float width, height;
	float opacity;
	float scale;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setDimensions(float w, float h) {

		this.transform.localScale = new Vector3(w, h, 1);

	}
}
