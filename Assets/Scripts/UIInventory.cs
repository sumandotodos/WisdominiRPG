using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum UIInventoryState { root, stats, items };
public enum UIInventoryClickItem { none, powerCircle, alphabet, progress, backpack, back, fb, twitter, scale, worldMap, mapLevel };

public class UIInventory : WisdominiObject {

	/* references */

	public Sprite powerCircle;
	public Sprite blueMana;
	public Sprite redMana;
	public Sprite cyanKey;
	public Sprite alphabet;
	public Sprite progress;
	public Sprite backpack;
	public Sprite back;
	public Sprite shadow;
	public Sprite questions;
	public Sprite wells;
	public Sprite facebook;
	public Sprite twitter;
	public Sprite map;
	public Sprite pill;
	public Sprite worldMap;
	public Sprite[] scales;
	public Sprite[] seventElementsEnergy;
	public Sprite[] lightBridgeKey;


	public InventoryObject backpackInventoryAnim;

	public GameObject container;

	public UIFaderScript fader;

	public MasterControllerScript mcRef;

	public LevelControllerScript levelRef;

	public InventoryObject inventory;

	public PlayerScript player;


	/* properties */

	UIInventoryState status;
	bool initialized = false;
	GameObject disposableContainer;

	GameObject powerCircleGO;
	GameObject alphabetGO;
	GameObject progressGO;
	GameObject backpackGO;
	GameObject backGO;
	GameObject back2GO;
	GameObject shadowGO;
	GameObject questionsGO;
	GameObject wellsGO;
	GameObject facebookGO;
	GameObject twitterGO;

	GameObject shadowsTextGO;
	GameObject questionsTextGO;
	GameObject wellsTextGO;

	GameObject[] elementsCrystalGO;
	GameObject[] lightBridgeKeyGO;
	GameObject redManaGO;
	GameObject blueManaGO;
	GameObject[] mapGO;
	GameObject[] valuesScaleGO;
	GameObject pillGO;
	GameObject redManaTextGO;
	GameObject blueManaTextGO;
	GameObject pillTextGO;
	GameObject worldMapGO;

	UIInventoryItem powerCircleItem;
	UIInventoryItem alphabetItem;
	UIInventoryItem progressItem;
	UIInventoryItem backpackItem;
	UIInventoryItem backItem;
	UIInventoryItem back2Item;
	UIInventoryItem shadowItem;
	UIInventoryItem questionsItem;
	UIInventoryItem wellsItem;
	UIInventoryItem facebookItem;
	UIInventoryItem twitterItem;
	UIInventoryItem blueManaItem;
	UIInventoryItem redManaItem;
	UIInventoryItem worldMapItem;

	UIInventoryItem[] elementsCrystalItem;
	UIInventoryItem[] lightBridgeKeyItem;
	UIInventoryItem[] mapItem; 
	UIInventoryItem[] valuesScaleItem;
	UIInventoryItem pillItem;

	UIInventoryText shadowsText;
	UIInventoryText questionsText;
	UIInventoryText wellsText;
	UIInventoryText redManaText;
	UIInventoryText blueManaText;
	UIInventoryText pillText;

	RectTransform rect;

	UIInventoryClickItem clickedItem;

	new void Start() {

		Initialize ();
	
	}

