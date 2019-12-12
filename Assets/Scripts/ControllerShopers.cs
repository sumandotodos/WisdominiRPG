using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerShopers : MonoBehaviour {

	public GameObject[] shopers;
	public GameObject[] paths;
	public GameObject[] doors;

	public AudioClip openSound;
	public AudioClip closeSound;
	public float temp;
	public float maxDist;

	LevelControllerScript lvl;
	float cont;
	int path;

	void Start () 
	{
		lvl = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		cont = temp;
	}
	
	void Update () 
	{
		cont -= Time.deltaTime;

		if (cont <= 0) 
		{
			StartCoroutine( SpawnShoper ());
			cont = temp;
		}
	}

	IEnumerator SpawnShoper()
	{
		GameObject shoper = shopers [Random.Range (0, shopers.Length)].gameObject;
		path = Random.Range (0, paths.Length);

		if (doors [path] != null)
			doors [path].GetComponent<BetterDoor2> ()._wm_open ();
		else
			if (CalculatePlayerPos())
				lvl.playSound (openSound);	

		yield return new WaitForSeconds (0.5f);
		
		shoper.GetComponent<NPCGoInPath> ().pathName = paths [path].name;
		shoper.GetComponent<NPCGoInPath> ().cont = 0;
		Instantiate (shoper, paths [path].transform.position, shoper.transform.rotation);

		yield return new WaitForSeconds (2);

		if (doors [path] != null) {	
			doors [path].GetComponent<BetterDoor2> ()._wm_close ();
		} else
			if (CalculatePlayerPos())
				lvl.playSound (closeSound);		
	}

	bool CalculatePlayerPos()
	{
		float dist = Mathf.Abs (Vector3.Distance (GameObject.Find ("Player").transform.position, paths[path].transform.position));

		if (dist > maxDist)
			return false;
		else
			return true;
	}

}
