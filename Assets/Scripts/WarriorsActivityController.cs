using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum WarriorsActivityState { Categories, FromCategoriesToClass, Class, FromClassToIndividual, Individual, FromIndividualToClass, FromClassToCategories, exitting };

public class WarriorsActivityController : WisdominiObject {

	/* constants */

	int numberOfCategories = 7;
	//int[] numberOfIndividualsInClass = { 6, 6, 6, 3, 5, 3 } ;

	/* references */

	/*public Texture[] warriorBig;
	public Texture[] masterBig;
	public Texture[] philoBig;
	public Texture[] sageBig;
	public Texture[] explorerBig;
	public Texture[] wizardBig;
	public Texture[] yogiBig;
	*/

	public HeroEffect[] heroEffect;

	public Text textHeaderRef;
	public Text descriptionRef;
	public Text individualNameRef;
	public Text individualDescrRef;
	new public Rosetta rosetta;
	public UIFaderScript fader;
	public Sprite[] Categories;
	public int[] IndividualsInClass;
	public Sprite[] Individuals;
	public string[] ClassNames;
	public string[] ClassDescription;
	public string[] IndividualNames;
	public string[] IndividualDescriptions;
	public GameObject[] spriteHolders; // these must have WarriorIcon script attached
	public BigHero bigHero;
	MasterControllerScript mcRef;

	/* properties */

	WarriorsActivityState state;
	int currentClass = -1;
	int currentIndividual = -1;
	float textHeaderOpacity = 0.0f;
	float targetTextHeaderOpacity = 0.0f;
	float textDescrOpacity = -0.5f;
	float targetTextDescrOpacity = -0.5f;
	float individualNameOpacity = 0.0f;
	float targetIndNameOpacity = 0.0f;
	float individualDescOpacity = -0.5f;
	float targetIndDescOpacity = -0.5f;

	/* methods */

	new void Start () 
	{	
		textHeaderRef.text = "";
		textHeaderRef.color = new Vector4 (1, 1, 1, 0);
		descriptionRef.text = "";
		descriptionRef.color = new Vector4 (1, 1, 1, 0);
		individualNameRef.text = "";
		individualNameRef.color = new Vector4 (1, 1, 1, 0);
		individualDescrRef.text = "";
		individualDescrRef.color = new Vector4 (1, 1, 1, 0);
		state = WarriorsActivityState.Categories;
		if (rosetta == null) {
			rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();
		}
		isWaitingForActionToComplete = false;
		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
	}
	
	new void Update ()
	{
		/* change icons */
		if (state == WarriorsActivityState.exitting) 
		{
			if (!isWaitingForActionToComplete) 
			{				
				// load level
				if (mcRef == null)
					return;

				DataStorage ds = mcRef.getStorage ();
				string returnLevel = ds.retrieveStringValue ("ReturnLocation");
				if (!returnLevel.Equals ("")) {
					SceneManager.LoadScene (returnLevel);
				}				
			}
		}

		if (state == WarriorsActivityState.FromClassToCategories) 
		{
			WarriorIcon w;
			w = spriteHolders [0].GetComponent<WarriorIcon> (); // Any sprite holder will do. We choose 0
			if (w.status == warriorIconState.folded) 
			{
				// change icon sprites here!!!!
				int nItems = numberOfCategories;
				for (int i = 0; i < nItems; ++i) {
					w = spriteHolders [i].GetComponent<WarriorIcon> ();
					w.setLevel (0, -1);
					w.setNumberOfElements (numberOfCategories);
					w.status = warriorIconState.unfolding;
					spriteHolders[i].GetComponent<Image>().sprite = Categories[i];
					spriteHolders [i].GetComponent<WarriorIcon> ().Index = i;
				}
				state = WarriorsActivityState.Categories;
			}
		}

		if (state == WarriorsActivityState.FromCategoriesToClass) 
		{
			WarriorIcon w;
			w = spriteHolders [0].GetComponent<WarriorIcon> (); // Any sprite holder will do. We choose 0
			if (w.status == warriorIconState.folded) 
			{
				// change icon sprites here!!!!
				int sprOffset = 0;
				int curClass = 0;
				while(curClass != currentClass) 
				{
					sprOffset += IndividualsInClass[curClass++];
				}

				int nItems = IndividualsInClass[currentClass];
				for (int i = 0; i < nItems; ++i) {
					w = spriteHolders [i].GetComponent<WarriorIcon> ();
					w.setLevel (1, currentClass);
					w.setNumberOfElements (nItems);
					w.status = warriorIconState.unfolding;
					spriteHolders[i].GetComponent<Image>().sprite = Individuals[sprOffset++];
				}
				state = WarriorsActivityState.Class;
				textHeaderRef.text = rosetta.retrieveString ("WarriorsActContClassNames" + currentClass);
				descriptionRef.text = rosetta.retrieveString ("WarriorsActContClassDesc" + currentClass);
				targetTextHeaderOpacity = 1.0f;
				targetTextDescrOpacity = 1.0f;
			}
		}

		if (textHeaderOpacity < targetTextHeaderOpacity) 
		{
			textHeaderOpacity += Time.deltaTime;
			if (textHeaderOpacity > targetTextHeaderOpacity)
				textHeaderOpacity = targetTextHeaderOpacity;
			textHeaderRef.color = new Vector4 (1, 1, 1, textHeaderOpacity);
		}

		if (textHeaderOpacity > targetTextHeaderOpacity) 
		{
			textHeaderOpacity -= Time.deltaTime;
			if (textHeaderOpacity < targetTextHeaderOpacity)
				textHeaderOpacity = targetTextHeaderOpacity;
			textHeaderRef.color = new Vector4 (1, 1, 1, textHeaderOpacity);
		}

		if (textDescrOpacity < targetTextDescrOpacity) 
		{
			textDescrOpacity += Time.deltaTime;
			if (textDescrOpacity > targetTextDescrOpacity)
				textDescrOpacity = targetTextDescrOpacity;
			descriptionRef.color = new Vector4 (1, 1, 1, textDescrOpacity);
		}

		if (textDescrOpacity > targetTextDescrOpacity) 
		{
			textDescrOpacity -= Time.deltaTime;
			if (textDescrOpacity < targetTextDescrOpacity)
				textDescrOpacity = targetTextDescrOpacity;
			descriptionRef.color = new Vector4 (1, 1, 1, textDescrOpacity);
		}

		if (individualNameOpacity < targetIndNameOpacity) 
		{
			individualNameOpacity += Time.deltaTime;
			if (individualNameOpacity > targetIndNameOpacity)
				individualNameOpacity = targetIndNameOpacity;
			individualNameRef.color = new Vector4 (1, 1, 1, individualNameOpacity);
		}

		if (individualNameOpacity > targetIndNameOpacity) 
		{
			individualNameOpacity -= Time.deltaTime;
			if (individualNameOpacity < targetIndNameOpacity)
				individualNameOpacity = targetIndNameOpacity;
			individualNameRef.color = new Vector4 (1, 1, 1, individualNameOpacity);
		}

		if (individualDescOpacity < targetIndDescOpacity) 
		{
			individualDescOpacity += Time.deltaTime;
			if (individualDescOpacity > targetIndDescOpacity)
				individualDescOpacity = targetIndDescOpacity;
			individualDescrRef.color = new Vector4 (1, 1, 1, individualDescOpacity);
		}

		if (individualDescOpacity > targetIndDescOpacity) 
		{
			individualDescOpacity -= Time.deltaTime;
			if (individualDescOpacity < targetIndDescOpacity)
				individualDescOpacity = targetIndDescOpacity;
			individualDescrRef.color = new Vector4 (1, 1, 1, individualDescOpacity);
		}	
	}

