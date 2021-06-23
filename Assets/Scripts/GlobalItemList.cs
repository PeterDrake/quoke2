using System.Collections.Generic;
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
            {"Cup", new Item(new Vector3(6.5f,0.5f,0.5f), "Cup", 
                "PreQuakeHouse", "")},
            {"Water Bottle", new Item(new Vector3(-6.5f,0.5f,0.5f), "Water Bottle", 
                "PreQuakeHouse", "")},
            {"Sandwich", new Item(new Vector3(3.5f,0.5f,3.5f), "Sandwich", 
                "PreQuakeHouse", "")},
            {"Book", new Item(new Vector3(-5.5f,0.5f,-7.5f), "Book", 
                "PreQuakeHouse", "")},
            {"Plywood", new Item(new Vector3(1.5f,0f,8.5f), "Plywood", 
                "School", "")},
            {"Bucket", new Item(new Vector3(-8.5f,0f,5.5f), "Bucket", 
                "Park", "")},
            {"Tarp", new Item(new Vector3(0,0f,0), "Tarp", 
                "Inventory", "safi0")},
            {"Mask", new Item(new Vector3(1, 0, 0), "Mask", 
                "Inventory", "safi0")},
            {"Rope", new Item(new Vector3(0,0f,0), "Rope", 
                "Inventory", "dem0")},
            {"Dog Collar", new Item(new Vector3(1,0f,0), "Dog Collar", 
                "Inventory", "dem0")},
            {"Shovel", new Item(new Vector3(0,0f,0), "Shovel", 
                "Inventory", "rainer0")},
            {"Wrench", new Item(new Vector3(1,0f,0), "Wrench", 
                "Inventory", "rainer0")},
            {"Chlorine Tablet", new Item(new Vector3(0,0f,0), "Chlorine Tablet", 
                "Inventory", "fred0")},
            {"Can Opener", new Item(new Vector3(1,0,0), "Can Opener", 
                "Inventory", "fred0")},
            {"Pet Rat", new Item(new Vector3(-11.5f,0f,-2.5f), "Pet Rat", 
                "NONE", "")},
            {"Burger", new Item(new Vector3(2,0,0), "Burger", 
                "NONE", "")},
        };
    }

    /** Updates itemList with the picked up item's position and scene. */
    public static void UpdateItemList(string name, string target, Vector3 position, string containerName)
    {
        name = name.Replace("(Clone)","").Trim();
        itemList[name] = new Item(position, name, target, containerName);
    }

    public static void Reset()
    {
        itemList = new Dictionary<string, Item>
        {
            {"Cup", new Item(new Vector3(6.5f,0.5f,0.5f), "Cup", 
                "PreQuakeHouse", "")},
            {"Water Bottle", new Item(new Vector3(-6.5f,0.5f,0.5f), "Water Bottle", 
                "PreQuakeHouse", "")},
            {"Sandwich", new Item(new Vector3(3.5f,0.5f,3.5f), "Sandwich", 
                "PreQuakeHouse", "")},
            {"Book", new Item(new Vector3(-5.5f,0.5f,-7.5f), "Book", 
                "PreQuakeHouse", "")},
            {"Plywood", new Item(new Vector3(1.5f,0f,8.5f), "Plywood", 
                "School", "")},
            {"Bucket", new Item(new Vector3(-8.5f,0f,5.5f), "Bucket", 
                "Park", "")},
            {"Tarp", new Item(new Vector3(0,0f,0), "Tarp", 
                "Inventory", "safi0")},
            {"Mask", new Item(new Vector3(1, 0, 0), "Mask", 
                "Inventory", "safi0")},
            {"Rope", new Item(new Vector3(0,0f,0), "Rope", 
                "Inventory", "dem0")},
            {"Dog Collar", new Item(new Vector3(1,0f,0), "Dog Collar", 
                "Inventory", "dem0")},
            {"Shovel", new Item(new Vector3(0,0f,0), "Shovel", 
                "Inventory", "rainer0")},
            {"Wrench", new Item(new Vector3(1,0f,0), "Wrench", 
                "Inventory", "rainer0")},
            {"Chlorine Tablet", new Item(new Vector3(0,0f,0), "Chlorine Tablet", 
                "Inventory", "fred0")},
            {"Can Opener", new Item(new Vector3(1,0,0), "Can Opener", 
                "Inventory", "fred0")},
            {"Pet Rat", new Item(new Vector3(-11.5f,0f,-2.5f), "Pet Rat", 
                "NONE", "")},
            {"Burger", new Item(new Vector3(2,0,0), "Burger", 
                "NONE", "")},
        };
    }
    
}