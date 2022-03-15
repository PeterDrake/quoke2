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

public enum InventoryE{
    Parent = 0, Player = 1, PlayerBin = 2, NPC = 3, NPCBin = 4,  IOU = 5 
}

public class TradeManagerUI : MonoBehaviour
{

    public Inventory[] inventories;
    public InventoryUI[] inventoryUIs;
    private string npcName;
    private int cursorLocation = 0;
    public Button button;
    private Sprite unselected;
    private Sprite selected;
 
    private ReferenceManager referenceManager;
    private PlayerKeyboardManager keyboardManager;
    private GameStateManager gameStateManager;
    private TradeManager tradeLogic;
    private Text tooltipText;

    private void OnEnable()
    {
        InitializeInventories();
        tradeLogic = new TradeManager(inventories, inventoryUIs); // Passes the references to the logic class
        ConnectToManagers();
    }

    private void InitializeInventories()
    {
        inventories = new Inventory[6];
        inventoryUIs = new InventoryUI[6];

        foreach (Inventory child in GetComponentsInChildren<Inventory>(true))
        {
            if (child.gameObject.name.Equals("Inventory (Player)")) inventories[(int) InventoryE.Player] = child;
            else if (child.gameObject.name.Equals("Inventory (Player To Trade)")) inventories[(int)InventoryE.PlayerBin] = child;
            else if (child.gameObject.name.Equals("Inventory (NPC)")) inventories[(int)InventoryE.NPC] = child;
            else if (child.gameObject.name.Equals("Inventory (NPC To Trade)")) inventories[(int)InventoryE.NPCBin] = child;
            else if (child.gameObject.name.Equals("Inventory (IOU)")) inventories[(int)InventoryE.IOU] = child;
        }
        foreach (InventoryUI child in GetComponentsInChildren<InventoryUI>(true))
        {
            if (child.gameObject.name.Equals("Inventory (Player)")) inventoryUIs[(int)InventoryE.Player] = child;
            else if (child.gameObject.name.Equals("Inventory (Player To Trade)")) inventoryUIs[(int)InventoryE.PlayerBin] = child;
            else if (child.gameObject.name.Equals("Inventory (NPC)")) inventoryUIs[(int)InventoryE.NPC] = child;
            else if (child.gameObject.name.Equals("Inventory (NPC To Trade)")) inventoryUIs[(int)InventoryE.NPCBin] = child;
            else if (child.gameObject.name.Equals("Inventory (IOU)")) inventoryUIs[(int)InventoryE.IOU] = child;
        }

        for (int i = 1; i < inventories.Length; i++)
            inventories[i].gameObject.SetActive(true);

        inventories[(int)InventoryE.NPC].SetAvailableSlots(4);
        inventories[(int)InventoryE.NPCBin].SetAvailableSlots(4);
    }


