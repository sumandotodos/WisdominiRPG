using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum LetterState { fadingInScrollingUp, fadingInScrollingDown, idle, 
	fadingOutScrollingUp, fadingOutScrollingDown, steppingUp, steppingDown };

public class Letter : MonoBehaviour {

	/* references */


	public Texture[] letter;
	public Texture[] litLetter;
	public RawImage letter1;
	public RawImage letter2;
	public RawImage frame;
	float targetColor; // targetColor is for letter1
	float targetOpacity; // targetOpacity is for letter2
	int targetLetterNum;
	float letter1color, letter2opacity;
	float globalOpacity;
	float targetGlobalOpacity;
	float idleElapsedTime;
	public LevelControllerScript level;
	public UIFaderScript fader;

	/* public properties */


	public int letterToDisplay;
	//int displayedLetter;
	public float fullHeight = 100.0f;
	float changeSpeed = 0.6f;


	/* constants */

	float ySpeed = 40.0f;
	const float opacitySpeed = 0.6f;
	const int nLetters = 26;
	const float idleMaxTime = 3.0f;
	const float looseScreenDelay = 3.5f;

	/* properties */

	LetterState state; // slot 0 state
	int state2; // slot 1 state
	float y1, y2;
	//RectTransform r1, r2;
	float opacity;
	int lettersToDec;
	[HideInInspector]
	static public int substeps = 6;
	int step;// = substeps/2;
	int letterNum;// = (nLetters/2);
	int globalStep;
	int globalTargetStep;
	int maxGlobalStep;
	float slot1elapsedTime;




	/* methods */


	// Use this for initialization
	void Start () {

		if (level == null)
			level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		bool alphabetInitialized = level.retrieveBoolValue ("AlphabetInitialized");
		if (!alphabetInitialized) {
			letterNum = nLetters / 2;
			step = (substeps / 2) - 1;
			int gs = letterNum * substeps + step;
			level.storeIntValue ("AlphabetGlobalStep", gs);
			level.storeBoolValue ("AlphabetInitialized", true);
		} else {
			globalStep = level.retrieveIntValue ("AlphabetGlobalStep");
			letterNum = globalStep / substeps;
			step = globalStep % substeps;
		}

		targetGlobalOpacity = globalOpacity = 0.0f;
		idleElapsedTime = 0.0f;
	
		state = LetterState.idle;
		state2 = 0;

		slot1elapsedTime = 0.0f;

		if (letterToDisplay >= letter.Length)
			letterToDisplay = 0;
		
		letter1.color = new Color (1, 1, 1, 1);
		letter2.color = new Color (1, 1, 1, 0);
		letter1color = 1.0f;
		letter2opacity = 0.0f;
		letter1.texture = letter [letterToDisplay];


		step = (substeps / 2)-1;

		lettersToDec = 0;
		targetLetterNum = letterNum;
		globalStep = letterNum * substeps + (substeps/2)-1;
		globalTargetStep = globalStep;
		maxGlobalStep = nLetters * substeps - 1;
		letter1.texture = letter [letterNum];
		letter2.texture = litLetter [letterNum];

		// consume reward
		int reward = level.retrieveIntValue ("AlphabetReward");
		if (reward > 0)
			addStep (reward);
		else
			subStep (-reward);
		level.storeIntValue ("AlphabetReward", 0);
		if (reward != 0) {
			level.storeIntValue ("AlphabetGlobalStep", globalTargetStep);
		}

	}

	public int getStep(int globalStep) {

		return globalStep % substeps;

	}
		

	public void incStep() {

		if (globalStep == maxGlobalStep)
			return;
		if (globalTargetStep == maxGlobalStep)
			return;

		++globalTargetStep;
		level.storeIntValue ("AlphabetGlobalStep", globalTargetStep);

		targetGlobalOpacity = 1.0f;
		idleElapsedTime = 0.0f;

	}

	public void decStep() {

		if (globalStep == 0)
			return;
		if (globalTargetStep == 0) {
			return;
		}

		--globalTargetStep;
		level.storeIntValue ("AlphabetGlobalStep", globalTargetStep);

		targetGlobalOpacity = 1.0f;
		idleElapsedTime = 0.0f;

	}

	public void addStep(int s) {

		while (s > 0) {
			incStep ();
			--s;
		}

	}

	public void subStep(int s) {

		while (s > 0) {
			decStep ();
			--s;
		}

	}

