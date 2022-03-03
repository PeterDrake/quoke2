using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Gamemode
{
    Segue = 1,
    Conversing = 2,
    Exploring = 3,
    Death = 4,
    Trading = 5
};

public class GameStateManager : MonoBehaviour
{
    /// <summary>
    /// Manages and changes the state of the game, such as trading, exploring, talking, etc.
    /// </summary>

    private ReferenceManager referenceManager;
    private TooltipManager tooltipManager;
    private PlayerKeyboardManager keyboardManager;
    private GameObject deathCanvas;
    private GameObject segueCanvas;
    private GameObject metersCanvas;

    public Gamemode currentGamemode;

    private bool inventoryInScene = true; //Set to false if no inventory in scene (Ex. QuakeHouse)
    private bool npcInteractedInScene = true;
    private bool pointsInScene = true;
    
    private Inventory inventory;
    private InventoryUI inventoryUI;

    private GameObject inventoryObject;
    
    // Start is called before the first frame update
    public void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        tooltipManager = referenceManager.tooltipManager;
        keyboardManager = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        
        deathCanvas = referenceManager.deathCanvas;
        deathCanvas.SetActive(false);
        segueCanvas = referenceManager.segueCanvas;
        metersCanvas = referenceManager.metersCanvas;

        // foreach (Inventory obj in referenceManager.inventoryCanvas.GetComponentsInChildren<Inventory>())
        // {
        //     Debug.Log("obj: " + obj);
        //     if (obj.gameObject.name.Equals("Inventory"))
        //     {
        //         inventory = obj;
        //     }
        // }
        // foreach (InventoryUI obj in referenceManager.inventoryCanvas.GetComponentsInChildren<InventoryUI>())
        // {
        //     if (obj.gameObject.name.Equals("Inventory"))
        //     {
        //         inventoryUI = obj;
        //     }
        // }        
        // foreach (GameObject obj in referenceManager.inventoryCanvas.GetComponents())
        // {
        //     if (obj.gameObject.name.Equals("Inventory"))
        //     {
        //         inventoryObject = obj;
        //     }
        // }
        inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        inventoryUI = referenceManager.inventoryCanvas.GetComponent<InventoryUI>();

        tooltipManager.HandleTooltip();

        //Handle start of scene things
        if (SceneManager.GetActiveScene().name.Contains("Quake"))
        {
            GlobalControls.globalControlsProperties.Remove("metersEnabled");
        }
        else
        {
            GlobalControls.globalControlsProperties.Add("metersEnabled");
        }

        if (SceneManager.GetActiveScene().name.Equals("GameEnd"))
        {
            inventoryInScene = false;
            GlobalControls.globalControlsProperties.Remove("objectivesEnabled");
            npcInteractedInScene = false;
            SetExploring();
        }
        else if (SceneManager.GetActiveScene().name.Equals("QuakeApartment") ||
                 SceneManager.GetActiveScene().name.Equals("QuakeHouse"))
        {
            if (!inventory.gameObject.activeSelf) inventory.gameObject.SetActive(true);
            inventory.SetAvailableSlots(2);
            pointsInScene = false;
            npcInteractedInScene = false;
            GlobalControls.currentObjective = 3;
            SetSegue();
        }
        else if (SceneManager.GetActiveScene().name.Equals("PreQuakeHouse") ||
                 SceneManager.GetActiveScene().name.Equals("PreQuakeApartment"))
        {
            if (!inventory.gameObject.activeSelf) inventory.gameObject.SetActive(true);
            inventory.SetAvailableSlots(1);
            npcInteractedInScene = false;
            pointsInScene = false;
            if (GlobalControls.globalControlsProperties.Contains("apartmentCondition"))
                GlobalControls.currentObjective = 2;
            else if (!GlobalControls.globalControlsProperties.Contains("apartmentCondition"))
                GlobalControls.currentObjective = 1;
            SetExploring();
        }
        else if (SceneManager.GetActiveScene().name.Equals("StrategicMap"))
        {
            keyboardManager.EnableStrategicMapKeyboard();
            enabled = false;
        }
        else if (GlobalControls.currentObjective <= 4)
        {
            GlobalControls.currentObjective = 5;
            SetExploring();
        }
        else SetExploring();

