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
            try
            {
                Inventory inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
            
                foreach (var collectible in GlobalControls.ItemList)
                {
                    if (collectible.currentObject.activeInHierarchy)
                    {
                        collectible.location = collectible.currentObject.transform.position;
                    }
                    else
                    {
                        var inventorySlot = Array.IndexOf(inventory.items, collectible.currentObject);
                        if (inventorySlot > -1)
                        {
                            collectible.scene = "Inventory";
                            collectible.location = new Vector3(inventorySlot, 0, 0);
                        }
                        else
                        {
                            collectible.scene = "Container/NPC";
                        }
                    }
                }
            }
            catch (NullReferenceException n)
            {
                Debug.Log("WARNING: Inventory Reference Null");
            }
            // Set GlobalControls to current scene
            GlobalControls.CurrentScene = Array.IndexOf(previousScenes, SceneManager.GetActiveScene().name);
            SceneManager.LoadSceneAsync(sceneToLoad);
            
        }
    }
        
}
