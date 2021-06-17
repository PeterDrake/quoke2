using UnityEngine;

/// <summary>
/// This class holds references to various objects within the scene. It would be possible to do this directly
/// when those objects are needed, but doing this reliably for (e.g.) objects that are disabled at the beginning
/// of a scene is nontrivial. The class encapsulates all of that ugliness.
/// </summary>
public class ReferenceManager : MonoBehaviour
{

    public GameObject canvases;
    public GameObject dialogueCanvas;
    public GameObject tradeCanvas;
    public GameObject inventoryCanvas;
    public GameObject player;
    public GameObject keyboardManager;
    public GameObject meters;
    
    private void Awake()
    {
        canvases = GameObject.Find("Canvases");
        keyboardManager = GameObject.Find("Keyboard Manager");
        dialogueCanvas = canvases.GetComponentInChildren<DialogueManager>(true).gameObject;
        tradeCanvas = canvases.GetComponentInChildren<TradeManager>(true).gameObject;
        player = GameObject.Find("Player");
        meters = GameObject.Find("Meters Canvas");
        foreach (Inventory i in canvases.GetComponentsInChildren<Inventory>(true))
        {
            if (i.gameObject.name.Equals("Inventory Canvas"))
            {
                inventoryCanvas = i.gameObject;
                break;
            }
        }
    }
    
}