        if (!SceneManager.GetActiveScene().name.Equals("StrategicMap") &&
            GlobalControls.globalControlsProperties.Contains("metersEnabled"))
        {
            if (!GlobalControls.globalControlsProperties.Contains("poopTaskCompleted") &&
                GlobalControls.poopTimeLeft == 0)
            {
                Debug.Log("You died by poop meter going to zero!!");
                referenceManager.deathManager.GetComponent<PlayerDeath>()
                    .KillPlayer(metersCanvas, 4);
            }

            if (!GlobalControls.globalControlsProperties.Contains("waterTaskCompleted") &&
                GlobalControls.waterTimeLeft == 0)
            {
                Debug.Log("You died of thirst!");
                referenceManager.deathManager.GetComponent<PlayerDeath>()
                    .KillPlayer(metersCanvas, 2);
            }
        }
    }

    public void SetExploring()
    {
        currentGamemode = Gamemode.Exploring;

        deathCanvas.SetActive(false);
        segueCanvas.SetActive(false);
        referenceManager.player.GetComponent<PlayerMover>().enabled = true;
        
        if (GlobalControls.globalControlsProperties.Contains("metersEnabled"))
            referenceManager.metersCanvas.SetActive(true);
        else if (!GlobalControls.globalControlsProperties.Contains("metersEnabled"))
            referenceManager.metersCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(false);
        referenceManager.tradeCanvas.SetActive(false);
        if (GlobalControls.globalControlsProperties.Contains("objectivesEnabled"))
        {
            tooltipManager.SetObjectivesActive();
            referenceManager.objectiveManager.UpdateObjectiveBanner();
        }
        else if (!GlobalControls.globalControlsProperties.Contains("objectivesEnabled"))
            tooltipManager.SetObjectivesInactive();


        if (GlobalControls.globalControlsProperties.Contains("tooltipsEnabled"))
        {
            tooltipManager.SetTooltipsActive();
        }
        else if (!GlobalControls.globalControlsProperties.Contains("tooltipsEnabled"))
            tooltipManager.SetTooltipsInactive();

        if (inventoryInScene)
        {
            referenceManager.tooltipCanvas.SetActive(true);
            referenceManager.inventoryCanvas.SetActive(true);

            keyboardManager.SetCursorLocation(0);

            //Update to show Inventory selected
            inventoryUI.EnableSelectedSlot();
            inventory.SelectSlotNumber(0);
        }

        if (pointsInScene)
        {
            referenceManager.pointsText.gameObject.SetActive(true);
            referenceManager.pointsText.GetComponentInChildren<Text>().text = GlobalControls.currentPoints.ToString();
        }
        else if (!pointsInScene) referenceManager.pointsText.gameObject.SetActive(false);
        
        if (npcInteractedInScene)
        {
            keyboardManager.EnableNPCInteracted();
        }
        else referenceManager.npcInteractedCanvas.SetActive(false);

        tooltipManager.SetNPCInventoryTooltipInactive();

        if (GlobalControls.globalControlsProperties.Contains("keybindsEnabled"))
        {
            referenceManager.keybinds.SetActive(true);
            string exploringText = GlobalControls.keybinds["Exploring"];
            if (!npcInteractedInScene) exploringText = exploringText.Replace("\n[ ] => Switch inventory", "");
            if (inventoryInScene)
            {
                Image[] images = inventory.gameObject.GetComponentsInChildren<Image>(true);
                foreach (Image image in images)
                {
                    if (image.gameObject.name.Equals("Frame 1") && !image.gameObject.activeSelf)
                    {
                        exploringText = exploringText.Replace("\n< > => Move slots", "");
                        break;
                    }
                }
            }

            referenceManager.keybinds.GetComponentInChildren<Text>().text = exploringText;
        }
        else if (GlobalControls.globalControlsProperties.Contains("keybindsEnabled"))
            referenceManager.keybinds.SetActive(false);
    }

    public void SetConversing()
    {
        currentGamemode = Gamemode.Conversing;
        keyboardManager.SetCursorLocation(0);

        deathCanvas.SetActive(false);
        segueCanvas.SetActive(false);
        referenceManager.tooltipCanvas.SetActive(true);
        referenceManager.player.GetComponent<PlayerMover>().enabled = false;
        referenceManager.metersCanvas.SetActive(false);
        referenceManager.inventoryCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(true);
        referenceManager.tradeCanvas.SetActive(false);
        referenceManager.npcInteractedCanvas.SetActive(false);
        if (GlobalControls.globalControlsProperties.Contains("objectivesEnabled"))
        {
            tooltipManager.SetObjectivesActive();
            referenceManager.objectiveManager.UpdateObjectiveBanner();
        }
        else if (!GlobalControls.globalControlsProperties.Contains("objectivesEnabled"))
            tooltipManager.SetObjectivesInactive();

        if (GlobalControls.globalControlsProperties.Contains("keybindsEnabled"))
        {
            referenceManager.keybinds.SetActive(true);
            referenceManager.keybinds.GetComponentInChildren<Text>().text = GlobalControls.keybinds["Conversing"];
        }
        else if (GlobalControls.globalControlsProperties.Contains("keybindsEnabled"))
            referenceManager.keybinds.SetActive(false);

        tooltipManager.SetTooltipsInactive();

        referenceManager.dialogueCanvas.GetComponent<DialogueManager>().BeginConversation();
        tooltipManager.SetNPCInventoryTooltipInactive();
        if (pointsInScene)
        {
            referenceManager.pointsText.gameObject.SetActive(true);
            referenceManager.pointsText.GetComponentInChildren<Text>().text = GlobalControls.currentPoints.ToString();
        }
        else if (!pointsInScene) referenceManager.pointsText.gameObject.SetActive(false);
    }

    public void SetTrading()
    {
        currentGamemode = Gamemode.Trading;
        keyboardManager.SetCursorLocation(0);
        keyboardManager.SetInventoryNumber(0);

        deathCanvas.SetActive(false);
        segueCanvas.SetActive(false);
        referenceManager.tooltipCanvas.SetActive(true);
        referenceManager.player.GetComponent<PlayerMover>().enabled = false;
        referenceManager.metersCanvas.SetActive(false);
        referenceManager.inventoryCanvas.SetActive(false);
        referenceManager.dialogueCanvas.SetActive(false);
        referenceManager.tradeCanvas.SetActive(true);
        referenceManager.npcInteractedCanvas.SetActive(false);
        tooltipManager.SetObjectivesInactive();
        if (GlobalControls.globalControlsProperties.Contains("tooltipsEnabled"))
        {
            tooltipManager.SetTooltipsActive();
        }
        else if (!GlobalControls.globalControlsProperties.Contains("tooltipsEnabled"))
            tooltipManager.SetTooltipsInactive();

        if (GlobalControls.globalControlsProperties.Contains("keybindsEnabled"))
        {
            referenceManager.keybinds.SetActive(true);
            referenceManager.keybinds.GetComponentInChildren<Text>().text = GlobalControls.keybinds["Trading"];
        }
        else if (GlobalControls.globalControlsProperties.Contains("keybindsEnabled"))
            referenceManager.keybinds.SetActive(false);

        if (GlobalControls.globalControlsProperties.Contains("objectivesEnabled"))
        {
            tooltipManager.SetObjectivesActive();
            referenceManager.objectiveManager.UpdateObjectiveBanner();
        }
        else if (!GlobalControls.globalControlsProperties.Contains("objectivesEnabled"))
            tooltipManager.SetObjectivesInactive();

        if (pointsInScene)
        {
            referenceManager.pointsText.gameObject.SetActive(true);
            referenceManager.pointsText.GetComponentInChildren<Text>().text = GlobalControls.currentPoints.ToString();
        }
        else if (!pointsInScene) referenceManager.pointsText.gameObject.SetActive(false);

        referenceManager.tradeCanvas.GetComponent<TradeManagerUI>().BeginTrading();
        tooltipManager.SetNPCInventoryTooltipInactive();
    }
    
    public void SetSegue()
    {
        currentGamemode = Gamemode.Segue;

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
        currentGamemode = Gamemode.Death;

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

    public bool NPCInteractedInScene()
    {
        return npcInteractedInScene;
    }
    
    public bool InventoryInScene()
    {
        return inventoryInScene;
    }
}
