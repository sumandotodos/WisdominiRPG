using UnityEngine;
using System.Collections;

public class MirrorObject : WisdominiObject {

	public LevelControllerScript level;
	public GameObject[] mirrorSurface;
	public Mirror[] mirror;
	public Color[] mirrorColor;
	public stargate[] Stargates;
	[HideInInspector]
	public PlayerScript player;
	public MirroredPlayer mirroredPlayer;
	public Material mat;
	bool mustActivateStargates = false;
	float elapsedTime;
	public int[] newLevels;

	const float StargateActivationDelay = 1.5f;

	new void Start () 
	{	
		mustActivateStargates = false;
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		player = GameObject.Find ("Player").GetComponent<PlayerScript>();
	}
	
	new void Update () 
	{

		if (player == null) 
			player = GameObject.Find ("Player").GetComponent<PlayerScript> ();

		float minDistance = 10000000.0f;
		int closestMirrorIndex = 0;

		if (player == null)
			return;

		/* get index of mirror closest to player */
		int nMirrors = mirror.Length;
		for (int i = 0; i < nMirrors; ++i) {
			float distance = (mirror [i].transform.position - player.transform.position).magnitude;
			if (distance < minDistance) {
				minDistance = distance;
				closestMirrorIndex = i;
			}
		}

		mat.SetColor ("_TintColor", mirrorColor [closestMirrorIndex]);

		if (mirroredPlayer != null)
		mirroredPlayer.mirror = mirrorSurface [closestMirrorIndex];

		if (mustActivateStargates) 
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > StargateActivationDelay) 
			{
				for(int i = 0; i<Stargates.Length; ++i) 
					Stargates [i].activate(); 
				mustActivateStargates = false;
				for (int i = 0; i < newLevels.Length; i++) {
					level.storeBoolValue ("Level" + newLevels[i] + "StargateActivated", true);
				}
			}
		}

		/*Transform[] t = mirroredPlayer.mirror = mirror [closestMirrorIndex].GetComponentsInChildren<Transform>();
		for (int i = 0; i < t.Length; ++i) {
			if (t [i].gameObject.name.Equals ("Point light")) {
				((Light)(t [i].gameObject)).color = mirrorColor [closestMirrorIndex];
			}
		}*/
	}

	public void activate(int m) 
	{
		if (m < mirror.Length) 
		{
			mirror [m].activate ();
			string lvl = level.locationName.Substring (0, 6);
			level.storeBoolValue ("Has" + lvl + mirror [m].MirrorColor + "Energy", true);
            level.mcRef.saveGame(true);
        } 
		else 
		{
			mustActivateStargates = true;
		}
	}

	public void _wm_activate(int m) 
	{
		activate (m);
	}
}
