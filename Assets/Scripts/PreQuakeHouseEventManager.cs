﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class PreQuakeHouseEventManager : MonoBehaviour
{
    public StorageContainer[] containers;
    
    void FixedUpdate()
    {
        if (AllContainersFull())
        {
            if (GlobalControls.ApartmentCondition)
            {
                foreach (StorageContainer container in containers)
                {
                    GameObject item = container.contents;
                    GlobalItemList.UpdateItemList(item.name, "QuakeApartment", item.transform.position, container.name);
                }

                SceneManager.LoadScene("QuakeApartment");
            }
            else
            {
                foreach (StorageContainer container in containers)
                {
                    GameObject item = container.contents;
                    GlobalItemList.UpdateItemList(item.name, "QuakeHouse", item.transform.position, container.name);
                }

                SceneManager.LoadScene("QuakeHouse");
            }
        }
    }
    
    private bool AllContainersFull()
    {
        foreach (StorageContainer container in containers)
        {
            if (!container.contents)
            {
                return false;
            }
        }
        return true;
    }
}
