using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemLoader : MonoBehaviour
{
    /* TODO: */
    /** Add public GameObject currentObject to collectible [Allows us to get a reference to the current object]
     *  For this to work
     *      Format foreach loop to make work good
     *      Create Prefabs for items
     *      ALL Items need to ALL be unique prefabs somehow in the scripts
     *      Instantiate GlobalControls list to have the collectibles with the correct prefabs and everything created
     *      *
     */
    
    public string scene;
    public Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        List<Collectible> localCopy = GlobalControls.ItemList;
        List<Collectible> inventoryList = new List<Collectible>(5);
        foreach (var collectible in localCopy)
        {
            if (scene.Equals(collectible.scene))
            {
                collectible.currentObject = Instantiate(collectible.prefab, collectible.location, Quaternion.identity);
            } 
            else if("Inventory".Equals(collectible.scene))
            {
                inventoryList.Add(collectible);
            }

        }
        
        inventoryList.Sort(SortByLocation);
        
        foreach (var collectible in inventoryList)
        {
            collectible.currentObject = Instantiate(collectible.prefab);
            inventory.PickUp(collectible.currentObject);
        }
    }
    
    private static int SortByLocation(Collectible a, Collectible b)
    {
        return a.location.x.CompareTo(b.location.x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}