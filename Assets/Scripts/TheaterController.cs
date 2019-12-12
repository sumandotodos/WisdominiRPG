using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TheaterController : WisdominiObject {
	
	public string folderToPlay;
	public DurationList imageDurationList;
	public float transitionDuration = 1.0f;
	public RectTransform frameTransform;
	Texture t1, t2;
	AudioClip clip;
	public float scaleSpeed;

	public RawImage raw1;
	public RawImage raw2;

	public UIFaderScript fader;

	float elapsedTime;

	int image;

	/* A lot of unorganized data that WON'T SCALE (use a new class for that), but we give a shit */

	Vector2 pos1Begin;
	Vector2 pos1End;
	float scale1Begin;
	float scale1End;
	float scale1;
	float opacity1;

	Vector2 pos2Begin;
	Vector2 pos2End;
	float scale2Begin;
	float scale2End;
	float scale2;
	float opacity2;

	float frameScale;

	float imageDuration1;
	float imageDuration2;

	bool even;
	bool transitioning;
	bool finishActivity; 

	DataStorage ds;

	string locale;

	new AudioClip audio;

	void Start () 
	{
		MasterControllerScript masterController = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = masterController.getStorage ();
		locale = masterController.getLocale ();

		//Borrar luego
		ds.storeStringValue("TheaterFolder", "Yogi1");
		folderToPlay = ds.retrieveStringValue ("TheaterFolder");

		AudioRosetta aros = GameObject.Find ("AudioRosetta").GetComponent<AudioRosetta> ();
		audio = aros.returnClip ("video/" + folderToPlay + "/audio");

		if (audio != null) 
		{
			this.GetComponent<AudioSource> ().PlayOneShot (audio);
		}

		string path = "FinalAssets/Textures/Videos/" + folderToPlay + "/0";
		t1 = Resources.Load<Texture> (path);
		path = "FinalAssets/Textures/Videos/" + folderToPlay + "/audio";
		clip = Resources.Load<AudioClip> (path);
		raw1.texture = t1;

		path = "FinalAssets/Textures/Videos/" + folderToPlay + "/1";
		t2 = Resources.Load<Texture> (path);
		if (t2 != null) {
			raw2.texture = t2;
		} else {
			raw2.color = new Color (0, 0, 0, 1);
		}

		even = true;
		transitioning = false;

		image = 0;

		frameScale = 2.2f;
		scale1 = 2.0f;

		frameTransform.localScale = new Vector2 (1.0f + frameScale, 1.0f + frameScale);

		imageDuration1 = imageDurationList.duration [0];
		if (imageDurationList.duration.Length > 1) {
			imageDuration2 = imageDurationList.duration [1];
		} else
			imageDuration2 = 1.0f;

		finishActivity = false;	
	}
	
	new void Update ()
	{
		if (finishActivity) {
			if (!isWaitingForActionToComplete) {
				string returnLocation = ds.retrieveStringValue ("ReturnLocation");
				SceneManager.LoadScene (returnLocation);
			}
		} else {
			frameScale = (frameScale * (1.0f - Time.deltaTime * scaleSpeed));
			frameTransform.localScale = new Vector2 (1.0f + frameScale, 1.0f + frameScale);

			scale1 -= scaleSpeed * Time.deltaTime / imageDuration1;
			scale2 -= scaleSpeed * Time.deltaTime / imageDuration2;

			if (!transitioning) {
				elapsedTime += Time.deltaTime;
				if (elapsedTime > imageDuration1) {
					elapsedTime = 0.0f;
					transitioning = true;
					scale2 = 2.0f;
				}
			} else {
				elapsedTime += Time.deltaTime;
				float t = elapsedTime / transitionDuration;
				if (t > 1.0f) {
					t = 1.0f;
					transitioning = false;
					elapsedTime = 0.0f;
				}
				float opacity = Mathf.Lerp (1.0f, 0.0f, t);
				raw1.color = new Color (1, 1, 1, opacity);
				if (t == 1.0f) {
					if (t2 != null) {
						++image;
						raw1.texture = t2;
						scale1 = scale2;
						raw1.color = new Color (1, 1, 1, 1);
						imageDuration1 = imageDuration2;
						string path = "FinalAssets/Textures/Videos/" + folderToPlay + "/" + (image + 1);
						t2 = Resources.Load<Texture> (path);
						if (t2 != null)
							raw2.texture = t2;
						else {
							raw2.color = new Color (0, 0, 0, 1);
							fader._wa_fadeOut (this);
							this.isWaitingForActionToComplete = true;
							finishActivity = true;
						}
						if (imageDurationList.duration.Length > image) {
							imageDuration2 = imageDurationList.duration [image];
						} else
							imageDuration2 = 1.0f;
					} 
				}
			}
			raw1.rectTransform.localScale = new Vector2 (scale1, scale1);
			raw2.rectTransform.localScale = new Vector2 (scale2, scale2);
		}
	}
}
