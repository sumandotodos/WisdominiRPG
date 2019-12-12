using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class iTweenTest : MonoBehaviour
{
    public float Time;
    public GameObject Dest;
    public Text dt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //iTween.MoveUpdate(this.gameObject, iTween.Hash("position", Dest.transform.position, "time", 8.0f));
        iTween.MoveUpdate(this.gameObject, iTween.Hash("position", Dest.transform.position, "lookTarget", Dest, "time", Time));
        if(dt!=null)
        {
            dt.text = "shiiiit";
        }
    }
}
