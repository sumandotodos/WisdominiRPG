using UnityEngine;
using System.Collections;

public enum PortalStatus { on, off, fadingOut };

public class DimensionalPortal : WisdominiObject {

	/* references */

	public Material matRef;
	public GameObject MeshRef;
	Collider collRef;


	/* properties */

	float vOffset = 0.0f;
	const float vSpeed = -1.0f;
	PortalStatus status;
	float opacity = 1.0f;
	const float opacitySpeed = 0.75f;


	// Use this for initialization
	new void Start () {

		status = PortalStatus.on;
		collRef = MeshRef.GetComponent<Collider> ();
		collRef.enabled = true;
		Vector4 newColor = new Vector4 (1.0f, 1.0f, 1.0f, 1.0f);
		matRef.color = newColor;
	
	}
	
	// Update is called once per frame
	new void Update () {

		Vector2 off = new Vector2 (0.0f, vOffset);
		matRef.mainTextureOffset = off;

		vOffset += vSpeed * Time.deltaTime;

		if (status == PortalStatus.fadingOut) {

			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < 0.0f) {
				opacity = 0.0f;
				status = PortalStatus.off;
				collRef.enabled = false;
			}

			Vector4 newColor = new Vector4 (1.0f, 1.0f, 1.0f, opacity);
			matRef.color = newColor;


		}


	
	}

	public void _wm_disable() {

		status = PortalStatus.fadingOut;

	}



}
