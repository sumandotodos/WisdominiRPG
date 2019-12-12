using UnityEngine;
using System.Collections;

public enum FakePanelAxis { x, y, z };

public class FakePanel : Interactor {

	MasterControllerScript master;
	DataStorage ds;
	LevelControllerScript level;
	public AudioClip dragSound;
	public FakePanelAxis axis = FakePanelAxis.x;


	public float xSpeed = 6.0f;

	public string PanelID;

	float targetX;
	float X;

	public float displacedX = -40.0f;

	Vector3 originalPos;

	// Use this for initialization
	new void Start () {

		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		master = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = master.getStorage ();
	
		originalPos = this.transform.position;

		bool isOpen = ds.retrieveBoolValue (PanelID + "isOpen");
		if (isOpen) {
			X = targetX = displacedX;
			switch(axis) {
			case FakePanelAxis.x:
				this.transform.position = new Vector3 (originalPos.x + X, originalPos.y, originalPos.z);
				break;
			case FakePanelAxis.y:
				this.transform.position = new Vector3 (originalPos.x, originalPos.y + X, originalPos.z);
				break;
			case FakePanelAxis.z:
				this.transform.position = new Vector3 (originalPos.x, originalPos.y, originalPos.z + X);
				break;
			}
		} else {
			
			X = targetX = 0.0f;

		}

	}

	override public string interactIcon() {
		return "Hand";
	}

	public void _wm_close() {
		targetX = 0;
		if ((level != null) && (dragSound != null)) {
			//level.playSound (dragSound);
			level.playSound(dragSound, level.player.transform.position, this.transform.position, 0.00025f);

		}
	}

	override public void effect() {

		targetX = displacedX;
		if ((level != null) && (dragSound != null))
			//level.playSound (dragSound);
			level.playSound(dragSound, level.player.transform.position, this.transform.position, 0.00025f);
		ds.storeBoolValue (PanelID + "isOpen", true);

	}
	
	// Update is called once per frame
	new void Update () {

		if (X > targetX) {

			X -= xSpeed * Time.deltaTime;
			switch(axis) {
			case FakePanelAxis.x:
				this.transform.position = new Vector3 (originalPos.x + X, originalPos.y, originalPos.z);
				break;
			case FakePanelAxis.y:
				this.transform.position = new Vector3 (originalPos.x, originalPos.y + X, originalPos.z);
				break;
			case FakePanelAxis.z:
				this.transform.position = new Vector3 (originalPos.x, originalPos.y, originalPos.z + X);
				break;
			}
			if (X < targetX)
				X = targetX;

		}

		if (X < targetX) {

			X += xSpeed * Time.deltaTime;
			switch(axis) {
			case FakePanelAxis.x:
				this.transform.position = new Vector3 (originalPos.x + X, originalPos.y, originalPos.z);
				break;
			case FakePanelAxis.y:
				this.transform.position = new Vector3 (originalPos.x, originalPos.y + X, originalPos.z);
				break;
			case FakePanelAxis.z:
				this.transform.position = new Vector3 (originalPos.x, originalPos.y, originalPos.z + X);
				break;
			}
			if (X > targetX)
				X = targetX;

		}
	
	}

	public void _wm_open() {


		effect ();

	}
}
