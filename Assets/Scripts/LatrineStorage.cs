using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatrineStorage : MonoBehaviour
{

    private int timesShoveled = 0;

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
}
