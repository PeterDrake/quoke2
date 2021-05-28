using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// The Collectible script should be attached to any game object you want to be an inventory item.
/// Both the player and the item must have a Collider; the item must also have a Rigidbody.
/// The collider on the item must have the "Is Trigger" box checked.
/// A very small (0.1 for all axes) BoxCollider on the item worked best in testing.
/// Be sure to uncheck the "Use Gravity" box in the RigidBody options or else the item will fall!
/// Finally, make sure items are aligned with the grid properly when you place the game object
/// </summary>
public class Collectible : MonoBehaviour
{
    public Sprite sprite;
    public Inventory inventory;
    public bool inStorageContainer;
    public string scene;
    public Vector3 location;
    public GameObject prefab;
    public GameObject currentObject;
    
    void OnTriggerEnter(Collider other)
    {
        // When the player runs into it, add it to inventory
        if (other.CompareTag("Player") && !inStorageContainer)
        {
            inventory.PickUp(this.gameObject);
        }
    }
}