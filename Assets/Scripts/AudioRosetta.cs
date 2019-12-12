using UnityEngine;
using System.Collections;

public class AudioRosetta : MonoBehaviour {

	public MasterControllerScript masterController;

	// Use this for initialization
	void Start () {

		DontDestroyOnLoad (this);
	
	}

	public AudioClip returnClip(string name) {

		AudioClip result;

		string path = "FinalAssets/Audio/Speech/" + masterController.getLocale() + "/" + name;
		result = Resources.Load<AudioClip>(path);
		return result;

	}


}
