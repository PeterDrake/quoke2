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

    // Start is called before the first frame update
    void Start()
    {
        //WHY?!?!??!?!??!! WHY DOES IT NEED TO BE INITIALIZED HERE?!?!?!
        fullSlot = new bool[5];
        
        selectedSprite = Resources.Load<Sprite>("SelectedSlot 1");
        unselectedSprite = Resources.Load<Sprite>("UnselectedSlot 1");
        fullSelectedSprite = Resources.Load<Sprite>("FullSelectedSlot 1");
        fullUnselectedSprite = Resources.Load<Sprite>("FullUnselectedSlot 1");
        
        slots = GameObject.FindGameObjectsWithTag("Slots");
        slots[0].GetComponent<Image>().sprite = selectedSprite;
        
        //fullSlot[0] = fullSlot[1] = true;

        currentSlot = 0;

        for (int i = 0; i < 5; i++)
        {
            if (fullSlot[i])
            {
                if (currentSlot == i)
                {
                    slots[i].GetComponent<Image>().sprite = fullSelectedSprite;
                }
                else
                {
                    slots[i].GetComponent<Image>().sprite = fullUnselectedSprite;
                }
                
            }
            else
            {
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

    // Update is called once per frame
    void Update()
    {
        // If user presses 1, select this slot
        // Else if user presses 2 - n, select that other slot
        if(Input.GetKey(KeyCode.Alpha1))
        {
            if (fullSlot[0])
            {
                slots[0].GetComponent<Image>().sprite = fullSelectedSprite;
            }
            else
            {
                slots[0].GetComponent<Image>().sprite = selectedSprite;
            }
            
            if (currentSlot != 0)
            {
                if (fullSlot[currentSlot])
                {
                    slots[currentSlot].GetComponent<Image>().sprite = fullUnselectedSprite;
                }
                else
                {
                    slots[currentSlot].GetComponent<Image>().sprite = unselectedSprite;
                }
                
                currentSlot = 0;
            }
        }
        if(Input.GetKey(KeyCode.Alpha2))
        {
            if (fullSlot[1])
            {
                slots[1].GetComponent<Image>().sprite = fullSelectedSprite;
            }
            else
            {
                slots[1].GetComponent<Image>().sprite = selectedSprite;
            }
            
            if (currentSlot != 1)
            {
                if (fullSlot[currentSlot])
                {
                    slots[currentSlot].GetComponent<Image>().sprite = fullUnselectedSprite;
                }
                else
                {
                    slots[currentSlot].GetComponent<Image>().sprite = unselectedSprite;
                }
                
                currentSlot = 1;
            }
        }
        if(Input.GetKey(KeyCode.Alpha3))
        {
            if (fullSlot[2])
            {
                slots[2].GetComponent<Image>().sprite = fullSelectedSprite;
            }
            else
            {
                slots[2].GetComponent<Image>().sprite = selectedSprite;
            }
            
            if (currentSlot != 2)
            {
                if (fullSlot[currentSlot])
                {
                    slots[currentSlot].GetComponent<Image>().sprite = fullUnselectedSprite;
                }
                else
                {
                    slots[currentSlot].GetComponent<Image>().sprite = unselectedSprite;
                }
                
                currentSlot = 2;
            }
        }
        if(Input.GetKey(KeyCode.Alpha4))
        {
            if (fullSlot[3])
            {
                slots[3].GetComponent<Image>().sprite = fullSelectedSprite;
            }
            else
            {
                slots[3].GetComponent<Image>().sprite = selectedSprite;
            }
            
            if (currentSlot != 3)
            {
                if (fullSlot[currentSlot])
                {
                    slots[currentSlot].GetComponent<Image>().sprite = fullUnselectedSprite;
                }
                else
                {
                    slots[currentSlot].GetComponent<Image>().sprite = unselectedSprite;
                }
                
                currentSlot = 3;
            }
        }
        if(Input.GetKey(KeyCode.Alpha5))
        {
            if (fullSlot[4])
            {
                slots[4].GetComponent<Image>().sprite = fullSelectedSprite;
            }
            else
            {
                slots[4].GetComponent<Image>().sprite = selectedSprite;
            }
            
            if (currentSlot != 4)
            {
                if (fullSlot[currentSlot])
                {
                    slots[currentSlot].GetComponent<Image>().sprite = fullUnselectedSprite;
                }
                else
                {
                    slots[currentSlot].GetComponent<Image>().sprite = unselectedSprite;
                }
                
                currentSlot = 4;
            }
        }
    }

   

}