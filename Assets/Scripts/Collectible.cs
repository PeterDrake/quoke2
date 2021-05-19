﻿using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// The Collectable script should be attached to any game object you want to be an inventory item.
/// Both the player and the item must have a Collider; the item must also have a Rigidbody.
/// The collider on the item must have the "Is Trigger" box checked.
/// A very small (0.1 for all axes) BoxCollider on the item worked best in testing.
/// Be sure to uncheck the "Use Gravity" box in the RigidBody options or else the item will fall!
/// Finally, make sure items are aligned with the grid properly when you place the game object
/// </summary>
public class Collectible : MonoBehaviour
{
    public Sprite sprite;
    public KeyboardInventoryManager keyboardInventoryManager;
    public bool inStorageContainer;
    
    void OnTriggerEnter(Collider other)
    {
        // When the player runs into it, add it to inventory
        if (other.CompareTag("Player") && !inStorageContainer)
        {
            keyboardInventoryManager.AddItemToInventory(this.gameObject);
        }
    }
}
