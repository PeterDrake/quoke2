using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class convoNode
{
    public convoNode(XmlNode xml)
    {
        foreach (XmlNode sectionInfo in xml)
        {
            if (sectionInfo.Value != null)
            {
                npcText = sectionInfo.Value;
            }

            playerArray = new List<string>();
            
            foreach (XmlNode playerOptions in sectionInfo.FirstChild)
            {
                playerArray.Add(playerOptions.Value);

            }
            nextNode = new List<string>();
            foreach (XmlNode playerOptions in sectionInfo.LastChild)
            {
                nextNode.Add(playerOptions.Value);

            }
            //file.Add(tree.Name, tree.Value);
        }
        
    }
    //array of what player can say here
    public List<string> playerArray;
    //array of next nodes based on what player says
    public List<string> nextNode;
    //the text that is displayed by the npc in this node
    public string npcText;

}
