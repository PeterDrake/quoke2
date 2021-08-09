using System.Linq;
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
    private bool crouchFlag;
    public bool virtualKeyboard;

    private int gamemode; //{1 = segue, 2 = conversing, 3 = exploring, 4 = death, 5 = trading}
    private bool inventoryInScene = true; //Set to false if no inventory in scene (Ex. QuakeHouse)
    private bool npcInteractedInScene = true;
    private bool pointsInScene = true;

    private ReferenceManager referenceManager;
    private TradeManager tradeManager;
    private DialogueManager dialogueManager;
    private GameObject deathCanvas;
    private GameObject segueCanvas;
    private GameObject npcInteractedCanvas;
    private GameObject metersCanvas;
    private GameObject toolTips;
    private GameObject objectives;
    private Text tooltipText;
    private Text npcInventoryTooltipName;
    private Image[] npcInventoryTooltipSprites;
    private GameObject npcInventoryTooltip;
    private Text[] npcInventoryTooltipItemName;

    public bool leftTrading = false;

    private int cursorLocation;

    private int inventoryNumber;
    // Note that the 1 key is at index 0, and so on. This neatly accounts for 0-based array index and doesn't have to be
    // accounted for elsewhere.
    private readonly KeyCode[] validInputs = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, 
        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0};
    private readonly KeyCode[] validNPCInputs = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, 
        KeyCode.Alpha6};
    private readonly string[] npcList = {"safi0", "dem0", "rainer0", "annette0", "carlos0", "angie0"};
    private readonly GameObject[] npcFrames = new GameObject[6];
    private Sprite unselected;
    private Sprite selected;

    private KeyCode keyDown = KeyCode.JoystickButton0;
    
    void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        tradeManager = referenceManager.tradeCanvas.GetComponent<TradeManager>();
        dialogueManager = referenceManager.dialogueCanvas.GetComponent<DialogueManager>();
        inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        metersCanvas = referenceManager.metersCanvas;
        npcInteractedCanvas = referenceManager.npcInteractedCanvas;
        if (GlobalControls.TooltipsEnabled)
        {
            npcInventoryTooltipSprites = new Image[4];
            npcInventoryTooltipItemName = new Text[4];
            int i = 0;
            int j = 0;
            foreach (Transform child in referenceManager.tooltipCanvas.GetComponentsInChildren<Transform>(true))
            {
                if (child.gameObject.name.Equals("Tooltip"))
                {
                    toolTips = child.gameObject;
                    tooltipText = child.gameObject.GetComponentInChildren<Text>(true);
                }
                else if (child.gameObject.name.Equals("Objectives")) objectives = child.gameObject;
                else if (child.gameObject.name.Equals("NPC Inventory"))
                {
                    npcInventoryTooltip = child.gameObject;
                    npcInventoryTooltipName = child.gameObject.GetComponentInChildren<Text>(true);
                }
                else if (child.gameObject.name.Contains("NPC Inventory"))
                {
                    if (child.gameObject.name.Equals("NPC Inventory")) continue;
                    if (child.gameObject.name.Contains("Image"))
                    {
                        npcInventoryTooltipSprites[i] = child.gameObject.GetComponentInChildren<Image>();
                        i++;
                    }
                    else
                    {
                        npcInventoryTooltipItemName[j] = child.gameObject.GetComponentInChildren<Text>(true);
                        j++;
                    }
                }
            }
            referenceManager.pointsText.GetComponentInChildren<Text>().text = GlobalControls.CurrentPoints.ToString();
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
            if (image.gameObject.name.Equals("Annette Frame")) npcFrames[3] = image.gameObject;
            if (image.gameObject.name.Equals("Carlos Frame")) npcFrames[4] = image.gameObject;
            if (image.gameObject.name.Equals("Angie Frame")) npcFrames[5] = image.gameObject;
            
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
            pointsInScene = false;
            npcInteractedInScene = false;
            GlobalControls.CurrentObjective = 3;
            SetSegue();
        }
        else if(SceneManager.GetActiveScene().name.Equals("PreQuakeHouse") || SceneManager.GetActiveScene().name.Equals("PreQuakeApartment"))
        {
            if(!inventory.gameObject.activeSelf) inventory.gameObject.SetActive(true);
            inventory.SetAvailableSlots(1);
            npcInteractedInScene = false;
            pointsInScene = false;
            if(GlobalControls.ApartmentCondition) GlobalControls.CurrentObjective = 2;
            else if(!GlobalControls.ApartmentCondition) GlobalControls.CurrentObjective = 1;
            SetExploring();
        }
        else if (SceneManager.GetActiveScene().name.Equals("StrategicMap"))
        {
            gameObject.GetComponent<StrategicMapKeyboardController>().enabled = true;
            enabled = false;
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

    /// <summary>
    /// SetKeyDown sets a specific key to be pressed for one update frame. Pass KeyCode.JoystickButton0 for none
    /// </summary>
    /// <param name="pressed"></param>
    public void SetKeyDown(KeyCode pressed)
    {
        keyDown = pressed;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!virtualKeyboard)
        {
            if (Input.GetKeyDown(KeyCode.C)) keyDown = KeyCode.C;
            else if (Input.GetKeyDown(KeyCode.Space)) keyDown = KeyCode.Space;
            else if (Input.GetKeyDown(KeyCode.Escape)) keyDown = KeyCode.Escape;
            else if (Input.GetKeyDown(KeyCode.Return)) keyDown = KeyCode.Return;
            else if (Input.GetKeyDown(KeyCode.Comma)) keyDown = KeyCode.Comma;
            else if (Input.GetKeyDown(KeyCode.Period)) keyDown = KeyCode.Period;
            else if (Input.GetKeyDown(KeyCode.LeftBracket)) keyDown = KeyCode.LeftBracket;
            else if (Input.GetKeyDown(KeyCode.RightBracket)) keyDown = KeyCode.RightBracket;
            else if (gamemode != 2 && Input.GetKey(KeyCode.W)) keyDown = KeyCode.W;
            else if (gamemode != 2 && Input.GetKey(KeyCode.A)) keyDown = KeyCode.A;
            else if (gamemode != 2 && Input.GetKey(KeyCode.S)) keyDown = KeyCode.S;
            else if (gamemode != 2 && Input.GetKey(KeyCode.D)) keyDown = KeyCode.D;
            else if (gamemode == 2 && Input.GetKeyDown(KeyCode.W)) keyDown = KeyCode.W;
            else if (gamemode == 2 && Input.GetKeyDown(KeyCode.A)) keyDown = KeyCode.A;
            else if (gamemode == 2 && Input.GetKeyDown(KeyCode.S)) keyDown = KeyCode.S;
            else if (gamemode == 2 && Input.GetKeyDown(KeyCode.D)) keyDown = KeyCode.D;
            else if (gamemode != 2 && Input.GetKey(KeyCode.UpArrow)) keyDown = KeyCode.W;
            else if (gamemode != 2 && Input.GetKey(KeyCode.LeftArrow)) keyDown = KeyCode.A;
            else if (gamemode != 2 && Input.GetKey(KeyCode.DownArrow)) keyDown = KeyCode.S;
            else if (gamemode != 2 && Input.GetKey(KeyCode.RightArrow)) keyDown = KeyCode.D;
            else if (gamemode == 2 && Input.GetKeyDown(KeyCode.UpArrow)) keyDown = KeyCode.W;
            else if (gamemode == 2 && Input.GetKeyDown(KeyCode.LeftArrow)) keyDown = KeyCode.A;
            else if (gamemode == 2 && Input.GetKeyDown(KeyCode.DownArrow)) keyDown = KeyCode.S;
            else if (gamemode == 2 && Input.GetKeyDown(KeyCode.RightArrow)) keyDown = KeyCode.D;
            foreach (var key in validInputs)
            {
                if (Input.GetKeyDown(key))
                {
                    keyDown = key;
                    break;
                }
            }
        }

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
        
        keyDown = KeyCode.JoystickButton0;
    }

    private void UpdateSegue()
    {
        if (keyDown.Equals(KeyCode.Space))
        {
            SetExploring();
        }
    }
    
    private void UpdateConversing()
    {
        // Change the cursor's location with < and >
        if (keyDown.Equals(KeyCode.Comma) || keyDown.Equals(KeyCode.W))
        {
            cursorLocation--;
            if (cursorLocation < 0)
            {
                cursorLocation = dialogueManager.buttons.Length - 1;
            }

            if (dialogueManager.buttons[cursorLocation].gameObject.activeSelf)
            {
                cursorLocation = dialogueManager.ChangeCursorLocations(cursorLocation);
            }
            else
            {
                while (!dialogueManager.buttons[cursorLocation].gameObject.activeSelf)
                {
                    cursorLocation--;
                    if (cursorLocation < 0)
                    {
                        cursorLocation = dialogueManager.buttons.Length - 1;
                    }
                }
                cursorLocation = dialogueManager.ChangeCursorLocations(cursorLocation);
            }
        }
        
        if (keyDown.Equals(KeyCode.Period) || keyDown.Equals(KeyCode.S))
        {
            cursorLocation++;
            if (cursorLocation > dialogueManager.buttons.Length - 1)
            {
                cursorLocation = 0;
            }
            if (dialogueManager.buttons[cursorLocation].gameObject.activeSelf)
            {
                cursorLocation = dialogueManager.ChangeCursorLocations(cursorLocation);
            }
            else
            {
                while (!dialogueManager.buttons[cursorLocation].gameObject.activeSelf)
                {
                    cursorLocation++;
                    if (cursorLocation > dialogueManager.buttons.Length - 1)
                    {
                        cursorLocation = 0;
                    }
                }
                cursorLocation = dialogueManager.ChangeCursorLocations(cursorLocation);
            }
        }
            
        
        if (keyDown.Equals(KeyCode.Escape))
        {
            SetExploring();
        }
        
        //Selects an option from the player options
        if (keyDown.Equals(KeyCode.Space))
        { 
            cursorLocation = dialogueManager.EncapsulateSpace();
        }
    }    
    
    private void UpdateExploring()
    {
        switch (crouchFlag)
        {
            // Crouch (c)
            case true when keyDown.Equals(KeyCode.C):
                crouchFlag = !crouchFlag;
                player.SetCrouching(crouchFlag);
                break;
            case false when keyDown.Equals(KeyCode.C):
                crouchFlag = !crouchFlag;
                player.SetCrouching(crouchFlag);
                break;
        }

        // Move (wasd)
        if (keyDown.Equals(KeyCode.W)) player.StartMoving(new Vector3(0,0,1));
        else if (keyDown.Equals(KeyCode.A)) player.StartMoving(new Vector3(-1,0,0));
        else if (keyDown.Equals(KeyCode.S)) player.StartMoving(new Vector3(0,0,-1));
        else if (keyDown.Equals(KeyCode.D)) player.StartMoving(new Vector3(1, 0, 0));
        
        // Select from inventory (1-9)
        if (cursorLocation < inventory.slotContents.Length && inventory)
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
        else if (cursorLocation >= inventory.slotContents.Length && npcInteractedCanvas.activeSelf)
        {
            for (int i = 0; i < validNPCInputs.Length; i++)
            {
                if (keyDown.Equals(validNPCInputs[i]))
                {
                    cursorLocation = inventory.slotContents.Length + i;
                    for (int j = 0; j < validNPCInputs.Length; j++)
                    {
                        if(j == i) npcFrames[j].GetComponent<Image>().sprite = selected;
                        else npcFrames[j].GetComponent<Image>().sprite = unselected;
                    }
                
                    if (GlobalControls.TooltipsEnabled && GlobalControls.npcList[npcList[i]].interacted)
                    {
                        if(!tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.activeSelf)
                            tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(true);
                        tooltipText.text = GlobalControls.npcList[npcList[i]].description;
                        npcInventoryTooltip.SetActive(true);
                        npcInventoryTooltipName.text = GlobalControls.npcList[npcList[cursorLocation - inventory.slotContents.Length]].name + "'s Inventory";
                        for (int j = npcInventoryTooltipSprites.Length - 1; j >= 0; j--)
                        {
                            npcInventoryTooltipSprites[j].sprite = unselected;
                            npcInventoryTooltipItemName[j].text = "";
                        }
                        foreach (Item item in GlobalItemList.ItemList.Values)
                        {
                            if (item.scene.Equals("Inventory") && item.containerName.Equals(npcList[cursorLocation - inventory.slotContents.Length]))
                            {
                                GameObject prefab = (GameObject) Resources.Load(item.name, typeof(GameObject));
                                Sprite sprite = prefab.GetComponent<Collectible>().sprite;
                                npcInventoryTooltipSprites[(int) item.location.x].sprite = sprite;
                                npcInventoryTooltipItemName[(int) item.location.x].text = item.name;
                            }
                        }
                    }
                    else tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
                }
            }
        }

        
        if (inventoryInScene && cursorLocation < inventory.slotContents.Length  && keyDown.Equals(KeyCode.Space))
        {
            inventory.PickUpOrDrop();
        }

        if (inventory && npcInteractedCanvas.activeSelf && (keyDown.Equals(KeyCode.RightBracket) || keyDown.Equals(KeyCode.LeftBracket)))
        {
            if (cursorLocation > inventory.slotContents.Length - 1)
            {
                npcFrames[cursorLocation - inventory.slotContents.Length].GetComponent<Image>().sprite = unselected;
                inventory.selectedSlotSprite = selected;
                cursorLocation = 0;
                inventory.SelectSlotNumber(cursorLocation);
                
                npcInventoryTooltip.SetActive(false);
            }
            else
            {
                npcFrames[0].GetComponent<Image>().sprite = selected;
                inventory.selectedSlotSprite = unselected;
                cursorLocation = inventory.slotContents.Length;
                inventory.SelectSlotNumber(2);
                
                if (GlobalControls.TooltipsEnabled && GlobalControls.npcList[npcList[0]].interacted)
                {
                    if(!tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.activeSelf)
                        tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(true);
                    tooltipText.text = GlobalControls.npcList[npcList[0]].description;
                    npcInventoryTooltip.SetActive(true);
                    npcInventoryTooltipName.text = GlobalControls.npcList[npcList[cursorLocation - inventory.slotContents.Length]].name + "'s Inventory";
                    for (int j = npcInventoryTooltipSprites.Length - 1; j >= 0; j--)
                    {
                        npcInventoryTooltipSprites[j].sprite = unselected;
                        npcInventoryTooltipItemName[j].text = "";
                    }
                    foreach (Item item in GlobalItemList.ItemList.Values)
                    {
                        if (item.scene.Equals("Inventory") && item.containerName.Equals(npcList[cursorLocation - inventory.slotContents.Length]))
                        {
                            GameObject prefab = (GameObject) Resources.Load(item.name, typeof(GameObject));
                            Sprite sprite = prefab.GetComponent<Collectible>().sprite;
                            npcInventoryTooltipSprites[(int) item.location.x].sprite = sprite;
                            npcInventoryTooltipItemName[(int) item.location.x].text = item.name;
                        }
                    }

                    
                }
                else tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
            }
        }

        if (inventory && keyDown.Equals(KeyCode.Period))
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
                        GlobalControls.npcList[npcList[cursorLocation - inventory.slotContents.Length]].interacted)
                    {
                        if(!tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.activeSelf)
                            tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(true);
                        tooltipText.text = GlobalControls.npcList[npcList[cursorLocation - inventory.slotContents.Length]].description;
                        npcInventoryTooltip.SetActive(true);
                        npcInventoryTooltipName.text = GlobalControls.npcList[npcList[cursorLocation - inventory.slotContents.Length]].name + "'s Inventory";
                        for (int j = npcInventoryTooltipSprites.Length - 1; j >= 0; j--)
                        {
                            npcInventoryTooltipSprites[j].sprite = unselected;
                            npcInventoryTooltipItemName[j].text = "";
                        }
                        foreach (Item item in GlobalItemList.ItemList.Values)
                        {
                            if (item.scene.Equals("Inventory") && item.containerName.Equals(npcList[cursorLocation - inventory.slotContents.Length]))
                            {
                                GameObject prefab = (GameObject) Resources.Load(item.name, typeof(GameObject));
                                Sprite sprite = prefab.GetComponent<Collectible>().sprite;
                                npcInventoryTooltipSprites[(int) item.location.x].sprite = sprite;
                                npcInventoryTooltipItemName[(int) item.location.x].text = item.name;
                            }
                        }
                    }
                    else
                    {
                        tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
                        npcInventoryTooltip.SetActive(false);
                    }
                    
                }
                else
                {
                    cursorLocation = 0;
                    inventory.SelectSlotNumber(0);
                    npcInventoryTooltip.SetActive(false);
                }

            }
            else
            {
                inventory.SelectSlotNumber(cursorLocation);
                npcInventoryTooltip.SetActive(false);
            }
        }

        if (inventory && keyDown.Equals(KeyCode.Comma))
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
                        GlobalControls.npcList[npcList[cursorLocation - inventory.slotContents.Length]].interacted)
                    {
                        if(!tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.activeSelf)
                            tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(true);
                        tooltipText.text = GlobalControls.npcList[npcList[cursorLocation - inventory.slotContents.Length]].description;
                        npcInventoryTooltip.SetActive(true);
                        npcInventoryTooltipName.text = GlobalControls.npcList[npcList[cursorLocation - inventory.slotContents.Length]].name + "'s Inventory";
                        for (int j = npcInventoryTooltipSprites.Length - 1; j >= 0; j--)
                        {
                            npcInventoryTooltipSprites[j].sprite = unselected;
                            npcInventoryTooltipItemName[j].text = "";
                        }
                        foreach (Item item in GlobalItemList.ItemList.Values)
                        {
                            if (item.scene.Equals("Inventory") && item.containerName.Equals(npcList[cursorLocation - inventory.slotContents.Length]))
                            {
                                GameObject prefab = (GameObject) Resources.Load(item.name, typeof(GameObject));
                                Sprite sprite = prefab.GetComponent<Collectible>().sprite;
                                npcInventoryTooltipSprites[(int) item.location.x].sprite = sprite;
                                npcInventoryTooltipItemName[(int) item.location.x].text = item.name;
                            }
                        }
                    }
                    else
                    {
                        tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
                        npcInventoryTooltip.SetActive(false);
                    }
                }
                else
                {
                    cursorLocation = 0;
                    npcInventoryTooltip.SetActive(false);
                    inventory.SelectSlotNumber(0);
                }

            }
            else
            {
                if (cursorLocation.Equals(inventory.slotContents.Length - 1))
                {
                    npcFrames[0].GetComponent<Image>().sprite = unselected;
                }

                npcInventoryTooltip.SetActive(false);
                inventory.selectedSlotSprite = selected;
                inventory.SelectSlotNumber(cursorLocation);
            }
        }
        
    }
    
    private void UpdateDeath()
    {
        if (keyDown.Equals(KeyCode.Space))
        {
            referenceManager.sceneManagement.GetComponent<SceneManagement>().Restart();
        }
    }
    private void UpdateTrading()
    {
        if (keyDown.Equals(KeyCode.Comma))
        {
            cursorLocation--;
        }
        if (keyDown.Equals(KeyCode.Period))
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
        else if (cursorLocation > 9 && cursorLocation < 14)
        {
            inventoryNumber = 2;
        }
        else if (cursorLocation > 13 && cursorLocation < 18)
        {
            inventoryNumber = 3;
        }
        else if (cursorLocation > 17)
        {
            cursorLocation = 0;
            inventoryNumber = 0;
        }
        else if (cursorLocation < 0)
        {
            cursorLocation = 17;
            inventoryNumber = 3;
        }

        tradeManager.ChangeSelectedInventory(inventoryNumber);
        if (inventoryNumber == 0) tradeManager.SelectSlot(inventoryNumber, cursorLocation);
        else if (inventoryNumber == 1) tradeManager.SelectSlot(inventoryNumber, cursorLocation - 5);
        else if (inventoryNumber == 2) tradeManager.SelectSlot(inventoryNumber, cursorLocation - 10);
        else if (inventoryNumber == 3) tradeManager.SelectSlot(inventoryNumber, cursorLocation - 14);

        if (keyDown.Equals(KeyCode.LeftBracket))
        {
            inventoryNumber--;
            inventoryNumber = tradeManager.ChangeSelectedInventory(inventoryNumber);

            if (inventoryNumber == 0) cursorLocation = 0;
            else if (inventoryNumber == 1) cursorLocation = 5;
            else if (inventoryNumber == 2) cursorLocation = 10;
            else if (inventoryNumber == 3) cursorLocation = 14;
        }
        if (keyDown.Equals(KeyCode.RightBracket))
        {
            inventoryNumber++;
            inventoryNumber = tradeManager.ChangeSelectedInventory(inventoryNumber);
            if (inventoryNumber == 0) cursorLocation = 0;
            else if (inventoryNumber == 1) cursorLocation = 5;
            else if (inventoryNumber == 2) cursorLocation = 10;
            else if (inventoryNumber == 3) cursorLocation = 14;
        }
            
        //select slots
        for (int i = 0; i < validInputs.Length; i++)
        {
            if (Input.GetKey(validInputs[i]))
            {
                tradeManager.SelectSlot(inventoryNumber, i);
                if (inventoryNumber == 0) cursorLocation = 0 + i;
                else if (inventoryNumber == 1) cursorLocation = 5 + i;
                else if (inventoryNumber == 2) cursorLocation = 10 + i;
                else if (inventoryNumber == 3) cursorLocation = 14 + i;
            }
        }
        
        
        // Transfer items (space)
        if (keyDown.Equals(KeyCode.Space))
        {
            tradeManager.EncapsulateSpace(inventoryNumber);
        }

        if (keyDown.Equals(KeyCode.Return))
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
            
        if (keyDown.Equals(KeyCode.Escape))
        {
            leftTrading = true;
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

        if (GlobalControls.TooltipsEnabled)
        {
            toolTips.SetActive(true);
        }
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

        if (pointsInScene)
        {
            referenceManager.pointsText.gameObject.SetActive(true);
            referenceManager.pointsText.GetComponentInChildren<Text>().text = GlobalControls.CurrentPoints.ToString();
        }
        else if (!pointsInScene) referenceManager.pointsText.gameObject.SetActive(false);
        
        if (npcInteractedInScene)
        {
            referenceManager.npcInteractedCanvas.SetActive(true);
            
            for (int i = 0; i < validNPCInputs.Length; i++)
            {
                npcFrames[i].GetComponent<Image>().sprite = unselected;
            }
        }
        else if(!npcInteractedInScene) referenceManager.npcInteractedCanvas.SetActive(false);

        npcInventoryTooltip.SetActive(false);
        
        if (GlobalControls.KeybindsEnabled)
        {
            referenceManager.keybinds.SetActive(true);
            string exploringText = GlobalControls.Keybinds["Exploring"];
            if(!npcInteractedInScene) exploringText = exploringText.Replace("\n[ ] => Switch inventory","");
            if (inventoryInScene)
            {
                Image[] images = inventory.gameObject.GetComponentsInChildren<Image>(true);
                foreach (Image image in images)
                {
                    if (image.gameObject.name.Equals("Frame 1") && !image.gameObject.activeSelf)
                    {
                        exploringText = exploringText.Replace("\n< > => Move slots","");
                        break;
                    }
                }
            }
            referenceManager.keybinds.GetComponentInChildren<Text>().text = exploringText;
        }
        else if (GlobalControls.KeybindsEnabled) referenceManager.keybinds.SetActive(false);

        

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
        npcInventoryTooltip.SetActive(false);
        if (pointsInScene)
        {
            referenceManager.pointsText.gameObject.SetActive(true);
            referenceManager.pointsText.GetComponentInChildren<Text>().text = GlobalControls.CurrentPoints.ToString();
        }
        else if (!pointsInScene) referenceManager.pointsText.gameObject.SetActive(false);

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
        if (GlobalControls.TooltipsEnabled)
        {
            toolTips.SetActive(true);
        }
        else if(!GlobalControls.TooltipsEnabled) toolTips.SetActive(false);
        if (GlobalControls.KeybindsEnabled)
        {
            referenceManager.keybinds.SetActive(true);
            referenceManager.keybinds.GetComponentInChildren<Text>().text = GlobalControls.Keybinds["Trading"];
        }
        else if (GlobalControls.KeybindsEnabled) referenceManager.keybinds.SetActive(false);
        if (GlobalControls.ObjectivesEnabled)
        {
            objectives.SetActive(true);
            referenceManager.objectiveManager.UpdateObjectiveBanner();
        }
        else if(!GlobalControls.ObjectivesEnabled) objectives.SetActive(false);
        
        if (pointsInScene)
        {
            referenceManager.pointsText.gameObject.SetActive(true);
            referenceManager.pointsText.GetComponentInChildren<Text>().text = GlobalControls.CurrentPoints.ToString();
        }
        else if (!pointsInScene) referenceManager.pointsText.gameObject.SetActive(false);
        
        referenceManager.tradeCanvas.GetComponent<TradeManager>().BeginTrading();
        npcInventoryTooltip.SetActive(false);
        
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
        referenceManager.pointsText.gameObject.SetActive(false);
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
        referenceManager.pointsText.gameObject.SetActive(false);
        
    }
    
    
}
