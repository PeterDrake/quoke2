using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private GameStateManager gameStateManager;
    private DialogueUI dialogueUI;
    private GameObject npcInteractedCanvas;

    private void Start()
    {
        keyboardManager = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        gameStateManager = referenceManager.gameStateManager.GetComponent<GameStateManager>();
        dialogueUI = referenceManager.dialogueCanvas.GetComponentInChildren<DialogueUI>(true);
    }

    private void OnEnable()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        forest = new Dictionary<string, ConvoNode>();
        convoFile = new XmlDocument();
        cursorLocation = 0;
        npcInteractedCanvas = referenceManager.npcInteractedCanvas;
    }

    public void BeginConversation()
    {
        dialogueUI.LoadNPC(GlobalControls.CurrentNPC);

        //Paste the path of the xml file you want to look at here
        TextAsset text = Resources.Load<TextAsset>(GlobalControls.CurrentNPC);
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
        currentNode = forest[GlobalControls.npcList[GlobalControls.CurrentNPC].node];

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

            if (GlobalControls.CurrentNPC.Contains("Angie"))
            {
                //If Angie is given the first aid kit and the epi pen at the same time, update globalControlsProperties to
                //say Angie has the first aid kit and the epi pen. Move to the appropriate dialogue node
                if (!GlobalControls.globalControlsProperties.Contains("angieHasEpiPen") &&
                    !GlobalControls.globalControlsProperties.Contains("angieHasFirstAidKit") &&
                    GlobalItemList.ItemList["First Aid Kit"].containerName.Equals("Angie") &&
                    GlobalItemList.ItemList["Epi Pen"].containerName.Equals("Angie"))

                    if (!GlobalControls.globalControlsProperties.Contains("angieSeriousDialogue"))
                    {
                        GlobalControls.SetCheckpoint("basic_angie_8.0");
                        currentNode = forest["leave_angie_0"];
                        GlobalControls.globalControlsProperties.Add("angieHasFirstAidKit");
                        GlobalControls.globalControlsProperties.Add("angieHasEpiPen");
                    }
                    else
                    {
                        GlobalControls.SetCheckpoint("basic_angie_9.0");
                        currentNode = forest["leave_angie_1"];
                        GlobalControls.globalControlsProperties.Add("angieHasFirstAidKit");
                        GlobalControls.globalControlsProperties.Add("angieHasEpiPen");
                    }

                //If Angie is given the first aid kit, update globalControlsProperties to say Angie has the first aid kit
                //Move to the appropriate dialogue node
                else if (GlobalItemList.ItemList["First Aid Kit"].containerName.Equals("Angie") &&
                         !GlobalControls.globalControlsProperties.Contains("angieHasFirstAidKit"))

                    if (!GlobalControls.globalControlsProperties.Contains("angieSeriousDialogue"))
                    {
                        GlobalControls.SetCheckpoint("basic_angie_4.0");
                        currentNode = forest["leave_angie_0.7"];
                        GlobalControls.globalControlsProperties.Add("angieHasFirstAidKit");
                    }
                    else
                    {
                        GlobalControls.SetCheckpoint("basic_angie_5.0");
                        currentNode = forest["leave_angie_1.4"];
                        GlobalControls.globalControlsProperties.Add("angieHasFirstAidKit");
                    }
                //If Angie is given the epi pen (we assume that she already has the first aid kit),
                //update globalControlsProperties to say Angie has the epi pen. Move to the appropriate dialogue node
                else if (GlobalItemList.ItemList["Epi Pen"].containerName.Equals("Angie") &&
                         !GlobalControls.globalControlsProperties.Contains("angieHasEpiPen"))

                    if (!GlobalControls.globalControlsProperties.Contains("angieSeriousDialogue"))
                    {
                        GlobalControls.SetCheckpoint("basic_angie_8.0");
                        currentNode = forest["leave_angie_0"];
                        GlobalControls.globalControlsProperties.Add("angieHasEpiPen");
                    }
                    else
                    {
                        GlobalControls.SetCheckpoint("basic_angie_9.0");
                        currentNode = forest["leave_angie_1"];
                        GlobalControls.globalControlsProperties.Add("angieHasEpiPen");
                    }

                else if (!GlobalControls.globalControlsProperties.Contains("angieSeriousDialogue"))
                    currentNode = forest["leave_angie_0"];
                else if (GlobalControls.globalControlsProperties.Contains("angieSeriousDialogue"))
                    currentNode = forest["leave_angie_1"];
                else
                    currentNode = forest["leave_error"];
            }
            else 
            {
                int propertiesSet = 0;
                string checkpoint = "basic_" + GlobalControls.CurrentNPC.ToLower() + "_";
                string leave = "leave_" + GlobalControls.CurrentNPC.ToLower() + "_0";
                // iterate through the npc's needs
                foreach (string need in GlobalControls.npcList[GlobalControls.CurrentNPC].needs)
                {
                    string property = GlobalControls.CurrentNPC.ToLower() + "Has" + RemoveWhitespace(need);
                    Debug.Log(property);
                    // if we just traded it to them
                    if (GlobalItemList.ItemList[need].containerName.Equals(GlobalControls.CurrentNPC) &&
                        !GlobalControls.globalControlsProperties.Contains(property))
                    {
                        propertiesSet++;
                        // set checkpoint to 3.0
                        GlobalControls.SetCheckpoint(checkpoint + "3.0");
                        // set the global controls property about it
                        GlobalControls.globalControlsProperties.Add(property);
                    }
                    else if (GlobalControls.globalControlsProperties.Contains(property)) propertiesSet++;
                }
                // then if we set both are set now, set checkpoint to 4.0
                if (propertiesSet == 2) GlobalControls.SetCheckpoint(checkpoint + "4.0");
                currentNode = forest[leave];
            }
            npcInteractedCanvas.GetComponent<NPCInteracted>().UpdateNPCInteracted(GlobalControls.CurrentNPC);

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
                if (GlobalControls.npcList[GlobalControls.CurrentNPC]
                    .actionsComplete[Int32.Parse(currentNode.nextNode[i].Substring(6, 1))])
                {
                    Debug.Log("Action Complete");
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }

        //This displays the initial nodes npc text
        dialogueUI.AddDialogue(currentNode.npcText, GlobalControls.npcList[GlobalControls.CurrentNPC].name);

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

        EventSystem.current.SetSelectedGameObject(null);
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
            gameStateManager.SetTrading();
            return cursorLocation;
        }

        if (currentNode.nodeName.Contains("leave"))
        {
            buttons[cursorLocation].Select();
            gameStateManager.SetExploring();
            return cursorLocation;
        }

        //This will change the node you're looking at
        dialogueUI.AddDialogue(buttons[cursorLocation].GetComponentInChildren<Text>().text, "Duc");
        GlobalControls.npcList[GlobalControls.CurrentNPC].dialogueList
            .Add(new DialogueNode(buttons[cursorLocation].GetComponentInChildren<Text>().text, "Duc"));
        string nextNode = currentNode.nextNode[cursorLocation];
        if (nextNode.Contains("dynamic_option"))
        {
            if (currentNode.nodeName.Equals("basic_rainer_3.0"))
            {
                if (!GlobalItemList.ItemList["Tent"].containerName.Equals("Rainer"))
                {
                    currentNode = forest["basic_rainer_3.1"];
                }
                else currentNode = forest["basic_rainer_3.2"];
            }
            
            if (currentNode.nodeName.Equals("basic_rainer_4.0"))
            {
                if (true) // Have not comforted Rainer
                {
                    currentNode = forest["basic_rainer_3.1"];
                }
                else currentNode = forest["leave_rainer_4.1"];
            }
        }
        else
        {
            currentNode = forest[nextNode];
        }
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
                if (!GlobalControls.npcList[GlobalControls.CurrentNPC]
                    .actionRequirements[Int32.Parse(currentNode.nodeName.Substring(7, 1))].Equals("Water Bottle Clean"))
                {
                    GlobalControls.npcList[GlobalControls.CurrentNPC]
                        .actionsComplete[Int32.Parse(currentNode.nodeName.Substring(7, 1))] = true;
                    GlobalControls.CurrentPoints += GlobalControls.Points["favors"];
                    GlobalControls.npcList[GlobalControls.CurrentNPC].satisfaction++;
                    referenceManager.pointsText.GetComponentInChildren<Text>(true).text =
                        GlobalControls.CurrentPoints.ToString();
                }
                else
                {
                    GlobalControls.npcList[GlobalControls.CurrentNPC]
                        .actionsComplete[Int32.Parse(currentNode.nodeName.Substring(7, 1))] = true;
                    GlobalControls.CurrentPoints += GlobalControls.Points["drink"];
                    GlobalControls.npcList[GlobalControls.CurrentNPC].satisfaction++;
                    referenceManager.pointsText.GetComponentInChildren<Text>(true).text =
                        GlobalControls.CurrentPoints.ToString();
                }
            }

            string node = currentNode.nextNode[i];
            if (node.Contains("need"))
            {
                Debug.Log("The next node's name" + currentNode.nextNode[i]);
                //This checks the itemList for the item that the npc needs at the index specified in the key name
                //need0_angie_12.2
                //Here the index is 0
                if (!GlobalItemList
                    .ItemList[
                        GlobalControls.npcList[GlobalControls.CurrentNPC].needs[Int32.Parse(node.Substring(4, 1))]]
                    .containerName.Equals("Player"))
                {
                    Debug.Log("turning off one of the need's button");
                    buttons[i].gameObject.SetActive(false);
                }
            }
            else if (node.Contains("action"))
            {
                if (GlobalControls.npcList[GlobalControls.CurrentNPC]
                    .actionsComplete[Int32.Parse(node.Substring(6, 1))])
                {
                    Debug.Log("Action Complete");
                    buttons[i].gameObject.SetActive(false);
                }
            }
            else if (node.Contains("success"))
            {
                if (!GlobalControls.npcList[GlobalControls.CurrentNPC]
                        .actionRequirements[Int32.Parse(node.Substring(7, 1))].Equals("") &&
                    !GlobalItemList
                        .ItemList[
                            GlobalControls.npcList[GlobalControls.CurrentNPC]
                                .actionRequirements[Int32.Parse(node.Substring(7, 1))]].containerName.Equals("Player"))
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
                GlobalControls.globalControlsProperties.Add("angieSeriousDialogue");
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


        dialogueUI.AddDialogue(currentNode.npcText, GlobalControls.npcList[GlobalControls.CurrentNPC].name);
        GlobalControls.npcList[GlobalControls.CurrentNPC].dialogueList.Add(new DialogueNode(currentNode.npcText,
            GlobalControls.npcList[GlobalControls.CurrentNPC].name));

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
    
    public string RemoveWhitespace(string str) {
        return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
    }
}