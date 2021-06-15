using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TradeScreenTester : MonoBehaviour
{
    private Inventory inventory;
    public string npcName;
    private StorageContainer container;
    private Dictionary<string, Item> itemList = new Dictionary<string, Item>
            {
                {"Wrench", new Item(new Vector3(0,0,0), "Wrench", 
                    "Inventory", "Player")},
                {"Water Bottle", new Item(new Vector3(1,0,0), "Water Bottle", 
                    "Inventory", "Player")},
                {"Sandwich", new Item(new Vector3(2,0,0), "Sandwich", 
                    "Inventory", "Player")},
                {"Shovel", new Item(new Vector3(0,0,0), "Shovel", 
                    "Inventory", "safi0")},
                {"Tarp", new Item(new Vector3(1,0,0), "Tarp", 
                    "Inventory", "safi0")},
                {"Bucket", new Item(new Vector3(2,0,0), "Bucket", 
                    "Inventory", "safi0")},
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

        npcName = this.gameObject.GetComponent<TradingScreenManager>().npcName;
        
        foreach (Item item in itemList.Values)
        {
            if (item.scene.Equals("Inventory") && item.containerName.Equals("Player") && inventory)
            {
                //populate inventory with many things
                GameObject prefab = (GameObject)Resources.Load(item.name, typeof(GameObject));
                GameObject itemInInventory = Instantiate(prefab, item.location, Quaternion.identity);
                inventory.PickUpAtSlot((int) item.location.x, itemInInventory);
            }
            else if (item.scene.Equals("Inventory") && item.containerName.Equals(npcName))
            {
                GameObject inventoryNPC = GameObject.Find("Inventory (NPC)");
                if (inventoryNPC)
                {
                    GameObject prefab = (GameObject)Resources.Load(item.name, typeof(GameObject));
                    GameObject itemInScene = Instantiate(prefab, item.location, Quaternion.identity);
                    inventoryNPC.GetComponent<Inventory>().PickUpAtSlot((int) itemInScene.transform.position.x, itemInScene);

                }
            }
        }
    }
}