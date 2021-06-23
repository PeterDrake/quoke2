using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatrineStorage : MonoBehaviour
{

    public int timesShoveled;
    public GameObject contents;

    public bool shovelingDone;
    public bool plywoodDone;
    public bool ropeDone;
    public bool tarpDone;

    public Meters meters;
    public ReferenceManager referenceManager;

    public void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        meters = referenceManager.metersCanvas.GetComponent<Meters>();
    }


    /// Removes and returns the currently stored item (or null if there is no such item)
    public GameObject RemoveLatrineItem()
    {
        GameObject temp = contents;
        contents = null;
        return temp;
    }
    
    public bool CheckAllLatrineItems()
    {
        if (inventoryHasItem("Shovel") || inventoryHasItem("Tarp") || inventoryHasItem("Rope") || inventoryHasItem("Plywood"))
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



    public bool ShovelingComplete()
    {
        if (timesShoveled >= 2 && timesShoveled <= 4)
        {
            return true;
        }
        return false;
    }


    public bool LatrineComplete()
    {
        if (!ShovelingComplete()) return false;
        if (!plywoodDone) return false;
        if (!ropeDone) return false;
        if (!tarpDone) return false;
        Debug.Log("Completed latrine uwu");
        meters.MarkTaskAsDone("poop");
        return true;
    }
    
    
    
}
