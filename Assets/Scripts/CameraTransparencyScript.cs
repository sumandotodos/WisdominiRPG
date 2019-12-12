using UnityEngine;
using System.Collections;

public class CameraTransparencyScript : MonoBehaviour {
	
	/**
	 * This version of the script works with Mobile->Particles->Alpha Blended shader
	 * */

	public Camera cameraRef;

	public float minDist = 50.0f;
	public float region = 10.0f;

	public int axis = 0;

	Material matRef;

	// Use this for initialization
	void Start () {
	
		matRef = this.GetComponent<Renderer> ().material;
		Vector4 n;
		n = matRef.color;
		//n = matRef.GetColor("_TintColor");
		n.w = 0.5f;
		matRef.color = n;
		//matRef.SetColor("_TintColor", n);

	}
	
	// Update is called once per frame
	void Update () {

		float cameraCoord;
		float myCoord;

		// in Z

		Vector3 temp;
		Vector4 color;

		temp = cameraRef.transform.position;

		if (axis == 0)
			cameraCoord = temp.z;
		else
			cameraCoord = temp.y;

		temp = this.GetComponent<Renderer> ().bounds.center;

		if (axis == 0)
			myCoord = temp.z;
		else
			myCoord = temp.y;

		float dist = myCoord - cameraCoord;
		if (dist < minDist) {
			color = matRef.color;
			//color = matRef.GetColor("_TintColor");
			color.w = 0.0f;
			matRef.color = color;
			//matRef.SetColor("_TintColor", color);
		} else if (dist > minDist + region) {
			//color = matRef.GetColor("_TintColor");
			color = matRef.color;
			color.w = 1.0f;
			matRef.color = color;
			//matRef.SetColor("_TintColor", color);
		} else {
			//color = matRef.GetColor("_TintColor");
			color = matRef.color;
			color.w = (dist-minDist) / region;
			matRef.color = color;
			//matRef.SetColor("_TintColor", color);
		}

	
	}
}
