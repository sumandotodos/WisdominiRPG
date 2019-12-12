using UnityEngine;
using System.Collections;

public class WaterRipple : MonoBehaviour {

	float minX;
	float minZ;
	float maxX;
	float maxZ;

	/* references */

	public Camera cam;

	public GameObject tank;
	Mesh mesh;

	/* public properties */

	public float k;
	public float b;
	public float mass;
	public float amplitude= 0.03f;
	public float pressY = -0.04f;
	public float fingerThickness = 6.0f; // in mattress units

	/* properties */

	Vector3[] vtx;
	int dimension;
	float angle;
	const float angleSpeed = 12.0f;
	//const float amplitude = 0.1f;

	float y__;
	float [] y_;
	int [] reorder;

	int x, z;
	// Use this for initialization
	void Start () {



		mesh = tank.GetComponent<MeshFilter> ().mesh;
		vtx = mesh.vertices;
		dimension = (int)Mathf.Sqrt (vtx.Length);
		reorder = new int [vtx.Length];

		minX = 10000.0f;
		minZ = 10000.0f;
		maxX = -10000.0f;
		maxZ = -10000.0f;
		for (int i = 0; i < vtx.Length; ++i) {

			if (vtx [i].x < minX)
				minX = vtx [i].x;
			if (vtx [i].x > maxX)
				maxX = vtx [i].x;
			if (vtx [i].z < minZ)
				minZ = vtx [i].z;
			if (vtx [i].z > maxZ)
				maxZ = vtx [i].z;

		}

		// reorder the damned vertices
		for (int i = 0; i < dimension; ++i) {
	
			float targetZ = minZ + i * (maxZ - minZ) / dimension;
			for (int j = 0; j < dimension; ++j) {

				float targetX = minX + j * (maxZ - minX) / dimension;

				// find the vertex closest to (targetX, targetZ)
				Vector2 target = new Vector2(targetX, targetZ);
				float minDistance = 10000.0f;
				for (int k = 0; k < vtx.Length; ++k) {
					Vector2 testVtx = new Vector2 (vtx [k].x, vtx [k].z);
					float dist = (testVtx - target).magnitude;
					if (dist < minDistance) {
						reorder [j + i * dimension] = k;
						minDistance = dist;
					}
				}

			}

		}

		// now we can access worldvertex j, i by vtx[reorder[j + i*dimension]]

		y_ = new float [vtx.Length];
		for (int i = 0; i < vtx.Length; ++i) {
			y_ [0] = 0.0f;
		}
		angle = 0.0f;

		x = z = -1;
	
	}

	// Update is called once per frame
	void Update () {

		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);

		x = z = -1;
		if (Input.GetMouseButton (0)) {
			if (Physics.Raycast (ray, out hit)) {
				GameObject objectHit = hit.transform.gameObject;

				if (objectHit.tag == "Mattress") {

					Vector3 worldCoords;
					worldCoords = hit.point;
					int hitX, hitZ;
					hitX = (int)((worldCoords.x/100.0f - minX) * ((float)dimension / (maxX - minX)));
					hitZ = (int)((worldCoords.z/100.0f - minZ) * ((float)dimension / (maxZ - minZ)));
					if (hitX < 0)
						hitX = 0;
					if (hitX > dimension - 1)
						hitX = dimension - 1;
					if (hitZ < 0)
						hitZ = 0;
					if (hitZ > dimension - 1)
						hitZ = dimension - 1;

					x = hitX;
					z = hitZ;


				}


			}
		}

		/* force oscillation */
		angle += angleSpeed * Time.deltaTime;

	
		if (x != -1 && z != -1) {
			Vector2 here = new Vector2 (x, z);
			for(int i = 0; i<dimension; ++i) {
				for(int j = 0; j<dimension; ++j) {
					Vector2 there = new Vector2 (j, i);
					float mag = (there - here).magnitude;
					if( mag < fingerThickness) {
						if(mag > 0.5f) {
						vtx [reorder [j + i * dimension]].y = pressY/mag; //amplitude * Mathf.Sin (angle);
						}
						else vtx [reorder [j + i * dimension]].y = pressY;
					}
				}
			}
		}

		for (int i = 1; i < dimension - 1; ++i) {

			for (int j = 1; j < dimension - 1; ++j) {

				/* calculate deltas */
				float deltaY1 = vtx [reorder[(j + 1) + dimension * i]].y - vtx [reorder[j + dimension * i]].y;
				float deltaY2 = vtx [reorder[j +  dimension * (i+1)]].y - vtx [reorder[j + dimension * i]].y;
				float deltaY3 = vtx [reorder[(j - 1) + dimension * i]].y - vtx [reorder[j + dimension * i]].y;
				float deltaY4 = vtx [reorder[j + dimension * (i-1)]].y - vtx [reorder[j + dimension * i]].y;

				/* calculate acceleration */
				y__ = ((k * deltaY1)/mass + (k * deltaY2)/mass + (k * deltaY3)/mass + (k * deltaY4)/mass) 
					- (k*vtx [reorder[j + dimension * i]].y)/mass - b*y_ [j + dimension * i];

				/* accumulate acceleration into speed */
				y_ [j + dimension * i] += y__ * Time.deltaTime;

				/* accumulate speed into position */
				vtx [reorder[j + dimension * i]].y += y_ [j + dimension * i] * Time.deltaTime;

			}

		}


		mesh.vertices = vtx;
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds();
	
	}

	void chorra() {
		
	}
}
