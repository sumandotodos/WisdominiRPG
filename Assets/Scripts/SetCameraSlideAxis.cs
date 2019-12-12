using UnityEngine;
using System.Collections;

public enum EnumFreeAxis { none, x, y, z };

public class SetCameraSlideAxis : WisdominiObject {

	public EnumFreeAxis axis;
	public bool snapToFace;
	public bool disableCollisions;
	LevelControllerScript level;

	public TriggerMode mode = TriggerMode.cameraAndPlayer;

	/* references */
	CameraManager cam;
	BoxCollider col;
	Vector3 center;

	public bool cameraIn;
	public bool playerIn;

	public bool enabled = true;
	public bool reentrant = false;

	void Start () 
	{
		cam = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		col = this.gameObject.GetComponent<BoxCollider> ();
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		cameraIn = false;
		playerIn = false;

		if (reentrant) {
			enabled = level.retrieveBoolValue ("is" + this.name + "Enabled");
		}
	}

	void OnTriggerEnter(Collider other) 
	{
        Debug.Log("<color=blue>SetCameraSlideAxis</color>");
		if (!enabled)
			return;
		if ((other.tag != "Player") && (other.tag != "MainCamera"))
			return;
		if (other.tag == "Player" || (other.tag == "MainCamera" && mode == TriggerMode.cameraAndPlayer)) 
		{			
			if (other.tag == "MainCamera") {
				cameraIn = true;
			}
			if (other.tag == "Player") {
				playerIn = true;
			}
			FreeAxis (axis);
			Debug.Log ("Lock axis");
			if (disableCollisions)
				cam.setCollisionsEnabled (false);
		}
	}

	void OnTriggerExit(Collider other) 
	{
		if (!enabled)
			return;
		if ((other.tag != "Player") && (other.tag != "MainCamera"))
			return;
		if(other.tag == "Player") 
			playerIn = false;
		if (other.tag == "MainCamera")
			cameraIn = false;
		if(((!playerIn) && (!cameraIn) && mode == TriggerMode.cameraAndPlayer) || ((!playerIn) && mode == TriggerMode.onlyPlayer))
		{			
			cam.UnBlockAllAxis();
			Debug.Log ("Unlock axis (" + other.name + ")");
			cam.setCollisionsEnabled (true);
		}
	}

	void FreeAxis(EnumFreeAxis _axis)
	{
		if (!snapToFace) 
		{
			cam.Block2Axis (_axis);
		} 

		else 
		{
			cam.Block2Axis (_axis, col.center.x + transform.position.x, col.center.y + transform.position.y, col.center.z + transform.position.z);
		}
	}

	public void _wm_enable() 
	{
		if (reentrant) {
			level.storeBoolValue ("is" + this.name + "Enabled", true);
		}
		enabled = true;
	}

	public void _wm_disable() 
	{
		if (reentrant) {
			level.storeBoolValue ("is" + this.name + "Enabled", false);
		}
		enabled = false;
	}
}
