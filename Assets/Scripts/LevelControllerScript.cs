using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelControllerScript : WisdominiObject {

	/* references */

	public MasterControllerScript mcRef;
	public string BGMusic;
	public DialogueObject dialogueController;
	public alertImageController alertController;
	public UIFaderScript fader;
    public UIFaderScript[] fullScreenFaders;
	public VignetteScript vignette;
	public CameraUtils cameraGrab;
	public int floor;
	public Rosetta rosetta;
	DataStorage ds;
	public PlayerScript player;
	AudioSource aSource;

	/* properties */

	private GameObject plyr;
	FootstepSoundManager footstepMngr;


	/* public properties */

	public float CameraYAngleOverride = 0;
	public float CameraXAngleOverride = 0;
	public float CameraZDistOverride = 0;

	public string groundType = "";
	public string locationName;
	public string upperFloorName;
	public string lowerFloorName;
	//public int locationId;
	//public int worldId;
	public bool openVignette = true;

	/* constants */
	const int MaxFlames = 7;

	bool started = false;

	public void storeBoolValue(string key, bool val) {

		ds.storeBoolValue (key, val);

	}

	public void storeIntValue(string key, int val) {

		ds.storeIntValue (key, val);

	}

	public void storeStringValue(string key, string val) {
		
		ds.storeStringValue (key, val);

	}

	public void storeFloatValue(string key, float val) {

		ds.storeFloatValue (key, val);

	}

	public void _wm_storeReturnLocation() {

		ds.storeStringValue ("ReturnLocation", locationName);

	}

	public void blockPlayerControls() {
		player.blockControls ();
	}

	public void unblockPlayerControls() 
	{
		player.unblockControls ();
	}

	public int retrieveIntValue(string key) 
	{
		return ds.retrieveIntValue (key);
	}

	public string retrieveStringValue(string key) 
	{
		return ds.retrieveStringValue (key);
	}

	public float retrieveFloatValue(string key) 
	{
		return ds.retrieveFloatValue (key);
	}

	public bool retrieveBoolValue(string key) 
	{
		return ds.retrieveBoolValue (key);
	}

    public void storePlayerCoordinates()
    {
        storePlayerCoordinates(locationName);
    }

    public void storePlayerCoordinates(string locNam) 
	{
		ds.storeFloatValue ("Coords" + locNam + "X", plyr.transform.position.x);
		ds.storeFloatValue ("Coords" + locNam + "Y", plyr.transform.position.y);
		ds.storeFloatValue ("Coords" + locNam + "Z", plyr.transform.position.z);
        //		int orient = plyr.GetComponent<PlayerScript> ().orientation ();
        //		ds.storeIntValue ("Orientation", orient);
        if (plyr.GetComponent<PlayerScript>() != null)
        {
            ds.storeIntValue("Orientation", plyr.GetComponent<PlayerScript>().orientation());
        }
        FindObjectOfType<CameraManager>().StoreCameraAngles();
	}

	public void _wm_storePlayerCoordinates() 
	{
        Debug.Log("_wm_storePlayerCoordinates");
		storePlayerCoordinates ();
	}

    public void _wm_storePlayerCoordinatesOnLocation(string loc)
    {
        storePlayerCoordinates(loc);
    }

    public void footstep() 
	{
		footstepMngr.step ();
	}

	void Awake() 
	{
		locationName = SceneManager.GetActiveScene().name;
		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript>();
		ds = mcRef.getStorage ();

		rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();

		GameObject PlayerGO = GameObject.Find ("Player");
		if(PlayerGO != null) player = PlayerGO.GetComponent<PlayerScript> ();

		footstepMngr = GameObject.Find ("FootstepSoundManager").GetComponent<FootstepSoundManager> ();

		GameObject cam = GameObject.Find ("Main Camera");
		if (cam != null) {
			cameraGrab = cam.GetComponent<CameraUtils> ();
		}

		string savedSound = ds.retrieveStringValue ("GroundType");
		if (!savedSound.Equals ("")) {
			footstepMngr.setGroundType (savedSound);
		} else {
			if (!groundType.Equals (""))
				footstepMngr.setGroundType (groundType);
		}

		aSource = this.GetComponent<AudioSource> ();
	}

	public void playSound(string sound)
	{
		aSource.volume = 1.0f;
		aSource.PlayOneShot (Resources.Load<AudioClip> (sound));
	}

	public void playSound(AudioClip sound) 
	{
		aSource.volume = 1.0f;
		aSource.PlayOneShot (sound);
	}

	public void playSound(AudioClip sound, GameObject _go)
	{
		if (Mathf.Abs (Vector3.Distance (_go.transform.position, player.gameObject.transform.position)) < 30) {
			aSource.PlayOneShot (sound);
		}
	}

	public void playSound(AudioClip sound, Vector3 listenerPos, Vector3 sourcePos, float falloffRate) {

		float vol = 1.0f - (listenerPos - sourcePos).sqrMagnitude * falloffRate;
		if (vol < 0.0f)
			vol = 0.0f;
		aSource.volume = vol;
		aSource.PlayOneShot (sound);

	}

    bool FaderColorSet = false;
    void SetFaderColor()
    {
        if (FaderColorSet)
            return;
        FaderColorSet = true;
        if(ds.retrieveBoolValue("FadeWhite"))
        {
            fader.setFadeColor(1.0f, 1.0f, 1.0f);

            storeBoolValue("InhibitVignette", true);

        }
        else
        {
            fader.setFadeColor(0.0f, 0.0f, 0.0f);

        }

    }

    new public void Start () 
	{
		if (started)
			return;
		started = true;

		GameObject hud = GameObject.FindGameObjectWithTag ("HUD");
		if (hud != null)
		{
			dialogueController = hud.GetComponentInChildren<DialogueObject> ();
			alertController = hud.GetComponentInChildren<alertImageController> ();
			fader = hud.GetComponentInChildren<UIFaderScript> ();
            SetFaderColor();
			vignette = hud.GetComponentInChildren<VignetteScript> ();
		}

		Application.targetFrameRate = 30;

		mcRef.changeLocation (locationName);

		if (locationName.StartsWith("Level")) {
			Debug.Log ("<color=purple>Storing CurrentLevel : " + locationName.Substring (0, 6) + "</color>");
			ds.storeStringValue ("CurrentLevel", locationName.Substring (0, 6));
		}

		if (locationName.StartsWith ("Level")) {
			string lvl = locationName.Substring (5, 1);
			mcRef.getStorage ().storeBoolValue ("HasVisitedWorld" + lvl, true);
		}

		plyr = GameObject.Find ("Player");
		//GameObject hud = null;
		if(plyr != null) {
			hud = plyr.GetComponent<PlayerScript>().currentHUD;//GameObject.FindGameObjectWithTag ("HUD");
		}

		if (hud != null)
		{
			dialogueController = hud.GetComponentInChildren<DialogueObject> ();
			alertController = hud.GetComponentInChildren<alertImageController> ();
			fader = hud.GetComponentInChildren<UIFaderScript> ();
			vignette = hud.GetComponentInChildren<VignetteScript> ();
            SetFaderColor();
		}

		string objectToDestroy;


		// take care of flame re-spawning into the level
		int flameCount;
		for (int k = 0; k < MaxFlames; ++k) 
		{
			flameCount = mcRef.getStorage ().retrieveIntValue ("Flame" + k + "Resurrect" + locationName);
			if (flameCount > 0) {
				--flameCount;
				if (flameCount == 0) {
					string flamename = mcRef.getStorage ().retrieveStringValue ("Flame" + k + "Resurrect" + locationName);
					mcRef.unPickUpObject (flamename);
				}
				mcRef.getStorage ().storeIntValue ("Flame" + k + "Resurrect" + locationName, flameCount);
			}
		}

		/* destroy all objects that have already been picked up
		 * on this location
		 */
		objectToDestroy = mcRef.nextPickedUpObject ();
		while (!objectToDestroy.Equals ("")) {
			Destroy (GameObject.Find (objectToDestroy));
			objectToDestroy = mcRef.nextPickedUpObject ();
		}

		if (mcRef.hasCurrentLocationBeenVisited ()) 
		{
			//plyr.transform.position = mcRef.getPlayerSpawnLocation ();
		}

		if (openVignette) {
			//vignette.setVignetteScale (0.0f);
			if (vignette != null) {
				bool inhibit = retrieveBoolValue ("InhibitVignette");
				if (!inhibit) {
					vignette._wm_open ();
				} else {
                    if (ds.retrieveBoolValue("FadeWhite"))
                    {
                        ds.storeBoolValue("FadeWhite", false);
                        foreach (UIFaderScript fullScreenFader in fullScreenFaders)
                        {
                            fullScreenFader.setFadeColor(1.0f, 1.0f, 1.0f);
                            fullScreenFader.autoFadeIn = true;
                            fullScreenFader.setFadeValue(0.0f);
                            fullScreenFader.fadeIn();
                        }
                    }
                    else
                    {
                        fader.autoFadeIn = false;
                        fader.preventAutoFader();
                        fader.setFadeValue(1.0f);
                        foreach (UIFaderScript fullScreenFader in fullScreenFaders)
                        {
                            fullScreenFader.setFadeValue(1.0f);
                            fullScreenFader.autoFadeIn = false;
                        }
                    }
				}
				storeBoolValue ("InhibitVignette", false);
			}
		}

		if (alertController != null) {
			alertController._wm_reset ();
		}

		if (!BGMusic.Equals ("")) {
			mcRef.playMusic (BGMusic);
		}
			

	}

	public Vector3 getPlayerPosition() 
	{
		GameObject playerGO = GameObject.Find ("Player");
		if (playerGO != null) {
			return playerGO.transform.position;
		}
		return Vector3.zero;
	}

	public void grabFrame() 
	{
		if(cameraGrab != null)
		cameraGrab.grab ();
	}

	public void saveCameraState() {

		CameraManager camMan = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();

		float ry, rx;
		ry = camMan.pivotY.transform.localEulerAngles.y;
		rx = camMan.pivotX.transform.localEulerAngles.x;

		if (ry > 180.0f)
			ry -= 360.0f;

		if (rx > 180.0f)
			rx -= 360.0f;

		// pivots
		storeFloatValue ("Rot" + locationName + "Y", ry);
		storeFloatValue ("Rot" + locationName + "X", rx);
		storeFloatValue ("PosIn" + locationName + "Z", camMan.pivotZ.transform.localPosition.z);
		// location
		storeFloatValue ("Pos" + locationName + "X", camMan.gameObject.transform.position.x);
		storeFloatValue ("Pos" + locationName + "Y", camMan.gameObject.transform.position.y);
		storeFloatValue ("Pos" + locationName + "Z", camMan.gameObject.transform.position.z);
		Debug.Log ("<color=blue>Camera state save: P(" + camMan.gameObject.transform.position.x +
		"," + camMan.gameObject.transform.position.y + "," + camMan.gameObject.transform.position.z +
		")  R(" + camMan.pivotY.transform.localEulerAngles.y + "," + camMan.pivotX.transform.localEulerAngles.x +
		"," + camMan.pivotZ.transform.localPosition.z + ")</color>");


	}

	public void loadScene(string s) 
	{
		saveCameraState ();
		SceneManager.LoadScene (s);
	}

	public void _wm_registerPickedUpObject(string name) {
		registerPickedUpObject (name);
	}

	public void registerPickedUpObject(string name) 
	{
		mcRef.registerPickedUpObject (name);
	}
	
	new void Update () 
	{
	
	}

	public void _wm_levelExportedMethod() {

	}

	public void _wa_levelExportedAction(WisdominiObject waiter) {

	}

	public float _wm_returnGaugeLevel(int gauge, int typeOfGauge, int typeOfMeasure) {

		return 3.1416f;

	}



	public void _wm_alert(string msg) {

		alertController.Start ();
		alertController._wm_setAlertMessage (msg);
		alertController._wm_setSelfTimeout (2.0f + (float)msg.Length / 10.0f);

	}

	public void fadeout() {

		fader.fadeOut ();

	}

	public void dipMusic(float time) {
		mcRef.dipMusic (time);
	}

	public void musicFadeOut() {

		mcRef.setVolume (0.0f, 0.3f);

	}

	public void musicFadeOut(float time) {

		mcRef.setVolume (0.0f, 1.0f / time);

	}

	public void musicFadeIn() {

		mcRef.setVolume (1.0f, 0.3f);

	}

	public void musicFadeIn(float time) {

		mcRef.setVolume (1.0f, 1.0f / time);

	}

	public void fadeout(WisdominiObject waiter) {

		waiter.isWaitingForActionToComplete = true;
		fader._wa_fadeOut (waiter);

	}

    public void _wm_loadUndergroundLevel()
    {
        string currentLevel = retrieveStringValue("CurrentLevel");
        loadScene(currentLevel + "Plane-1_exterior");
    }

    public void _wm_loadGroundLevel()
    {
        string currentLevel = retrieveStringValue("CurrentLevel");
        string levelToLoad = currentLevel + "Plane0_exterior";
        Debug.Log("       >>>>>>> Attempting to load: " + levelToLoad);
        loadScene(levelToLoad);
    }

    public void _wm_loadHeavenLevel()
    {
        string currentLevel = retrieveStringValue("CurrentLevel");
        loadScene(currentLevel + "Plane+1_exterior");
    }

    //	public void storePhysicalCameraPosition() {
    ////		CameraFollowAux physicalCamera = GameObject.Find ("PhysicalCameraFollow").GetComponent<CameraFollowAux> ();
    ////		storeFloatValue ("PCX" + locationName, physicalCamera.transform.position.x);
    ////		storeFloatValue ("PCY" + locationName , physicalCamera.transform.position.y);
    ////		storeFloatValue ("PCZ" + locationName , physicalCamera.transform.position.z);
    //
    //	}
    //	public void storePhysicalCameraPosition(string otherLocation) {
    ////		CameraFollowAux physicalCamera = GameObject.Find ("PhysicalCameraFollow").GetComponent<CameraFollowAux> ();
    ////		storeFloatValue ("PCX" + otherLocation, physicalCamera.transform.position.x);
    ////		storeFloatValue ("PCY" + otherLocation , physicalCamera.transform.position.y);
    ////		storeFloatValue ("PCZ" + otherLocation , physicalCamera.transform.position.z);
    //
    //	}

    public void loadPhysicalCameraPosition() {
		CameraFollowAux physicalCamera = GameObject.Find ("PhysicalCameraFollow").GetComponent<CameraFollowAux> ();
		CameraManager cameraLerp = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		float x = retrieveFloatValue ("Coords" + locationName + "X");
		float y = retrieveFloatValue ("Coords" + locationName + "Y");
		float z = retrieveFloatValue ("Coords" + locationName + "Z");
		if ((x != 0) || (y != 0) || (z != 0)) {
			cameraLerp.transform.position = new Vector3 (x, y, z) + cameraLerp.offset;
			cameraLerp.destination = new Vector3 (x, y, z)  + cameraLerp.offset;
			physicalCamera.transform.position = cameraLerp.whereTheCameraShouldBe.transform.position;
		} else {
			cameraLerp.warpToOriginalPosition ();
			physicalCamera.transform.position = cameraLerp.whereTheCameraShouldBe.transform.position;
		}
	}
}
