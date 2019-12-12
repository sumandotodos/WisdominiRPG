using UnityEngine;
using System.Collections;

public class StringBankCollection : MonoBehaviour {

	public StringBank[] bank;
	public bool random;
	public Rosetta rosetta;

	int nextItem;
	int prevRandom;

	void Start () 
	{	
		reset ();
	}

	public void reset() 
	{
		nextItem = -1;
		prevRandom = -1;
	}
	
	void Update () 
	{
	
	}

	virtual public StringBank getNextBank() 
	{		
		if (random) 
		{
			nextItem = Random.Range (0, bank.Length - 1);
			if (bank.Length > 1) {
				if (nextItem == prevRandom)
					nextItem = (nextItem + 1) % bank.Length;
			}
			prevRandom = nextItem;
		} else
			nextItem = (nextItem + 1) % bank.Length;

		return bank [nextItem];
	}

	public StringBank getBank(int index) 
	{
		if (bank.Length > index)
			return bank [index];
		else
			return null;
	}

	public string GetName()
	{
		return this.name;
	}
}
