using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class holds references to various objects within the scene. It would be possible to do this directly
/// when those objects are needed, but doing this reliably for (e.g.) objects that are disabled at the beginning
/// of a scene is nontrivial. The class encapsulates all of that ugliness.
/// </summary>
public class ReferenceManager : MonoBehaviour
{

    public GameObject canvases;
    
    public GameObject inventoryCanvas;
    public GameObject metersCanvas;
    public GameObject dialogueCanvas;
    public GameObject tradeCanvas;
    public GameObject deathCanvas;
    public GameObject segueCanvas;
    public GameObject npcInteractedCanvas;
    public GameObject tooltipCanvas;

    public GameObject player;
    public GameObject keyboardManager;
    public GameObject deathManager;
    public GameObject sceneManagement;
    public GameObject keybinds;
    public ObjectiveManager objectiveManager;
    public TooltipManager tooltipManager;
    public HelpManager helpManager;
    public GameObject gameStateManager;
    public GameObject pointsText;

    public GameObject itemLoader;

    public InventoryController inventoryController;
    public GameObject metersController;
    public GameObject meters;
    public NPCInteractedController npcInteractedController;
    
    private void Awake()
    {
        Debug.Log("awake in reference manager");
        canvases = GameObject.Find("Canvases");

        foreach (Canvas canvas in canvases.GetComponentsInChildren<Canvas>(true))
        {
            if (canvas.gameObject.name.Equals("Inventory Canvas"))
            {
                inventoryCanvas = canvas.gameObject;
                //metersCanvas = canvas.gameObject;
            }
            else 
            if (canvas.gameObject.name.Equals("New UI Canvas"))
            {
                metersCanvas = canvas.gameObject;
            }
            else if (canvas.gameObject.name.Equals("Dialogue Canvas"))
            {
                dialogueCanvas = canvas.gameObject;
            }
            else if (canvas.gameObject.name.Equals("Trade Canvas"))
            {
                tradeCanvas = canvas.gameObject;
            }
            else if (canvas.gameObject.name.Equals("Death Canvas"))
            {
                deathCanvas = canvas.gameObject;
            }
            else if (canvas.gameObject.name.Equals("Segue Canvas"))
            {
                segueCanvas = canvas.gameObject;
            }
            else if (canvas.gameObject.name.Equals("NPC Interacted Canvas"))
            {
                npcInteractedCanvas = canvas.gameObject;
            }
            else if (canvas.gameObject.name.Equals("Tooltip Canvas"))
            {
                tooltipCanvas = canvas.gameObject;
            }

            // foreach (Transform bar in inventoryCanvas.GetComponentsInChildren<Transform>())
            // {
            //     if (bar.gameObject.name.Equals("TopBar"))
            //     {
            //         foreach (Meters meter in bar.GetComponentsInChildren<Meters>())
            //         {
            //             meters = meter.gameObject;
            //         }
            //     }
            // }
            
            
        }

        meters = GameObject.Find("Meters").gameObject;
        metersCanvas = GameObject.Find("New UI Canvas").gameObject;
        inventoryController = GameObject.Find("Inventory Controller").GetComponent<InventoryController>();
        helpManager = GameObject.Find("Help Canvas").GetComponent<HelpManager>();
        
        keyboardManager = GameObject.Find("Keyboard Manager");
        deathManager = GameObject.Find("Death Manager");
        player = GameObject.Find("Player");
        sceneManagement = GameObject.Find("Scene Management");
        itemLoader = GameObject.Find("Item Manager");
        objectiveManager = GameObject.Find("Objective Manager").GetComponent<ObjectiveManager>();
        gameStateManager = GameObject.Find("Game State Manager");
        tooltipManager = tooltipCanvas.GetComponent<TooltipManager>();
        foreach (Transform child in tooltipCanvas.GetComponentsInChildren<Transform>(true))
        {
            if (child.gameObject.name.Equals("Keybinds")) keybinds = child.gameObject;
            if (child.gameObject.name.Equals("Points")) pointsText = child.gameObject;
        }
        
    }
    
    
    
}
