using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillIfGreaterThanZero : MonoBehaviour
{
    public string VarName;

    // Start is called before the first frame update
    void Start()
    {
        LevelControllerScript level = FindObjectOfType<LevelControllerScript>();
        if(level!=null)
        {
            int value = level.retrieveIntValue(VarName);
            if(value > 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

}
