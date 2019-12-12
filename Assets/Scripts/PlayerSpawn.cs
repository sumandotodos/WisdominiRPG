using UnityEngine;
using System.Collections;

enum PlayerSpawnState { idle1, appearing, idle2, disappearing };

public class PlayerSpawn : MonoBehaviour {

	/* references */ // lots of references

	public GameObject Verticals;
	public GameObject[] Pillars;
	public GameObject Skirt1;
	public GameObject[] skirt1Pieces;
	public GameObject Skirt2;
	public GameObject[] skirt2Pieces;


	/* properties */

	float skirt1Angle, skirt2Angle;
	float verticalsAngle;

	float scaleAngle;
	float opacityAngle;

	float globalOpacity;

	float[] initialPhase;
	Material[] materials;

	float elapsedTime;

	PlayerSpawnState state;

	/* constants */

	const float skirtSpeed = 80.0f;
	const float verticalsSpeed = 50.0f;
	const float scaleAngleSpeed = 9.0f;
	const float opacitySpeed = 6.0f;
	const float idle1Delay = 0.6f;
	const float globalOpacitySpeed = 1.0f;
	const float idle2Delay = 3.0f;

	// Use this for initialization
	void Start () {

		verticalsAngle = skirt1Angle = skirt2Angle = scaleAngle = 0.0f;

		initialPhase = new float[3 + Pillars.Length + skirt1Pieces.Length + skirt2Pieces.Length];

		globalOpacity = 0.0f;

		elapsedTime = 0.0f;

		for (int i = 0; i < initialPhase.Length; ++i) {
			initialPhase [i] = FloatRandom.floatRandomRange (0, 360.0f);
		}

		/*for (int i = 0; i < Pillars.Length; ++i) {
			Pillars[i].GetComponent<Renderer>().material.SetInt ("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
		}
		for (int i = 0; i < skirt1Pieces.Length; ++i) {
			skirt1Pieces[i].GetComponent<Renderer>().material.SetInt ("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
		}
		for (int i = 0; i < skirt2Pieces.Length; ++i) {
			skirt2Pieces[i].GetComponent<Renderer>().material.SetInt ("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
		}*/

		int k = 0; int j = 0;
		materials = new Material[Pillars.Length + skirt1Pieces.Length + skirt2Pieces.Length];
		for(j = 0; j<Pillars.Length; ++j) {
			materials[k++] = Pillars[j].GetComponent<Renderer>().material;
		}
		for(j = 0; j<skirt1Pieces.Length; ++j) {
			materials[k++] = skirt1Pieces[j].GetComponent<Renderer>().material;
		}
		for(j = 0; j<skirt2Pieces.Length; ++j) {
			materials[k++] = skirt2Pieces[j].GetComponent<Renderer>().material;
		}

		state = PlayerSpawnState.idle1;


	
	}
	
	// Update is called once per frame
	void Update () {

		if (state == PlayerSpawnState.idle1) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > idle1Delay) {
				elapsedTime = 0.0f;
				state = PlayerSpawnState.appearing;
			}
		}

		if (state == PlayerSpawnState.appearing) {
			globalOpacity += globalOpacitySpeed * Time.deltaTime;
			if (globalOpacity > 1.0f) {
				globalOpacity = 1.0f;
				state = PlayerSpawnState.idle2;
			}
		}

		if (state == PlayerSpawnState.idle2) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > idle2Delay) {
				elapsedTime = 0.0f;
				state = PlayerSpawnState.disappearing;
			}
		}

		if (state == PlayerSpawnState.disappearing) {
			globalOpacity -= globalOpacitySpeed * Time.deltaTime;
			if (globalOpacity < 0.0f) {
				globalOpacity = 0.0f;
				Destroy (this.gameObject);
			}
		}

		int k = 0;

		opacityAngle += opacitySpeed * Time.deltaTime;

		verticalsAngle += verticalsSpeed * Time.deltaTime;
		skirt1Angle += skirtSpeed * Time.deltaTime;
		skirt2Angle -= skirtSpeed * Time.deltaTime;

		Verticals.transform.rotation = Quaternion.Euler (0, initialPhase[k++]+verticalsAngle, 0);
		Skirt1.transform.rotation = Quaternion.Euler (0, initialPhase[k++]+skirt1Angle, 0);
		Skirt2.transform.rotation = Quaternion.Euler (0, initialPhase[k++]+skirt2Angle, 0);

		scaleAngle += scaleAngleSpeed * Time.deltaTime;

		for (int i = 0; i < skirt1Pieces.Length; ++i) {

			skirt1Pieces[i].transform.parent.transform.localScale = new Vector3(1, 0.75f + Mathf.Cos(initialPhase[k++]+scaleAngle)/4.0f ,1);

		}

		for (int i = 0; i < skirt2Pieces.Length; ++i) {

			skirt2Pieces[i].transform.parent.transform.localScale = new Vector3(1, 0.75f + Mathf.Cos(initialPhase[k++]+scaleAngle)/4.0f ,1);


		}

		for (int i = 0; i < Pillars.Length; ++i) {

			Pillars [i].transform.rotation = Quaternion.Euler (0, initialPhase[k++]+verticalsAngle*2, 0);

		}

		for (int i = 0; i < materials.Length; ++i) {

			materials [i].SetColor ("_Tint", new Color (1, 1, 1, (0.5f + Mathf.Sin (initialPhase[i]+opacityAngle)/2.0f)*globalOpacity));

		}
	
	}
}
