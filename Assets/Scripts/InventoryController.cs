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
    public void UpdateUIWithPickup(int slot, GameObject item)
    {
        inventoryUI.AddToSlot(slot, item);
    }

    public void ClearUI()
    {
        inventoryUI.Clear();
    }

    public void UpdateUITooltip()
    {
        inventoryUI.UpdateTooltip();
    }

    /// <summary>
    /// General function that should be called whenever any change has been made to the Inventory.
    /// </summary>
    public void UpdateUI()
    {
        
    }
   
}
