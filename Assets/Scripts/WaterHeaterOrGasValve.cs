using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// controls actions for Safi
/// </summary>
public class WaterHeaterOrGasValve : MonoBehaviour
{

    private ReferenceManager referenceManager;
    private PlayerMover player;
    private LayerMask gasValve, waterHeater; 
    private GameObject npcCanvas;


    // Start is called before the first frame update
    void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        player = referenceManager.player.GetComponent<PlayerMover>();
        npcCanvas = referenceManager.npcInteractedCanvas;
        gasValve = LayerMask.GetMask("GasValve");
        waterHeater = LayerMask.GetMask("WaterHeater");

    }

    private void Update()
    {
        CheckSafiActions(); //Checks for player input in order to complete the two Safi actions with wrench
    }

    public void CheckSafiActions()
    {
        if (Input.GetKeyDown(KeyCode.Space) && HasWrench())
        {
            if (GlobalControls.globalControlsProperties.Contains("safiAsksForGasHelp"))
            {
                UpdateInteractables("Gas", gasValve);
            }
            if (GlobalControls.globalControlsProperties.Contains("safiAsksForWaterHelp"))
            {
                UpdateInteractables("Heater", waterHeater);
            }
        }
    }

    public void UpdateInteractables(string interactable ,LayerMask layer)
    {
        if (player.ObjectAhead(layer) && (!GlobalControls.globalControlsProperties.Contains("safi" + interactable+ "Done")))
        {
            GlobalControls.globalControlsProperties.Add("safi" + interactable + "Done");
            GlobalControls.npcList["Safi"].satisfaction++;
            GlobalControls.CurrentPoints += GlobalControls.Points["favors"];
            npcCanvas.GetComponent<NPCInteracted>().UpdateNPCInteracted("Safi");
            referenceManager.pointsText.GetComponentInChildren<Text>().text = GlobalControls.CurrentPoints.ToString();

        }
    }
    

    

    private bool HasWrench()
    {
        if (GlobalItemList.ItemList["Wrench"].containerName.Equals("Player"))
        {
            GlobalControls.globalControlsProperties.Add("playerHasWrench");
            return true;
        }
        return false;

    }
}


