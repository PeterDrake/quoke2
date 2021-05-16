using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// Handles movement of the player, including not moving through obstacles.
/// </summary>
public class PlayerMover : MonoBehaviour
{
    public GameObject Head;  // Used for our temporary crouch animation
    public float MovementSpeed;
    public Transform Destination;  // Point the player is current moving toward
    public bool Crouching;
    public bool UnderTable;

    private int interactableLayers;  // Player interacts with objects in these layers by moving into them
    private int obstacleLayers;  // Player cannot move into objects in these layers
    
    void Start()
    {
        interactableLayers = LayerMask.GetMask("NPC");
        obstacleLayers = LayerMask.GetMask("Wall", "NPC", "Table", "StorageContainer");
        Destination.parent = null; // So that moving player doesn't move its child Destination
    }

    /// <summary>
    /// Returns the GameObject, if any, that the player would hit if they took a step in direction.
    /// </summary>
    /// <param name="direction">A unit vector in the direction the player is facing</param>
    /// <param name="layers">Layers to check for objects</param>
    /// <returns></returns>
    private GameObject ObjectAhead(Vector3 direction, int layers)
    {
        // Get all things in the space on the grid the player is trying to move to
        Collider[] hitColliders = Physics.OverlapSphere(
            Destination.position + direction * 0.5f,
            0.2f, layers);
        if (hitColliders.Length == 1)  // There should be 0 or 1 such objects
        {
            // It's okay to move into (under) a table if you're crouching
            if (Crouching && hitColliders[0].gameObject.layer == LayerMask.NameToLayer("Table"))
            {
                return null;
            }
            return hitColliders[0].gameObject;
        }
        return null;
    }

    /// <summary>
    /// Starts moving in direction, unless the player is already moving (that is, far from Destination).
    /// </summary>
    public void StartMoving(Vector3 direction)
    {
        if (Vector3.Distance(transform.position, Destination.position) <= 0.05f)
        {
            // Is the player about to walk into an interactable object?
            GameObject ahead = ObjectAhead(direction, interactableLayers);
            if (ahead)  // TODO This seems to be assuming the only interactables are NPCs
            {
                GlobalControls.CurrentNPC = ahead.name;
                transform.LookAt(transform.position + direction,
                    transform.up);
                ahead.GetComponent<npcscript>().LoadScene("npcScreen");
            }
            // Is there an obstacle ahead?
            // Note that using the result of ObjectAhead as if it were a bool (using Unity's truthiness) is better
            // than checking for null, because destroyed GameObjects might not be null.
            else if (!ObjectAhead(direction, obstacleLayers)){
                Destination.position += direction;
            }
            transform.LookAt(transform.position + direction, transform.up);
        }
    }

    public void SetCrouching(bool crouching)
    {
        // If there's an obstacle where you are, it must be a table you're under
        Collider[] tableCheckColliders = Physics.OverlapSphere(transform.position, 0.2f, obstacleLayers);
        UnderTable = tableCheckColliders.Length != 0;
        this.Crouching = crouching || UnderTable;
        Head.transform.localPosition = new Vector3(Head.transform.localPosition.x, this.Crouching ? 0f : 0.5f,
                Head.transform.localPosition.z);
    }

    public void FixedUpdate()
    {
        transform.position =
            Vector3.MoveTowards(transform.position, Destination.position, MovementSpeed * Time.fixedDeltaTime);
    }
}