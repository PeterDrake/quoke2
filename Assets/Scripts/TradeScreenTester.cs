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
                    "Assets/Resources/Wrench.prefab" , "Inventory", "")},
                {"Water Bottle", new Item(new Vector3(1,0,0), "Water Bottle", 
                    "Assets/Resources/Water Bottle.prefab" , "Inventory", "")},
                {"Sandwich", new Item(new Vector3(2,0,0), "Sandwich", 
                    "Assets/Resources/Sandwich.prefab" , "Inventory", "")},
                {"Shovel", new Item(new Vector3(0,0,0), "Shovel", 
                    "Assets/Resources/Shovel.prefab", "NPCName", "Inventory (NPC)")},
                {"Tarp", new Item(new Vector3(1,0,0), "Tarp", 
                    "Assets/Resources/Tarp.prefab", "NPCName", "Inventory (NPC)")},
                {"Bucket", new Item(new Vector3(2,0,0), "Bucket", 
                    "Assets/Resources/Bucket.prefab", "NPCName", "Inventory (NPC)")},
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
                GameObject prefab = (GameObject) AssetDatabase.LoadAssetAtPath(item.prefab, typeof(GameObject));
                GameObject itemInInventory = Instantiate(prefab, item.location, Quaternion.identity);
                inventory.PickUpAtSlot((int) item.location.x, itemInInventory);
            }
            else if (!item.containerName.Equals(""))
            {
                GameObject itemInContainer = GameObject.Find(item.containerName);
                if (itemInContainer)
                {
                    GameObject prefab = (GameObject) AssetDatabase.LoadAssetAtPath(item.prefab, typeof(GameObject));
                    GameObject itemInScene = Instantiate(prefab, item.location, Quaternion.identity);
                    itemInContainer.GetComponent<Inventory>().PickUpAtSlot((int) itemInScene.transform.position.x, itemInScene);

                }
            }
        }
    }
}

public class itemAttribs
{
    public List<string> itemName;
    public bool tradeable;
    public List<string> equivalences;

    //if itemName length == 1 , only need that one item
    //if itemName length == 2 , need both items
    //fork and spoon equivalent to plate
    //if only one, no plate
    //if both, one plate
}
public class ItemCopy
{
    public Vector3 location;
    public string itemName;
    public string prefab;
    public string scene;
    public string containerName;

    public ItemCopy(Vector3 location, string itemName, string prefab, string scene, string containerName)
    {
        this.location = location;
        this.itemName = itemName;
        this.prefab = prefab;
        this.scene = scene;
        this.containerName = containerName;
    }
}
