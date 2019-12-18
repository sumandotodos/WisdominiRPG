using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public enum GameState { CheckVersion, NewUser, LoadingTitle, Title, Video, MainGame, SkippingToLevel, Idle }; 

[System.Serializable]
public class StartingLevel
{
    public string LevelName;
    public bool StartHere = false;
}

public class MasterControllerScript : WisdominiObject {

	public bool EnableMusic;
    public bool TestFadeWhite = false;

    public List<IntermediateTarget> intermediateTarget;

    bool levelCheckPoint;

	public int N3FerHechas;
	public int N3FerCorrectas;

	public bool N3LiftComing;
	public bool N3InGround;

	public Scene fasePrueba;

    public StartingLevel[] startingLevels;

	/* pseudo-constants */

	int numberOfMixers;

	/* properties */

	float[] musicVolume;
	float[] musicMaxVolume;
	float[] musicTargetVolume;
	float[] musicVolumeFadeSpeed;
	int currentMixer = 0;

	float dipRemainingTime = 0;

	AudioSource[] aSources;

	public AudioClip[] musicClip;

	public string locale = "es";

	new Rosetta rosetta;

	string savedLocation;
	public string currentLocation;
	//string locationName;

	List<string> sentencePiece;

	/* Gamestate data that will be saved to disk as savegame */

	public List<PickedUpObjectsList> pickedUpObjectDB; 
    int currentPickedUpListIndex;

	DataStorage _storage;
	public GameState gameState;

	/* end of section */

	const string firstLevel = "Title";

	bool waitForActivityToFinish;

	public string firstSceneName;


	CameraManager cam;


