using UnityEngine;

public class StorageContainer : MonoBehaviour
{
    public GameObject contents;

    /// Removes and returns the currently stored item (or null if there is no such item)
    public GameObject RemoveItem()
    {
        GameObject temp = contents;
        contents = null;
        return temp;
    }
    
}
