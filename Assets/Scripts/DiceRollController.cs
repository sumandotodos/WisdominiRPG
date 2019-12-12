using UnityEngine;
using System.Collections;

public class DiceRollController : MonoBehaviour {

	/* references */

	public GameObject left;
	public GameObject center;
	public GameObject right;

	public GameObject diceQuad;

	public TextMesh theText;

	Material diceMat;

	public bool resultReady;

	/* properties */

	float elapsedTime;
	float deltaX;
	float delay;
	public int state;
	float initialScale;
	Vector3 initialLeftPos;
	Vector3 initialRightPos;
	public float opacity;
	public int rollResult;

	/* constants */

	const float initialDelay = 1.2f;
	const float xSpeed = 20.0f;
	const float maxOffset = 10.0f;
	const float opacitySpeed = 2.0f;

	// Use this for initialization
	void Start () {
	
		initialLeftPos = left.transform.position;
		initialRightPos = right.transform.position;
		initialScale = left.transform.localScale.x;
		reset ();

		//play ();
	}
	
	// Update is called once per frame
	void Update () {
	
		/* idle */
		if (state == 0) {

		}

		/* initial delay*/
		if (state == 1) {
			elapsedTime += Time.deltaTime;
			if (opacity < 1.0f) {
				opacity += opacitySpeed * Time.deltaTime;
				if (opacity > 1.0f)
					opacity = 1.0f;
				diceMat.color = new Color (1, 1, 1, opacity);
			}
			if (elapsedTime > initialDelay)
				++state;
		}

		/* triplicating dices */
		if (state == 2) {
			deltaX += xSpeed * Time.deltaTime;
			Vector3 pos = initialLeftPos + new Vector3 (deltaX, 0, 0);
			left.transform.position = pos;
			pos = initialRightPos - new Vector3 (deltaX, 0, 0);

			float percent = initialScale * (0.5f + 0.5f * (deltaX / maxOffset));
			left.transform.localScale = new Vector3 (percent, percent, percent);
			right.transform.localScale = new Vector3 (percent, percent, percent);

			right.transform.position = pos;
			if (deltaX > maxOffset) {
				elapsedTime = 0.0f;
				++state;
			}
		}

		/* small pause */
		if (state == 3) {

			elapsedTime += Time.deltaTime;
			if (elapsedTime > initialDelay/6) {
				++state;
				rollResult = 0;
			}

		}


		/* roll dice */
		if (state == 4) {
			if (valorDado1 == 0) {
				rollResult += left.GetComponentInChildren<Dice> ().roll ();
			} 
			else
				rollResult += left.GetComponentInChildren<Dice> ().roll (valorDado1);
			if (valorDado2 == 0) {
				rollResult += center.GetComponentInChildren<Dice> ().roll ();
			}
			else
				rollResult += center.GetComponentInChildren<Dice> ().roll (valorDado2);
			if (valorDado3 == 0) {
				rollResult += right.GetComponentInChildren<Dice> ().roll ();
			} 
			else
				rollResult += right.GetComponentInChildren<Dice> ().roll (valorDado3);

			Debug.Log ("<color=purple>Actual res : " + rollResult + "</color>");

			elapsedTime = 0.0f;
			++state;
		}

		if (state == 5) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > initialDelay * 3.4f) {
				++state;
			}
		}

		if (state == 6) {
			

			diceMat.color = new Color (1, 1, 1, opacity);
			theText.text = "" + rollResult;
			theText.GetComponent<DiceText> ().go ();
			++state;
		}

		if (state == 7) {
			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < 0.0f) {
				opacity = 0.0f;
				resultReady = true;
				++state;
			}
			diceMat.color = new Color (1, 1, 1, opacity);
		}

	}

	public void play() {

		valorDado1 = 0;
		valorDado2 = 0;
		valorDado3 = 0;

		state = 1;

		rollResult = 0;

	}

	int valorDado1;
	int valorDado2;
	int valorDado3;

	public void play(int result) {

		int testResult = 0;

		while (testResult != result) { // flipa con mi algoritmia
			valorDado1 = Random.Range (1, 6 + 1);
			valorDado2 = Random.Range (1, 6 + 1);
			valorDado3 = Random.Range (1, 6 + 1);
			testResult = valorDado1 + valorDado2 + valorDado3;
		}

		Debug.Log ("<color=red>Res : " + result + "...   Dadetes : " + valorDado1 + ", " + valorDado2 + ", " + valorDado3 + "</color>");

		rollResult = 0;

		state = 1;

	}

	public void reset() {
		state = 0;
		deltaX = 0.0f;
		elapsedTime = 0;
		left.transform.localScale = new Vector3 (initialScale / 2, initialScale / 2, initialScale / 2);
		right.transform.localScale = new Vector3 (initialScale / 2, initialScale / 2, initialScale / 2);
		left.transform.position = initialLeftPos;
		right.transform.position = initialRightPos;
		//initialScale = left.transform.localScale.x;
		diceMat = diceQuad.GetComponent<Renderer> ().material;
		diceMat.color = new Color (1, 1, 1, 0);
		rollResult = 0;
		opacity = 0.0f;
		resultReady = false;
		left.GetComponentInChildren<Dice> ().reset ();
		center.GetComponentInChildren<Dice> ().reset ();
		right.GetComponentInChildren<Dice> ().reset ();
		theText.GetComponent<DiceText> ().reset ();
	}

	public void resetAnimation() {
		left.GetComponentInChildren<Dice> ().resetAnimation ();
		center.GetComponentInChildren<Dice> ().resetAnimation ();
		right.GetComponentInChildren<Dice> ().resetAnimation ();
	}

}
