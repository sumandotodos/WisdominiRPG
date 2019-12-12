using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIFaderScript : WisdominiObject {

	float fade; // 0.0 is totally black; 1.0 is totally transparent
	float targetFade;
	Material matRef;
	Vector3 color;

	public float fadeSpeed;
	public bool autoFadeIn = true;
	public Image imageRef;

	bool preventAutofade = false;

	public void preventAutoFader() {
		preventAutofade = true;
	}

	public bool isFading; 
	bool initialized;

	Vector4 col;

	new void Start ()
	{	
		if (!initialized)
			Initialize ();
	}

	public void Initialize() 
	{
		col = new Vector4 ();

		isFading = false;

		initialized = true;

		//matRef = imageRef.GetComponent<Renderer> ().material;

		isFading = false;

		//imageRef.enabled = false;

		if (autoFadeIn && (!preventAutofade)) {

			setFadeValue (0.0f);
			fadeIn ();

		} else {
			setFadeValue (1.0f);
		}
	}

	public void setFadeColor(float r, float g, float b) 
	{
		Vector4 newCol = new Vector4 (r, g, b, 1.0f-fade);
        Debug.Log("##########" + r);
		imageRef.color = newCol;
	}

	void updateMaterial() 
	{
		//if (matRef == null)
		//	return;
		col = imageRef.color;//matRef.color;
		col.w = 1.0f-fade;
		//col.w = 0.5f;
		//col = new Vector4(0.8f, 0.2f, 0.67f, 0.5f);
		imageRef.color = col;
	}

	public void setFadeValue(float f) // fade=0: totally opaque   fade=1: totally transparent
	{
		fade = f;
		updateMaterial ();
	}
	
	new void Update ()
	{	
		if (!isFading)
			return;

		if (fade > targetFade) {
			fade -= fadeSpeed*Time.deltaTime;
			if (fade < targetFade) {
				fade = targetFade;

				isFading = false;

				if (waitingRef != null) {
					waitingRef.isWaitingForActionToComplete = false;
				}
			}
		}
		else {
			fade += fadeSpeed*Time.deltaTime;
			if (fade > targetFade) {
				fade = targetFade;
				if (fade == 1.0f)
					imageRef.enabled = false;
				isFading = false;
				if (waitingRef != null) {
					waitingRef.isWaitingForActionToComplete = false;
				}

			} else if (fade == targetFade) {
				isFading = false;
			}
		}		
		updateMaterial ();
	}

    public void _wm_white()
    {
        imageRef.color = Color.white;
    }

    public void _wm_black()
    {
        imageRef.color = Color.black;
    }

    public void fadeOut() 
	{
		imageRef.enabled = true;
		targetFade = 0.0f;
		isFading = true;
		imageRef.enabled = true;
	}

	public void fadeIn() 
	{
		targetFade = 1.0f;
		isFading = true;
	}

    public void _wm_fadeOut()
    {
        fadeOut();
    }

    public void _wm_fadeIn()
    {
        fadeIn();
    }

    public void _wa_fadeOut(WisdominiObject waiter) 
	{
		waitingRef = waiter;
		fadeOut ();
	}

	public void _wa_fadeIn(WisdominiObject waiter) 
	{
		waitingRef = waiter;
		fadeIn ();
	}
}
