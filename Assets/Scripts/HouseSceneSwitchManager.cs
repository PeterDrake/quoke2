using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseSceneSwitchManager : MonoBehaviour
{
    public StorageContainer[] containerArray;

    // Update is called once per frame
    void Update()
    {
        if (checkIfAllFull())
        {
            SceneManager.LoadScene("QuakeHouse");
        }
    }
    
    public bool checkIfAllFull()
    {
        foreach (StorageContainer container in containerArray)
        {
            if (!container.isItemStored())
            {
                return false;
            }
        }
        return true;
    }
}
