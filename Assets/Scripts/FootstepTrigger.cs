using UnityEngine;
using System.Collections;

public class FootstepTrigger : WisdominiObject {

	FootstepSoundManager footsoundMgr;

	public string groundType = "";
	public string exitGroundType = "";
    LevelControllerScript lvl;
    const float RecoilTime = 0.15f;
    float RecoilRemain;

	bool enabled = true;

	// Use this for initialization
	void Start () {

        lvl = FindObjectOfType<LevelControllerScript>();
		footsoundMgr = GameObject.Find ("FootstepSoundManager").GetComponent<FootstepSoundManager>();
        RecoilRemain = RecoilTime;
		enabled = true;
	
	}

	public void _wm_setEnable(bool en) {
		enabled = en;
	}
	
	void OnTriggerEnter(Collider other) {

        if (RecoilRemain > 0.0f) return;
        if (!enabled)
			return;
		if (other.tag == "Player") {

            int FootstepLevel = lvl.retrieveIntValue("FootstepTriggerLevel");
            ++FootstepLevel;
            lvl.storeIntValue("FootstepTriggerLevel", FootstepLevel);
            Debug.Log("FSTLevel: " + FootstepLevel);
            if (!groundType.Equals ("")) {
                Debug.Log("Setting sound: " + groundType);
                footsoundMgr.setGroundType (groundType);
			}

		}

	}

	void OnTriggerExit(Collider other) {
    
		if (!enabled)
			return;
		if (other.tag == "Player") {
            int FootstepLevel = lvl.retrieveIntValue("FootstepTriggerLevel");
            FootstepLevel = FootstepLevel > 0 ? FootstepLevel-1 : 0;
            lvl.storeIntValue("FootstepTriggerLevel", FootstepLevel);
            Debug.Log("FSTLevel: " + FootstepLevel);
            if (FootstepLevel == 0 && !exitGroundType.Equals ("")) {
                Debug.Log("Setting sound: " + exitGroundType);
                footsoundMgr.setGroundType (exitGroundType);
			}
		}

	}

    private void Update()
    {
        if(RecoilRemain > 0.0f)
        {
            RecoilRemain -= Time.deltaTime;
        }
    }

}
