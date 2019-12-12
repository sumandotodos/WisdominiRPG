using UnityEngine;
using System.Collections;

public class WordFighter : MonoBehaviour {

	public Texture[] standUp;
	public Texture[] healthy;
	public Texture[] damaged;
	public Texture[] neutral;
	public Texture[] damagedHit;

	public GameObject healthSprite;
	public GameObject damagedSprite;
	public GameObject normalSprite;

	Material healthyMat;
	Material damagedMat;
	Material normalMat;

	const float damagedHitDuration = 1.0f;
	bool blink;
	bool blinking;

	int animation = -1;

	float elapsedTime;
	float blinkingElapsedTime;
	float blinkElapsedTime;
	const float blinkDuration = 1.0f / 30.0f;
	int frame;

	float health;
	float targetHelth;

	public float animationSpeed = 8.0f;

	public GameObject hitPrefab;
	public Vector3 hitOffset;
	Vector3 worldPosition;

	float t = 1.0f;
	float targetT = 0.0f;

	float op = 0.0f;
	float targetOp = 1.0f;

	float op2 = 0.0f;
	float targetOp2 = 0.0f;

	public bool displaced = true;
	public bool faded = true;
	public bool fighting = false;

	public Vector3 initialDisplacement;

	public void setHealth(float pc) 
	{
		targetHelth = pc;
	}

	public void hit() 
	{
		Instantiate (hitPrefab, this.transform.position + hitOffset, Quaternion.Euler (0, 0, 0));
		blinking = true;
	}

	void Start () 
	{
		healthyMat = healthSprite.GetComponent<Renderer> ().material;
		damagedMat = damagedSprite.GetComponent<Renderer> ().material;
		normalMat = normalSprite.GetComponent<Renderer> ().material;

		health = targetHelth = 1.0f;
		elapsedTime = 0.0f;

		healthyMat.SetColor ("_TintColor", new Color (1, 1, 1, 0));
		damagedMat.SetColor ("_TintColor", new Color (1, 1, 1, 0));
		normalMat.SetColor ("_TintColor", new Color (1, 1, 1, 0));

		setAnimation (-1);

		healthyMat.SetTexture ("_MainTex", standUp [0]);
		damagedMat.SetTexture ("_MainTex", standUp [0]);
		normalMat.SetTexture ("_MainTex", standUp [0]);

		blink = false;
		blinking = false;
		blinkingElapsedTime = 0.0f;
		blinkElapsedTime = 0.0f;

		worldPosition = this.transform.position;

		this.transform.position = worldPosition + initialDisplacement;	
	}

	public void setAnimation(int a) 
	{
		animation = a;
	}

	public void showHalo () 
	{
		targetOp2 = 1.0f;
	}
	
	void Update () 
	{
		bool changed;

		if (!faded) 
		{
			changed = Utils.updateSoftVariable (ref op, targetOp, 0.2f);
			if (changed) {
				//healthyMat.SetColor ("_TintColor", new Color (1, 1, 1, op));
				//damagedMat.SetColor ("_TintColor", new Color (1, 1, 1, op));
				normalMat.SetColor ("_TintColor", new Color (1, 1, 1, op));
			}
		}

		if (!displaced) 
		{
			changed = Utils.updateSoftVariable (ref t, targetT, 0.8f);
			if (changed) 
			{
				this.transform.position = worldPosition + initialDisplacement * t;
			}
		}

		if (fighting) 
		{
			changed = Utils.updateSoftVariable (ref health, targetHelth, 6.0f);
			if (changed) 
			{
				healthyMat.SetColor ("_TintColor", new Color (1, 1, 1, health));
				damagedMat.SetColor ("_TintColor", new Color (1, 1, 1, 1.0f - health));
			}
		} else {
			changed = Utils.updateSoftVariable (ref op2, targetOp2, 0.2f);
			if (changed) {
				healthyMat.SetColor("_TintColor", new Color(1, 1, 1, op2));
			}
		}

		if (animation >= 0)
			elapsedTime += Time.deltaTime;

		if (animation == 0) 
		{
			if (elapsedTime > (2.0f / animationSpeed)) {
				healthyMat.SetTexture ("_MainTex", standUp [frame]);
				damagedMat.SetTexture ("_MainTex", standUp [frame]);
				normalMat.SetTexture ("_MainTex", standUp [frame]);
				++frame;
				if (frame >= standUp.Length)
					frame = standUp.Length - 1;
			}

		} else if (animation == 1) 
		{
			if (elapsedTime > (1.0f / animationSpeed)) 
			{
				++frame;
				elapsedTime = 0.0f;
				if (frame == healthy.Length) // WARNING: healty assumed same length as rest!!!
				frame = 0;

				if (blink) {
					healthyMat.SetTexture ("_MainTex", damagedHit [frame]);
					damagedMat.SetTexture ("_MainTex", damagedHit [frame]);
					normalMat.SetTexture ("_MainTex", damagedHit [frame]);
				} else {
					healthyMat.SetTexture ("_MainTex", healthy [frame]);
					damagedMat.SetTexture ("_MainTex", damaged [frame]);
					normalMat.SetTexture ("_MainTex", neutral [frame]);
				}
			}
		}

		if (blinking) {
			blinkElapsedTime += Time.deltaTime;
			if (blinkElapsedTime > blinkDuration) {
				blink = !blink;
				blinkElapsedTime = 0.0f;
			}
			blinkingElapsedTime += Time.deltaTime;
			if (blinkingElapsedTime > damagedHitDuration) {
				blinkingElapsedTime = 0.0f;
				blinking = false;
				blink = false;
				healthyMat.SetTexture ("_MainTex", healthy [frame]);
				damagedMat.SetTexture ("_MainTex", damaged [frame]);
				normalMat.SetTexture ("_MainTex", neutral [frame]);
			}
		}	
	}
}
