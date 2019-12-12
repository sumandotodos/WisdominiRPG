using UnityEngine;
using System.Collections;

public class MatrixCorridorController : MonoBehaviour {

	public LevelControllerScript level;
	public StringBankCollection stringBanks;

	public TextMesh[] texts;
	Camera cam;

	Rosetta ros;

	void Start () 
	{
		ros = GameObject.Find ("Rosetta").GetComponent<Rosetta>();
		cam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		cam.farClipPlane = 300000;

		StringBank bank = stringBanks.bank[0];
		bank.reset ();
		bank.rosetta = ros;
		for (int i = 0; i < 7; ++i) {
			string sId = bank.getNextString ();
			//texts [i].text = StringUtils.chopLines (sId, 35);
			texts[i].GetComponent<ThoughtText>().setText(StringUtils.chopLines (sId, 35));
		}	
	}
	
	void Update ()
	{
	
	}
}
