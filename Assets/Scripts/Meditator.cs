using UnityEngine;
using System.Collections;

enum MeditatorState { canMeditate, cannotMeditate };
enum MeditationState { idle, waitingForChispa, meditating, meditated };

public class Meditator : Interactor {

	/* constants */

	const int minMeditationDuration = 3;
	const int maxMeditationDuration = 8;
	Color immatureColor = Color.green;
	Color ripeColor = Color.red;
	const float minFruitScale = 0.3f;
	const float maxFruitScale = 1.0f;



	/* references */

	public WisdominiObject effectProgram; // chispa guidance program
	public Fruit fruit;
	public PlayerScript player;
	public LevelControllerScript level;
	public UIBreather breather;


	/* properties */

	float fruitScale;
	MeditationState meditationState = MeditationState.idle;
	float timeToCompletion = 2.0f;
	public float meditationTime = 2.0f;
	public float darkMeditationTime = 5.0f;
	public bool dark;  // are we meditating in plane -1??
					   //  if so, just ascend, no meditation subgame

	public string InteractIcon;

	/* methods */

	new void Start () 
	{	
		reset ();
	}


	new void reset() 
	{
		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		if (player == null)
			player = GameObject.Find ("Player").GetComponent<PlayerScript> ();

		if (dark)
			timeToCompletion = darkMeditationTime;
			else timeToCompletion = meditationTime; // 40 seconds //(float)Random.Range (minMeditationDuration, maxMeditationDuration);

		breather = GameObject.Find ("Breather").GetComponent<UIBreather> ();

		meditationState = MeditationState.idle;
		fruit.reset ();	
	}
	
	new void Update () 
	{
		/*if (meditationStatus == MeditationStatus.meditating)
		 * {
			elapsedTime += Time.deltaTime;
			if (tParam > 1.0f)
				meditationStatus = MeditationStatus.meditated;
			player.ascend ();
		}*/

		if (meditationState == MeditationState.waitingForChispa) 
		{
			if (dark) {
				player.meditate ();
				level.musicFadeOut (5.0f);
				meditationState = MeditationState.meditating;
				fruit._wa_grow (this, timeToCompletion);
			} else {
				if (effectProgram != null) {
					if (effectProgram.isProgramRunning [0]) // wait for chispa
					return;
				}

				fruit._wa_grow (this, timeToCompletion);
				breather.activate ();
				player.meditate ();
				level.musicFadeOut (10.0f);
				meditationState = MeditationState.meditating;
			}

		}

		if (meditationState == MeditationState.meditating) {

			if (isWaitingForActionToComplete)
				return;

			player.ascend ();
			level.musicFadeIn ();
			meditationState = MeditationState.meditated;


		}

		if (meditationState == MeditationState.meditated) 
		{

		}	
	}

	/*void OnTriggerEnter(Collider other) {

		if (other.tag == "Player") {
			meditationStatus = MeditationStatus.meditating;
			player = other.gameObject.GetComponent<PlayerScript> ();
			player.meditate ();
		}

	}*/

	override public void effect() 
	{
		meditationState = MeditationState.waitingForChispa;
		player.blockControls ();
		player.setOrientation (0);

		if (!dark) {
			if (effectProgram != null) {
				effectProgram.startProgram (0);
			} 
		}
		else
			player.meditate ();
	}

	override public string interactIcon() 
	{
		return InteractIcon;
	}

}


