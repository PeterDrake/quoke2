using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    private Text DeathText;
    private Text TipsText;
    private Text ScoreText;
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
                DeathText = text;
            }
            else if (text.gameObject.name.Equals("Tips Text"))
            {
                TipsText = text;
            }
            else if (text.name.Equals("Score Number"))
            {
                ScoreText = text;
            }
        }
    }

    public void DeathType(int x)
    {
        switch (x)
        {
            case 0:
                DeathText.text = "You got crushed!";
                TipsText.text = "Get under a solid structure during the earthquake!";
                ScoreText.text = "-1";
                break;
            case 1:
                DeathText.text = "You died in an aftershock!";
                TipsText.text = "Exit buildings as soon as the earthquake ends";
                ScoreText.text = "-1";
                break;
            case 2:
                DeathText.text = "You died of thirst!";
                TipsText.text = "Regular access to clean water is vital!";
                ScoreText.text = "-2";
                break;
            case 3:
                DeathText.text = "You got crushed!";
                TipsText.text = "Don't go through doors during the earthquake!";
                ScoreText.text = "-1";
                break;
            case 4:
                DeathText.text = "You died of unhygienic ways!";
                TipsText.text = "Staying sanitary is important!";
                ScoreText.text = "-1000";
                break;
            case 5:
                DeathText.text = "You got crushed!";
                TipsText.text = "Stay under cover until earthquake ends!";
                ScoreText.text = "-1";
                break;
            case 6:
                DeathText.text = "You died in an aftershock!";
                TipsText.text = "Don't re-enter unstable buildings!";
                ScoreText.text = "-1";
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
