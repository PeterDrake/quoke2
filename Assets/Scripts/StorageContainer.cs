using UnityEngine;

public class StorageContainer : MonoBehaviour
{
    public GameObject contents;
    public bool isGoodPlace; // specific to sheds/to go bag in the pre-quake conditions. If true, items survive quake

    /// Removes and returns the currently stored item (or null if there is no such item)
    public GameObject RemoveItem()
    {
        GameObject temp = contents;
        contents = null;
        return temp;
    }
    
}
