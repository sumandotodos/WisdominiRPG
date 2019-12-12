using UnityEngine;
using System.Collections;

public class WellObject : MonoBehaviour {

	public WellScript[] well;
	public LevelControllerScript level;
	public string variableName;

	void Start () {
	
		bool b = level.retrieveBoolValue (variableName);
		if(!b) 
		{
			for (int i = 0; i < well.Length; ++i) 
			{
				well [i].closeWell ();
			}
		}
	}
}
