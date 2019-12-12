using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum MirrorState { deactivated, activated, activating };
enum PlayerAutopilotState { seeking1, seeking2, seeking3, seeking4, waitingForBurst, idle };

public class Mirror : Interactor {

	/* references */

	new public Light light;
	public Color color;
	public GameObject energyBurstPrefab;
	MasterControllerScript master;
	DataStorage ds;
	public PlayerScript player;
	public HeroGlow hGlow;
	ShadowSpawnController spawnControl;
	public LevelControllerScript level;
	public MirrorAnimAux animAux;
	public Texture HeroImage;
	public WisdominiObject NewHeroProgram;
	public GameObject HeroReflection;
	public CameraManager camManager;
	public AudioClip activationSound;

	/* properties */

	public MirrorState state;
	PlayerAutopilotState autopilotState;
	public string HeroType;
	public int indexHero; // ZERO INDEX
	public string MirrorColor;
	float elapsedTime;
	public GameObject objectWithMaterial;
	public GameObject glow;
	Material mat;
	Material glowMat;
	float opacity = 0.0f;
	float tremoloAngle = 0.0f;
	MirrorHeroEnergyBurst energyBurst = null;
	public bool isDark;
	int substate;
	bool grabbingFrame = false;

	bool started = false;

	/* constants */

	const float angleSpeed = 2.0f;
	const float tremoloAngleSpeed = 88.0f;
	const float opacitySpeed = 0.26f;
	const float initialDelay = 1.5f;

	public override void effect() 
	{
		player._wa_autopilotTo(this, this.transform.position.x - 0.0f, this.transform.position.y - 0.0f, this.transform.position.z -  5.0f);
		//hGlow.glow ();
		autopilotState = PlayerAutopilotState.seeking1;
		string lvl = level.locationName.Substring (0, 6);
		level.storeBoolValue ("Has" + lvl + MirrorColor + "Energy", false);
	}

	public override string interactIcon() 
	{
		if (state == MirrorState.deactivated)
			return "";
		else
			return "Eye";
	}

	new public void Start () 
	{
		if (started)
			return;
		started = true;
		if (isDark) 
		{
			spawnControl = GameObject.Find ("ShadowSpawnController").GetComponent<ShadowSpawnController> ();
		}

		master = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		ds = master.getStorage ();
		camManager = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
		player = GameObject.Find ("Player").GetComponent<PlayerScript> ();

		opacity = 0.0f;
		state = MirrorState.deactivated;
		glowMat = glow.GetComponent<Renderer> ().material;
		light.color = new Color (color.r, color.g, color.b, 1.0f);
		light.intensity = 0.0f;
		elapsedTime = 0.0f;
		mat = objectWithMaterial.GetComponent<Renderer> ().material;
		mat.SetColor ("_EmissionColor", new Color (0, 0, 0, 0));
		glowMat.SetColor ("_TintColor", new Color (0, 0, 0, 0));

		autopilotState = PlayerAutopilotState.idle;
		string lvl = level.locationName.Substring (0, 6);
		bool depleted = ds.retrieveBoolValue (lvl + MirrorColor + "MirrorDepleted");

		if (depleted) 
		{
			interactEnabled = false;
			animAux.Broken ();
			state = MirrorState.deactivated;
		} 
		else 
		{
			bool activated = ds.retrieveBoolValue (level.locationName + MirrorColor + "MirrorActivated");

			if (activated)
				state = MirrorState.activated;
				//activate ();
		}
	}

	public void deplete() 
	{
		Start ();
		state = MirrorState.deactivated;
		interactEnabled = false;
		string lvl = level.locationName.Substring (0, 6);
		ds.storeBoolValue (lvl + MirrorColor + "MirrorDepleted", true);
		deactivate ();
		interactIcon ();
		animAux.Break ();
		this.GetComponent<SphereCollider> ().center -= Vector3.down * 200;
		//this.GetComponent<SphereCollider> ().enabled = false;
	}

	public void _wm_deplete() 
	{
		deplete ();
	}

