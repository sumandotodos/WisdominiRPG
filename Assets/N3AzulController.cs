using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N3AzulController : WisdominiObject {

	public bool[] accomplished;
	public FakePanel fakePanel;
    public SetCameraFollowDirectionTime direction;
    public SetCameraInclinationTime inclination;
    public CameraSwitch cameraSwitch;
    PlayerScript player;
    LevelControllerScript lvl;
    public WisdominiObject AllDoneProgram;

	// Use this for initialization
	void Start () {
		accomplished = new bool[3];
        lvl = FindObjectOfType<LevelControllerScript>();
        for (int i = 0; i < 3; ++i)
        {
            accomplished[i] = lvl.retrieveBoolValue(this.name + "Accomplised"+i);
        }
        player = FindObjectOfType<PlayerScript>();
	}
	
	public void _wm_toggleAccomplishment(int position) {
		accomplished [position] = !accomplished [position];
        lvl.storeBoolValue(this.name + "Accomplised" + position, accomplished[position]);
        bool allDone = true;
		foreach (bool c in accomplished) {
			if (c == false) {
				allDone = false;
				break;
			}
		}
		if (allDone) {
            //fakePanel._wm_open ();
            //direction._wm_enable();
            //inclination._wm_enable();
            //player.blocked = true;
            //cameraSwitch._wm_switchToCameraName("L3VerdeSecretCamera");
            AllDoneProgram.startProgram(0);
		}
	}
}
