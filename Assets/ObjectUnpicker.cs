using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUnpicker : MonoBehaviour
{
    public string ObjectToUnpick;
    MasterControllerScript mc;
    // Start is called before the first frame update
    void Start()
    {
        mc = FindObjectOfType<MasterControllerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        mc.unPickUpObject(ObjectToUnpick);
    }
}
