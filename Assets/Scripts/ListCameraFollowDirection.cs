using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListCameraFollowDirection : MonoBehaviour {

	public List<string> camDirections;
	MasterControllerScript mcRef;
	DataStorage ds;
	LevelControllerScript level;
	string currentLevel;

	void Start () 
	{
		DontDestroyOnLoad (this.gameObject);
		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		currentLevel = mcRef.currentLocation;
		//ds = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ().getStorage ();
	}

	void Reset()
	{		
		//ds = mcRef.getStorage ();
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		camDirections = new List<string> ();
		GameObject[] directions = GameObject.FindGameObjectsWithTag ("CamDirection");

		for (int i = 0; i < directions.Length; i++) 
		{
			camDirections.Add (directions [i].name);
		}
	}

	void Update()
	{
		if (currentLevel == mcRef.currentLocation) 
		{
			return;
		} 
		else 
		{
			Reset ();
			currentLevel = mcRef.currentLocation;
		}
	}
	
	public void SaveLastTrigger(GameObject _obj, string _lvl)
	{
		mcRef.getStorage().storeStringValue ("LastTriggerIn" + _lvl, _obj.name);
	}

	public void NextLastTrigger(string _lvl)
	{
		string lastTrigger = mcRef.getStorage().retrieveStringValue ("LastTriggerIn" + _lvl);

		if (camDirections.Contains (lastTrigger)) 
		{
			int lastNumber = camDirections.IndexOf (lastTrigger);

			if (lastNumber < camDirections.Count - 1) 
			{
				mcRef.getStorage().storeStringValue ("LastTriggerIn" + _lvl, camDirections [lastNumber++]);
			} 
			else {
				mcRef.getStorage().storeStringValue ("LastTriggerIn" + _lvl, "");
			}
		}
	}
}
