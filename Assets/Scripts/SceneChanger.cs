using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public string sceneName;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Changing to scene " + sceneName);
        SceneManager.LoadSceneAsync(sceneName);
    }
        
}
