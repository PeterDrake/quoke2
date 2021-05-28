using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemLoader : MonoBehaviour
{

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
                Instantiate(collectible.prefab, collectible.location, Quaternion.identity);
            } 
            else if("Inventory".Equals(collectible.scene))
            {
                inventoryList.Add(collectible);
            }

        }
        
        inventoryList.Sort(SortByLocation);
        
        foreach (var collectible in inventoryList)
        {
            inventory.PickUp(Instantiate(collectible.prefab));
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
