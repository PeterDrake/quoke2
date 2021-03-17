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
    public Sprite itemImage;

    private Sprite unselectedSprite;
    private Sprite selectedSprite;
    private GameObject[] slots;
    private GameObject[] items;
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

        //locate Gameobjects with "InventoryItems" tag and sets all of them unactive in scene
        items = GameObject.FindGameObjectsWithTag("InventoryItems");
        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }

        //sets first selected slot to be 0
        currentSlot = 0;
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

    // removes item at specific slotNumber by deactivating it from scene
    void RemoveItemFromInventory(int slotNumber)
    {
        if (items[slotNumber].activeSelf)
        {
            items[slotNumber].SetActive(false);        
        }
    }

    // finds next empty spot and if there is changes sprite to that item's sprite
    void AddItemToInventory(Sprite itemSprite)
    {
        int nextSpot = FindNextEmptySpot();
        if (nextSpot >= 0)
        {
            items[nextSpot].SetActive(true);
            items[nextSpot].GetComponent<Image>().sprite = itemSprite;
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

        //if B key is pressed, add a item to the inventory
        if (Input.GetKeyDown(KeyCode.B))
        {
            AddItemToInventory(itemImage);
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