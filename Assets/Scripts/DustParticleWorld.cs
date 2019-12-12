using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DustParticleWorld : MonoBehaviour {

	/* references */

	public Texture [] dust;
	//RectTransform rect;
	//Image theImage;
	Material mat;


	/* properties */

	int frame;
	float ySpeed;
	float yPos;
	float elapsedTime;
	Vector3 originalPos;

	/* constants */

	const float animationSpeed = 8.0f;
	const float gravity = -0.3f;

	// Use this for initialization
	public void initialize () {

		mat = this.GetComponent<Renderer> ().material;
		frame = 0;
		ySpeed = 0;
		elapsedTime = 0.0f;
		mat.mainTexture = dust [0];
		originalPos = this.transform.position;
		yPos = originalPos.y;

	}

	void Start() {

		initialize ();

	}

	void Update() {

		ySpeed += gravity * Time.deltaTime;
		yPos += ySpeed;

		this.transform.position = new Vector3 (originalPos.x, yPos, originalPos.z);

		elapsedTime += Time.deltaTime;

		if (elapsedTime > (1.0f / animationSpeed)) {
			elapsedTime = 0.0f;
			++frame;
			if (frame < dust.Length) {
				mat.mainTexture  = dust [frame];
			} else {
				mat.color = new Color (0, 0, 0, 0);
				Destroy (this.gameObject);
			}
		}

	}


}

