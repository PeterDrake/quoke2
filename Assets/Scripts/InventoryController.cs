using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private NewInventory inventory;
    private NewInventoryUI inventoryUI;

    void Start()
    {
        inventoryUI = GameObject.Find("Inventory").GetComponent<NewInventoryUI>();
        inventory = new NewInventory(this);
    }
    
    public void PickUp(GameObject item)
    {
        inventory.PickUp(item);
    }

    /// <summary>
    /// General function that should be called whenever any change has been made to the Inventory.
    /// </summary>
    public void UpdateUI()
    {
        inventoryUI.UpdateUI();
    }

    public int GetSelectedSlotNumber()
    {
        return inventory.GetSelectedSlotNumber();
    }

    public GameObject GetItemInSlot(int slotNumber)
    {
        return inventory.GetItemInSlot(slotNumber);
    }

    public int GetNumberOfSlots()
    {
        return inventory.GetNumberOfSlots();
    }
}
