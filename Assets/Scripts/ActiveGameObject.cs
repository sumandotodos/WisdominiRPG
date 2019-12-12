using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGameObject : WisdominiObject {

	GameObject grupo;

	new void Start () 
	{
		grupo = this.gameObject;
		//grupo.SetActive (false);
	}

	public void _wm_Active()
	{
		grupo.SetActive(true);
	}

	public void _wm_Deactive() {
		grupo.SetActive (false);
	}
}
