using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackSignLocation : WisdominiObject
{
    MasterControllerScript mc;
    // Start is called before the first frame update
    void Start()
    {
        mc = FindObjectOfType<MasterControllerScript>();
        Vector3 storedPos = new Vector3(
            mc.getStorage().retrieveFloatValue("GoBackSignsX"),
            mc.getStorage().retrieveFloatValue("GoBackSignsY"),
            mc.getStorage().retrieveFloatValue("GoBackSignsZ"));
        if(storedPos.magnitude > 0.01f)
        {
            this.transform.position = storedPos;
        }
    }

    public void _wm_updatePos(string nameOfObject)
    {
        GameObject obj = GameObject.Find(nameOfObject);
        this.transform.position = obj.transform.position - new Vector3(0, 0, 3);
        mc.getStorage().storeFloatValue("GoBackSignsX", this.transform.position.x);
        mc.getStorage().storeFloatValue("GoBackSignsY", this.transform.position.y);
        mc.getStorage().storeFloatValue("GoBackSignsZ", this.transform.position.z);
    }
}
