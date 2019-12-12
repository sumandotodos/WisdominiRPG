using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UILoding : MonoBehaviour {

	/* references */

	public Text label;
	public Image image;
	public Image yinYang;

	// Use this for initialization
	void Start () {
	
		label.enabled = false;
		image.enabled = false;
		yinYang.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void load() {

		label.enabled = true;
		image.enabled = true;
		yinYang.enabled = true;

	}
}
