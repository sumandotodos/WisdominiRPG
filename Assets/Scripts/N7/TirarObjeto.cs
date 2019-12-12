using UnityEngine;
using System.Collections;

public class TirarObjeto : MonoBehaviour {

	public string VariableNombre = "TirarObjeto";

	public GameObject objeto;

	public LevelControllerScript level;

	public Vector3 mover;

	public float tiempo;

	float contador;


	// Use this for initialization
	void Start () {

		level.storeBoolValue (VariableNombre, false); 
	
	}
	
	// Update is called once per frame
	void Update () {

		bool comprobar = true;

		if (comprobar) {
			

			if (level.retrieveBoolValue (VariableNombre) == true) {

				Debug.Log ("Tiro este objeto");


				contador = contador + 1 * Time.deltaTime;

				objeto.transform.position = objeto.transform.position + mover * Time.deltaTime;
		
				
			}
		}
		if (contador > tiempo) {

			comprobar = false;

			level.storeBoolValue (VariableNombre, false); 

		}

	
	}
}