	public void openClass(int cId) 
	{
		if (state == WarriorsActivityState.Categories)
		{
			state = WarriorsActivityState.FromCategoriesToClass;
		}

		currentClass = cId;
	}

	public void openIndividual(int iId) 
	{

	}

	public void goBack() 
	{
		if(state == WarriorsActivityState.Individual) 
		{
			for (int i = 0; i < spriteHolders.Length; ++i) {
				spriteHolders [i].GetComponent<WarriorIcon> ().fadein ();
			}
			state = WarriorsActivityState.Class;
			targetIndNameOpacity = 0.0f;
			targetIndDescOpacity = -0.5f;
			return;
		}

		if(state == WarriorsActivityState.Class) 
		{
			for (int i = 0; i < IndividualsInClass[currentClass]; ++i) 
			{
				/* tell the warriors to fold */
				WarriorIcon w;
				w = spriteHolders [i].GetComponent<WarriorIcon> ();
				w.status = warriorIconState.folding;
			}
			targetTextDescrOpacity = -0.5f;
			targetTextHeaderOpacity = 0.0f;
			state = WarriorsActivityState.FromClassToCategories;
			bigHero.reset ();
			heroEffect [currentClass].reset ();
		}

		if(state == WarriorsActivityState.Categories) 
		{
			for (int i = 0; i < numberOfCategories; ++i) 
			{
				WarriorIcon w;
				w = spriteHolders [i].GetComponent<WarriorIcon> ();
				w.status = warriorIconState.folding;
			}
			state = WarriorsActivityState.exitting;
			fader._wa_fadeOut (this);
			this.isWaitingForActionToComplete = true;
		}
	}

	public void advance(int id) 
	{
		WarriorIcon icn = spriteHolders [id].GetComponent<WarriorIcon> ();
		if (icn.globalOpacity < 1.0f)
			return; // Awkward way to check, but works perfectly. Why not do things like these??

		if (state == WarriorsActivityState.Categories) 
		{
			currentClass = id;
			for (int i = 0; i < numberOfCategories; ++i)
			{
				WarriorIcon w;
				w = spriteHolders [i].GetComponent<WarriorIcon> ();
				w.status = warriorIconState.folding;
			}

			state = WarriorsActivityState.FromCategoriesToClass;

			heroEffect [id].effect ();
			bigHero.fadeIn ();

			// small ad-hoc fix
			if (id == 6) { // a yogi
				bigHero.finalY = 120;
			} else {
				bigHero.finalY = 80;
			}

			bigHero.scrollIn ();
		}

		if (state == WarriorsActivityState.Class) 
		{
			currentIndividual = id;
			int nItems = IndividualsInClass[currentClass];
			for (int i = 0; i < nItems; ++i) {
				if (i != currentIndividual)
					spriteHolders [i].GetComponent<WarriorIcon> ().fadeout();
			}
			int sprOffset = 0;
			int curClass = 0;
			while(curClass != currentClass) {
				sprOffset += IndividualsInClass[curClass++];
			}
			individualNameRef.text = rosetta.retrieveString ("WarriorsActContIndNames" + (currentIndividual + sprOffset));
				individualDescrRef.text = rosetta.retrieveString ("WarriorsActContIndDesc" + (currentIndividual + sprOffset));
			targetIndNameOpacity = 1.0f;
			targetIndDescOpacity = 1.0f;
			state = WarriorsActivityState.Individual;
		}
	}
}
