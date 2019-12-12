using UnityEngine;
using System.Collections;

public class ItemElevator : WisdominiObject {

	public float topY = -37;
	public float bottomY = 200;
	public float speed = 6.0f;
	public float Y, targetY;
	public string reentryVariable;
	public AudioClip elevateClip;
	public AudioClip depressClip;
	public LevelControllerScript level;

	Vector3 originalPos;

	void Start () 
	{
        _wm_initialize();
	}

    public void _wm_initialize()
    {
        originalPos = this.transform.position;
        Vector3 pos = originalPos;
        Y = targetY = pos.y = bottomY;
        this.transform.position = pos;

        level = FindObjectOfType<LevelControllerScript>();
        bool b = level.retrieveBoolValue(reentryVariable);
        Debug.Log("Must raise signs again: " + b);
        if (b)
        {
            if (!level.retrieveBoolValue("HasAlphabet"))
            {
                immediateElevate();
            }
        }
    }

    new void Update () 
	{
		if (Y < targetY) {
			Y += speed * Time.deltaTime;
			if (Y > targetY) {
				Y = targetY;
			}
			Vector3 pos = originalPos;
			pos.y = Y;
			this.transform.position = pos;
		}

		if (Y > targetY) {
			Y -= speed * Time.deltaTime;
			if (Y < targetY) {
				Y = targetY;
			}
			Vector3 pos = originalPos;
			pos.y = Y;
			this.transform.position = pos;
		}
	}

	public void _wm_elevate() {

		targetY = topY;
		if ((level != null) && (elevateClip != null)) 
		{
			level.playSound (elevateClip);
		}
		if(level!=null)
		level.storeBoolValue (reentryVariable, true);

	}

	public void _wm_sink() 
	{
		targetY = bottomY;
	}

    public void _wm_immediateElevate()
    {
        immediateElevate();
    }

    public void immediateElevate() 
	{
		Y = targetY = topY;
		Vector3 pos = originalPos;
		pos.y = Y;
		this.transform.position = pos;
	}
}
