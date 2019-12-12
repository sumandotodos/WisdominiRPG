using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : WisdominiObject {

	public AudioClip clip;
	public AudioSource aSource;

	// Use this for initialization
	void Start () {
		aSource.clip = clip;
	}

	public void _wm_play() {
		aSource.Play ();
	}

	public void _wm_stop() {
		aSource.Stop ();
	}



}
