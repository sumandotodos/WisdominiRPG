using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform[] tp;
    public Vector3 GetLocation(int i)
    {
        return tp[i - 1].position;
    }
    public string GetLocationName(int i)
    {
        return tp[i - 1].gameObject.name;
    }
}
