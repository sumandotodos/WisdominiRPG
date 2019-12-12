using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LogoController : WisdominiObject {

	/* references */

	public UIFaderScript fader;

	/* public properties */

	public string nextLevel;

	/* properties */

	float elapsedTime;
	int state;

	/* constants */

	new const float delayTime = 4.0f;
	const float minTime = 0.6f;

	// Use this for initialization
	new void Start () {

		elapsedTime = 0.0f;
		state = 0;

	
	}
	
	// Update is called once per frame
	new void Update () {

		if (this.isWaitingForActionToComplete)
			return;

		if (state == 1) {
			SceneManager.LoadScene (nextLevel);
		}

		elapsedTime += Time.deltaTime;

		if (elapsedTime > minTime) {
			if (Input.GetMouseButtonDown (0)) {
				elapsedTime = delayTime + 1.0f;
			}
		}

		if ((elapsedTime > delayTime) && state == 0) {
			fader._wa_fadeOut (this);
			this.isWaitingForActionToComplete = true;
			state = 1;
		}
	
	}
}
