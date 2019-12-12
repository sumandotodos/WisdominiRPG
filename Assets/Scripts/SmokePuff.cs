using UnityEngine;
using System.Collections;

public class SmokePuff : MonoBehaviour {

	float scale;

	const float INITIALSCALE = 1.0f;
	const float ENDSCALE = 2.4f;

	public float buoyancy;
	public float turbulenceStregth;
	public float turbulenceFrequency;
	public float intertia;

	const float GRANULARITY = 10000.0f;

	public Vector3 wind;

	Vector3 speed;

	Vector3 position;

	float timeForDirectionChange;
	float elapsedTime;
	Vector3 targetTurbulence;
	Vector3 turbulence;
	Vector3 deltaTurbulence;

	public Color tintColor;

	float transparency;

	Material matRef;

	float deltaScale;
	float deltaTransparency;

	public Vector3 initialPosition;

	public float timeToLive;

	float totalElapsedTime;

	// Use this for initialization
	void Start () {

		position = initialPosition;
		speed = new Vector3 (0, 1, 0) * buoyancy + wind;
		timeForDirectionChange = 1.0f / turbulenceFrequency;

		targetTurbulence = new Vector3 (Random.Range (0, GRANULARITY) / GRANULARITY, 
			Random.Range (0, GRANULARITY) / GRANULARITY,
			Random.Range (0, GRANULARITY) / GRANULARITY);

		turbulence = targetTurbulence;

		matRef = this.GetComponent<Renderer> ().material;
		matRef = this.GetComponent<MeshRenderer> ().material;

		transparency = 0.0f;

		Color newColor = new Color (tintColor.r, tintColor.g, tintColor.b, 1.0f - transparency);

		//matRef.color = newColor;
		matRef.SetColor ("_TintColor", newColor);

		scale = INITIALSCALE;

		deltaScale = (ENDSCALE - INITIALSCALE) / timeToLive;

		deltaTransparency = 1.0f / timeToLive;
	
	}
	
	// Update is called once per frame
	void Update () {

		totalElapsedTime += Time.deltaTime;

		elapsedTime += Time.deltaTime;
		if (elapsedTime > timeForDirectionChange) {
			elapsedTime = 0.0f;
			targetTurbulence = new Vector3 (Random.Range (0, GRANULARITY) / GRANULARITY, 
				Random.Range (0, GRANULARITY) / GRANULARITY,
				Random.Range (0, GRANULARITY) / GRANULARITY) * turbulenceStregth;
			deltaTurbulence = (targetTurbulence - turbulence) / timeForDirectionChange;
		}

		turbulence += deltaTurbulence * Time.deltaTime;

		position += (speed + turbulence) * Time.deltaTime;

		if (totalElapsedTime > timeToLive) {

			Destroy (this.gameObject);

		}

		scale += deltaScale * Time.deltaTime;

		transparency += deltaTransparency * Time.deltaTime;

		Color newColor = new Color (tintColor.r, tintColor.g, tintColor.b, 1.0f - transparency);

		matRef.SetColor ("_TintColor", newColor);

		this.transform.position = position;
		this.transform.localScale = new Vector3 (scale, scale, scale);
	
	}
}
