using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVController : MonoBehaviour {

	public Texture tvCracked;
	public Texture tvEmision;
	public GameObject particles;
	public float speedOffset;

	Material mat;
	bool moveTV;

	void Start () 
	{
		particles.SetActive (false);
		mat = this.GetComponent<Renderer> ().material;
		mat.mainTexture = tvCracked;
		moveTV = true;
	}

	public void ChangeTexture()
	{
		moveTV = !moveTV;
		particles.SetActive (false);
		mat.mainTexture = tvEmision;
		mat.mainTextureOffset = Vector2.zero;
	}

	public void ShowParticles()
	{
		particles.SetActive (true);
	}

	void Update () 
	{
		if (moveTV) 
		{
			mat.mainTextureOffset += new Vector2(0, speedOffset * Time.deltaTime);
		}
	}
}
