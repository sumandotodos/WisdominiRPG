using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HOVMode { show, hide };

public class HideOnVariable : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public int RefreshFrames = 6;
    int RemainingFrames;
    public string BoolVariable;
    MasterControllerScript mc;
    public HOVMode mode = HOVMode.show;

    // Start is called before the first frame update
    void Start()
    {
        RemainingFrames = Random.Range(0, RefreshFrames) + 1;
        mc = FindObjectOfType<MasterControllerScript>();
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        --RemainingFrames;
        if(RemainingFrames == 0)
        {
            RemainingFrames = RefreshFrames;
            Refresh();
        }
    }

    public void Refresh()
    {
        bool VariableTrue = mc.getStorage().retrieveBoolValue(BoolVariable);
        if (mode == HOVMode.show)
        {
            spriteRenderer.enabled = VariableTrue;
        }
        else
        {
            spriteRenderer.enabled = !VariableTrue;
        }
    }
}
