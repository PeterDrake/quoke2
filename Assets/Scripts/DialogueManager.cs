using System;
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
    private string filepath;
    private string fileContents;

    private void OnEnable()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        forest = new Dictionary<string, ConvoNode>();
        convoFile = new XmlDocument();
        cursorLocation = 0;
        keyboardManager = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
    }
    
    IEnumerator GetWebText()
    {
        UnityWebRequest request = UnityWebRequest.Get(filepath);
        Debug.Log("Attempting to communicate with server at " + filepath);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("ERROR ERROR Download Failed <3");
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("WOOHOO DOWNLOAD SUCCEEDED <3");
            fileContents = request.downloadHandler.text;
            Debug.Log("File Contents = " + fileContents);
        }

        
        request.Dispose();
        
        Debug.Log("Loading XML File");

        Debug.Log(convoFile);
        convoFile.LoadXml(fileContents);

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

    public void BeginConversation()
    {
        
        //Paste the path of the xml file you want to look at here
        filepath = Application.streamingAssetsPath + "/2TestTree.xml";

        if (filepath.Contains("http"))
        {
            StartCoroutine(GetWebText());
        }
        else
        {
            convoFile.LoadXml(File.ReadAllText(filepath));
            
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
