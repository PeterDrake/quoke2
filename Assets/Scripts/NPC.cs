using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC
{
    public List<string> needs;
    public string name;
    public string scene;

    public NPC(string name, string scene, List<string> needs)
    {
        this.name = name;
        this.scene = scene;
        this.needs = needs;
    }
}
