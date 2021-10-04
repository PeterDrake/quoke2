using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class InventoryUI : MonoBehaviour
{
    public int selectedSlotNumber;
    public Sprite unselectedSlotSprite;
    public Sprite selectedSlotSprite;
    public Sprite unselectedSlotSpriteInUse;
    public Sprite selectedSlotSpriteInUse;

    private Inventory inventory;

    [SerializeField] private GameObject grayscaleOverlay;
    
    // These are UI Images showing sprites (either unselectedSlotSprite or selectedSlotSprite).
    public GameObject[] slotFrames;

    // These are UI Images showing sprites for occupied slots. They're inactive for unoccupied slots.
    public GameObject[] slotContents;
    
    private ReferenceManager referenceManager;
    private Text tooltipText;

    private void Awake()
    {
        inventory = this.gameObject.GetComponent<Inventory>();
        slotFrames = new GameObject[5];
        slotContents = new GameObject[5];
        int frameCounter = 0;
        int contentsCounter = 0;
        foreach (Image child in this.gameObject.GetComponentsInChildren<Image>())
        {
            if (child.gameObject.name.Contains("Frame"))
            {
                slotFrames[frameCounter] = child.gameObject;
                frameCounter++;
            }
            if (child.gameObject.name.Contains("Contents"))
            {
                slotContents[contentsCounter] = child.gameObject;
                contentsCounter++;
            }
        }

        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        
        unselectedSlotSpriteInUse = unselectedSlotSprite;
        selectedSlotSpriteInUse = selectedSlotSprite;
        
        // Set initial state of all the arrays
        foreach (GameObject frame in slotFrames)
        {
            frame.GetComponent<Image>().sprite = unselectedSlotSpriteInUse;
        }
        foreach (GameObject item in slotContents)
        {
            item.SetActive(false);
        }

        // Select the first slot
        selectedSlotNumber = 0;
        slotFrames[selectedSlotNumber].GetComponent<Image>().sprite = selectedSlotSpriteInUse;
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (GlobalControls.globalControlsProperties.Contains("tooltipsEnabled"))
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
    
    public void SelectSlotNumber(int slotNumber)
    {
        if (selectedSlotNumber != slotNumber)
        {
            slotFrames[slotNumber].GetComponent<Image>().sprite = selectedSlotSpriteInUse;
            slotFrames[selectedSlotNumber].GetComponent<Image>().sprite = unselectedSlotSpriteInUse;
            selectedSlotNumber = slotNumber;
        }

        UpdateTooltip();
    }

    public void UpdateTooltip()
    {
        if (GlobalControls.globalControlsProperties.Contains("tooltipsEnabled")
            && slotContents[selectedSlotNumber].activeSelf)
        {
            if(!referenceManager.tooltipCanvas.GetComponentInChildren<Image>(true).gameObject.activeSelf)
                referenceManager.tooltipCanvas.GetComponentInChildren<Image>(true)
                    .gameObject.SetActive(true);
            {
                if (inventory.items[selectedSlotNumber])
                {
                    Comment comment = inventory.items[selectedSlotNumber].GetComponent<Comment>();
                    if (comment)
                        referenceManager.tooltipCanvas.GetComponentInChildren<Text>(true).text =
                            comment.notes;
                    else
                        referenceManager.tooltipCanvas.GetComponentInChildren<Image>(true)
                            .gameObject.SetActive(false);
                }

            }
            
        }
        else if(referenceManager.tooltipCanvas.GetComponentInChildren<Image>(true).gameObject.activeSelf) 
            referenceManager.tooltipCanvas.GetComponentInChildren<Image>(true).gameObject.SetActive(false);
    }
    
    public void SetAvailableSlots(int numSlots)
    {
        GameObject[] tempSlotFrames = new GameObject[numSlots];
        GameObject[] tempSlotContents = new GameObject[numSlots];
        
        for (int i = 0; i < numSlots; i++)
        {
            tempSlotContents[i] = slotContents[i];
            tempSlotFrames[i] = slotFrames[i];
        }

        if (numSlots < slotFrames.Length)
        {
            for (int i = slotFrames.Length - 1; i >= numSlots; i--)
            {
                slotFrames[i].SetActive(false);
            }
        }
        
        slotContents = tempSlotContents;
        slotFrames = tempSlotFrames;


        // Select the first slot
        selectedSlotNumber = 0;
        slotFrames[selectedSlotNumber].GetComponent<Image>().sprite = selectedSlotSpriteInUse;
        SelectSlotNumber(0);
    }
    
    private bool SlotIsOccupied(int i)
    {
        return slotContents[i].activeSelf;
    }

    public void AddToSlot(int slot, Sprite sprite)
    {
        if (slot >= 0 && !(slot >= slotContents.Length))
        {
            // Display the sprite for this item
            slotContents[slot].SetActive(true);
            slotContents[slot].GetComponent<Image>().sprite = sprite;
        }

        //reselect slot to current slot number to update tooltip if necessary
        SelectSlotNumber(selectedSlotNumber);
    }

    public void RemoveFromSlot(int slot)
    {
        if (slot >= 0 && !(slot >= slotContents.Length))
        {
            slotContents[slot].SetActive(false);
            if (tooltipText.gameObject.activeSelf)
                tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
        }
    }

    public void DisableSelectedSlot()
    {
        selectedSlotSpriteInUse = unselectedSlotSprite;
        slotFrames[selectedSlotNumber].GetComponent<Image>().sprite = selectedSlotSpriteInUse;
    }
    
    public void EnableSelectedSlot()
    {
        selectedSlotSpriteInUse = selectedSlotSprite;
        slotFrames[selectedSlotNumber].GetComponent<Image>().sprite = selectedSlotSpriteInUse;
    }

    public void EnableSlotGrayscale(int slot)
    {
        if (slotFrames[slot].transform.childCount == 1) Instantiate(grayscaleOverlay, slotFrames[slot].transform);
    }
    
    public void DisableSlotGrayscale(int slot)
    {
        if (slotFrames[slot].transform.childCount != 1)
        {
            foreach (Transform child in slotFrames[slot].transform)
            {
                if (child.name.Replace("(Clone)", "").Trim() == grayscaleOverlay.name) Destroy(child);
            }
        }
    }
}