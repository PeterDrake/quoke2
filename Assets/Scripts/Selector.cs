using System.Collections;
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
    public GameObject[] inventory;

    private Sprite unselectedSprite;
    private Sprite selectedSprite;
    private GameObject[] slots;
    private GameObject[] items;
    private GameObject player;
    private KeyCode[] validInputs;

    // Start is called before the first frame update
    void Start()
    {
        // This would only allow for 10 inventory slots max
        validInputs = new []{KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0};
        
        //Load sprites
        selectedSprite = Resources.Load<Sprite>("SelectedSlot 1");
        unselectedSprite = Resources.Load<Sprite>("UnselectedSlot 1");
        
        //locate GameObjecs with "slots" tag.
        slots = GameObject.FindGameObjectsWithTag("Slots");
        slots[0].GetComponent<Image>().sprite = selectedSprite;

        //locate Gameobjects with "InventoryItems" tag 
        items = GameObject.FindGameObjectsWithTag("SlotItems");

        //starts the game with an empty inventory
        inventory = new GameObject[slots.Length];
        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }

        //sets first selected slot to be 0
        currentSlot = 0;

        player = GameObject.FindWithTag("Player");
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

    // removes item at specific slotNumber by deactivating its image from 
    void RemoveItemFromInventory(int slotNumber)
    {
        if (items[slotNumber].activeSelf)
        {
            inventory[slotNumber].SetActive(true);
            inventory[slotNumber].transform.position = player.transform.position + (player.transform.forward * 2);
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
            // collectable.SetActive(false);    // need this to only pick up object once
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
        //If spacebar is pressed, empty selected slot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RemoveItemFromInventory(currentSlot);
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (Input.GetKey(validInputs[i]))
            {
                SelectSlotNumber(i);
            }
        }

    }

}