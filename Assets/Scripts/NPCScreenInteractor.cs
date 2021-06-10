using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Rendering;
using Object = System.Object;

public class NPCScreenInteractor : MonoBehaviour
{

    //public Button button1;
    //public Button button2;
    //public Button button3;
    public int cursorLocation = 0;
    public Button[] buttons;
    public GameObject npcText;
    private String npcName;
    private XmlDocument convoFile = new XmlDocument();
    public Dictionary<string, convoNode> forest = new Dictionary<string, convoNode>();
    public convoNode currentNode;
    
    // Start is called before the first frame update
    private void Start()
    {
        //Paste the path of the xml file you want to look at here
        convoFile.Load("Assets/Resources/2TestTree.xml"); 
        
        //looks through all the npc nodes instead of looking at just the <convoForest> tag
        foreach (XmlNode node in convoFile.LastChild) 
        {
            forest.Add(node.Name, new convoNode(node));
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
        
        //Exits the NPC screen
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
                //enter trading screen
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
