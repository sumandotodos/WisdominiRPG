using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nuke : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MonoBehaviour[] objects = FindObjectsOfType<MonoBehaviour>();
        foreach(MonoBehaviour obj in objects)
        {
            if (obj != this)
            {
                Destroy(obj.gameObject);
            }
        }
        SceneManager.LoadScene("root");
    }

}