	new void Update () 
	{
		if (autopilotState == PlayerAutopilotState.idle) 
		{

		}
		if (autopilotState == PlayerAutopilotState.seeking1) 
		{
			if (!isWaitingForActionToComplete) {
				//hGlow.glow ();
				//player._wa_autopilotTo (this, this.transform.position - new Vector3 (0, 0, 5.02f));
				player.setOrientation(4); // looking back
				autopilotState = PlayerAutopilotState.seeking2;
			}
		}
		if (autopilotState == PlayerAutopilotState.seeking2) 
		{
			if (!isWaitingForActionToComplete) {
				
				if (hGlow != null) {
					hGlow.glow ();
				}
				if (energyBurstPrefab != null) { // if mirror is light mirror

					HeroReflection.GetComponent<Renderer> ().material.mainTexture = HeroImage;
					HeroReflection.GetComponent<Renderer> ().enabled = true;
					HeroReflection.GetComponent<TextureFader> ().fadeIn ();
					//camManager.moveRelative (new Vector3(0, -0.3f, 8.0f));
					player.blockControls ();
					autopilotState = PlayerAutopilotState.waitingForBurst;
					GameObject go = (GameObject)Instantiate (energyBurstPrefab, this.gameObject.transform.position, Quaternion.Euler (0, 0, 0));
					energyBurst = go.GetComponent<MirrorHeroEnergyBurst> ();
					energyBurst.mirrorGO = this.gameObject;
					energyBurst.playerGO = player.gameObject;
					ParticleSystem s = go.GetComponent<ParticleSystem> ();
					s.startColor = new Color (color.r, color.g, color.b, 1.0f);
					energyBurst.initialize ();
					energyBurst._wm_go ();
					this.GetComponent<SphereCollider> ().center = Vector3.down * 200;
					this.state = MirrorState.deactivated;
					this.interactEnabled = false;
					deactivate ();
					substate = 0;
					//animAux.Break (); not yet
				} 

				else { // if mirror is dark mirror
					player.blockControls ();
					spawnControl.callShadows (this.transform.position);
					autopilotState = PlayerAutopilotState.seeking3;
					elapsedTime = 0.0f;
				}
			}
		}
		if (autopilotState == PlayerAutopilotState.waitingForBurst) 
		{
			if (substate == 0) 
			{
				if (energyBurst != null && energyBurst.state > 2) 
				{
					//player.unblockControls ();
					Destroy (energyBurst.gameObject);
					if (hGlow != null)
						hGlow.glow ();
					camManager.moveToOriginalPosition ();
					HeroReflection.GetComponent<TextureFader> ().fadeOut ();
					//autopilotState = PlayerAutopilotState.idle;
					++substate;

					elapsedTime = 0.0f;
				}
			}

			if (substate == 1) 
			{
				elapsedTime += Time.deltaTime;
				if (elapsedTime > 2.0f) 
				{
					++substate;
					//HeroReflection.GetComponent<Renderer> ().enabled = false;
					//this.GetComponent<SphereCollider> ().enabled = false;
					animAux.Break ();
					string lvl = level.locationName.Substring (0, 6);
					ds.storeBoolValue (lvl + MirrorColor + "MirrorDepleted", true);
					//ds.storeBoolValue ("CurrentLevelHasHero" + HeroType, true);
					ds.storeBoolValue ("Has" + lvl + HeroType, true);
					ds.storeBoolValue ("Has" + lvl + HeroType + (indexHero-1), true);
					ds.storeIntValue ("Has" + lvl + HeroType, indexHero-1);
					elapsedTime = 0.0f;
				}
			}
			if (substate == 2)
			{
				elapsedTime += Time.deltaTime;
				if (elapsedTime > 2.0f) {
					if (NewHeroProgram != null) {
						NewHeroProgram.startProgram (0);
					} else {
						player.unblockControls ();
					}
					autopilotState = PlayerAutopilotState.idle;
                    level.mcRef.saveGame(true);
				}
			}
		}
		if (autopilotState == PlayerAutopilotState.seeking3) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 12.0f) 
			{
				player.enterMirror ();
				elapsedTime = 0.0f;
				autopilotState = PlayerAutopilotState.seeking4;
			}
		}

		if (autopilotState == PlayerAutopilotState.seeking4)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1.9f && !grabbingFrame) 
			{
				level.grabFrame ();
				grabbingFrame = true;
			}
			if (elapsedTime > 2.0f) {
				level.storeStringValue ("ReturnLocation", level.locationName);
				level.storeStringValue ("CurrentMirror", level.locationName + "DarkMirrorActivated");
				level.storeStringValue ("CurrentLevel", level.locationName);
				level.loadScene ("WordFight");
			}
		}

		if (state == MirrorState.deactivated) 
		{
			interactEnabled = false;
			glowMat.SetColor ("_TintColor", new Color (0, 0, 0, 0) );
		}

		if (state == MirrorState.activating) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > initialDelay) 
			{
				state = MirrorState.activated;
				elapsedTime = 0.0f;
			}
		}

		if (state == MirrorState.activated) 
		{
			elapsedTime += Time.deltaTime;

			glowMat.SetColor ("_TintColor", new Color (color.r, color.g, color.b, opacity * (1.0f + Mathf.Cos(tremoloAngle)/12.0f) ) );

			tremoloAngle += tremoloAngleSpeed * Time.deltaTime;

			if (opacity < 0.19f) {
			
				opacity += opacitySpeed * Time.deltaTime;
			}

			light.intensity = 0.5f + 0.5f * Mathf.Sin (elapsedTime * angleSpeed);
			mat.SetColor ("_EmissionColor", color * light.intensity/8.0f);
		}
	}

	public void activate() 
	{
		if (state == MirrorState.activated) 
		{
			return;
		}

		elapsedTime = 0.0f;
		state = MirrorState.activating;
		ds.storeBoolValue (level.locationName + MirrorColor + "MirrorActivated", true);
		interactEnabled = true;
		interactIcon ();

		if (activationSound) 
		{
			level.playSound (activationSound);
		}
	}

	public void deactivate() 
	{
		elapsedTime = 0.0f;
		state = MirrorState.deactivated;
		ds.storeBoolValue (level.locationName + MirrorColor + "MirrorActivated", false);
	}

	public void _wm_activate() 
	{
		activate ();
	}

	public void _wm_deactivate()
	{
		deactivate ();
	}
}
