using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

enum WorldMapStatus { exitting, idle };

public class WorldMapController : WisdominiObject {

	/* references */

	int a;
	public Image[] level;
	MasterControllerScript mcRef;
	public UIFaderScript fader;


	/* properties */

	WorldMapStatus status;


	// Use this for initialization
	void Start () {


		status = WorldMapStatus.idle;
		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		DataStorage ds = mcRef.getStorage ();
		for (int i = 0; i < level.Length; ++i) {

			bool visited;
			visited = ds.retrieveBoolValue ("HasVisitedWorld" + (i+1));
			if (visited)
				level [i].enabled = true;
			else
				level [i].enabled = false;

		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (this.isWaitingForActionToComplete)
			return;

		if (status == WorldMapStatus.exitting) {

			// load level
			if (mcRef == null)
				return;

			DataStorage ds = mcRef.getStorage ();
			string returnLevel = ds.retrieveStringValue ("ReturnLocation");
			if (!returnLevel.Equals ("")) {
				SceneManager.LoadScene (returnLevel);
			}


		}

	}

	public void exit() {
		

			fader._wa_fadeOut (this);
			this.isWaitingForActionToComplete = true;
			status = WorldMapStatus.exitting;


	}
}
