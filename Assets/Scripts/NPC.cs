using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC
{
    public List<string> needs;
    public string name;
    public string scene;
    public string node;
    public int satisfaction;
    public int totalSatisfaction;
    public bool interracted;
    public string description;
    public int owes;
    public List<DialogueNode> dialogueList;
    public Dictionary<string, string> actions;

    public NPC(string name, string scene, List<string> needs, string node, int satisfaction, bool interracted, string description, int totalSatisfaction, int owes, Dictionary<string, string> actions)
    {
        this.name = name;
        this.scene = scene;
        this.needs = needs;
        this.node = node;
        this.satisfaction = satisfaction;
        this.interracted = interracted;
        this.description = description;
        this.totalSatisfaction = totalSatisfaction;
        this.owes = owes;
        this.dialogueList = new List<DialogueNode>();
        this.actions = actions;
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
