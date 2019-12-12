using UnityEngine;
using System.Collections;

public enum AspectAxis { x, y };

public class AutoAspect : MonoBehaviour {

	public AspectAxis axis = AspectAxis.x;

	// Use this for initialization
	void Start () {
	
		float aspect = Screen.width / Screen.height;

		if (axis == AspectAxis.x) {
			this.transform.localScale = new Vector3(this.transform.localScale.x * aspect, this.transform.localScale.y, this.transform.localScale.z);
		}
		else {
			this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y / aspect, this.transform.localScale.z);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
