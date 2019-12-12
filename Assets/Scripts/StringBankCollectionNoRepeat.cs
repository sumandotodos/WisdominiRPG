﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringBankCollectionNoRepeat : StringBankCollection {


	public string objName;
	int[] rows;
	int round;
	int roundRemain;
	MasterControllerScript mc;
	DataStorage ds;

	//public int tableIndex;

	//public SituationChooser parent;

	//public TablaAuxiliar correspondences;

	void Awake()
	{		
		initialize ();
	}

	public void initialize(/*int nRows*/) 
	{
		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();
		ds = mc.getStorage ();
		//rows = new int[nRows];
		rows = new int[bank.Length];

		for (int i = 0; i < bank.Length; ++i) 
		{
			//rows [i] = 0; // Cargar cada bank
			rows [i] = ds.retrieveIntValue(bank[i].name);
		}
		//round = 0; // RETRIEVE
		round = ds.retrieveIntValue("Round" + objName);
		//roundRemain = bank.Length;
		roundRemain = ds.retrieveIntValue ("RoundRemain" + objName);
	}

	override public StringBank getNextBank() 
	{
		initialize ();
		// find a free index
		int index = Random.Range (0, rows.Length);

		while (rows [index] != round) 
		{
			index = Random.Range (0, rows.Length);
		}

		rows [index]++; // mark as used
		ds.storeIntValue(bank[index].name, rows[index]);

		//--roundRemain;
		++roundRemain;

		//if (roundRemain == 0) 
		if (roundRemain == bank.Length)
		{
			//roundRemain = rows.Length;
			roundRemain = 0;
			round++;
			ds.storeIntValue ("Round" + objName, round);
		}

		ds.storeIntValue ("RoundRemain" + objName, roundRemain);
		return bank[index];
	}
}
