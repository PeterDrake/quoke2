using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class NewInventoryUI : MonoBehaviour
{
    public Sprite unselectedSlotSprite;
    public Sprite selectedSlotSprite;
    public Sprite unselectedSlotSpriteInUse;
    public Sprite selectedSlotSpriteInUse;
    
    private InventoryController inventoryController;

    [SerializeField] private GameObject grayscaleOverlay;
    
    // These are UI Images showing sprites (either unselectedSlotSprite or selectedSlotSprite).
    public GameObject[] slotFrames;

    // These are UI Images showing sprites for occupied slots. They're inactive for unoccupied slots.
    public GameObject[] slotContents;
    
    private ReferenceManager referenceManager;
    private Text tooltipText;

    private void Awake()
    {
        slotFrames = new GameObject[6];
        slotContents = new GameObject[6];
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
        inventoryController = GameObject.Find("Inventory Controller").GetComponent<InventoryController>();
        
        unselectedSlotSpriteInUse = unselectedSlotSprite;
        selectedSlotSpriteInUse = selectedSlotSprite;
        
        // Set initial state of all the arrays
        foreach (GameObject frame in slotFrames)
        {
            frame.GetComponent<Image>().sprite = unselectedSlotSpriteInUse;
        }
        foreach (GameObject item in slotContents)
        {
//            item.SetActive(false);
        }
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
                    Debug.Log("tooltipText: " + tooltipText);
                }
            }
        }

        //SelectSlotNumber(0);
    }
    
    public void SelectSlotNumber(int slotNumber)
    {
        slotFrames[slotNumber].GetComponent<Image>().sprite = selectedSlotSpriteInUse;
        UpdateTooltip();
    }

    /// <summary>
    /// updates the visual indicator (tooltip) of which item is selected
    /// </summary>
    /// this is what it makes it in the box
    public void UpdateTooltip()
    {
        int selectedSlotNumber = inventoryController.GetSelectedSlotNumber();
        if (GlobalControls.globalControlsProperties.Contains("tooltipsEnabled")
            && slotContents[selectedSlotNumber].activeSelf)
        {
            if(!referenceManager.tooltipCanvas.GetComponentInChildren<Image>(true).gameObject.activeSelf)
                referenceManager.tooltipCanvas.GetComponentInChildren<Image>(true)
                    .gameObject.SetActive(true);
            {
                GameObject item = inventoryController.GetItemInSlot(selectedSlotNumber);
                if (item)
                {
                    Comment comment = item.GetComponent<Comment>();
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
    
    /// <summary>
    /// visualizes how many inventory slots the player has access to (1 before quake, 2 during, 5 after)
    /// </summary>
    /// <param name="numSlots"></param>
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
        
        SelectSlotNumber(0);
    }
    
    private bool SlotIsOccupied(int i)
    {
        return slotContents[i].activeSelf;
    }

    public void AddToSlot(int slot, GameObject item)
    {
        Sprite sprite = item.GetComponent<Collectible>().sprite;
        if (slot >= 0 && !(slot >= slotContents.Length))
        {
            // Display the sprite for this item
            slotContents[slot].SetActive(true);
            slotContents[slot].GetComponent<Image>().sprite = sprite;
        }

        UpdateTooltip();
    }

    public void RemoveFromSlot(int slot)
    {
        if (slot >= 0 && slot < slotContents.Length && slotContents[slot].activeSelf)
        {
            slotContents[slot].SetActive(false);
            if (tooltipText.gameObject.activeSelf)
                tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
        }
    }
    
    public void DisableSelectedSlot()
    {
        selectedSlotSpriteInUse = unselectedSlotSprite;
        int selectedSlotNumber = inventoryController.GetSelectedSlotNumber();
        slotFrames[selectedSlotNumber].GetComponent<Image>().sprite = selectedSlotSpriteInUse;
    }
    
    public void EnableSelectedSlot()
    {
        selectedSlotSpriteInUse = selectedSlotSprite;
        int selectedSlotNumber = inventoryController.GetSelectedSlotNumber();
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
    
    /// <summary>
    /// Sets the inventory to contain no items, with slot 0 selected.
    /// </summary>
    public void Clear()
    {
        for (int i = 0; i < slotContents.Length; i++)
        {
            RemoveFromSlot(i);
        }
    }

    public void UpdateUI()
    {
//        Clear();
        int selectedSlotNumber = inventoryController.GetSelectedSlotNumber();
        // Update to correct number of slots
        int numberOfSlots = inventoryController.GetNumberOfSlots();
        SetAvailableSlots(numberOfSlots);
        // iterate through each slot and set it to have the correct slot
        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject item = inventoryController.GetItemInSlot(i);
            if (item)
            {
                AddToSlot(i, item);
            }
        }
        SelectSlotNumber(selectedSlotNumber);
    }
}