using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemLoader : MonoBehaviour
{
    private Inventory inventory;
    private StorageContainer container;

    void Start()
    {
        List<Item> itemsToChange = new List<Item>();
        if (SceneManager.GetActiveScene().name.Equals("PreQuakeApartment"))
        {
            foreach (Item item in GlobalItemList.ItemList.Values)
            {
                if (item.scene.Equals("PreQuakeHouse"))
                {
                    itemsToChange.Add(item);
                }
            }
        }

        foreach (Item item in itemsToChange)
        {
            GlobalItemList.UpdateItemList(item.name, "PreQuakeApartment", new Vector3(item.location.x - 2, item.location.y, item.location.z), item.containerName);
        }
        GameObject g = GameObject.FindWithTag("Inventory");
        if (g)
        {
            inventory = g.GetComponent<Inventory>();
        }
        foreach (Item item in GlobalItemList.ItemList.Values)
        {
            if (item.scene.Equals(SceneManager.GetActiveScene().name) && item.containerName.Equals(""))
            {
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
                GameObject itemInInventory = Instantiate(prefab, item.location, Quaternion.identity);
                inventory.PickUpAtSlot((int) item.location.x, itemInInventory);
            }

            //case when we have an occupied container in the scene to be loaded
            else if (item.scene.Equals(SceneManager.GetActiveScene().name) && !item.containerName.Equals(""))
            {
                GameObject itemInContainer = GameObject.Find(item.containerName);
                if (itemInContainer)
                {
                    GameObject prefab = (GameObject)Resources.Load(item.name, typeof(GameObject));
                    // prefab.transform.position = item.location;
                    GameObject itemInScene = Instantiate(prefab, item.location, Quaternion.identity);
                    itemInScene.transform.position = item.location;
                    
                    //place this item into the storage container's contents
                    itemInContainer.GetComponent<StorageContainer>().contents = itemInScene;

                }
            }
        }
    }

    
}
