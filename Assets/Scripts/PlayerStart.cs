using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour {

	public string groundType;
	public string music;
	public string locationName;
	public string upperFloor;
	public string bottomFloor;
	public float cameraYOverride;
	public float cameraXOverride;
	public float cameraZOverride;

	public GameObject[] cameraMarker;

	public bool skybox = false;
	public Color solidColor;


	public const float playerSpeed = 10;

	LevelControllerScript level;
	PlayerScript player;

	public bool openVignette = true;

	// Use this for initialization
	void Awake() {
		
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		level.groundType = groundType;
		level.BGMusic = music;
		//level.locationName = locationName;
		level.upperFloorName = upperFloor;
		level.lowerFloorName = bottomFloor;
		level.CameraYAngleOverride = cameraYOverride;
		level.CameraZDistOverride = cameraZOverride;
		level.CameraXAngleOverride = cameraXOverride;
		level.openVignette = openVignette;

		player = GameObject.Find ("Player").GetComponent<PlayerScript> ();
		player.SelectHUD ();
		player.speed = playerSpeed;

		Camera cam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		if (skybox) {
			cam.clearFlags = CameraClearFlags.Skybox;
		} else {
			cam.clearFlags = CameraClearFlags.Color;
			cam.backgroundColor = solidColor;
		}

		CameraManager camMan = null;
		if (cameraMarker.Length > 0) {
			camMan = GameObject.Find ("CameraLerp").GetComponent<CameraManager> ();
			camMan.marker = new GameObject[cameraMarker.Length];
			for (int i = 0; i < cameraMarker.Length; ++i) {
				camMan.marker [i] = cameraMarker [i];
			}
		}


	}

}
