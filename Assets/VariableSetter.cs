using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class VariableSetter : MonoBehaviour {

	public string variableName;

	public DataType dataType;
	public float floatValue;
	public int intValue;
	public string stringValue;
	public bool boolValue;

	LevelControllerScript levelController;

	// Use this for initialization
	void Start () {
		levelController = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			if (dataType == DataType.Bool) {
				levelController.storeBoolValue (variableName, boolValue);
			}
			if (dataType == DataType.String) {
				levelController.storeStringValue (variableName, stringValue);
			}
			if (dataType == DataType.Int) {
				levelController.storeIntValue (variableName, intValue);
			}
			if (dataType == DataType.Float) {
				levelController.storeFloatValue (variableName, floatValue);
			}
		}
	}
}
