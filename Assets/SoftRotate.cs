using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftRotate : WisdominiObject {

	SoftFloat angle;

	public float initialAngle;
	public float finalAngle;
	public FakePanelAxis axis;

	public bool reentrant = false;

	bool atFinalAngle = false;

    LevelControllerScript lvl;

	// Use this for initialization
	void Start () {
		angle = new SoftFloat (initialAngle);
		angle.setSpeed (30.0f);
		angle.setTransformation (TweenTransforms.cubicOut);
        lvl = FindObjectOfType<LevelControllerScript>();
        if(reentrant)
        {
            if(lvl.retrieveBoolValue(this.name+"Rotated"))
            {
                rotateImmediately();
            }
            else
            {
                unrotateImmediately();
            }
        }
    }

    void UpdateAngle()
    {
        switch (axis)
        {
            case FakePanelAxis.x:
                this.transform.localRotation = Quaternion.Euler(angle.getValue(), 0, 0);
                break;
            case FakePanelAxis.y:
                this.transform.localRotation = Quaternion.Euler(0, angle.getValue(), 0);
                break;
            case FakePanelAxis.z:
                this.transform.localRotation = Quaternion.Euler(0, 0, angle.getValue());
                break;
        }
    }

    // Update is called once per frame
    void Update () {
		if (angle.update ()) {
            UpdateAngle();
		}
	}

    public void rotateImmediately()
    {
        angle.setValueImmediate(finalAngle);
        atFinalAngle = true;
        UpdateAngle();
    }

    public void unrotateImmediately()
    {
        angle.setValueImmediate(initialAngle);
        atFinalAngle = false;
        UpdateAngle();
    }

    public void _wm_rotate() {
		angle.setValue (finalAngle);
		atFinalAngle = true;
        if(reentrant)
            lvl.storeBoolValue(this.name + "Rotated", true);
	}

	public void _wm_unrotate() {
		angle.setValue (initialAngle);
		atFinalAngle = false;
        if(reentrant)
            lvl.storeBoolValue(this.name + "Rotated", false);
    }

	public void _wm_toggle() {
		if (atFinalAngle)
			_wm_unrotate ();
		else
			_wm_rotate ();
	}
}
