using UnityEngine;
using System.Collections;

public class MirrorAnimAux : MonoBehaviour {

	public GameObject unbroken;
	public GameObject broken;

	bool started = false;

	void Start () 
	{
		if (started)
			return;
		started = true;
		unbroken.SetActive (true);
		broken.SetActive (false);
	}

	public void Break() 
	{
		started = true;
		unbroken.SetActive (false);
		broken.SetActive (true);
		Animator anim = broken.transform.parent.gameObject.GetComponent<Animator> ();
		anim.SetBool ("isBroken", true);
	}

	public void Broken()
	{
		started = true;
		unbroken.SetActive (false);
		broken.SetActive (true);
		Animator anim = broken.transform.parent.gameObject.GetComponent<Animator> ();
		anim.SetBool ("broken", true);
	}
	

}
