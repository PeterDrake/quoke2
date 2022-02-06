using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// the brain behind the start button of the game
/// </summary>
public class StartButton : MonoBehaviour
{
    
    private string currentScene;

    public void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void Update()
    {
        if (currentScene.Equals("TitleScreen") && Input.GetKeyDown("space"))
        {
            GameObject.Find("Scene Management").GetComponent<SceneManagement>().Restart();
        }
        else if (currentScene.Equals("GameEnd") && Input.GetKeyDown("space"))
        {
            GameObject.Find("Scene Management").GetComponent<SceneManagement>().Restart();
        }
        
    }
}
