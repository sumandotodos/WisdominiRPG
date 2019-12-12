using UnityEngine;
using System.Collections;

public class Drawer : Interactor {

	float targetZ, z;
	const float zSpeed = 12.0f;
	const float closedZ = 0.1f;
	const float openZ = 1.35f;
	bool isOpen;
	bool isFull;

	public AudioClip openSound;
	public AudioClip closeSound;

	public Interactor contents;

	MasterControllerScript mc;
	DataStorage ds;
	LevelControllerScript lvl;

	new void Start ()
	{	
		targetZ = z = closedZ;
		isOpen = false;
		if (contents != null)
			isFull = true;

		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = mc.getStorage ();
		lvl = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		if (ds.retrieveBoolValue ("Is" + this.name + lvl.locationName + "Empty")) 
		{
			isFull = false;
		}
	}

	public override string interactIcon()
	{
		return "Hand";
	}
	
	new void Update () 
	{	
		if (z < targetZ) {
			z += zSpeed * Time.deltaTime;
			if (z > targetZ) {
				z = targetZ;
				notifyFinishAction ();
			}
			Vector3 localPos = this.transform.localPosition;
			localPos.z = z;
			this.transform.localPosition = localPos;
		}

		if (z > targetZ) {
			z -= zSpeed * Time.deltaTime;
			if (z < targetZ) {
				z = targetZ;
				notifyFinishAction ();
			}
			Vector3 localPos = this.transform.localPosition;
			localPos.z = z;
			this.transform.localPosition = localPos;
		}
	}

	public void openDrawer() 
	{
		targetZ = openZ;
		isOpen = true;
		if ((lvl != null) && (openSound != null)) {
			lvl.playSound (openSound);
		}
	}

	public void closeDrawer() 
	{
		targetZ = closedZ;
		isOpen = false;
		if ((lvl != null) && (closeSound != null)) {
			lvl.playSound (closeSound);
		}
	}

	override public void effect()
	{		
		if (!isOpen) 
		{
			openDrawer ();
		} 
		else 
		{
			if (isFull) 
			{
				contents.startProgram (contents.programIndexFromEventName ("onInteract"));
				ds.storeBoolValue ("Is" + this.name + lvl.locationName + "Empty", true);
				isFull = false;
			} else {
				closeDrawer ();
			}
		}
	}
}
