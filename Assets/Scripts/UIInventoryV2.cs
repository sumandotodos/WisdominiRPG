using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIInventoryV2 : WisdominiObject {

	/* Oh my god, it's full of references */

	public LevelControllerScript levelRef;

	// general
	public RawImage powerCircle;
	public RawImage alphabet;
	public RawImage progress;
	public RawImage backpack;
	public RawImage back0;
	public RawImage canvas1;
	public RawImage alphabetLetter;
	public Texture[] letter;

	// progress
	public RawImage shadow;
	public RawImage flame;
	public RawImage well;
	public Text shadowText;
	public Text flameText;
	public Text wellText;
	public RawImage back1;
	public RawImage canvas2;

	// backpack
	public RawImage canvas3;
	public RawImage saltRed0;
	public Pulsate saltRed1;
	public RawImage saltOrange0;
	public Pulsate saltOrange1;
	public RawImage saltGreen0;
	public Pulsate saltGreen1;
	public RawImage saltBlue0;
	public Pulsate saltBlue1;
	public RawImage saltPurple0;
	public Pulsate saltPurple1;
	public RawImage saltYellow0;
	public Pulsate saltYellow1;
	public RawImage saltBrown0;
	public Pulsate saltBrown1;
	public RawImage redKey;
	public RawImage yellowKey;
	public RawImage orangeKey;
	public RawImage blueKey;
	public RawImage greenKey;
	public RawImage purpleKey;
	public RawImage brownKey;
	public RawImage back2;
	public RawImage invisuit;
	public RawImage blackDice1;
	public RawImage blackDice2;
	public RawImage ferDice1;
	public RawImage ferDice2;
	public RawImage ferDice3;
	public RawImage stoneKey;
	public RawImage yellowLens;
	public RawImage redLens;
	public RawImage orangeLens;
	public RawImage creditCard;
	public RawImage bracelet;
	public RawImage tnt;
	public RawImage ferfufloCardsWhite;
	public RawImage ferfufloCardsBlack;
	public RawImage yellowMap;
	public RawImage redMap;
	public RawImage greenMap;
	public RawImage blueMap;
	public RawImage brownMap;
	public RawImage purpleMap;
	public RawImage greyMap;
	public RawImage yellowKeyMage;
	public RawImage redKeyMage;
	public RawImage greenKeyMage;
	public RawImage blueKeyMage;
	public RawImage brownKeyMage;
	public RawImage purpleKeyMage;
	public RawImage greyKeyMage;
	public Text redManaText;
	public Text blueManaText;
	public RawImage whitePiramid01; // Con int; 0 = no hay, 1 = 1 escalon...
	public RawImage whitePiramid02;
	public RawImage whitePiramid03;
	public RawImage whitePiramid04;
	public RawImage blackPiramid01; // bool = pinta
	public RawImage blackPiramid02;
	public RawImage blackPiramid03;
	public RawImage blackPiramid04;
	public Sprite[] piramidWhiteSteps;

	//... to be defined

	public UIFaderScript fader;

	public GameObject menuGeneral;
	public GameObject menuProgress;
	public GameObject menuBackback;

	int level;

	UIInventoryClickItem clickedItem;

	public float xSpeed = 120.0f;
	float x1, targetX1;
	float x2, targetX2;
	float x3, targetX3;
	public float x1Hidden = 750.0f;
	public float x1Showing = 0.0f;
	public float x2Hidden = 750.0f;
	public float x2Showing = 50.0f;
	public float x3Hidden = 750.0f;
	public float x3Showing = 135.0f;

	int mustDisable = -1;

	new void Start () 
	{
		level = 0; // closed inventory
		x1Hidden = 1.5f * Screen.width;
		x2Hidden = 1.5f * Screen.width;
		x3Hidden = 1.5f * Screen.width;
		x1 = targetX1 = x1Hidden;
		x2 = targetX2 = x2Hidden;
		x3 = targetX3 = x3Hidden;

		if (levelRef == null)
		{
			levelRef = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		}

		menuGeneral.transform.localPosition = new Vector3 (x1, 0, 0);
		menuProgress.transform.localPosition = new Vector3 (x2, 0, 0);
		menuBackback.transform.localPosition = new Vector3 (x3, 0, 0);

		setEnableLevel0 (false);
		setEnableLevel1 (false);
		setEnableLevel2 (false);
	}
	
	new void Update () 
	{
		if (mustDisable == 2) {
			if (x3 >= x3Hidden) {
				mustDisable = -1;
				setEnableLevel2 (false);
			}
		} else if (mustDisable == 1) {
			if (x2 >= x2Hidden) {
				mustDisable = -1;
				setEnableLevel1 (false);
			}

		} else if (mustDisable == 0) {
			if (x1 >= x1Hidden) {
				mustDisable = -1;
				setEnableLevel0 (false);
			}
		}

		bool changed = Utils.updateSoftVariable (ref x1, targetX1, xSpeed);
		if (changed)
			menuGeneral.transform.localPosition = new Vector3(x1, 0, 0);

		changed = Utils.updateSoftVariable (ref x2, targetX2, xSpeed);
		if (changed)
			menuProgress.transform.localPosition = new Vector3(x2, 0, 0);

		changed = Utils.updateSoftVariable (ref x3, targetX3, xSpeed);
		if (changed)
			menuBackback.transform.localPosition = new Vector3(x3, 0, 0);

		if (isWaitingForActionToComplete)
			return;
		if (clickedItem == UIInventoryClickItem.powerCircle) 
		{
			SceneManager.LoadScene ("Scenes/Heroes");
		}

		if (clickedItem == UIInventoryClickItem.mapLevel) 
		{
			SceneManager.LoadScene ("Scenes/Maps");
		}

		if (clickedItem == UIInventoryClickItem.alphabet)
		{
			SceneManager.LoadScene ("Scenes/Alphabet");
		}

		if (clickedItem == UIInventoryClickItem.scale) 
		{
			SceneManager.LoadScene ("Scenes/ValuesScale");
		}

		if (clickedItem == UIInventoryClickItem.worldMap) 
		{
			SceneManager.LoadScene ("Scenes/WorldMap");
		}

		if (clickedItem == UIInventoryClickItem.back) 
		{
			close ();
		}	
	}

	public void immediateOpen(int l) 
	{ // for reentry
		if(l>0) {
			x1 = targetX1 = x1Showing;
			menuGeneral.transform.localPosition = new Vector3(x1, 0, 0);
		}
		if(l==2) {
			x2 = targetX2 = x2Showing;
			menuProgress.transform.localPosition = new Vector3(x2, 0, 0);
		}
		if(l==3) {
			x3 = targetX3 = x3Showing;
			menuBackback.transform.localPosition = new Vector3(x3, 0, 0);
		}
		level = l;
	}

	public void open(int l)
	{
		if (l == 1) {
			setEnableLevel0 (true);
			targetX1 = x1Showing;
			levelRef.blockPlayerControls ();
		} else if (l == 2) {
			setEnableLevel1 (true);
			targetX2 = x2Showing;
		} else if (l == 3) {
			setEnableLevel2 (true);
			targetX3 = x3Showing;
		}
		level = l;
	}

	public void reenter(int level) {

		if (level >= 0) {
			setEnableLevel0 (true);
			immediateOpen (1);
			levelRef.blockPlayerControls ();
		}
		if (level >= 1) {
			setEnableLevel1 (true);
			immediateOpen (2);
		}
		if (level >= 2) {
			setEnableLevel2 (true);
			immediateOpen (3);
		}
	}

	public void close() {

		if (level == 3) {
			targetX3 = x3Hidden;
			mustDisable = 2;
			level = 1;
		} else if (level == 2) {
			targetX2 = x2Hidden;
			mustDisable = 1;
			level = 1;
		} else if (level == 1) {
			targetX1 = x1Hidden;
			mustDisable = 0;
			level = 0;
			levelRef.unblockPlayerControls ();
		}
	}

	public void immediateClose()
	{
		targetX3 = x3Hidden;
		targetX2 = x2Hidden;
		targetX1 = x1Hidden;
		mustDisable = 0;
		level = 0;
		levelRef.unblockPlayerControls ();
	}

	public void setEnableLevel0(bool en) 
	{
		canvas1.enabled = en;
		bool b = levelRef.retrieveBoolValue ("HasAlphabet");
		alphabet.enabled = (en && b);
		powerCircle.enabled = en;
		backpack.enabled = en;
		back0.enabled = en;

		if (en && b) {
			bool alphabetInit = levelRef.retrieveBoolValue ("AlphabetInitialized");
			if (alphabetInit) {
				int gs = levelRef.retrieveIntValue ("AlphabetGlobalStep");
				alphabetLetter.texture = letter [gs / Letter.substeps];
				alphabetLetter.enabled = true;
			} else
				alphabetLetter.enabled = false;
		} else {
			alphabetLetter.enabled = false;
		}
	}

	public void setEnableLevel1(bool en) {
		canvas2.enabled = en;
		shadow.enabled = en;
		if (en) {
			int s = levelRef.retrieveIntValue ("DefeatedShadows");
			shadowText.text = "x " + s;
		}
		shadowText.enabled = en;
		flame.enabled = en;
		flameText.enabled = en;
		well.enabled = en;
		wellText.enabled = en;
		back1.enabled = en;

		redManaText.enabled = en;
		if (en) {
			if (levelRef.retrieveIntValue ("RedMana") <= 0) 
			{
				levelRef.storeIntValue ("RedMana", 0);
			}
			redManaText.text = "x " + levelRef.retrieveIntValue ("RedMana");
		}
		blueManaText.enabled = en;
		if (en) {
			if (levelRef.retrieveIntValue ("BlueMana") <= 0) 
			{
				levelRef.storeIntValue ("BlueMana", 0);
			}
			blueManaText.text = "x " + levelRef.retrieveIntValue ("BlueMana");
		}
	}

	public void setEnableLevel2(bool en) {
		canvas3.enabled = en;
		back2.enabled = en;
		saltRed0.enabled = en;
		saltOrange0.enabled = en;
		saltBlue0.enabled = en;
		saltGreen0.enabled = en;
		saltPurple0.enabled = en;
		saltYellow0.enabled = en;
		saltBrown0.enabled = en;
		bool key;
		string lvl = levelRef.locationName.Substring (0, 6);
		key = levelRef.retrieveBoolValue ("Has" + lvl + "RedKey");
		redKey.enabled = key;
		key = levelRef.retrieveBoolValue ("Has" + lvl + "YellowKey");
		yellowKey.enabled = key;
		key = levelRef.retrieveBoolValue ("Has" + lvl + "BlueKey");
		blueKey.enabled = key;
		key = levelRef.retrieveBoolValue ("Has" + lvl + "GreenKey");
		greenKey.enabled = key;
		key = levelRef.retrieveBoolValue ("Has" + lvl + "PurpleKey");
		purpleKey.enabled = key;
		key = levelRef.retrieveBoolValue ("Has" + lvl + "OrangeKey");
		orangeKey.enabled = key;
		key = levelRef.retrieveBoolValue ("Has" + lvl + "BrownKey");
		brownKey.enabled = key;

		key = levelRef.retrieveBoolValue ("Has" + lvl + "RedEnergy");
		saltRed1.setEnable (key);
		key = levelRef.retrieveBoolValue ("Has" + lvl + "YellowEnergy");
		saltYellow1.setEnable (key);
		key = levelRef.retrieveBoolValue ("Has" + lvl + "BlueEnergy");
		saltBlue1.setEnable (key);
		key = levelRef.retrieveBoolValue ("Has" + lvl + "GreenEnergy");
		saltGreen1.setEnable (key);
		key = levelRef.retrieveBoolValue ("Has" + lvl + "OrangeEnergy");
		saltOrange1.setEnable (key);
		key = levelRef.retrieveBoolValue ("Has" + lvl + "PurpleEnergy");
		saltPurple1.setEnable (key);
		key = levelRef.retrieveBoolValue ("Has" + lvl + "BrownEnergy");
		saltBrown1.setEnable (key);



		key = levelRef.retrieveBoolValue ("HasYellowLens");
		if (key && en) {
			yellowLens.enabled = true;
		} else
			yellowLens.enabled = false;

		key = levelRef.retrieveBoolValue ("HasOrangeLens");
		if (key && en) {
			orangeLens.enabled = true;
		} else
			orangeLens.enabled = false;

		key = levelRef.retrieveBoolValue ("HasRedLens");
		if (key && en) {
			redLens.enabled = true;
		} else
			redLens.enabled = false;

		key = levelRef.retrieveBoolValue ("HasInvisuit");
		if (key && en) {
			invisuit.enabled = true;
		} else
			invisuit.enabled = false;

		key = levelRef.retrieveBoolValue ("HasStoneKey");
		if (key && en) {
			stoneKey.enabled = true;
		} else
			stoneKey.enabled = false;

		key = levelRef.retrieveBoolValue ("HasTNT");
		if (key && en) {
			tnt.enabled = true;
		} else
			tnt.enabled = false;

		key = levelRef.retrieveBoolValue ("HasBlackDice1");
		if (key && en) {
			blackDice1.enabled = true;
		} else
			blackDice1.enabled = false;

		key = levelRef.retrieveBoolValue ("HasBackDice2");
		if (key && en) {
			blackDice2.enabled = true;
		} else
			blackDice2.enabled = false;

		key = levelRef.retrieveBoolValue ("HasFerfufloDice1");
		if (key && en) {
			ferDice1.enabled = true;

		} else {
			ferDice1.enabled = false;

		}
		key = levelRef.retrieveBoolValue ("HasFerfufloDice2");
		if (key && en) {
			
			ferDice2.enabled = true;

		} else {
			
			ferDice2.enabled = false;

		}
		key = levelRef.retrieveBoolValue ("HasFerfufloDice3");
		if (key && en) {
			
			ferDice3.enabled = true;
		} else {
			
			ferDice3.enabled = false;
		}

		key = levelRef.retrieveBoolValue ("HasBracelet");
		if (key && en) {
			bracelet.enabled = true;
		} else
			bracelet.enabled = false;

		key = levelRef.retrieveBoolValue ("HasCreditCard");
		if (key && en) {
			creditCard.enabled = true;
		} else
			creditCard.enabled = false;

		key = levelRef.retrieveBoolValue ("HasFerfufloCards");
		if (key && en) {
			ferfufloCardsWhite.enabled = true;
			ferfufloCardsBlack.enabled = true;
		} else {
			ferfufloCardsWhite.enabled = false;
			ferfufloCardsBlack.enabled = false;
		}

		key = levelRef.retrieveBoolValue ("HasRedKeyMage");
		redKeyMage.enabled = key;
		key = levelRef.retrieveBoolValue ("HasYellowKeyMage");
		yellowKeyMage.enabled = key;
		key = levelRef.retrieveBoolValue ("HasBlueKeyMage");
		blueKeyMage.enabled = key;
		key = levelRef.retrieveBoolValue ("HasGreenKeyMage");
		greenKeyMage.enabled = key;
		key = levelRef.retrieveBoolValue ("HasPurpleKeyMage");
		purpleKeyMage.enabled = key;
		key = levelRef.retrieveBoolValue ("HasGreyKeyMage");
		greyKeyMage.enabled = key;
		key = levelRef.retrieveBoolValue ("HasBrownKeyMage");
		brownKeyMage.enabled = key;

		key = levelRef.retrieveBoolValue ("HasRedMap");
		redMap.enabled = key;
		key = levelRef.retrieveBoolValue ("HasYellowMap");
		yellowMap.enabled = key;
		key = levelRef.retrieveBoolValue ("HasBlueMap");
		blueMap.enabled = key;
		key = levelRef.retrieveBoolValue ("HasGreenMap");
		greenMap.enabled = key;
		key = levelRef.retrieveBoolValue ("HasPurpleMap");
		purpleMap.enabled = key;
		key = levelRef.retrieveBoolValue ("HasGreyMap");
		greyMap.enabled = key;
		key = levelRef.retrieveBoolValue ("HasBrownMap");
		brownMap.enabled = key;

		key = levelRef.retrieveBoolValue ("RedMapDone");
		redMap.GetComponent<Button>().interactable = !key;
		key = levelRef.retrieveBoolValue ("YellowMapDone");
		yellowMap.GetComponent<Button>().interactable = !key;
		key = levelRef.retrieveBoolValue ("BlueMapDone");
		blueMap.GetComponent<Button>().interactable = !key;
		key = levelRef.retrieveBoolValue ("GreenMapDone");
		greenMap.GetComponent<Button>().interactable = !key;
		key = levelRef.retrieveBoolValue ("PurpleMapDone");
		purpleMap.GetComponent<Button>().interactable = !key;
		key = levelRef.retrieveBoolValue ("GreyMapDone");
		greyMap.GetComponent<Button>().interactable = !key;
		key = levelRef.retrieveBoolValue ("BrownMapDone");
		brownMap.GetComponent<Button>().interactable = !key;

		key = levelRef.retrieveBoolValue ("HasBlackPiramid01");
		blackPiramid01.enabled = key;
		key = levelRef.retrieveBoolValue ("HasBlackPiramid02");
		blackPiramid02.enabled = key;
		key = levelRef.retrieveBoolValue ("HasBlackPiramid03");
		blackPiramid03.enabled = key;
		key = levelRef.retrieveBoolValue ("HasBlackPiramid04");
		blackPiramid04.enabled = key;

		int num;
		num = levelRef.retrieveIntValue ("HasWhitePiramid01");
		if (num == 0)
			whitePiramid01.enabled = false;
		else if (num > 0) {
			whitePiramid01.enabled = true;
			whitePiramid01.GetComponent<SpriteRenderer> ().sprite = piramidWhiteSteps [num-1];
		}
		if (num == 5)
			whitePiramid01.GetComponent<Button> ().interactable = true;
		num = levelRef.retrieveIntValue ("HasWhitePiramid02");
		if (num == 0)
			whitePiramid02.enabled = false;
		else if (num > 0) {
			whitePiramid02.enabled = true;
			whitePiramid02.GetComponent<SpriteRenderer> ().sprite = piramidWhiteSteps [num-1];
		}
		if (num == 5)
			whitePiramid02.GetComponent<Button> ().interactable = true;
		num = levelRef.retrieveIntValue ("HasWhitePiramid03");
		if (num == 0)
			whitePiramid03.enabled = false;
		else if (num > 0) {
			whitePiramid03.enabled = true;
			whitePiramid03.GetComponent<SpriteRenderer> ().sprite = piramidWhiteSteps [num-1];
		}
		if (num == 5)
			whitePiramid03.GetComponent<Button> ().interactable = true;
		num = levelRef.retrieveIntValue ("HasWhitePiramid04");
		if (num == 0)
			whitePiramid04.enabled = false;
		else if (num > 0) {
			whitePiramid04.enabled = true;
			whitePiramid04.GetComponent<SpriteRenderer> ().sprite = piramidWhiteSteps [num-1];
		}
		if (num == 5)
			whitePiramid04.GetComponent<Button> ().interactable = true;
	}

	public void clickOnPowerCircle() 
	{
		fader._wa_fadeOut (this);
		this.isWaitingForActionToComplete = true;
		clickedItem = UIInventoryClickItem.powerCircle;
		levelRef.storeStringValue ("ReturnLocation", levelRef.locationName);
		levelRef.storeStringValue ("ReentryCondition", "Inventory");
		levelRef.storeIntValue ("InventoryLevel", 0);
		levelRef.storePlayerCoordinates ();
		//levelRef.storePhysicalCameraPosition ();
	}

	public void clickOnMap(int _num) 
	{
		fader._wa_fadeOut (this);
		this.isWaitingForActionToComplete = true;
		clickedItem = UIInventoryClickItem.mapLevel;
		levelRef.storeIntValue ("CurrentMap", _num);
		levelRef.storeStringValue ("ReturnLocation", levelRef.locationName);
		levelRef.storeStringValue ("ReentryCondition", "Inventory");
		levelRef.storeIntValue ("InventoryLevel", 2);
		levelRef.storePlayerCoordinates ();
	}

	public void clickOnScaleValues(int _num) 
	{
		fader._wa_fadeOut (this);
		this.isWaitingForActionToComplete = true;
		clickedItem = UIInventoryClickItem.scale;
		levelRef.storeStringValue ("ReturnLocation", levelRef.locationName);
		levelRef.storeStringValue ("ReentryCondition", "Inventory");
		levelRef.storeIntValue ("InventoryLevel", 2);
		levelRef.storePlayerCoordinates ();
	}

	public void clickOnAlphabet() 
	{
		fader._wa_fadeOut (this);
		this.isWaitingForActionToComplete = true;
		clickedItem = UIInventoryClickItem.alphabet;
		levelRef.storeStringValue ("ReturnLocation", levelRef.locationName);
		levelRef.storeStringValue ("ReentryCondition", "Inventory");
		levelRef.storeIntValue ("InventoryLevel", 0);
		levelRef.storePlayerCoordinates ();
	}

	public void clickOnProgress() 
	{
		open (2);
	}

	public void clickOnBackpack() 
	{
		open (3);
	}

	public void clickOnBack0()
	{
		close ();
	}

	public void clickOnBack1() 
	{
		close ();
	}

	public void clickOnFacebook() 
	{

	}

	public void clickOnTwitter() 
	{

	}
}
