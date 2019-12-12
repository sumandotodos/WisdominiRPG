using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

//enum UIInventoryItemStatus { appearing, idle, disappearing };

public class UIInventoryText : MonoBehaviour {


	/* references */

	public UIInventory inventoryController;
	public GameObject parent;

	/* public properties */

	public float delay;
	public bool running;
	public string theString = "";

    public Font font;

	/* properties */

	float opacity;
	float targetOpacity;
	float elapsedTime;
	UIInventoryItemStatus status;

	GameObject go;

	Text theText;
	RectTransform imageRect;

	/* constants */

	const float opacitySpeed = 1.4f;


	// Use this for initialization

	public void initialize () {

		elapsedTime = 0.0f;
		opacity = 0.0f;
		targetOpacity = 0.0f;
		status = UIInventoryItemStatus.appearing;

		//go = new GameObject ();
		theText = this.gameObject.AddComponent<Text> ();

		theText.text = theString;
		theText.fontStyle = FontStyle.Bold;
        theText.font = font;  //Resources.Load<Font>("Fonts/Arial");


		this.transform.parent = parent.transform;
		theText.transform.parent = this.transform;
		theText.raycastTarget = false;

		imageRect = theText.GetComponent<RectTransform> ();

		theText.color = new Vector4 (0.0f, 0.0f, 0.0f, opacity);


		running = false;

	}
		



	public void show() {

		elapsedTime = 0.0f;
		status = UIInventoryItemStatus.appearing;
		targetOpacity = 1.0f;
		running = true;

	}



	public void hide() {

		targetOpacity = 0.0f;
		//elapsedTime = 0.0f;

	}

	public void hide(float initialOpacity) {

		opacity = initialOpacity;
		targetOpacity = 0.0f;

	}



	// Update is called once per frame
	void Update () {

		if (status == UIInventoryItemStatus.appearing) {
			if (running && (elapsedTime < delay)) {
				elapsedTime += Time.deltaTime;
				if (elapsedTime > delay) {

					show ();
					status = UIInventoryItemStatus.idle;

				}
				return;
			}
		}

		if (opacity > targetOpacity) {

			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < targetOpacity) {

				opacity = targetOpacity;

			}

			theText.color = new Vector4 (0.0f, 0.0f, 0.0f, opacity);

		}

		if (opacity < targetOpacity) {

			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > targetOpacity) {

				opacity = targetOpacity;

			}

			theText.color = new Vector4 (0.0f, 0.0f, 0.0f, opacity);

		}

	}
}