	void setColorOpacityTargets(int s) {
		if (s < (substeps / 2)) { // first half
			float percent = (((float)s + 1.0f) / (((float)substeps / 2.0f)));
			targetColor = percent;
			targetOpacity = 0.0f;
		} else { // second half
			float percent = (((float)s - ((float)substeps)/2.0f + 1.0f) / (((float)substeps) / 2.0f));
			targetColor = 1.0f;
			targetOpacity = percent;
		}
	}

	// Update is called once per frame
	void Update () {

		float ySize;


		/* slot 1 */
		if (state2 == 0) { // idle

		}
		if (state2 == 1) { // delay
			slot1elapsedTime += Time.deltaTime;
			if (slot1elapsedTime > looseScreenDelay) {
				++state2;
			}
		}
		if (state2 == 2) {
			level.loadScene ("LevelOver");
		}
		/* end of slot 1 */





		/* slot 0 */
		if ((globalStep == 0) && (state2==0)) {
			state2 = 1;
			level.musicFadeOut ();
			fader.fadeOut ();
		}
		if (state == LetterState.idle) { // We set target1col&target2op Here
			if (globalStep < globalTargetStep) { // need to go UP!
				
				if (step == (substeps - 1)) {
					setColorOpacityTargets (0);
					opacity = 1.0f;
					state = LetterState.fadingOutScrollingUp;
				} else {
					setColorOpacityTargets (step + 1);
					state = LetterState.steppingUp;
				}
			} else if (globalStep > globalTargetStep) { // need to go DOWN!
				
				if (step == 0) {
					setColorOpacityTargets (substeps - 1);
					opacity = 1.0f;
					state = LetterState.fadingOutScrollingDown;
				} else {
					setColorOpacityTargets (step - 1);
					state = LetterState.steppingDown;
				}
			} else {
				if(idleElapsedTime < idleMaxTime) {
					idleElapsedTime += Time.deltaTime;
					if (idleElapsedTime > idleMaxTime) {
						targetGlobalOpacity = 0.0f;
					}
				}
			}
		}
		

		if (state == LetterState.fadingInScrollingDown) { // when a letter disappears and the next
			// appears

			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > 1.0f) {
				--globalStep;
				step = getStep (globalStep);
				state = LetterState.idle;
				opacity = 1.0f;
			}

			//letter1.color = new Color (1.0f, 1.0f, 1.0f, opacity);
			//letter2.color = new Color (1.0f, 1.0f, 1.0f, opacity);
			letter1color = opacity;
			letter2opacity = opacity;

		}

