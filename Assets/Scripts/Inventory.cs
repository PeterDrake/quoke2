using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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

    private LatrineStorage latrineStorage;
    // Start is called before the first frame update
    //Awake not start because Inventory must load first
    void Awake()
    {
        if (SceneManager.GetActiveScene().name.Equals("Yard"))
        {
            latrineStorage = GameObject.Find("Latrine Hole").GetComponent<LatrineStorage>();
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
        dropObstructionLayers = LayerMask.GetMask("Wall", "NPC", "Table", "Exit", "StorageContainer");
        storageContainerLayers = LayerMask.GetMask("StorageContainer");
        latrineContainerLayers = LayerMask.GetMask("LatrineContainer");
        waterLayer = LayerMask.GetMask("Water");
    }

    private void Start()
    {
        player = referenceManager.player.GetComponent<PlayerMover>();
    }

    public void setAvailableSlots(int numSlots)
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
    }

    private void DropSelectedItem()
    {
        // Sends a Raycast out one space in front of the player, to check if there is anything in the way before item placement
        if (!player.ObjectAhead(dropObstructionLayers))
        {
            int i = selectedSlotNumber; // Just to make the later expressions less hairy
            if (SlotIsOccupied(i))
            {
                if (items[i].name.Equals("Shovel(Clone)") && player.ObjectAhead(latrineContainerLayers) && 
                    latrineStorage.CheckAllLatrineItems())
                {
                    latrineStorage.timesShoveled++;
                    Debug.Log("Time shoveled: " + latrineStorage.timesShoveled);
                    if (latrineStorage.ShovelingComplete())
                    {
                        latrineStorage.shovelingDone = true;
                    }else if (latrineStorage.timesShoveled > 4)
                    {
                        Debug.Log("Resetting shoveling");
                        latrineStorage.timesShoveled = 0;
                        latrineStorage.shovelingDone = false;
                    }
                }
                else if (items[i].name.Equals("Chlorine Tablet(Clone)") && player.ObjectAhead(waterLayer))
                {
                    Debug.Log("Dropping chlorine tablet to water bottle");
                    GlobalItemList.UpdateItemList("Chlorine Tablet", "", new Vector3(0,0,0), "");
                    items[i] = null;
                    slotContents[i].SetActive(false);
                    
                    
                    
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
            }
        }
    }

    void removeLatrineItem(int i)
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
            i = selectedSlotNumber; // Just to make the later expressions less hairy
            if (SlotIsOccupied(i) && !latrineStorage.contents)
            {
                // Place item in the container if the item is a latrine task item
                if (items[i].name.Equals("Shovel(Clone)") && player.ObjectAhead(latrineContainerLayers)) x = 1; 
                if (items[i].name.Equals("Plywood(Clone)") && latrineStorage.shovelingDone) x = 2;
                if (items[i].name.Equals("Rope(Clone)") && latrineStorage.plywoodDone) x = 3;
                if (items[i].name.Equals("Tarp(Clone)") && latrineStorage.ropeDone) x = 4;

                switch (x)
                {
                    case 0:
                        Debug.Log("You cant do that try again");
                        break;
                    case 1:
                        //When you try to dig
                        break;
                    case 2:
                        removeLatrineItem(i);
                        latrineStorage.plywoodDone = true;
                        Debug.Log("Plywood Complete");
                        break;
                    case 3:
                        removeLatrineItem(i);
                        latrineStorage.ropeDone = true;
                        Debug.Log("Rope Complete");
                        break;
                    case 4:
                        removeLatrineItem(i);
                        latrineStorage.tarpDone = true;
                        Debug.Log("Tarp Complete");
                        latrineStorage.LatrineComplete();
                        break;
                }
            }
        }
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
        GameObject container = player.ObjectAhead(storageContainerLayers);
        GameObject latrine = player.ObjectAhead(latrineContainerLayers);
        if (container) {
            InteractWithStorageContainer(container.GetComponent<StorageContainer>());
        }
        else if (latrine)
        {
            InteractWithLatrine();
            Debug.Log("Interacting with latrine");
        }
        //check if the player is in front of the latrine
        
        
        
        DropSelectedItem();
    }

    
    /** Only used by ItemLoader.cs */
    public void PickUpAtSlot(int slot, GameObject item)
    {
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