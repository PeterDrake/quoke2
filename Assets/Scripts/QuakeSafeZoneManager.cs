using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuakeSafeZoneManager : MonoBehaviour
{
    public bool playerInSafeZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInSafeZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInSafeZone = false;
        }
    }
}
