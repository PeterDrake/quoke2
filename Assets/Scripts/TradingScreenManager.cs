using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

/**
 * Class TradingScreenManager handles keyboard input for the trade screen
 * 
 * TODO:
 * * Check for Valid Trade
 * * * * Trading rules as follows: NPC never trades away item they need;
 * * * *                           Always 1:1 for unneeded items;
 * * * *                           1:2 if 1 is item NPC needs (player will never need if NPC needs)
 * * Implement completed Trade
 * * Update GlobalItemList
 */
public class TradingScreenManager : MonoBehaviour
{
    public int cursorLocation = 0;
    public Button button;
    public Sprite unselected;
    public Sprite selected;
    public string npcName;
    private Inventory inventoryPlayer;
    private Inventory inventoryNPC;
    private Inventory inventoryPlayerBin;
    private Inventory inventoryNPCBin;
    private readonly KeyCode[] validInputs = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0};
    
    // Start is called before the first frame update
    private void Start()
    {
        inventoryPlayer = GameObject.Find("Inventory").GetComponent<Inventory>();
        inventoryPlayerBin = GameObject.Find("Inventory (Player To Trade)").GetComponent<Inventory>();
        inventoryNPC = GameObject.Find("Inventory (NPC)").GetComponent<Inventory>();
        inventoryNPCBin = GameObject.Find("Inventory (NPC To Trade)").GetComponent<Inventory>();

        inventoryPlayerBin.selectedSlotSprite = inventoryPlayerBin.unselectedSlotSprite;
        inventoryPlayerBin.SelectSlotNumber(1);
        inventoryNPC.selectedSlotSprite = inventoryNPCBin.unselectedSlotSprite;
        inventoryNPC.SelectSlotNumber(1);
        inventoryNPCBin.selectedSlotSprite = inventoryNPCBin.unselectedSlotSprite;
        inventoryNPCBin.SelectSlotNumber(1);
    }


    void Update()
    {
        
        // Change the cursor's location (< and >)
        if (Input.GetKeyDown(","))
        {
            cursorLocation--;
            ChangeSelectedInventory();
        }
        if (Input.GetKeyDown("."))
        {
            cursorLocation++;
            ChangeSelectedInventory();
        }

        //select slot (1-9)
        for (var i = 0; i < validInputs.Length; i++)
        {
            if (cursorLocation == 0 && Input.GetKey(validInputs[i]))
            {
                inventoryPlayer.SelectSlotNumber(i);
            }
            else if (cursorLocation == 1 && Input.GetKey(validInputs[i]))
            {
                inventoryPlayerBin.SelectSlotNumber(i);
            }
            else if (cursorLocation == 2 && Input.GetKey(validInputs[i]))
            {
                inventoryNPCBin.SelectSlotNumber(i);
            }
            else if (cursorLocation == 3 && Input.GetKey(validInputs[i]))
            {
                inventoryNPC.SelectSlotNumber(i);
            }
        }
        
        
        // Transfer items (space)
        if (cursorLocation == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            TransferItem(inventoryPlayer, inventoryPlayerBin);
        }
        else if (cursorLocation == 1 && Input.GetKeyDown(KeyCode.Space))
        {
            TransferItem(inventoryPlayerBin, inventoryPlayer);
        }
        else if (cursorLocation == 2 && Input.GetKeyDown(KeyCode.Space))
        {
            TransferItem(inventoryNPCBin, inventoryNPC);
        }
        else if (cursorLocation == 3 && Input.GetKeyDown(KeyCode.Space))
        {
            TransferItem(inventoryNPC, inventoryNPCBin);
        }

        // TODO make button move items
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(CheckValidTrade()) button.Select();
        }
        
        // TODO Make activate NPC text screen and update global InventoryList
        if (Input.GetKeyDown(KeyCode.Escape))
        {  
            
        }
        
    }

    private void ChangeSelectedInventory()
    {
        if (cursorLocation < 0)
        {
            cursorLocation = 3;
        }
        else if (cursorLocation > 3)
        {
            cursorLocation = 0;
        }

        if (cursorLocation == 0)
        {
            //change selected slot sprite
            inventoryPlayer.selectedSlotSprite = selected;
            inventoryPlayerBin.selectedSlotSprite = unselected;
            inventoryNPCBin.selectedSlotSprite = unselected;
            inventoryNPC.selectedSlotSprite = unselected;
        }
        else if (cursorLocation == 1)
        {
            //change selected slot sprite
            inventoryPlayer.selectedSlotSprite = unselected;
            inventoryPlayerBin.selectedSlotSprite = selected;
            inventoryNPCBin.selectedSlotSprite = unselected;
            inventoryNPC.selectedSlotSprite = unselected;
        }
        else if (cursorLocation == 2)
        {
            //change selected slot sprite
            inventoryPlayer.selectedSlotSprite = unselected;
            inventoryPlayerBin.selectedSlotSprite = unselected;
            inventoryNPCBin.selectedSlotSprite = selected;
            inventoryNPC.selectedSlotSprite = unselected;
        }
        else if (cursorLocation == 3)
        {
            //change selected slot sprite
            inventoryPlayer.selectedSlotSprite = unselected;
            inventoryPlayerBin.selectedSlotSprite = unselected;
            inventoryNPCBin.selectedSlotSprite = unselected;
            inventoryNPC.selectedSlotSprite = selected;
        }
        
        //reset highlight locations (must be done to clear selected slot of past inventory
        inventoryPlayerBin.SelectSlotNumber(0);
        inventoryPlayer.SelectSlotNumber(0);
        inventoryNPC.SelectSlotNumber(0);
        inventoryNPCBin.SelectSlotNumber(0);
    }

    private void TransferItem(Inventory inventory, Inventory destination)
    {
        var i = inventory.selectedSlotNumber;
        
        //copied private method FindEmptySlot
        var firstSlot = 0;
        for (var j = 0; j < destination.slotFrames.Length; j++)
        {
            if (destination.slotContents[j].activeSelf) continue;
            firstSlot = j;
            break;
        }
        
        //Customized inventory.PickUp() method
        if (inventory.slotContents[i].activeSelf)
        {
            //inventory.items[i].SetActive(true);
            
            // Add item to destination
            destination.slotContents[firstSlot].SetActive(true);
            destination.slotContents[firstSlot].GetComponent<Image>().sprite = inventory.items[i].GetComponent<Collectible>().sprite;
            destination.items[firstSlot] = inventory.items[i];
            
            // Remove item from inventory
            inventory.items[i] = null;
            inventory.slotContents[i].SetActive(false);

        }
    }

    private bool CheckValidTrade()
    {
        //To be created
        return false;
    }
}
