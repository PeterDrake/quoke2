using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoBucketScript : MonoBehaviour
{
    
    public GameObject contents;

    public bool bucketDone;
    public bool bucketTwoDone;
    public bool bagDone;
    public bool toiletPaperDone;
    public bool woodChipsDone;

    public GameObject bucketOne;
    public GameObject bucketTwo;
    public GameObject bag;
    public GameObject woodChips;
    public GameObject toiletPaper;

    public Meters meters;
    public ReferenceManager referenceManager;

    public void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        meters = referenceManager.metersCanvas.GetComponent<Meters>();
        bucketDone = GlobalControls.PoopTaskProgress[0];
        bucketTwoDone = GlobalControls.PoopTaskProgress[1];
        bagDone = GlobalControls.PoopTaskProgress[2];
        toiletPaperDone = GlobalControls.PoopTaskProgress[3];
        woodChipsDone = GlobalControls.PoopTaskProgress[4];
        
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (child.gameObject.name.Equals("Bucket 1")) bucketOne = child.gameObject;
            else if (child.gameObject.name.Equals("Bucket 2")) bucketTwo = child.gameObject;
            else if (child.gameObject.name.Equals("Bag")) bag = child.gameObject;
            else if (child.gameObject.name.Equals("Toilet Paper")) toiletPaper = child.gameObject;
            else if (child.gameObject.name.Equals("Wood Chips")) woodChips = child.gameObject;
        }
        UpdateVisuals();
    }


    public void UpdateVisuals()
    {
        if(bucketDone) bucketOne.SetActive(true);
        else bucketOne.SetActive(false);
        if(bucketTwoDone) bucketTwo.SetActive(true);
        else bucketTwo.SetActive(false);
        if(bagDone) bag.SetActive(true);
        else bag.SetActive(false);
        if(toiletPaperDone) toiletPaper.SetActive(true);
        else toiletPaper.SetActive(false);
        if(woodChipsDone) woodChips.SetActive(true);
        else woodChips.SetActive(false);
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
