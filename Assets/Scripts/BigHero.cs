using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BigHero : MonoBehaviour {

	public float initialY;
	public float finalY;
	public float ySpeed;
	public float finalOpacity;
	public float opacitySpeed;
	public HeroEffect heroEffect;
	float y;
	float targetY;
	float targetOpacity;
	float opacity = 0.0f;
	float x;

	Material mat;

	bool showing;

	bool resetting;

	// Use this for initialization
	void Start () {

		float aspect = Screen.width / Screen.height;

		mat = this.GetComponent<Renderer> ().material;
		opacity = targetOpacity = 0.0f;
		y = targetY = initialY;
		showing = false;
		resetting = false;
		x = 180 - (aspect-1.0f)*80.0f;
		mat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
		//this.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
		this.transform.localPosition = new Vector3(x, y, 0);

	}

	public void fadeIn() {

		targetOpacity = finalOpacity;

	}

	public void fadeOut() {

		targetOpacity = 0.0f;

	}

	public void scrollIn() {

		targetY = finalY;

	}

	public void effectGo() {

	}

	public void reset() {

		fadeOut ();
		resetting = true;


	}
	
	// Update is called once per frame
	void Update () {
	
		bool poschanged = Utils.updateSoftVariable(ref y, targetY, ySpeed);
		bool opchanged = Utils.updateSoftVariable(ref opacity, targetOpacity, opacitySpeed);

		if(poschanged) {
			this.transform.localPosition = new Vector3(x, y, 0);
		}

		if(opchanged) {
			mat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));		
		}

		if(resetting) {
			if(opacity == 0.0f) {
				resetting = false;
				targetY = y = initialY;
				this.transform.localPosition = new Vector3(x, y, 0);
			}
		}

	}
}
