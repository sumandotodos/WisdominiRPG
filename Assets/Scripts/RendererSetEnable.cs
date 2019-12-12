using UnityEngine;
using System.Collections;

public class RendererSetEnable : WisdominiObject {

	Renderer r;

	// Use this for initialization
	new void Start () {
	
		r = this.GetComponent<Renderer> ();

	}
	
	public void _wm_setRendererEnabled(bool e) {
		r.enabled = e;
	}
}
