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
    private InventoryUI inventoryUI;
    private bool crouchFlag;
    // TODO Can we get rid of virtualKeyboard?
    public bool virtualKeyboard;

    private Gamemode currentGamemode;

    private ReferenceManager referenceManager;
    private TradeManagerUI tradeManager;
    private DialogueManager dialogueManager;
    private TooltipManager tooltipManager;
    private GameStateManager gameStateManager;
    private GameObject npcInteractedCanvas;

    public bool leftTrading = false;

    private int cursorLocation;

    private int inventoryNumber;

    // Note that the 1 key is at index 0, and so on. This neatly accounts for 0-based array index and doesn't have to be
    // accounted for elsewhere.
    private readonly KeyCode[] validInputs =
    {
        KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0
    };

    private readonly KeyCode[] validNPCInputs =
    {
        KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
        KeyCode.Alpha6
    };

    private readonly string[] npcList = {"Safi", "Dem", "Rainer", "Annette", "Carlos", "Angie"};
    private readonly GameObject[] npcFrames = new GameObject[6];
    private Sprite unselected;
    private Sprite selected;

    private KeyCode keyDown = KeyCode.JoystickButton0;

    void InitalizeManagers()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        tradeManager = referenceManager.tradeCanvas.GetComponent<TradeManagerUI>();
        dialogueManager = referenceManager.dialogueCanvas.GetComponent<DialogueManager>();
        tooltipManager = referenceManager.tooltipManager;
        gameStateManager = referenceManager.gameStateManager.GetComponent<GameStateManager>();
        // inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        foreach (Inventory obj in referenceManager.inventoryCanvas.GetComponentsInChildren<Inventory>())
        {
            if (obj.gameObject.name.Equals("Inventory"))
            {
                inventory = obj;
            }
        }
        inventoryUI = referenceManager.inventoryCanvas.GetComponent<InventoryUI>();
        npcInteractedCanvas = referenceManager.npcInteractedCanvas;
    }

    
    void Start()
    {
        InitalizeManagers();
        tooltipManager.HandleTooltip();

        player = referenceManager.player.GetComponent<PlayerMover>();
        cursorLocation = 0;

        unselected = Resources.Load<Sprite>("UnselectedSlot 1");
        selected = Resources.Load<Sprite>("SelectedSlot 1");

        int index = 0;
        foreach (Transform child in npcInteractedCanvas.GetComponentsInChildren<Transform>(true))
        {
            if (child.name.EndsWith("Panel"))
            {
                npcFrames[index] = child.Find("Frame").gameObject;
                index++;
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
        currentGamemode = gameStateManager.currentGamemode;
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
            else if (currentGamemode != Gamemode.Conversing && Input.GetKey(KeyCode.W)) keyDown = KeyCode.W;
            else if (currentGamemode != Gamemode.Conversing && Input.GetKey(KeyCode.A)) keyDown = KeyCode.A;
            else if (currentGamemode != Gamemode.Conversing && Input.GetKey(KeyCode.S)) keyDown = KeyCode.S;
            else if (currentGamemode != Gamemode.Conversing && Input.GetKey(KeyCode.D)) keyDown = KeyCode.D;
            else if (currentGamemode == Gamemode.Conversing && Input.GetKeyDown(KeyCode.W)) keyDown = KeyCode.W;
            else if (currentGamemode == Gamemode.Conversing && Input.GetKeyDown(KeyCode.A)) keyDown = KeyCode.A;
            else if (currentGamemode == Gamemode.Conversing && Input.GetKeyDown(KeyCode.S)) keyDown = KeyCode.S;
            else if (currentGamemode == Gamemode.Conversing && Input.GetKeyDown(KeyCode.D)) keyDown = KeyCode.D;
            else if (currentGamemode != Gamemode.Conversing && Input.GetKey(KeyCode.UpArrow)) keyDown = KeyCode.W;
            else if (currentGamemode != Gamemode.Conversing && Input.GetKey(KeyCode.LeftArrow)) keyDown = KeyCode.A;
            else if (currentGamemode != Gamemode.Conversing && Input.GetKey(KeyCode.DownArrow)) keyDown = KeyCode.S;
            else if (currentGamemode != Gamemode.Conversing && Input.GetKey(KeyCode.RightArrow)) keyDown = KeyCode.D;
            else if (currentGamemode == Gamemode.Conversing && Input.GetKeyDown(KeyCode.UpArrow)) keyDown = KeyCode.W;
            else if (currentGamemode == Gamemode.Conversing && Input.GetKeyDown(KeyCode.LeftArrow)) keyDown = KeyCode.A;
            else if (currentGamemode == Gamemode.Conversing && Input.GetKeyDown(KeyCode.DownArrow)) keyDown = KeyCode.S;
            else if (currentGamemode == Gamemode.Conversing && Input.GetKeyDown(KeyCode.RightArrow)) keyDown = KeyCode.D;
            foreach (var key in validInputs)
            {
                if (Input.GetKeyDown(key))
                {
                    keyDown = key;
                    break;
                }
            }
        }

        if (currentGamemode == Gamemode.Segue)
        {
            UpdateSegue();
        }
        else if (currentGamemode == Gamemode.Conversing) //if talking to an npc
        {
            UpdateConversing();
        }
        else if (currentGamemode == Gamemode.Exploring) //if moving around the map
        {
            UpdateExploring();
        }
        else if (currentGamemode == Gamemode.Death) //if ded
        {
            UpdateDeath();
        }
        else if (currentGamemode == Gamemode.Trading) //if trading with an npc
        {
            UpdateTrading();
        }

        keyDown = KeyCode.JoystickButton0;
    }

    public void UpdateSegue()
    {
        if (keyDown.Equals(KeyCode.Space))
        {
            gameStateManager.SetExploring();
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
            gameStateManager.SetExploring();
        }

        //Selects an option from the player options
        if (keyDown.Equals(KeyCode.Space))
        {
            cursorLocation = dialogueManager.EncapsulateSpace();
        }
    }

    private void Crouch()
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
    }

    private void MovePlayer()
    {
        // Move (wasd)
        if (keyDown.Equals(KeyCode.W)) player.StartMoving(new Vector3(0, 0, 1));
        else if (keyDown.Equals(KeyCode.A)) player.StartMoving(new Vector3(-1, 0, 0));
        else if (keyDown.Equals(KeyCode.S)) player.StartMoving(new Vector3(0, 0, -1));
        else if (keyDown.Equals(KeyCode.D)) player.StartMoving(new Vector3(1, 0, 0));
    }

    private void SelectSlotsInventory()
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

    private void SelectSlotsNPCInteracted()
    {
        for (int i = 0; i < validNPCInputs.Length; i++)
        {
            if (keyDown.Equals(validNPCInputs[i]))
            {
                cursorLocation = inventory.items.Length + i;

                npcInteractedCanvas.GetComponent<NPCInteracted>().SelectSlotNumber(i);
                
                tooltipManager.UpdateTooltipWithNPCInteracted(npcList[i]);
                // didnt include SetNPCInventoryTooltipInactive();
            }
        }
    }
    

    private void MoveBetweenInventories()
    {
        if (cursorLocation > inventory.items.Length - 1)
        {
            npcFrames[cursorLocation - inventory.items.Length].GetComponent<Image>().sprite = unselected;
            inventoryUI.EnableSelectedSlot();
            cursorLocation = 0;
            inventory.SelectSlotNumber(cursorLocation);

            tooltipManager.SetNPCInventoryTooltipInactive();
        }
        else
        {
            npcFrames[0].GetComponent<Image>().sprite = selected;
            inventoryUI.DisableSelectedSlot();
            cursorLocation = inventory.items.Length;
            inventory.SelectSlotNumber(2);

            tooltipManager.UpdateTooltipWithNPCInteracted(npcList[0]);
            // didnt include SetNPCInventoryTooltipInactive();
        }
    }

    private void InventoryDown()
    {
        cursorLocation++;
        if (npcInteractedCanvas.activeSelf && cursorLocation >= inventory.items.Length + npcFrames.Length)
        {
            cursorLocation = 0;
            inventoryUI.EnableSelectedSlot();
            inventory.SelectSlotNumber(0);

            for (int i = 0; i < validNPCInputs.Length; i++)
            {
                npcFrames[i].GetComponent<Image>().sprite = unselected;
            }
        }
        else if (!npcInteractedCanvas.activeSelf && cursorLocation >= inventory.items.Length)
        {
            cursorLocation = 0;
            inventory.SelectSlotNumber(cursorLocation);
        }

        if (cursorLocation >= inventory.items.Length)
        {
            if (npcInteractedCanvas.activeSelf)
            {
                inventoryUI.DisableSelectedSlot();
                inventory.SelectSlotNumber(1);

                npcFrames[cursorLocation - inventory.items.Length].GetComponent<Image>().sprite = selected;
                if (cursorLocation - inventory.items.Length > 0)
                    npcFrames[cursorLocation - inventory.items.Length - 1].GetComponent<Image>().sprite =
                        unselected;

                tooltipManager.UpdateTooltipWithNPCInteracted(npcList[cursorLocation - inventory.items.Length]);
            }
            else
            {
                cursorLocation = 0;
                inventory.SelectSlotNumber(0);
                tooltipManager.SetNPCInventoryTooltipInactive();
            }
        }
        else
        {
            inventory.SelectSlotNumber(cursorLocation);
            tooltipManager.SetNPCInventoryTooltipInactive();
        }
    }

    private void InventoryUp()
    {
        cursorLocation--;
        if (cursorLocation < 0)
        {
            if (npcInteractedCanvas.activeSelf)
            {
                cursorLocation = inventory.items.Length + npcFrames.Length - 1;
            }
            else
            {
                cursorLocation = inventory.items.Length - 1;
            }
        }

        if (cursorLocation >= inventory.items.Length)
        {
            if (npcInteractedCanvas.activeSelf)
            {
                inventoryUI.DisableSelectedSlot();
                inventory.SelectSlotNumber(1);

                npcFrames[cursorLocation - inventory.items.Length].GetComponent<Image>().sprite = selected;
                if (cursorLocation != inventory.items.Length + npcFrames.Length - 1)
                    npcFrames[cursorLocation - inventory.items.Length + 1].GetComponent<Image>().sprite =
                        unselected;

                tooltipManager.UpdateTooltipWithNPCInteracted(npcList[cursorLocation - inventory.items.Length]);
            }
            else
            {
                cursorLocation = 0;
                tooltipManager.SetNPCInventoryTooltipInactive();
                inventory.SelectSlotNumber(0);
            }
        }
        else
        {
            if (cursorLocation.Equals(inventory.items.Length - 1))
            {
                npcFrames[0].GetComponent<Image>().sprite = unselected;
            }

            tooltipManager.SetNPCInventoryTooltipInactive();
            inventoryUI.EnableSelectedSlot();
            inventory.SelectSlotNumber(cursorLocation);
        }
    }
    
    private void UpdateExploring()
    {
        Crouch();
        MovePlayer();
        
        if (cursorLocation < inventory.items.Length && inventory)
            SelectSlotsInventory();
        else if (cursorLocation >= inventory.items.Length && npcInteractedCanvas.activeSelf)
            SelectSlotsNPCInteracted();

        if (gameStateManager.InventoryInScene() && cursorLocation < inventory.items.Length && keyDown.Equals(KeyCode.Space))
            inventory.PickUpOrDrop();

        if (inventory && npcInteractedCanvas.activeSelf &&
            (keyDown.Equals(KeyCode.RightBracket) || keyDown.Equals(KeyCode.LeftBracket)))
            MoveBetweenInventories();

        if (inventory && keyDown.Equals(KeyCode.Period))
            InventoryDown();

        if (inventory && keyDown.Equals(KeyCode.Comma))
            InventoryUp();
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

    public void SetCursorLocation(int location)
    {
        cursorLocation = location;
    }
    
    public void SetInventoryNumber(int number)
    {
        inventoryNumber = number;
    }

    public void EnableStrategicMapKeyboard()
    {
        gameObject.GetComponent<StrategicMapKeyboardController>().enabled = true;
    }
    
    public void DisableStrategicMapKeyboard()
    {
        gameObject.GetComponent<StrategicMapKeyboardController>().enabled = false;
    }

    public void EnableNPCInteracted()
    {
        // GameStateManager will call this method once before PlayerKeyboardManager has run it's start method
        // This check is to prevent that from throwing an error
        if (referenceManager)
        {
            referenceManager.npcInteractedCanvas.SetActive(true);
            
            for (int i = 0; i < validNPCInputs.Length; i++)
            {
                npcFrames[i].GetComponent<Image>().sprite = unselected;
            }
        }
    }
}