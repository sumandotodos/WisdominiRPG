using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbMaterialChanger : MonoBehaviour
{
    public PhysicMaterial FrictionMaterial;
    public float LotsOfFriction = 1.0f;
    public float LittleFriction = 0.25f;
    PlayerScript player;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.GetComponent<PlayerScript>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            player = null;
        }
    }

    private void Update()
    {
        if (player != null)
        {
            if(player.IsMoving())
            {
                FrictionMaterial.staticFriction = LittleFriction;
            }
            else
            {
                FrictionMaterial.staticFriction = LotsOfFriction;
            }
        }
    }

}
