using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGoInPath : MonoBehaviour {

	public float speed;
	public bool stop = false;
	public float cont;
	public string pathName;
	CharacterGenerator cg;
	GameObject player;

	void Start ()
	{
		cg = this.GetComponent<CharacterGenerator> ();
		player = GameObject.Find ("Player");

		Physics.IgnoreCollision(this.GetComponent<Collider>(), player.GetComponent<Collider> ());

		if (pathName != "")
			StartCoroutine (FollowPath());

		switch (pathName) {
		case "Ruta01":
			cg.walkingSpeed = 10;
			speed = 0.05f;
			break;

		case "Ruta02":
			cg.walkingSpeed = 10; // ?
			speed = 0.04f;
			break;
		}
	}

	void Update ()
	{
		if (pathName != "")
		{
			if (cont < 1)
				cont += Time.deltaTime * speed;
			else
				cont = 1;
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.transform.tag == "NPCAuto")
			Physics.IgnoreCollision (this.GetComponent<Collider> (), col.gameObject.GetComponent<Collider> ());
	}

	IEnumerator FollowPath()
	{
		do {			
			Vector3 pathPoint = iTween.PointOnPath (iTweenPath.GetPath (pathName), cont);
			cg.autopilotTo (pathPoint.x, pathPoint.z);

			yield return new WaitForSeconds (0.01f);
		} while (Vector3.Distance(this.transform.position, iTween.PointOnPath(iTweenPath.GetPath(pathName), 1)) > 1);

		yield return new WaitForSeconds(0.5f);

		Destroy (this.gameObject);
	}
}
