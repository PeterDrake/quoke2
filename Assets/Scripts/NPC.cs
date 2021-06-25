using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC
{
    public List<string> needs;
    public string name;
    public string scene;
    public string node;
    public int satisfaction;
    public bool interracted;
    public string description;

    public NPC(string name, string scene, List<string> needs, string node, int satisfaction, bool interracted, string description)
    {
        this.name = name;
        this.scene = scene;
        this.needs = needs;
        this.node = node;
        this.satisfaction = satisfaction;
        this.interracted = interracted;
        this.description = description;
    }
}
