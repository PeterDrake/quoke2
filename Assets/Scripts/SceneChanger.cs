using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public string sceneName;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            // Set GlobalControls to current scene
            string previousScene = SceneManager.GetActiveScene().name;
            switch (previousScene)
            {
                case "School":
                    GlobalControls.CurrentScene = 0;
                    break;
                case "Park":
                    GlobalControls.CurrentScene = 1;
                    break;
                case "Yard":
                    GlobalControls.CurrentScene = 2;
                    break;
                    
            }
            
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
        
}
