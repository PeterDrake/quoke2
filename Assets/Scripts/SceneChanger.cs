using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public string sceneToLoad;
    
    private readonly string[] previousScenes = {"School", "Park", "Yard"};
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set GlobalControls to current scene
            GlobalControls.CurrentScene = Array.IndexOf(previousScenes, SceneManager.GetActiveScene().name);
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
            SceneManager.LoadSceneAsync(sceneToLoad);
        }
    }
        
}