	// Use this for initialization
	public void Initialize () {

		if (initialized)
			return;

		initialized = true;

		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript>();

		disposableContainer = Instantiate (container);
		disposableContainer.transform.SetParent(container.transform);
		disposableContainer.transform.localScale = Vector3.one;            
		disposableContainer.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		disposableContainer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

		disposableContainer.name = "UIDisposableContainer";


		status = UIInventoryState.root;

		UIInventoryItem item;

		powerCircleGO = new GameObject ();
		powerCircleGO.transform.parent = this.gameObject.transform;
		powerCircleGO.name = "UIInventoryPowerCircle";
		powerCircleItem = powerCircleGO.AddComponent<UIInventoryItem> ();
		powerCircleItem.id = UIInventoryClickItem.powerCircle;
		powerCircleItem.delay = 1.25f;
		powerCircleItem.theSprite = powerCircle;
		powerCircleItem.parent = disposableContainer;
		powerCircleItem.inventoryController = this;
		powerCircleItem.initialize ();

		alphabetGO = new GameObject ();
		alphabetGO.transform.parent = this.gameObject.transform;
		alphabetGO.name = "UIInventoryAlphabet";
		alphabetItem = alphabetGO.AddComponent<UIInventoryItem> ();
		alphabetItem.id = UIInventoryClickItem.alphabet;
		alphabetItem.delay = 1.50f;
		alphabetItem.theSprite = alphabet;
		alphabetItem.parent = disposableContainer;
		alphabetItem.inventoryController = this;
		alphabetItem.initialize ();

		progressGO = new GameObject ();
		progressGO.transform.parent = this.gameObject.transform;
		progressGO.name = "UIInventoryProgress";
		progressItem = progressGO.AddComponent<UIInventoryItem> ();
		progressItem.id = UIInventoryClickItem.progress;
		progressItem.delay = 1.75f;
		progressItem.theSprite = progress;
		progressItem.parent = disposableContainer;
		progressItem.inventoryController = this;
		progressItem.initialize ();

		backpackGO = new GameObject ();
		backpackGO.transform.parent = this.gameObject.transform;
		backpackGO.name = "UIInventoryBackpack";
		backpackItem = backpackGO.AddComponent<UIInventoryItem> ();
		backpackItem.id = UIInventoryClickItem.backpack;
		backpackItem.delay = 2.00f;
		backpackItem.theSprite = backpack;
		backpackItem.parent = disposableContainer;
		backpackItem.inventoryController = this;
		backpackItem.initialize ();

		backGO = new GameObject ();
		backGO.transform.parent = this.gameObject.transform;
		backGO.name = "UIInventoryBack";
		backItem = backGO.AddComponent<UIInventoryItem> ();
		backItem.id = UIInventoryClickItem.back;
		backItem.delay = 2.25f;
		backItem.theSprite = back;
		backItem.parent = disposableContainer;
		backItem.inventoryController = this;
		backItem.initialize ();

		back2GO = new GameObject ();
		back2GO.transform.parent = this.gameObject.transform;
		back2GO.name = "UIInventoryBack2";
		back2Item = back2GO.AddComponent<UIInventoryItem> ();
		back2Item.id = UIInventoryClickItem.back;
		back2Item.delay = 0.25f;
		back2Item.theSprite = back;
		back2Item.parent = disposableContainer;
		back2Item.inventoryController = this;
		back2Item.initialize ();

		shadowGO = new GameObject ();
		shadowGO.transform.parent = this.gameObject.transform;
		shadowGO.name = "UIInventoryShadow";
		shadowItem = shadowGO.AddComponent<UIInventoryItem> ();
		//backItem.id = UIInventoryClickItem.back;
		shadowItem.delay = 0.25f;
		shadowItem.theSprite = shadow;
		shadowItem.parent = disposableContainer;
		shadowItem.inventoryController = this;
		shadowItem.initialize ();

		questionsGO = new GameObject ();
		questionsGO.transform.parent = this.gameObject.transform;
		questionsGO.name = "UIInventoryQuestions";
		questionsItem = questionsGO.AddComponent<UIInventoryItem> ();
		//questionsItem.id = UIInventoryClickItem.back;
		questionsItem.delay = 0.50f;
		questionsItem.theSprite = questions;
		questionsItem.parent = disposableContainer;
		questionsItem.inventoryController = this;
		questionsItem.initialize ();

		wellsGO = new GameObject ();
		wellsGO.transform.parent = this.gameObject.transform;
		wellsGO.name = "UIInventoryWells";
		wellsItem = wellsGO.AddComponent<UIInventoryItem> ();
		//wellsItem.id = UIInventoryClickItem.back;
		wellsItem.delay = 0.75f;
		wellsItem.theSprite = wells;
		wellsItem.parent = disposableContainer;
		wellsItem.inventoryController = this;
		wellsItem.initialize ();

		facebookGO = new GameObject ();
		facebookGO.transform.parent = this.gameObject.transform;
		facebookGO.name = "UIInventoryFacebook";
		facebookItem = facebookGO.AddComponent<UIInventoryItem> ();
		facebookItem.id = UIInventoryClickItem.fb;
		facebookItem.delay = 1.00f;
		facebookItem.theSprite = facebook;
		facebookItem.parent = disposableContainer;
		facebookItem.inventoryController = this;
		facebookItem.initialize ();

		twitterGO = new GameObject ();
		twitterGO.transform.parent = this.gameObject.transform;
		twitterGO.name = "UIInventoryTwitter";
		twitterItem = twitterGO.AddComponent<UIInventoryItem> ();
		twitterItem.id = UIInventoryClickItem.twitter;
		twitterItem.delay = 1.25f;
		twitterItem.theSprite = twitter;
		twitterItem.parent = disposableContainer;
		twitterItem.inventoryController = this;
		twitterItem.initialize ();

		redManaGO = new GameObject ();
		redManaGO.transform.parent = this.gameObject.transform;
		redManaGO.name = "UIInventoryRedMana";
		redManaItem = redManaGO.AddComponent<UIInventoryItem> ();
		redManaItem.delay = 2.00f;
		redManaItem.theSprite = redMana;
		redManaItem.parent = disposableContainer;
		redManaItem.inventoryController = this;
		redManaItem.initialize ();
		redManaItem.disableRaycast ();

		blueManaGO = new GameObject ();
		blueManaGO.transform.parent = this.gameObject.transform;
		blueManaGO.name = "UIInventoryBlueMana";
		blueManaItem = blueManaGO.AddComponent<UIInventoryItem> ();
		blueManaItem.delay = 2.00f;
		blueManaItem.theSprite = blueMana;
		blueManaItem.parent = disposableContainer;
		blueManaItem.inventoryController = this;
		blueManaItem.initialize ();
		blueManaItem.disableRaycast ();


		worldMapGO = new GameObject ();
		worldMapGO.transform.parent = this.gameObject.transform;
		worldMapGO.name = "UIInventoryWorldMap";
		worldMapItem = worldMapGO.AddComponent<UIInventoryItem> ();
		worldMapItem.delay = 2.00f;
		worldMapItem.id = UIInventoryClickItem.worldMap;
		worldMapItem.theSprite = worldMap;
		worldMapItem.parent = disposableContainer;
		worldMapItem.inventoryController = this;
		worldMapItem.initialize ();
		worldMapItem.disableRaycast ();


		shadowsTextGO = new GameObject ();
		shadowsTextGO.transform.parent = this.gameObject.transform;
		shadowsTextGO.name = "UIInventoryShadowsText";
		shadowsText = shadowsTextGO.AddComponent<UIInventoryText> ();
		shadowsText.delay = 0.25f;
		DataStorage ds = mcRef.getStorage ();
		int nShadows = ds.retrieveIntValue ("transformedShadows");
		shadowsText.theString = "x " + nShadows;
		shadowsText.parent = disposableContainer;
		shadowsText.inventoryController = this;
		shadowsText.initialize ();

		questionsTextGO = new GameObject ();
		questionsTextGO.transform.parent = this.gameObject.transform;
		questionsTextGO.name = "UIInventoryQuestionsText";
		questionsText = questionsTextGO.AddComponent<UIInventoryText> ();
		questionsText.delay = 0.50f;
		int okQuestions = ds.retrieveIntValue ("correctlyAnsweredQuestions");
		int nQuestions = ds.retrieveIntValue ("totalAnsweredQuestions");
		if (nQuestions > 0) {
			questionsText.theString = "x " + okQuestions + " (" + (((float)okQuestions)/((float)nQuestions)*100.0f) + "%%)";
		} else
			questionsText.theString = "x 0";
		questionsText.parent = disposableContainer;
		questionsText.inventoryController = this;
		questionsText.initialize ();

		wellsTextGO = new GameObject ();
		wellsTextGO.transform.parent = this.gameObject.transform;
		wellsTextGO.name = "UIInventoryWellsText";
		wellsText = wellsTextGO.AddComponent<UIInventoryText> ();
		wellsText.delay = 0.75f;
		int okWells = ds.retrieveIntValue ("CorrectlySolvedWells");
		int nWells = ds.retrieveIntValue ("TotalWells");
		if (nWells > 0) {
			wellsText.theString = "x " + okWells + " (" + (((float)okWells) / ((float)nWells) * 100.0f) + ")";
		} else {
			wellsText.theString = "x 0";
		}
		wellsText.parent = disposableContainer;
		wellsText.inventoryController = this;
		wellsText.initialize ();


		redManaTextGO = new GameObject ();
		redManaTextGO.transform.parent = this.gameObject.transform;
		redManaTextGO.name = "UIInventoryRedManaText";
		redManaText = redManaTextGO.AddComponent<UIInventoryText> ();
		redManaText.delay = 0.75f;
		int redManaAmount = ds.retrieveIntValue ("RedManaAmount");
		redManaText.theString = "x " + redManaAmount;
		redManaText.parent = disposableContainer;
		redManaText.inventoryController = this;
		redManaText.initialize ();


		blueManaTextGO = new GameObject ();
		blueManaTextGO.transform.parent = this.gameObject.transform;
		blueManaTextGO.name = "UIInventoryBlueManaText";
		blueManaText = blueManaTextGO.AddComponent<UIInventoryText> ();
		blueManaText.delay = 0.75f;
		int blueManaAmount = ds.retrieveIntValue ("BlueManaAmount");
		blueManaText.theString = "x " + blueManaAmount;
		blueManaText.parent = disposableContainer;
		blueManaText.inventoryController = this;
		blueManaText.initialize ();


		elementsCrystalGO = new GameObject[7];
		elementsCrystalItem = new UIInventoryItem[7];
		for (int i = 0; i < 7; ++i) {

			elementsCrystalGO [i] = new GameObject ();
			elementsCrystalGO [i].name = "UIInventoryElementCrystal" + i;
			elementsCrystalItem[i] = elementsCrystalGO[i].AddComponent<UIInventoryItem> ();
			elementsCrystalItem[i].delay = 1.0f + 0.12f * i;
			bool active = ds.retrieveBoolValue ("7elementsEnergy(" + i + ")Active");
			if(active) 
				elementsCrystalItem[i].theSprite = seventElementsEnergy[i*2 + 1];
			else
				elementsCrystalItem[i].theSprite = seventElementsEnergy[i*2];
			elementsCrystalItem[i].parent = disposableContainer;
			elementsCrystalItem[i].inventoryController = this;
			elementsCrystalItem[i].initialize ();
			elementsCrystalItem [i].disableRaycast ();


		}

		lightBridgeKeyGO = new GameObject[7];
		lightBridgeKeyItem = new UIInventoryItem[7];
		for (int i = 0; i < 7; ++i) {

			lightBridgeKeyGO [i] = new GameObject ();
			lightBridgeKeyGO [i].name = "UIInventoryLightBridgeKey" + i;
			lightBridgeKeyItem[i] = lightBridgeKeyGO[i].AddComponent<UIInventoryItem> ();
			lightBridgeKeyItem[i].delay = 1.88f + 0.12f * i;
			bool gotKey = ds.retrieveBoolValue ("HasLightBridgeKey(" + i + ")");
			if (gotKey) {
				lightBridgeKeyItem [i].maxOpacity = 1.0f;
			} else {
				lightBridgeKeyItem [i].maxOpacity = 0.2f;
			}
			lightBridgeKeyItem[i].theSprite = lightBridgeKey[i];

			lightBridgeKeyItem[i].parent = disposableContainer;
			lightBridgeKeyItem[i].inventoryController = this;
			lightBridgeKeyItem[i].initialize ();
			lightBridgeKeyItem [i].disableRaycast ();


		}

		valuesScaleGO = new GameObject[8];
		valuesScaleItem = new UIInventoryItem[8];
		for (int i = 0; i < 8; ++i) {

			valuesScaleGO [i] = new GameObject ();
			valuesScaleGO [i].name = "UIInventoryScaleOfValues" + i;
			valuesScaleItem[i] = valuesScaleGO[i].AddComponent<UIInventoryItem> ();
			valuesScaleItem[i].delay = 2.33f + 0.12f * i;
			int numberOfSteps = ds.retrieveIntValue ("ValuesScale(" + i + ")steps");

			valuesScaleItem[i].theSprite = scales[numberOfSteps];
			valuesScaleItem [i].id = UIInventoryClickItem.scale;
			valuesScaleItem [i].intId = i;
			valuesScaleItem[i].parent = disposableContainer;
			valuesScaleItem[i].inventoryController = this;
			valuesScaleItem[i].initialize ();
			if(numberOfSteps > 0) 
				valuesScaleItem[i].enableRaycast ();
			else
				valuesScaleItem[i].disableRaycast ();

		}

		mapGO = new GameObject[7];
		mapItem = new UIInventoryItem[7];
		for (int i = 0; i < 7; ++i) {
			mapGO[i] = new GameObject ();
			mapGO [i].name = "UIInventoryMap" + i;
			mapItem [i] = mapGO [i].AddComponent<UIInventoryItem> ();
			mapItem [i].delay = 2.5f + 0.06f * i;
			bool gotMap = ds.retrieveBoolValue ("HasLightMap(" + i + ")");
			if (gotMap) {
				mapItem [i].maxOpacity = 1.0f;
			} else {
				mapItem [i].maxOpacity = 0.2f;
			}
			mapItem [i].theSprite = map;
			mapItem [i].parent = disposableContainer;
			mapItem [i].inventoryController = this;
			mapItem [i].initialize ();
			mapItem [i].disableRaycast ();
		}

		pillGO = new GameObject ();
		pillGO.name = "UIInventoryPill";
		pillGO.transform.parent = this.gameObject.transform;
		pillItem = pillGO.AddComponent<UIInventoryItem> ();
		pillItem.theSprite = pill;
		pillItem.parent = disposableContainer;
		pillItem.inventoryController = this;
		pillItem.initialize ();
		pillItem.disableRaycast ();

		pillTextGO = new GameObject ();
		pillTextGO.transform.parent = this.gameObject.transform;
		pillTextGO.name = "UIInventoryPillText";
		pillText = pillTextGO.AddComponent<UIInventoryText> ();
		pillText.delay = 0.75f;
		int numberOfPills = ds.retrieveIntValue ("WisdomPillsAmount");
		pillText.theString = "x " + numberOfPills;
		pillText.parent = disposableContainer;
		pillText.inventoryController = this;
		pillText.initialize ();

		rect = this.gameObject.GetComponent<RectTransform> ();

		powerCircleGO.transform.localPosition = new Vector3 (-this.rect.rect.height * 0.3f, 0, 0);
		alphabetGO.transform.localPosition = new Vector3 (-this.rect.rect.height * 0.1f, 0, 0);
		progressGO.transform.localPosition = new Vector3 (this.rect.rect.height * 0.1f, 0, 0);
		backpackGO.transform.localPosition = new Vector3 (this.rect.rect.height * 0.3f, 0, 0);
		backGO.transform.localPosition = new Vector3 (-this.rect.rect.height * 0.35f, this.rect.rect.height * 0.25f, 0);
		back2GO.transform.localPosition = new Vector3 (-this.rect.rect.height * 0.60f, this.rect.rect.height * 0.33f, 0);

		shadowGO.transform.localPosition = new Vector3 (-this.rect.rect.height * 0.25f, this.rect.rect.height * 0.15f, 0);
		questionsGO.transform.localPosition = new Vector3 (-this.rect.rect.height * 0.25f, -this.rect.rect.height * 0.00f, 0);
		wellsGO.transform.localPosition = new Vector3 (-this.rect.rect.height * 0.25f, -this.rect.rect.height * 0.17f, 0);
		facebookGO.transform.localPosition = new Vector3 (this.rect.rect.height * 0.22f, -this.rect.rect.height * 0.17f, 0);
		twitterGO.transform.localPosition = new Vector3 (this.rect.rect.height * 0.35f, -this.rect.rect.height * 0.17f, 0);

		redManaGO.transform.localPosition = new Vector3 (-this.rect.rect.height * 0.35f, this.rect.rect.height * 0.08f, 0);
		blueManaGO.transform.localPosition = new Vector3 (this.rect.rect.height * 0.15f, this.rect.rect.height * 0.08f, 0);
		worldMapGO.transform.localPosition = new Vector3 (this.rect.rect.height * 0.45f, this.rect.rect.height * 0.10f, 0);

		powerCircleGO.transform.localScale = new Vector2 (this.rect.rect.height/600.0f, this.rect.rect.height/600.0f);
		alphabetGO.transform.localScale = new Vector2 (this.rect.rect.height/600.0f, this.rect.rect.height/600.0f);
		progressGO.transform.localScale = new Vector2 (this.rect.rect.height/600.0f, this.rect.rect.height/600.0f);
		backpackGO.transform.localScale = new Vector2 (this.rect.rect.height/600.0f, this.rect.rect.height/600.0f);
		backGO.transform.localScale = new Vector2 (this.rect.rect.height/700.0f, this.rect.rect.height/700.0f);
		back2GO.transform.localScale = new Vector2 (this.rect.rect.height/700.0f, this.rect.rect.height/700.0f);

		shadowGO.transform.localScale = new Vector2 (this.rect.rect.height/1200.0f, this.rect.rect.height/600.0f);
		questionsGO.transform.localScale = new Vector2 (this.rect.rect.height/700.0f, this.rect.rect.height/700.0f);
		wellsGO.transform.localScale = new Vector2 (this.rect.rect.height/700.0f, this.rect.rect.height/700.0f);
		facebookGO.transform.localScale = new Vector2 (this.rect.rect.height/800.0f, this.rect.rect.height/800.0f);
		twitterGO.transform.localScale = new Vector2 (this.rect.rect.height/800.0f, this.rect.rect.height/800.0f);

		redManaGO.transform.localScale = new Vector2 (this.rect.rect.height/600.0f, this.rect.rect.height/600.0f);
		blueManaGO.transform.localScale = new Vector2 (this.rect.rect.height/600.0f, this.rect.rect.height/600.0f);
		worldMapGO.transform.localScale = new Vector2 (this.rect.rect.height/660.0f, this.rect.rect.height/660.0f);

		shadowsTextGO.transform.localPosition = new Vector3 (0, 0, 0);
		questionsTextGO.transform.localPosition = new Vector3 (0, -this.rect.rect.height * 0.17f, 0);
		wellsTextGO.transform.localPosition = new Vector3 (0, -this.rect.rect.height * 0.35f, 0);

		redManaTextGO.transform.localPosition = new Vector3 (-this.rect.rect.height * 0.20f, this.rect.rect.height * 0.00f, 0);
		blueManaTextGO.transform.localPosition = new Vector3 (this.rect.rect.height * 0.35f, this.rect.rect.height * 0.00f, 0);



		for (int i = 0; i < 7; ++i) {
			elementsCrystalGO [i].transform.localScale = new Vector2 (this.rect.rect.height / 2400.0f,
				this.rect.rect.height / 1200.0f);
			elementsCrystalGO [i].transform.localPosition =
				new Vector3 (-this.rect.rect.height * 0.45f + this.rect.rect.height * 0.07f * i, 
				this.rect.rect.height * 0.23f,
				0);
		}
		for (int i = 0; i < 7; ++i) {
			lightBridgeKeyGO [i].transform.localScale = new Vector2 (this.rect.rect.height / 1800.0f,
				this.rect.rect.height / 900.0f);
			lightBridgeKeyGO [i].transform.localPosition =
				new Vector3 (this.rect.rect.height * 0.15f + this.rect.rect.height * 0.07f * i, 
					this.rect.rect.height * 0.23f,
					0);
		}
		for (int i = 0; i < 8; ++i) {
			valuesScaleGO [i].transform.localScale = new Vector2 (this.rect.rect.height / 900.0f,
				this.rect.rect.height / 900.0f);
			valuesScaleGO [i].transform.localPosition =
				new Vector3 (-this.rect.rect.height * 0.45f + this.rect.rect.height * 0.12f * i, 
					-this.rect.rect.height * 0.20f,
					0);
		}
		for (int i = 0; i < 7; ++i) {
			mapGO [i].transform.localScale = new Vector2 (this.rect.rect.height / 900.0f, this.rect.rect.height / 900.0f);
			mapGO[i].transform.localPosition = new Vector3 (-this.rect.rect.height * 0.55f + this.rect.rect.height * 0.18f * i, 
				-this.rect.rect.height * 0.08f,
				0);
		}

		pillGO.transform.localPosition = new Vector3 (-this.rect.rect.height * 0.07f, -this.rect.rect.height * 0.31f, 0);
		pillTextGO.transform.localPosition = new Vector3 (this.rect.rect.height * 0.07f, -this.rect.rect.height * 0.38f, 0);
		pillGO.transform.localScale = new Vector2 (this.rect.rect.height/1200.0f, this.rect.rect.height/1200.0f);
	



		shadowItem.disableRaycast ();
		questionsItem.disableRaycast ();
		wellsItem.disableRaycast ();
		facebookItem.disableRaycast ();
		twitterItem.disableRaycast ();

		clickedItem = UIInventoryClickItem.none;

		hide ();
	
	}
	
