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
    
    private void Awake()
    {
        canvases = GameObject.Find("Canvases");
        dialogueCanvas = canvases.GetComponentInChildren<NPCScreenInteractor>(true).gameObject;
        tradeCanvas = canvases.GetComponentInChildren<TradingManager>(true).gameObject;
        foreach (Inventory i in canvases.GetComponentsInChildren<Inventory>(true))
        {
            if (i.name.Equals("Inventory Canvas"))
            {
                inventoryCanvas = i.gameObject;
                break;
            }
        }
    }
    
}
