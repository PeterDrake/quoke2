using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{

    public string FirstScene;
    public string currentScene;

    public void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void Update()
    {
        if (currentScene.Equals("TitleScreen") && Input.GetKeyDown("space"))
        {
            GameObject.Find("Managers").GetComponent<ReferenceManager>().sceneManagement.GetComponent<SceneManagement>().Restart();
        }
    }
}
