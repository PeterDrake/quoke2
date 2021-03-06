﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Inventory : MonoBehaviour
{
    public int selectedSlotNumber;
    public PlayerMover player;
    public Sprite unselectedSlotSprite;
    public Sprite selectedSlotSprite;

    // These are UI Images showing sprites (either unselectedSlotSprite or selectedSlotSprite).
    public GameObject[] slotFrames;

    // These are UI Images showing sprites for occupied slots. They're inactive for unoccupied slots.
    public GameObject[] slotContents;

    // These are the actual 3D GameObjects that the player has picked up.
    public GameObject[] items;
    
    private int dropObstructionLayers;  // You cannot drop an item if something in one of theses layers (e.g., a wall) is in front of you.
    private int storageContainerLayers;
    private int latrineContainerLayers;
    private int waterLayer;

    private ReferenceManager referenceManager;
    private Text tooltipText;
    private Meters meters;

    private LatrineStorage latrineStorage;
    private TwoBucketScript twoBucket;
    // Start is called before the first frame update
    //Awake not start because Inventory must load first
    void Awake()
    {
        if (SceneManager.GetActiveScene().name.Equals("Yard"))
        {
            latrineStorage = GameObject.Find("Latrine Hole").GetComponent<LatrineStorage>();
        }
        else if (SceneManager.GetActiveScene().name.Equals("Street"))
        {
            twoBucket = GameObject.Find("Two Bucket Spot").GetComponent<TwoBucketScript>();
        }
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        
        // Set initial state of all the arrays
        foreach (GameObject frame in slotFrames)
        {
            frame.GetComponent<Image>().sprite = unselectedSlotSprite;
        }
        foreach (GameObject item in slotContents)
        {
            item.SetActive(false);
        }
        
        items = new GameObject[slotFrames.Length];
        
        // Select the first slot
        selectedSlotNumber = 0;
        slotFrames[selectedSlotNumber].GetComponent<Image>().sprite = selectedSlotSprite;
        // Find layers for various interactions
        dropObstructionLayers = LayerMask.GetMask("Wall", "NPC", "Table", "Exit", "StorageContainer", "LatrineContainer");
        storageContainerLayers = LayerMask.GetMask("StorageContainer");
        latrineContainerLayers = LayerMask.GetMask("LatrineContainer");
        waterLayer = LayerMask.GetMask("Water");
    }

    private void Start()
    {
        player = referenceManager.player.GetComponent<PlayerMover>();
        if (GlobalControls.TooltipsEnabled)
        {
            foreach (Image image in referenceManager.tooltipCanvas.GetComponentsInChildren<Image>(true))
            {
                if (image.gameObject.name.Equals("Tooltip"))
                {
                    tooltipText = image.gameObject.GetComponentInChildren<Text>(true);
                }
            }
        }

        SelectSlotNumber(0);
    }

    private void OnEnable()
    {
        if(items.Length > 0) SelectSlotNumber(0);
    }

    public void SetAvailableSlots(int numSlots)
    {
        GameObject[] tempSlotFrames = new GameObject[numSlots];
        GameObject[] tempItems = new GameObject[numSlots];
        GameObject[] tempSlotContents = new GameObject[numSlots];
        
        for (int i = 0; i < numSlots; i++)
        {
            tempItems[i] = items[i];
            tempSlotContents[i] = slotContents[i];
            tempSlotFrames[i] = slotFrames[i];
        }

        items = tempItems;
        slotContents = tempSlotContents;
        slotFrames = tempSlotFrames;

        if (numSlots < 5)
        {
            for (int i = 4; i >= numSlots; i--)
            {
                GameObject.Find("Frame " + i).SetActive(false);
            }
        }
        
        
        // Select the first slot
        selectedSlotNumber = 0;
        slotFrames[selectedSlotNumber].GetComponent<Image>().sprite = selectedSlotSprite;
        SelectSlotNumber(0);
    }
    
    
    public void SelectSlotNumber(int slotNumber)
    {
        if (slotNumber < 0 || slotNumber >= slotFrames.Length)
        {
            return;
        }

        slotFrames[slotNumber].GetComponent<Image>().sprite = selectedSlotSprite;
        if (selectedSlotNumber != slotNumber)
        {
            slotFrames[selectedSlotNumber].GetComponent<Image>().sprite = unselectedSlotSprite;
            selectedSlotNumber = slotNumber;
        }
        if (GlobalControls.TooltipsEnabled && slotContents[selectedSlotNumber].activeSelf)
        {
            if(!referenceManager.tooltipCanvas.GetComponentInChildren<Image>(true).gameObject.activeSelf)
                referenceManager.tooltipCanvas.GetComponentInChildren<Image>(true).gameObject.SetActive(true);
            referenceManager.tooltipCanvas.GetComponentInChildren<Text>(true).text = items[selectedSlotNumber].GetComponent<Comment>().notes;
        }
        else if(referenceManager.tooltipCanvas.GetComponentInChildren<Image>(true).gameObject.activeSelf) 
            referenceManager.tooltipCanvas.GetComponentInChildren<Image>(true).gameObject.SetActive(false);
    }

    private void DropSelectedItem()
    {
        // Sends a Raycast out one space in front of the player, to check if there is anything in the way before item placement
        if (!player.ObjectAhead(dropObstructionLayers))
        {
            int i = selectedSlotNumber; // Just to make the later expressions less hairy
            if (SlotIsOccupied(i))
            {
                if (items[i].name.Equals("Chlorine Tablet(Clone)") && player.ObjectAhead(waterLayer))
                {
                    Debug.Log("Dropping chlorine tablet to water bottle");
                    GlobalItemList.UpdateItemList("Chlorine Tablet", "", new Vector3(0,0,0), "");
                    items[i] = null;
                    slotContents[i].SetActive(false);
                    meters = referenceManager.metersCanvas.GetComponent<Meters>();
                    meters.MarkTaskAsDone("water");
                    
                    GlobalItemList.UpdateItemList("Water Bottle", "", new Vector3(0,0,0), "");
                    GameObject.Find("Water Bottle(Clone)").SetActive(false);
                    GlobalItemList.UpdateItemList("Water Bottle Clean", SceneManager.GetActiveScene().name, 
                        player.destination.transform.position + player.transform.forward, "");
                    
                    GameObject prefab = (GameObject) Resources.Load("Water Bottle Clean", typeof(GameObject));
                    GameObject waterBottleClean = Instantiate(prefab,player.destination.transform.position + player.transform.forward , Quaternion.identity);

                }
                else
                {
                    // Place item in front of player
                    items[i].SetActive(true);
                    items[i].transform.position = player.destination.transform.position + player.transform.forward;
                
                    //updates item list accordingly
                    GlobalItemList.UpdateItemList(items[i].name, SceneManager.GetActiveScene().name, items[i].transform.position, "");
                    // Remove item from inventory
                    items[i] = null;
                    slotContents[i].SetActive(false);
                    //turn off tooltip
                    if (tooltipText.gameObject.activeSelf)
                        tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
                }
                
            }
        }
    }

    /// <summary>
    /// Takes the item in container (if there is one) or puts the selected item into container.
    /// </summary>
    void InteractWithStorageContainer(StorageContainer container)
    {
        int i = FirstEmptySlot();
        if (i >= 0 && !SlotIsOccupied(i) && container.contents)
        {
            GameObject item = container.RemoveItem();
            item.GetComponent<Collectible>().inStorageContainer = false;
            PickUp(item);
        }
        else
        {
            i = selectedSlotNumber; // Just to make the later expressions less hairy
            if (SlotIsOccupied(i) && !container.contents)
            {
                // Place item in the container
                container.contents = items[i];
                items[i].SetActive(true);
                Transform t = player.transform;
                items[i].transform.position = player.destination.transform.position + player.transform.forward + Vector3.up;
                items[i].GetComponent<Collectible>().inStorageContainer = true;
                
                GlobalItemList.UpdateItemList(items[i].name, SceneManager.GetActiveScene().name,
                    items[i].transform.position, container.name);

                // Remove item from inventory
                items[i] = null;
                slotContents[i].SetActive(false);
                if (tooltipText.gameObject.activeSelf)
                    tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
            }
        }
    }

    void RemoveLatrineItem(int i)
    {
        latrineStorage.contents = items[i];
        items[i].SetActive(true);
        items[i].transform.position = player.destination.transform.position + player.transform.forward + Vector3.up;
        items[i].GetComponent<Collectible>().inLatrine = true;
                    
        GlobalItemList.UpdateItemList(items[i].name, "",
            new Vector3(0,0,0), "");
        GameObject.Find(items[i].name).SetActive(false);
        // Remove item from inventory
        items[i] = null;
        slotContents[i].SetActive(false);
        latrineStorage.contents = null;
        if (tooltipText.gameObject.activeSelf)
            tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
    }
    
    void InteractWithLatrine()
    {
        int x = 0;
        int i = FirstEmptySlot();
        if (i >= 0 && !SlotIsOccupied(i) && latrineStorage.contents)
        {
            GameObject item = latrineStorage.RemoveLatrineItem();
            item.GetComponent<Collectible>().inStorageContainer = false;
            PickUp(item);
        }
        else
        {
            if (items[selectedSlotNumber].name.Equals("Shovel(Clone)") && player.ObjectAhead(latrineContainerLayers))
            {
                latrineStorage.timesShoveled++;
                GlobalControls.TimesShoveled++;
                Debug.Log("Time shoveled: " + latrineStorage.timesShoveled);
                if (latrineStorage.ShovelingComplete())
                {
                    GlobalControls.PoopTaskProgress[0] = true;
                    latrineStorage.shovelingDone = true;
                }else if (latrineStorage.timesShoveled > 4)
                {
                    Debug.Log("Resetting shoveling");
                    latrineStorage.timesShoveled = 0;
                    GlobalControls.TimesShoveled = 0;
                    latrineStorage.shovelingDone = false;
                }
            }
            i = selectedSlotNumber; // Just to make the later expressions less hairy
            if (SlotIsOccupied(i) && !latrineStorage.contents)
            {
                // Place item in the container if the item is a latrine task item
                if (items[i].name.Equals("Shovel(Clone)") && player.ObjectAhead(latrineContainerLayers)) x = 1; 
                if (items[i].name.Equals("Plywood(Clone)") && latrineStorage.shovelingDone) x = 2;
                if (items[i].name.Equals("Rope(Clone)") && latrineStorage.plywoodDone) x = 3;
                if (items[i].name.Equals("Tarp(Clone)") && latrineStorage.ropeDone) x = 4;
                if (items[i].name.Equals("Toilet Paper(Clone)") && latrineStorage.tarpDone) x = 5;

                switch (x)
                {
                    case 0:
                        Debug.Log("You cant do that try again");
                        break;
                    case 1:
                        //When you try to dig
                        break;
                    case 2:
                        RemoveLatrineItem(i);
                        latrineStorage.plywoodDone = true;
                        GlobalControls.PoopTaskProgress[1] = true;
                        Debug.Log("Plywood Complete");
                        break;
                    case 3:
                        RemoveLatrineItem(i);
                        latrineStorage.ropeDone = true;
                        GlobalControls.PoopTaskProgress[2] = true;
                        Debug.Log("Rope Complete");
                        break;
                    case 4:
                        RemoveLatrineItem(i);
                        latrineStorage.tarpDone = true;
                        GlobalControls.PoopTaskProgress[3] = true;
                        Debug.Log("Tarp Complete");
                        break;
                    case 5:
                        RemoveLatrineItem(i);
                        latrineStorage.toiletPaperDone = true;
                        GlobalControls.PoopTaskProgress[4] = true;
                        Debug.Log("Toilet Paper Complete");
                        latrineStorage.LatrineComplete();
                        break;
                }
                latrineStorage.UpdateVisuals();
            }
        }
    }

    void InteractWithTwoBucket()
    {
        int x = 0;
        int i = FirstEmptySlot();
        if (i >= 0 && !SlotIsOccupied(i) && twoBucket.contents)
        {
            GameObject item = twoBucket.RemoveBucketItem();
            item.GetComponent<Collectible>().inStorageContainer = false;
            PickUp(item);
        }
        else
        {
            i = selectedSlotNumber; // Just to make the later expressions less hairy
            if (SlotIsOccupied(i) && !twoBucket.contents && player.ObjectAhead(latrineContainerLayers))
            {
                // Place item in the container if the item is a latrine task item
                if (items[i].name.Equals("Bucket(Clone)")) x = 1; 
                if (items[i].name.Equals("Bucket 2(Clone)")) x = 2;
                if (items[i].name.Equals("Bag(Clone)") && twoBucket.bucketTwoDone && twoBucket.bucketDone) x = 3;
                if (items[i].name.Equals("Toilet Paper(Clone)") && twoBucket.bagDone) x = 4;
                if (items[i].name.Equals("Wood Chips(Clone)") && twoBucket.toiletPaperDone) x = 5;

                switch (x)
                {
                    case 0:
                        Debug.Log("You cant do that try again");
                        break;
                    case 1:
                        RemoveBucketItem(i);
                        twoBucket.bucketDone = true;
                        GlobalControls.PoopTaskProgress[0] = true;
                        Debug.Log("Bucket Complete");
                        break;
                    case 2:
                        RemoveBucketItem(i);
                        twoBucket.bucketTwoDone = true;
                        GlobalControls.PoopTaskProgress[1] = true;
                        Debug.Log("Bucket two Complete");
                        break;
                    case 3:
                        RemoveBucketItem(i);
                        twoBucket.bagDone = true;
                        GlobalControls.PoopTaskProgress[2] = true;
                        Debug.Log("Bag Complete");
                        break;
                    case 4:
                        RemoveBucketItem(i);
                        twoBucket.toiletPaperDone = true;
                        GlobalControls.PoopTaskProgress[3] = true;
                        Debug.Log("Toilet Paper Complete");
                        break;
                    case 5:
                        RemoveBucketItem(i);
                        twoBucket.woodChipsDone = true;
                        GlobalControls.PoopTaskProgress[4] = true;
                        Debug.Log("Wood Chips Complete");
                        twoBucket.TwoBucketComplete();
                        break;
                        
                }
                twoBucket.UpdateVisuals();
            }
        }
    }
    
    void RemoveBucketItem(int i)
    {
        twoBucket.contents = items[i];
        items[i].SetActive(true);
        items[i].transform.position = player.destination.transform.position + player.transform.forward + Vector3.up;
        items[i].GetComponent<Collectible>().inLatrine = true;
                    
        GlobalItemList.UpdateItemList(items[i].name, "",
            new Vector3(0,0,0), "");
        GameObject.Find(items[i].name).SetActive(false);
        // Remove item from inventory
        items[i] = null;
        slotContents[i].SetActive(false);
        twoBucket.contents = null;
        if (tooltipText.gameObject.activeSelf)
            tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
    }
    
    private bool SlotIsOccupied(int i)
    {
        return slotContents[i].activeSelf;
    }
    
    public void PickUp(GameObject item)
    {
        int i = FirstEmptySlot();
        if (i >= 0)
        {
            // Display the sprite for this item
            slotContents[i].SetActive(true);
            slotContents[i].GetComponent<Image>().sprite = item.GetComponent<Collectible>().sprite;
            // Add item to the items array
            items[i] = item;

            //updates item list to add item to list
            GlobalItemList.UpdateItemList(item.name, "Inventory", new Vector3(i, 0, 0), "Player");
            
            // Remove item from the world
            item.SetActive(false);
        }
        //reselect slot to current slot number to update tooltip if necessary
        SelectSlotNumber(selectedSlotNumber);
    }

    /// <summary>
    /// Returns the index of the first empty slot, or -1 if all slots are full.
    /// <returns></returns>
    private int FirstEmptySlot()
    {
        for (int i = 0; i < slotFrames.Length; i++)
        {
            if (!slotContents[i].activeSelf)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// If in front of a container, take the item in the container (if there is one) or drop the selected item.
    /// Otherwise drop the selected item unless obstructed.
    /// </summary>
    public void PickUpOrDrop()
    {
        if (!SceneManager.GetActiveScene().name.Equals("GameEnd"))
        {
            GameObject container = player.ObjectAhead(storageContainerLayers);
            GameObject latrine = player.ObjectAhead(latrineContainerLayers);
            if (container)
            {
                InteractWithStorageContainer(container.GetComponent<StorageContainer>());
            }
            else if (latrine && !GlobalControls.ApartmentCondition)
            {
                InteractWithLatrine();
            }
            else if (GlobalControls.ApartmentCondition && latrine)
            {
                InteractWithTwoBucket();
            }
            //check if the player is in front of the latrine



            DropSelectedItem();
        }
    }

    
    /** Only used by ItemLoader.cs */
    public void PickUpAtSlot(int slot, GameObject item)
    {
        // Debug.Log(slotContents.Length + " slotcontents length");
        int i = slot;
        if (i >= 0)
        {
            // Display the sprite for this item
            slotContents[i].SetActive(true);
            slotContents[i].GetComponent<Image>().sprite = item.GetComponent<Collectible>().sprite;
            // Add item to the items array
            items[i] = item;

            // Remove item from the world
            item.SetActive(false);
        }
        
    }
    
    
    
}