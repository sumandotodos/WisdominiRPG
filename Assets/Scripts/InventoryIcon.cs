using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InventoryIcon : MonoBehaviour {

	public InventoryObject inventory;
	public Camera camRef;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit2D hit;

		if (Input.GetMouseButton (0)) {
			Ray ray = camRef.ScreenPointToRay (Input.mousePosition);

			hit = Physics2D.Raycast (ray.origin, new Vector2 (0, 0));
			if (hit != null) {
				if (hit.collider != null) {
					inventory._wm_invertState ();
				}
			}

		}
	
	}

	public void OnPointerClick(PointerEventData eventData) 
	{
		inventory._wm_invertState ();
	}
}
