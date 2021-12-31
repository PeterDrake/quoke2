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

    public static bool isInApartment;

    static GlobalItemList()
    {
        isInApartment = GlobalControls.globalControlsProperties.Contains("apartmentCondition");
        initiateItemDictionary();
    }

    /** Updates itemList with the picked up item's position and scene. */
    public static void UpdateItemList(string name, string target, Vector3 position, string containerName)
    {
        name = name.Replace("(Clone)","").Trim();
        itemList[name] = new Item(position, name, target, containerName);
    }

    public static void Reset()
    {
        initiateItemDictionary();
    }

    public static void initiateItemDictionary() 
    {
        isInApartment = GlobalControls.globalControlsProperties.Contains("apartmentCondition");
        itemList = new Dictionary<string, Item>
            {
                {"Book", new Item(new Vector3(-5.5f,0.5f,-7.5f), "Book",
                    "PreQuakeHouse", "")},
                {"Flashlight", new Item(new Vector3(3.5f,0.5f,3.5f), "Flashlight",
                    "PreQuakeHouse", "")},
                {"Sunscreen", new Item(new Vector3(6.5f,0.5f,0.5f), "Sunscreen",
                    "PreQuakeHouse", "")},
                {"Tent", new Item(new Vector3(1,0f,0), "Tent",
                    "Inventory", "Dem")},
                {"Dog Crate", new Item(new Vector3(2,0f,0), "Dog Crate",
                    "Inventory", "Dem")},
                {"Rubber Chicken", new Item(new Vector3(3,0f,0), "Rubber Chicken",
                    "Inventory", "Dem")},
                {"Toilet Paper", new Item(new Vector3(0,0,0), "Toilet Paper",
                    "Inventory", "Rainer")},
                {"Batteries", new Item(new Vector3(1,0f,0), "Batteries",
                    "Inventory", "Rainer")},
                {"Epi Pen", new Item(new Vector3(2,0f,0), "Epi Pen",
                    "Inventory", "Rainer")},
                {"Wrench", new Item(new Vector3(3,0f,0), "Wrench",
                    "Inventory", "Rainer")},
                {"Can Opener", new Item(new Vector3(1,0f,0), "Can Opener",
                    "Inventory", "Carlos")},
                {"Leash", new Item(new Vector3(2,0f,0), "Leash",
                    "Inventory", "Carlos")},
                {"Whistle", new Item(new Vector3(3,0f,0), "Whistle",
                    "Inventory", "Carlos")},
                {"Radio", new Item(new Vector3(1,0f,0), "Radio",
                    "Inventory", "Angie")},
                {"Blanket", new Item(new Vector3(2,0f,0), "Blanket",
                    "Inventory", "Angie")},
                {"Knife", new Item(new Vector3(3,0f,0), "Knife",
                    "Inventory", "Angie")},
                {"First Aid Kit", new Item(new Vector3(1,0f,0), "First Aid Kit",
                    "Inventory", "Annette")},
                {"N95 Mask", new Item(new Vector3(2,0f,0), "N95 Mask",
                    "Inventory", "Annette")},
                {"Fire Extinguisher", new Item(new Vector3(3,0f,0), "Fire Extinguisher",
                    "Inventory", "Annette")},
                {"Shovel", new Item(new Vector3(0,0f,0), "Shovel",
                    "Inventory", "Safi")},
                {"Canned Food", new Item(new Vector3(1,0f,0), "Canned Food",
                    "Inventory", "Safi")},
                {"Gloves", new Item(new Vector3(2,0f,0), "Gloves",
                    "Inventory", "Safi")},
                {"Playing Cards", new Item(new Vector3(3,0f,0), "Playing Cards",
                    "Inventory", "Safi")},
                {"Water Bottle Clean", new Item(new Vector3(-7.5f,0.5f,0.5f), "Water Bottle Clean",
                    "", "")},

            };
        if (isInApartment)
        {
            itemList.Add(
                "Succulent", new Item(new Vector3(4.5f,0.5f,-8.5f), "Succulent",
                    "PreQuakeHouse", ""));
            itemList.Add(
                "Vintage Game Console", new Item(new Vector3(0f,0.5f,7.5f), "Vintage Game Console",
                    "PreQuakeHouse", ""));
            itemList.Add(
                "Bleach", new Item(new Vector3(-6.5f,0.5f,0.5f), "Bleach",
                    "PreQuakeHouse", ""));
            itemList.Add(
                "Bucket", new Item(new Vector3(0, 0f, 0), "Bucket",
          "Inventory", "Carlos"));
            itemList.Add(
                "Bucket 2", new Item(new Vector3(0, 0f, 0), "Bucket 2",
                    "Inventory", "Angie"));
            itemList.Add(
                "Bag", new Item(new Vector3(0, 0f, 0), "Bag",
                    "Inventory", "Annette"));
            itemList.Add(
                "Dirty Water Bottle", new Item(new Vector3(0,0f,0), "Dirty Water Bottle",
                "Inventory", "Dem"));
        }
        else
        {
            itemList.Add(
                "Succulent", new Item(new Vector3(6.5f,0.5f,-7.5f), "Succulent",
                "PreQuakeHouse", ""));
            itemList.Add(
                "Vintage Game Console", new Item(new Vector3(-2.5f,0.5f,7.5f), "Vintage Game Console",
                "PreQuakeHouse", ""));
            itemList.Add(
                "Dirty Water Bottle", new Item(new Vector3(-6.5f,0.5f,0.5f), "Dirty Water Bottle",
                "PreQuakeHouse", ""));
            itemList.Add(
                "Plywood", new Item(new Vector3(0, 0f, 0), "Plywood",
                "Inventory", "Carlos"));
            itemList.Add(
                "Tarp", new Item(new Vector3(0, 0f, 0), "Tarp",
                "Inventory", "Angie"));
            itemList.Add(
                "Rope", new Item(new Vector3(0, 0f, 0), "Rope",
                "Inventory", "Annette"));
            itemList.Add(
                "Bleach", new Item(new Vector3(0,0f,0), "Bleach",
                    "Inventory", "Dem"));
        }
    }
    
}