    private void ConnectToManagers()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        keyboardManager = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        gameStateManager = referenceManager.gameStateManager.GetComponent<GameStateManager>();
        //inventories[(int)InventoryE.Parent] = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        //inventoryUIs[(int)InventoryE.Parent] = referenceManager.inventoryCanvas.GetComponent<InventoryUI>();
        selected = Resources.Load<Sprite>("SelectedSlot 1");
        unselected = Resources.Load<Sprite>("UnselectedSlot 1");
    }
    
    
    public void BeginTrading()
    {
        npcName = GlobalControls.currentNpc;

        List<int> playerSlotsUsed = new List<int>();
        List<int> npcSlotsUsed = new List<int>();
        foreach (Item item in GlobalItemList.ItemList.Values)
        {
            if (item.scene.Equals("Inventory") && item.containerName.Equals(npcName))
            {
                GameObject prefab = (GameObject) Resources.Load(item.name, typeof(GameObject));
                GameObject itemInInventory = Instantiate(prefab, item.location, Quaternion.identity);
                inventories[(int)InventoryE.NPC].PickUpAtSlot((int) item.location.x, itemInInventory);
                npcSlotsUsed.Add((int) item.location.x);
            }
            else if (item.scene.Equals("Inventory") && item.containerName.Equals("Player"))
            {
                GameObject prefab = (GameObject) Resources.Load(item.name, typeof(GameObject));
                GameObject itemInInventory = Instantiate(prefab, item.location, Quaternion.identity);
                inventories[(int)InventoryE.Player].PickUpAtSlot((int) item.location.x, itemInInventory);
                playerSlotsUsed.Add((int) item.location.x);
            }
        }

        //Set unused slots to null if not already.
        tradeLogic.SetNullSlots(playerSlotsUsed, InventoryE.Player);
        tradeLogic.SetNullSlots(npcSlotsUsed, InventoryE.NPC);

        button.interactable = false;
        ActivateIOU();

    }

    /// <summary>
    /// If the NPC owes you additional items (because you gave them an essential item), but you don't have enough
    /// inventory space, use an IOU to prevent item loss.
    /// </summary>
    public void ActivateIOU()
    {
        //Load IOU inventory
        inventoryUIs[(int)InventoryE.IOU].selectedSlotSprite = unselected;
        inventoryUIs[(int)InventoryE.IOU].SelectSlotNumber(0);
        for (int i = inventoryUIs[(int)InventoryE.IOU].slotFrames.Length - 1; i > GlobalControls.npcList[npcName].owes - 1; i--)
        {
            inventoryUIs[(int)InventoryE.IOU].slotFrames[i].SetActive(false);
        }
        //inventoryIOU.SetAvailableSlots(GlobalControls.npcList[npcName].owes);
        foreach (GameObject game in inventoryUIs[(int)InventoryE.IOU].slotContents)
        {
            game.SetActive(true);
            Sprite prefab = (Sprite)Resources.Load("IOU Sprite", typeof(Sprite));
            game.GetComponent<Image>().sprite = prefab;
        }
        ChangeSelectedInventory(0);
    }

    public int ChangeSelectedInventory(int location)
    {
        cursorLocation = location;
        InventoryE CurrentInventory = InventoryE.Player;
        if (cursorLocation < 0)
        {
            cursorLocation = 3;
        }
        else if (cursorLocation > 3)
        {
            cursorLocation = 0;
        }

        //reset highlight locations (must be done to clear selected slot of past inventory
        for (int i = 0; i <= 3; i++)
        {

            if (i == 0)
                CurrentInventory = InventoryE.Player;
            else if (i == 1)
                CurrentInventory = InventoryE.PlayerBin;
            else if (i == 2)
                CurrentInventory = InventoryE.NPCBin;
            else if (i == 3)
                CurrentInventory = InventoryE.NPC;
            if (cursorLocation == i)
            {
                for (int j = (int)InventoryE.Player; j <= (int)InventoryE.NPCBin; j++)
                {
                    if (j == (int)CurrentInventory)
                        inventoryUIs[j].EnableSelectedSlot();
                    else
                        inventoryUIs[j].DisableSelectedSlot();
                    inventories[j].SelectSlotNumber(0);
                }
                
            }

        }      
        return cursorLocation;
    }
    public void CompleteTrade()
    {
        button.Select();
        
        List<int> numContents = new List<int>{0,0,0};    

        tradeLogic.increaseNumContent(numContents, InventoryE.PlayerBin, 0);
        tradeLogic.increaseNumContent(numContents, InventoryE.NPCBin, 1);
        tradeLogic.increaseNumContent(numContents, InventoryE.IOU, 2);

        List<string> playerOffers = new List<string>(); // list of names of items player offered
        // fill lists of names
        foreach (GameObject item in inventories[(int) InventoryE.PlayerBin].items)
        {
            if (item)
            {
                playerOffers.Add(item.name.Replace("(Clone)","").Trim());
                if (item.name.Equals("Water Bottle Clean(Clone)")) GlobalControls.globalControlsProperties.Remove("playerHasCleanWater");
                if (item.name.Equals("First Aid Kit(Clone)")) GlobalControls.globalControlsProperties.Remove("playerHasFirstAidKit");
                if (item.name.Equals("Epi Pen(Clone)")) GlobalControls.globalControlsProperties.Remove("playerHasEpiPen");

            }
        }
        
        foreach (GameObject item in inventories[(int)InventoryE.NPCBin].items)
        {
            if (item && item.name.Equals("Water Bottle Clean(Clone)")) GlobalControls.globalControlsProperties.Add("playerHasCleanWater");
            if (item && item.name.Equals("First Aid Kit(Clone)")) GlobalControls.globalControlsProperties.Add("playerHasFirstAidKit");
            if (item && item.name.Equals("Epi Pen(Clone)")) GlobalControls.globalControlsProperties.Add("playerHasEpiPen");

        }
        int playerTradePoints = 0;  
        //will not trade away item they need
        foreach (string need in GlobalControls.npcList[npcName].needs)
        {
            if (playerOffers.Contains(need))
            {
                playerTradePoints++; //add an extra trade point if offered a need
                GlobalControls.currentPoints += GlobalControls.points["tradeneeds"];
            }
        }

        playerTradePoints += numContents[0];

        int endIouTotal = playerTradePoints - numContents[1] + numContents[2];

        for (int i = 0; i < inventoryUIs[(int) InventoryE.PlayerBin].slotContents.Length; i++)
        {
            if (inventoryUIs[(int)InventoryE.PlayerBin].slotContents[i].activeSelf)
                tradeLogic.TransferItem(InventoryE.PlayerBin, InventoryE.NPC, i);
        }
        for (int i = 0; i < inventoryUIs[(int)InventoryE.NPCBin].slotContents.Length; i++)
        {
            if (inventoryUIs[(int)InventoryE.NPCBin].slotContents[i].activeSelf)
                tradeLogic.TransferItem(InventoryE.NPCBin, InventoryE.Player, i);
        }
        for (int i = 0; i < inventoryUIs[(int)InventoryE.IOU].slotFrames.Length; i++)
        {
            if(i < endIouTotal) inventoryUIs[(int)InventoryE.IOU].slotFrames[i].SetActive(true);
            else inventoryUIs[(int)InventoryE.IOU].slotFrames[i].SetActive(false);
        }
        
        button.interactable = false;
        referenceManager.pointsText.GetComponentInChildren<Text>().text = GlobalControls.currentPoints.ToString();
        CheckForNeededItemInNPCInventory();
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
            inventories[(int)InventoryE.Player].SelectSlotNumber(slotNumber);
        else if (cursorLocation == 1)
            inventories[(int)InventoryE.PlayerBin].SelectSlotNumber(slotNumber);
        else if (cursorLocation == 2)
            inventories[(int)InventoryE.NPCBin].SelectSlotNumber(slotNumber);
        else if (cursorLocation == 3)
            inventories[(int)InventoryE.NPC].SelectSlotNumber(slotNumber);
    }    
    


    public void LeaveTrading()
    {
        for (int i = 0; i < inventoryUIs[(int)InventoryE.PlayerBin].slotContents.Length; i++)
        {
            if (inventoryUIs[(int)InventoryE.PlayerBin].slotContents[i].activeSelf)
                tradeLogic.TransferItem(InventoryE.PlayerBin, InventoryE.Player, i);
        }
        for (int i = 0; i < inventoryUIs[(int)InventoryE.NPCBin].slotContents.Length; i++)
        {
            if (inventoryUIs[(int)InventoryE.NPCBin].slotContents[i].activeSelf)
                tradeLogic.TransferItem(InventoryE.NPCBin,InventoryE.NPC, i);
        }
        tradeLogic.UpdateGlobalItemList(npcName);
       
        //update IOUs
        int counter = 0;
        foreach (GameObject go in inventoryUIs[(int)InventoryE.IOU].slotFrames)
        {
            if (go.activeSelf) counter++;
        }

        GlobalControls.npcList[npcName].owes = counter;
        referenceManager.inventoryController.EnableUI();
        
        //overwrite parent inventory with inventory here
        for (int i = 0; i < inventoryUIs[(int)InventoryE.Player].slotContents.Length; i++)
        {
            if (inventoryUIs[(int)InventoryE.Player].slotContents[i].activeSelf)
            {
                inventories[(int)InventoryE.Parent].items[i] = null;
                inventoryUIs[(int)InventoryE.Parent].slotContents[i].SetActive(false);
                tradeLogic.TransferItem(InventoryE.Player, InventoryE.Parent, i);
                
            }
            else if (inventoryUIs[(int)InventoryE.Parent].slotContents[i].activeSelf)
            {
                inventories[(int)InventoryE.Parent].items[i] = null;
                inventoryUIs[(int)InventoryE.Parent].slotContents[i].SetActive(false);
            }
        }
        
        for (int i = 0; i < inventoryUIs[(int)InventoryE.Parent].slotContents.Length; i++)
        {
            if(inventoryUIs[(int)InventoryE.Parent].slotContents[i].activeSelf) inventories[(int)InventoryE.Parent].items[i].GetComponent<Collectible>().inventory = inventories[(int)InventoryE.Parent];
        }
        
        for (int i = 0; i < inventoryUIs[(int)InventoryE.Parent].slotContents.Length; i++)
        {
            if (inventoryUIs[(int)InventoryE.Parent].slotContents[i].activeSelf) 
                GlobalItemList.UpdateItemList(inventories[(int)InventoryE.Parent].items[i].name, "Inventory", 
                    new Vector3(i, 0, 0), "Player");
        }
        gameStateManager.SetConversing();
    }

    /// <summary>
    /// changes what the space button does depending on several conditions (e.g. if the item's in a bin, put it back
    /// into the specific inventory)
    /// </summary>
    /// <param name="location"></param>
    public void EncapsulateSpace(int location)
    {
        cursorLocation = location;

        if (cursorLocation == 0)
            tradeLogic.TransferItem(InventoryE.Player, InventoryE.PlayerBin, inventories[(int) InventoryE.Player].selectedSlotNumber);
        else if (cursorLocation == 1)
            tradeLogic.TransferItem(InventoryE.PlayerBin, InventoryE.Player, inventories[(int)InventoryE.PlayerBin].selectedSlotNumber);
        else if (cursorLocation == 2)
            tradeLogic.TransferItem(InventoryE.NPCBin, InventoryE.NPC, inventories[(int)InventoryE.NPCBin].selectedSlotNumber);
        else if (cursorLocation == 3)
        {
            int selectedSlot = inventories[(int) InventoryE.NPC].selectedSlotNumber;
            if (!(inventoryUIs[(int) InventoryE.NPC].slotContents[selectedSlot].activeSelf
            && GlobalControls.npcList[npcName].needs.Contains(
                inventories[(int) InventoryE.NPC].items[selectedSlot].name.Replace("(Clone)", "").Trim())))
            {
                tradeLogic.TransferItem(InventoryE.NPC, InventoryE.NPCBin, inventories[(int)InventoryE.NPC].selectedSlotNumber);
            }
        }

        if (CheckValidTrade()) button.interactable = true;
        else button.interactable = false;
    }
    
    /// <summary>
    /// returns false if no items offered, not enough inventory, NPC offered need, incorrect number of items offered.
    /// Returns true if Exact :1 of unneeded items, or Exact 1:2 if 1 is item NPC needs.
    /// </summary>
    /// <returns></returns>
    public bool CheckValidTrade()
    {
        return tradeLogic.IsValidTrade(npcName);
    }

    private void CheckForNeededItemInNPCInventory()
    {
        for (int i = 0; i < inventories[(int)InventoryE.NPC].items.Length; i++)
        {
            GameObject item = inventories[(int)InventoryE.NPC].items[i];
            if (item
                && GlobalControls.npcList[npcName].needs.Contains(item.name.Replace("(Clone)", "").Trim()))
            {
                inventoryUIs[(int) InventoryE.NPC].EnableSlotGrayscale(i);
            } 
            else inventoryUIs[(int)InventoryE.NPC].DisableSlotGrayscale(i);
        }
    }
}
