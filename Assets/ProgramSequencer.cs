using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramSequencer : Interactor
{
    LevelControllerScript level;
    public string InteractIconName;
    public WisdominiObject[] Programs;
    int state = 0;

    private void Start()
    {
        level = FindObjectOfType<LevelControllerScript>();
        state = level.retrieveIntValue("Seq" + this.name + "State");
    }

    public override string interactIcon()
    {
        return InteractIconName;
    }

    public override void effect()
    {
        Programs[state].startProgram(0);
        state = (state + 1) % Programs.Length;
        level.storeIntValue("Seq" + this.name + "State", state);
    }
}
