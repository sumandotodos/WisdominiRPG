using UnityEngine;
using System.Collections;

/*
 * 
 *  Tenemos WorldStatic (todo lo que no se mueve) y WorldDynamic: todo lo que se mueve, a excepción de Player
 * 
 * Player   todos tienen que tener collider para chocarse con WorldStatic
 * Shadow
 * RedEyes
 * 
 * 
 *    Vamos a hacer que, en principio, no se puedan chocar entre sí
 * 
 *    colocamos a Player en capa Player
 *    shadow en capa Shadow
 *    eyes en capa Eyes
 * 
 *   no se pueden chocar entre sí...
 *    
 *    
 * 
 * 
 */

public class Redeyes : MonoBehaviour {

	/* references */

	public Freezer freezer;
	public GameObject spriteQuad;
	new public Light light;
	public Texture[] redeyes;
	Rigidbody rigidBody;
	Material matRef;
	public StringBank stringBank;
	public Rosetta rosetta;
	public GameObject thoughtTextPrefab;
	public GameObject player;
	public Letter letter;
	public IconCooldown eyeDispelController;

	/* properties */

	int frame;
	float angle;
	float ttl;
	bool properties;
	float linearSpeed;
	bool initialized = false;
	public float animationSpeed = 2.0f;
	float animElapsedTime;
	float opacity;
	float opacitySpeed;


	/* constants */

	const float TTL = 2.0f;
	const float maxLinearSpeed = 12.0f;
	const float minOpacitySpeed = 0.3f;
	const float maxOpacitySpeed = 3.55f;
	const float maxAngleVariation = 0.6f;


	/* methods */

	public void initialize () 
	{
		frame = 0;
		ttl = TTL;
		rigidBody = this.GetComponent<Rigidbody> ();
		initialized = true;
		angle = floatRandomRange (0.0f, 2.0f * 3.1416f);

		Vector3 playerPos = player.transform.position;
		Vector3 thisPos = this.transform.position;

		Vector3 diff = playerPos - thisPos;

		diff.Normalize ();

		angle = Mathf.Acos (diff.x);
		if (diff.z < 0.0)
			angle = (2.0f * 3.1416f) - angle;

		float angleVar = floatRandomRange (0.0f, maxAngleVariation) - maxAngleVariation/2.0f;

		angle += angleVar;

		animElapsedTime = 0.0f;
		frame = 0;
		matRef = spriteQuad.GetComponent<Renderer> ().material;
		matRef.mainTexture = redeyes [0];
		opacity = 1.0f;
		linearSpeed = maxLinearSpeed;
		opacitySpeed = minOpacitySpeed;

		if (freezer == null) {
			freezer = GameObject.Find ("Freezer").GetComponent<Freezer> ();
		}	
	}
	
	void Start () 
	{	
		if (!initialized)
			initialize ();
	}

	void Update() 
	{
		if (freezer.frozen)
			return;

		animElapsedTime += Time.deltaTime;
		if (animElapsedTime > (1.0f / animationSpeed)) 
		{

			animElapsedTime = 0.0f;

			if (frame < redeyes.Length) 
			{
				matRef.mainTexture = redeyes [frame++];
			}
		}

		opacity -= opacitySpeed * Time.deltaTime;
		if (opacity < 0.0f) {
			opacity = 0.0f;
			Destroy (this.gameObject);
		}

		light.intensity = opacity;
		Vector4 color = new Vector4 (1, 1, 1, opacity);
		matRef.color = color;

		if(eyeDispelController.mustDispel()) opacitySpeed = maxOpacitySpeed * 10.0f;

	}

	void FixedUpdate() 
	{
		if (freezer.frozen) 
		{
			rigidBody.velocity = new Vector3 (0, 0, 0);
			rigidBody.useGravity = false;
		} else {

			//rigidBody.useGravity = true;
			Vector3 velocity = new Vector3 (Mathf.Cos (angle), 0, Mathf.Sin (angle) * 1.3f);
			velocity *= linearSpeed;
			rigidBody.velocity = velocity;
		}
	}

	/* WARNING put floatRangomRange somewhere sensible, do not repeat across classes */
	public float floatRandomRange(float min, float max) 
	{
		int iMax, iMin;

		const float granularity = 10000.0f;

		iMax = (int)(max * granularity);
		iMin = (int)(min * granularity);

		int iRes = Random.Range(iMin, iMax);

		float fRes = ((float)iRes) / granularity;

		return fRes;
	}

	void OnCollisionEnter(Collision other) 
	{
		Collision o = other;
		opacitySpeed = maxOpacitySpeed;
	}

	void OnTriggerEnter(Collider other) 
	{		
		if(other.tag == "WorldStatic")
		opacitySpeed = maxOpacitySpeed;
		
		if (other.tag == "Player")
		opacitySpeed = maxOpacitySpeed;

		RedEyesController rec = GameObject.Find ("RedEyesController").GetComponent<RedEyesController> ();

		if (other.tag == "Player") 
		{
			if (!other.GetComponent<PlayerScript> ().blocked) 
			{
				if (rec.canMakeText ()) {
                    Debug.Log("Player blink!!");
                    other.GetComponent<PlayerScript>().Blink(4.0f);
                    Vector3 newPos = other.gameObject.transform.position;
					newPos.y = newPos.y + 2.5f;
					GameObject newText;
					newText = (GameObject)Instantiate (thoughtTextPrefab, newPos, Quaternion.Euler (0, 0, 0));
					string sss = stringBank.getNextStringId ();
					sss = stringBank.getNextString ();
					newText.GetComponent<TextMesh> ().text = stringBank.getNextString ();
					rec.textMade ();
					//letter.decLetter ();
					letter.decStep ();
                    letter.decStep();
                    //letter.decStep();
                    //letter.decStep();
                    //letter.decStep();
                    //letter.decStep();
                }
			}
		}
	}
}
