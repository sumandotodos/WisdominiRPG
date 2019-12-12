using UnityEngine;
using System.Collections;

public class FogZone : MonoBehaviour {

	public PlayerScript player;
	Vector3 entryPoint;
	bool inside;
	public float fullFogRadius;
	float fogLinStart, fogLinEnd;
	public float targetFogLinStart;
	public float targetFogLinEnd;
	float radius;
	float newFogStart;
	float newFogEnd;
	public string mapVariable;
	public Color fogColor;
	Color originalFogColor;
	LevelControllerScript lvl;

	float parameter;

	void Start () 
	{
		lvl = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		player = GameObject.Find ("Player").GetComponent<PlayerScript> ();
		if (lvl.retrieveBoolValue (mapVariable))
		{
			Destroy (this.gameObject);
		}

		fogLinEnd = RenderSettings.fogEndDistance;
		fogLinStart = RenderSettings.fogStartDistance;
		originalFogColor = RenderSettings.fogColor;	
	}
	
	void Update () 
	{	
		if (inside) {
			Vector3 flatEntryPoint = entryPoint;
			flatEntryPoint.y = 0.0f;
			Vector3 flatPlayerPos = player.transform.position;
			flatPlayerPos.y = 0.0f;
			Vector3 flatCentroid = this.transform.position;
			flatCentroid.y = 0.0f;
			Vector3 flatDiff = flatCentroid - flatPlayerPos;
			float distFromCentroid = flatDiff.magnitude;
			if (distFromCentroid < fullFogRadius) { // totally inside
				RenderSettings.fogStartDistance = targetFogLinStart;
				RenderSettings.fogEndDistance = targetFogLinEnd;

			} else if (distFromCentroid > radius) { // totally outside
				RenderSettings.fogStartDistance = fogLinStart;
				RenderSettings.fogEndDistance = fogLinEnd;

			} else { // in between: lerp time
				// calculate lerp parameter

				Vector3 transformedPlayerPos = flatPlayerPos - flatCentroid;
				Vector3 transformedInPoint = flatEntryPoint - flatCentroid;

				float distToPlayer = transformedPlayerPos.magnitude - fullFogRadius;
				float distToEntryPoint = transformedInPoint.magnitude - fullFogRadius;
				parameter = (distToPlayer / distToEntryPoint);

				newFogStart = Mathf.Lerp (targetFogLinStart, fogLinStart, parameter);
				newFogEnd = Mathf.Lerp (targetFogLinEnd, fogLinEnd, parameter);
				Color newColor = Color.Lerp (fogColor, originalFogColor, parameter);

				// apply newly calculated fog settings
				RenderSettings.fogStartDistance = newFogStart;
				RenderSettings.fogEndDistance = newFogEnd;
				RenderSettings.fogColor = newColor;
			}
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Player")
		{
			entryPoint = other.transform.position;
			radius = (entryPoint - this.transform.position).magnitude;
			fogLinEnd = RenderSettings.fogEndDistance;
			fogLinStart = RenderSettings.fogStartDistance;
			inside = true;
		}
	}

	void OnTriggerExit(Collider other) 
	{
		if (other.tag == "Player") 
		{
			inside = false;
			RenderSettings.fogStartDistance = fogLinStart;
			RenderSettings.fogEndDistance = fogLinEnd;
		}
	}
}
