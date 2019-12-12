using UnityEngine;
using System.Collections;

public class InfiniteSlice : MonoBehaviour {

	public InfiniteSliceController controller;

	public int sliceId;

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player") {

			controller.crossGate (sliceId);

		}

	}

}
