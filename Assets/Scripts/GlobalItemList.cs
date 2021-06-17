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
            {"Rope", new Item(new Vector3(-7.5f,0f,6.5f), "Rope", 
                "Park", "")},
            {"Dog Collar", new Item(new Vector3(6.5f,0f,3.5f), "Dog Collar", 
                "School", "")},
            {"Pet Rat", new Item(new Vector3(-11.5f,0f,-2.5f), "Pet Rat", 
                "School", "")},
            {"Bag", new Item(new Vector3(2.5f,0f,9.5f), "Bag", 
                "School", "")},
            {"Mask", new Item(new Vector3(0, 0, 0), "Mask", 
                "Inventory", "safi0")},
            {"Phone", new Item(new Vector3(1,0,0), "Phone", 
                "Inventory", "safi0")},
            {"Burger", new Item(new Vector3(2,0,0), "Burger", 
                "Inventory", "safi0")},
            {"Book", new Item(new Vector3(3,0,0), "Book", 
                "Inventory", "dem0")},
            {"Cup", new Item(new Vector3(4,0,0), "Cup", 
                "Inventory", "dem0")},
            
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
            {"Rope", new Item(new Vector3(-7.5f,0f,6.5f), "Rope", 
                "Park", "")},
            {"Dog Collar", new Item(new Vector3(6.5f,0f,3.5f), "Dog Collar", 
                "School", "")},
            {"Pet Rat", new Item(new Vector3(-11.5f,0f,-2.5f), "Pet Rat", 
                "School", "")},
            {"Bag", new Item(new Vector3(2.5f,0f,9.5f), "Bag", 
                "School", "")},
            {"Mask", new Item(new Vector3(0, 0, 0), "Mask", 
                "Inventory", "safi0")},
            {"Phone", new Item(new Vector3(1,0,0), "Phone", 
                "Inventory", "safi0")},
            {"Burger", new Item(new Vector3(2,0,0), "Burger", 
                "Inventory", "safi0")},
            {"Book", new Item(new Vector3(3,0,0), "Book", 
                "Inventory", "dem0")},
            {"Cup", new Item(new Vector3(4,0,0), "Cup", 
                "Inventory", "dem0")},
            
        };
    }
    
}