using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBarrierController : WisdominiObject {

	public SoftMover[] energyRods;
	public SoftMover[] energyRings;
	public GameObject colliderBarrier;
	public Light theLight;



	public bool reentrant = true;

	float lightIntensity;

	int solvedRods = 0;

	LevelControllerScript level;

	// Use this for initialization
	void Start () {
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		solvedRods = level.retrieveIntValue (this.name + "SolvedRods");
		_wm_reset ();
	}

	public void _wm_reset() {
		
		theLight.intensity = 0;
		colliderBarrier.SetActive (false);
		int r = 0;
		foreach (SoftMover sm in energyRods) {
			sm.gameObject.SetActive ((r >= solvedRods));
			r++;
			sm.reset ();
		}
		foreach (SoftMover sm in energyRings) {
			sm.gameObject.SetActive (false);
			sm.reset ();
		}
		if (level.retrieveBoolValue (this.name + "Init"))
			_wm_go ();

	}

	public void _wm_go() {
		colliderBarrier.SetActive (true);
		level.storeBoolValue (this.name + "Init", true);
		lightIntensity = 16 + 8 * (energyRods.Length - solvedRods);
		if (energyRods.Length == solvedRods) {
			lightIntensity = 0;
			colliderBarrier.SetActive (false);
		} else {
			foreach (SoftMover sm in energyRings) {
				sm.gameObject.SetActive (true);
			}
		}
		foreach (SoftMover sm in energyRods) {
			sm._wm_move ();
		}

		updateLightIntensity ();
	}

	private void updateLightIntensity() {
		theLight.intensity = lightIntensity;
	}

	public void _wm_step() {
		energyRods [solvedRods]._wm_unmove ();
		++solvedRods;
		if (reentrant)
			level.storeIntValue (this.name + "SolvedRods", solvedRods);
		lightIntensity -= 8;
		
		if (solvedRods == energyRods.Length) {
			foreach (SoftMover sm in energyRings) {
				sm.gameObject.SetActive (false);
				sm.reset ();
				lightIntensity = 0;
			}
		}

		updateLightIntensity ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
