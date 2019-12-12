using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AdjustAspect : MonoBehaviour {

	public float aspect = 1.777f;

	Image theImage;
	RectTransform rect;

	// Use this for initialization
	void Start () {

		theImage = this.GetComponent<Image> ();
		rect = theImage.GetComponent<RectTransform> ();


		Rect r = rect.rect;
		r.width = r.height * aspect;
		rect.localScale = new Vector3 (0.7f * aspect, 0.7f, 1);
		//rect.rect.Set(r.x, r.y, r.width, r.width);

	
	}
	

}
