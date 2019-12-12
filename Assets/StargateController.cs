using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StargateController : MonoBehaviour
{
    public StargateInfo[] stargates;
    MasterControllerScript mc;

    // Start is called before the first frame update
    void Start()
    {
        mc = FindObjectOfType<MasterControllerScript>();    
        for(int i = 1; i <= stargates.Length; ++i)
        {
            string key = "Level" + i + "Plane0_exterior" + "StargateActivated";
            if (mc.getStorage().retrieveBoolValue(key))
            {
                Debug.Log("Stargate " + i + " is activated");
                stargates[i - 1].SetActivated(true);
            }
            else
            {
                Debug.Log("Stargate " + i + " is NOT activated");
                stargates[i - 1].SetActivated(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
