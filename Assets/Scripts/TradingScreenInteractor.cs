using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class TradingScreenInteractor : MonoBehaviour
{
    //public Button button1;
    //public Button button2;
    //public Button button3;
    public int cursorLocation = 0;
    public string[] textArray;
    public string[] npcArray;
    public Button[] buttons;
    public GameObject npcText;
    private String npcName;
    private XmlDocument convoFile = new XmlDocument();
    public Dictionary<string, convoNode> forest = new Dictionary<string, convoNode>();
    public convoNode currentNode;
    private Inventory inventoryPlayer;
    private Inventory inventoryNPC;
    private Inventory inventoryPlayerBin;
    private Inventory inventoryNPCBin;
    private readonly KeyCode[] validInputs = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0};
    
    // Start is called before the first frame update
    private void Start()
    {
        inventoryPlayer = GameObject.Find("Inventory").GetComponent<Inventory>();
        inventoryPlayerBin = GameObject.Find("Inventory").GetComponent<Inventory>();
        inventoryNPC = GameObject.Find("Inventory").GetComponent<Inventory>();
        inventoryNPCBin = GameObject.Find("Inventory").GetComponent<Inventory>();
        
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
        textArray = new[] {"Hey here's text for button 1", "Hey here's text for button 2", "Hey here's text for button 3"};
       
    }


    void Update()
    {
        
        // Change the cursor's location with < and >
        if (Input.GetKeyDown(","))
        {
            cursorLocation--;
        }
        if (Input.GetKeyDown("."))
        {
            cursorLocation++;
        }
        if (cursorLocation < 0)
        {
            cursorLocation = 1;
        }
        else if (cursorLocation > 1)
        {
            cursorLocation = 0;
        }
        for (int i = 0; i < validInputs.Length; i++)
        {
            if (cursorLocation == 0 && inventoryPlayer && Input.GetKey(validInputs[i]))
            {
                inventoryPlayer.SelectSlotNumber(i);
            }
            else if (cursorLocation == 1 && inventoryPlayer && Input.GetKey(validInputs[i]))
            {
                inventoryPlayerBin.SelectSlotNumber(i);
            }
        }
        // Pick up / drop (space)
        if (inventoryPlayer && Input.GetKeyDown(KeyCode.Space))
        {
            inventoryPlayer.PickUpOrDrop();
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
}
