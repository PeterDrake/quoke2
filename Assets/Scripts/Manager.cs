using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public GameObject npcScreen;
    public GameObject tradingScreen;
    public GameObject player;
    public GameObject playerInventory;
    public GameObject interactor;

    // Start is called before the first frame update
    void Awake()
    {
        npcScreen = GameObject.Find("NpcCanvas");
        tradingScreen = GameObject.Find("Trading Screen");
        player = GameObject.Find("Player");
        playerInventory = GameObject.FindWithTag("Inventory");
        interactor = GameObject.Find("Interactor");
    }
}