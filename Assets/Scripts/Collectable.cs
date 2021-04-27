using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Sprite itemSprite;
    public bool inStorageContainer;

    /*
     * The Collectable script should be attached to any game object you want to be an inventory item.
     * Both the player and the item must have a collider, and the item should also have a Rigidbody.
     * The collider on the item must have the "Is Trigger" box checked.
     * A very small (0.1 for all sizes) box collider on the item worked best in testing.
     * Make sure to uncheck the "Use Gravity" box in the Rigidbody options or else the item will fall!
     * Finally, make sure items are aligned with the grid properly when you place the game object
     */

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter is called");
        if (other.CompareTag("Player") && !inStorageContainer)
        {
            Debug.Log("Ran into item");
            GameObject.Find("Selector").GetComponent<Selector>().AddItemToInventory(this.gameObject);
        }
    }

    public void PutInStorageContainer()
    {
        inStorageContainer = true;
    }

    public void TakeOutOfStorageContainer()
    {
        inStorageContainer = false;
    }
}
