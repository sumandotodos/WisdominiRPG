using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

enum UIInventoryItemStatus { appearing, idle, disappearing };

public class UIInventoryItem : MonoBehaviour {


	/* references */

	public Sprite theSprite;
	public UIInventory inventoryController;
	public GameObject parent;

	/* public properties */

	public UIInventoryClickItem id;
	public float delay;
	public bool running;
	public float maxOpacity = 1.0f;
	public int intId = 0;

	/* properties */

	float opacity;
	float targetOpacity;
	float elapsedTime;
	UIInventoryItemStatus status;

	GameObject go;

	Image theImage;
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

		theImage = this.gameObject.AddComponent<Image> ();

		theImage.sprite = theSprite;
		this.transform.parent = parent.transform;
		theImage.transform.parent = this.transform;

		imageRect = theImage.GetComponent<RectTransform> ();

		float op;
		op = maxOpacity * opacity;
		if (op > maxOpacity)
			op = maxOpacity;
		theImage.color = new Vector4 (1.0f, 1.0f, 1.0f, op);

		EventTrigger evt = this.gameObject.AddComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback = new EventTrigger.TriggerEvent();
		entry.callback.AddListener((eventData) => UIItemClickedOn());
		evt.triggers.Add (entry);
		running = false;

	}

	public void setWidth(float width) {

		imageRect.sizeDelta = new Vector2 (width, imageRect.sizeDelta.y);

	}


	public void setHeight(float height) {

		imageRect.sizeDelta = new Vector2 (imageRect.sizeDelta.x, height);
	}


	public void setDimensions(float width, float height) {

		imageRect.sizeDelta = new Vector2 (width, height);

	}


	public void UIItemClickedOn() {

		if (opacity < 0.9f)
			return;
		inventoryController.notifyItemClicked (id, intId);

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

	public void disableRaycast() {

		theImage.raycastTarget = false;

	}

	public void enableRaycast() {

		theImage.raycastTarget = true;

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
			float op;
			op = maxOpacity * opacity;
			if (op > maxOpacity)
				op = maxOpacity;
			theImage.color = new Vector4 (1.0f, 1.0f, 1.0f, op);

		}

		if (opacity < targetOpacity) {

			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > targetOpacity) {

				opacity = targetOpacity;

			}
			float op;
			op = maxOpacity * opacity;
			if (op > maxOpacity)
				op = maxOpacity;
			theImage.color = new Vector4 (1.0f, 1.0f, 1.0f, op);

		}

	}
}
