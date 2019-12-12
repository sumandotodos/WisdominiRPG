using UnityEngine;
using System.Collections;

public class SpriteQuadScript : MonoBehaviour {

	public Texture[] tex;
	public int nTexes;
	int texIndex;
	public float frameDuration;
	float latestTime;

	Renderer rRef;

	// Use this for initialization
	void Start () {
		
		texIndex = 0;
		latestTime = Time.time;
		rRef = this.GetComponent<Renderer> ();
		nTexes = tex.Length;
	
	}
	
	// Update is called once per frame
	void Update () {

		if ((Time.time - latestTime) > frameDuration) {
			latestTime = Time.time;
			rRef.material.mainTexture = tex [texIndex++];
			texIndex = texIndex % nTexes;
		}
	
	}
}
