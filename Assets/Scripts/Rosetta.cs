using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//[System.Serializable]
public class Rosetta : MonoBehaviour {


	/* constants */

	public int firstLevelHashSize = 300;
	public int secondLevelHashSize = 30;
	[HideInInspector]
	public List<RosettaHashDict> data;
	[HideInInspector]
	public List<string> singleData;
	[HideInInspector]
	public RosettaHashDict defaultTranslation;
	[HideInInspector]
	public int currentTranslationIndex;

	/* properties */

	[HideInInspector]
	string currentLocale;
	[HideInInspector]
	List<string> installedLocales;
	[HideInInspector]
	public int nTranslations;
	[HideInInspector]
	public int nStrings;
	//AllTerrainParser parser;


	/* non-volatile inspector properties */

	[HideInInspector]
	public int selectedTranslation = 0;

	public void Start() {

		DontDestroyOnLoad (this);

	}

	public void reset () {

		currentLocale = "default";
		installedLocales = new List<string> (); // read from disk

		data = new List<RosettaHashDict> ();
		defaultTranslation = new RosettaHashDict ();
		defaultTranslation.initialize (firstLevelHashSize, secondLevelHashSize);
		defaultTranslation.localeName = "default";
		data.Add (defaultTranslation);
	
		currentTranslationIndex = 0;

		nTranslations = 0;
		nStrings = 0;



	}

	public string[] getLoadedTranslationsNames() {

		List<string> list = new List<string> ();


		for (int i = 0; i < data.Count; ++i) {

			list.Add (data [i].localeName);

		}
	

		return list.ToArray ();

	}
	


	public void setLocale(string lang) {

		currentLocale = lang;

	}

	System.UInt32 hashMe(string str, int shiftBits)
	{
		System.UInt32 hash = 5381;
		System.UInt32 c;
		int i = 0;
		//System.UInt32 s = (System.UInt32)shiftBits;

		for (i = 0; i < str.Length; ++i) {
				c = (System.UInt32)str [i];
				hash = ((hash << shiftBits) + hash) + c; /* hash * 33 + c */
		}

		return hash;

	}

	void calculateHashCoordinates(string key, out int first, out int second) {

		int firstLevel, secondLevel;

		//firstLevel = key.Length % firstLevelHashSize; // this hash function is cheap, but utter shit
		firstLevel = (int)(hashMe(key, 7) % firstLevelHashSize);

		first = firstLevel;

		/*int sum = 0;
		for(int i=0; i<key.Length; ++i) {

			sum += (int)key[i];

		}
		sum = sum % secondLevelHashSize;
		second = sum;
		*/
		second = (int)(hashMe (key, 5) % secondLevelHashSize);

	}

	public void registerString(string key, string text, int dictionaryIndex) {

		int fl, sl;
		RosettaHashDict dict;

		calculateHashCoordinates (key, out fl, out sl);


		dict = data [dictionaryIndex];
		ListRosettaDictElement1D targetList = dict.data.theList[fl].theList[sl]; // you can only register on the default translation


		bool found = false;
		int i;
		for (i = 0; i < targetList.theList.Count; ++i) {
			if (targetList.theList [i].key.Equals (key)) {
				found = true;
				break;
			}
		}


		if (found) { // update only text

			targetList.theList [i].text = text;

		} else { // completely new entry

			RosettaDictElement e = new RosettaDictElement ();
			e.key = key;
			e.text = text;
			targetList.theList.Add (e);
			//++nStrings;

		}


	}

	public string retrieveString(string baseKey, int offset) {
		return retrieveString (baseKey + "_" + offset);
	}

	public string retrieveString(string key) {

		int fl, sl;

		RosettaHashDict currentTranslation;

		calculateHashCoordinates (key, out fl, out sl);

		currentTranslation = data[currentTranslationIndex];

		ListRosettaDictElement1D targetList = currentTranslation.data.theList[fl].theList[sl];

		string result = "";
		for (int i = 0; i < targetList.theList.Count; ++i) {
			if (targetList.theList [i].key.Equals (key)) {
				result = targetList.theList [i].text;
				break;
			}
			
		}
		if (result.Equals("")) { // return default text
			targetList = defaultTranslation.data.theList[fl].theList[sl];
			for (int i = 0; i < targetList.theList.Count; ++i) {
				if (targetList.theList [i].key.Equals (key)) {
					result = targetList.theList [i].text;
					break;
				}

			}
		}

		return result;

	}

	public void registerString(string key, string text) {

		int fl, sl;

		calculateHashCoordinates (key, out fl, out sl);

		ListRosettaDictElement1D targetList = defaultTranslation.data.theList[fl].theList[sl]; // you can only register on the default translation


		bool found = false;
		int i;
		for (i = 0; i < targetList.theList.Count; ++i) {
			if (targetList.theList [i].key.Equals (key)) {
				found = true;
				break;
			}
		}
	
	

		if (found) { // update only text

			targetList.theList [i].text = text;

		} else { // completely new entry

			RosettaDictElement e = new RosettaDictElement ();
			e.key = key;
			e.text = text;
			targetList.theList.Add (e);
			++nStrings;

		}


	}

	public string translationToString() {

		string res = "";

		if (currentTranslationIndex == 0) { // the default dict

			res += "locale: default\n";
			ListRosettaDictElement1D list;
			for(int i=0; i<defaultTranslation.firstLevelHashSize; ++i) {
				for(int j=0; j<defaultTranslation.secondLevelHashSize; ++j) {
					list = defaultTranslation.data.theList [i].theList[j];
					for (int k = 0; k < list.theList.Count; ++k) {
						res = res + list.theList[k].key + ":  \n\n\"" + list.theList[k].text + "\"\n\n  ->  \n\n\n\n"; 
					}
				}
			}

		} else { // another dict

			RosettaHashDict lDict;
			lDict = data [selectedTranslation];
			ListRosettaDictElement1D list;
			ListRosettaDictElement1D transList;
			res += "locale: " + lDict.localeName + "\n";
			for(int i=0; i<defaultTranslation.firstLevelHashSize; ++i) {
				for(int j=0; j<defaultTranslation.secondLevelHashSize; ++j) {
					list = defaultTranslation.data.theList [i].theList[j];
					transList = lDict.data.theList [i].theList [j];
					for (int k = 0; k < list.theList.Count; ++k) {
						// try and locate the key in transList
						string translation = "\"";
						for (int l = 0; l < transList.theList.Count; ++l) {
							if (transList.theList [l].key.Equals (list.theList [k].key))
								translation += transList.theList [l].text; // translation available, yay!
						}
						translation += "\"";
						res = res + list.theList[k].key + ":  \n\n\"" + list.theList[k].text + "\"\n\n  ->  " + translation + "\n\n\n\n"; 
					}
				}
			}


		}

		return res;

	}
}
