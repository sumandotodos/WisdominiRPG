using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TransWorldCamera : MonoBehaviour {

	public float speed;
	float z;
	public float maxZ;
	MasterControllerScript mc;


	// Use this for initialization
	void Start () {
	
		z = this.transform.position.z;
		mc = GameObject.Find ("MasterController").GetComponent<MasterControllerScript> ();

	}
	
	// Update is called once per frame
	void Update () {

		z += speed * Time.deltaTime;
		this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, z);
		if (z > maxZ) {
			string returnLoc = mc.getStorage ().retrieveStringValue ("ReturnLocation");
			SceneManager.LoadScene (returnLoc);
		}
	
	}
}
