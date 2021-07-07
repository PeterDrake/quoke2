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
        if (GlobalControls.ApartmentCondition)
        {
            itemList = new Dictionary<string, Item>
            {
                {"Cup", new Item(new Vector3(6.5f,0.5f,0.5f), "Cup", 
                    "PreQuakeHouse", "")},
                {"Chlorine Tablet", new Item(new Vector3(-6.5f,0.5f,0.5f), "Chlorine Tablet", 
                    "PreQuakeHouse", "")},
                {"Water Bottle Clean", new Item(new Vector3(-7.5f,0.5f,0.5f), "Water Bottle Clean", 
                    "", "")},
                {"Sandwich", new Item(new Vector3(3.5f,0.5f,3.5f), "Sandwich", 
                    "PreQuakeHouse", "")},
                {"Book", new Item(new Vector3(-5.5f,0.5f,-7.5f), "Book", 
                    "PreQuakeHouse", "")},
                {"Bucket", new Item(new Vector3(0,0f,0), "Bucket", 
                    "Inventory", "safi0")},
                {"Mask", new Item(new Vector3(1, 0, 0), "Mask", 
                    "Inventory", "safi0")},
                {"Wood Chips", new Item(new Vector3(0,0f,0), "Wood Chips", 
                    "Inventory", "dem0")},
                {"Tarp", new Item(new Vector3(1,0f,0), "Tarp", 
                    "Inventory", "dem0")},
                {"Bag", new Item(new Vector3(2,0f,0), "Bag", 
                    "Inventory", "dem0")},
                {"Dog Collar", new Item(new Vector3(3,0f,0), "Dog Collar", 
                    "Inventory", "dem0")},
                {"Bucket 2", new Item(new Vector3(0,0f,0), "Bucket 2", 
                    "Inventory", "rainer0")},
                {"Wrench", new Item(new Vector3(1,0f,0), "Wrench", 
                    "Inventory", "rainer0")},
                {"Water Bottle", new Item(new Vector3(0,0f,0), "Water Bottle", 
                    "Inventory", "fred0")},
                {"Can Opener", new Item(new Vector3(1,0,0), "Can Opener", 
                    "Inventory", "fred0")},
                {"Toilet Paper", new Item(new Vector3(2,0,0), "Toilet Paper", 
                    "Inventory", "rainer0")},
            };
        }
        else itemList = new Dictionary<string, Item>
        {
            {"Cup", new Item(new Vector3(6.5f,0.5f,0.5f), "Cup", 
                "PreQuakeHouse", "")},
            {"Water Bottle", new Item(new Vector3(-6.5f,0.5f,0.5f), "Water Bottle", 
                "PreQuakeHouse", "")},
            {"Water Bottle Clean", new Item(new Vector3(-7.5f,0.5f,0.5f), "Water Bottle Clean", 
                "", "")},
            {"Sandwich", new Item(new Vector3(3.5f,0.5f,3.5f), "Sandwich", 
                "PreQuakeHouse", "")},
            {"Book", new Item(new Vector3(-5.5f,0.5f,-7.5f), "Book", 
                "PreQuakeHouse", "")},
            {"Tarp", new Item(new Vector3(0,0f,0), "Tarp", 
                "Inventory", "safi0")},
            {"Mask", new Item(new Vector3(1, 0, 0), "Mask", 
                "Inventory", "safi0")},
            {"Plywood", new Item(new Vector3(0,0f,0), "Plywood", 
                "Inventory", "dem0")},
            {"Bucket", new Item(new Vector3(1,0f,0), "Bucket", 
                "Inventory", "dem0")},
            {"Rope", new Item(new Vector3(2,0f,0), "Rope", 
                "Inventory", "dem0")},
            {"Dog Collar", new Item(new Vector3(3,0f,0), "Dog Collar", 
                "Inventory", "dem0")},
            {"Shovel", new Item(new Vector3(0,0f,0), "Shovel", 
                "Inventory", "rainer0")},
            {"Wrench", new Item(new Vector3(1,0f,0), "Wrench", 
                "Inventory", "rainer0")},
            {"Chlorine Tablet", new Item(new Vector3(0,0f,0), "Chlorine Tablet", 
                "Inventory", "fred0")},
            {"Can Opener", new Item(new Vector3(1,0,0), "Can Opener", 
                "Inventory", "fred0")},
            {"Toilet Paper", new Item(new Vector3(2,0,0), "Toilet Paper", 
                "Inventory", "rainer0")},
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
        if (GlobalControls.ApartmentCondition)
        {
            itemList = new Dictionary<string, Item>
            {
                {"Cup", new Item(new Vector3(6.5f,0.5f,0.5f), "Cup", 
                    "PreQuakeHouse", "")},
                {"Chlorine Tablet", new Item(new Vector3(-6.5f,0.5f,0.5f), "Chlorine Tablet", 
                    "PreQuakeHouse", "")},
                {"Water Bottle Clean", new Item(new Vector3(-7.5f,0.5f,0.5f), "Water Bottle Clean", 
                    "", "")},
                {"Sandwich", new Item(new Vector3(3.5f,0.5f,3.5f), "Sandwich", 
                    "PreQuakeHouse", "")},
                {"Book", new Item(new Vector3(-5.5f,0.5f,-7.5f), "Book", 
                    "PreQuakeHouse", "")},
                {"Bucket", new Item(new Vector3(0,0f,0), "Bucket", 
                    "Inventory", "safi0")},
                {"Mask", new Item(new Vector3(1, 0, 0), "Mask", 
                    "Inventory", "safi0")},
                {"Wood Chips", new Item(new Vector3(0,0f,0), "Wood Chips", 
                    "Inventory", "dem0")},
                {"Tarp", new Item(new Vector3(1,0f,0), "Tarp", 
                    "Inventory", "dem0")},
                {"Bag", new Item(new Vector3(2,0f,0), "Bag", 
                    "Inventory", "dem0")},
                {"Dog Collar", new Item(new Vector3(3,0f,0), "Dog Collar", 
                    "Inventory", "dem0")},
                {"Bucket 2", new Item(new Vector3(0,0f,0), "Bucket 2", 
                    "Inventory", "rainer0")},
                {"Wrench", new Item(new Vector3(1,0f,0), "Wrench", 
                    "Inventory", "rainer0")},
                {"Water Bottle", new Item(new Vector3(0,0f,0), "Water Bottle", 
                    "Inventory", "fred0")},
                {"Can Opener", new Item(new Vector3(1,0,0), "Can Opener", 
                    "Inventory", "fred0")},
                {"Toilet Paper", new Item(new Vector3(2,0,0), "Toilet Paper", 
                    "Inventory", "rainer0")},
            };
        }
        else 
        {
            itemList = new Dictionary<string, Item>
            {
            {"Cup", new Item(new Vector3(6.5f,0.5f,0.5f), "Cup", 
                "PreQuakeHouse", "")},
            {"Water Bottle", new Item(new Vector3(-6.5f,0.5f,0.5f), "Water Bottle", 
                "PreQuakeHouse", "")},
            {"Water Bottle Clean", new Item(new Vector3(-7.5f,0.5f,0.5f), "Water Bottle Clean", 
                "", "")},
            {"Sandwich", new Item(new Vector3(3.5f,0.5f,3.5f), "Sandwich", 
                "PreQuakeHouse", "")},
            {"Book", new Item(new Vector3(-5.5f,0.5f,-7.5f), "Book", 
                "PreQuakeHouse", "")},
            {"Tarp", new Item(new Vector3(0,0f,0), "Tarp", 
                "Inventory", "safi0")},
            {"Mask", new Item(new Vector3(1, 0, 0), "Mask", 
                "Inventory", "safi0")},
            {"Plywood", new Item(new Vector3(0,0f,0), "Plywood", 
                "Inventory", "dem0")},
            {"Bucket", new Item(new Vector3(1,0f,0), "Bucket", 
                "Inventory", "dem0")},
            {"Rope", new Item(new Vector3(2,0f,0), "Rope", 
                "Inventory", "dem0")},
            {"Dog Collar", new Item(new Vector3(3,0f,0), "Dog Collar", 
                "Inventory", "dem0")},
            {"Shovel", new Item(new Vector3(0,0f,0), "Shovel", 
                "Inventory", "rainer0")},
            {"Wrench", new Item(new Vector3(1,0f,0), "Wrench", 
                "Inventory", "rainer0")},
            {"Chlorine Tablet", new Item(new Vector3(0,0f,0), "Chlorine Tablet", 
                "Inventory", "fred0")},
            {"Can Opener", new Item(new Vector3(1,0,0), "Can Opener", 
                "Inventory", "fred0")},
            {"Toilet Paper", new Item(new Vector3(2,0,0), "Toilet Paper", 
                "Inventory", "rainer0")},
            };
            
        }
    }
    
}