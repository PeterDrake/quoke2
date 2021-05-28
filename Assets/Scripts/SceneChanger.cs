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
                        if (collectible.scene.Equals("School"))
                        {
                            if (collectible.prefab.activeInHierarchy)
                            {
                                collectible.location = collectible.prefab.transform.position;
                            }
                            else
                            {
                                collectible.scene = "Inventory";
                                //collectible.location = new Vector3(Array.IndexOf(inventory.items));
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
