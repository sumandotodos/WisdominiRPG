using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Distributor : MonoBehaviour {

	public GameObject[] targets;
	public GameObject[] sources;

	List<int> selectedSources;

	// place target objects in locations taken at random from sources

	// Use this for initialization
	void Start () {

		selectedSources = new List<int>();

		for (int i = 0; i < targets.Length; ++i) {

			bool clash = true;
			int source = Random.Range (0, sources.Length - 1);
			while (clash) {
				clash = false;
				if (selectedSources.Contains (source))
					clash = true;
				source = (source + 1) % sources.Length;
			}

			targets [i].transform.position = sources [source].transform.position;
			selectedSources.Add (source); // do not reuse that source

		}
	
	}
	

}
