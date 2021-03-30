using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class convoNode
{
    //array of what player can say here
    public string[] playerArray;
    //array of next nodes based on what player says
    public convoNode[] nextNode;
    //the text that is displayed by the npc in this node
    public string npcText;
    

}
