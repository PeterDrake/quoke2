using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public string sceneName;
    
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
        
}
