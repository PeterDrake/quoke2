using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatrineStorage : MonoBehaviour
{

    public int timesShoveled;
    public GameObject contents;

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
            Debug.Log("Starting Latrine");
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
        timesShoveled = 0;
        return false;
    }

    public bool PlywoodComplete()
    {
        return false;
    }

    public bool RopeComplete()
    {
        return false;
    }

    public bool TarpComplete()
    {
        return false;
    }

    public bool LatrineComplete()
    {
        if (!ShovelingComplete()) return false;
        if (!PlywoodComplete()) return false;
        if (!RopeComplete()) return false;
        if (!TarpComplete()) return false;
        return true;
    }
    
    
    
}
