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
    
    public KeyCode keyDown = KeyCode.JoystickButton0;
    
    public readonly KeyCode[] validInputs = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, 
        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0};
    public readonly KeyCode[] validNPCInputs = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, 
        KeyCode.Alpha6};

    private Inventory inventory;
    private InventoryUI inventoryUI;

    // These are UI Images showing sprites (either unselectedSlotSprite or selectedSlotSprite).
    public GameObject[] slotFrames;
    
    private GameObject npcInteractedCanvas;

    // These are UI Images showing sprites for occupied slots. They're inactive for unoccupied slots.
    public GameObject[] slotContents;
    
    private ReferenceManager referenceManager;
    private PlayerKeyboardManager keyboardManager;
    private Text tooltipText;
    
    private int cursorLocation;

    private void Awake()
    {
        inventory = this.gameObject.GetComponent<Inventory>();
        inventoryUI = referenceManager.inventoryCanvas.GetComponent<InventoryUI>();
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
        keyboardManager = GameObject.Find("Managers").GetComponent<PlayerKeyboardManager>();
        
        npcInteractedCanvas = referenceManager.npcInteractedCanvas;
        
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

        if (cursorLocation < inventory.items.Length && inventory)
        {
            for (int i = 0; i < validInputs.Length; i++)
            {
                if (keyDown.Equals(validInputs[i]))
                {
                    inventory.SelectSlotNumber(i);
                    cursorLocation = i;
                }
            }
        } 
        else if (cursorLocation >= inventory.items.Length && npcInteractedCanvas.activeSelf)
        {
            for (int i = 0; i < validNPCInputs.Length; i++)
            {
                if (keyDown.Equals(validNPCInputs[i]))
                {
                    cursorLocation = inventory.items.Length + i;
                    for (int j = 0; j < validNPCInputs.Length; j++)
                    {
                        if(j == i) keyboardManager.npcFrames[j].GetComponent<Image>().sprite = keyboardManager.selected;
                        else keyboardManager.npcFrames[j].GetComponent<Image>().sprite = keyboardManager.unselected;
                    }
                
                    if (GlobalControls.globalControlsProperties.Contains("tooltipsEnabled") && GlobalControls.npcList[keyboardManager.npcList[i]].interacted)
                    {
                        if(!tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.activeSelf)
                            tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(true);
                        tooltipText.text = GlobalControls.npcList[keyboardManager.npcList[i]].description;
                        keyboardManager.npcInventoryTooltip.SetActive(true);
                        keyboardManager.npcInventoryTooltipName.text = GlobalControls.npcList[keyboardManager.npcList[cursorLocation - inventory.items.Length]].name + "'s Inventory";
                        for (int j = keyboardManager.npcInventoryTooltipSprites.Length - 1; j >= 0; j--)
                        {
                            keyboardManager.npcInventoryTooltipSprites[j].sprite = keyboardManager.unselected;
                            keyboardManager.npcInventoryTooltipItemName[j].text = "";
                        }
                        foreach (Item item in GlobalItemList.ItemList.Values)
                        {
                            if (item.scene.Equals("Inventory") && item.containerName.Equals(keyboardManager.npcList[cursorLocation - inventory.items.Length]))
                            {
                                GameObject prefab = (GameObject) Resources.Load(item.name, typeof(GameObject));
                                Sprite sprite = prefab.GetComponent<Collectible>().sprite;
                                keyboardManager.npcInventoryTooltipSprites[(int) item.location.x].sprite = sprite;
                                keyboardManager.npcInventoryTooltipItemName[(int) item.location.x].text = item.name;
                            }
                        }
                    }
                    else tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
                }
            }
        }
        
        // TODO: This probably should be in Inventory UI, as moves the cursor between NPCInteracted UI and Player Inventory UI
        if (inventory && npcInteractedCanvas.activeSelf 
                      && (keyDown.Equals(KeyCode.RightBracket) || keyDown.Equals(KeyCode.LeftBracket)))
        {
            if (cursorLocation > inventory.items.Length - 1)
            {
                keyboardManager.npcFrames[cursorLocation - inventory.items.Length].GetComponent<Image>().sprite = keyboardManager.unselected;
                inventoryUI.EnableSelectedSlot();
                cursorLocation = 0;
                inventory.SelectSlotNumber(cursorLocation);
                
                keyboardManager.npcInventoryTooltip.SetActive(false);
            }
            else
            {
                keyboardManager.npcFrames[0].GetComponent<Image>().sprite = keyboardManager.selected;
                inventoryUI.DisableSelectedSlot();
                cursorLocation = inventory.items.Length;
                inventory.SelectSlotNumber(2);
                
                if (GlobalControls.globalControlsProperties.Contains("tooltipsEnabled") && GlobalControls.npcList[keyboardManager.npcList[0]].interacted)
                {
                    if(!tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.activeSelf)
                        tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(true);
                    tooltipText.text = GlobalControls.npcList[keyboardManager.npcList[0]].description;
                    keyboardManager.npcInventoryTooltip.SetActive(true);
                    keyboardManager.npcInventoryTooltipName.text = GlobalControls.npcList[keyboardManager.npcList[cursorLocation - inventory.items.Length]].name + "'s Inventory";
                    for (int j = keyboardManager.npcInventoryTooltipSprites.Length - 1; j >= 0; j--)
                    {
                        keyboardManager.npcInventoryTooltipSprites[j].sprite = keyboardManager.unselected;
                        keyboardManager.npcInventoryTooltipItemName[j].text = "";
                    }
                    foreach (Item item in GlobalItemList.ItemList.Values)
                    {
                        if (item.scene.Equals("Inventory") && item.containerName.Equals(keyboardManager.npcList[cursorLocation - inventory.items.Length]))
                        {
                            GameObject prefab = (GameObject) Resources.Load(item.name, typeof(GameObject));
                            Sprite sprite = prefab.GetComponent<Collectible>().sprite;
                            keyboardManager.npcInventoryTooltipSprites[(int) item.location.x].sprite = sprite;
                            keyboardManager.npcInventoryTooltipItemName[(int) item.location.x].text = item.name;
                        }
                    }

                    
                }
                else tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
            }
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
}
