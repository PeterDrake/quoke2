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
    public GameObject[] slots;
    public int currentSlot;
    public bool[] fullSlot;

    // Start is called before the first frame update
    void Start()
    {
        selectedSprite = Resources.Load<Sprite>("SelectedSlot 1");
        unselectedSprite = Resources.Load<Sprite>("UnselectedSlot 1");
        slots = GameObject.FindGameObjectsWithTag("Slots");
        slots[0].GetComponent<Image>().sprite = selectedSprite;
        currentSlot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // If user presses 1, select this slot
        // Else if user presses 2 - n, select that other slot
        if(Input.GetKey(KeyCode.Alpha1))
        {
            slots[0].GetComponent<Image>().sprite = selectedSprite;
            if (currentSlot != 0)
            {
                slots[currentSlot].GetComponent<Image>().sprite = unselectedSprite;
                currentSlot = 0;
            }
        }
        if(Input.GetKey(KeyCode.Alpha2))
        {
            slots[1].GetComponent<Image>().sprite = selectedSprite;
            if (currentSlot != 1)
            {
                slots[currentSlot].GetComponent<Image>().sprite = unselectedSprite;
                currentSlot = 1;
            }
        }
        if(Input.GetKey(KeyCode.Alpha3))
        {
            slots[2].GetComponent<Image>().sprite = selectedSprite;
            if (currentSlot != 2)
            {
                slots[currentSlot].GetComponent<Image>().sprite = unselectedSprite;
                currentSlot = 2;
            }
        }
        if(Input.GetKey(KeyCode.Alpha4))
        {
            slots[3].GetComponent<Image>().sprite = selectedSprite;
            if (currentSlot != 3)
            {
                slots[currentSlot].GetComponent<Image>().sprite = unselectedSprite;
                currentSlot = 3;
            }
        }
        if(Input.GetKey(KeyCode.Alpha5))
        {
            slots[4].GetComponent<Image>().sprite = selectedSprite;
            if (currentSlot != 4)
            {
                slots[currentSlot].GetComponent<Image>().sprite = unselectedSprite;
                currentSlot = 4;
            }
        }
    }

   

}