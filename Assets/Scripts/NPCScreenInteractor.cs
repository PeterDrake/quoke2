using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Rendering;
using Object = System.Object;

public class NPCScreenInteractor : MonoBehaviour
{
    public int cursorLocation;
    public Button[] buttons;
    public GameObject npcText;
    private String npcName;
    private XmlDocument convoFile;
    public Dictionary<string, convoNode> forest;
    public convoNode currentNode;
    private GameObject tradingScreen;
    
    // Start is called before the first frame update
    private void Start()
    {
        forest = new Dictionary<string, convoNode>();
        convoFile = new XmlDocument();
        cursorLocation = 0;
        tradingScreen = GameObject.Find("Manager").GetComponent<Manager>().tradingScreen;
    }
    
    public void BeginConversation()
    {
        //Paste the path of the xml file you want to look at here
        string filepath = Application.streamingAssetsPath + "/2TestTree.xml";
        convoFile.Load(filepath); 
        
        //looks through all the npc nodes instead of looking at just the <convoForest> tag
        foreach (XmlNode node in convoFile.LastChild) 
        {
            if (!forest.ContainsKey(node.Name))
            {
                forest.Add(node.Name, new convoNode(node));
            }
        }

        // This is where the we let the NPC talk to the code. The npc we run into will pass back something like
        // "theirName0" to get to the appropriate starting node
        currentNode = forest[GlobalControls.ConvoDict[GlobalControls.CurrentNPC]]; 
        
        for (int c = 0; c < currentNode.playerArray.Count; c++)
        {
            buttons[c].gameObject.SetActive(true);
            
            //This displays the initial nodes player text
            buttons[c].GetComponentInChildren<Text>().text = currentNode.playerArray[c];     
            
            //Turns a button off if there is no text in the button
            if (buttons[c].GetComponentInChildren<Text>().text.Equals(""))
            {
                buttons[c].gameObject.SetActive(false);
            }
        }
        //This displays the initial nodes npc text
        npcText.GetComponentInChildren<Text>().text = currentNode.npcText; 

    }

    public void Reset()
    {
        Start();
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
            cursorLocation = buttons.Length - 1;
        }
        else if (cursorLocation > buttons.Length - 1)
        {
            cursorLocation = 0;
        }

        buttons[cursorLocation].Select();
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {  
            //Exits the NPC screen
        }
        
        //Selects an option from the player options
        if (Input.GetKeyDown("space"))
        {
            //This will change the node you're looking at
            currentNode = forest[currentNode.nextNode[cursorLocation]]; 

            if (currentNode.nodeName.Contains("checkpoint"))
            {
                GlobalControls.SetCheckpoint(currentNode.nodeName);
            }

            if (currentNode.nodeName.Contains("trade"))
            {
                tradingScreen.SetActive(true);
                GameObject.Find("Trading Manager").GetComponent<TradingManager>().BeginTrading();
                GameObject.Find("NpcCanvas").SetActive(false);
            }
        
            for (int c = 0; c < currentNode.playerArray.Count; c++)
            {
                buttons[c].gameObject.SetActive(true);
                
                //This will change the player text based on the node we're looking at
                buttons[c].GetComponentInChildren<Text>().text = currentNode.playerArray[c]; 
                if (buttons[c].GetComponentInChildren<Text>().text.Equals(""))
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
            //This will change the npc text based on the node
            npcText.GetComponentInChildren<Text>().text = currentNode.npcText; 
            cursorLocation = 0;

        }
        

        
    }

   
    
}
