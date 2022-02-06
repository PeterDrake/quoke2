using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour

{
    private string[] previousScenes; 
    private ReferenceManager referenceManager;
    private ObjectiveManager objectiveManager;

    
    private void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        objectiveManager = referenceManager.objectiveManager.GetComponent<ObjectiveManager>();

        if(GlobalControls.globalControlsProperties.Contains("apartmentCondition")) previousScenes = new []{"PSU", "WaterfrontPark", "Street", "PioneerCourthouseSquare"};
        else previousScenes = new []{ "PSU", "WaterfrontPark", "Yard", "PioneerCourthouseSquare" };
    }

    public void Restart()
    {
        GlobalControls.Reset();
        GlobalItemList.Reset();
        if (GlobalControls.globalControlsProperties.Contains("apartmentCondition"))
        {
            ChangeScene("PreQuakeApartmentStylizedv3");
        }
        else ChangeScene("PreQuakeHouse");
    }

    /// <summary>
    /// This handles All "end of scene" things we need to do
    /// </summary>
    /// <param name="sceneToLoad"></param>
    public void ChangeScene(string sceneToLoad)
    {
        // Set GlobalControls to current scene
        GlobalControls.currentScene = Array.IndexOf(previousScenes, SceneManager.GetActiveScene().name);

        if (sceneToLoad.Equals("StrategicMap"))
        {
            GlobalControls.globalControlsProperties.Add("isStrategicMap");
        }
        else if (sceneToLoad.Equals("GameEnd"))
        {
            GlobalControls.globalControlsProperties.Remove("metersEnabled");
        }
        else if (sceneToLoad.Contains("Quake"))
        {
            GlobalControls.globalControlsProperties.Remove("metersEnabled");
        }
        else
        {
            GlobalControls.globalControlsProperties.Add("metersEnabled");
            GlobalControls.globalControlsProperties.Remove("isStrategicMap");
            if(!GlobalControls.globalControlsProperties.Contains("poopTaskCompleted")) GlobalControls.poopTimeLeft--;
            if(!GlobalControls.globalControlsProperties.Contains("waterTaskCompleted")) GlobalControls.waterTimeLeft--;
        }

       
        if (sceneToLoad.Equals("Yard") && SceneManager.GetActiveScene().name.Equals("QuakeHouse"))
        {
            bool noStoredWater = true;
            StorageContainer[] containers = new StorageContainer[]
            {
                GameObject.Find("Shed 1").GetComponent<StorageContainer>(),
                GameObject.Find("Shed 2").GetComponent<StorageContainer>(),
                GameObject.Find("Cabinet 1").GetComponent<StorageContainer>(),
                GameObject.Find("Cabinet 2").GetComponent<StorageContainer>(),
                GameObject.Find("Cabinet 3").GetComponent<StorageContainer>(),
                GameObject.Find("Cabinet 4").GetComponent<StorageContainer>(),
            };
            foreach (StorageContainer container in containers)
            {
                GameObject item = container.contents;
                if (item)
                {
                    GlobalItemList.UpdateItemList(item.name, "Yard", item.transform.position, container.name);
                }

                if (item && item.name.Equals("Dirty Water Bottle(Clone)") && (container.name.Equals("Shed 1") || container.name.Equals("Shed 2")))
                {
                    noStoredWater = false;
                }
            }

            if (noStoredWater)
            {
                GlobalControls.waterTimeLeft = GlobalControls.noStoredWaterTime;
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
                GameObject.Find("Cabinet 3").GetComponent<StorageContainer>(),
                GameObject.Find("Cabinet 4").GetComponent<StorageContainer>(),
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
        else if (sceneToLoad.Equals("QuakeApartment") && SceneManager.GetActiveScene().name.Equals("PreQuakeApartmentStylizedv3"))
        {
            StorageContainer[] containers = new StorageContainer[]
            {
                GameObject.Find("Shelf 1").GetComponent<StorageContainer>(),
                GameObject.Find("Shelf 2").GetComponent<StorageContainer>(),
                GameObject.Find("Shelf 3").GetComponent<StorageContainer>(),
                GameObject.Find("Shelf 4").GetComponent<StorageContainer>(),
                GameObject.Find("Go Bag 1").GetComponent<StorageContainer>(),
                GameObject.Find("Go Bag 2").GetComponent<StorageContainer>(),
            };
            foreach (StorageContainer container in containers)
            {
                GameObject item = container.contents;
                if (item)
                {
                    GlobalItemList.UpdateItemList(item.name, "QuakeApartment", item.transform.position, container.name);
                }
            }
        }
        else if (sceneToLoad.Equals("Street") && SceneManager.GetActiveScene().name.Equals("QuakeApartment"))
        {
            bool noStoredWater = true;
            int inventoryCount = 0;
            foreach (Item item in GlobalItemList.ItemList.Values)
            {
                if (item.scene.Equals("Inventory") && item.containerName.Equals("Player"))
                {
                    if (item.name.Equals("Bleach"))
                    {
                        noStoredWater = false;
                    }
                    inventoryCount++;
                }
            }
            if (noStoredWater || inventoryCount != 2)
            {
                GlobalControls.waterTimeLeft = GlobalControls.noStoredWaterTime;
            }
        }
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
