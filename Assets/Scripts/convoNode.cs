using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Linq;
using Object = System.Object;

public class convoNode
{
    public convoNode(XmlNode xml)
    {
        IEnumerator sectionInfoEnumerator = xml.GetEnumerator();
        
        sectionInfoEnumerator.MoveNext();

        XmlElement playerArrayElement = (XmlElement) sectionInfoEnumerator.Current; //this block of code will put the text options for the player into appropriate nodes
        IEnumerator playerOptionsEnumerator = playerArrayElement.GetEnumerator();
        playerArray = new List<string>();
        while (playerOptionsEnumerator.MoveNext())
        {
           XmlElement playerOptionsElement = (XmlElement) playerOptionsEnumerator.Current;
           playerArray.Add(playerOptionsElement.InnerXml);
        }
        
        sectionInfoEnumerator.MoveNext();

        XmlElement keyArrayElement = (XmlElement) sectionInfoEnumerator.Current; //This block of code will put the node keys into the appropriate nodes
        IEnumerator keyOptionsEnumerator = keyArrayElement.GetEnumerator();
        nextNode = new List<string>();
        while (keyOptionsEnumerator.MoveNext())
        {
            XmlElement keyOptionsElement = (XmlElement) keyOptionsEnumerator.Current;
            nextNode.Add(keyOptionsElement.InnerXml);
        }

        sectionInfoEnumerator.MoveNext();

        XmlElement npcElement = (XmlElement) sectionInfoEnumerator.Current; // these two lines will attach the appropriate npc text to the corresponding node

        npcText = npcElement.InnerXml;




    }
    
    
    //array of what player can say here
    public List<string> playerArray;
    //array of next nodes based on what player says
    public List<string> nextNode;
    //the text that is displayed by the npc in this node
    public string npcText;

}
