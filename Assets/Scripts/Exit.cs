﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{

    public string sceneToLoad;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.Find("Managers").GetComponent<ReferenceManager>().sceneManagement
                .GetComponent<SceneManagement>().ChangeScene(sceneToLoad);
        }
    }
        
}
