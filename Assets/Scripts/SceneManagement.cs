using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour

{
    private readonly string[] previousScenes = {"School", "Park", "Yard"};

    public void Restart()
    {
        GlobalControls.Reset();
        GlobalItemList.Reset();
        ChangeScene("PreQuakeHouse");
    }

    /// <summary>
    /// This handles All "end of scene" things we need to do
    /// </summary>
    /// <param name="sceneToLoad"></param>
    public void ChangeScene(string sceneToLoad)
    {
        // Set GlobalControls to current scene
        GlobalControls.CurrentScene = Array.IndexOf(previousScenes, SceneManager.GetActiveScene().name);

        if (sceneToLoad.Equals("StrategicMap"))
        {
            GlobalControls.IsStrategicMap = true;
        }
        else if (sceneToLoad.Contains("Quake"))
        {
            GlobalControls.MetersEnabled = false;
        }
        else
        {
            GlobalControls.MetersEnabled = true;
            GlobalControls.IsStrategicMap = false;
            if(!GlobalControls.PoopTaskCompleted) GlobalControls.PoopTimeLeft--;
            if(!GlobalControls.WaterTaskCompleted) GlobalControls.WaterTimeLeft--;
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
