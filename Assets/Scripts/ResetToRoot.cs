using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetToRoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        MasterControllerScript mc = FindObjectOfType<MasterControllerScript>();
        if(mc == null)
        {
            PlayerPrefs.SetString("ForcedLevel", SceneManager.GetActiveScene().name);
            Debug.Log("Quickstarting: " + SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("root");
        }
    }
}
