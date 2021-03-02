using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Selector : MonoBehaviour
{
    public Sprite oldSprite;
    public Sprite newSprite;
    public GameObject[] slots;

    // Start is called before the first frame update
    void Start()
    {
        newSprite = Resources.Load<Sprite>("SelectedSlot 1.png");
        oldSprite = Resources.Load<Sprite>("UnselectedSlot 1.png");
        slots = GameObject.FindGameObjectsWithTag("Slots");
        slots[0].GetComponent<Image>().sprite = newSprite;
    }

    // Update is called once per frame
    void Update()
    {
        // If user presses 1, select this slot
        // Else if user presses 2 - n, select that other slot
        if(Input.GetKey(KeyCode.Alpha1))
        {
            slots[0].GetComponent<Image>().sprite = newSprite;
            slots[1].GetComponent<Image>().sprite = oldSprite;
            slots[2].GetComponent<Image>().sprite = oldSprite;
            slots[3].GetComponent<Image>().sprite = oldSprite;
            slots[4].GetComponent<Image>().sprite = oldSprite;

        }
        if(Input.GetKey(KeyCode.Alpha2))
        {
            slots[0].GetComponent<Image>().sprite = oldSprite;
            slots[2].GetComponent<Image>().sprite = oldSprite;
            slots[3].GetComponent<Image>().sprite = oldSprite;
            slots[4].GetComponent<Image>().sprite = oldSprite;
            slots[1].GetComponent<Image>().sprite = newSprite;
        }
    }

   

}