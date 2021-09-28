using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


public class TradeManager 
{

    Inventory[] inventories;
    InventoryUI[] inventoryUIs;



    public TradeManager(Inventory[] invs, InventoryUI[] invUIs)
    {

        inventories = invs;
        inventoryUIs = invUIs;

    }

    public void SetNullSlots(List<int> slotsUsed, InventoryE who)
    {

        for (int i = 0; i < inventoryUIs[(int)who].slotContents.Length; i++)
        {
            if (!slotsUsed.Contains(i) && inventoryUIs[(int)who].slotContents[i].activeSelf)
            {
                inventories[(int)who].items[i] = null;
                inventoryUIs[(int)who].slotContents[i].SetActive(false);
            }
        }

    }
    public void increaseNumContent(List<int> currentContents, InventoryE who, int slot)
    {

        for (int i = 0; i < inventoryUIs[(int)who].slotFrames.Length; i++)
        {
            if (who == InventoryE.IOU)

            {
                if (inventoryUIs[(int)who].slotFrames[i].activeSelf)
                    currentContents[slot]++; //number items
            }

            else
            {
                if (inventoryUIs[(int)who].slotContents[i].activeSelf)
                    currentContents[slot]++; //number items

            }
        }
    }

    public void TransferItem(InventoryE sender, InventoryE reciever, int slotNumber)
    {
        var i = slotNumber;

        //copied private method FindEmptySlot
        var firstSlot = 0;
        for (var j = 0; j < inventoryUIs[(int)reciever].slotFrames.Length; j++)
        {
            if (inventoryUIs[(int)reciever].slotContents[j].activeSelf) continue;
            firstSlot = j;
            break;
        }


        //Customized inventory.PickUp() method
        if (inventoryUIs[(int)sender].slotContents[i].activeSelf)
        {
            //inventory.items[i].SetActive(true);


            // Add item to destination
            inventoryUIs[(int)reciever].slotContents[firstSlot].SetActive(true);
            
            inventoryUIs[(int)reciever].slotContents[firstSlot].GetComponent<Image>().sprite
                = inventories[(int)sender].items[i].GetComponent<Collectible>().sprite;
            inventories[(int)reciever].items[firstSlot] = inventories[(int)sender].items[i];

            // Remove item from inventory
            inventories[(int)sender].items[i] = null;
            inventoryUIs[(int)sender].slotContents[i].SetActive(false);


        }
        inventories[(int)sender].SelectSlotNumber(i);

    }

