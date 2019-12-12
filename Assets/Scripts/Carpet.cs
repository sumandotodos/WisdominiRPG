using UnityEngine;
using System.Collections;

public class Carpet : WisdominiObject {

	Animator animator;

	new void Start() {

		animator = this.GetComponent<Animator> ();

	}

	public void _wm_open() {

		animator.SetBool ("isOpen", true);
		this.GetComponent<Collider> ().enabled = false;

	}


}
