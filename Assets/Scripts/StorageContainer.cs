using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StorageContainer : MonoBehaviour
{
    public GameObject storedItem;

    /// Removes and returns the currently stored item (or null if there is no such item)
    public GameObject RemoveItem()
    {
        GameObject temp = storedItem;
        storedItem = null;
        return temp;
    }
    
}
