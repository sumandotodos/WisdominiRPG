using UnityEngine;
using System.Collections;

public class FootstepSoundManager : MonoBehaviour {

	public MasterControllerScript mc;
	DataStorage ds;

	public AudioClip[] bog;
	public AudioClip[] sand;
	public AudioClip[] grass;
	public AudioClip[] dirt;
	public AudioClip[] concrete;
	public AudioClip[] wood;
	public AudioClip[] marble;
	public AudioClip[] metal;
	public AudioClip[] polished;
	public AudioClip[] carpet;
	public AudioClip[] ceramic;

	AudioClip[] current;

	AudioSource aSource;

	// Use this for initialization
	void Start () {
	
		current = grass;
		aSource = this.GetComponent<AudioSource> ();
		DontDestroyOnLoad (this.gameObject);
		ds = mc.getStorage ();

	}

	public void setGroundType(string gtype) {

		if (gtype.Equals ("bog"))
			current = bog;
		else if (gtype.Equals ("sand"))
			current = sand;
		else if (gtype.Equals ("grass"))
			current = grass;
		else if (gtype.Equals ("dirt"))
			current = dirt;
		else if (gtype.Equals ("concrete"))
			current = concrete;
		else if (gtype.Equals ("wood"))
			current = wood;
		else if (gtype.Equals ("marble"))
			current = marble;
		else if (gtype.Equals ("metal"))
			current = metal;
		else if (gtype.Equals ("polished"))
			current = polished;
		else if (gtype.Equals ("carpet"))
			current = carpet;
		else if (gtype.Equals ("ceramic"))
			current = ceramic;

		mc.getStorage().storeStringValue ("GroundType", gtype);
		

	}

	public void step() {

		int s = Random.Range (0, current.Length - 1);
		float p = Random.Range (0.8f, 1.2f);
		aSource.pitch = p;
		aSource.PlayOneShot (current [s]);

	}
	

}
