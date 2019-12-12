using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StargateInfo : MonoBehaviour
{

    public ParticleSystem particles;
    public GameObject portal;
    public GameObject lights;
    public GameObject swirl;

    public void SetActivated(bool act)
    {
        swirl.gameObject.SetActive(act);
        portal.gameObject.SetActive(act);
        lights.gameObject.SetActive(act);
        if (act)
        {
            particles.Play();
        }
        else
        {
            particles.Stop();
        }
    }

}
