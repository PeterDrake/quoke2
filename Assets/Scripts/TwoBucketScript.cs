using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoBucketScript : MonoBehaviour
{
    
    public GameObject contents;

    public bool bucketDone;
    public bool bucketTwoDone;
    public bool bagDone;
    public bool woodChipsDone;
    public bool toiletPaperDone;

    public Meters meters;
    public ReferenceManager referenceManager;

    public void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        meters = referenceManager.metersCanvas.GetComponent<Meters>();
    }


    /// Removes and returns the currently stored item (or null if there is no such item)
    public GameObject RemoveBucketItem()
    {
        GameObject temp = contents;
        contents = null;
        return temp;
    }
    
    public bool CheckAllBucketItems()
    {
        if (inventoryHasItem("Bucket") || inventoryHasItem("Bucket 2") || inventoryHasItem("Bag") || inventoryHasItem("Wood Chips")
        || inventoryHasItem("Toilet Paper"))
        {
            return true;
        }
        return false;
    }

    public bool inventoryHasItem(string itemName)
    {
        foreach (Item item in GlobalItemList.ItemList.Values)
        {
            if (itemName.Equals(item.name))
            {
                if (item.containerName.Equals("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool TwoBucketComplete()
    {
        if (!bucketDone) return false;
        if (!bucketTwoDone) return false;
        if (!bagDone) return false;
        if (!toiletPaperDone) return false;
        if (!woodChipsDone) return false;
        Debug.Log("Completed Two Bucket System uwu");
        meters.MarkTaskAsDone("poop");
        return true;
    }
}
