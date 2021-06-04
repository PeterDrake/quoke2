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
    
    // Start is called before the first frame update
    void Start()
    {
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
                // Place item in front of player
                Debug.Log("Slot found to be occupied and will drop");

                items[i].SetActive(true);
                items[i].transform.position = player.destination.transform.position + player.transform.forward;
                
                //updates item list accordingly
                GlobalItemList.UpdateItemList(items[i].name, SceneManager.GetActiveScene().name, items[i].transform.position);
                if (items[i] == true)
                {
                    Debug.Log("enabled an object");
                }
                // Remove item from inventory
                items[i] = null;
                slotContents[i].SetActive(false);

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
                
                GlobalItemList.UpdateItemList(items[i].name, SceneManager.GetActiveScene().name + " " + container.name, 
                    items[i].transform.position);

                // Remove item from inventory
                items[i] = null;
                slotContents[i].SetActive(false);
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
            GlobalItemList.UpdateItemList(item.name, "Inventory", new Vector3(i, 0, 0));
            
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
        if (container) {
            InteractWithStorageContainer(container.GetComponent<StorageContainer>());
        }
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