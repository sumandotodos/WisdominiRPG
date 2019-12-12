using UnityEngine;
using System.Collections;

public class MirroredPlayer : MonoBehaviour {

	/* references */

	public GameObject mirror;
	public PlayerScript player;
	public GameObject spriteQuad;

	SpriteRenderer mat;

	void Start () 
	{	
		mat = spriteQuad.GetComponent<SpriteRenderer> ();
		player = GameObject.Find ("Player").GetComponent<PlayerScript> ();
	}

	void Update () 
	{	
		Vector3 newPos;
		if (player == null)
			return;
		if (mirror != null) {
			newPos = player.transform.position;
			newPos.z = -(player.transform.position.z - mirror.transform.position.z) + mirror.transform.position.z;
			if (newPos.z < player.transform.position.z)
				mat.color = new Color (0, 0, 0, 0);
			else
				mat.color = new Color (0.7f, 0.8f, 0.9f, 0.7f);
			this.transform.position = newPos;
			Sprite t = player.getMirroredTexture ();
			mat.sprite = t;
		}
	}
}
