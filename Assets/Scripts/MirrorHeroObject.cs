using UnityEngine;
using System.Collections;

public class MirrorHeroObject : MonoBehaviour {

	public Texture[] warriors;
	public Texture[] masters;
	public Texture[] philosophers;
	public Texture[] sages;
	public Texture[] explorers;
	public Texture[] wizards;
	public Texture[] yogis;

	public int indexFromClassName(string className) {

		if(className.Equals("Warrior"))
			return 0;

		if(className.Equals("Master"))
			return 1;

		if (className.Equals ("Philosopher"))
			return 2;

		if (className.Equals ("Sage"))
			return 3;

		if (className.Equals ("Explorer"))
			return 4;

		if (className.Equals ("Wizard"))
			return 5;

		if (className.Equals ("Yogi"))
			return 6;

		return -1;

	}

	public Texture[] texArrayFromIndex(int index) {

		switch (index) {

			case 0:
				return warriors;

			case 1:
				return masters;

			case 2:
				return philosophers;

			case 3:
				return sages;

			case 4:
				return explorers;

			case 5:
				return wizards;

			case 6:
				return yogis;

		}

		return null;

	}

	public string nameFromClassIndex(int index) {

		switch(index) {

		case 0:
			return "Warrior";


		case 1:
			return "Master";


		case 2:
			return "Philosopher";


		case 3:
			return "Sage";


		case 4:
			return "Explorer";


		case 5:
			return "Wizard";


		case 6:
			return "Yogi";

			
		}

		return "Unknown";

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Texture heroTexture(int classId, int specificId) {

		Texture[] tex = texArrayFromIndex (classId);

		if (tex == null)
			return null;

		if (specificId < tex.Length) {
			return tex [specificId];
		} else
			return null;

	}
}
