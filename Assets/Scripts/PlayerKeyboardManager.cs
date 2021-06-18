using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles keyboard input related to moving the player.
/// </summary>
public class PlayerKeyboardManager : MonoBehaviour
{
    public PlayerMover player;
    public Inventory inventory;
    public bool isExploring;
    public bool isTrading;
    public bool isConversing;
    public bool isSegue;
    public bool isDeath;
    public ReferenceManager referenceManager;
    public TradeManager tradeManager;
    public DialogueManager dialogueManager;
    public GameObject deathCanvas;
    public GameObject segueCanvas;

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
        player = referenceManager.player.GetComponent<PlayerMover>();
        deathCanvas = referenceManager.deathCanvas;
        deathCanvas.SetActive(false);
        segueCanvas = referenceManager.segueCanvas;
        if(SceneManager.GetActiveScene().name.Equals("QuakeHouse")) SetSegue();
        else if (SceneManager.GetActiveScene().name.Equals("StrategicMap"))
        {
            this.gameObject.GetComponent<StrategicMapKeyboardController>().enabled = true;
            this.enabled = false;
        }
        else SetExploring();
        cursorLocation = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isSegue)
        {
            if (Input.GetKeyDown("space"))
            {
                SetExploring();
            }
        }
        else if (isExploring) //if moving around the map
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
        else if (isConversing) //if talking to an npc
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
        else if (isTrading) //if trading with an npc
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
    }
    
    public void SetExploring()
    {
        isExploring = true;
        isConversing = false;
        isTrading = false;
        isSegue = false;
        isDeath = false;
        
        deathCanvas.SetActive(false);
        segueCanvas.SetActive(false);
        referenceManager.player.GetComponent<PlayerMover>().enabled = true;
        referenceManager.inventoryCanvas.SetActive(true);
        referenceManager.dialogueCanvas.SetActive(false);
        referenceManager.tradeCanvas.SetActive(false);
    }

    public void SetConversing()
    {
        isExploring = false;
        isConversing = true;
        isTrading = false;
        isSegue = false;
        isDeath = false;
        cursorLocation = 0;
        
        deathCanvas.SetActive(false);
        segueCanvas.SetActive(false);
        referenceManager.player.GetComponent<PlayerMover>().enabled = false;
        referenceManager.inventoryCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(true);
        referenceManager.tradeCanvas.SetActive(false);
        referenceManager.dialogueCanvas.GetComponent<DialogueManager>().BeginConversation();
    }

    public void SetTrading()
    {
        isExploring = false;
        isConversing = false;
        isTrading = true;
        isSegue = false;
        isDeath = false;
        cursorLocation = 0;
        
        deathCanvas.SetActive(false);
        segueCanvas.SetActive(false);
        referenceManager.player.GetComponent<PlayerMover>().enabled = false;
        referenceManager.inventoryCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(false);
        referenceManager.tradeCanvas.SetActive(true);
        referenceManager.tradeCanvas.GetComponent<TradeManager>().BeginTrading();
    }

    public void SetSegue()
    {
        isExploring = false;
        isConversing = false;
        isTrading = false;
        isSegue = true;
        isDeath = false;

        referenceManager.player.GetComponent<PlayerMover>().enabled = false;
        referenceManager.inventoryCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(false);
        referenceManager.tradeCanvas.SetActive(false);

        deathCanvas.SetActive(false);
        segueCanvas.SetActive(true);

    }

    public void SetDeath()
    {
        isExploring = false;
        isConversing = false;
        isTrading = false;
        isSegue = false;
        isDeath = true;

        referenceManager.player.GetComponent<PlayerMover>().enabled = false;
        referenceManager.inventoryCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(false);
        referenceManager.tradeCanvas.SetActive(false);

        deathCanvas.SetActive(true);
        segueCanvas.SetActive(false);
    }
    
    
}
