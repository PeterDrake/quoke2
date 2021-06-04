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
        new Dictionary<string, Item>()
        {
            {"dum1", new Item(new Vector3(), "Green Capsule", 
                "Assets/Prefabs/Green Capsule" , "PreQuakeHouse")},
            {"dum2", new Item(new Vector3(), "Red Capsule", 
                "Assets/Prefabs/Red Capsule" , "PreQuakeHouse")},
            {"dum3", new Item(new Vector3(), "Yellow Capsule", 
                "Assets/Prefabs/Yellow Capsule" , "PreQuakeHouse")},
            {"dum5", new Item(new Vector3(), "Blue Capsule", 
                "Assets/Prefabs/Blue Capsule", "PreQuakeHouse")},
        };
    }

    /** Updates itemList with the picked up item's position and scene. */
    public static void UpdateItemList(GameObject pickupableCollectible, string target, Vector3 position)
    {
        ItemList[pickupableCollectible.name] = new Item(new Vector3(position.x, position.y, position.z), 
            pickupableCollectible.name, itemList[pickupableCollectible.name].prefab, target);
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

