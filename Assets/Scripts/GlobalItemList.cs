using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GlobalItemList
{
    private static Dictionary<string, Item> itemList;

    public static Dictionary<string, Item> ItemList
    {
        get => itemList;
        set => itemList = value;
    }

    static GlobalItemList()
    {
        //ihateuevenmorethanididbeforeuhavenoideahowmuchihateu
        itemList = new Dictionary<string, Item>
        {
            {"Green Capsule", new Item(new Vector3(6.5f,0.5f,0.5f), "Green Capsule", 
                "Assets/Resources/Green Capsule.prefab" , "PreQuakeHouse")},
            {"Red Capsule", new Item(new Vector3(-6.5f,0.5f,0.5f), "Red Capsule", 
                "Assets/Resources/Red Capsule.prefab" , "PreQuakeHouse")},
            {"Yellow Capsule", new Item(new Vector3(3.5f,0.5f,3.5f), "Yellow Capsule", 
                "Assets/Resources/Yellow Capsule.prefab" , "PreQuakeHouse")},
            {"Blue Capsule", new Item(new Vector3(-5.5f,0.5f,-7.5f), "Blue Capsule", 
                "Assets/Resources/Blue Capsule.prefab", "PreQuakeHouse")},
            {"Capsule", new Item(new Vector3(6.5f,0f,0.5f), "Capsule", 
                "Assets/Resources/Capsule.prefab", "Park")},
            {"Capsule (1)", new Item(new Vector3(-8.5f,0f,5.5f), "Capsule (1)", 
                "Assets/Resources/Capsule (1).prefab", "Park")},
            {"Capsule (2)", new Item(new Vector3(-4.5f,0f,7.5f), "Capsule (2)", 
                "Assets/Resources/Capsule (2).prefab", "School")},
            {"Capsule (3)", new Item(new Vector3(6.5f,0f,3.5f), "Capsule (3)", 
                "Assets/Resources/Capsule (3).prefab", "School")},
            {"Capsule (4)", new Item(new Vector3(-11.5f,0f,-2.5f), "Capsule (4)", 
                "Assets/Resources/Capsule (4).prefab", "School")},
            {"Capsule (5)", new Item(new Vector3(2.5f,0f,9.5f), "Capsule (5)", 
                "Assets/Resources/Capsule (5).prefab", "School")},
        };
    }

    /** Updates itemList with the picked up item's position and scene. */
    public static void UpdateItemList(string name, string target, Vector3 position)
    {
        name = name.Replace("(Clone)","").Trim();
        itemList[name] = new Item(position, name, itemList[name].prefab, target);
    }
    
}


public class Item
{
    public Vector3 location;
    public string itemName;
    public string prefab;
    public string scene;

    public Item(Vector3 location, string itemName, string prefab, string scene)
    {
        this.location = location;
        this.itemName = itemName;
        this.prefab = prefab;
        this.scene = scene;
    }
}

