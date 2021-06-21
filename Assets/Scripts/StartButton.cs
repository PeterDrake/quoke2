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
            RestartGame();
        }
    }

    public void RestartGame()
    {
        GlobalControls.PoopTimeLeft = 24;
        GlobalControls.WaterTimeLeft = 12;
        GlobalControls.PoopTaskCompleted = false;
        GlobalControls.WaterTaskCompleted = false;
        GlobalControls.TurnNumber = 0;
        GlobalItemList.Reset();
        SceneManager.LoadScene(FirstScene);
    }
}
