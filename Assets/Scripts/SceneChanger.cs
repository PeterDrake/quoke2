using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public string sceneToLoad;
    
    private readonly string[] previousScenes = {"School", "Park", "Yard"};
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set GlobalControls to current scene
            GlobalControls.CurrentScene = Array.IndexOf(previousScenes, SceneManager.GetActiveScene().name);
            SceneManager.LoadSceneAsync(sceneToLoad);
        }
    }
        
}
