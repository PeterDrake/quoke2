using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Foraging") && GlobalControls.DemActionDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }
                
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Drink") && GlobalControls.DemDrinkDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
            if (currentNode.nodeName.Contains("checkpointsafi1"))
            {
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Water") && GlobalControls.SafiWaterActionDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }
                
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Gas") && GlobalControls.SafiGasActionDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
            //Turns off button if Annette drinks or Annette action is complete
            if (currentNode.nodeName.Contains("annette0"))
            {
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Translation") && GlobalControls.AnnetteActionDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }
                
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Drink") && GlobalControls.AnnetteDrinkDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
            //Turns off button if Rainer drinks or Rainer action is complete
            if (currentNode.nodeName.Contains("rainer0"))
            {
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Action") && GlobalControls.RainerActionDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }
                
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Drink") && GlobalControls.RainerDrinkDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
            //Turns off button if Carlos drinks is complete
            if (currentNode.nodeName.Contains("carlos0"))
            {
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Drink") && GlobalControls.CarlosDrinkDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
            //Turns off button if Angie drinks is complete
            if (currentNode.nodeName.Contains("angie"))
            {
                if (GlobalControls.PlayerHasFirstAidKit && buttons[c].GetComponentInChildren<Text>(true).text.Contains
                    ("I've got an extra kit right here"))
                {
                    buttons[c].gameObject.SetActive(true);
                }
                else if (!GlobalControls.PlayerHasFirstAidKit && buttons[c].GetComponentInChildren<Text>(true).text.Contains
                    ("I've got an extra kit right here"))
                {
                    buttons[c].gameObject.SetActive(false);
                }
            }
            
            //If the player leaves a trading session...
            if (keyboardManager.leftTrading)
            {
                keyboardManager.leftTrading = false;
                
                //If Angie has the first aid kit...
                if (GlobalItemList.ItemList["First Aid Kit"].containerName.Equals("angie0"))
                {
                    //serious path
                    if (currentNode.nodeName.Equals("checkpointangie0.10"))
                    {
                        //Change the conversation node to angie1.5
                        Debug.Log("Change to angie1.5");
                        currentNode = forest["angie1.5"];
                        GlobalControls.SetCheckpoint("checkpointangie3.0");
                    }
                    //fun path
                    else
                    {
                        //Change the conversation node to angie0.7
                        Debug.Log("Change to angie0.7");
                        currentNode = forest["angie0.7"];
                        GlobalControls.SetCheckpoint("angie4.0");
                    }

                }
                //If trading but angie doesn't have the first aid kit
                else
                {
                    //serious path
                    if (currentNode.nodeName.Equals("checkpointangie0.10") || currentNode.nodeName.Equals("checkpointangie1.0"))
                    {
                        //Change the conversation node to angie1
                        Debug.Log("Change to angie1");
                        currentNode = forest["angie1"];
                    }
                    //fun path
                    else
                    {
                        //Change the conversation node to angie00000
                        Debug.Log("Change to angie00000");
                        currentNode = forest["angie00000"];
                    }
                }
                //Go through all the buttons and put that node's text into the buttons.
                for (int i = 0; i < currentNode.playerArray.Count; i++)
                {
                    buttons[i].gameObject.SetActive(true);
                    buttons[i].GetComponentInChildren<Text>().text = currentNode.playerArray[c];

                    //Turns a button off if there is no text in the button
                    if (buttons[i].GetComponentInChildren<Text>().text.Equals(""))
                    {
                        buttons[i].gameObject.SetActive(false);
                    }
                }
                
                
            }
            
        }
        //This displays the initial nodes npc text
        npcText.GetComponentInChildren<Text>().text = currentNode.npcText;
        
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
        //This will change the node you're looking at
        currentNode = forest[currentNode.nextNode[cursorLocation]];
        Debug.Log("Current Node " + currentNode.nodeName);
        
        //switch statement cases for actions
        int x = 0;
        if (GlobalControls.CurrentNPC.Equals("dem0"))
            x = 1;
        if (GlobalControls.CurrentNPC.Equals("rainer0"))
            x = 2;
        if (GlobalControls.CurrentNPC.Equals("annette0"))
            x = 3;
        if (GlobalControls.CurrentNPC.Equals("safi0"))
            x = 4;

        //switch statement cases for drinks
        int y = 0;
        if (GlobalControls.CurrentNPC.Equals("dem0"))
            y = 1;
        if (GlobalControls.CurrentNPC.Equals("carlos0"))
            y = 2;
        if (GlobalControls.CurrentNPC.Equals("angie0"))
            y = 3;
        if (GlobalControls.CurrentNPC.Equals("rainer0"))
            y = 4;
        if (GlobalControls.CurrentNPC.Equals("annette0"))
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
                    GlobalControls.CurrentPoints += GlobalControls.Points["favors"];
                    GlobalControls.NPCList["dem0"].satisfaction++;
                    referenceManager.pointsText.GetComponentInChildren<Text>(true).text = GlobalControls.CurrentPoints.ToString();
                    break;
                case 2:
                    GlobalControls.RainerActionDone = true;
                    GlobalControls.CurrentPoints += GlobalControls.Points["favors"];
                    GlobalControls.NPCList["rainer0"].satisfaction++;
                    referenceManager.pointsText.GetComponentInChildren<Text>(true).text = GlobalControls.CurrentPoints.ToString();
                    break;
                case 3:
                    GlobalControls.AnnetteActionDone = true;
                    GlobalControls.CurrentPoints += GlobalControls.Points["favors"];
                    GlobalControls.NPCList["annette0"].satisfaction++;
                    referenceManager.pointsText.GetComponentInChildren<Text>(true).text = GlobalControls.CurrentPoints.ToString();
                    break;
            }

        }
        if (x == 4)
        {
            if (currentNode.nodeName.Contains("safiwatersuccess"))
            {
                Debug.Log("Water Done");
                GlobalControls.SafiWaterActionDone = true;
                GlobalControls.CurrentPoints += GlobalControls.Points["favors"];
                GlobalControls.NPCList["safi0"].satisfaction++;
                referenceManager.pointsText.GetComponentInChildren<Text>(true).text = GlobalControls.CurrentPoints.ToString();
            }
            else if(currentNode.nodeName.Contains("safigassuccess"))
            {
                Debug.Log("Ass Done");
                GlobalControls.SafiGasActionDone = true;
                GlobalControls.CurrentPoints += GlobalControls.Points["favors"];
                GlobalControls.NPCList["safi0"].satisfaction++;
                referenceManager.pointsText.GetComponentInChildren<Text>(true).text = GlobalControls.CurrentPoints.ToString();
            }
            else if(currentNode.nodeName.Contains("checkpointsafi1") && !GlobalControls.SafiRescued)
            {
                GlobalControls.SafiRescued = true;
                GlobalControls.CurrentPoints += GlobalControls.Points["safirescue"];
                GlobalControls.NPCList["safi0"].satisfaction++;
                referenceManager.pointsText.GetComponentInChildren<Text>(true).text = GlobalControls.CurrentPoints.ToString();
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
                    GlobalControls.CurrentPoints += GlobalControls.Points["drink"];
                    GlobalControls.NPCList["dem0"].satisfaction++;
                    referenceManager.pointsText.GetComponentInChildren<Text>(true).text = GlobalControls.CurrentPoints.ToString();
                    break;
                case 2:
                    GlobalControls.CarlosDrinkDone = true;
                    GlobalControls.CurrentPoints += GlobalControls.Points["drink"];
                    GlobalControls.NPCList["carlos0"].satisfaction++;
                    referenceManager.pointsText.GetComponentInChildren<Text>(true).text = GlobalControls.CurrentPoints.ToString();
                    break;
                case 3:
                    GlobalControls.AngieDrinkDone = true;
                    GlobalControls.CurrentPoints += GlobalControls.Points["drink"];
                    GlobalControls.NPCList["angie0"].satisfaction++;
                    referenceManager.pointsText.GetComponentInChildren<Text>(true).text = GlobalControls.CurrentPoints.ToString();
                    break;
                case 4:
                    GlobalControls.RainerDrinkDone = true;
                    GlobalControls.CurrentPoints += GlobalControls.Points["drink"];
                    GlobalControls.NPCList["rainer0"].satisfaction++;
                    referenceManager.pointsText.GetComponentInChildren<Text>(true).text = GlobalControls.CurrentPoints.ToString();
                    break;
                case 5:
                    GlobalControls.AnnetteDrinkDone = true;
                    GlobalControls.CurrentPoints += GlobalControls.Points["drink"];
                    GlobalControls.NPCList["annette0"].satisfaction++;
                    referenceManager.pointsText.GetComponentInChildren<Text>(true).text = GlobalControls.CurrentPoints.ToString();
                    break;
            }

        }


        if (currentNode.nodeName.Contains("checkpoint"))
        {
            GlobalControls.SetCheckpoint(currentNode.nodeName);
        }
        
        //** If the player is in the fun path of Angie's dialogue and does not have the first aid kit, set the
        //   checkpoint to be checkpointangie2.0 */
        if (currentNode.nodeName.Contains("angie0.6"))
        {
            Debug.Log("Setting Checkpoint to checkpointangie2.0");
            GlobalControls.SetCheckpoint("checkpointangie2.0");
        }
        if (currentNode.nodeName.Contains("angie1.3"))
        {
            Debug.Log("Setting Checkpoint to checkpointangie1.0");
            GlobalControls.SetCheckpoint("checkpointangie1.0");
        }



        if (currentNode.nodeName.Contains("trade"))
        {
            buttons[cursorLocation].Select();
            keyboardManager.SetTrading();
        }
        
        if (currentNode.nodeName.Contains("leave"))
        {
            buttons[cursorLocation].Select();
            keyboardManager.SetExploring();
        }
        
        
        for (int c = 0; c < currentNode.playerArray.Count; c++)
        {
            buttons[c].gameObject.SetActive(true);
                
            //This will change the player text based on the node we're looking at
            buttons[c].GetComponentInChildren<Text>(true).text = currentNode.playerArray[c]; 
            if (buttons[c].GetComponentInChildren<Text>(true).text.Equals(""))
            {
                buttons[c].gameObject.SetActive(false);
            }

            if (currentNode.nodeName.Contains("drink"))
            {
                if (GlobalControls.PlayerHasCleanWater && buttons[c].GetComponentInChildren<Text>(true).text.Contains
                    ("I have water right here!"))
                {
                    buttons[c].gameObject.SetActive(true);
                }
                else if (!GlobalControls.PlayerHasCleanWater && buttons[c].GetComponentInChildren<Text>(true).text.Contains
                    ("I have water right here!"))
                {
                    buttons[c].gameObject.SetActive(false);
                }
            }
            
            if (currentNode.nodeName.Contains("angie"))
            {
                if (GlobalControls.PlayerHasFirstAidKit && buttons[c].GetComponentInChildren<Text>(true).text.Contains
                    ("I've got an extra kit right here"))
                {
                    buttons[c].gameObject.SetActive(true);
                }
                else if (!GlobalControls.PlayerHasFirstAidKit && buttons[c].GetComponentInChildren<Text>(true).text.Contains
                    ("I've got an extra kit right here"))
                {
                    buttons[c].gameObject.SetActive(false);
                }
            }
            
            if (currentNode.nodeName.Contains("safishutoffwater") || currentNode.nodeName.Contains("safishutoffgas"))
            {
                if (GlobalControls.PlayerHasWrench && buttons[c].GetComponentInChildren<Text>(true).text.Contains
                    ("I have one right here!"))
                {
                    buttons[c].gameObject.SetActive(true);
                }
                else if (!GlobalControls.PlayerHasWrench && buttons[c].GetComponentInChildren<Text>(true).text.Contains
                    ("I have one right here!"))
                {
                    buttons[c].gameObject.SetActive(false);
                }
            }
            
            
            //Turns off button if Demetrius drinks or Demetrius action is complete
            if (currentNode.nodeName.Contains("dem0"))
            {
                if (buttons[c].GetComponentInChildren<Text>(true).text.Contains("Foraging") && GlobalControls.DemActionDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }
                
                if (buttons[c].GetComponentInChildren<Text>(true).text.Contains("Drink") && GlobalControls.DemDrinkDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
            if (currentNode.nodeName.Contains("checkpointsafi1"))
            {
                if (buttons[c].GetComponentInChildren<Text>(true).text.Contains("Water") && GlobalControls.SafiWaterActionDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }
                
                if (buttons[c].GetComponentInChildren<Text>(true).text.Contains("Gas") && GlobalControls.SafiGasActionDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
            //Turns off button if Annette drinks or Annette action is complete
            if (currentNode.nodeName.Contains("annette0"))
            {
                if (buttons[c].GetComponentInChildren<Text>(true).text.Contains("Translation") && GlobalControls.AnnetteActionDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }
                
                if (buttons[c].GetComponentInChildren<Text>(true).text.Contains("Drink") && GlobalControls.AnnetteDrinkDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
            //Turns off button if Rainer drinks or Rainer action is complete
            if (currentNode.nodeName.Contains("rainer0"))
            {
                if (buttons[c].GetComponentInChildren<Text>(true).text.Contains("Action") && GlobalControls.RainerActionDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }
                
                if (buttons[c].GetComponentInChildren<Text>(true).text.Contains("Drink") && GlobalControls.RainerDrinkDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
            //Turns off button if Carlos drinks is complete
            if (currentNode.nodeName.Contains("carlos0"))
            {
                if (buttons[c].GetComponentInChildren<Text>(true).text.Contains("Drink") && GlobalControls.CarlosDrinkDone)
                {
                    buttons[c].gameObject.SetActive(false);
                }

            }
            
           
        }
            
        //This will change the npc text based on the node
        npcText.GetComponentInChildren<Text>(true).text = currentNode.npcText;
        
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
