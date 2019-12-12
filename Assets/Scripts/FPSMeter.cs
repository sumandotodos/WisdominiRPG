using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSMeter : MonoBehaviour {

	new Text guiText;

	public float updateInterval = 0.5f;

	private float accum = 0.0f; // FPS accumulated over the interval
	private int frames = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval

	//bool quarterRes;

	// Use this for initialization
	void Start () {

		guiText = this.GetComponent<Text> ();
		timeleft = updateInterval;  
		//quarterRes = false;
	
	}
	
	// Update is called once per frame
	void Update () {

		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;

		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0f )
		{
			// display two fractional digits (f2 format)
			float fps = (accum/frames);
			guiText.text = "" + fps.ToString("f2");
			timeleft = updateInterval;
			accum = 0.0f;
			frames = 0;

			/*if (fps < 26.0) {
				QualitySettings.vSyncCount = 0;
				if (QualitySettings.GetQualityLevel () > 0)
					QualitySettings.DecreaseLevel ();
				//else if (!quarterRes) {
				//	Screen.SetResolution (Screen.width / 2, Screen.height / 2, true);
				//	quarterRes = true;
				//}
			}*/
			//if (fps > 29.0) {
				//if (quarterRes) {
					
				//}
			//	QualitySettings.IncreaseLevel ();

			//}
			

		}
	
	}
}
