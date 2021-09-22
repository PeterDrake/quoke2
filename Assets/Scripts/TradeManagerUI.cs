using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * Class TradingScreenManager handles keyboard input for the trade screen
 * 
 * TODO:
 * * Make button flash
 * * Update GlobalItemList
 */
public class TradeManagerUI : MonoBehaviour
{
    private string npcName;
    private int cursorLocation = 0;
    public Button button;
    private Sprite unselected;
    private Sprite selected;

    private Inventory parentInventory;
    private Inventory inventoryPlayer;
    public Inventory inventoryNPC;
    private Inventory inventoryPlayerBin;
    private Inventory inventoryNPCBin;
    private Inventory inventoryIOU;

    private InventoryUI parentInventoryUI;
    private InventoryUI inventoryPlayerUI;
    public InventoryUI inventoryNPCUI;
    private InventoryUI inventoryPlayerBinUI;
    private InventoryUI inventoryNPCBinUI;
    private InventoryUI inventoryIOUUI;

    private ReferenceManager referenceManager;
    private PlayerKeyboardManager keyboardManager;
    private TradeManagerLogic myLogic;
    private Text tooltipText;


    // Start is called before the first frame update
    private void OnEnable()
    {

        foreach (Inventory child in GetComponentsInChildren<Inventory>(true))
        {
            if (child.gameObject.name.Equals("Inventory (Player)")) inventoryPlayer = child;
            else if (child.gameObject.name.Equals("Inventory (Player To Trade)")) inventoryPlayerBin = child;
            else if (child.gameObject.name.Equals("Inventory (NPC)")) inventoryNPC = child;
            else if (child.gameObject.name.Equals("Inventory (NPC To Trade)")) inventoryNPCBin = child;
            else if (child.gameObject.name.Equals("Inventory (IOU)")) inventoryIOU = child;
        }

        foreach (InventoryUI child in GetComponentsInChildren<InventoryUI>(true))
        {
            if (child.gameObject.name.Equals("Inventory (Player)")) inventoryPlayerUI = child;
            else if (child.gameObject.name.Equals("Inventory (Player To Trade)")) inventoryPlayerBinUI = child;
            else if (child.gameObject.name.Equals("Inventory (NPC)")) inventoryNPCUI = child;
            else if (child.gameObject.name.Equals("Inventory (NPC To Trade)")) inventoryNPCBinUI = child;
            else if (child.gameObject.name.Equals("Inventory (IOU)")) inventoryIOUUI = child;
        }

        inventoryPlayer.gameObject.SetActive(true);
        inventoryPlayerBin.gameObject.SetActive(true);
        inventoryNPC.gameObject.SetActive(true);
        inventoryNPCBin.gameObject.SetActive(true);
        inventoryIOU.gameObject.SetActive(true);

        myLogic = new TradeManagerLogic(inventoryNPC, inventoryPlayer, inventoryPlayerBin, inventoryNPCBin, inventoryIOU); // Pass the references to the logic class

        inventoryNPCBin.SetAvailableSlots(4);
        inventoryNPC.SetAvailableSlots(4);

        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        parentInventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        parentInventoryUI = referenceManager.inventoryCanvas.GetComponent<InventoryUI>();
        keyboardManager = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();

        selected = Resources.Load<Sprite>("SelectedSlot 1");
        unselected = Resources.Load<Sprite>("UnselectedSlot 1");


    }

