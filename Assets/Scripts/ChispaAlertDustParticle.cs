using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChispaAlertDustParticle : MonoBehaviour {

	/* references */

	public Sprite [] dust;
	RectTransform rect;
	Image theImage;


	/* properties */

	int frame;
	float ySpeed;
	float yPos;
	float elapsedTime;
	Vector2 originalPos;

	/* constants */

	const float animationSpeed = 3.0f;
	const float gravity = -3.0f;

	// Use this for initialization
	public void initialize () {
	
		frame = 0;
		ySpeed = 0;
		elapsedTime = 0.0f;
		theImage = this.GetComponent<Image> ();
		theImage.sprite = dust [0];
		rect = this.GetComponent<RectTransform> ();
		originalPos = rect.transform.position;
		yPos = originalPos.y;

	}

	void Start() {

		initialize ();

	}

	void Update() {

		ySpeed += gravity * Time.deltaTime;
		yPos += ySpeed;

		rect.transform.position = new Vector2 (originalPos.x, yPos);

		elapsedTime += Time.deltaTime;

		if (elapsedTime > (1.0f / animationSpeed)) {
			elapsedTime = 0.0f;
			++frame;
			if (frame < dust.Length) {
				theImage.sprite  = dust [frame];
			} else {
				theImage.color = new Color (0, 0, 0, 0);
				Destroy (this.gameObject);
			}
		}

	}
	

}
