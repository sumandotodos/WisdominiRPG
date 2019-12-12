using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour
{
    public Camera Camera_A;

    public float Height = 0.0f;

    Vector3 InitialLocalPosition;
    public Transform ParentLocation;

    private void Awake()
    {
        if(Camera_A==null)
        {
            Camera_A = FindObjectOfType<Camera>();
        }
        if(ParentLocation == null)
        {
            ParentLocation = this.transform;
        }
        InitialLocalPosition = this.transform.localPosition;
    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        transform.LookAt(transform.position + Camera_A.transform.rotation * Vector3.forward,
            Camera_A.transform.rotation * Vector3.up);

        if (Height > 0.0f)
        {
            this.transform.position = InitialLocalPosition + ParentLocation.position + (Camera_A.transform.position - (ParentLocation.position+ InitialLocalPosition)).normalized * Height;
            if(Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("<color=red>" + Camera_A.transform.position + "</color>");
                Debug.Log("<color=blue>" + ParentLocation.position + "</color>");
                Debug.Log("<color=green>" + (Camera_A.transform.position - ParentLocation.position).normalized + "</color>");
            }
        }

    }
}
