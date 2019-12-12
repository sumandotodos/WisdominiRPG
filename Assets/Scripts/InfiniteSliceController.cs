using UnityEngine;
using System.Collections;

public enum VectorDirection { positive, negative };

public class InfiniteSliceController : MonoBehaviour {

	public GameObject[] slice;
	public float sliceConstant;
	LevelControllerScript level;
	public string SliceControllerName;

	int nOfSlices;
	int sliceToMove;

	int advanceSlice;
	int retreatSlice;

	int actualSlice;

	public FakePanelAxis axis;
	public VectorDirection direction;

	void Start () {
	
		level = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		nOfSlices = slice.Length;
		advanceSlice = 2;
		retreatSlice = 0;

		int sliceToMove;
		actualSlice = level.retrieveIntValue (SliceControllerName + "slice");
		// take controller to previously stored state
		for (int i = 0; i < actualSlice; ++i) {
			sliceToMove = ((2+i) + nOfSlices - 2) % nOfSlices; // equivalent to (advanceSlice - 1) % nOfSlices
			moveSlice(sliceToMove, true);
			advanceSlice = (advanceSlice + 1) % nOfSlices;
			retreatSlice = (retreatSlice + 1) % nOfSlices;
		}
	}
	
	void Update () {
	
	}

	void moveSlice(int s, bool forward) 
	{
		float multiplicator = 1.0f;
		if (direction == VectorDirection.negative)
			multiplicator = -1.0f;

		if (!forward)
			multiplicator *= -1.0f;

		switch (axis) {
			case FakePanelAxis.x:
				slice [s].transform.localPosition =
				slice [s].transform.localPosition + new Vector3 (((float)nOfSlices) * sliceConstant * multiplicator, 0, 0);
				break;

			case FakePanelAxis.y:
				slice [s].transform.localPosition =
				slice [s].transform.localPosition + new Vector3 (0, ((float)nOfSlices) * sliceConstant * multiplicator, 0);
				break;

			case FakePanelAxis.z:
				slice [s].transform.localPosition =
				slice [s].transform.localPosition + new Vector3 (0, 0, ((float)nOfSlices) * sliceConstant * multiplicator);
				break;
		}
	}

	public void crossGate(int n) 
	{
		if (n == advanceSlice) 
		{
			int sliceToMove = (advanceSlice + nOfSlices - 2) % nOfSlices; // equivalent to (advanceSlice - 1) % nOfSlices
			moveSlice(sliceToMove, true);
			advanceSlice = (advanceSlice + 1) % nOfSlices;
			retreatSlice = (retreatSlice + 1) % nOfSlices;
			++actualSlice;
			level.storeIntValue (SliceControllerName + "slice", actualSlice);
		}

		else if (n == retreatSlice && actualSlice > 0) {
			int sliceToMove = (retreatSlice + nOfSlices - 1) % nOfSlices;
			moveSlice(sliceToMove, false);
			advanceSlice = (advanceSlice + nOfSlices - 1) % nOfSlices;
			retreatSlice = (retreatSlice + nOfSlices - 1) % nOfSlices;
			--actualSlice;
			level.storeIntValue (SliceControllerName + "slice", actualSlice);
		}
	}
}
