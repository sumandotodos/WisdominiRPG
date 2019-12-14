using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDistanceFader : MonoBehaviour
{
    public Material BaseMaterial;
    public Material OpacityMaterial;
    public Camera cam_A;
    public float minDistance = 4.6f;
    public float maxDistance = 8.0f;
    MeshRenderer mRenderer;
    public int state;
    const int Transparent = 0;
    const int Opaque = 2;
    const int Translucent = 1;
    // Start is called before the first frame update
    void Start()
    {
        cam_A = FindObjectOfType<Camera>();
        mRenderer = GetComponent<MeshRenderer>();
        mRenderer.material = BaseMaterial;
        state = Opaque;
    }

    // Update is called once per frame
    void Update()
    {
        float zDist = (this.transform.position.z - cam_A.transform.position.z);
        float op = (zDist - minDistance) / (maxDistance - minDistance);
        Debug.Log("Dist: " + zDist + ", op: " + op);
        if(op > 1.0f && state != Opaque)
        {
            mRenderer.material = BaseMaterial;
            mRenderer.enabled = true;
            state = Opaque;
        }
        else if(op < 0.0f && state != Transparent)
        {
            mRenderer.enabled = false;
            state = Transparent;
        }
        else if((op<1.0f) && (op > 0.0f) && (state != Translucent))
        {
            mRenderer.material = OpacityMaterial;
            mRenderer.material.mainTexture = BaseMaterial.mainTexture;
            mRenderer.material.SetColor("_TintColor", new Color(1, 1, 1, op));
            state = Translucent;
        }
        else if((op < 1.0f) && (op > 0.0f) && (state == Translucent))
        {
            mRenderer.material.SetFloat("_Opacity", op);
        }
    }
}
