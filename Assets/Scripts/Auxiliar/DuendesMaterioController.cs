using UnityEngine;
using System.Collections;

public class DuendesMaterioController : MonoBehaviour {

	MasterControllerScript mc;

	public CharacterGenerator[] duende;
	const int Duende1_1 = 0;
	const int Duende1_2 = 1;
	const int Duende1_3 = 2;
	const int Duende2_1 = 3;
	const int Duende2_2 = 4;
	const int Duende3_1 = 5;
	const int Duende3_2 = 6;
	const int Duende4_1 = 7;
	const int Duende5_1 = 8;
	const int Duende5_2 = 9;
	const int Duende6_2 = 10;
	const int Duende7_1 = 11;

	int stage = 0;
	public string VarName = "N4DuendesMatEtapa";

	void Start() {
		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		stage = mc.getStorage ().retrieveIntValue (VarName);
		zapem ();
	}

	public void zapem() {
		switch (stage) {
		case -1:
		case 0:
			Destroy (duende [Duende1_1].gameObject);
			Destroy (duende [Duende1_2].gameObject);
			Destroy (duende [Duende1_3].gameObject);
			Destroy (duende [Duende2_1].gameObject);
			Destroy (duende [Duende2_2].gameObject);
			Destroy (duende [Duende3_1].gameObject);
			Destroy (duende [Duende3_2].gameObject);
			Destroy (duende [Duende4_1].gameObject);
			Destroy (duende [Duende5_1].gameObject);
			Destroy (duende [Duende5_2].gameObject);
			Destroy (duende [Duende6_2].gameObject);
			Destroy (duende [Duende7_1].gameObject);
			break;

		case 1:
			
			Destroy (duende [Duende1_2].gameObject);
			Destroy (duende [Duende1_3].gameObject);

			Destroy (duende [Duende2_2].gameObject);

			Destroy (duende [Duende3_2].gameObject);
			Destroy (duende [Duende4_1].gameObject);
			Destroy (duende [Duende5_1].gameObject);
			Destroy (duende [Duende5_2].gameObject);
			Destroy (duende [Duende6_2].gameObject);
			Destroy (duende [Duende7_1].gameObject);
			break;

		case 2:
			Destroy (duende [Duende1_1].gameObject);

			Destroy (duende [Duende1_3].gameObject);

			Destroy (duende [Duende2_2].gameObject);

			Destroy (duende [Duende3_2].gameObject);
			Destroy (duende [Duende4_1].gameObject);
			Destroy (duende [Duende5_1].gameObject);
			Destroy (duende [Duende5_2].gameObject);
			Destroy (duende [Duende6_2].gameObject);
			Destroy (duende [Duende7_1].gameObject);
			break;

		case 3:
			Destroy (duende [Duende1_1].gameObject);
			Destroy (duende [Duende1_2].gameObject);


			Destroy (duende [Duende2_2].gameObject);

			Destroy (duende [Duende3_2].gameObject);
			Destroy (duende [Duende4_1].gameObject);
			Destroy (duende [Duende5_1].gameObject);
			Destroy (duende [Duende5_2].gameObject);
			Destroy (duende [Duende6_2].gameObject);
			Destroy (duende [Duende7_1].gameObject);
			break;

		case 4:
			Destroy (duende [Duende1_1].gameObject);
			Destroy (duende [Duende1_2].gameObject);


			Destroy (duende [Duende2_2].gameObject);

			Destroy (duende [Duende3_2].gameObject);
			Destroy (duende [Duende4_1].gameObject);
			Destroy (duende [Duende5_1].gameObject);
			Destroy (duende [Duende5_2].gameObject);
			Destroy (duende [Duende6_2].gameObject);
			Destroy (duende [Duende7_1].gameObject);
			break;

		case 5:
			Destroy (duende [Duende1_1].gameObject);
			Destroy (duende [Duende1_2].gameObject);
			Destroy (duende [Duende1_3].gameObject);

			Destroy (duende [Duende2_1].gameObject);


			Destroy (duende [Duende4_1].gameObject);
			Destroy (duende [Duende5_1].gameObject);
			Destroy (duende [Duende5_2].gameObject);
			Destroy (duende [Duende6_2].gameObject);
			Destroy (duende [Duende7_1].gameObject);
			break;

		case 6:
			Destroy (duende [Duende1_1].gameObject);
			Destroy (duende [Duende1_2].gameObject);
			Destroy (duende [Duende1_3].gameObject);
			Destroy (duende [Duende2_1].gameObject);



			Destroy (duende [Duende4_1].gameObject);
			Destroy (duende [Duende5_1].gameObject);
			Destroy (duende [Duende5_2].gameObject);
			Destroy (duende [Duende6_2].gameObject);
			Destroy (duende [Duende7_1].gameObject);
			break;

		case 7:
			Destroy (duende [Duende1_1].gameObject);
			Destroy (duende [Duende1_2].gameObject);
			Destroy (duende [Duende1_3].gameObject);
			Destroy (duende [Duende2_1].gameObject);
			Destroy (duende [Duende2_2].gameObject);

			Destroy (duende [Duende5_1].gameObject);
			Destroy (duende [Duende5_2].gameObject);
			Destroy (duende [Duende6_2].gameObject);
			Destroy (duende [Duende7_1].gameObject);
			break;

		case 8:
			Destroy (duende [Duende1_1].gameObject);
			Destroy (duende [Duende1_2].gameObject);
			Destroy (duende [Duende1_3].gameObject);
			Destroy (duende [Duende2_1].gameObject);
			Destroy (duende [Duende2_2].gameObject);
			Destroy (duende [Duende3_1].gameObject);

			Destroy (duende [Duende5_2].gameObject);
			Destroy (duende [Duende6_2].gameObject);
			Destroy (duende [Duende7_1].gameObject);
			break;

		case 9:
			Destroy (duende [Duende1_1].gameObject);
			Destroy (duende [Duende1_2].gameObject);
			Destroy (duende [Duende1_3].gameObject);
			Destroy (duende [Duende2_1].gameObject);
			Destroy (duende [Duende2_2].gameObject);
			Destroy (duende [Duende3_1].gameObject);
			Destroy (duende [Duende3_2].gameObject);

			Destroy (duende [Duende5_2].gameObject);

			Destroy (duende [Duende7_1].gameObject);
			break;

		case 10: // para el 6.2
			Destroy (duende [Duende1_1].gameObject);
			Destroy (duende [Duende1_2].gameObject);
			Destroy (duende [Duende1_3].gameObject);
			Destroy (duende [Duende2_1].gameObject);
			Destroy (duende [Duende2_2].gameObject);
			Destroy (duende [Duende3_1].gameObject);
			Destroy (duende [Duende3_2].gameObject);

			Destroy (duende [Duende5_1].gameObject);

			Destroy (duende [Duende7_1].gameObject);
			break;

		case 11: // para el 7.1
			Destroy (duende [Duende1_1].gameObject);
			Destroy (duende [Duende1_2].gameObject);
			Destroy (duende [Duende1_3].gameObject);
			Destroy (duende [Duende2_1].gameObject);
			Destroy (duende [Duende2_2].gameObject);
			Destroy (duende [Duende3_1].gameObject);
			Destroy (duende [Duende3_2].gameObject);

			Destroy (duende [Duende5_1].gameObject);
			Destroy (duende [Duende5_2].gameObject);
		
			break;

		case 12:
			Destroy (duende [Duende1_1].gameObject);
			Destroy (duende [Duende1_2].gameObject);
			Destroy (duende [Duende1_3].gameObject);
			Destroy (duende [Duende2_1].gameObject);
			Destroy (duende [Duende2_2].gameObject);
			Destroy (duende [Duende3_1].gameObject);
			Destroy (duende [Duende3_2].gameObject);

			Destroy (duende [Duende5_1].gameObject);
			Destroy (duende [Duende5_2].gameObject);
			break;

//		case 13:
//			Destroy (duende [Duende1_1].gameObject);
//			Destroy (duende [Duende1_2].gameObject);
//			Destroy (duende [Duende1_3].gameObject);
//			Destroy (duende [Duende2_1].gameObject);
//			Destroy (duende [Duende2_2].gameObject);
//			Destroy (duende [Duende3_1].gameObject);
//			Destroy (duende [Duende3_2].gameObject);
//			Destroy (duende [Duende4_1].gameObject);
//			Destroy (duende [Duende5_1].gameObject);
//			Destroy (duende [Duende5_2].gameObject);
//			Destroy (duende [Duende6_2].gameObject);
//
//			break;


		}
	}
	
}
