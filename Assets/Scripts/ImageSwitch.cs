using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageSwitch : MonoBehaviour {

	public Sprite [] sprite;
	public int channel = -1;

	public Sprite getSprite() {

		if (channel > -1) {
			return sprite [channel];
		} else
			return null;

	}

	public void setChannel(int n) {

		if (n > (sprite.Length - 1))
			n = sprite.Length - 1;

		channel = n;

	}
}
