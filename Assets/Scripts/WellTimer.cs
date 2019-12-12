using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WellTimer : MonoBehaviour {

	public DisorderedSentence ActivityController;

	public Texture[] images;

	Material matRef;

	bool even;

	bool gameOver = false;

	float blend;
	float targetBlend;

	public float blendSpeed;

	public float frameTime;

	float elapsedTime;
	public bool stopped;

	int nextImage;

	void Start () 
	{
		stopped = false;
		even = true;
		blend = 0.0f;
		elapsedTime = 0.0f;

		matRef = this.GetComponent<Image> ().material;

		matRef.SetTexture ("_Texture1", images [0]);
		//matRef.mainTexture = images [0];
		matRef.SetTexture ("_Texture2", images [1]);
		matRef.SetFloat ("_Blend", blend);

		nextImage = 2;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!stopped) 
		{
			elapsedTime += Time.deltaTime;
		}

		if (elapsedTime > frameTime) {

			if (nextImage >= images.Length) { // stand still and tell master to fadeout and wrap up...
				if (!gameOver) {
					ActivityController._wm_gameover ();
					gameOver = true;
				}
				return;
			}

			elapsedTime = 0.0f;
			if (even) {
				targetBlend = 1.0f;
				//even = false;
			} 
			else {
				targetBlend = 0.0f;
				//even = true;
			}

		}

		if (even) {
			if (blend < targetBlend) {
				blend += blendSpeed * Time.deltaTime;
				if (blend >= targetBlend) {
					blend = targetBlend;
					even = false;
					matRef.SetTexture ("_Texture1", images [nextImage]);
					if (nextImage < images.Length)
						++nextImage;
				}
			}
		} 
		else {
			if (blend > targetBlend) {
				blend -= blendSpeed * Time.deltaTime;
				if (blend <= targetBlend) {
					blend = targetBlend;
					even = true;
					matRef.SetTexture ("_Texture2", images [nextImage]);
					if (nextImage < images.Length)
						++nextImage;
				}
			}
		}

		matRef.SetFloat ("_Blend", blend);

	}

	void stop() 
	{
		stopped = true;
	}

	public void _wm_stop() 
	{
		stop();
	}
}
