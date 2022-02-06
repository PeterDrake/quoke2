using System;
using System.Collections.Generic;
using System.Linq;
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
        dialogueUI.LoadNPC(GlobalControls.currentNpc);

        // Paste the path of the xml file you want to look at here
        TextAsset text = Resources.Load<TextAsset>(GlobalControls.currentNpc);
        convoFile.LoadXml(text.text);

        // looks through all the npc nodes instead of looking at just the <convoForest> tag
        foreach (XmlNode node in convoFile.LastChild)
        {
            if (!forest.ContainsKey(node.Name))
            {
                forest.Add(node.Name, new ConvoNode(node));
            }
        }

        // This is where the we let the NPC talk to the code. When we run into an NPC, tbey will start at the
        // appropriate starting node.
        Debug.Log(GlobalControls.npcList[GlobalControls.currentNpc].node);
        currentNode = forest[GlobalControls.npcList[GlobalControls.currentNpc].node];

        for (int c = 0; c < currentNode.playerArray.Count; c++)
        {
            buttons[c].gameObject.SetActive(true);
            //This displays the initial node's player text
            buttons[c].GetComponentInChildren<Text>().text = currentNode.playerArray[c];

            // Turns a button off if there is no text in the button
            if (buttons[c].GetComponentInChildren<Text>().text.Equals(""))
            {
                buttons[c].gameObject.SetActive(false);
            }
        }

        if (keyboardManager.leftTrading)
        {
            keyboardManager.leftTrading = false;

            // Angie was used for testing the dialogue system, and does *not* follow latest conventions for dialogue.
            // (that's why it's so long)
            if (GlobalControls.currentNpc.Contains("Angie"))
            {
                // If Angie is given the first aid kit and the epi pen at the same time, update
                // globalControlsProperties to say Angie has the first aid kit and the epi pen. Move to the appropriate
                // dialogue node
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

                // If Angie is given the first aid kit, update globalControlsProperties to say Angie has the first
                // aid kit Move to the appropriate dialogue node
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
                // If Angie is given the epi pen (we assume that she already has the first aid kit),
                // update globalControlsProperties to say Angie has the epi pen. Move to the appropriate dialogue node
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
            // Safi needs no items (only actions), so she requires only the following code
            else if (GlobalControls.currentNpc.Equals("Safi"))
            {
                currentNode = forest["leave_safi_0"];
            }
            // Everyone other than Safi needs more code
            else
            {
                // Current NPC is *not* Safi
                int propertiesSet = 0;
                string checkpoint = "basic_" + GlobalControls.currentNpc.ToLower() + "_";
                string leave = "leave_" + GlobalControls.currentNpc.ToLower() + "_0";
                foreach (string need in GlobalControls.npcList[GlobalControls.currentNpc].needs)
                {
                    string property = GlobalControls.currentNpc.ToLower() + "Has" + RemoveWhitespace(need);
                    Debug.Log(property);
                    // If we just traded this needed item to the NPC and therefore the GlobalControls property for it
                    // has not yet been set
                    if (GlobalItemList.ItemList[need].containerName.Equals(GlobalControls.currentNpc) &&
                        !GlobalControls.globalControlsProperties.Contains(property))
                    {
                        propertiesSet++;
                        GlobalControls.SetCheckpoint(checkpoint + "3.0");
                        Debug.Log(checkpoint + "3.0");
                        GlobalControls.globalControlsProperties.Add(property);
                    }
                    else if (GlobalControls.globalControlsProperties.Contains(property)) propertiesSet++;
                }

                // If both properties are set now, set checkpoint to corresponding checkpoint
                if (propertiesSet == 2)
                {
                    GlobalControls.SetCheckpoint(checkpoint + "4.0");
                    Debug.Log(checkpoint + "4.0");
                }

                // Handles specific cases where dialogue should continue after trading with an NPC
                // In the case that the player just traded Annette some needed items in the 0 series of nodes
                if (currentNode.nodeName.Equals("basic_annette_1.0")
                    && (GlobalControls.globalControlsProperties.Contains("annetteHasLeash")
                        || GlobalControls.globalControlsProperties.Contains("annetteHasDogCrate")))
                {
                    currentNode = forest["basic_annette_0.6"];
                }
                else
                {
                    currentNode = forest[leave];
                }
            }

            npcInteractedCanvas.GetComponent<NPCInteracted>().UpdateNPCInteracted(GlobalControls.currentNpc);

            Debug.Log("Current Node A: " + currentNode.nodeName);
        }

        // Safi has specific actions do be done instead of trading, so handle that here
        if (GlobalControls.currentNpc.Equals("Safi"))
        {
            if (GlobalControls.globalControlsProperties.Contains("safiGasDone") &&
                !GlobalControls.globalControlsProperties.Contains("safiHeaterDone"))
            {
                currentNode = forest["basic_safi_3.0"];
            }

            if (GlobalControls.globalControlsProperties.Contains("safiHeaterDone"))
            {
                currentNode = forest["basic_safi_5.0"];
            }            
        }

        // Go through all the buttons and put that node's text into the buttons.
        for (int i = 0; i < currentNode.playerArray.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].GetComponentInChildren<Text>().text = currentNode.playerArray[i];

            // Turns a button off if there is no text in the button
            if (buttons[i].GetComponentInChildren<Text>().text.Equals(""))
            {
                buttons[i].gameObject.SetActive(false);
            }

            if (currentNode.nextNode[i].Contains("action"))
            {
                if (GlobalControls.npcList[GlobalControls.currentNpc]
                    .actionsComplete[Int32.Parse(currentNode.nextNode[i].Substring(6, 1))])
                {
                    Debug.Log("Action Complete");
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }

        //This displays the initial node's npc text
        dialogueUI.AddDialogue(currentNode.npcText, GlobalControls.npcList[GlobalControls.currentNpc].name);

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
            npcInteractedCanvas.GetComponent<NPCInteracted>().UpdateNPCInteracted(GlobalControls.currentNpc);
            return cursorLocation;
        }

        // This will change the node you're looking at
        dialogueUI.AddDialogue(buttons[cursorLocation].GetComponentInChildren<Text>().text, "Duc");
        GlobalControls.npcList[GlobalControls.currentNpc].dialogueList
            .Add(new DialogueNode(buttons[cursorLocation].GetComponentInChildren<Text>().text, "Duc"));
        string nextNode = currentNode.nextNode[cursorLocation];
        // dynamic options change the dialogue tree on later conversations
        if (nextNode.Contains("dynamic_option"))
        {
            switch (GlobalControls.currentNpc)
            {
                case "Rainer":
                    if (currentNode.nodeName.Equals("basic_rainer_3.0"))
                    {
                        if (GlobalControls.globalControlsProperties.Contains("rainerHasTent"))
                        {
                            currentNode = forest["basic_rainer_3.2"];
                        }
                        else currentNode = forest["basic_rainer_3.1"];
                    }
                    else if (currentNode.nodeName.Equals("basic_rainer_4.0"))
                    {
                        if (!GlobalControls.globalControlsProperties.Contains("rainerActionDone"))
                        {
                            currentNode = forest["action0_rainer_2.2"];
                        }
                        else currentNode = forest["leave_rainer_4.1"];
                    }

                    break;
                case "Annette":
                    if (currentNode.nodeName.Equals("basic_annette_1.1"))
                    {
                        if (GlobalControls.globalControlsProperties.Contains("annetteActionDone"))
                        {
                            currentNode = forest["basic_annette_1.3"];
                        }
                        else currentNode = forest["basic_annette_1.4"];
                    }
                    else if (currentNode.nodeName.Equals("basic_annette_3.0"))
                    {
                        if (GlobalControls.globalControlsProperties.Contains("annetteHasLeash"))
                        {
                            currentNode = forest["basic_annette_3.2"];
                        }
                        else currentNode = forest["basic_annette_3.1"];
                    }
                    else if (currentNode.nodeName.Equals("basic_annette_4.0"))
                    {
                        if (!GlobalControls.globalControlsProperties.Contains("annetteActionDone"))
                        {
                            currentNode = forest["basic_annette_1.4"];
                        }
                        else currentNode = forest["basic_annette_4.1"];
                    }
                    else if (currentNode.nodeName.Equals("success0_annette_0.4"))
                    {
                        currentNode = forest[GlobalControls.npcList[GlobalControls.currentNpc].node];
                    }

                    break;
                case "Carlos":
                    if (currentNode.nodeName.Equals("basic_carlos_3.0"))
                    {
                        if (GlobalControls.globalControlsProperties.Contains("carlosHasBatteries"))
                        {
                            currentNode = forest["basic_carlos_3.1"];
                        }
                        else currentNode = forest["basic_carlos_3.2"];
                    }
                    else if (currentNode.nodeName.Equals("basic_carlos_4.1"))
                    {
                        if (!GlobalControls.globalControlsProperties.Contains("carlosActionDone"))
                        {
                            currentNode = forest["action0_carlos_4.2"];
                        }
                        else currentNode = forest["basic_carlos_4.3"];
                    }

                    break;
                case "Dem":
                    if (currentNode.nodeName.Equals("basic_dem_3.0"))
                    {
                        if (!GlobalControls.globalControlsProperties.Contains("demHasCanOpener"))
                        {
                            currentNode = forest["basic_dem_3.1"];
                        }
                        else currentNode = forest["basic_dem_3.2"];
                    }
                    else if (currentNode.nodeName.Equals("basic_dem_4.0"))
                    {
                        if (GlobalControls.globalControlsProperties.Contains("demActionDone"))
                        {
                            currentNode = forest["leave_dem_4.1"];
                        }
                        else currentNode = forest["action0_dem_1.3"];
                    }

                    break;
            }
        }
        else
        {
            currentNode = forest[nextNode];
        }

        Debug.Log("Current Node B: " + currentNode.nodeName);

        for (int i = 0; i < currentNode.nextNode.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);

            //This will change the player text based on the node we're looking at
            buttons[i].GetComponentInChildren<Text>(true).text = currentNode.playerArray[i];
            if (buttons[i].GetComponentInChildren<Text>(true).text.Equals(""))
            {
                buttons[i].gameObject.SetActive(false);
            }

            if (currentNode.nodeName.Equals("basic_safi_1.1.1"))
            {
                GlobalControls.globalControlsProperties.Add("safiAsksForGasHelp");
                Debug.Log("Safi asked for gas help");
            }
            else if (currentNode.nodeName.Equals("basic_safi_3.1"))
            {
                GlobalControls.globalControlsProperties.Add("safiAsksForWaterHelp");
            }
            
            if (currentNode.nodeName.Contains("success"))
            {
                int actionIndex = Int32.Parse(currentNode.nodeName.Substring(7, 1));
                if (!GlobalControls.npcList[GlobalControls.currentNpc]
                    .actionsComplete[actionIndex])
                {
                    Debug.Log(GlobalControls.currentNpc + "'s action completed");
                    GlobalControls.npcList[GlobalControls.currentNpc]
                        .actionsComplete[actionIndex] = true;
                    GlobalControls.currentPoints += GlobalControls.points["favors"];
                    GlobalControls.npcList[GlobalControls.currentNpc].satisfaction++;
                    GlobalControls.globalControlsProperties.Add(GlobalControls.currentNpc.ToLower() + "ActionDone");
                    referenceManager.pointsText.GetComponentInChildren<Text>(true).text =
                        GlobalControls.currentPoints.ToString();
                }
            }

            string node = currentNode.nextNode[i];
            if (node.Contains("need"))
            {
                if (node.Contains("needsEither"))
                {
                    if (!GlobalItemList.ItemList[GlobalControls.npcList[GlobalControls.currentNpc].needs[0]]
                            .containerName.Equals("Player") &&
                        !GlobalItemList.ItemList[GlobalControls.npcList[GlobalControls.currentNpc].needs[1]]
                            .containerName.Equals("Player"))
                    {
                        Debug.Log("turning off one of the need's button");
                        buttons[i].gameObject.SetActive(false);
                    }
                }
                // This checks the itemList for the item that the npc needs at the index specified in the key name
                // need0_angie_12.2
                // Here the index is 0
                else if (!GlobalItemList.ItemList[
                        GlobalControls.npcList[GlobalControls.currentNpc].needs[
                            Int32.Parse(node.Substring(node.IndexOf("need") + 4, 1))]]
                    .containerName.Equals("Player"))
                {
                    Debug.Log("turning off one of the need's button");
                    buttons[i].gameObject.SetActive(false);
                }

                Debug.Log("The next node's name" + currentNode.nextNode[i]);
            }
            // If the player has already completed the action corresponding to this node/option, don't show it
            else if (node.Contains("action"))
            {
                if (GlobalControls.npcList[GlobalControls.currentNpc]
                    .actionsComplete[Int32.Parse(node.Substring(6, 1))])
                {
                    Debug.Log("Action Complete");
                    buttons[i].gameObject.SetActive(false);
                }
            }
            // If the player doesn't have the item needed to complete an action, don't let them
            else if (node.Contains("success"))
            {
                if (!GlobalControls.npcList[GlobalControls.currentNpc]
                        .actionRequirements[Int32.Parse(node.Substring(7, 1))].Equals("") &&
                    !GlobalItemList
                        .ItemList[
                            GlobalControls.npcList[GlobalControls.currentNpc]
                                .actionRequirements[Int32.Parse(node.Substring(7, 1))]].containerName.Equals("Player"))
                {
                    Debug.Log("BATTAN AAF");
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }

        if (currentNode.nodeName.Contains("checkpoint"))
        {
            if (currentNode.nodeName.Equals("checkpoint_angie_1.1"))
                GlobalControls.SetCheckpoint("basic_angie_3.0");
            else if (currentNode.nodeName.Equals("checkpoint_angie_0.10"))
            {
                GlobalControls.globalControlsProperties.Add("angieSeriousDialogue");
                GlobalControls.SetCheckpoint("basic_angie_1.0");
            }

            else if (currentNode.nodeName.Equals("checkpoint_angie_0.4"))
                GlobalControls.SetCheckpoint("basic_angie_2.0");
            else if (currentNode.nodeName.Equals("checkpoint_angie_5.1"))
                GlobalControls.SetCheckpoint("basic_angie_7.0");
            else if (currentNode.nodeName.Equals("checkpoint_angie_6.4"))
                GlobalControls.SetCheckpoint("basic_angie_6.0");
            else if (currentNode.nodeName.Equals("checkpoint_angie_10.2"))
                GlobalControls.SetCheckpoint("basic_angie_10.0");

            else if (currentNode.nodeName.Equals("basic_dem_0.2_checkpoint"))
                GlobalControls.SetCheckpoint("basic_dem_1.0");

            else if (currentNode.nodeName.Equals("basic_annette_0.4.1_checkpoint"))
                GlobalControls.SetCheckpoint("basic_annette_1.0");
            else if (currentNode.nodeName.Equals("basic_annette_2.3_checkpoint"))
                GlobalControls.SetCheckpoint("basic_annette_1.0");
            else if (currentNode.nodeName.Equals("basic_annette_2.3_checkpoint"))
                GlobalControls.SetCheckpoint("basic_annette_1.0");

            else if (currentNode.nodeName.Equals("basic_rainer_1.1_checkpoint"))
                GlobalControls.SetCheckpoint("basic_rainer_1.0");
            else if (currentNode.nodeName.Equals("trade_rainer_1.2_checkpoint"))
                GlobalControls.SetCheckpoint("basic_rainer_2.0");

            else if (currentNode.nodeName.Equals("trade_carlos_1.3_checkpoint"))
                GlobalControls.SetCheckpoint("basic_carlos_2.0");
            else if (currentNode.nodeName.Equals("basic_carlos_1.1_checkpoint"))
                GlobalControls.SetCheckpoint("basic_carlos_1.0");

            // gas leak, after completed should set checkpoint of basic_safi_3.0
            else if (currentNode.nodeName.Equals("leave_safi_1.2_checkpoint"))
                GlobalControls.SetCheckpoint("leave_safi_1.3");
            // water heater, after completed should set checkpoint of basic_safi_5.0
            else if (currentNode.nodeName.Equals("leave_safi_3.2_checkpoint"))
                GlobalControls.SetCheckpoint("leave_safi_3.3");
            else if (currentNode.nodeName.Equals("success0_safi_0.3_checkpoint"))
                GlobalControls.SetCheckpoint("basic_safi_1.0");
            else if (currentNode.nodeName.Equals("success0_safi_0.4_checkpoint"))
                GlobalControls.SetCheckpoint("basic_safi_1.0");
            else if (currentNode.nodeName.Equals("success0_safi_0.2_checkpoint"))
                GlobalControls.SetCheckpoint("basic_safi_1.0");
            else if (currentNode.nodeName.Equals("basic_safi_1.3_checkpoint"))
                GlobalControls.SetCheckpoint("basic_safi_2.0");
            else if (currentNode.nodeName.Equals("basic_safi_3.3_checkpoint"))
                GlobalControls.SetCheckpoint("basic_safi_4.0");
            else if (currentNode.nodeName.Equals("basic_safi_3.2_checkpoint"))
                GlobalControls.SetCheckpoint("basic_safi_5.0");
        }


        dialogueUI.AddDialogue(currentNode.npcText, GlobalControls.npcList[GlobalControls.currentNpc].name);
        GlobalControls.npcList[GlobalControls.currentNpc].dialogueList.Add(new DialogueNode(currentNode.npcText,
            GlobalControls.npcList[GlobalControls.currentNpc].name));

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

    public string RemoveWhitespace(string str)
    {
        return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
    }
}