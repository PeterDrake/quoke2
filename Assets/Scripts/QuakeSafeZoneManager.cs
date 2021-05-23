using UnityEngine;

public class QuakeSafeZoneManager : MonoBehaviour
{
    public bool playerInSafeZone;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInSafeZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInSafeZone = false;
        }
    }
}