	public void loadGame(bool restartLevel, int slot = 0) {

		string saveFile = "/save00"+slot+".dat";
        Debug.Log(" $$$$$$$$$$$$$$$$$$ loading file: " + saveFile + " $$$$$$$$$$$$ ");
		if (restartLevel)
			saveFile = "/save000.cp.dat";

		if (File.Exists (Application.persistentDataPath + saveFile)) {

			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + saveFile, FileMode.Open);
			SaveGameData data = (SaveGameData) formatter.Deserialize (file);
			file.Close ();

			savedLocation = data.locationName;
			
			/* rebuild storage from serialized data */
			pickedUpObjectDB = data.pickedUpObjectDB;
			currentPickedUpListIndex = data.currentPickedUpListIndex;

			SerializableDictionary dict = data.dict;
			_storage = new DataStorage ();
			_storage.initialize ();
			for (int i = 0; i < dict.bKey.Count; ++i) {
				_storage.storeBoolValue(dict.bKey[i], dict.bValue[i]);
			}
			for (int i = 0; i < dict.fKey.Count; ++i) {
				_storage.storeFloatValue(dict.fKey[i], dict.fValue[i]);
			}
			for (int i = 0; i < dict.iKey.Count; ++i) {
				_storage.storeIntValue(dict.iKey[i], dict.iValue[i]);
			}
			for (int i = 0; i < dict.sKey.Count; ++i) {
				_storage.storeStringValue(dict.sKey[i], dict.sValue[i]);
			}



		} else { // new game

			_storage.storeBoolValue ("PlayerMustMaterialize", true);

		}

	}

	public void deleteSaveGame() {

		//FileUtil.DeleteFileOrDirectory (Application.persistentDataPath + "/save000.dat");

	}

	public void saveGame(bool levelCheckPoint, int slot = 0) {

		string saveFile = "/save00"+slot+".dat";
		if (levelCheckPoint)
			saveFile = "/save000.cp.dat";

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + saveFile, FileMode.Create);

		SaveGameData data = new SaveGameData ();
		//data.conversationList = conversationList;
		/*data.currentPickedUpList = currentPickedUpList;
		data.gameState = gameState;
		data.pickedUpObjectDB = pickedUpObjectDB;*/
		data.dict = _storage.serial;
		/*
		data.playerPosition = new SerializableVector3 (Vector3.zero);*/
		data.locationName = "";
		data.pickedUpObjectDB = pickedUpObjectDB;
		data.currentPickedUpListIndex = currentPickedUpListIndex;

		GameObject levelGO = GameObject.Find ("LevelController");
		LevelControllerScript level;
		if (levelGO != null) {
			level = levelGO.GetComponent<LevelControllerScript> ();
			data.locationName = level.locationName;
			//data.playerPosition = new SerializableVector3(level.getPlayerPosition ());
		}

		formatter.Serialize (file, data);
		file.Close ();
	}

	string currentMusic = "";

    private string GetStartingLevel()
    {
        string ForcedLevel = PlayerPrefs.GetString("ForcedLevel");
        if(ForcedLevel != "")
        {
            return ForcedLevel;
        }
        for (int i = 0; i < startingLevels.Length; ++i)
        {
            if (startingLevels[i].StartHere)
            {
                return startingLevels[i].LevelName;
            }
        }
        return "";
    }

    new void Start () {

		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Screen.autorotateToPortrait = false;
		Application.targetFrameRate = 30;
	
		_storage = new DataStorage ();
		_storage.initialize ();

		DontDestroyOnLoad (this);
		
		pickedUpObjectDB = new List<PickedUpObjectsList> ();
		currentPickedUpListIndex = -1;


		waitForActivityToFinish = false;

        if (GetStartingLevel() == "")
        {
            gameState = GameState.Title;
            int slot = PlayerPrefs.GetInt("LoadSlot");
            PlayerPrefs.SetInt("LoadSlot", 0);
            loadGame(false, slot);
        }
        else
        {
            gameState = GameState.SkippingToLevel;
        }

        _storage.storeBoolValue ("IsLevel1MusicPlaying", false);
		_storage.storeBoolValue ("IsLevel2MusicPlaying", false);
		_storage.storeBoolValue ("IsLevel3MusicPlaying", false);
		_storage.storeBoolValue ("IsLevel4MusicPlaying", false);
		_storage.storeBoolValue ("IsLevel5MusicPlaying", false);

		aSources = this.GetComponents<AudioSource> ();
		numberOfMixers = aSources.Length;
		musicVolume = new float[numberOfMixers];
		musicMaxVolume = new float[numberOfMixers];
		musicTargetVolume = new float[numberOfMixers];
		musicVolumeFadeSpeed = new float[numberOfMixers];
		currentMixer = 0;

		for (int i = 0; i < numberOfMixers; ++i) {
			musicVolume [i] = 1.0f;
			musicMaxVolume [i] = 1.0f;
			musicTargetVolume [i] = 1.0f;
			musicVolumeFadeSpeed [i] = 0.3f;
		}

		if (getStorage ().retrieveStringValue ("ReentryCondition") == "Inventory") 
		{
			Debug.Log (getStorage ().retrieveStringValue ("ReentryCondition"));
			getStorage ().storeStringValue ("ReentryCondition", "CloseInventory");
		}
	}

	public void nextMixer() {
		currentMixer = (currentMixer + 1) % numberOfMixers;
	}

	public void selectMixer(int m) 
	{
		m = m % numberOfMixers;
		currentMixer = m;
	}

	public void stopMusic() 
	{
		//aSources [currentMixer].Stop ();
		musicTargetVolume[currentMixer] = 0.0f;
		nextMixer ();
	}

	private void playMusic(int index) 
	{
		if (!EnableMusic)
			return;
		if((index < musicClip.Length) && musicClip[index] != null) {
			aSources [currentMixer].clip = musicClip [index];
			aSources [currentMixer].volume = 1.0f;
			musicTargetVolume [currentMixer] = 1.0f;
			aSources [currentMixer].loop = true;
			aSources [currentMixer].mute = false;
			playMusic (); 
		}
	}

	public void playMusic(string name) {
		if (!EnableMusic)
			return;
		for (int i = 0; i < musicClip.Length; ++i) {
			if (musicClip [i].name.Equals (name) && (!currentMusic.Equals(name))) {
				stopMusic ();
				currentMusic = name;
				playMusic (i);
			}
		}
	}

	public void attachMusic(AudioClip clip)
	{
		aSources [currentMixer].clip = clip;
	}

	public void attachMusic(string resName) 
	{
		aSources [currentMixer].clip = Resources.Load<AudioClip> (resName);
	}

	public void fadeOutMusic() {
		musicTargetVolume [currentMixer] = 0;
	}

	public void fadeInMusic() {
		musicTargetVolume [currentMixer] = musicMaxVolume[currentMixer];
	}

	public void dipMusic(float time) {
		fadeOutMusic ();
		dipRemainingTime = time;
	}

	public void dipMusic() {
		dipMusic (4);
	}

	public void setVolume(float v) 
	{
		musicVolume [currentMixer] = musicTargetVolume [currentMixer] = v;
		aSources [currentMixer].volume = v;
	}

	public void setVolume(float v, float duration) 
	{
		musicTargetVolume [currentMixer] = v;
		musicVolumeFadeSpeed [currentMixer] = 1.0f/duration;
	}

	public void playMusic() 
	{
		if (!EnableMusic)
			return;
		aSources [currentMixer].Play ();
	}

	public void resetSentencePiecesList() 
	{
		sentencePiece = new List<string> ();
	}

	public void addSentencePiece(string piece)
	{
		sentencePiece.Add (piece);
	}

	public List<string> getSentencePieces() 
	{
		return sentencePiece;
	}

	public void unPickUpObject(string name) 
	{
        Debug.Log("Unpicking up: " + name + " @" + currentPickedUpListIndex);
        pickedUpObjectDB[currentPickedUpListIndex].removePickedObject (name);
	}

	public void changeLocation(string locName) 
	{
		/* WARNING change to hash table?? Maybe */
		int i;

		for (i = 0; i < pickedUpObjectDB.Count; ++i) {

			currentPickedUpListIndex = i;

			if (pickedUpObjectDB[currentPickedUpListIndex].locationName ().Equals (locName))
				break;
		}

		if (i == pickedUpObjectDB.Count) { // if location not found...
			PickedUpObjectsList newList = new PickedUpObjectsList ();
			newList.setLocationName (locName);
			pickedUpObjectDB.Add (newList); // ... Add it
			currentPickedUpListIndex = i;
			pickedUpObjectDB[currentPickedUpListIndex] = newList;
		}

		// currentPickedUpList is now pointing to the current location's pickedUpObjectList
		pickedUpObjectDB[currentPickedUpListIndex].rewindList();
		currentLocation = locName;
	}

	public string getCurrentLocation() 
	{
		return currentLocation;
	}

	public string nextPickedUpObject() 
	{
		return pickedUpObjectDB[currentPickedUpListIndex].nextObject ();
	}

	public void registerPickedUpObject(string name) 
	{
        Debug.Log("Picking up object: " + name + " @" + currentPickedUpListIndex);
        if (!pickedUpObjectDB[currentPickedUpListIndex].isInList(name))
        {
            pickedUpObjectDB[currentPickedUpListIndex].addPickedObject(name);
        }
	}

	public bool hasCurrentLocationBeenVisited() 
	{
		return pickedUpObjectDB[currentPickedUpListIndex].hasBeenVisited ();
	}

	private void TeleportPlayer(int LocIndex)
    {
        PlayerScript player = FindObjectOfType<PlayerScript>();
        Teleport teleport = FindObjectOfType<Teleport>();
        Vector3 dest = teleport.GetLocation(LocIndex);
        player.teleport(dest);
    }

    new void Update () 
	{
        // CHETOS A PORRÓN

        // Cheat load/saves, remove afterwards
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            saveGame(false, 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            saveGame(false, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            saveGame(false, 2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            saveGame(false, 3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            saveGame(false, 4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            saveGame(false, 5);
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            PlayerPrefs.SetInt("LoadSlot", 1);
            SceneManager.LoadScene("Nuke");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayerPrefs.SetInt("LoadSlot", 2);
            SceneManager.LoadScene("Nuke");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetInt("LoadSlot", 3);
            SceneManager.LoadScene("Nuke");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.SetInt("LoadSlot", 4);
            SceneManager.LoadScene("Nuke");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerPrefs.SetInt("LoadSlot", 5);
            SceneManager.LoadScene("Nuke");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.SetInt("LoadSlot", 0);
            SceneManager.LoadScene("Nuke");
        }
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            TeleportPlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            TeleportPlayer(2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            TeleportPlayer(3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            TeleportPlayer(4);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            TeleportPlayer(5);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            TeleportPlayer(6);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            TeleportPlayer(7);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            TeleportPlayer(8);
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            TeleportPlayer(9);
        }




        //		N4LiftDirection = getStorage ().retrieveBoolValue ("N4LiftDirection");
        //		N4TestPuentePassed = getStorage ().retrieveBoolValue ("N4TestPuentePassed");
        //		N4Dynamite = getStorage ().retrieveIntValue ("N4Dynamite");
        N3InGround = getStorage().retrieveBoolValue("InGround");
		N3LiftComing = getStorage ().retrieveBoolValue ("LiftComing");

		for(int i = 0; i < numberOfMixers; ++i) {
			bool changed = Utils.updateSoftVariable (ref musicVolume [i], musicTargetVolume [i], musicVolumeFadeSpeed [i]);
			if (changed)
				aSources [i].volume = musicVolume [i];
		}
	
		if (!waitForActivityToFinish) 
		{
			int returned;
			returned = 5;
			returned = returned / 2;
		}

		if (waitForActivityToFinish)
			return;

		if (gameState == GameState.CheckVersion) {
			SceneManager.LoadScene ("Scenes/VersionCheck");
			waitForActivityToFinish = true;
			gameState = GameState.NewUser;
			return;
		}

		if (gameState == GameState.NewUser) {
			SceneManager.LoadScene ("Scenes/UserDetails");
			waitForActivityToFinish = true;
			gameState = GameState.Title;
			return;
		}

		if (gameState == GameState.Title) { // start Title activity

			//SceneManager.LoadScene ("Scenes/Logo");
			SceneManager.LoadScene("Title");
			//SceneManager.LoadScene ("Scenes/ConversationReader");
			waitForActivityToFinish = true;		
			return;
		}

		if (gameState == GameState.MainGame) 
		{
			waitForActivityToFinish = true;
			gameState = GameState.MainGame;
			return;
		}

		if (dipRemainingTime > 0.0f) {
			dipRemainingTime -= Time.deltaTime;
			if (dipRemainingTime <= 0.0f) {
				fadeInMusic ();
			}
		}

        if(gameState == GameState.SkippingToLevel)
        {
            savedLocation = GetStartingLevel();
            LoadAsync();
            gameState = GameState.Idle;
        }

    }

	public void LoadAsync()
	{
		if (savedLocation == null) 
		{
            savedLocation = GetStartingLevel();
            Debug.Log("Saved Location: " + savedLocation);
            if(savedLocation == "")
            {
                savedLocation = firstSceneName;
            }
        }
        _storage.storeBoolValue("FadeWhite", TestFadeWhite);
        PlayerPrefs.SetString("ForcedLevel", "");
        SceneManager.LoadSceneAsync(savedLocation); 
	}

	public void setActivityFinished() 
	{	
		waitForActivityToFinish = false;
	}

	public void saveLocation(string loc) 
	{
		savedLocation = loc;
	}

	public string getSavedLocation() 
	{
		return savedLocation;
	}

	public DataStorage getStorage()
	{
		return _storage;
	}

	public void setLocale(string lang) 
	{

	}

	public string getLocale() 
	{
		return locale;
	}

	public void registerString(string text) 
	{

	}

}

[Serializable]
class SaveGameData {

	public List<PickedUpObjectsList> pickedUpObjectDB; 
	public int currentPickedUpListIndex;
	//public PickedUpObjectsList currentPickedUpList;
	//public DataStorage _storage;
	public SerializableDictionary dict;
	//public GameState gameState;
	//public List<StoredConversation> conversationList;
	public string locationName;
	//public SerializableVector3 playerPosition;

}
