using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Linq;
using Object = System.Object;

public class ConvoNode
{
    public ConvoNode(XmlNode xml)
    {
        nodeName = xml.Name;
        IEnumerator sectionInfoEnumerator = xml.GetEnumerator();
        sectionInfoEnumerator.MoveNext();

        // Attach the appropriate npc text to the corresponding node
        XmlElement npcElement = (XmlElement) sectionInfoEnumerator.Current;
        npcText = npcElement.InnerXml;
        sectionInfoEnumerator.MoveNext();

        // Attach the text options for the player into appropriate nodes
        XmlElement playerArrayElement = (XmlElement) sectionInfoEnumerator.Current;
        IEnumerator playerOptionsEnumerator = playerArrayElement.GetEnumerator();
        playerArray = new List<string>();
        while (playerOptionsEnumerator.MoveNext())
        {
            XmlElement playerOptionsElement = (XmlElement) playerOptionsEnumerator.Current;
            playerArray.Add(playerOptionsElement.InnerXml);
        }
        sectionInfoEnumerator.MoveNext();

        // Attach the node keys to the appropriate nodes
        XmlElement keyArrayElement = (XmlElement) sectionInfoEnumerator.Current;
        IEnumerator keyOptionsEnumerator = keyArrayElement.GetEnumerator();
        nextNode = new List<string>();
        while (keyOptionsEnumerator.MoveNext())
        {
            XmlElement keyOptionsElement = (XmlElement) keyOptionsEnumerator.Current;
            nextNode.Add(keyOptionsElement.InnerXml);
        }
    }


    //array of what player can say here
    public List<string> playerArray;

    //array of next nodes based on what player says
    public List<string> nextNode;

    //the text that is displayed by the npc in this node
    public string npcText;

    //name of node
    public string nodeName;
}