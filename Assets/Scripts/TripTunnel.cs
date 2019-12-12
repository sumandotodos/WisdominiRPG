using UnityEngine;
using System.Collections;

public class TripTunnel : MonoBehaviour {

	float za;
	public float rotationSpeed;
	public AudioClip sound;
	AudioSource aSource;

	// Use this for initialization
	void Start () {
	
		za = 0.0f;
		aSource = this.GetComponent<AudioSource> ();
		aSource.PlayOneShot (sound);

	}
	
	// Update is called once per frame
	void Update () {

		za += rotationSpeed * Time.deltaTime;

		this.transform.localRotation = Quaternion.Euler (0, 0, za);
	
	}
}
