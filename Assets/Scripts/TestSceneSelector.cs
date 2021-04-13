using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneSelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene("movement revised");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene("HouseScene");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("QuakeHouse");
        }
    }
}