    public void UpdateGlobalItemList(string npcName)
    {
        for (int i = 0; i < inventoryUIs[(int)InventoryE.NPC].slotContents.Length; i++)
        {
            if (inventoryUIs[(int)InventoryE.NPC].slotContents[i].activeSelf) 
            {
                inventories[(int)InventoryE.NPC].items[i].name
                    = inventories[(int)InventoryE.NPC].items[i].name.Replace("(Clone)", "").Trim();
                //If new item for NPC and it's one of their needs increase satisfaction
                int indexInNPCNeeds = GlobalControls.npcList[npcName].needs
                    .IndexOf(inventories[(int) InventoryE.NPC].items[i].name);
                if (indexInNPCNeeds >= 0 
                    && !GlobalItemList.ItemList[inventories[(int)InventoryE.NPC].items[i].name]
                        .containerName.Equals(npcName)
                    && !GlobalControls.npcList[npcName].needsMet[indexInNPCNeeds])
                    //GlobalControls.npcList[npcName].needs.Contains(inventories[(int)InventoryE.NPC].items[i].name
                {
                    GlobalControls.npcList[npcName].satisfaction++;
                    Debug.Log(npcName
                              + " Satisfaction increased to " + GlobalControls.npcList[npcName].satisfaction);
                    GlobalControls.npcList[npcName].needsMet[indexInNPCNeeds] = true;
                    // adjust the NPC's description to be only what they still need
                    List<string> neededItems = new List<string>();
                    for (int j = 0; j < GlobalControls.npcList[npcName].needsMet.Count; j++)
                    {
                        if (!GlobalControls.npcList[npcName].needsMet[j])
                        {
                            neededItems.Add(GlobalControls.npcList[npcName].needs[j]);
                        }
                    }
                    string description;
                    if (neededItems.Count == 0) {
                        description = GlobalControls.npcList[npcName].name + " is happy and needs nothing more";
                    }
                    else if (neededItems.Count == 1)
                    {
                        description = GlobalControls.npcList[npcName].name + " needs a " + neededItems[0];
                    }
                    else {
                        description = GlobalControls.npcList[npcName].name + " needs a " + neededItems[0] +
                                       " and a " + neededItems[1];
                    }
                    GlobalControls.npcList[npcName].description = description;
                }
                GlobalItemList.UpdateItemList(inventories[(int)InventoryE.NPC].items[i].name, "Inventory",
                    new Vector3(i, 0, 0), npcName);
            }
        }
    }
    public bool IsValidTrade(string npcName)
    {
        List<string> npcOffers = new List<string>(); //list of names of items NPC offered
        List<string> playerOffers = new List<string>(); //list of names of items player offered
        List<bool> playerOfferedNeed = new List<bool>(); //Counts number of needs player offered

        //fill lists of names
        foreach (GameObject item in inventories[(int)InventoryE.NPCBin].items)
        {
            if (item) npcOffers.Add(item.name.Replace("(Clone)", "").Trim());
        }
        foreach (GameObject item in inventories[(int)InventoryE.PlayerBin].items)
        {
            if (item) playerOffers.Add(item.name.Replace("(Clone)", "").Trim());
        }

        //Will not trade if not enough inventory

        int[] numContents = { 0,
            inventoryUIs[(int)InventoryE.PlayerBin].slotFrames.Length,
            inventoryUIs[(int)InventoryE.NPC].slotFrames.Length, 0, 0 };

        for (int i = 0; i < inventoryUIs[(int)InventoryE.Player].slotFrames.Length; i++)
        {
            if (!inventoryUIs[(int)InventoryE.Player].slotContents[i].activeSelf)

            {
                numContents[0]++;
            }
        }

        for (int i = 0; i < inventoryUIs[(int)InventoryE.PlayerBin].slotFrames.Length; i++)
        {
            if (!inventoryUIs[(int)InventoryE.PlayerBin].slotContents[i].activeSelf)

            {
                numContents[1]--; //number items
            }
        }

        for (int i = 0; i < inventoryUIs[(int)InventoryE.NPCBin].slotFrames.Length; i++)
        {
            if (!inventoryUIs[(int)InventoryE.NPCBin].slotContents[i].activeSelf)

            {
                numContents[2]--; //number items
            }
        }

        for (int i = 0; i < inventoryUIs[(int)InventoryE.NPC].slotFrames.Length; i++)
        {
            if (!inventoryUIs[(int)InventoryE.NPC].slotContents[i].activeSelf)

            {
                numContents[3]++;
            }
        }

        for (int i = 0; i < inventoryUIs[(int)InventoryE.IOU].slotFrames.Length; i++)
        {
            if (inventoryUIs[(int)InventoryE.IOU].slotFrames[i].activeSelf)

            {
                numContents[4]++;
            }
        }

        if (numContents[0] - numContents[2] < 0 || numContents[3] - numContents[1] < 0)
        {
            Debug.Log("Not Enough Inventory to complete trade!");
            return false;
        }

        int playerTradePoints = 0;

        //will not trade away item they need
        foreach (string need in GlobalControls.npcList[npcName].needs)
        {
            if (npcOffers.Contains(need)) return false;
            if (playerOffers.Contains(need))
            {
                playerOfferedNeed.Add(true);
                playerTradePoints++; //add an extra trade point if offered a need
            }
        }
        playerTradePoints += playerOffers.Count; //add number of player offered items
        if (playerTradePoints == 0 && npcOffers.Count == 0) return false; //if player and npc offer no items return false
        if (playerTradePoints + numContents[4] - npcOffers.Count > 5) return false; //if player will end with more than 5 IOUs return false
        if (playerTradePoints + numContents[4] >= npcOffers.Count) return true; //if combined IOU and player offers are >= than npcOffer      
        return false;
    }
}
