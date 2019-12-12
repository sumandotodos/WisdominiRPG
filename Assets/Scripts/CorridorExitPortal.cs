using UnityEngine;
using System.Collections;

public class CorridorExitPortal : WisdominiObject {

	public Texture whiteTex;
	public UIFaderScript fader;
	public LevelControllerScript level;
	//public CameraFrameGrab cameraGrab;
	public RenderTexture rt;
	bool exit;
	int framesElapsed = 0;

	new void Start () 
	{
		exit = false;
		if (fader == null)
			fader = GameObject.Find ("Fader").GetComponent<UIFaderScript> ();
	}
	
	new void Update () 
	{
		if (exit == false)
			return;
		if (isWaitingForActionToComplete)
			return;

		if (framesElapsed == 0) {
			//cameraGrab.grab ();
			Graphics.Blit(whiteTex, rt);
		}
		framesElapsed++;
		if (framesElapsed == 3) {
			string returnLocation = level.retrieveStringValue ("ReturnLocation");
			level.loadScene (returnLocation);
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Player") 
		{
			fader.setFadeColor (1, 1, 1);
			fader._wa_fadeOut (this);
			this.isWaitingForActionToComplete = true;
			level.storeBoolValue ("IsChangingPlanes", true);
			exit = true;
		}
	}
}
