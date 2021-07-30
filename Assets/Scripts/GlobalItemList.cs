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
                {"Sunscreen", new Item(new Vector3(6.5f,0.5f,0.5f), "Sunscreen", 
                    "PreQuakeHouse", "")},
                {"Bleach", new Item(new Vector3(-6.5f,0.5f,0.5f), "Bleach", 
                    "PreQuakeHouse", "")},
                {"Book", new Item(new Vector3(-5.5f,0.5f,-7.5f), "Book", 
                    "PreQuakeHouse", "")},
                {"Flashlight", new Item(new Vector3(3.5f,0.5f,3.5f), "Flashlight", 
                    "PreQuakeHouse", "")},
                {"Dirty Water Bottle", new Item(new Vector3(0,0f,0), "Dirty Water Bottle", 
                    "Inventory", "dem0")},
                {"Tent", new Item(new Vector3(1,0f,0), "Tent", 
                    "Inventory", "dem0")},
                {"Dog Crate", new Item(new Vector3(2,0f,0), "Dog Crate", 
                    "Inventory", "dem0")},
                {"Rubber Chicken", new Item(new Vector3(3,0f,0), "Rubber Chicken", 
                    "Inventory", "dem0")},
                {"Toilet Paper", new Item(new Vector3(0,0,0), "Toilet Paper", 
                    "Inventory", "rainer0")},
                {"Batteries", new Item(new Vector3(1,0f,0), "Batteries", 
                    "Inventory", "rainer0")},
                {"Epi Pen", new Item(new Vector3(2,0f,0), "Epi Pen", 
                    "Inventory", "rainer0")},
                {"Wrench", new Item(new Vector3(3,0f,0), "Wrench", 
                    "Inventory", "rainer0")},
                {"Bucket", new Item(new Vector3(0,0f,0), "Bucket", 
                    "Inventory", "carlos0")},
                {"Can Opener", new Item(new Vector3(1,0f,0), "Can Opener", 
                    "Inventory", "carlos0")},
                {"Leash", new Item(new Vector3(2,0f,0), "Leash", 
                    "Inventory", "carlos0")},
                {"Whistle", new Item(new Vector3(3,0f,0), "Whistle", 
                    "Inventory", "carlos0")},
                {"Bucket 2", new Item(new Vector3(0,0f,0), "Bucket 2", 
                    "Inventory", "angie0")},
                {"Radio", new Item(new Vector3(1,0f,0), "Radio", 
                    "Inventory", "angie0")},
                {"Blanket", new Item(new Vector3(2,0f,0), "Blanket", 
                    "Inventory", "angie0")},
                {"Knife", new Item(new Vector3(3,0f,0), "Knife", 
                    "Inventory", "angie0")},
                {"Bag", new Item(new Vector3(0,0f,0), "Bag", 
                    "Inventory", "annette0")},
                {"First Aid Kit", new Item(new Vector3(1,0f,0), "First Aid Kit", 
                    "Inventory", "annette0")},
                {"N95 Mask", new Item(new Vector3(2,0f,0), "N95 Mask", 
                    "Inventory", "annette0")},
                {"Fire Extinguisher", new Item(new Vector3(3,0f,0), "Fire Extinguisher", 
                    "Inventory", "annette0")},
                {"Wood Chips", new Item(new Vector3(0,0f,0), "Wood Chips", 
                    "Inventory", "safi0")},
                {"Canned Food", new Item(new Vector3(1,0f,0), "Canned Food", 
                    "Inventory", "safi0")},
                {"Gloves", new Item(new Vector3(2,0f,0), "Gloves", 
                    "Inventory", "safi0")},
                {"Playing Cards", new Item(new Vector3(3,0f,0), "Playing Cards", 
                    "Inventory", "safi0")},
                {"Water Bottle Clean", new Item(new Vector3(-7.5f,0.5f,0.5f), "Water Bottle Clean", 
                    "", "")},

            };
        }
        else itemList = new Dictionary<string, Item>
        {
            {"Sunscreen", new Item(new Vector3(6.5f,0.5f,0.5f), "Sunscreen", 
                    "PreQuakeHouse", "")},
            {"Dirty Water Bottle", new Item(new Vector3(-6.5f,0.5f,0.5f), "Dirty Water Bottle", 
                "PreQuakeHouse", "")},
            {"Book", new Item(new Vector3(-5.5f,0.5f,-7.5f), "Book", 
                "PreQuakeHouse", "")},
            {"Flashlight", new Item(new Vector3(3.5f,0.5f,3.5f), "Flashlight", 
                "PreQuakeHouse", "")},
            {"Bleach", new Item(new Vector3(0,0f,0), "Bleach", 
                "Inventory", "dem0")},
            {"Tent", new Item(new Vector3(1,0f,0), "Tent", 
                "Inventory", "dem0")},
            {"Dog Crate", new Item(new Vector3(2,0f,0), "Dog Crate", 
                "Inventory", "dem0")},
            {"Rubber Chicken", new Item(new Vector3(3,0f,0), "Rubber Chicken", 
                "Inventory", "dem0")},
            {"Toilet Paper", new Item(new Vector3(0,0,0), "Toilet Paper", 
                "Inventory", "rainer0")},
            {"Batteries", new Item(new Vector3(1,0f,0), "Batteries", 
                "Inventory", "rainer0")},
            {"Epi Pen", new Item(new Vector3(2,0f,0), "Epi Pen", 
                "Inventory", "rainer0")},
            {"Wrench", new Item(new Vector3(3,0f,0), "Wrench", 
                "Inventory", "rainer0")},
            {"Plywood", new Item(new Vector3(0,0f,0), "Plywood", 
                "Inventory", "carlos0")},
            {"Can Opener", new Item(new Vector3(1,0f,0), "Can Opener", 
                "Inventory", "carlos0")},
            {"Leash", new Item(new Vector3(2,0f,0), "Leash", 
                "Inventory", "carlos0")},
            {"Whistle", new Item(new Vector3(3,0f,0), "Whistle", 
                "Inventory", "carlos0")},
            {"Tarp", new Item(new Vector3(0,0f,0), "Tarp", 
                "Inventory", "angie0")},
            {"Radio", new Item(new Vector3(1,0f,0), "Radio", 
                "Inventory", "angie0")},
            {"Blanket", new Item(new Vector3(2,0f,0), "Blanket", 
                "Inventory", "angie0")},
            {"Knife", new Item(new Vector3(3,0f,0), "Knife", 
                "Inventory", "angie0")},
            {"Rope", new Item(new Vector3(0,0f,0), "Rope", 
                "Inventory", "annette0")},
            {"First Aid Kit", new Item(new Vector3(1,0f,0), "First Aid Kit", 
                "Inventory", "annette0")},
            {"N95 Mask", new Item(new Vector3(2,0f,0), "N95 Mask", 
                "Inventory", "annette0")},
            {"Fire Extinguisher", new Item(new Vector3(3,0f,0), "Fire Extinguisher", 
                "Inventory", "annette0")},
            {"Shovel", new Item(new Vector3(0,0f,0), "Shovel", 
                "Inventory", "safi0")},
            {"Canned Food", new Item(new Vector3(1,0f,0), "Canned Food", 
                "Inventory", "safi0")},
            {"Gloves", new Item(new Vector3(2,0f,0), "Gloves", 
                "Inventory", "safi0")},
            {"Playing Cards", new Item(new Vector3(3,0f,0), "Playing Cards", 
                "Inventory", "safi0")},
            {"Water Bottle Clean", new Item(new Vector3(-7.5f,0.5f,0.5f), "Water Bottle Clean", 
                "", "")},
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
                {"Sunscreen", new Item(new Vector3(6.5f,0.5f,0.5f), "Sunscreen", 
                    "PreQuakeHouse", "")},
                {"Bleach", new Item(new Vector3(-6.5f,0.5f,0.5f), "Bleach", 
                    "PreQuakeHouse", "")},
                {"Book", new Item(new Vector3(-5.5f,0.5f,-7.5f), "Book", 
                    "PreQuakeHouse", "")},
                {"Flashlight", new Item(new Vector3(3.5f,0.5f,3.5f), "Flashlight", 
                    "PreQuakeHouse", "")},
                {"Dirty Water Bottle", new Item(new Vector3(0,0f,0), "Dirty Water Bottle", 
                    "Inventory", "dem0")},
                {"Tent", new Item(new Vector3(1,0f,0), "Tent", 
                    "Inventory", "dem0")},
                {"Dog Crate", new Item(new Vector3(2,0f,0), "Dog Crate", 
                    "Inventory", "dem0")},
                {"Rubber Chicken", new Item(new Vector3(3,0f,0), "Rubber Chicken", 
                    "Inventory", "dem0")},
                {"Toilet Paper", new Item(new Vector3(0,0,0), "Toilet Paper", 
                    "Inventory", "rainer0")},
                {"Batteries", new Item(new Vector3(1,0f,0), "Batteries", 
                    "Inventory", "rainer0")},
                {"Epi Pen", new Item(new Vector3(2,0f,0), "Epi Pen", 
                    "Inventory", "rainer0")},
                {"Wrench", new Item(new Vector3(3,0f,0), "Wrench", 
                    "Inventory", "rainer0")},
                {"Bucket", new Item(new Vector3(0,0f,0), "Bucket", 
                    "Inventory", "carlos0")},
                {"Can Opener", new Item(new Vector3(1,0f,0), "Can Opener", 
                    "Inventory", "carlos0")},
                {"Leash", new Item(new Vector3(2,0f,0), "Leash", 
                    "Inventory", "carlos0")},
                {"Whistle", new Item(new Vector3(3,0f,0), "Whistle", 
                    "Inventory", "carlos0")},
                {"Bucket 2", new Item(new Vector3(0,0f,0), "Bucket 2", 
                    "Inventory", "angie0")},
                {"Radio", new Item(new Vector3(1,0f,0), "Radio", 
                    "Inventory", "angie0")},
                {"Blanket", new Item(new Vector3(2,0f,0), "Blanket", 
                    "Inventory", "angie0")},
                {"Knife", new Item(new Vector3(3,0f,0), "Knife", 
                    "Inventory", "angie0")},
                {"Bag", new Item(new Vector3(0,0f,0), "Bag", 
                    "Inventory", "annette0")},
                {"First Aid Kit", new Item(new Vector3(1,0f,0), "First Aid Kit", 
                    "Inventory", "annette0")},
                {"N95 Mask", new Item(new Vector3(2,0f,0), "N95 Mask", 
                    "Inventory", "annette0")},
                {"Fire Extinguisher", new Item(new Vector3(3,0f,0), "Fire Extinguisher", 
                    "Inventory", "annette0")},
                {"Wood Chips", new Item(new Vector3(0,0f,0), "Wood Chips", 
                    "Inventory", "safi0")},
                {"Canned Food", new Item(new Vector3(1,0f,0), "Canned Food", 
                    "Inventory", "safi0")},
                {"Gloves", new Item(new Vector3(2,0f,0), "Gloves", 
                    "Inventory", "safi0")},
                {"Playing Cards", new Item(new Vector3(3,0f,0), "Playing Cards", 
                    "Inventory", "safi0")},
                {"Water Bottle Clean", new Item(new Vector3(-7.5f,0.5f,0.5f), "Water Bottle Clean", 
                    "", "")},
            };
        }
        else 
        {
            itemList = new Dictionary<string, Item>
            {
                {"Sunscreen", new Item(new Vector3(6.5f,0.5f,0.5f), "Sunscreen", 
                    "PreQuakeHouse", "")},
                {"Dirty Water Bottle", new Item(new Vector3(-6.5f,0.5f,0.5f), "Dirty Water Bottle", 
                    "PreQuakeHouse", "")},
                {"Book", new Item(new Vector3(-5.5f,0.5f,-7.5f), "Book", 
                    "PreQuakeHouse", "")},
                {"Flashlight", new Item(new Vector3(3.5f,0.5f,3.5f), "Flashlight", 
                    "PreQuakeHouse", "")},
                {"Bleach", new Item(new Vector3(0,0f,0), "Bleach", 
                    "Inventory", "dem0")},
                {"Tent", new Item(new Vector3(1,0f,0), "Tent", 
                    "Inventory", "dem0")},
                {"Dog Crate", new Item(new Vector3(2,0f,0), "Dog Crate", 
                    "Inventory", "dem0")},
                {"Rubber Chicken", new Item(new Vector3(3,0f,0), "Rubber Chicken", 
                    "Inventory", "dem0")},
                {"Toilet Paper", new Item(new Vector3(0,0,0), "Toilet Paper", 
                    "Inventory", "rainer0")},
                {"Batteries", new Item(new Vector3(1,0f,0), "Batteries", 
                    "Inventory", "rainer0")},
                {"Epi Pen", new Item(new Vector3(2,0f,0), "Epi Pen", 
                    "Inventory", "rainer0")},
                {"Wrench", new Item(new Vector3(3,0f,0), "Wrench", 
                    "Inventory", "rainer0")},
                {"Plywood", new Item(new Vector3(0,0f,0), "Plywood", 
                    "Inventory", "carlos0")},
                {"Can Opener", new Item(new Vector3(1,0f,0), "Can Opener", 
                    "Inventory", "carlos0")},
                {"Leash", new Item(new Vector3(2,0f,0), "Leash", 
                    "Inventory", "carlos0")},
                {"Whistle", new Item(new Vector3(3,0f,0), "Whistle", 
                    "Inventory", "carlos0")},
                {"Tarp", new Item(new Vector3(0,0f,0), "Tarp", 
                    "Inventory", "angie0")},
                {"Radio", new Item(new Vector3(1,0f,0), "Radio", 
                    "Inventory", "angie0")},
                {"Blanket", new Item(new Vector3(2,0f,0), "Blanket", 
                    "Inventory", "angie0")},
                {"Knife", new Item(new Vector3(3,0f,0), "Knife", 
                    "Inventory", "angie0")},
                {"Rope", new Item(new Vector3(0,0f,0), "Rope", 
                    "Inventory", "annette0")},
                {"First Aid Kit", new Item(new Vector3(1,0f,0), "First Aid Kit", 
                    "Inventory", "annette0")},
                {"N95 Mask", new Item(new Vector3(2,0f,0), "N95 Mask", 
                    "Inventory", "annette0")},
                {"Fire Extinguisher", new Item(new Vector3(3,0f,0), "Fire Extinguisher", 
                    "Inventory", "annette0")},
                {"Shovel", new Item(new Vector3(0,0f,0), "Shovel", 
                    "Inventory", "safi0")},
                {"Canned Food", new Item(new Vector3(1,0f,0), "Canned Food", 
                    "Inventory", "safi0")},
                {"Gloves", new Item(new Vector3(2,0f,0), "Gloves", 
                    "Inventory", "safi0")},
                {"Playing Cards", new Item(new Vector3(3,0f,0), "Playing Cards", 
                    "Inventory", "safi0")},
                {"Water Bottle Clean", new Item(new Vector3(-7.5f,0.5f,0.5f), "Water Bottle Clean", 
                    "", "")},
            };
            
        }
    }
    
}