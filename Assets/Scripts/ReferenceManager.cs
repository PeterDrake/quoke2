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
    public GameObject pointsText;

    public GameObject itemLoader;
    
    private void Awake()
    {
        canvases = GameObject.Find("Canvases");

        foreach (Canvas canvas in canvases.GetComponentsInChildren<Canvas>(true))
        {
            if (canvas.gameObject.name.Equals("Inventory Canvas"))
            {
                inventoryCanvas = canvas.gameObject;
            }
            else if (canvas.gameObject.name.Equals("Meters Canvas"))
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
        }

        keyboardManager = GameObject.Find("Keyboard Manager");
        deathManager = GameObject.Find("Death Manager");
        player = GameObject.Find("Player");
        sceneManagement = GameObject.Find("Scene Management");
        itemLoader = GameObject.Find("Item Manager");
        objectiveManager = GameObject.Find("Objective Manager").GetComponent<ObjectiveManager>();
        foreach (Transform child in tooltipCanvas.GetComponentsInChildren<Transform>(true))
        {
            if (child.gameObject.name.Equals("Keybinds")) keybinds = child.gameObject;
            if (child.gameObject.name.Equals("Points")) pointsText = child.gameObject;
        }
        
    }
    
    
    
}
