using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    public GameObject DeathText;
    public GameObject TipsText;
    public bool playerDeath;
    private GameObject canvas;

    private void Start()
    {
        playerDeath = false;
        canvas = GameObject.Find("Managers").GetComponent<ReferenceManager>().deathCanvas;
        
        foreach (Text text in canvas.GetComponentsInChildren<Text>(true))
        {
            if (text.gameObject.name.Equals("Death Reason"))
            {
                DeathText = text.gameObject;
            }
            else if (text.gameObject.name.Equals("Tips Text"))
            {
                TipsText = text.gameObject;
            }
        }
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
                TipsText.GetComponent<Text>().text = "Exit buildings as soon as the earthquake ends";
                break;
            case 2:
                DeathText.GetComponent<Text>().text = "You died of thirst!";
                TipsText.GetComponent<Text>().text = "Regular access to clean water is vital!";
                break;
            case 3:
                DeathText.GetComponent<Text>().text = "You got crushed!";
                TipsText.GetComponent<Text>().text = "Don't go through doors during the earthquake!";
                break;
            case 4:
                DeathText.GetComponent<Text>().text = "You died of unhygienic ways!";
                TipsText.GetComponent<Text>().text = "Staying sanitary is important!";
                break;
            case 5:
                DeathText.GetComponent<Text>().text = "You got crushed!";
                TipsText.GetComponent<Text>().text = "Stay under cover until earthquake ends!";
                break;
            case 6:
                DeathText.GetComponent<Text>().text = "You died in an aftershock!";
                TipsText.GetComponent<Text>().text = "Don't re-enter unstable buildings!";
                break;
        }
    }
    public void KillPlayer(GameObject callingObject, int y)
    {
        GameObject.Find("Managers").GetComponent<ReferenceManager>().keyboardManager.GetComponent<PlayerKeyboardManager>().SetDeath();
        playerDeath = true;
        callingObject.SetActive(false);
        DeathType(y);
    }
}
