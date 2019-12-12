using UnityEngine;
using System.Collections;

public class SpriteBillboardScript : MonoBehaviour {

	/* references */

	public Texture image;
	Material mat;

	// Use this for initialization
	void Start () {

		mat = null;

		Renderer r = this.GetComponent<Renderer> ();
		if(r!=null) {
			mat = r.material;
		}

		if(mat!=null) mat.mainTexture = image;
	
	}

	void OnDrawGizmos() {

		if (image != null) {

			Material m = this.GetComponent<Renderer> ().material;
			if (m != null) {

				m.mainTexture = image;

			}

		}

	}
	
	public Camera m_Camera;

	void Update()
	{
		if (m_Camera == null) {
			m_Camera = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		}
		transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
			m_Camera.transform.rotation * Vector3.up);
	}
}
