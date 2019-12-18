using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PickedUpObjectsList  {

	public string LocationName;
	public List<string> pickedUpNames;
	public int currentIndex;
	public float spawnPointX;
	public float spawnPointY;
	public float spawnPointZ;

	public bool locationVisited;

	public PickedUpObjectsList() {

		pickedUpNames = new List<string>();
		currentIndex = 0;
		locationVisited = false;

	}

	public void rewindList() {

		currentIndex = 0;

	}

	public string nextObject() {

		if (currentIndex < pickedUpNames.Count) {

			return pickedUpNames [currentIndex++];

		} else
			return "";

	}

	public void setLocationName(string ln) {

		LocationName = ln;

	}

	public void addPickedObject(string name) {

		pickedUpNames.Add (name);

	}

    public bool isInList(string name)
    {
        for(int i = 0; i<pickedUpNames.Count; ++i)
        {
            if(pickedUpNames[i]==name)
            {
                return true;
            }
        }
        return false;
    }

    public void removePickedObject(string name) {

		pickedUpNames.Remove (name);

	}

	public string locationName() {

		return LocationName;

	}
	public string objectName(int index) {

		if(index<pickedUpNames.Count) {

			return pickedUpNames[index];

		}

		return "";

	}

	public int count() {

		return pickedUpNames.Count;

	}
		

	public void setSpawnPoint(Vector3 coords) {


		spawnPointX = coords.x;
		spawnPointY = coords.y;
		spawnPointZ = coords.z;
		locationVisited = true;

	}
		

//	public Vector3 getSpawnPoint() {
//
//		return new Vector3 (spawnPointX, spawnPointY, spawnPointZ);
//
//	}

	public bool hasBeenVisited() {

		return locationVisited;

	}

}
