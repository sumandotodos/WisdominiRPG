using UnityEngine;
using System.Collections;

[System.Serializable]
public class VisitedLocationCoords {

	private Vector3 locationCoordinates;
	private string locationName;
	private bool locationVisited;

	public void setLocation(string loc) {

		locationName = loc;

	}

	public void setCoordinates(Vector3 coords) {

		locationCoordinates = coords;

	}

	public string getLocationName() {

		return locationName;

	}

	public Vector3 getLocationCoordinates() {

		return locationCoordinates;

	}

}
