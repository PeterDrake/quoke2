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

    private void OnCollisionEnter(Collision other)
    {
        if (enabled && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("You were hit by a door!");
            //Systems.Status.PlayerDeath("Hit by a door", "You were hit by a door!");

        }
        if (aftershock && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("The house collapsed due to an after shock!");
            //Systems.Status.PlayerDeath("Aftershock", "The house collapsed due to an after shock!");

        }
    }
}
