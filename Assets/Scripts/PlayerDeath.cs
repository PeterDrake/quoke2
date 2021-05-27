using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    public GameObject textChange;
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
                textChange.GetComponent<Text>().text = "You got crushed!";
                break;
            case 1:
                textChange.GetComponent<Text>().text = "You died in an aftershock!";
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
