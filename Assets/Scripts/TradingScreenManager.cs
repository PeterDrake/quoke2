using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

/**
 * Class TradingScreenManager handles keyboard input for the trade screen
 */
public class TradingScreenManager : MonoBehaviour
{
    public int cursorLocation = 0;
    public string[] textArray;
    public string[] npcArray;
    public Button button;
    public GameObject npcText;
    private String npcName;
    //private XmlDocument convoFile = new XmlDocument();
    //public Dictionary<string, convoNode> forest = new Dictionary<string, convoNode>();
    //public convoNode currentNode;
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
        
        /*
        convoFile.Load("Assets/Resources/2TestTree.txt"); //Paste the path of the xml file you want to look at here
        
       
        foreach (XmlNode node in convoFile.LastChild) //looks through all the npc nodes instead of looking at just the <convoForest> tag
        {
            forest.Add(node.Name, new convoNode(node));
        }

        currentNode = forest[GlobalControls.ConvoDict[GlobalControls.CurrentNPC]]; // This is where the we let the NPC talk to the code. The npc we run into will pass back something like "theirName0" to get to the appropriate starting node
        
        for (int c = 0; c < currentNode.playerArray.Count; c++)
        {
            buttons[c].gameObject.SetActive(true);
            buttons[c].GetComponentInChildren<Text>().text = currentNode.playerArray[c]; //This displays the initial nodes player text    
            if (buttons[c].GetComponentInChildren<Text>().text.Equals(""))
            {
                buttons[c].gameObject.SetActive(false);
            }
        }

        npcText.GetComponentInChildren<Text>().text = currentNode.npcText; //This displays the initial nodes npc text
        
        npcArray = new[] {"You said 'button 1'", "You said 'button 2'", "You said 'button 3'"};
        textArray = new[] {"Hey here's text for button 1", "Hey here's text for button 2", "Hey here's text for button 3"};*/
       
    }


    void Update()
    {
        
        // Change the cursor's location with < and >
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

        /*Dont know how this works so it's a work in progress
        if (Input.GetKeyDown("Enter"))
        {
            button.Select();
        }*/

        //change inventories based on cursor location
        
        
        //select slot
        for (var i = 0; i < validInputs.Length; i++)
        {
            if (cursorLocation == 0 && inventoryPlayer && Input.GetKey(validInputs[i]))
            {
                inventoryPlayer.SelectSlotNumber(i);
            }
            else if (cursorLocation == 1 && inventoryPlayerBin && Input.GetKey(validInputs[i]))
            {
                inventoryPlayerBin.SelectSlotNumber(i);
            }
        }
        
        
        // Transfer items (space)
        if (cursorLocation == 0 && inventoryPlayer && Input.GetKeyDown(KeyCode.Space))
        {
            TransferItem(inventoryPlayer, inventoryPlayerBin);
            UpdateOfferItems();
        }
        else if (cursorLocation == 1 && inventoryPlayerBin && Input.GetKeyDown(KeyCode.Space))
        {
            TransferItem(inventoryPlayerBin, inventoryPlayer);
            UpdateOfferItems();
        }

        
        if (Input.GetKeyDown(KeyCode.Escape))
        {  
            //:D :3
        }
        
        /*
        if (Input.GetKeyDown("space"))
        {
            currentNode = forest[currentNode.nextNode[cursorLocation]]; //This will change the node you're looking at

            if (currentNode.nodeName.Contains("checkpoint"))
            {
                Debug.Log("Checkpoint");
                GlobalControls.SetCheckpoint(currentNode.nodeName);
            }
            else
            {
                Debug.Log("n o . . . ");
            }
            
            if (currentNode.nodeName.Contains("trade"))
            {
                Debug.Log("Trading Time!");
            }
        
            for (int c = 0; c < currentNode.playerArray.Count; c++)
            {
                buttons[c].gameObject.SetActive(true);
                buttons[c].GetComponentInChildren<Text>().text = currentNode.playerArray[c]; //This will change the player text based on the node were looking at
                if (buttons[c].GetComponentInChildren<Text>().text.Equals(""))
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }

            npcText.GetComponentInChildren<Text>().text = currentNode.npcText; //This will change the npc text based on the node
            cursorLocation = 0;

        }*/
        

        
    }

    private void ChangeSelectedInventory()
    {
        if (cursorLocation < 0)
        {
            cursorLocation = 1;
        }
        else if (cursorLocation > 1)
        {
            cursorLocation = 0;
        }
        if (cursorLocation == 0) //inventory is selected
        {
            //change selected slot sprite
            inventoryPlayer.selectedSlotSprite = inventoryPlayerBin.selectedSlotSprite;
            inventoryPlayerBin.selectedSlotSprite = inventoryPlayerBin.unselectedSlotSprite;
            inventoryPlayerBin.SelectSlotNumber(0);
            inventoryPlayer.SelectSlotNumber(0);
        }
        else if (cursorLocation == 1) //bin is selected
        {
            //change selected slot sprite
            inventoryPlayerBin.selectedSlotSprite = inventoryPlayer.selectedSlotSprite;
            inventoryPlayer.selectedSlotSprite = inventoryPlayer.unselectedSlotSprite;
            inventoryPlayer.SelectSlotNumber(0);
            inventoryPlayerBin.SelectSlotNumber(0);
        }
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

    private void UpdateOfferItems()
    {
        List<string> playerOffered = new List<string>();
        
        foreach(GameObject item in inventoryPlayerBin.items)
        {
            if (item) playerOffered.Add(item.name);
        }

        //if player offered nothing, skip method
        if (playerOffered.Count == 0) return;
        
        //otherwise check if any possible trades
        //populate NPC bin and NPC inventory accordingly
        
        
    }
}
