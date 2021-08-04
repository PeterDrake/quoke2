using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    public int cursorLocation;
    public Button[] buttons;
    private String npcName;
    private XmlDocument convoFile;
    public Dictionary<string, ConvoNode> forest;
    public ConvoNode currentNode;
    private ReferenceManager referenceManager;
    private PlayerKeyboardManager keyboardManager;
    private DialogueUI dialogueUI;

    private void Start()
    {
        keyboardManager = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        dialogueUI = referenceManager.dialogueCanvas.GetComponentInChildren<DialogueUI>(true);
    }

    private void OnEnable()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        forest = new Dictionary<string, ConvoNode>();
        convoFile = new XmlDocument();
        cursorLocation = 0;
    }

    public void BeginConversation()
    {
        dialogueUI.LoadNPC(GlobalControls.CurrentNPC);
        
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
            
        //If the player leaves a trading session...
        if (keyboardManager.leftTrading)
        {
            keyboardManager.leftTrading = false;

            if (GlobalItemList.ItemList["First Aid Kit"].containerName.Equals("angie0") && 
                !GlobalControls.AngieHasFirstAidKit && GlobalControls.CurrentNPC.Contains("angie"))
                if(!GlobalControls.AngieSeriousDialogue)
                {                        
                    GlobalControls.SetCheckpoint("basic_angie_4.0");
                    currentNode = forest["leave_angie_0.7"];
                    GlobalControls.AngieHasFirstAidKit = true;
                }
                else
                {                        
                    GlobalControls.SetCheckpoint("basic_angie_5.0");
                    currentNode = forest["leave_angie_1.4"];
                    GlobalControls.AngieHasFirstAidKit = true;
                }
            else if (GlobalItemList.ItemList["Epi Pen"].containerName.Equals("angie0") && 
                !GlobalControls.AngieHasEpiPen && GlobalControls.CurrentNPC.Contains("angie"))
                if(!GlobalControls.AngieSeriousDialogue)
                {                        
                    GlobalControls.SetCheckpoint("basic_angie_8.0");
                    currentNode = forest["leave_angie_0"];
                    GlobalControls.AngieHasEpiPen = true;
                }
                else
                {                        
                    GlobalControls.SetCheckpoint("basic_angie_9.0");
                    currentNode = forest["leave_angie_1"];
                    GlobalControls.AngieHasEpiPen = true;
                }
            else if (!GlobalControls.AngieSeriousDialogue && GlobalControls.CurrentNPC.Contains("angie"))
                currentNode = forest["leave_angie_0"];
            else if (GlobalControls.AngieSeriousDialogue && GlobalControls.CurrentNPC.Contains("angie"))
                currentNode = forest["leave_angie_1"];
            else
                currentNode = forest["leave_error"];

            Debug.Log("Current Node: " + currentNode.nodeName);

           
        }

        //Go through all the buttons and put that node's text into the buttons.
        for (int i = 0; i < currentNode.playerArray.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].GetComponentInChildren<Text>().text = currentNode.playerArray[i];

            //Turns a button off if there is no text in the button
            if (buttons[i].GetComponentInChildren<Text>().text.Equals(""))
            {
                buttons[i].gameObject.SetActive(false);
            }
            
            if (currentNode.nextNode[i].Contains("action"))
            {
                if (GlobalControls.NPCList[GlobalControls.CurrentNPC].actionsComplete[Int32.
                    Parse(currentNode.nextNode[i].Substring(6, 1))])
                {
                    Debug.Log("Action Complete");
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }
        
        //This displays the initial nodes npc text
        dialogueUI.AddDialogue(currentNode.npcText, GlobalControls.NPCList[GlobalControls.CurrentNPC].name);
        
        if (cursorLocation > buttons.Length - 1)
        {
            cursorLocation = 0;
        }
        if (buttons[cursorLocation].gameObject.activeSelf)
        {
            cursorLocation = ChangeCursorLocations(cursorLocation);
        }
        else
        {
            while (!buttons[cursorLocation].gameObject.activeSelf)
            {
                cursorLocation++;
                if (cursorLocation > buttons.Length - 1)
                {
                    cursorLocation = 0;
                }
            }
            cursorLocation = ChangeCursorLocations(cursorLocation);
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

    public int EncapsulateSpace()
    {
        if (!buttons[cursorLocation].gameObject.activeSelf)
        {
            return cursorLocation;
        }
        
        if (currentNode.nodeName.Contains("trade") || currentNode.nodeName.Contains("need"))
        {
            buttons[cursorLocation].Select();
            keyboardManager.SetTrading();
            return cursorLocation;
        }
        
        if (currentNode.nodeName.Contains("leave"))
        {
            buttons[cursorLocation].Select();
            keyboardManager.SetExploring();
            return cursorLocation;
        }

        //This will change the node you're looking at
        dialogueUI.AddDialogue(buttons[cursorLocation].GetComponentInChildren<Text>().text, "Duc");
        GlobalControls.NPCList[GlobalControls.CurrentNPC].dialogueList.Add(new DialogueNode(buttons[cursorLocation].GetComponentInChildren<Text>().text, "Duc"));
        currentNode = forest[currentNode.nextNode[cursorLocation]];
        Debug.Log("Current Node " + currentNode.nodeName);
        
        for (int i = 0; i < currentNode.nextNode.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);

            //This will change the player text based on the node we're looking at
            buttons[i].GetComponentInChildren<Text>(true).text = currentNode.playerArray[i];
            if (buttons[i].GetComponentInChildren<Text>(true).text.Equals(""))
            {
                buttons[i].gameObject.SetActive(false);
            }

            if (currentNode.nodeName.Contains("success"))
            {
                if (!GlobalControls.NPCList[GlobalControls.CurrentNPC].
                    actionRequirements[Int32.Parse(currentNode.nodeName.
                    Substring(7, 1))].Equals("Water Bottle Clean"))
                {
                    GlobalControls.NPCList[GlobalControls.CurrentNPC].actionsComplete[Int32.
                        Parse(currentNode.nodeName.Substring(7, 1))] = true;
                    GlobalControls.CurrentPoints += GlobalControls.Points["favors"];
                    GlobalControls.NPCList[GlobalControls.CurrentNPC].satisfaction++;
                    referenceManager.pointsText.GetComponentInChildren<Text>(true).text =
                        GlobalControls.CurrentPoints.ToString();
                }
                else
                {
                    GlobalControls.NPCList[GlobalControls.CurrentNPC].actionsComplete[Int32.
                        Parse(currentNode.nodeName.Substring(7, 1))] = true;
                    GlobalControls.CurrentPoints += GlobalControls.Points["drink"];
                    GlobalControls.NPCList[GlobalControls.CurrentNPC].satisfaction++;
                    referenceManager.pointsText.GetComponentInChildren<Text>(true).text =
                        GlobalControls.CurrentPoints.ToString();
                }
                     
            }
            
            string node = currentNode.nextNode[i];
            if (node.Contains("need"))
            {
                Debug.Log( "The next node's name" + currentNode.nextNode[i]);
                //This checks the itemList for the item that the npc needs at the index specified in the key name
                //need0_angie_12.2
                //Here the index is 0
                if (!GlobalItemList.ItemList[GlobalControls.NPCList[GlobalControls.CurrentNPC].
                    needs[Int32.Parse(node.Substring(4, 1))]].containerName.Equals("Player"))
                {
                    Debug.Log("turning off one of the need's button");
                    buttons[i].gameObject.SetActive(false);
                }
            }

            if (node.Contains("action"))
            {
                if (GlobalControls.NPCList[GlobalControls.CurrentNPC].actionsComplete[Int32.Parse(node.Substring(6, 1))])
                {
                    Debug.Log("Action Complete");
                    buttons[i].gameObject.SetActive(false);
                }
            }
            if (node.Contains("success"))
            {
                if (!GlobalControls.NPCList[GlobalControls.CurrentNPC].actionRequirements[Int32.Parse(node.Substring(7, 1))].Equals("") &&
                    !GlobalItemList.ItemList[GlobalControls.NPCList[GlobalControls.CurrentNPC].actionRequirements[Int32.Parse(node.Substring(7, 1))]].containerName.Equals("Player"))
                {
                    Debug.Log("BATTAN AAF");
                    buttons[i].gameObject.SetActive(false);
                }
            }
            
        }
        
        if (currentNode.nodeName.Contains("checkpoint"))
        {
            if (currentNode.nodeName.Equals("checkpoint_safi_1"))
                GlobalControls.SetCheckpoint("checkpoint_safi_1");
            if (currentNode.nodeName.Equals("checkpoint_angie_1.1"))
                GlobalControls.SetCheckpoint("basic_angie_3.0");
            if (currentNode.nodeName.Equals("checkpoint_angie_0.10"))
            {
                GlobalControls.AngieSeriousDialogue = true;
                GlobalControls.SetCheckpoint("basic_angie_1.0");
            }
            if (currentNode.nodeName.Equals("checkpoint_angie_0.4"))
                GlobalControls.SetCheckpoint("basic_angie_2.0");
            if (currentNode.nodeName.Equals("checkpoint_angie_5.1"))
                GlobalControls.SetCheckpoint("basic_angie_7.0");
            if (currentNode.nodeName.Equals("checkpoint_angie_6.4"))
                GlobalControls.SetCheckpoint("basic_angie_6.0");
            if (currentNode.nodeName.Equals("checkpoint_angie_10.2"))
                GlobalControls.SetCheckpoint("basic_angie_10.0");
        }
        
        
        
        dialogueUI.AddDialogue(currentNode.npcText, GlobalControls.NPCList[GlobalControls.CurrentNPC].name);
        GlobalControls.NPCList[GlobalControls.CurrentNPC].dialogueList.Add(new DialogueNode(currentNode.npcText, GlobalControls.NPCList[GlobalControls.CurrentNPC].name));

        if (cursorLocation > buttons.Length - 1)
        {
            cursorLocation = 0;
        }
        if (buttons[cursorLocation].gameObject.activeSelf)
        {
            cursorLocation = ChangeCursorLocations(cursorLocation);
        }
        else
        {
            while (!buttons[cursorLocation].gameObject.activeSelf)
            {
                cursorLocation++;
                if (cursorLocation > buttons.Length - 1)
                {
                    cursorLocation = 0;
                }
            }
            cursorLocation = ChangeCursorLocations(cursorLocation);
        }

        return cursorLocation;

    }


}