    public void BeginTrading()
    {
        npcName = GlobalControls.CurrentNPC;
        List<int> playerUsedSlots = new List<int>();
        List<int> npcUsedSlots = new List<int>();

        foreach (Item item in GlobalItemList.ItemList.Values)
        {
            if (item.scene.Equals("Inventory") && item.containerName.Equals(npcName))
            {
                GameObject prefab = (GameObject) Resources.Load(item.name, typeof(GameObject));
                GameObject itemInInventory = Instantiate(prefab, item.location, Quaternion.identity);
                inventoryNPC.PickUpAtSlot((int) item.location.x, itemInInventory);
                npcUsedSlots.Add((int)item.location.x);

            }
            else if (item.scene.Equals("Inventory") && item.containerName.Equals("Player"))
            {
                GameObject prefab = (GameObject) Resources.Load(item.name, typeof(GameObject));
                GameObject itemInInventory = Instantiate(prefab, item.location, Quaternion.identity);
                inventoryPlayer.PickUpAtSlot((int) item.location.x, itemInInventory);
                playerUsedSlots.Add((int)item.location.x);
       
            }
        }

        myLogic.SetNullSlots(playerUsedSlots, npcUsedSlots, inventoryPlayerUI, inventoryNPCUI);


        //Set empty slots to null



        button.interactable = false;
        //Load IOU inventory
        inventoryIOUUI.selectedSlotSprite = unselected;
        //inventoryIOU.SetAvailableSlots(GlobalControls.npcList[npcName].owes);
        inventoryIOUUI.SelectSlotNumber(0);

        for (int i = inventoryIOUUI.slotFrames.Length - 1; i > GlobalControls.npcList[npcName].owes - 1; i--)
        {
            inventoryIOUUI.slotFrames[i].SetActive(false);
        }

        foreach (GameObject game in inventoryIOUUI.slotContents)
        {
            game.SetActive(true);
            Sprite prefab = (Sprite) Resources.Load("IOU Sprite", typeof(Sprite));
            game.GetComponent<Image>().sprite = prefab;
        }

        ChangeSelectedInventory(0);
    }



    

    
    public void CompleteTrade()
    {
        //StartCoroutine(SelectButton());
        button.Select();
        
        int[] numContents = {0,0,0};
        
        for (int i = 0; i < inventoryPlayerBinUI.slotFrames.Length; i++)
        {
            if (inventoryPlayerBinUI.slotContents[i].activeSelf)
            {
                numContents[0]++; //number items
            }
        }
        for (int i = 0; i < inventoryNPCBinUI.slotFrames.Length; i++)
        {
            if (inventoryNPCBinUI.slotContents[i].activeSelf)
            {
                numContents[1]++; //number items
            }
        }
        for (int i = 0; i < inventoryIOUUI.slotFrames.Length; i++)
        {
            if (inventoryIOUUI.slotFrames[i].activeSelf)
            {
                numContents[2]++;
            }
        }

        myLogic.CompleteTradeLogic(numContents, npcName);
        int endIouTotal = myLogic.endIouTot;

        for (int i = 0; i < inventoryPlayerBinUI.slotContents.Length; i++)
        {
            if (inventoryPlayerBinUI.slotContents[i].activeSelf) 
                TransferItem(inventoryPlayerBin, inventoryPlayerBinUI,
    inventoryNPC, inventoryNPCUI, i);
        }
        for (int i = 0; i < inventoryNPCBinUI.slotContents.Length; i++)
        {
            if (inventoryNPCBinUI.slotContents[i].activeSelf) 
                TransferItem(inventoryNPCBin, inventoryNPCBinUI,
                    inventoryPlayer, inventoryPlayerUI, i);
        }
        for (int i = 0; i < inventoryIOUUI.slotFrames.Length; i++)
        {
            if(i < endIouTotal) inventoryIOUUI.slotFrames[i].SetActive(true);
            else inventoryIOUUI.slotFrames[i].SetActive(false);
        }
        
        button.interactable = false;
        referenceManager.pointsText.GetComponentInChildren<Text>().text = GlobalControls.CurrentPoints.ToString();

    }

