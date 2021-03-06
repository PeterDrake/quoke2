﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
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

        }
            
        //This will change the npc text based on the node
        npcText.GetComponentInChildren<Text>().text = currentNode.npcText;

    }

   
    
}
