using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles keyboard input related to moving the player.
/// </summary>
public class PlayerKeyboardManager : MonoBehaviour
{
    private PlayerMover player;
    private Inventory inventory;

    private int gamemode; //{1 = segue, 2 = conversing, 3 = exploring, 4 = death, 5 = trading}
    private bool inventoryInScene = true; //Set to false if no inventory in scene (Ex. QuakeHouse)
    
    private ReferenceManager referenceManager;
    private TradeManager tradeManager;
    private DialogueManager dialogueManager;
    private GameObject deathCanvas;
    private GameObject segueCanvas;
    private GameObject npcInteractedCanvas;
    private GameObject metersCanvas;

    private int cursorLocation;
    // Note that the 1 key is at index 0, and so on. This neatly accounts for 0-based array index and doesn't have to be
    // accounted for elsewhere.
    private readonly KeyCode[] validInputs = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0};

    void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        tradeManager = referenceManager.tradeCanvas.GetComponent<TradeManager>();
        dialogueManager = referenceManager.dialogueCanvas.GetComponent<DialogueManager>();
        inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        metersCanvas = referenceManager.metersCanvas;
        npcInteractedCanvas = referenceManager.npcInteractedCanvas;
        player = referenceManager.player.GetComponent<PlayerMover>();
        deathCanvas = referenceManager.deathCanvas;
        deathCanvas.SetActive(false);
        segueCanvas = referenceManager.segueCanvas;
        cursorLocation = 0;
        
        
        //Handle start of scene things
        if (SceneManager.GetActiveScene().name.Equals("QuakeHouse"))
        {
            inventoryInScene = false;
            SetSegue();
        }
        else if(SceneManager.GetActiveScene().name.Equals("PreQuakeHouse"))
        {
            inventory.setAvailableSlots(1);
            SetExploring();
        }
        else if (SceneManager.GetActiveScene().name.Equals("StrategicMap"))
        {
            this.gameObject.GetComponent<StrategicMapKeyboardController>().enabled = true;
            this.enabled = false;
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
        // Crouch (c)
        player.SetCrouching(Input.GetKey(KeyCode.C));
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
        for (int i = 0; i < validInputs.Length; i++)
        {
            if (inventory && Input.GetKey(validInputs[i]))
            {
                inventory.SelectSlotNumber(i);
            }
        }
        if (inventory && Input.GetKeyDown(KeyCode.Space))
        {
            inventory.PickUpOrDrop();
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
            cursorLocation = tradeManager.ChangeSelectedInventory(cursorLocation);
                
        }
        if (Input.GetKeyDown("."))
        {
            cursorLocation++;
            cursorLocation = tradeManager.ChangeSelectedInventory(cursorLocation);
        }
            
        //select slots
        for (int i = 0; i < validInputs.Length; i++)
        {
            if (Input.GetKey(validInputs[i]))
            {
                tradeManager.SelectSlot(cursorLocation, i);
            }
        }
        
        
        // Transfer items (space)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tradeManager.EncapsulateSpace(cursorLocation);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (tradeManager.CheckValidTrade())
            {
                Debug.Log("Valid trade!");
                tradeManager.CompleteTrade();
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
        if(inventoryInScene) referenceManager.inventoryCanvas.SetActive(true);
        referenceManager.dialogueCanvas.SetActive(false);
        referenceManager.tradeCanvas.SetActive(false);
        referenceManager.npcInteractedCanvas.SetActive(true);
    }

    public void SetConversing()
    {
        gamemode = 2;
        cursorLocation = 0;
        
        deathCanvas.SetActive(false);
        segueCanvas.SetActive(false);
        referenceManager.player.GetComponent<PlayerMover>().enabled = false;
        referenceManager.inventoryCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(true);
        referenceManager.tradeCanvas.SetActive(false);
        referenceManager.npcInteractedCanvas.SetActive(false);
        referenceManager.dialogueCanvas.GetComponent<DialogueManager>().BeginConversation();
    }

    public void SetTrading()
    {
        gamemode = 5;
        cursorLocation = 0;
        
        deathCanvas.SetActive(false);
        segueCanvas.SetActive(false);
        referenceManager.player.GetComponent<PlayerMover>().enabled = false;
        referenceManager.inventoryCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(false);
        referenceManager.tradeCanvas.SetActive(true);
        referenceManager.npcInteractedCanvas.SetActive(false);
        referenceManager.tradeCanvas.GetComponent<TradeManager>().BeginTrading();
    }

    public void SetSegue()
    {
        gamemode = 1;

        referenceManager.player.GetComponent<PlayerMover>().enabled = false;
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

        referenceManager.player.GetComponent<PlayerMover>().enabled = false;
        referenceManager.inventoryCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(false);
        referenceManager.tradeCanvas.SetActive(false);
        referenceManager.npcInteractedCanvas.SetActive(false);

        deathCanvas.SetActive(true);
        segueCanvas.SetActive(false);
    }
    
    
}
