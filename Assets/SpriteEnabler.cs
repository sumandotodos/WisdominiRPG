using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEnabler : WisdominiObject
{

    LevelControllerScript level;
    public SpriteRenderer sr;
    public bool reentrant = true;

    // Start is called before the first frame update
    new void Start()
    {
        level = FindObjectOfType<LevelControllerScript>();
        if(reentrant)
        {
            bool en = level.retrieveBoolValue(this.name + "IsDisabled");
            if(!en)
            {
                _wm_enable();
            }
            else
            {
                _wm_disable();
            }
        }
    }

    public void _wm_enable()
    {
        sr.enabled = true;
        if(reentrant)
        {
            level.storeBoolValue(this.name + "IsDisabled", false);
        }
    }

    public void _wm_disable()
    {
        sr.enabled = false;
        if (reentrant)
        {
            level.storeBoolValue(this.name + "IsDisabled", true);
        }
    }

}
