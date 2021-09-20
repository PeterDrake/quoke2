using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManagerLogic
{
    string npcName;
    Inventory playerInventory;
    Inventory npcInventory;
    Inventory playerInvBin;
    Inventory npcInvBin;
    Inventory IOU;
    public int endIouTot;

    public TradeManagerLogic(Inventory npcINV, Inventory playerINV, Inventory playerBin, Inventory npcBin, Inventory invIOU)
    {
        playerInventory = playerINV;
        playerInvBin = playerBin;
        npcInventory = npcINV;
        npcInvBin = npcBin;
        IOU = invIOU;

        
    }

    public void PlayerSetNullSlots(List<int> playerUsedSlots) //handle functionality of the begin process
    {  
        //Set unused slots to null if not already.
        for (int i = 0; i < playerInventory.slotContents.Length; i++)
        {
            if (!playerUsedSlots.Contains(i) && playerInventory.slotContents[i].activeSelf)
            {
                playerInventory.items[i] = null;
                playerInventory.slotContents[i].SetActive(false);
            }
        }
    }
    public void NPCSetNullSlots(List<int> npcUsedSlots)
    {
        
        for (int i = 0; i < npcInventory.slotContents.Length; i++)
        {
            if (!npcUsedSlots.Contains(i) && npcInventory.slotContents[i].activeSelf)
            {
                npcInventory.items[i] = null;
                npcInventory.slotContents[i].SetActive(false);
            }
        }
    }

    public void CompleteTradeLogic(int[] numOfContents, string nameOfNPC)
    {
        List<string> playerOffers = new List<string>(); //list of names of items player offered
        npcName = nameOfNPC;
        //fill lists of names
        foreach (GameObject item in playerInvBin.items)
        {
            if (item)
            {
                playerOffers.Add(item.name.Replace("(Clone)", "").Trim());
                if (item.name.Equals("Water Bottle Clean(Clone)")) GlobalControls.globalControlsProperties.Remove("playerHasCleanWater");
                if (item.name.Equals("First Aid Kit(Clone)")) GlobalControls.globalControlsProperties.Remove("playerHasFirstAidKit");
                if (item.name.Equals("Epi Pen(Clone)")) GlobalControls.globalControlsProperties.Remove("playerHasEpiPen");

            }
        }

        foreach (GameObject item in npcInvBin.items)
        {
            if (item && item.name.Equals("Water Bottle Clean(Clone)")) GlobalControls.globalControlsProperties.Add("playerHasCleanWater");
            if (item && item.name.Equals("First Aid Kit(Clone)")) GlobalControls.globalControlsProperties.Add("playerHasFirstAidKit");
            if (item && item.name.Equals("Epi Pen(Clone)")) GlobalControls.globalControlsProperties.Add("playerHasEpiPen");

        }

        int playerTradePoints = 0;

        //will not trade away item they need
        foreach (string need in GlobalControls.npcList[nameOfNPC].needs)
        {
            if (playerOffers.Contains(need))
            {
                playerTradePoints++; //add an extra trade point if offered a need
                GlobalControls.CurrentPoints += GlobalControls.Points["tradeneeds"];
            }
        }

        playerTradePoints += numOfContents[0];

        endIouTot = playerTradePoints - numOfContents[1] + numOfContents[2];
    }
    
    public void LeaveTrade()
    {
        //Update globalItemList

        for (int i = 0; i < npcInventory.slotContents.Length; i++)
        {
            if (npcInventory.slotContents[i].activeSelf)
            {
                npcInventory.items[i].name = npcInventory.items[i].name.Replace("(Clone)", "").Trim();
                //If new item for NPC and it's one of their needs increase satisfaction
                if (!GlobalItemList.ItemList[npcInventory.items[i].name].containerName.Equals(npcName) &&
                    GlobalControls.npcList[npcName].needs.Contains(npcInventory.items[i].name))
                {
                    GlobalControls.npcList[npcName].satisfaction++;
                    Debug.Log(npcName + " Satisfaction increased to " + GlobalControls.npcList[npcName].satisfaction);
                    if (GlobalControls.npcList[npcName].needs.Count == GlobalControls.npcList[npcName].satisfaction)
                        GlobalControls.npcList[npcName].description = GlobalControls.npcList[npcName].name + " is happy and needs nothing more";
                    else
                    {
                        string description = GlobalControls.npcList[npcName].description;
                        description = description.Replace(npcInventory.items[i].name, "").Trim();
                        description = description.Replace("and", "").Trim();
                        GlobalControls.npcList[npcName].description = description;
                    }
                }

                GlobalItemList.UpdateItemList(npcInventory.items[i].name, "Inventory",
                    new Vector3(i, 0, 0), npcName);
            }
        }
    }
}
