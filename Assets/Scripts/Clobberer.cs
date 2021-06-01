using System;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
///  When the player enters a collider with this script they will be killed
/// </summary>
public class Clobberer : MonoBehaviour
{
    // whether a clobberer is able to kill a player or not with a normal death message
    public new bool enabled;
    // whether a clobberer is able to kill a player with the aftershock death message
    public bool aftershock;

    private GameObject quakeEvent;
    private PlayerDeath playerDeathScript;
    private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        quakeEvent = GameObject.Find("Event Manager");
        player = GameObject.FindGameObjectWithTag("Player");
        playerDeathScript = player.GetComponent<PlayerDeath>();
        
        int quakeAtTurn = quakeEvent.GetComponent<QuakeManager>().turnsTillQuakeStart;
        int aftershockAtTurn = quakeEvent.GetComponent<QuakeManager>().turnsTillAftershock;
        
       
        if (enabled && (GlobalControls.TurnNumber > quakeAtTurn))
        {
            Debug.Log("You were hit by a door during the earthquake!");
            playerDeathScript.KillPlayer(this.gameObject, 3);

        }
        // if (aftershock && (GlobalControls.TurnNumber > aftershockAtTurn))
        // {
        //     Debug.Log("The house collapsed due to an after shock!");
        //     //Systems.Status.PlayerDeath("Aftershock", "The house collapsed due to an after shock!");
        //     // playerDeathScript.KillPlayer(this.gameObject, 0);
        //
        // }
    }
}