    private IEnumerator SelectButton()
    {
        button.Select();
        yield return new WaitForSeconds(0.1f);
        
        button.interactable = false;
        button.interactable = true;
        yield return new WaitForSeconds(0.1f);
        button.interactable = false;
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
            inventoryPlayerUI.EnableSelectedSlot();
            inventoryPlayerBinUI.DisableSelectedSlot();
            inventoryNPCBinUI.DisableSelectedSlot();
            inventoryNPCUI.DisableSelectedSlot();
            inventoryPlayerBin.SelectSlotNumber(0);
            inventoryNPCBin.SelectSlotNumber(0);
            inventoryNPC.SelectSlotNumber(0);
            inventoryPlayer.SelectSlotNumber(0);
        }
        else if (cursorLocation == 1)
        {
            //change selected slot sprite
            inventoryPlayerUI.DisableSelectedSlot();
            inventoryPlayerBinUI.EnableSelectedSlot();
            inventoryNPCBinUI.DisableSelectedSlot();
            inventoryNPCUI.DisableSelectedSlot();
            inventoryPlayer.SelectSlotNumber(0);
            inventoryNPCBin.SelectSlotNumber(0);
            inventoryNPC.SelectSlotNumber(0);
            inventoryPlayerBin.SelectSlotNumber(0);
        }
        else if (cursorLocation == 2)
        {
            //change selected slot sprite
            inventoryPlayerUI.DisableSelectedSlot();
            inventoryPlayerBinUI.EnableSelectedSlot();
            inventoryNPCBinUI.DisableSelectedSlot();
            inventoryNPCUI.DisableSelectedSlot();
            inventoryPlayerBin.SelectSlotNumber(0);
            inventoryPlayer.SelectSlotNumber(0);
            inventoryNPC.SelectSlotNumber(0);
            inventoryNPCBin.SelectSlotNumber(0);
        }
        else if (cursorLocation == 3)
        {
            //change selected slot sprite
            inventoryPlayerUI.DisableSelectedSlot();
            inventoryPlayerBinUI.EnableSelectedSlot();
            inventoryNPCBinUI.DisableSelectedSlot();
            inventoryNPCUI.DisableSelectedSlot();
            inventoryPlayerBin.SelectSlotNumber(0);
            inventoryPlayer.SelectSlotNumber(0);
            inventoryNPCBin.SelectSlotNumber(0);
            inventoryNPC.SelectSlotNumber(0);
        }
        
