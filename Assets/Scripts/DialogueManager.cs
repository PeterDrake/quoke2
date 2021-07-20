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
            if (currentNode.nodeName.Contains("angie0"))
            {
                if (buttons[c].GetComponentInChildren<Text>().text.Contains("Drink") && GlobalControls.AngieDrinkDone)
                {
                    buttons[c].gameObject.SetActive(false);
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

        if (currentNode.nodeName.Contains("trade"))
        {
            buttons[cursorLocation].Select();
            keyboardManager.SetTrading();
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
            
            //Turns off button if Angie drinks is complete
            if (currentNode.nodeName.Contains("angie0"))
            {
                if (buttons[c].GetComponentInChildren<Text>(true).text.Contains("Drink") && GlobalControls.AngieDrinkDone)
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
