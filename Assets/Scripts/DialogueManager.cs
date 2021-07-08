using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    public int cursorLocation;
    public Button[] buttons;
    public GameObject npcText;
    private String npcName;
    private XmlDocument convoFile;
    public Dictionary<string, ConvoNode> forest;
    public ConvoNode currentNode;
    private ReferenceManager referenceManager;
    private PlayerKeyboardManager keyboardManager;

    private void OnEnable()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        forest = new Dictionary<string, ConvoNode>();
        convoFile = new XmlDocument();
        cursorLocation = 0;
        keyboardManager = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
    }

    public void BeginConversation()
    {
        
        //Paste the path of the xml file you want to look at here
        TextAsset text = Resources.Load<TextAsset>("2TestTree");
        convoFile.LoadXml(text.text);

        //looks through all the npc nodes instead of looking at just the <convoForest> tag
        foreach (XmlNode node in convoFile.LastChild) 
        {
            if (!forest.ContainsKey(node.Name))
            {
                forest.Add(node.Name, new ConvoNode(node));
            }
        }

        // This is where the we let the NPC talk to the code. The npc we run into will pass back something like
        // "theirName0" to get to the appropriate starting node
        currentNode = forest[GlobalControls.NPCList[GlobalControls.CurrentNPC].node]; 
        
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
            
            //Turns off button if Demitrius drinks or Demitrius action is complete
            if (currentNode.nodeName.Contains("dem0"))
            {
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Action") && GlobalControls.DemActionDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }
                
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Drink") && GlobalControls.DemDrinkDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
            //Turns off button if Fred drinks or Fred action is complete
            if (currentNode.nodeName.Contains("fred0"))
            {
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Action") && GlobalControls.DemActionDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }
                
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Drink") && GlobalControls.DemDrinkDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
        }
        //This displays the initial nodes npc text
        npcText.GetComponentInChildren<Text>().text = currentNode.npcText; 
        buttons[cursorLocation].Select();


    }
    
    public int ChangeCursorLocations(int location)
    {
        cursorLocation = location;
        if (cursorLocation < 0)
        {
            cursorLocation = buttons.Length - 1;
        }
        else if (cursorLocation > buttons.Length - 1)
        {
            cursorLocation = 0;
        }
        buttons[cursorLocation].Select();
        return cursorLocation;
    }

    public void EncapsulateSpace()
    {
        //This will change the node you're looking at
        currentNode = forest[currentNode.nextNode[cursorLocation]]; 
        
        //switch statement cases for actions
        int x = 0;
        if (GlobalControls.CurrentNPC.Equals("dem0"))
            x = 1;
        if (GlobalControls.CurrentNPC.Equals("safi0"))
            x = 2;
        if (GlobalControls.CurrentNPC.Equals("rainer0"))
            x = 3;
        if (GlobalControls.CurrentNPC.Equals("fred0"))
            x = 4;
        
        //switch statement cases for drinks
        int y = 0;
        if (GlobalControls.CurrentNPC.Equals("dem0"))
            y = 1;
        if (GlobalControls.CurrentNPC.Equals("carlos0"))
            y = 2;
        if (GlobalControls.CurrentNPC.Equals("bob0"))
            y = 3;
        if (GlobalControls.CurrentNPC.Equals("rainer0"))
            y = 4;
        if (GlobalControls.CurrentNPC.Equals("fred0"))
            y = 5;
        
        //If we do an NPC Action
        if (currentNode.nodeName.Contains("action"))
        {
            switch (x)
            {
                case 0:
                    break;
                case 1:
                    GlobalControls.DemActionDone = true;
                    break;
                case 2:
                    //safi has 3 actions. will take care of it differently here
                    break;
                case 3:
                    GlobalControls.RainerActionDone = true;
                    break;
                case 4:
                    GlobalControls.FredActionDone = true;
                    break;
            }
            
        }

        //If we give water to an NPC
        if (currentNode.nodeName.Contains("givewater"))
        {
            switch (y)
            {
                case 0:
                    break;
                case 1:
                    GlobalControls.DemDrinkDone = true;
                    break;
                case 2:
                    GlobalControls.CarlosDrinkDone = true;
                    break;
                case 3:
                    GlobalControls.BobDrinkDone = true;
                    break;
                case 4:
                    GlobalControls.RainerDrinkDone = true;
                    break;
                case 5:
                    GlobalControls.FredDrinkDone = true;
                    break;
            }

        }


        if (currentNode.nodeName.Contains("checkpoint"))
        {
            GlobalControls.SetCheckpoint(currentNode.nodeName);
        }

        if (currentNode.nodeName.Contains("trade"))
        {
            buttons[cursorLocation].Select();
            keyboardManager.SetTrading();
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

            if (currentNode.nodeName.Contains("drink"))
            {
                if (GlobalControls.PlayerHasCleanWater && buttons[c].GetComponentInChildren<Text>().text.Contains
                    ("I have water right here!"))
                {
                    buttons[c].gameObject.SetActive(true);
                }
                else if (!GlobalControls.PlayerHasCleanWater && buttons[c].GetComponentInChildren<Text>().text.Contains
                    ("I have water right here!"))
                {
                    buttons[c].gameObject.SetActive(false);
                }
            }
            
            //Turns off button if Demetrius drinks or Demetrius action is complete
            if (currentNode.nodeName.Contains("dem0"))
            {
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Action") && GlobalControls.DemActionDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }
                
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Drink") && GlobalControls.DemDrinkDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
            //Turns off button if Fred drinks or Fred action is complete
            if (currentNode.nodeName.Contains("fred0"))
            {
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Action") && GlobalControls.FredActionDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }
                
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Drink") && GlobalControls.FredDrinkDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
            
        }
            
        //This will change the npc text based on the node
        npcText.GetComponentInChildren<Text>().text = currentNode.npcText;

    }

   
    
}
