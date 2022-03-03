using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public InventoryController inventoryController;
    public bool inStorageContainer;
    public bool inLatrine;

    public Vector3 location;
    public string itemName;
    public string prefab;
    public string scene;

    public Collectible(Vector3 location, string itemName, string prefab, string scene)
    {
        this.location = location;
        this.itemName = itemName;
        this.prefab = prefab;
        this.scene = scene;
    }

    private void Awake()
    {
        if (!SceneManager.GetActiveScene().name.Equals("QuakeHouse"))
        {
            foreach (Inventory obj in GameObject.Find("Managers").GetComponent<ReferenceManager>().inventoryCanvas.GetComponentsInChildren<Inventory>())
            {
                Debug.Log("obj: " + obj);
                if (obj.gameObject.name.Equals("Inventory"))
                {
                    inventory = obj;
                }
            }
            inventoryController = GameObject.Find("Inventory Controller").GetComponent<InventoryController>();
            // inventory = GameObject.Find("Managers").GetComponent<ReferenceManager>().inventoryCanvas.GetComponent<Inventory>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // When the player runs into it, add it to inventory
        if (other.CompareTag("Player") && (!inStorageContainer || !inLatrine))
        {
            inventoryController.PickUp(this.gameObject);
        }
    }
}
