﻿using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// checks if the shed is full in the PreQuakeHouse scene. If so, move on to the QuakeHouse scene
/// </summary>
public class PreQuakeHouseEventManager : MonoBehaviour
{
    public StorageContainer[] containers;
    
    void FixedUpdate()
    {
        if (AllContainersFull())
        {
            if (GlobalControls.globalControlsProperties.Contains("apartmentCondition"))
            {
                foreach (StorageContainer container in containers)
                {
                    GameObject item = container.contents;
                    if (item)
                    {
                        GlobalItemList.UpdateItemList(item.name, "QuakeApartment", item.transform.position, container.name);
                    }
                }

                SceneManager.LoadScene("QuakeApartment");
            }
            else
            {
                foreach (StorageContainer container in containers)
                {
                    GameObject item = container.contents;
                    if (item)
                    {
                        GlobalItemList.UpdateItemList(item.name, "QuakeHouse", item.transform.position, container.name);
                    }
                }

                SceneManager.LoadScene("QuakeHouse");
            }
        }
    }
    
    private bool AllContainersFull()
    {
        int goodPlaceFullCount = 0;
        foreach (StorageContainer container in containers)
        {
            if (container.contents && container.isGoodPlace)
            {
                goodPlaceFullCount++;
            }
        }

        if (goodPlaceFullCount < 2) return false;
        return true;
    }
}
