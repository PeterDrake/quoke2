using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TradeScreenTester : MonoBehaviour
{
    private Inventory inventory;
    private StorageContainer container;
    private Dictionary<string, Item> itemList = new Dictionary<string, Item>
            {
                {"Wrench", new Item(new Vector3(0,0,0), "Wrench", 
                    "Inventory", "")},
                {"Water Bottle", new Item(new Vector3(1,0,0), "Water Bottle", 
                    "Inventory", "")},
                {"Sandwich", new Item(new Vector3(2,0,0), "Sandwich", 
                    "Inventory", "")},
                {"Shovel", new Item(new Vector3(0,0,0), "Shovel", 
                    "NPCName", "Inventory (NPC)")},
                {"Tarp", new Item(new Vector3(1,0,0), "Tarp", 
                    "NPCName", "Inventory (NPC)")},
                {"Bucket", new Item(new Vector3(2,0,0), "Bucket", 
                    "NPCName", "Inventory (NPC)")},
            };

    //weknowwhatstartdoesthx
    void Start()
    {
        //can i keep it? I like it <3
        GameObject g = GameObject.FindWithTag("Inventory");
        if (g)
        {
            inventory = g.GetComponent<Inventory>();
        }
        
        
        foreach (Item item in itemList.Values)
        {
            if (item.scene.Equals("Inventory") && inventory)
            {
                //populate inventory with many things
                GameObject prefab = (GameObject)Resources.Load(item.name, typeof(GameObject));
                GameObject itemInInventory = Instantiate(prefab, item.location, Quaternion.identity);
                inventory.PickUpAtSlot((int) item.location.x, itemInInventory);
            }
            else if (!item.containerName.Equals(""))
            {
                GameObject itemInContainer = GameObject.Find(item.containerName);
                if (itemInContainer)
                {
                    GameObject prefab = (GameObject)Resources.Load(item.name, typeof(GameObject));
                    GameObject itemInScene = Instantiate(prefab, item.location, Quaternion.identity);
                    itemInContainer.GetComponent<Inventory>().PickUpAtSlot((int) itemInScene.transform.position.x, itemInScene);

                }
            }
        }
    }
}