	// Update is called once per frame
	new void Update () {

		if (isWaitingForActionToComplete)
			return;

		if (clickedItem == UIInventoryClickItem.powerCircle) {

			SceneManager.LoadScene ("Scenes/Heroes");

		}

		if (clickedItem == UIInventoryClickItem.alphabet) {

			SceneManager.LoadScene ("Scenes/Alphabet");

		}

		if (clickedItem == UIInventoryClickItem.scale) {

			SceneManager.LoadScene ("Scenes/ValuesScale");

		}

		if (clickedItem == UIInventoryClickItem.worldMap) {

			SceneManager.LoadScene ("Scenes/WorldMap");

		}

		if (clickedItem == UIInventoryClickItem.back) {
			Destroy (disposableContainer);
			initialized = false;
			clickedItem = UIInventoryClickItem.none;
		}


	}

	public void show() {

		if (!initialized)
			Initialize ();
		powerCircleItem.show ();
		powerCircleItem.enableRaycast ();
		alphabetItem.show ();
		alphabetItem.enableRaycast ();
		progressItem.show ();
		progressItem.enableRaycast ();
		backpackItem.show ();
		backpackItem.enableRaycast ();
		backItem.show ();
		backItem.enableRaycast ();

	}

	public void hide() {

		//powerCircleItem.hide ();
		//alphabetItem.hide ();
		//progressItem.hide ();
		//backpackItem.hide ();
		//backItem.hide ();


	}

