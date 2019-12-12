using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MirrorReentryScript : WisdominiObject {

	public LevelControllerScript level;
	public string exteriorLocationName;
	public int mirrorIndex;
	public string mirrorName;

	public void _wm_setupReentry() {

		MasterControllerScript master = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		DataStorage ds = master.getStorage ();
		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		ds.storeStringValue ("ReturnLocation", level.locationName);
		ds.storeIntValue ("ActivateMirror", mirrorIndex);
		ds.storeStringValue ("ActivatedMirror", mirrorName);
		ds.storeStringValue ("ReentryCondition", "MirrorActivation");
        FindObjectOfType<CameraManager>().StoreCameraAngles();

		SceneManager.LoadScene (exteriorLocationName);
	}
}
