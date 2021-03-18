using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenMap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OpenMap: Collision");
        SceneManager.LoadScene("StrategicMap");
    }
}
