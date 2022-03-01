using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private NewInventory inventory;
    private NewInventoryUI inventoryUI;

    void Start()
    {
        inventory = new NewInventory();
        inventoryUI = GameObject.Find("Inventory").GetComponent<NewInventoryUI>();
        
    }
    
    // player/game to model
    public void PickUp(GameObject item)
    {
        inventory.PickUp(item);
    }

    // model to UI
    public void UpdateUIWithPickup(GameObject item)
    {
        inventoryUI.AddToSlot(i, item);

    }
   
}
