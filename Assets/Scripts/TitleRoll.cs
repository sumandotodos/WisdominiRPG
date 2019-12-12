using UnityEngine;
using System.Collections;

public class TitleRoll : MonoBehaviour {

	public float speed;

	Vector3 initialPos;

	float displ;

	// Use this for initialization
	void Start () {
	
		initialPos = this.transform.localPosition;
		displ = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {

		displ += speed * Time.deltaTime;
		this.transform.localPosition = initialPos + new Vector3 (0, displ, 0);
	
	}
}
