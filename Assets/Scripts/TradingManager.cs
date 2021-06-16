using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * Class TradingScreenManager handles keyboard input for the trade screen
 * 
 * TODO:
 * * Make button flash
 * * Update GlobalItemList
 */
public class TradingManager : MonoBehaviour
{
    public int cursorLocation = 0;
    public Button button;
    public Sprite unselected;
    public Sprite selected;
    public string npcName;
    private Inventory parentInventory;
    private Inventory inventoryPlayer;
    private Inventory inventoryNPC;
    private Inventory inventoryPlayerBin;
    private Inventory inventoryNPCBin;
    private readonly KeyCode[] validInputs = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0};
    private GameObject npcCanvas;
    private ReferenceManager referenceManager;

    // Start is called before the first frame update
    private void OnEnable()
    {
        inventoryPlayer = GameObject.Find("Inventory (Player)").GetComponent<Inventory>();
        inventoryPlayerBin = GameObject.Find("Inventory (Player To Trade)").GetComponent<Inventory>();
        inventoryNPC = GameObject.Find("Inventory (NPC)").GetComponent<Inventory>();
        inventoryNPCBin = GameObject.Find("Inventory (NPC To Trade)").GetComponent<Inventory>();
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        parentInventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        //
        npcCanvas = referenceManager.dialogueCanvas;

    }

    public void BeginTrading()
    {
        button.interactable = false;
        npcName = GlobalControls.CurrentNPC;
        referenceManager.inventoryCanvas.SetActive(true);
        if (parentInventory)
        {
            //overwrite inventoryPlayer with parentInventory
            for (int i = 0; i < parentInventory.slotContents.Length; i++)
            {
                if (parentInventory.slotContents[i].activeSelf)
                {
                    inventoryPlayer.items[i] = null;
                    inventoryPlayer.slotContents[i].SetActive(false);
                    TransferItem(parentInventory, inventoryPlayer, i);
                }
            }
        }
        
        
        //TODO will be deleted / changed
        referenceManager.inventoryCanvas.SetActive(false);
        
        
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (cursorLocation == 0)
            {
                TransferItem(inventoryPlayer, inventoryPlayerBin, inventoryPlayer.selectedSlotNumber);
            }
            else if (cursorLocation == 1)
            {
                TransferItem(inventoryPlayerBin, inventoryPlayer, inventoryPlayerBin.selectedSlotNumber);
            }
            else if (cursorLocation == 2)
            {
                TransferItem(inventoryNPCBin, inventoryNPC, inventoryNPCBin.selectedSlotNumber);
            }
            else if (cursorLocation == 3)
            {
                TransferItem(inventoryNPC, inventoryNPCBin, inventoryNPC.selectedSlotNumber);
            }

            if (CheckValidTrade()) button.interactable = true;
            else button.interactable = false;

        }

        // TODO make button move items
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (CheckValidTrade())
            {
                Debug.Log("Valid trade!");
                CompleteTrade();
            }
            else Debug.Log("Invalid Trade!");
        }
        
        // TODO Make activate NPC text screen
        if (Input.GetKeyDown(KeyCode.Escape))
        {  
            Debug.Log("Leaving the trading screen");
            for (int i = 0; i < inventoryPlayerBin.slotContents.Length; i++)
            {
                if (inventoryPlayerBin.slotContents[i].activeSelf) TransferItem(inventoryPlayerBin, inventoryPlayer, i);
            }
            for (int i = 0; i < inventoryNPCBin.slotContents.Length; i++)
            {
                if (inventoryNPCBin.slotContents[i].activeSelf) TransferItem(inventoryNPCBin, inventoryNPC, i);
            }
            
            //Update globalItemList
            for (int i = 0; i < inventoryPlayer.slotContents.Length; i++)
            {
                if (inventoryPlayer.slotContents[i].activeSelf) 
                    GlobalItemList.UpdateItemList(inventoryPlayer.items[i].name, "Inventory", new Vector3(i, 0, 0), "");
            }
            for (int i = 0; i < inventoryNPC.slotContents.Length; i++)
            {
                if (inventoryNPC.slotContents[i].activeSelf)
                    GlobalItemList.UpdateItemList(inventoryNPC.items[i].name, SceneManager.GetActiveScene().name,
                        new Vector3(i, 0, 0), npcName);
            }
            
            
            
            
            //Update parent Inventory object
            referenceManager.inventoryCanvas.SetActive(true);
            if (referenceManager.inventoryCanvas)
            {
                //overwrite parent inventory with inventory here
                for (int i = 0; i < inventoryPlayer.slotContents.Length; i++)
                {
                    if (inventoryPlayer.slotContents[i].activeSelf)
                    {
                        parentInventory.items[i] = null;
                        parentInventory.slotContents[i].SetActive(false);
                        TransferItem(inventoryPlayer, parentInventory, i);
                    }
                }
            }
            referenceManager.inventoryCanvas.SetActive(false);
            npcCanvas.SetActive(true);
            referenceManager.dialogueCanvas.GetComponent<NPCScreenInteractor>().BeginConversation();
            referenceManager.tradeCanvas.SetActive(false);
        }
        
    }
    
    /**
     * returns false if not enough inventory
     */
    private bool CompleteTrade()
    {
        int[] numContents = new int[] {0,5,5,0};
        for (int i = 0; i < inventoryPlayer.slotFrames.Length; i++)
        {
            if (!inventoryPlayer.slotContents[i].activeSelf)
            {
                numContents[0]++;
            }
            if (!inventoryPlayerBin.slotContents[i].activeSelf)
            {
                numContents[1]--; //number items
            }
            if (!inventoryNPCBin.slotContents[i].activeSelf)
            {
                numContents[2]--; //number items
            }
            if (!inventoryNPC.slotContents[i].activeSelf)
            {
                numContents[3]++;
            }
        }

        if (numContents[0] - numContents[2] < 0 || numContents[3] - numContents[1] < 0)
        {
            Debug.Log("Not Enough Inventory to complete trade!");
            return false;
        }

        for (int i = 0; i < inventoryPlayerBin.slotContents.Length; i++)
        {
            if (inventoryPlayerBin.slotContents[i].activeSelf) TransferItem(inventoryPlayerBin, inventoryNPC, i);
        }
        for (int i = 0; i < inventoryNPCBin.slotContents.Length; i++)
        {
            if (inventoryNPCBin.slotContents[i].activeSelf) TransferItem(inventoryNPCBin, inventoryPlayer, i);
        }
        return true;
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

    private void TransferItem(Inventory inventory, Inventory destination, int slotNumber)
    {
        var i = slotNumber;
        
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
        List<string> npcOffers = new List<string>(); //list of names of items NPC offered
        List<string> playerOffers = new List<string>(); //list of names of items player offered
        List<bool> playerOfferedNeed = new List<bool>(); //Counts number of needs player offered
        
        //fill lists of names
        foreach (GameObject item in inventoryNPCBin.items)
        {
            if(item) npcOffers.Add(item.name.Replace("(Clone)","").Trim());
        }
        foreach (GameObject item in inventoryPlayerBin.items)
        {
            if(item) playerOffers.Add(item.name.Replace("(Clone)","").Trim());
        }

        
        //will not trade nothing
        if (playerOffers.Count == 0 || npcOffers.Count == 0) return false;
        
        //will not trade away item they need
        foreach (string need in GlobalItemList.NPCList[npcName].needs)
        {
            if (npcOffers.Contains(need)) return false;
            if (playerOffers.Contains(need)) playerOfferedNeed.Add(true);
        }

        //give extra items if player offered items they need
        if (playerOfferedNeed.Count > 0 && npcOffers.Count == playerOffers.Count + playerOfferedNeed.Count) return true;
        
        //1:1 for anything else
        if (npcOffers.Count == playerOffers.Count) return true;
        
        //If none of the above, not valid trade.
        return false;
    }
}
