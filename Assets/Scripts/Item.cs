using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public Vector3 location;
    public string name;
    public string scene;
    public string containerName;

    public Item(Vector3 location, string name, string scene, string containerName)
    {
        this.location = location;
        this.name = name;
        this.scene = scene;
        this.containerName = containerName;
    }
    
    public new string ToString()
    {
        return this.name + " in " + this.scene + " at " + this.location + " with container " + this.containerName;
    }
}
