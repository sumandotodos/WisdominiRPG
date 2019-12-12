using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//public class TitleControllerScript : MonoBehaviour {
public class TitleControllerScript : WisdominiObject{

	public MasterControllerScript mcRef;
	public StringBank loadingStringBank;
	public Text vText;
	public AudioClip music;

	Rosetta rosetta;

	public int state;

	public float aspect;

	public Camera theCamera;

	public Text loadingText;

	public TextMesh touchToStartText;

	public HeroGlow heroGlow;

	//public GameObject fadeRef;
	public UIFaderScript fRef;
	public UILoding uiload;
	int count;

	float elapsedTimeTitle;

	float beginTime;

	const float minAspect = 1.00f;
	const float maxAspect = 2.0f;
	const float maxFOV = 72;
	const float minFOV = 60;

	// Variables de logo
	float elapsedTime;
	new const float delayTime = 4.0f;
	const float minTime = 1.6f;
	public GameObject grupoLogo;
	public GameObject tocaParaEmpezar;

	public Text debuggg;

	//Diferenciador de logo-titulo
	public string estado;

	float contador;
	bool carga = false;

//	void Awake()
//	{
//		
//	}


	// Use this for initialization
	void Start () {
		
		estado = "logo";
		elapsedTime = 0.0f;

		beginTime = 0.0f;

		mcRef = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		rosetta = GameObject.Find ("Rosetta").GetComponent<Rosetta> ();

		tocaParaEmpezar.SetActive (false);
//		loadingStringBank.rosetta = rosetta;
//		loadingStringBank.reset ();
//		loadingText.text = loadingStringBank.getNextString ();
//		touchToStartText.text = loadingStringBank.getNextString ();
//		touchToStartText.GetComponent<Renderer> ().enabled = false;
		count = 0;

		elapsedTimeTitle = 0.0f;

		state = 0;

		aspect = ((float)Screen.width) / ((float)Screen.height);

		theCamera.fieldOfView = minFOV + (1.0f - (aspect - minAspect) / (maxAspect - minAspect)) * (maxFOV - minFOV);

		if (Version.mustShowVersion ()) {
			vText.text = Version.getVersionString ();
		} else {
			vText.enabled = false;
		}

//		mcRef.setVolume (0);
//		mcRef.selectMixer (2);
//		mcRef.attachMusic (music);
		//mcRef.playMusic (2);
//		mcRef.setVolume (1, 3);

	}
	
	// Update is called once per frame
	void Update () {

		debuggg.text = "" + estado + " (" + state + ")";
		//mcRef.loadLvl.allowSceneActivation = carga;

		if (estado == "logo") 
		{

			elapsedTime += Time.deltaTime;

			if (elapsedTime > minTime) {
				if (Input.GetMouseButtonDown (0)) {
					elapsedTime = delayTime + 1.0f;
				}
			}

			if ((elapsedTime > delayTime) && state == 0) {
				//fader._wa_fadeOut (this);
				fRef.fadeOut ();
				state = 1;
			}

			if (!fRef.isFading && state == 1) 
			{
				estado = "titulo";
				grupoLogo.SetActive (false);
				fRef.fadeIn ();

				state = 0;
			}
		}

		if (estado == "titulo") 
		{
			Debug.Log ("ESTADO TITULO");
			if (state == 0) {
				if (elapsedTimeTitle < 2.0f)
					elapsedTimeTitle += Time.deltaTime;
				else
					++state;
			}
			if (state == 1) {
				heroGlow.glow ();
				tocaParaEmpezar.SetActive (true);
				loadingStringBank.rosetta = rosetta;
				loadingStringBank.reset ();
                loadingText.text = loadingStringBank.getNextString ();
				tocaParaEmpezar.GetComponent<TextMesh> ().text = loadingStringBank.getNextString ();
				//touchToStartText.text = loadingStringBank.getNextString ();
//				touchToStartText.transform.localScale = Vector3.zero;
//				touchToStartText.GetComponent<TouchToStart> ().reset ();
//				touchToStartText.GetComponent<Renderer> ().enabled = true;
				++state;
			}

			++count;
			if (count == 10) {
				//fRef = fadeRef.GetComponent<FaderScript> ();

				//fRef.setFadeValue (0.0f);

				//fRef.fadeIn ();
			}

			if (Input.GetMouseButtonDown (0) && !tocaParaEmpezar.GetComponent<TouchToStart>().wait) { // if screen touched
		
				fRef.fadeOut (); // start a fadeout
				beginTime = Time.time;
				mcRef.setVolume (0, 1.5f);

			}

			if ((beginTime > 0.0f) && (Time.time - beginTime) > 2.0) { // wait for 2 seconds

				vText.enabled = false;
				uiload.load ();
				//this.activityFinish ();
				estado = "cargando";
				mcRef.LoadAsync ();
				contador = 2;
			}
		}

		if (estado == "cargando") 
		{
			Debug.Log ("ESTADO CARGANDO");
			contador -= Time.deltaTime;
			if (contador <= 0) 
			{
				this.activityFinish ();
			}
		}

	}

	void activityFinish() 
	{
		carga = true;
		//mcRef.loadLvl.allowSceneActivation = true;
	}
}
