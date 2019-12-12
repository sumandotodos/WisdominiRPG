using UnityEngine;
using System.Collections;

public class ValuesScale : MonoBehaviour {

	/* references */

	public Rosetta rosetta;
	public MasterControllerScript mcRef;


	/* public properties */

	public string scaleName;



	/* to be passed onto Rosetta */

	public string Valor1;
	public string Valor2;
	public string Valor3;
	public string Valor4;
	public string Valor5;

	string[] valor;
	int[] order;


	/* constants */

	const int MAXVALUES = 5;


	// Use this for initialization
	void Start () {

		valor = new string[MAXVALUES];
		/* load contents from Rosetta */
		for (int i = 0; i < MAXVALUES; ++i) {
			valor [i] = rosetta.retrieveString ("ValuesScale" + scaleName + i);
		}

		DataStorage ds = mcRef.getStorage ();
		bool init;
		init = ds.retrieveBoolValue ("ValueScale" + scaleName + "Init");


		if (init) { // order is extracted from MasterController

			for (int i = 0; i < MAXVALUES; ++i) {
				order [i] = ds.retrieveIntValue ("ValueScale" + scaleName + i);
			}

		} else { // need to initialize
			order = new int[MAXVALUES];
			for (int i = 0; i < MAXVALUES; ++i) {
				order [i] = i;
				ds.storeIntValue ("ValueScale" + scaleName + i, i);
			}
		}
	
	}


	public void interchange(int i, int j) {

		int temp;
		temp = order [i];
		order [i] = order [j];
		order [j] = temp;



	}

	public void store() {

		DataStorage ds = mcRef.getStorage ();
		for (int i = 0; i < MAXVALUES; ++i) {
			ds.storeIntValue ("ValueScale" + scaleName + i, order[i]);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
