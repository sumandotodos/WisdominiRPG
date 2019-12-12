using UnityEngine;
using System.Collections;

public class StargateUVRotate : MonoBehaviour {

	/* references */

	Mesh m;


	/* properties */

	float angle;
	Vector2[] originalUV;
	Vector2 offset;
	public Vector2 fixOffset;

	/* public properties */

	public Color tint = Color.red;


	/* constants */

	const float angleSpeed = 200.0f;



	// Use this for initialization
	void Start () {

		m = this.GetComponent<MeshFilter> ().mesh;
		angle = 2.0f;
		originalUV = m.uv;
		for (int i = 0; i < originalUV.Length; ++i) {
			originalUV [i] = originalUV[i] + fixOffset;
		}
		float avgU, avgV;
		avgU = avgV = 0.0f;
		for (int i = 0; i < originalUV.Length; ++i) {
			avgU += originalUV [i].x;
			avgV += originalUV [i].y;
		}
		avgU /= (float)originalUV.Length;
		avgV /= (float)originalUV.Length;
		offset = new Vector2 (avgU, avgV);


		//this.GetComponent<Renderer> ().material.SetColor ("_TintColor", tint);

	}

	void OnSceneGUI()
	{

		//this.GetComponent<Renderer> ().material.SetColor ("_TintColor", tint);

	}
	
	// Update is called once per frame
	void Update () {
		
		Vector2[] newUVs = originalUV;
		//Vector2 offset = new Vector2 (0.5f, 0.5f);


		for (int i = 0; i < newUVs.Length; ++i) {
			newUVs [i] -= offset;
			newUVs [i] = Quaternion.AngleAxis (angleSpeed * Time.deltaTime, Vector3.forward) * newUVs [i];
			newUVs [i] += offset;
		}

		m.uv = newUVs;
	
		//angle += angleSpeed * Time.deltaTime;

		//Vector2 source_uv = new Vector2(someX, someY);
		//Vrctor2 rotated_uv = Quaternion.AngleAxis(30f,Vector3.up) * source_uv;
	
	}
}
