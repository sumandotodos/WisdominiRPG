/*using UnityEngine;
using System.Collections;

public class stargate : WisdominiObject {



	public LevelControllerScript level;
	public ParticleSystem particles;
	public GameObject background;
	public GameObject swirl;
	Material backm;
	Material swirlm;
	Color swirlCol;



	public string destinationScene;



	bool activated;
	float targetOpacity;
	float opacity;



	float opacitySpeed = 0.3f;

	new void Start () 
	{
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		activated = false;
		//backm = background.GetComponent<Renderer> ().material;
		swirlm = swirl.GetComponent<Renderer> ().material;
		swirlCol = swirl.GetComponent<StargateUVRotate> ().tint;
		//backm.color = new Color (0, 0, 0, 0);
		swirlm.SetColor("_TintColor", new Color (0, 0, 0, 0));
		targetOpacity = opacity = 0.0f;
		particles.Stop ();
		deactivate ();
		if (level.retrieveBoolValue (level.locationName + "StargateActivated")) 
		{
			activate ();
		}
	}
	
	void Update () 
	{
		if (opacity < targetOpacity) {
			opacity += opacitySpeed * Time.deltaTime;
			if (opacity > targetOpacity) {
				opacity = targetOpacity;
			}
			//backm.color = new Color (1, 1, 1, opacity);
			Color tempColor = swirlCol;
			tempColor.a = opacity * 0.8f;
			swirlm.SetColor("_TintColor", tempColor);
		}

		if (opacity > targetOpacity) {
			opacity -= opacitySpeed * Time.deltaTime;
			if (opacity < targetOpacity) {
				opacity = targetOpacity;
			}
			//backm.color = new Color (1, 1, 1, opacity);
			Color tempColor = swirlCol;
			tempColor.a = opacity * 0.8f;
			swirlm.SetColor("_TintColor", tempColor);
		}	
	}

	public void activate() 
	{
		activated = true;
		targetOpacity = 1.0f;
		level.storeBoolValue (level.locationName + "StargateActivated", true);
				
		particles.Play ();
	}

	public void deactivate() 
	{
		activated = false;
		targetOpacity = 0.0f;
		particles.Stop ();
	}

	public void _wm_activate() 
	{
		activate ();
	}

	public void _wm_deactivate()
	{
		deactivate ();
	}
}

*/

using UnityEngine;
using System.Collections;

public class stargate : WisdominiObject {

    /* references */

    public string explicitName = "";
	public LevelControllerScript level;
	public ParticleSystem particles;
	public GameObject background;
	public GameObject swirl;
    public GameObject Teleporter;
    public string OtherLocationKeyActivate = "";
	Material backm;
	Material swirlm;
	Color swirlCol;

	/* public properties */

	public string destinationScene;

    /* properties */

    public bool cheat = false;

	bool activated;
	float targetOpacity;
	float opacity;

	/* constants */

	float opacitySpeed = 0.3f;

	new void Awake () 
	{

        level = GameObject.Find("LevelController").GetComponent<LevelControllerScript>();

        if (explicitName!="")
        {
            deactivate();
            if(level.retrieveBoolValue(explicitName + "StargateActivated"))
            {
                activate();
            }
        }


        else {
            deactivate();

            if (cheat || level.retrieveBoolValue(level.locationName + "StargateActivated"))
            {
                activate();
            }
        }
	}

	public void activate() 
	{
		level.storeBoolValue (level.locationName + "StargateActivated", true);
        if(OtherLocationKeyActivate != "")
        {
            level.storeBoolValue(OtherLocationKeyActivate + "StargateActivated", true);
        }
        swirl.SetActive (true);
		particles.Play ();
        Teleporter.SetActive(true);
	}

	public void deactivate() 
	{
		particles.Stop ();
		swirl.SetActive (false);
		activated = false;
        Teleporter.SetActive(false);
	}

	public void _wm_activate() 
	{
		activate ();
	}

	public void _wm_deactivate()
	{
		deactivate ();
	}
}