        //reset highlight locations (must be done to clear selected slot of past inventory
        
        
        return cursorLocation;
    }

    public void LeaveTrading()
    {
        for (int i = 0; i < inventoryPlayerBinUI.slotContents.Length; i++)
        {
            if (inventoryPlayerBinUI.slotContents[i].activeSelf)
                TransferItem(inventoryPlayerBin, inventoryPlayerBinUI,
                    inventoryPlayer, inventoryPlayerUI, i);
        }
        for (int i = 0; i < inventoryNPCBinUI.slotContents.Length; i++)
        {
            if (inventoryNPCBinUI.slotContents[i].activeSelf)
                TransferItem(inventoryNPCBin, inventoryNPCBinUI,
                    inventoryNPC, inventoryNPCUI, i);
        }


        myLogic.LeaveTrade(inventoryNPCUI); 
        
        //update IOUs
        int counter = 0;
        foreach (GameObject game in inventoryIOUUI.slotFrames)
        {
            if (game.activeSelf) counter++;
        }

        GlobalControls.npcList[npcName].owes = counter;
        
        referenceManager.inventoryCanvas.SetActive(true);
        if (referenceManager.inventoryCanvas)
        {
            //overwrite parent inventory with inventory here
            for (int i = 0; i < inventoryPlayerUI.slotContents.Length; i++)
            {
                if (inventoryPlayerUI.slotContents[i].activeSelf)
                {
                    parentInventory.items[i] = null;
                    parentInventoryUI.slotContents[i].SetActive(false);
                    TransferItem(inventoryPlayer, inventoryPlayerUI,
                        parentInventory, parentInventoryUI, i);

                }
                else if (parentInventoryUI.slotContents[i].activeSelf)
                {
                    parentInventory.items[i] = null;
                    parentInventoryUI.slotContents[i].SetActive(false);
                }
            }
            
            for (int i = 0; i < parentInventoryUI.slotContents.Length; i++)
            {
                if(parentInventoryUI.slotContents[i].activeSelf) parentInventory.items[i].GetComponent<Collectible>().inventory = parentInventory;
            }
            
            for (int i = 0; i < parentInventoryUI.slotContents.Length; i++)
            {
                if (parentInventoryUI.slotContents[i].activeSelf) 
                    GlobalItemList.UpdateItemList(parentInventory.items[i].name, "Inventory", 
                        new Vector3(i, 0, 0), "Player");
            }
            
        }
        
        keyboardManager.SetConversing();
    }
    
    public void EncapsulateSpace(int location)
    {
        cursorLocation = location;
        if (cursorLocation == 0)
        {
            TransferItem(inventoryPlayer, inventoryPlayerUI,
                inventoryPlayerBin, inventoryPlayerBinUI, inventoryPlayer.selectedSlotNumber);
        }
        else if (cursorLocation == 1)
        {
            TransferItem(inventoryPlayerBin, inventoryPlayerBinUI,
                inventoryPlayer, inventoryPlayerUI, inventoryPlayerBin.selectedSlotNumber);
        }
        else if (cursorLocation == 2)
        {
            TransferItem(inventoryNPCBin, inventoryNPCBinUI,
                inventoryNPC, inventoryNPCUI, inventoryNPCBin.selectedSlotNumber);
        }
        else if (cursorLocation == 3)
        {
            TransferItem(inventoryNPC, inventoryNPCUI,
                inventoryNPCBin, inventoryNPCBinUI, inventoryNPC.selectedSlotNumber);
        }

        if (CheckValidTrade()) button.interactable = true;
        else button.interactable = false;
    }
    
    private void TransferItem(Inventory inventory, InventoryUI inventoryUI, Inventory destination, InventoryUI destinationUI, int slotNumber)
    {
        var i = slotNumber;
        
        //copied private method FindEmptySlot
        var firstSlot = 0;
        for (var j = 0; j < destinationUI.slotFrames.Length; j++)
        {
            if (destinationUI.slotContents[j].activeSelf) continue;
            firstSlot = j;
            break;
        }
        
        //Customized inventory.PickUp() method
        if (inventoryUI.slotContents[i].activeSelf)
        {
            //inventory.items[i].SetActive(true);
            
            // Add item to destination
            destinationUI.slotContents[firstSlot].SetActive(true);
            destinationUI.slotContents[firstSlot].GetComponent<Image>().sprite = inventory.items[i].GetComponent<Collectible>().sprite;
            destination.items[firstSlot] = inventory.items[i];
            
            // Remove item from inventory
            inventory.items[i] = null;
            inventoryUI.slotContents[i].SetActive(false);

        }
        inventory.SelectSlotNumber(i);
    }

    /// <summary>
    /// returns false if no items offered, not enough inventory, NPC offered need, incorrect number of items offered.
    /// Returns true if Exact :1 of unneeded items, or Exact 1:2 if 1 is item NPC needs.
    /// </summary>
    /// <returns></returns>
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

        //Will not trade if not enough inventory
        int[] numContents = {0,inventoryPlayerBinUI.slotFrames.Length,inventoryNPCUI.slotFrames.Length,0,0};
        
        for (int i = 0; i < inventoryPlayerUI.slotFrames.Length; i++)
        {
            if (!inventoryPlayerUI.slotContents[i].activeSelf)
            {
                numContents[0]++;
            }
        }
        for (int i = 0; i < inventoryPlayerBinUI.slotFrames.Length; i++)
        {
            if (!inventoryPlayerBinUI.slotContents[i].activeSelf)
            {
                numContents[1]--; //number items
            }
        }
        for (int i = 0; i < inventoryNPCBinUI.slotFrames.Length; i++)
        {
            if (!inventoryNPCBinUI.slotContents[i].activeSelf)
            {
                numContents[2]--; //number items
            }
        }
        for (int i = 0; i < inventoryNPCUI.slotFrames.Length; i++)
        {
            if (!inventoryNPCUI.slotContents[i].activeSelf)
            {
                numContents[3]++;
            }
        }
        for (int i = 0; i < inventoryIOUUI.slotFrames.Length; i++)
        {
            if (inventoryIOUUI.slotFrames[i].activeSelf)
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

        if (playerTradePoints + numContents[4] >= npcOffers.Count) return true; //if combined IOU and player offers are >= than npcOffers
        
        return false;
    }
}
