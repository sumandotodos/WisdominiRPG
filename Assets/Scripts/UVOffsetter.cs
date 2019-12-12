using UnityEngine;
using System.Collections;

public class UVOffsetter : MonoBehaviour {

	public float uSpeed;
	public float vSpeed;

	float deltaU, deltaV;

	Material mat;

	// Use this for initialization
	void Start () {
	
		mat = this.GetComponent<Renderer> ().material;

		deltaU = 0.0f;
		deltaV = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {

		deltaU += uSpeed * Time.deltaTime;
		deltaV += vSpeed * Time.deltaTime;

		mat.SetTextureOffset("_MainTex", new Vector2(deltaU, deltaV));
		mat.SetTextureOffset("_DiffuseMapTransA", new Vector2(deltaU, deltaV));
	
	}
}
