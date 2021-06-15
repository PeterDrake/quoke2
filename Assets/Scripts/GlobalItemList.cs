using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public static class GlobalItemList
{
    private static Dictionary<string, Item> itemList;
    private static Dictionary<string, NPC> npcList;

    public static Dictionary<string, Item> ItemList
    {
        get => itemList;
        set => itemList = value;
    }
    
    public static Dictionary<string, NPC> NPCList
    {
        get => npcList;
        set => npcList = value;
    }

    static GlobalItemList()
    {
        //ihateuevenmorethanididbeforeuhavenoideahowmuchihateu
        itemList = new Dictionary<string, Item>
        {
            {"Wrench", new Item(new Vector3(6.5f,0.5f,0.5f), "Wrench", 
                "PreQuakeHouse", "")},
            {"Water Bottle", new Item(new Vector3(-6.5f,0.5f,0.5f), "Water Bottle", 
                "PreQuakeHouse", "")},
            {"Sandwich", new Item(new Vector3(3.5f,0.5f,3.5f), "Sandwich", 
                "PreQuakeHouse", "")},
            {"Shovel", new Item(new Vector3(-5.5f,0.5f,-7.5f), "Shovel", 
                "PreQuakeHouse", "")},
            {"Tarp", new Item(new Vector3(6.5f,0f,0.5f), "Tarp", 
                "Park", "")},
            {"Bucket", new Item(new Vector3(-8.5f,0f,5.5f), "Bucket", 
                "Park", "")},
            {"Rope", new Item(new Vector3(-4.5f,0f,7.5f), "Rope", 
                "School", "")},
            {"Dog Collar", new Item(new Vector3(6.5f,0f,3.5f), "Dog Collar", 
                "School", "")},
            {"Pet Rat", new Item(new Vector3(-11.5f,0f,-2.5f), "Pet Rat", 
                "School", "")},
            {"Bag", new Item(new Vector3(2.5f,0f,9.5f), "Bag", 
                "School", "")},
            };
        npcList = new Dictionary<string, NPC>
        {
            {"safi0", new NPC("safi0", "Park", new List<string>{"Wrench"})},
            {"dem0", new NPC("dem0", "Park", new List<string>{"Dog Collar"})},
            {"fred0", new NPC("fred0", "School", new List<string>{"Wrench"})},
            {"rainer0", new NPC("rainer0", "School", new List<string>{"Wrench"})},
        };
    }

    /** Updates itemList with the picked up item's position and scene. */
    public static void UpdateItemList(string name, string target, Vector3 position, string containerName)
    {
        name = name.Replace("(Clone)","").Trim();
        itemList[name] = new Item(position, name, target, containerName);
    }
    
}