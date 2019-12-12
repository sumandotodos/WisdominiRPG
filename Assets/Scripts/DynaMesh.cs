using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynaMesh : MonoBehaviour {

	public Camera cam;

	public Shader shader;

	public Material mat;

	float[] disturbKernel;
	int disturbKernelSize;
	float disturbRadius;

	float latticeConstant;

	Mesh mesh;
	public int size = 20;
	float length = 0.1f;
	public float amplitude = 0.2f;

	public Texture renderTexture;

	public float mass = 1.0f;
	public float damp = 0.1f;
	public float k = 1.0f;

	public float wellRadius = 6.0f;

	List<Vector3> vertices;
	float[] y_; // array of velocities;

	float phase;

	public void create(int n, float a) {

		latticeConstant = a;
		//List<Vector3> vertices;
		List<Vector2> uvs;
		List<int> tris;

		y_ = new float[n * n];

		vertices = new List<Vector3>();
		uvs = new List<Vector2>();
		tris = new List<int>();

		int i, j;
		float x, z;

		// generate all vertices in usual order

		z = -(n / 2) * a;
		for (i = 0; i < n; ++i) {

			x = -(n / 2) * a;
			for (j = 0; j < n; ++j) {

				y_ [i + j * n] = 0;
				vertices.Add(new Vector3(x, 0, z));
				uvs.Add(new Vector2(j/((float)n-1.0f), i/((float)n-1.0f)));
				x += a;

			}
			z += a;

		}


		// generate all tris
		for(i = 0; i<(n-1); ++i) {
			for(j=0; j<(n-1); ++j) {
				
				tris.Add(j 		+ i*n);
				tris.Add(j+n+1 	+ i*n);
				tris.Add(j+1 	+ i*n);

				tris.Add(j 		+ i*n);
				tris.Add(j+n 	+ i*n);
				tris.Add(j+n+1 	+ i*n);


			}
		}

		mesh.name = "RefractionMesh";
		mesh.vertices = vertices.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.RecalculateNormals();


	}

	// Use this for initialization
	void Start () {

		length = (1f / size) * 21.6f; // WARNING CONSTANTIZE MAGIC
		mesh = new Mesh ();
		create (size, length);
		MeshFilter meshFilter = (MeshFilter)this.gameObject.AddComponent(typeof(MeshFilter));
		meshFilter.mesh = mesh;
		MeshRenderer renderer = this.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		renderer.material.shader = shader; //Shader.Find ("Mobile/Bumped Specular");
		//Texture2D tex = new Texture2D(1, 1);
		//tex.SetPixel(0, 0, Color.green);
		//tex.Apply();
		//renderer.material.shader = shader;
		renderer.material = mat;
		renderer.material.mainTexture = renderTexture;
		renderer.material.color = Color.white;
		this.transform.localScale = new Vector3 (1, 2, 1);
		generateDisturbKernel (1.0f, latticeConstant);

	}

	public void generateDisturbKernel(float r, float a) {

		int ksize = 2*(int)(r / a);
		disturbKernelSize = ksize;
		disturbRadius = r;

		disturbKernel = new float[ksize * ksize];
		for (int i = 0; i < ksize; ++i) {
			for (int j = 0; j < ksize; ++j) {
				float dx = (-(ksize / 2) + i) * a;
				float dy = (-(ksize / 2) + j) * a;
				float dist = Mathf.Sqrt (dx * dx + dy * dy);
				disturbKernel [j + ksize * i] = Mathf.Exp (-(3.0f * dist / r));
			}
		}

	}

	public void disturb(float x, float z, float value) {

		int i0, j0, i, j;

		int convMax = (int)(disturbRadius / latticeConstant);

		j0 = (int)(x / latticeConstant) + size / 2;
		i0 = (int)(z / latticeConstant) + size / 2;


		for(i=-convMax; i<convMax; ++i) {
			for(j=-convMax; j<convMax; ++j) {
				Vector3 miV = vertices[j0 + j + (i0+i)*size]; // get vertex
				miV.y -= disturbKernel[(j+convMax) + (i+convMax)*disturbKernelSize] * value * Time.deltaTime;
				//miV.y -= value * Time.deltaTime;
				if (miV.y > 0.3f)
					miV.y = 0.3f;
				//miV.y = value;
				vertices [j0 + j + (i0+i) * size] = miV;
				//y_[j * i*size] = value;
			}
		}


	}

	public void disturb(float x, float z, float r, float value) {

		int i0, j0, i, j;

		int convMax = (int)(r / length);

		j0 = (int)(x / length) + size / 2;
		i0 = (int)(z / length) + size / 2;


		for(i=-convMax; i<(convMax+1); ++i) {
			for(j=-convMax; j<(convMax+1); ++j) {
				Vector3 miV = vertices[j0 + j + (i0+i)*size]; // get vertex
				miV.y += value * Time.deltaTime;
				if (miV.y > 0.3f)
					miV.y = 0.3f;
					//miV.y = value;
				vertices [j0 + j + (i0+i) * size] = miV;
				//y_[j * i*size] = value;
			}
		}


	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// let's see if this is fast enough

		// force test oscillation
		//Vector3 miV = vertices[size/2 + (size/2)*size]; // get center vertex
		//miV.y = amplitude*Mathf.Sin(phase);
		//vertices [size / 2 + (size / 2) * size] = miV;

		int i, j;
		for (i = 1; i < size-1; ++i) {
			for (j = 1; j < size-1; ++j) {

				float deltai = Mathf.Abs ((size / 2) - i);
				float deltaj = Mathf.Abs ((size / 2) - j);
				Vector2 vec = new Vector2 (deltaj, deltai);
				if (vec.magnitude * length > wellRadius) {
					Vector3 v = vertices [j + i * size];
					v.y = 0.0f;
					vertices [j + i * size] = v;
				} else {

					Vector3 v = vertices [j + i * size];
					//v.y = amplitude * Mathf.Sin (i + j + phase);
					float deltaY1 = vertices [(j + 1) + size * i].y - vertices [j + size * i].y;
					float deltaY2 = vertices [j + size * (i + 1)].y - vertices [j + size * i].y;
					float deltaY3 = vertices [(j - 1) + size * i].y - vertices [j + size * i].y;
					float deltaY4 = vertices [j + size * (i - 1)].y - vertices [j + size * i].y;

					/* calculate acceleration */
					float y__ = ((k * deltaY1) / mass + (k * deltaY2) / mass + (k * deltaY3) / mass + (k * deltaY4) / mass)
					           - (k * vertices [j + size * i].y) / mass - damp * y_ [j + size * i]; 

					/* accumulate acceleration into speed */
					y_ [j + size * i] += y__ * Time.deltaTime;

					/* accumulate speed into position */
					v.y += y_ [j + size * i] * Time.deltaTime;

					if (v.y > 0.5f)
						v.y = 0.5f;
					if (v.y < -0.5f)
						v.y = -0.5f;

					vertices [j + i * size] = v;
				}
			}
		}
		phase += Time.deltaTime;

		GetComponent<MeshFilter>().mesh.vertices = vertices.ToArray ();

		GetComponent<MeshFilter>().mesh.RecalculateNormals ();

		if (Input.GetMouseButton (0)) {
			RaycastHit vHit = new RaycastHit ();
			Ray vRay = cam.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (vRay, out vHit, 1000)) {
				float xxx = vHit.point.x;
				float zzz = vHit.point.z;
				//this.disturb (vHit.point.x, vHit.point.z, 0.66f, 8.3f);
				this.disturb(vHit.point.x, vHit.point.z, 16.6f);
			}
		}
	
	}
}
