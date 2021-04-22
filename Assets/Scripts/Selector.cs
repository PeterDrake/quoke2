﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Selector : MonoBehaviour
{
    public int currentSlot;
    
    // inventory holds the actual gameObjects which represent items in the world, that can be picked up and set down
    public GameObject[] inventory;

    private Sprite unselectedSprite;
    private Sprite selectedSprite;
    
    // The arrays for slots and items are public, and the objects must be manually associated in the Unity editor.
    // Select the Selector object and look under the script component to see the arrays.
    // Set the size of both arrays to the number of slots, then select the appropriate GameObjects in the right order.
    public GameObject[] slots;
    // items represents an array of empty game objects, one attached to each slot.
    // An item gameObject is set to active if that slot is full, and inactive if it is empty.
    // The inventory sprite for the inventory item is also attached to this gameObject so it displays in the slot.
    public GameObject[] items;
    private GameObject player;

    private int invalidItemSpaces;
    
    // This would only allow for 10 inventory slots max
    // If more than 10 slots are needed, add the KeyCodes you want associated with those slots to validInputs here
    private readonly KeyCode[] validInputs = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0};
    
    

    // Start is called before the first frame update
    void Start()
    {

        //Load sprites
        selectedSprite = Resources.Load<Sprite>("SelectedSlot 1");
        unselectedSprite = Resources.Load<Sprite>("UnselectedSlot 1");
        
        slots[0].GetComponent<Image>().sprite = selectedSprite;

        //starts the game with an empty inventory
        inventory = new GameObject[slots.Length];
        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }

        //sets first selected slot to be 0
        currentSlot = 0;

        player = GameObject.FindWithTag("Player");

        invalidItemSpaces = LayerMask.GetMask("Wall", "NPC", "Table", "StorageContainer");
    }

    // changes slot background of specific slotNumber to selected sprite 
    void SelectSlotNumber(int slotNumber)
    {
        if (slotNumber < 0 || slotNumber >= slots.Length)
        {
            return;
        }

        slots[slotNumber].GetComponent<Image>().sprite = selectedSprite;

            
        if (currentSlot != slotNumber)
        {
            slots[currentSlot].GetComponent<Image>().sprite = unselectedSprite;
            currentSlot = slotNumber;
        }
    }

    // removes item at specific slotNumber and sets item one grid unit in front of the player
    void RemoveItemFromInventory(int slotNumber)
    {
        // Sends a Raycast out one space in front of the player, to check if there is anything in the way before item placement
        if (!Physics.Raycast(player.transform.position, player.transform.forward, 1f, invalidItemSpaces))
        {
            if (items[slotNumber].activeSelf)
            {
                inventory[slotNumber].SetActive(true);
                inventory[slotNumber].transform.position = player.transform.position + (player.transform.forward);
                inventory[slotNumber] = null;
                items[slotNumber].SetActive(false);  
            }
        }
    }

    // Handles placing an item in a storage container or taking an item out of a storage container
    void InteractWithStorageContainer(int slotNumber, RaycastHit storageContainerCheck)
    {
        StorageContainer container = storageContainerCheck.collider.gameObject.GetComponent<StorageContainer>();
        if (container.isItemStored() && !items[slotNumber].activeSelf)
        {
            GameObject item = container.removeItem();
            item.GetComponent<Collectable>().TakeOutOfStorageContainer();
            AddItemToInventory(item);
        }
        else if (container.storeItem(inventory[slotNumber]))
        {
            inventory[slotNumber].SetActive(true);
            inventory[slotNumber].transform.position = player.transform.position + (player.transform.forward) + new Vector3(0f, 1f, 0f);
            inventory[slotNumber].GetComponent<Collectable>().PutInStorageContainer();
            inventory[slotNumber] = null;
            items[slotNumber].SetActive(false);
        }
    }

    // finds next empty spot and if there is changes sprite to that item's sprite
    public void AddItemToInventory(GameObject collectable)
    {
        int nextSpot = FindNextEmptySpot();
        if (nextSpot >= 0)
        {
            items[nextSpot].SetActive(true);
            items[nextSpot].GetComponent<Image>().sprite = collectable.GetComponent<Collectable>().itemSprite;
            inventory[nextSpot] = collectable;
            collectable.SetActive(false);
        }

    }

    //goes through all the slots to find the first deactivated item and returns that slot number
    int FindNextEmptySpot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (!items[i].activeSelf)
            {
                return i;
            }
        }
        return -1;
    }
    // Update is called once per frame
    void Update()
    {
        //If spacebar is pressed, run RemoveItemFromInventory on currently selected slot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit storageContainerCheck;
            if (Physics.Raycast(player.transform.position, player.transform.forward, out storageContainerCheck, 1f, LayerMask.GetMask("StorageContainer")))
            {
                InteractWithStorageContainer(currentSlot, storageContainerCheck);
            }
            RemoveItemFromInventory(currentSlot);
        }
        
        // If the user presses a key that is one of the valid inputs for slot selection, select that slot
        for (int i = 0; i < slots.Length; i++)
        {
            if (Input.GetKey(validInputs[i]))
            {
                SelectSlotNumber(i);
            }
        }

    }

}