using UnityEngine;
using System.Collections;

enum HeroFanState { closed, opening, open, closing };

public class HeroFan : MonoBehaviour {

	/* references */

	public WordFightController controller;
	public GameObject[] heroHolder;
	public string[] wisdom;

	/* public properties */

	public float openSpeed = 10.0f;
	public float approachSpeed = 0.75f;
	float speed;

	/* properties */

	float radius;
	float angle;
	HeroFanState state;
	Vector3 originalScale;

	int nHeroes;

	/* constants */

	const float maxRadius = 1.85f;
	const float minRadius = 0.05f;
	const float angleSpeed = 6.0f;
	float aSpeed;


	void Start () 
	{	
		nHeroes = heroHolder.Length;
		radius = 0.0f;
		angle = 0.0f;
		state = HeroFanState.closed;

		aSpeed = angleSpeed;

		originalScale = heroHolder [0].transform.localScale;

		for(int i = 0; i<nHeroes; ++i) 
		{
			heroHolder [i].transform.localScale = Vector3.zero;
			Material m;
			m = heroHolder [i].GetComponent<Renderer> ().material;
			m.SetInt ("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
		}

		speed = openSpeed;
	}
	
	void Update () 
	{
		if (state == HeroFanState.closed) 
		{

		}

		if(state == HeroFanState.opening) 
		{
			radius += speed * Time.deltaTime;
			angle += aSpeed * Time.deltaTime;

			if (radius > maxRadius) 
			{
				radius = maxRadius;
				state = HeroFanState.open;
			}

			for (int i = 0; i < nHeroes; ++i) 
			{
				float x, y;

				x = radius * Mathf.Cos (angle + i*(2.0f * 3.1416f / (float)nHeroes));
				y = radius * Mathf.Sin (angle + i*(2.0f * 3.1416f / (float)nHeroes));

				Vector3 newPos = new Vector3 (x, y, 0);

				heroHolder[i].transform.localPosition = newPos;
				heroHolder[i].transform.localScale = originalScale * (radius/maxRadius);
			}
		}

		if (state == HeroFanState.open) 
		{

		}

		if(state == HeroFanState.closing)
		{
			radius -= speed * Time.deltaTime;
			angle += aSpeed * Time.deltaTime;

			if (radius < minRadius) 
			{
				radius = 0.0f;
				state = HeroFanState.closed;
				angle = 0.0f;
			}

			for (int i = 0; i < nHeroes; ++i) 
			{
				float x, y;

				x = radius * Mathf.Cos (angle + i*(2.0f * 3.1416f / (float)nHeroes));
				y = radius * Mathf.Sin (angle + i*(2.0f * 3.1416f / (float)nHeroes));

				Vector3 newPos = new Vector3 (x, y, 0);

				heroHolder[i].transform.localPosition = newPos;
				heroHolder[i].transform.localScale = originalScale * (radius/maxRadius);
			}
		}	
	}

	public void keep(int index) 
	{
		for (int i = 0; i < nHeroes; ++i) 
		{
			if(i != index)
				heroHolder [i].GetComponent<WordFightHero> ().setTargetOpacity (0.0f);
		}
	}

	public void keep(string type) 
	{
		int idx = 0;
		for (int i = 0; i < nHeroes; ++i) {
			if (wisdom [i].Equals (type)) {
				idx = i;
				break;
			}
		}
		keep (idx);
	}

	public void open()
	{
		aSpeed = angleSpeed;
		speed = openSpeed;
		state = HeroFanState.opening;
	}

	public void close() 
	{
		aSpeed = angleSpeed;
		speed = openSpeed;
		state = HeroFanState.closing;
	}

	public void closeSlowly() 
	{
		aSpeed = 0.0f;
		speed = approachSpeed;
		state = HeroFanState.closing;
	}
}
