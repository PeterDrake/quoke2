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
    public string[] textArray;
    public string[] npcArray;
    public Button[] buttons;
    public GameObject npcText;
    private XmlDocument convoFile = new XmlDocument();
    public Dictionary<string, convoNode> forest = new Dictionary<string, convoNode>();
    public convoNode currentNode;
    
        // Start is called before the first frame update
        private void Start()
        {
            //buttons = new[] {button1, button2, button3};
            
            convoFile.Load("Assets/Resources/TestTree.txt");
            
           
            foreach (XmlNode node in convoFile.LastChild) //looks through all the npc nodes instead of looking at just the <convoForest> tag
            {
                forest.Add(node.Name, new convoNode(node));
            }

            currentNode = forest["dem0"]; // This is where the we let the NPC talk to the code. The npc we run into will pass back something like "theirName0" to get to the appropriate starting node
            
            for (int c = 0; c < currentNode.playerArray.Count; c++)
            {
                buttons[c].GetComponentInChildren<Text>().text = currentNode.playerArray[c];
            }

            npcText.GetComponentInChildren<Text>().text = currentNode.npcText;
            
            
           
            npcArray = new[] {"You said 'button 1'", "You said 'button 2'", "You said 'button 3'"};
            textArray = new[] {"Hey here's text for button 1", "Hey here's text for button 2", "Hey here's text for button 3"};
           
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
          
        }
        
        if (Input.GetKeyDown("space"))
        {
            currentNode = forest[currentNode.nextNode[cursorLocation]];
            
            for (int c = 0; c < currentNode.playerArray.Count; c++)
            {
                buttons[c].GetComponentInChildren<Text>().text = currentNode.playerArray[c];
            }

            npcText.GetComponentInChildren<Text>().text = currentNode.npcText;
            

        }
        //we can use the below comment to display text on our buttons through an array of strings,

        
    }

   
    
}
