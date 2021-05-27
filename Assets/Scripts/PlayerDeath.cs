using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
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
    public void KillPlayer(string x, int y)
    {
        //CallingObject = GameObject.Find("Event Manager");
        //CallingObject.GetComponent("QuakeManager").enabled = false;
        playerDeath = true;
        player = GameObject.FindGameObjectWithTag("Player");
        playerMoverScript = player.GetComponent<PlayerMover>();
        player.GetComponent<PlayerMover>().enabled = false;
        canvas.SetActive(true);
    }
}
