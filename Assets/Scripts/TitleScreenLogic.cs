using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenLogic : MonoBehaviour
{
    public string DestinationScene = "HouseScene";
    public void clickStartButton()
    {
        SceneManager.LoadScene(DestinationScene);
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            clickStartButton();
        }
    }
}
