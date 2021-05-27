using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    public GameObject DeathText;
    public GameObject TipsText;
    public bool playerDeath;
    private GameObject player;
    //private GameObject CallingObject;
    private PlayerMover playerMoverScript;
    private GameObject canvas;

    public void DeactivateKillScreen(GameObject y)
    {
        canvas = y;
        canvas.SetActive(false);
    }

    public void DeathType(int x)
    {
        switch (x)
        {
            case 0:
                DeathText.GetComponent<Text>().text = "You got crushed!";
                TipsText.GetComponent<Text>().text = "Get under a solid structure during the earthquake!";
                break;
            case 1:
                DeathText.GetComponent<Text>().text = "You died in an aftershock!";
                TipsText.GetComponent<Text>().text = "Don't re-enter unstable structures!";
                break;
            case 2:
                DeathText.GetComponent<Text>().text = "You died of thirst!";
                TipsText.GetComponent<Text>().text = "Regular access to clean water is vital!";
                break;
        }
    }
    public void KillPlayer(string x, int y)
    {
        //CallingObject = GameObject.Find("Event Manager");
        //CallingObject.GetComponent("QuakeManager").enabled = false;
        playerDeath = true;
        player = GameObject.FindGameObjectWithTag("Player");
        playerMoverScript = player.GetComponent<PlayerMover>();
        player.GetComponent<PlayerMover>().enabled = false;
        canvas.SetActive(true);
        DeathType(y);
     
    }
}
