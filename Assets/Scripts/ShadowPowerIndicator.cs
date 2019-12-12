using UnityEngine;
using System.Collections;

public class ShadowPowerIndicator : MonoBehaviour {

	public Texture controlTex;
	public Texture strengthTex;
	public TextMesh theText;

	bool isControl = false;

	Material mat;

	const float opacitySpeed = 0.4f;
	const float delay = 2.0f;

	float elapsedTime;

	float opacity;

	int state;

	void Start () 
	{	
		//initialize ();
	}

	public void initialize(bool control, int value) 
	{
		mat = this.GetComponent<Renderer> ().material;
		isControl = control;
		if (isControl)
			mat.mainTexture = controlTex;
		else
			mat.mainTexture = strengthTex;

		theText.text = "" + value;

		elapsedTime = 0.0f;
		opacity = 0.0f;

		mat.SetColor ("_TintColor", new Color (1, 1, 1, 0));
		theText.color = new Color (1, 1, 1, 0);

		state = -1;
	}

	void Update () 
	{	
		if (state == -1) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > delay / 2.0f) {
				elapsedTime = 0.0f;
				++state;
			}
		}

		if (state == 0) 
		{
			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > 1.0f) {
				opacity = 1.0f;
				++state;
			}
			mat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
			theText.color = new Color (1, 1, 1, opacity);
		}

		if (state == 1) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > delay) {
				elapsedTime = 0.0f;
				++state;
			}
		}

		if (state == 2) 
		{
			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < 0.0f) {
				opacity = 0.0f;
				++state;
			}
			mat.SetColor ("_TintColor", new Color (1, 1, 1, opacity));
			theText.color = new Color (1, 1, 1, opacity);
		}

		if (state == 3) 
		{
			Destroy (this.gameObject);
			++state;
		}
	}
}
