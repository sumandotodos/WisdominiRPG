using UnityEngine;
using System.Collections;

public class FaderScriptBetter : WisdominiObject {

	Material theMaterial = null;
	TextMesh theTextMesh = null;
	SoftFloat opacity;
	public bool startOpaque = true;
	Color initialColor;

	public float opValue;

	// Use this for initialization
	new void Start () {
	
		SpriteRenderer r = this.GetComponent<SpriteRenderer> ();
		if (r != null) {
			theMaterial = r.material;
			initialColor = theMaterial.color;
		}
		else {

			theTextMesh = this.GetComponent<TextMesh> ();
			if (theTextMesh != null) {
				
				initialColor = theTextMesh.GetComponent<MeshRenderer>().material.GetColor("_Color");

			}

		}
		opacity = new SoftFloat ();
		opacity.setSpeed (1.0f);
		opacity.setTransformation (TweenTransforms.linear);
		if (startOpaque)
			opacity.setValueImmediate (1.0f);
		else
			opacity.setValueImmediate (0.0f);

	}
	
	// Update is called once per frame
	new void Update () {
		if (opacity.update ()) {
			if(theMaterial != null) theMaterial.color = new Color (initialColor.r, initialColor.g, initialColor.b, opacity.getValue ());
			if (theTextMesh != null) {
				//theTextMesh.color = new Color (initialColor.r, initialColor.g, initialColor.b, opacity.getValue ());
				theTextMesh.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color (initialColor.r, initialColor.g, initialColor.b, opacity.getValue ()));
			}
			opValue = opacity.getValue ();
		}
	}

	public void _wm_setOpacity(float op) {

		FaderScriptBetter[] fs = this.GetComponentsInChildren<FaderScriptBetter> ();
		foreach (FaderScriptBetter f in fs) {
			if (f == this) {
				opacity.setValueImmediate (op);
				if(theMaterial != null) theMaterial.color = new Color (initialColor.r, initialColor.g, initialColor.b, opacity.getValue ());
				if (theTextMesh != null)
					theTextMesh.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color (initialColor.r, initialColor.g, initialColor.b, opacity.getValue ()));
			} else
				f._wm_setOpacity (op);
		}


	}

	public void _wm_fadeToOpaque() {
		FaderScriptBetter[] fs = this.GetComponentsInChildren<FaderScriptBetter> ();
		foreach (FaderScriptBetter f in fs) {
			if (f == this)
				opacity.setValue (1.0f);
			else
				f._wm_fadeToOpaque ();
		}
	}

	public void _wm_fadeToTransparent() {
		FaderScriptBetter[] fs = this.GetComponentsInChildren<FaderScriptBetter> ();
		foreach (FaderScriptBetter f in fs) {
			if (f == this)
				opacity.setValue (0.0f);
			else
				f._wm_fadeToTransparent ();
		}
	}
}
