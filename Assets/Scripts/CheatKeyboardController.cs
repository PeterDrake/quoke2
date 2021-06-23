﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatKeyboardController : MonoBehaviour
{
    // For scenes with no meters (before and during the quake), this can remain null
    public Meters meters;
    public SceneManagement sceneManagement;
    private string currentScene;

    void Start()
    {
        meters = GameObject.Find("Managers").GetComponent<ReferenceManager>().metersCanvas.GetComponent<Meters>();
        sceneManagement = GameObject.Find("Managers").GetComponent<ReferenceManager>().sceneManagement.GetComponent<SceneManagement>();
        currentScene = SceneManager.GetActiveScene().name;

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            sceneManagement.ChangeScene("PreQuakeHouse");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            sceneManagement.ChangeScene("QuakeHouse");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            sceneManagement.ChangeScene("StrategicMap");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            sceneManagement.ChangeScene("Park");
        }
        if (meters && Input.GetKeyDown(KeyCode.O))
        {
            meters.MarkTaskAsDone("water");
        }
        if (meters && Input.GetKeyDown(KeyCode.P))
        {
            meters.MarkTaskAsDone("poop");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            sceneManagement.Restart();
        }
        if (currentScene.Equals("Yard") && Input.GetKeyDown(KeyCode.U))
        {
            GameObject.Find("Quake Event Manager").GetComponent<QuakeManager>().TriggerQuake();
        }
        
    }
}
