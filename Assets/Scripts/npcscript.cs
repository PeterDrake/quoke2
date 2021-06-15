using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class npcscript : MonoBehaviour
{

    /// Update brings back the previous scene when player presses the escape key
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameObject.Find("NpcCanvas"))
        {
            GameObject.Find("Player").GetComponent<PlayerMover>().enabled = true;
            GameObject.Find("NpcCanvas").SetActive(false);
        }
        
    }

}
