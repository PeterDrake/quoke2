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
    public Sprite unselectedSprite;
    public Sprite selectedSprite;
    public Sprite fullUnselectedSprite;
    public Sprite fullSelectedSprite;
    public GameObject[] slots;
    public int currentSlot;
    public bool[] fullSlot;
    public KeyCode[] validInputs;

    // Start is called before the first frame update
    void Start()
    {
        
        fullSlot = new bool[5];

        // This would only allow for 10 inventory slots max
        validInputs = new []{KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0};
        
        //Load sprites
        selectedSprite = Resources.Load<Sprite>("SelectedSlot 1");
        unselectedSprite = Resources.Load<Sprite>("UnselectedSlot 1");
        fullSelectedSprite = Resources.Load<Sprite>("FullSelectedSlot 1");
        fullUnselectedSprite = Resources.Load<Sprite>("FullUnselectedSlot 1");
        
        //locate GameObjecs with "slots" tag.
        slots = GameObject.FindGameObjectsWithTag("Slots");
        slots[0].GetComponent<Image>().sprite = selectedSprite;

        currentSlot = 0;

        //Check if any slots are full. If they are full, change sprite
        for (int i = 0; i < slots.Length; i++)
        {
            //if slot is full
            if (fullSlot[i])
            {
                // if currentSlot is selected, then change currentSlot to fullSelectedSprite. Otherwise, change fullUnselectedSprite
                if (currentSlot == i)
                {
                    slots[i].GetComponent<Image>().sprite = fullSelectedSprite;
                }
                else
                {
                    slots[i].GetComponent<Image>().sprite = fullUnselectedSprite;
                }
                
            }
            //if slot is empty
            else
            {
                // if currentSlot is selected, then change currentSlot to selectedSprite. Otherwise, change unselectedSprite
                if (currentSlot == i)
                {
                    slots[i].GetComponent<Image>().sprite = selectedSprite;
                }
                else
                {
                    slots[i].GetComponent<Image>().sprite = unselectedSprite;
                }
            }
        }

    }

    void changeSlot(int slotNumber)
    {
        if (slotNumber < 0 || slotNumber >= slots.Length)
        {
            return;
        }

        if (fullSlot[slotNumber])
        {
            slots[slotNumber].GetComponent<Image>().sprite = fullSelectedSprite;
        }
        else
        {
            slots[slotNumber].GetComponent<Image>().sprite = selectedSprite;
        }
            
        if (currentSlot != slotNumber)
        {
            if (fullSlot[currentSlot])
            {
                slots[currentSlot].GetComponent<Image>().sprite = fullUnselectedSprite;
            }
            else
            {
                slots[currentSlot].GetComponent<Image>().sprite = unselectedSprite;
            }
                
            currentSlot = slotNumber;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If spacebar is pressed, fill/empty selected slot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (fullSlot[currentSlot])
            {
                slots[currentSlot].GetComponent<Image>().sprite = selectedSprite;
                fullSlot[currentSlot] = false;
            }
            else
            {
                slots[currentSlot].GetComponent<Image>().sprite = fullSelectedSprite;
                fullSlot[currentSlot] = true;
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (Input.GetKey(validInputs[i]))
            {
                changeSlot(i);
            }
        }

    }

}