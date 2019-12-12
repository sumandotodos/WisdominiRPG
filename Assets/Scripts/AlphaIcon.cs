using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaIcon : MonoBehaviour {

	public GameObject target;
	SpriteRenderer sr;

	public float temp;
	float cont;

	void Start () 
	{
		target = this.gameObject;
		sr = this.GetComponent<SpriteRenderer> ();
	}
	
	void Update () 
	{
		cont -= Time.deltaTime;

		if (cont <= 0) {
			StartCoroutine (AlphaAnim ());
			cont = float.MaxValue;
		}
	}

	IEnumerator AlphaAnim()
	{
		iTween.FadeTo (target, 0, 1);

		yield return new WaitForSeconds (1.5f);
		
		iTween.FadeTo (target, 1, 1);

		yield return new WaitForSeconds(1.5f);

		iTween.FadeTo (target, 0, 1);

		yield return new WaitForSeconds (1.5f);

		iTween.FadeTo (target, 1, 1);

		yield return new WaitForSeconds(3f);

		cont = temp;
	}
}
