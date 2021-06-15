using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemLoader : MonoBehaviour
{
    private Inventory inventory;
    private StorageContainer container;

    //weknowwhatstartdoesthx
    void Start()
    {
        
        //DEBUG
        foreach (Item value in GlobalItemList.ItemList.Values)
        {
            //Debug.Log(value.name + " in " + value.scene + " at " + value.location + " with container " + value.containerName);
            Debug.Log(value.ToString());
        }
        
        
        //can i keep it?
        GameObject g = GameObject.FindWithTag("Inventory");
        if (g)
        {
            inventory = g.GetComponent<Inventory>();
        }
        foreach (Item item in GlobalItemList.ItemList.Values)
        {
            if (item.scene.Equals(SceneManager.GetActiveScene().name) && item.containerName.Equals(""))
            {
                // Debug.Log("When item is not in storage :" + item.containerName);
                //poot item here
                GameObject prefab = (GameObject)Resources.Load(item.name, typeof(GameObject));
                // prefab.transform.position = item.location;
                GameObject itemInScene = Instantiate(prefab, item.location, Quaternion.identity);
                itemInScene.transform.position = item.location;

            }
            else if (item.scene.Equals("Inventory") && item.containerName.Equals("Player") && inventory)
            {
                //populate inventory with many things
                GameObject prefab = (GameObject)Resources.Load(item.name, typeof(GameObject));
                // inventory.PickUpAtSlot((int) item.location.x, prefab);
                GameObject itemInInventory = Instantiate(prefab, item.location, Quaternion.identity);
                inventory.PickUpAtSlot((int) item.location.x, itemInInventory);
            }

            //case when we have an occupied container in the scene to be loaded
            else if (item.scene.Equals(SceneManager.GetActiveScene().name) && !item.containerName.Equals(""))
            {
                GameObject itemInContainer = GameObject.Find(item.containerName);
                if (itemInContainer)
                {
                    GameObject prefab = (GameObject) AssetDatabase.LoadAssetAtPath(item.name, typeof(GameObject));
                    // prefab.transform.position = item.location;
                    GameObject itemInScene = Instantiate(prefab, item.location, Quaternion.identity);
                    itemInScene.transform.position = item.location;
                    
                    //place this item into the storage container's contents
                    // Debug.Log("<" + item.containerName + ">");
                    itemInContainer.GetComponent<StorageContainer>().contents = itemInScene;

                }
            }
        }
    }

    
}
