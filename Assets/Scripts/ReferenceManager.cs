using UnityEngine;

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
    
    public GameObject player;
    public GameObject keyboardManager;
    
    
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
        }
        keyboardManager = GameObject.Find("Keyboard Manager");
        //dialogueCanvas = canvases.GetComponentInChildren<DialogueManager>(true).gameObject;
        //tradeCanvas = canvases.GetComponentInChildren<TradeManager>(true).gameObject;
        player = GameObject.Find("Player");
        //metersCanvas = GameObject.Find("Meters Canvas");
        /*foreach (Inventory i in canvases.GetComponentsInChildren<Inventory>(true))
        {
            if (i.gameObject.name.Equals("Inventory Canvas"))
            {
                inventoryCanvas = i.gameObject;
                break;
            }
        }*/
    }
    
}
