using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour

{
    private string[] previousScenes;

    private void Start()
    {
        if(GlobalControls.ApartmentCondition) previousScenes = new []{"School", "Park", "Street"};
        else previousScenes = new []{"School", "Park", "Yard"};
    }

    public void Restart()
    {
        GlobalControls.Reset();
        GlobalItemList.Reset();
        if (GlobalControls.ApartmentCondition)
        {
            ChangeScene("PreQuakeApartment");
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
            bool noStoredWater = true;
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

                if (item.name.Equals("Water Bottle(Clone)"))
                {
                    noStoredWater = false;
                }
            }

            if (noStoredWater)
            {
                GlobalControls.WaterTimeLeft = GlobalControls.NoStoredWaterTime;
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
        else if (sceneToLoad.Equals("QuakeApartment") && SceneManager.GetActiveScene().name.Equals("PreQuakeApartment"))
        {
            StorageContainer[] containers = new StorageContainer[]
            {
                GameObject.Find("Shelf 1").GetComponent<StorageContainer>(),
                GameObject.Find("Shelf 2").GetComponent<StorageContainer>(),
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
                    if (item.name.Equals("Chlorine Tablet"))
                    {
                        noStoredWater = false;
                    }
                    inventoryCount++;
                }
            }
            if (noStoredWater || inventoryCount != 2)
            {
                GlobalControls.WaterTimeLeft = GlobalControls.NoStoredWaterTime;
            }
        }
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
