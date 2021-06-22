using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour

{
    private readonly string[] previousScenes = {"School", "Park", "Yard"};
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene(string sceneToLoad)
    {
        // Set GlobalControls to current scene
        GlobalControls.CurrentScene = Array.IndexOf(previousScenes, SceneManager.GetActiveScene().name);

        if (sceneToLoad.Equals("StrategicMap"))
        {
            GlobalControls.IsStrategicMap = true;
            Debug.Log("IsStrategicMap = " + GlobalControls.IsStrategicMap);
        }
        else
        {
            GlobalControls.IsStrategicMap = false;
            Debug.Log("IsStrategicMap = " + GlobalControls.IsStrategicMap);
        }
            
        if (sceneToLoad.Equals("Yard") && SceneManager.GetActiveScene().name.Equals("QuakeHouse"))
        {
            StorageContainer[] containers = new StorageContainer[]
            {
                GameObject.Find("Shed 1").GetComponent<StorageContainer>(),
                GameObject.Find("Shed 2").GetComponent<StorageContainer>(),
                GameObject.Find("Cabinet 1").GetComponent<StorageContainer>(),
                GameObject.Find("Cabinet 2").GetComponent<StorageContainer>(),
            };
            foreach (StorageContainer container in containers)
            {
                GameObject item = container.contents;
                if (item)
                {
                    GlobalItemList.UpdateItemList(item.name, "Yard", item.transform.position, container.name);
                }
            }
        }
        
        else if (sceneToLoad.Equals("QuakeHouse") && SceneManager.GetActiveScene().name.Equals("PreQuakeHouse"))
        {
            StorageContainer[] containers = new StorageContainer[]
            {
                GameObject.Find("Shed 1").GetComponent<StorageContainer>(),
                GameObject.Find("Shed 2").GetComponent<StorageContainer>(),
                GameObject.Find("Cabinet 1").GetComponent<StorageContainer>(),
                GameObject.Find("Cabinet 2").GetComponent<StorageContainer>(),
            };
            foreach (StorageContainer container in containers)
            {
                GameObject item = container.contents;
                if (item)
                {
                    GlobalItemList.UpdateItemList(item.name, "QuakeHouse", item.transform.position, container.name);
                }
            }
        }
        
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