	public void notifyItemClicked(int item) {

		if (item == 0)
			notifyItemClicked (UIInventoryClickItem.powerCircle, 0);

	}


	public void notifyItemClicked(UIInventoryClickItem item, int integerId) {

		if (status == UIInventoryState.root) {

			if (item == UIInventoryClickItem.powerCircle) {

				DataStorage ds;
				ds = mcRef.getStorage ();
				ds.storeStringValue ("HeroesActivityMode", "Inventory");
				ds.storeStringValue ("MainActivityMode", "Inventory");
				ds.storeStringValue ("MainActivityInventoryLocation", "Root");
				ds.storeStringValue ("ReturnLocation", levelRef.locationName);

				fader._wa_fadeOut (this);
				this.isWaitingForActionToComplete = true;

				clickedItem = UIInventoryClickItem.powerCircle;

			}

			if (item == UIInventoryClickItem.back) {

				powerCircleItem.hide ();
				alphabetItem.hide ();
				backItem.hide ();
				backpackItem.hide ();
				progressItem.hide ();
				player.unblockControls ();
				inventory._wa_close (this);
				this.isWaitingForActionToComplete = true;
				clickedItem = item;

			}

			if (item == UIInventoryClickItem.alphabet) {

				DataStorage ds;
				ds = mcRef.getStorage ();
				ds.storeStringValue ("MainActivityMode", "Inventory");
				ds.storeStringValue ("MainActivityInventoryLocation", "Root");
				ds.storeStringValue ("ReturnLocation", levelRef.locationName);

				fader._wa_fadeOut (this);
				this.isWaitingForActionToComplete = true;

				clickedItem = UIInventoryClickItem.alphabet;

			}

			if (item == UIInventoryClickItem.progress) {

				powerCircleItem.hide (1.0f);
				alphabetItem.hide (1.25f);
				progressItem.hide (1.50f);
				backpackItem.hide (1.75f);
				powerCircleItem.disableRaycast ();
				alphabetItem.disableRaycast ();
				progressItem.disableRaycast ();
				backpackItem.disableRaycast ();

				shadowItem.show ();
				questionsItem.show ();
				wellsItem.show ();
				facebookItem.show ();
				twitterItem.show ();
				shadowItem.enableRaycast ();
				questionsItem.enableRaycast ();
				wellsItem.enableRaycast ();
				facebookItem.enableRaycast ();
				twitterItem.enableRaycast ();
				shadowsText.show ();
				questionsText.show ();
				wellsText.show ();

				status = UIInventoryState.stats;

			}

			if (item == UIInventoryClickItem.backpack) {

				powerCircleItem.hide (1.0f);
				alphabetItem.hide (1.0f);
				progressItem.hide (1.0f);
				backpackItem.hide (1.0f);
				backItem.hide (1.0f);
				back2Item.show ();
				back2Item.enableRaycast ();
				backItem.disableRaycast ();
				powerCircleItem.disableRaycast ();
				alphabetItem.disableRaycast ();
				progressItem.disableRaycast ();
				backpackItem.disableRaycast ();

				for (int i = 0; i < 7; ++i) {
					elementsCrystalItem [i].show ();
				}
				for (int i = 0; i < 7; ++i) {
					lightBridgeKeyItem [i].show ();
				}
				for (int i = 0; i < 7; ++i) {
					mapItem [i].show ();
				}
				for (int i = 0; i < 8; ++i) {
					valuesScaleItem [i].show ();
				}

				worldMapItem.show ();
				blueManaItem.show ();
				redManaItem.show ();
				blueManaText.show ();
				redManaText.show ();
				pillItem.show ();
				pillText.show ();
				worldMapItem.enableRaycast ();

				backpackInventoryAnim._wm_open ();


				status = UIInventoryState.items;

			}

		}

		if (status == UIInventoryState.stats) {

			if (item == UIInventoryClickItem.back) {

				powerCircleItem.show ();
				alphabetItem.show ();
				backpackItem.show ();
				progressItem.show ();
				powerCircleItem.enableRaycast ();
				alphabetItem.enableRaycast ();
				progressItem.enableRaycast ();
				backpackItem.enableRaycast ();


				shadowItem.hide (1.00f);
				questionsItem.hide (1.25f);
				wellsItem.hide (1.50f);
				facebookItem.hide (1.75f);
				twitterItem.hide (2.0f);
				shadowItem.disableRaycast ();
				questionsItem.disableRaycast ();
				wellsItem.disableRaycast ();
				facebookItem.disableRaycast ();
				twitterItem.disableRaycast ();
				shadowsText.hide (1.0f);
				questionsText.hide (1.25f);
				wellsText.hide (1.5f);

				status = UIInventoryState.root;


			}

		}

		if (status == UIInventoryState.items) {

			if (item == UIInventoryClickItem.worldMap) {

				DataStorage ds;
				ds = mcRef.getStorage ();
				//ds.storeIntValue ("ScaleId", integerId);
				ds.storeStringValue ("MainActivityMode", "Inventory");
				ds.storeStringValue ("MainActivityInventoryLocation", "Items");
				ds.storeStringValue ("ReturnLocation", levelRef.locationName);

				fader._wa_fadeOut (this);
				this.isWaitingForActionToComplete = true;

				clickedItem = UIInventoryClickItem.worldMap;

			}

			if (item == UIInventoryClickItem.scale) {

				DataStorage ds;
				ds = mcRef.getStorage ();
				ds.storeIntValue ("ScaleId", integerId);
				ds.storeStringValue ("MainActivityMode", "Inventory");
				ds.storeStringValue ("MainActivityInventoryLocation", "Items");
				ds.storeStringValue ("ReturnLocation", levelRef.locationName);

				fader._wa_fadeOut (this);
				this.isWaitingForActionToComplete = true;

				clickedItem = UIInventoryClickItem.scale;

			}

			if (item == UIInventoryClickItem.back) {

				backpackInventoryAnim._wm_close ();

				powerCircleItem.show ();
				alphabetItem.show ();
				backpackItem.show ();
				progressItem.show ();
				backItem.show ();
				backItem.enableRaycast ();
				powerCircleItem.enableRaycast ();
				alphabetItem.enableRaycast ();
				progressItem.enableRaycast ();
				backpackItem.enableRaycast ();

				back2Item.hide (1.0f);
				back2Item.enableRaycast ();

				for (int i = 0; i < 7; ++i) {
					elementsCrystalItem [i].hide (1.06f + 0.06f * i);
				}
				for (int i = 0; i < 7; ++i) {
					lightBridgeKeyItem [i].hide (1.42f + 0.06f * i);
				}
				for (int i = 0; i < 7; ++i) {
					mapItem [i].hide (2.15f + 0.06f * i);
				}
				for (int i = 0; i < 8; ++i) {
					valuesScaleItem [i].hide (2.75f + 0.06f * i);
				}
				blueManaItem.hide (2.0f);
				redManaItem.hide (2.06f);
				blueManaText.hide (2.03f);
				redManaText.hide (2.09f);
				pillItem.hide (2.5f);
				pillText.hide (2.5f);

				status = UIInventoryState.root;

			}

		}


	}
}
