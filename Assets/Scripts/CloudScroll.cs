using UnityEngine;
using System.Collections;

public class CloudScroll : MonoBehaviour {

	public float xSize;
	public float xScroll = 0.0f;
	public float xSpeed;
	Vector3 initialPos;

	// Use this for initialization
	void Start () {
	
		initialPos = this.transform.position;

	}
	
	// Update is called once per frame
	void Update () {

		xScroll += xSpeed * Time.deltaTime;

		if (xScroll > xSize * 2)
			xScroll -= xSize * 2; 

		this.transform.position = initialPos + new Vector3 (xScroll, 0, 0);
	
	}


}
