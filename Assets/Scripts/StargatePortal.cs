using UnityEngine;
using System.Collections;

public struct StringValue
{

	string key;
	string value;

}

public class StargatePortal : WisdominiObject {
	
	public string targetWorld;

    public UIFaderScript fader;

    public bool cheat = false;
    public string VariableName;

	public LevelControllerScript level;

	public StringValue[] str;
	DataStorage ds;
	public string[] keys;
	public string[] svalues;
	public int[] ivalues;
	public float[] fvalues;
	public bool[] bvalues;

	int state;

	void Start() {
		
		state = -1;
        level = GameObject.Find("LevelController").GetComponent<LevelControllerScript>();
		ds = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ().getStorage ();
    }

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player")
		{
			RestartCoordinateLogger ();
			fader.setFadeColor (1, 1, 1);
			fader._wa_fadeOut (this);
			this.isWaitingForActionToComplete = true;
			state = 0;
		}
	}

	new void Update() {

		if (state == 0) {

			if (!isWaitingForActionToComplete) {

				level.storeBoolValue ("PlayerMustMaterialize", true);
				level.storeStringValue ("ReturnLocation", targetWorld);
				level.loadScene ("TransWorldTunnel");

			}
		}
	}

	void RestartCoordinateLogger()
	{
		string loc = level.locationName;
		ds.storeFloatValue ("Coords" + loc + "X", 0);
		ds.storeFloatValue ("Coords" + loc + "Y", 0);
		ds.storeFloatValue ("Coords" + loc + "Z", 0);
		ds.storeIntValue ("Orientation", 0);

		// Es necesario reiniciar lo inferior?
//		ds.storeFloatValue ("Pos" + loc + "X", cam.gameObject.transform.position.x);
//		ds.storeFloatValue ("Pos" + loc + "Y", cam.gameObject.transform.position.y);
//		ds.storeFloatValue ("Pos" + loc + "Z", cam.gameObject.transform.position.z);
//		ds.storeFloatValue ("Rot" + loc + "Y", cam.pivotY.transform.localEulerAngles.y);
//		ds.storeFloatValue ("Rot" + loc + "X", cam.pivotX.transform.localEulerAngles.x);
//		ds.storeFloatValue ("PosIn" + loc + "Z", cam.pivotZ.transform.localPosition.z);
//		//ds.storeFloatValue ("PosIn" + loc + "M", cam.mainCamera.transform.localPosition.z);
	}

}
