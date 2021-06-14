using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
            {"Wrench", new Item(new Vector3(6.5f,0.5f,0.5f), "Wrench", 
                "Assets/Prefabs/Wrench.prefab" , "PreQuakeHouse", "")},
            {"Water Bottle", new Item(new Vector3(-6.5f,0.5f,0.5f), "Water Bottle", 
                "Assets/Prefabs/Water Bottle.prefab" , "PreQuakeHouse", "")},
            {"Sandwich", new Item(new Vector3(3.5f,0.5f,3.5f), "Sandwich", 
                "Assets/Prefabs/Sandwich.prefab" , "PreQuakeHouse", "")},
            {"Shovel", new Item(new Vector3(-5.5f,0.5f,-7.5f), "Shovel", 
                "Assets/Prefabs/Shovel.prefab", "PreQuakeHouse", "")},
            {"Tarp", new Item(new Vector3(6.5f,0f,0.5f), "Tarp", 
                "Assets/Prefabs/Tarp.prefab", "Park", "")},
            {"Bucket", new Item(new Vector3(-8.5f,0f,5.5f), "Bucket", 
                "Assets/Prefabs/Bucket.prefab", "Park", "")},
            {"Rope", new Item(new Vector3(-4.5f,0f,7.5f), "Rope", 
                "Assets/Prefabs/Rope.prefab", "School", "")},
            {"Dog Collar", new Item(new Vector3(6.5f,0f,3.5f), "Dog Collar", 
                "Assets/Prefabs/Dog Collar.prefab", "School", "")},
            {"Pet Rat", new Item(new Vector3(-11.5f,0f,-2.5f), "Pet Rat", 
                "Assets/Prefabs/Pet Rat.prefab", "School", "")},
            {"Bag", new Item(new Vector3(2.5f,0f,9.5f), "Bag", 
                "Assets/Prefabs/Bag.prefab", "School", "")},
        };
    }

    /** Updates itemList with the picked up item's position and scene. */
    public static void UpdateItemList(string name, string target, Vector3 position, string containerName)
    {
        name = name.Replace("(Clone)","").Trim();
        itemList[name] = new Item(position, name, itemList[name].prefab, target, containerName);
    }
    
}


public class Item
{
    public Vector3 location;
    public string itemName;
    public string prefab;
    public string scene;
    public string containerName;

    public Item(Vector3 location, string itemName, string prefab, string scene, string containerName)
    {
        this.location = location;
        this.itemName = itemName;
        this.prefab = prefab;
        this.scene = scene;
        this.containerName = containerName;
    }
}

