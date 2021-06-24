using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Class TradingScreenManager handles keyboard input for the trade screen
 * 
 * TODO:
 * * Make button flash
 * * Update GlobalItemList
 */
public class TradeManager : MonoBehaviour
{
    private string npcName;
    private int cursorLocation = 0;
    public Button button;
    private Sprite unselected;
    private Sprite selected;
    private Inventory parentInventory;
    private Inventory inventoryPlayer;
    private Inventory inventoryNPC;
    private Inventory inventoryPlayerBin;
    private Inventory inventoryNPCBin;
    private ReferenceManager referenceManager;
    private PlayerKeyboardManager keyboardManager;
    private Text tooltipText;

    // Start is called before the first frame update
    private void OnEnable()
    {
        inventoryPlayer = GameObject.Find("Inventory (Player)").GetComponent<Inventory>();
        inventoryPlayerBin = GameObject.Find("Inventory (Player To Trade)").GetComponent<Inventory>();
        inventoryNPC = GameObject.Find("Inventory (NPC)").GetComponent<Inventory>();
        inventoryNPCBin = GameObject.Find("Inventory (NPC To Trade)").GetComponent<Inventory>();
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        parentInventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        keyboardManager = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        tooltipText = GameObject.Find("Trade Item Tooltip").GetComponentInChildren<Text>();
        selected = Resources.Load<Sprite>("SelectedSlot 1");
        unselected = Resources.Load<Sprite>("UnselectedSlot 1");

    }

    public void BeginTrading()
    {
        
        npcName = GlobalControls.CurrentNPC;

        foreach (Item item in GlobalItemList.ItemList.Values)
        {
            if (item.scene.Equals("Inventory") && item.containerName.Equals(npcName))
            {
                GameObject prefab = (GameObject) Resources.Load(item.name, typeof(GameObject));
                GameObject itemInInventory = Instantiate(prefab, item.location, Quaternion.identity);
                inventoryNPC.PickUpAtSlot((int) item.location.x, itemInInventory);
            }
            else if (item.scene.Equals("Inventory") && item.containerName.Equals("Player"))
            {
                GameObject prefab = (GameObject) Resources.Load(item.name, typeof(GameObject));
                GameObject itemInInventory = Instantiate(prefab, item.location, Quaternion.identity);
                inventoryPlayer.PickUpAtSlot((int) item.location.x, itemInInventory);
            }
        }
        
        inventoryPlayer.SelectSlotNumber(0);
        inventoryPlayerBin.selectedSlotSprite = inventoryPlayerBin.unselectedSlotSprite;
        inventoryPlayerBin.SelectSlotNumber(1);
        inventoryNPC.selectedSlotSprite = inventoryNPCBin.unselectedSlotSprite;
        inventoryNPC.SelectSlotNumber(1);
        inventoryNPCBin.selectedSlotSprite = inventoryNPCBin.unselectedSlotSprite;
        inventoryNPCBin.SelectSlotNumber(1);
    }

    /**
     * returns false if not enough inventory
     */
    public bool CompleteTrade()
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
            button.interactable = false;
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
        button.Select();
        button.interactable = false;
        return true;
    }

    public void SelectSlot(int location, int slotNumber)
    {
        this.cursorLocation = location;
        if (cursorLocation == 0)
        {
            inventoryPlayer.SelectSlotNumber(slotNumber);
        }
        else if (cursorLocation == 1)
        {
            inventoryPlayerBin.SelectSlotNumber(slotNumber);
        }
        else if (cursorLocation == 2)
        {
            inventoryNPCBin.SelectSlotNumber(slotNumber);
        }
        else if (cursorLocation == 3)
        {
            inventoryNPC.SelectSlotNumber(slotNumber);
        }

    }    
    
    public int ChangeSelectedInventory(int location)
    {
        cursorLocation = location;
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
        
        return cursorLocation;
    }

    public void LeaveTrading()
    {
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
                GlobalItemList.UpdateItemList(inventoryPlayer.items[i].name, "Inventory", 
                    new Vector3(i, 0, 0), "Player");
        }
        for (int i = 0; i < inventoryNPC.slotContents.Length; i++)
        {
            if (inventoryNPC.slotContents[i].activeSelf)
            {
                inventoryNPC.items[i].name = inventoryNPC.items[i].name.Replace("(Clone)","").Trim();
                //If new item for NPC and it's one of their needs increase satisfaction
                if (!GlobalItemList.ItemList[inventoryNPC.items[i].name].containerName.Equals(npcName) && 
                    GlobalControls.NPCList[npcName].needs.Contains(inventoryNPC.items[i].name))
                {
                    GlobalControls.NPCList[npcName].satisfaction++;
                    Debug.Log(npcName + " Satisfaction increased to " + GlobalControls.NPCList[npcName].satisfaction);
                }
                
                GlobalItemList.UpdateItemList(inventoryNPC.items[i].name, "Inventory",
                    new Vector3(i, 0, 0), npcName);
            }
        }
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

                    parentInventory.items[i].GetComponent<Collectible>().inventory = parentInventory;

                }
            }
        }
        referenceManager.inventoryCanvas.SetActive(false);
        keyboardManager.SetConversing();
    }
    
    public void EncapsulateSpace(int location)
    {
        cursorLocation = location;
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

    public bool CheckValidTrade()
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
        foreach (string need in GlobalControls.NPCList[npcName].needs)
        {
            if (npcOffers.Contains(need)) return false;
            if (playerOffers.Contains(need)) playerOfferedNeed.Add(true);
        }

        //give extra items if player offered items they need
        if (playerOfferedNeed.Count > 0 && npcOffers.Count == playerOffers.Count + playerOfferedNeed.Count) return true;
        
        //1:1 for anything else
        if (npcOffers.Count == playerOffers.Count && playerOfferedNeed.Count == 0 ) return true;
        
        //If none of the above, not valid trade.
        return false;
    }
}
