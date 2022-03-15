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
        UpdateUI();
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

    public bool UIIsActive()
    {
        return inventoryUI.enabled;
    }

    public void EnableUI()
    {
        return;
    }

    public void SelectSlotNumber(int slot)
    {
        return;
    }

    public bool IsFrame1Enabled()
    {
        return false;
    }

    public void PickUpOrDrop()
    {
        return;
    }

    /// <summary>
    /// Moves the selected slot cursor in the specified direction. Returns -1 if the cursor is somewhere in the
    /// inventory. Returns the slot number of the Neighbors grid to select if the cursor is outside the inventory.
    /// </summary>
    public int MoveCursor(CursorDirection direction)
    {
        return -1;
    }
}
