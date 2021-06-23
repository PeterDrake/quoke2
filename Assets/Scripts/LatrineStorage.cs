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
        if (CheckPlywood() && CheckRope() && CheckShovel() && CheckTarp())
        {
            Debug.Log("Starting Latrine");
            return true;
        }
        return false;
    }
    
    public bool CheckShovel()
    {
        foreach (Item item in GlobalItemList.ItemList.Values)
        {
            if (item.name.Equals("Shovel"))
            {
                if (item.containerName.Equals("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    public bool CheckRope()
    {
        foreach (Item item in GlobalItemList.ItemList.Values)
        {
            if (item.name.Equals("Rope"))
            {
                if (item.containerName.Equals("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    public bool CheckTarp()
    {
        foreach (Item item in GlobalItemList.ItemList.Values)
        {
            if (item.name.Equals("Tarp"))
            {
                if (item.containerName.Equals("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    public bool CheckPlywood()
    {
        foreach (Item item in GlobalItemList.ItemList.Values)
        {
            if (item.name.Equals("Plywood"))
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
