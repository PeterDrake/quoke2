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
    
    public QuakeSafeZoneManager quakeSafeZoneManager;
    
    private PlayerDeath playerDeathScript;
    private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerDeathScript = player.GetComponent<PlayerDeath>();
        quakeSafeZoneManager = quakeSafeZoneManager.GetComponent<QuakeSafeZoneManager>();
        
        if (enabled && !quakeSafeZoneManager.playerInSafeZone)
        {
            Debug.Log("You were hit by a door during the earthquake!");
            playerDeathScript.KillPlayer(this.gameObject, 3);

        }
        if (aftershock && quakeSafeZoneManager.playerInSafeZone)
        {
            Debug.Log("You tried entering and the house collapsed due to an after shock!");
            playerDeathScript.KillPlayer(this.gameObject, 6);
        }
    }
}
