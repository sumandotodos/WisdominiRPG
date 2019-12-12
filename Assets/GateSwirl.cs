using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GateSwirl : MonoBehaviour
{
    float op;
    public float opSpeed = 1.0f;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        op = 0.0f;
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(op < 1.0f)
        {
            op += opSpeed * Time.deltaTime;
            sr.color = new Color(1, 1, 1, op);
        }
    }
}