		if (state == LetterState.fadingOutScrollingDown) {

			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < 0.0f) {
				state = LetterState.fadingInScrollingDown;
				--letterNum;
				opacity = 0.0f;
				letter1.color = new Color (1.0f, 1.0f, 1.0f, opacity);
				letter2.color = new Color (1.0f, 1.0f, 1.0f, opacity);
				letter1.texture = letter [letterNum]; // change letter
				letter2.texture = litLetter [letterNum];

			}
			float percent = (1.0f / (substeps / 2));
			//letter1.color = new Color (percent, percent, percent, opacity);
			////letter2.color = new Color (1.0f, 1.0f, 1.0f, opacity);
			letter1color = opacity * percent;
			letter2opacity = 0.0f;

		}

		if (state == LetterState.fadingInScrollingUp) { // when a letter disappears and the next
														// appears

			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > 1.0f) {
				++globalStep;
				step = getStep (globalStep);
				state = LetterState.idle;
			}
			float percent = (1.0f / ((float)substeps / 2.0f));
			letter1color = percent;
			letter2opacity = 0.0f;
			//letter1.color = new Color (percent, percent, percent, opacity);
			//letter2.color = new Color (0, 0, 0, 0);

		}

		if (state == LetterState.fadingOutScrollingUp) {

			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < 0.0f) {
				state = LetterState.fadingInScrollingUp;
				++letterNum;
				opacity = 0.0f;
				//letter1.color = new Color (1.0f, 1.0f, 1.0f, opacity);
				//letter2.color = new Color (1.0f, 1.0f, 1.0f, opacity);
				letter1.texture = letter [letterNum]; // change letter
				letter2.texture = litLetter [letterNum];

			}
			//letter1.color = new Color (1.0f, 1.0f, 1.0f, opacity);
			//letter2.color = new Color (1.0f, 1.0f, 1.0f, opacity);
			letter1color = opacity;
			letter2opacity = opacity;

		}

		if (state == LetterState.steppingUp) {

			if (letter1color < targetColor) {
				letter1color += changeSpeed * Time.deltaTime;
				if (letter1color > targetColor) {
					letter1color = targetColor;
					++globalStep;
					step = getStep (globalStep);
					state = LetterState.idle;
				}
			}
			if (letter2opacity < targetOpacity) {
				letter2opacity += changeSpeed * Time.deltaTime;
				if (letter2opacity > targetOpacity) {
					letter2opacity = targetOpacity;
					++globalStep;
					step = getStep (globalStep);
					state = LetterState.idle;
				}
			}

		}

		if (state == LetterState.steppingDown) {

			if (letter1color > targetColor) {
				letter1color -= changeSpeed * Time.deltaTime;
				if (letter1color < targetColor) {
					letter1color = targetColor;
					--globalStep;
					step = getStep (globalStep);
					state = LetterState.idle;
				}
			}
			if (letter2opacity > targetOpacity) {
				letter2opacity -= changeSpeed * Time.deltaTime;
				if (letter2opacity < targetOpacity) {
					letter2opacity = targetOpacity;
					--globalStep;
					step = getStep (globalStep);
					state = LetterState.idle;
				}
			}

		}

		letter1.color = new Color (letter1color, letter1color, letter1color, letter1color*globalOpacity);
		letter2.color = new Color (1, 1, 1, letter2opacity*globalOpacity);
		frame.color = new Color (1, 1, 1, globalOpacity);

		if (globalOpacity < targetGlobalOpacity) {
			globalOpacity += opacitySpeed * 2.0f * Time.deltaTime;
			if (globalOpacity > targetGlobalOpacity) {
				globalOpacity = targetGlobalOpacity;
			}
		}
		if (globalOpacity > targetGlobalOpacity) {
			globalOpacity -= opacitySpeed * 2.0f * Time.deltaTime;
			if (globalOpacity < targetGlobalOpacity) {
				globalOpacity = targetGlobalOpacity;
			}
		}
		/* end of slot 0 */

	
	}

	void setTopToDown() { // letter2 shown

		Vector2 sizeDelta;
		Rect r;

		sizeDelta = new Vector2 (fullHeight * 1.0f, fullHeight * 0.0f);
		r = new Rect ();
		r.x = 1;
		r.y = 0;
		r.width = 1;
		r.height = 1;

		//letter1.uvRect = r;
		//r1.sizeDelta = sizeDelta;


		sizeDelta = new Vector2 (fullHeight * 1.0f, fullHeight * 1.0f);
		r = new Rect ();
		r.x = 1;
		r.y = 0;
		r.width = 1;
		r.height = 1;

		//letter2.uvRect = r;
		//r2.sizeDelta = sizeDelta;

	}

	void setDownToTop() { // letter1 shown

		Vector2 sizeDelta;
		Rect r;

		sizeDelta = new Vector2 (fullHeight * 1.0f, fullHeight * 1.0f);
		r = new Rect ();
		r.x = 1;
		r.y = 0;
		r.width = 1;
		r.height = 1;

		//letter1.uvRect = r;
		//r1.sizeDelta = sizeDelta;


		sizeDelta = new Vector2 (fullHeight * 1.0f, fullHeight * 0.0f);
		r = new Rect ();
		r.x = 1;
		r.y = 1;
		r.width = 1;
		r.height = 1;

		//letter2.uvRect = r;
		//r2.sizeDelta = sizeDelta;

	}

	public void incLetter() { // current letter in letter2, next letter in letter1

		if (letterToDisplay < (letter.Length-1)) {

			letter2.texture = litLetter [letterToDisplay];
			letter1.texture = letter [letterToDisplay + 1];
			setTopToDown ();
			++letterToDisplay;
			y1 = 0.0f;
			y2 = fullHeight * 1.0f;
			state = LetterState.fadingInScrollingUp;

		}

	}

	public void decLetterHelper() {

		if (letterToDisplay > 0) {

			letter2.texture = litLetter [letterToDisplay - 1];
			letter1.texture = letter [letterToDisplay];
			setDownToTop ();
			--letterToDisplay;
			y1 = fullHeight * 1.0f;
			y2 = 0.0f;

		}

	}

	public void decLetter() { // current letter in letter1, next letter in letter2

		if (state == LetterState.idle) {

			if (letterToDisplay > 0) {

				decLetterHelper ();
				state = LetterState.fadingInScrollingDown;

			}
		} else
			++lettersToDec;


	}
}
