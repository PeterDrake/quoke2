using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageContainer : MonoBehaviour
{
    public GameObject itemStored;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            Debug.Log("Placed in storage");
            other.gameObject.GetComponent<Collectable>();
            //GameObject.Find("Selector").GetComponent<Selector>().AddItemToInventory(this.gameObject);
        }
    }

    // Attempts to store the selected item in the container and returns a boolean of success/failure
    public bool storeItem(GameObject item)
    {
        if (itemStored == null)
        {
            itemStored = item;
            return true;
        }
        return false;
    }

    // Removes the currently stored item and returns it
    public GameObject removeItem()
    {
        if (itemStored != null)
        {
            GameObject temp = itemStored;
            itemStored = null;
            return temp;
        }
        return null;
    }

    public bool isItemStored()
    {
        return itemStored != null;
    }
}
