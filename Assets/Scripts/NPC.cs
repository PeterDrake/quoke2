using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC
{
    public List<string> needs;
    public List<bool> needsMet;
    public string name;
    public string scene;
    public string node;
    public int satisfaction;
    public int totalSatisfaction;
    public bool interacted;
    public string description;
    public int owes;
    public List<DialogueNode> dialogueList;
    public Dictionary<string, string> actions;
    public List<string> actionRequirements;
    public bool[] actionsComplete = new bool[3];

    public NPC(string name, string scene, List<string> needs, List<bool> needsMet, string node, int satisfaction, 
        bool interacted, string description, int totalSatisfaction, int owes, bool[] actionsComplete,
        List<string> actionRequirements)
    {
        this.name = name;
        this.scene = scene;
        this.needs = needs;
        this.needsMet = needsMet;
        this.node = node;
        this.satisfaction = satisfaction;
        this.interacted = interacted;
        this.description = description;
        this.totalSatisfaction = totalSatisfaction;
        this.owes = owes;
        this.dialogueList = new List<DialogueNode>();
        this.actionsComplete = actionsComplete;
        this.actionRequirements = actionRequirements;
    }
}

public class DialogueNode
{
    public string text;
    public string name;
    public DialogueNode(string text, string name)
    {
        this.text = text;
        this.name = name;
    }
}
