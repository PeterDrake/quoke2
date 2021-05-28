using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public string sceneName;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set GlobalControls to current scene
            string previousScene = SceneManager.GetActiveScene().name;
            switch (previousScene)
            {
                case "School":
                    Inventory inventory = GameObject.Find("Inventory").GetComponent<Inventory>();    
                    GlobalControls.CurrentScene = 0;
                    foreach (var collectible in GlobalControls.ItemList)
                    {
                        if (collectible.currentObject.activeInHierarchy)
                        {
                            collectible.location = collectible.currentObject.transform.position;
                        }
                        else
                        {
                            var inventorySlot = Array.IndexOf<GameObject>(inventory.items, collectible.currentObject);
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
                    break;
                case "Park":
                    GlobalControls.CurrentScene = 1;
                    break;
                case "Yard":
                    GlobalControls.CurrentScene = 2;
                    break;
                    
            }
            
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
        
}
