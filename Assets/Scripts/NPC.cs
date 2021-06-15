using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC
{
    public List<string> needs;
    public string name;

    public NPC(string name, List<string> needs)
    {
        this.name = name;
        this.needs = needs;
    }
}
