using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PositionChoose : MonoBehaviour {

	public GameObject[] position;
	public List<bool> usedPosition;
	public LevelControllerScript level;
	public string heavenVariableName;

	//int cycles = 0;

	public GameObject[] relocate;

	void Start () {

		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
	
		usedPosition = new List<bool> ();
		for (int i = 0; i < position.Length; ++i) 
		{
			usedPosition.Add (false);
		}
		int n = level.retrieveIntValue (heavenVariableName);
		//Random.seed = n;
		Random.InitState (n);
		distribute ();
	}

	Vector3 choosePosition() 
	{
		int r = Random.Range (0, position.Length - 1);
		while (usedPosition [r])
		{
			r = Random.Range (0, position.Length - 1);
		}
		usedPosition [r] = true; // do not repeat!!
		return position [r].transform.position;
	}

	public void distribute() 
	{
		if (relocate.Length >= position.Length)
			return; // cannot do

		for (int i = 0; i < relocate.Length; ++i) 
		{
			Vector3 newLocation = choosePosition ();
			relocate [i].gameObject.transform.position = newLocation;
		}
	}
}
