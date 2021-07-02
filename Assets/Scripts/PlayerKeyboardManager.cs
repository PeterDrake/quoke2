using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles keyboard input related to moving the player.
/// </summary>
public class PlayerKeyboardManager : MonoBehaviour
{
    private PlayerMover player;
    private Inventory inventory;
    private bool crouchFlag = false;

    private int gamemode; //{1 = segue, 2 = conversing, 3 = exploring, 4 = death, 5 = trading}
    private bool inventoryInScene = true; //Set to false if no inventory in scene (Ex. QuakeHouse)
    private bool npcInteractedInScene = true;

    private ReferenceManager referenceManager;
    private TradeManager tradeManager;
    private DialogueManager dialogueManager;
    private GameObject deathCanvas;
    private GameObject segueCanvas;
    private GameObject npcInteractedCanvas;
    private GameObject metersCanvas;
    private GameObject tooltipCanvas;
    private GameObject toolTips;
    private GameObject objectives;
    private Text tooltipText;

    private int cursorLocation;

    private int inventoryNumber;
    // Note that the 1 key is at index 0, and so on. This neatly accounts for 0-based array index and doesn't have to be
    // accounted for elsewhere.
    private readonly KeyCode[] validInputs = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, 
        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0};
    private readonly KeyCode[] validNPCInputs = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4};
    private readonly string[] npcList = {"safi0", "dem0", "rainer0", "fred0"};
    private readonly GameObject[] npcFrames = new GameObject[4];
    private Sprite unselected;
    private Sprite selected;
    
    void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        tradeManager = referenceManager.tradeCanvas.GetComponent<TradeManager>();
        dialogueManager = referenceManager.dialogueCanvas.GetComponent<DialogueManager>();
        inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        metersCanvas = referenceManager.metersCanvas;
        npcInteractedCanvas = referenceManager.npcInteractedCanvas;
        tooltipCanvas = referenceManager.tooltipCanvas;
        foreach (Image image in tooltipCanvas.GetComponentsInChildren<Image>(true))
        {
            if (image.gameObject.name.Equals("Tooltip")) toolTips = image.gameObject;
            if (image.gameObject.name.Equals("Objectives")) objectives = image.gameObject;
        }
        if (GlobalControls.TooltipsEnabled)
        {
            foreach (Image image in referenceManager.tooltipCanvas.GetComponentsInChildren<Image>(true))
            {
                if (image.gameObject.name.Equals("Tooltip"))
                {
                    tooltipText = image.gameObject.GetComponentInChildren<Text>(true);
                }
            }
        }
        player = referenceManager.player.GetComponent<PlayerMover>();
        deathCanvas = referenceManager.deathCanvas;
        deathCanvas.SetActive(false);
        segueCanvas = referenceManager.segueCanvas;
        cursorLocation = 0;
        
        unselected = Resources.Load<Sprite>("UnselectedSlot 1");
        selected = Resources.Load<Sprite>("SelectedSlot 1");

        foreach (Image image in npcInteractedCanvas.GetComponentsInChildren<Image>(true))
        {
            if (image.gameObject.name.Equals("Safi Frame")) npcFrames[0] = image.gameObject;
            if (image.gameObject.name.Equals("Dem Frame")) npcFrames[1] = image.gameObject;
            if (image.gameObject.name.Equals("Rainer Frame")) npcFrames[2] = image.gameObject;
            if (image.gameObject.name.Equals("Fred Frame")) npcFrames[3] = image.gameObject;
        }
        
        
        //Handle start of scene things
        if (SceneManager.GetActiveScene().name.Contains("Quake"))
        {
            GlobalControls.MetersEnabled = false;
        }
        else
        {
            GlobalControls.MetersEnabled = true;
        }
        
        if (SceneManager.GetActiveScene().name.Equals("GameEnd"))
        {
            inventoryInScene = false;
            GlobalControls.ObjectivesEnabled = false;
            npcInteractedInScene = false;
            SetExploring();
        }
        else if (SceneManager.GetActiveScene().name.Equals("QuakeApartment") || SceneManager.GetActiveScene().name.Equals("QuakeHouse"))
        {
            if(!inventory.gameObject.activeSelf) inventory.gameObject.SetActive(true);
            inventory.SetAvailableSlots(2);
            npcInteractedInScene = false;
            GlobalControls.CurrentObjective = 3;
            SetSegue();
        }
        else if(SceneManager.GetActiveScene().name.Equals("PreQuakeHouse") || SceneManager.GetActiveScene().name.Equals("PreQuakeApartment"))
        {
            if(!inventory.gameObject.activeSelf) inventory.gameObject.SetActive(true);
            inventory.SetAvailableSlots(1);
            npcInteractedInScene = false;
            if(GlobalControls.ApartmentCondition) GlobalControls.CurrentObjective = 2;
            else if(!GlobalControls.ApartmentCondition) GlobalControls.CurrentObjective = 1;
            SetExploring();
        }
        else if (SceneManager.GetActiveScene().name.Equals("StrategicMap"))
        {
            this.gameObject.GetComponent<StrategicMapKeyboardController>().enabled = true;
            this.enabled = false;
        }
        else if (GlobalControls.CurrentObjective <= 4)
        {
            GlobalControls.CurrentObjective = 5;
            SetExploring();
        }
        else SetExploring();
        
        if (!SceneManager.GetActiveScene().name.Equals("StrategicMap") && GlobalControls.MetersEnabled)
        {
            if (!GlobalControls.PoopTaskCompleted && GlobalControls.PoopTimeLeft == 0)
            {
                Debug.Log("You died by poop meter going to zero!!");
                GameObject.Find("Managers").GetComponent<ReferenceManager>().deathManager.GetComponent<PlayerDeath>().KillPlayer(metersCanvas, 4);
            }
            if (!GlobalControls.WaterTaskCompleted && GlobalControls.WaterTimeLeft == 0)
            {
                Debug.Log("You died of thirst!");
                GameObject.Find("Managers").GetComponent<ReferenceManager>().deathManager.GetComponent<PlayerDeath>().KillPlayer(metersCanvas, 2);
            }
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (gamemode == 1)
        {
            UpdateSegue();
        }
        else if (gamemode == 2) //if talking to an npc
        {
            UpdateConversing();
        }
        else if (gamemode == 3) //if moving around the map
        {
            UpdateExploring();
        } 
        else if (gamemode == 4) //if ded
        {
            UpdateDeath();
        } 
        else if (gamemode == 5) //if trading with an npc
        {
            UpdateTrading();
        }
    }

    private void UpdateSegue()
    {
        if (Input.GetKeyDown("space"))
        {
            SetExploring();
        }
    }
    
    private void UpdateConversing()
    {
        // Change the cursor's location with < and >
        if (Input.GetKeyDown(","))
        {
            cursorLocation--;
            cursorLocation = dialogueManager.ChangeCursorLocations(cursorLocation);
        }
        if (Input.GetKeyDown("."))
        {
            cursorLocation++;   
            cursorLocation = dialogueManager.ChangeCursorLocations(cursorLocation);
        }
            
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {  
            SetExploring();
        }
        
        //Selects an option from the player options
        if (Input.GetKeyDown("space"))
        {
            dialogueManager.EncapsulateSpace();
            cursorLocation = dialogueManager.ChangeCursorLocations(0);
        }
    }    
    
    private void UpdateExploring()
    {
        switch (crouchFlag)
        {
            // Crouch (c)
            case true when Input.GetKeyDown(KeyCode.C):
                crouchFlag = !crouchFlag;
                player.SetCrouching(crouchFlag);
                break;
            case false when Input.GetKeyDown(KeyCode.C):
                crouchFlag = !crouchFlag;
                player.SetCrouching(crouchFlag);
                break;
        }
        
        // Move (wasd or arrow keys)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (Math.Abs(h) >= 1f)
        {
            player.StartMoving(new Vector3(h, 0f, 0f));
        } else if (Math.Abs(v) >= 1f)
        {
            player.StartMoving(new Vector3(0f, 0f, v));
        }
            
            
        // Select from inventory (1-9)
        if (cursorLocation < inventory.slotContents.Length && inventory)
        {
            for (int i = 0; i < validInputs.Length; i++)
            {
                if (Input.GetKey(validInputs[i]))
                {
                    inventory.SelectSlotNumber(i);
                    cursorLocation = i;
                }
            }
        } 
        else if (cursorLocation >= inventory.slotContents.Length && npcInteractedCanvas.activeSelf)
        {
            for (int i = 0; i < validNPCInputs.Length; i++)
            {
                if (Input.GetKey(validNPCInputs[i]))
                {
                    cursorLocation = inventory.slotContents.Length + i;
                    for (int j = 0; j < validNPCInputs.Length; j++)
                    {
                        if(j == i) npcFrames[j].GetComponent<Image>().sprite = selected;
                        else npcFrames[j].GetComponent<Image>().sprite = unselected;
                    }
                
                    if (GlobalControls.TooltipsEnabled && GlobalControls.NPCList[npcList[i]].interracted)
                    {
                        if(!tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.activeSelf)
                            tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(true);
                        tooltipText.text = GlobalControls.NPCList[npcList[i]].description;
                    }
                    else tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
                }
            }
        }

        
        if (inventoryInScene && cursorLocation < inventory.slotContents.Length  && Input.GetKeyDown(KeyCode.Space))
        {
            inventory.PickUpOrDrop();
        }

        if (inventory && npcInteractedCanvas.activeSelf && Input.GetKeyDown(KeyCode.RightBracket))
        {
            if (cursorLocation > inventory.slotContents.Length - 1)
            {
                npcFrames[cursorLocation - inventory.slotContents.Length].GetComponent<Image>().sprite = unselected;
                inventory.selectedSlotSprite = selected;
                cursorLocation = 0;
                inventory.SelectSlotNumber(cursorLocation);
            }
            else
            {
                npcFrames[0].GetComponent<Image>().sprite = selected;
                inventory.selectedSlotSprite = unselected;
                cursorLocation = inventory.slotContents.Length;
                inventory.SelectSlotNumber(2);
            }
        }
        
        if (inventory && npcInteractedCanvas.activeSelf && Input.GetKeyDown(KeyCode.LeftBracket))
        {
            if (cursorLocation > inventory.slotContents.Length - 1)
            {
                npcFrames[cursorLocation - inventory.slotContents.Length].GetComponent<Image>().sprite = unselected;
                inventory.selectedSlotSprite = selected;
                cursorLocation = 0;
                inventory.SelectSlotNumber(cursorLocation);
            }
            else
            {
                npcFrames[0].GetComponent<Image>().sprite = selected;
                inventory.selectedSlotSprite = unselected;
                cursorLocation = inventory.slotContents.Length;
                inventory.SelectSlotNumber(2);
            }
        }
        
        if (inventory && Input.GetKeyDown("."))
        {
            cursorLocation++;
            if (npcInteractedCanvas.activeSelf && cursorLocation >= inventory.slotContents.Length + npcFrames.Length)
            {
                cursorLocation = 0;
                inventory.selectedSlotSprite = selected;
                inventory.SelectSlotNumber(0); 
        
                for (int i = 0; i < validNPCInputs.Length; i++)
                {
                    npcFrames[i].GetComponent<Image>().sprite = unselected;
                }
            }
            else if (!npcInteractedCanvas.activeSelf && cursorLocation >= inventory.slotContents.Length)
            {
                cursorLocation = 0;
                inventory.SelectSlotNumber(cursorLocation);
            }
            
            if (cursorLocation >= inventory.slotContents.Length)
            {
                if (npcInteractedCanvas.activeSelf)
                {
                    inventory.selectedSlotSprite = unselected;
                    inventory.SelectSlotNumber(1);

                    npcFrames[cursorLocation - inventory.slotContents.Length].GetComponent<Image>().sprite = selected;
                    if(cursorLocation - inventory.slotContents.Length > 0)
                        npcFrames[cursorLocation - inventory.slotContents.Length - 1].GetComponent<Image>().sprite = unselected;

                    if (GlobalControls.TooltipsEnabled && 
                        GlobalControls.NPCList[npcList[cursorLocation - inventory.slotContents.Length]].interracted)
                    {
                        if(!tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.activeSelf)
                            tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(true);
                        tooltipText.text = GlobalControls.NPCList[npcList[cursorLocation - inventory.slotContents.Length]].description;
                    }
                    else tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
                }
                else
                {
                    cursorLocation = 0;
                    inventory.SelectSlotNumber(0);
                }

            }
            else
            {
                inventory.SelectSlotNumber(cursorLocation);
            }
        }

        if (inventory && Input.GetKeyDown(","))
        {
            cursorLocation--;
            if (cursorLocation < 0)
            {
                if (npcInteractedCanvas.activeSelf)
                {
                    cursorLocation = inventory.slotContents.Length + npcFrames.Length - 1;
                }
                else
                {
                    cursorLocation = inventory.slotContents.Length - 1;
                }
            }
            
            if (cursorLocation >= inventory.slotContents.Length)
            {
                if (npcInteractedCanvas.activeSelf)
                {
                    inventory.selectedSlotSprite = unselected;
                    inventory.SelectSlotNumber(1);

                    npcFrames[cursorLocation - inventory.slotContents.Length].GetComponent<Image>().sprite = selected;
                    if(cursorLocation != inventory.slotContents.Length + npcFrames.Length - 1)
                        npcFrames[cursorLocation - inventory.slotContents.Length + 1].GetComponent<Image>().sprite = unselected;

                    if (GlobalControls.TooltipsEnabled && 
                        GlobalControls.NPCList[npcList[cursorLocation - inventory.slotContents.Length]].interracted)
                    {
                        if(!tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.activeSelf)
                            tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(true);
                        tooltipText.text = GlobalControls.NPCList[npcList[cursorLocation - inventory.slotContents.Length]].description;
                    }
                    else tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
                }
                else
                {
                    cursorLocation = 0;
                    inventory.SelectSlotNumber(0);
                }

            }
            else
            {
                if (cursorLocation.Equals(inventory.slotContents.Length - 1))
                {
                    npcFrames[0].GetComponent<Image>().sprite = unselected;
                }

                inventory.selectedSlotSprite = selected;
                inventory.SelectSlotNumber(cursorLocation);
            }
        }
        
    }
    
    private void UpdateDeath()
    {
        if (Input.GetKeyDown("space"))
        {
            referenceManager.sceneManagement.GetComponent<SceneManagement>().Restart();
        }
    }
    private void UpdateTrading()
    {
        if (Input.GetKeyDown(","))
        {
            cursorLocation--;
        }
        if (Input.GetKeyDown("."))
        {
            cursorLocation++;
        }

        if (cursorLocation > -1 && cursorLocation < 5)
        {
            inventoryNumber = 0;
        }
        else if (cursorLocation > 4 && cursorLocation < 10)
        {
            inventoryNumber = 1;
        }
        else if (cursorLocation > 9 && cursorLocation < 15)
        {
            inventoryNumber = 2;
        }
        else if (cursorLocation > 14 && cursorLocation < 20)
        {
            inventoryNumber = 3;
        }
        else if (cursorLocation > 19)
        {
            cursorLocation = 0;
            inventoryNumber = 0;
        }
        else if (cursorLocation < 0)
        {
            cursorLocation = 19;
            inventoryNumber = 3;
        }

        tradeManager.ChangeSelectedInventory(inventoryNumber);
        tradeManager.SelectSlot(inventoryNumber, cursorLocation % 5);
        
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            inventoryNumber--;
            inventoryNumber = tradeManager.ChangeSelectedInventory(inventoryNumber);
            cursorLocation = 5 * inventoryNumber;
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            inventoryNumber++;
            inventoryNumber = tradeManager.ChangeSelectedInventory(inventoryNumber);
            cursorLocation = 5 * inventoryNumber;
        }
            
        //select slots
        for (int i = 0; i < validInputs.Length; i++)
        {
            if (Input.GetKey(validInputs[i]))
            {
                tradeManager.SelectSlot(inventoryNumber, i);
                cursorLocation = inventoryNumber * 5 + i;
            }
        }
        
        
        // Transfer items (space)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tradeManager.EncapsulateSpace(inventoryNumber);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (tradeManager.CheckValidTrade())
            {
                Debug.Log("Valid trade!");
                tradeManager.CompleteTrade();
                referenceManager.itemLoader.SetActive(false);
                referenceManager.itemLoader.SetActive(true);
            }
            else Debug.Log("Invalid Trade!");
        }
            
        if (Input.GetKeyDown(KeyCode.Escape))
        {  
            tradeManager.LeaveTrading();
        }
    }
    
    public void SetExploring()
    {
        gamemode = 3;
        
        deathCanvas.SetActive(false);
        segueCanvas.SetActive(false);
        referenceManager.player.GetComponent<PlayerMover>().enabled = true;
        if(GlobalControls.MetersEnabled) referenceManager.metersCanvas.SetActive(true);
        else if(!GlobalControls.MetersEnabled) referenceManager.metersCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(false);
        referenceManager.tradeCanvas.SetActive(false);
        if (GlobalControls.ObjectivesEnabled)
        {
            objectives.SetActive(true);
            referenceManager.objectiveManager.UpdateObjectiveBanner();
        }
        else if(!GlobalControls.ObjectivesEnabled) objectives.SetActive(false);

        if (GlobalControls.KeybindsEnabled)
        {
            referenceManager.keybinds.SetActive(true);
            referenceManager.keybinds.GetComponentInChildren<Text>().text = GlobalControls.Keybinds["Exploring"];
        }
        else if (GlobalControls.KeybindsEnabled) referenceManager.keybinds.SetActive(false);
        if (GlobalControls.TooltipsEnabled) toolTips.SetActive(true);
        else if(!GlobalControls.TooltipsEnabled) toolTips.SetActive(false);
        if (inventoryInScene)
        {
            referenceManager.tooltipCanvas.SetActive(true);
            referenceManager.inventoryCanvas.SetActive(true);

            cursorLocation = 0;
                
            //Update to show Inventory selected
            inventory.selectedSlotSprite = selected;
            inventory.SelectSlotNumber(0);
        }
        if (npcInteractedInScene)
        {
            referenceManager.npcInteractedCanvas.SetActive(true);
            
            for (int i = 0; i < validNPCInputs.Length; i++)
            {
                npcFrames[i].GetComponent<Image>().sprite = unselected;
            }
        }
        else if(!npcInteractedInScene) referenceManager.npcInteractedCanvas.SetActive(false);

        
    }

    public void SetConversing()
    {
        gamemode = 2;
        cursorLocation = 0;
        
        deathCanvas.SetActive(false);
        segueCanvas.SetActive(false);
        referenceManager.tooltipCanvas.SetActive(true);
        referenceManager.player.GetComponent<PlayerMover>().enabled = false;
        referenceManager.metersCanvas.SetActive(false);
        referenceManager.inventoryCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(true);
        referenceManager.tradeCanvas.SetActive(false);
        referenceManager.npcInteractedCanvas.SetActive(false);
        if (GlobalControls.ObjectivesEnabled)
        {
            objectives.SetActive(true);
            referenceManager.objectiveManager.UpdateObjectiveBanner();
        }
        else if(!GlobalControls.ObjectivesEnabled) objectives.SetActive(false);
        if (GlobalControls.KeybindsEnabled)
        {
            referenceManager.keybinds.SetActive(true);
            referenceManager.keybinds.GetComponentInChildren<Text>().text = GlobalControls.Keybinds["Conversing"];
        }
        else if (GlobalControls.KeybindsEnabled) referenceManager.keybinds.SetActive(false);
        toolTips.SetActive(false);
        
        referenceManager.dialogueCanvas.GetComponent<DialogueManager>().BeginConversation();
    }

    public void SetTrading()
    {
        gamemode = 5;
        cursorLocation = 0;
        inventoryNumber = 0;
        
        deathCanvas.SetActive(false);
        segueCanvas.SetActive(false);
        referenceManager.tooltipCanvas.SetActive(true);
        referenceManager.player.GetComponent<PlayerMover>().enabled = false;
        referenceManager.metersCanvas.SetActive(false);
        referenceManager.inventoryCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(false);
        referenceManager.tradeCanvas.SetActive(true);
        referenceManager.npcInteractedCanvas.SetActive(false);
        objectives.SetActive(false);
        if (GlobalControls.TooltipsEnabled) toolTips.SetActive(true);
        else if(!GlobalControls.TooltipsEnabled) toolTips.SetActive(false);
        if (GlobalControls.KeybindsEnabled)
        {
            referenceManager.keybinds.SetActive(true);
            referenceManager.keybinds.GetComponentInChildren<Text>().text = GlobalControls.Keybinds["Trading"];
        }
        else if (GlobalControls.KeybindsEnabled) referenceManager.keybinds.SetActive(false);
        
        referenceManager.tradeCanvas.GetComponent<TradeManager>().BeginTrading();
        
    }

    public void SetSegue()
    {
        gamemode = 1;

        referenceManager.tooltipCanvas.SetActive(false);
        referenceManager.player.GetComponent<PlayerMover>().enabled = false;
        referenceManager.metersCanvas.SetActive(false);
        referenceManager.inventoryCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(false);
        referenceManager.tradeCanvas.SetActive(false);
        referenceManager.npcInteractedCanvas.SetActive(false);

        deathCanvas.SetActive(false);
        segueCanvas.SetActive(true);

    }

    public void SetDeath()
    {
        gamemode = 4;

        referenceManager.tooltipCanvas.SetActive(false);
        referenceManager.player.GetComponent<PlayerMover>().enabled = false;
        referenceManager.metersCanvas.SetActive(false);
        referenceManager.inventoryCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(false);
        referenceManager.tradeCanvas.SetActive(false);
        referenceManager.npcInteractedCanvas.SetActive(false);

        deathCanvas.SetActive(true);
        segueCanvas.SetActive(false);
    }
    
    
}
