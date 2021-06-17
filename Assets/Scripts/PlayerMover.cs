using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

/// <summary>
/// Handles movement of the player, including not moving through obstacles.
/// </summary>
public class PlayerMover : MonoBehaviour
{
    public GameObject head;  // Used for our temporary crouch animation
    public float movementSpeed;
    public Transform destination;  // Point the player is current moving toward
    public bool crouching;
    public bool underTable;
    
    public DialogueManager interactor;

    private int interactableLayers;  // Player interacts with objects in these layers by moving into them
    private int obstacleLayers;  // Player cannot move into objects in these layers
    private ReferenceManager referenceManager;
    
    void Start()
    {
        interactableLayers = LayerMask.GetMask("NPC");
        obstacleLayers = LayerMask.GetMask("Wall", "NPC", "Table", "StorageContainer");
        destination.parent = null; // So that moving player doesn't move its child Destination
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
    }

    /// <summary>
    /// Returns the GameObject, if any, that the player would hit if they took a step in direction.
    /// </summary>
    /// <param name="layers">Layers to check for objects</param>
    /// <returns></returns>
    public GameObject ObjectAhead(int layers)
    {
        Vector3 direction = transform.forward;
        // Get all things in the space on the grid the player is trying to move to
        Collider[] hitColliders = Physics.OverlapSphere(destination.position + direction, 0.2f, layers);
        if (hitColliders.Length == 1)  // There should be 0 or 1 such objects
        {
            // It's okay to move into (under) a table if you're crouching
            if (crouching && hitColliders[0].gameObject.layer == LayerMask.NameToLayer("Table"))
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
        if (Vector3.Distance(transform.position, destination.position) <= 0.05f)
        {
            transform.LookAt(transform.position + direction, transform.up);
            // Is the player about to walk into an interactable object?
            GameObject ahead = ObjectAhead(interactableLayers);
            
            // TODO This seems to be assuming the only interactables are NPCs
            if (ahead)
            {
                GlobalControls.CurrentNPC = ahead.name;
                transform.LookAt(transform.position + direction, transform.up);
                referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>().SetConversing();
            }
            // Is there an obstacle ahead?
            // Note that using the result of ObjectAhead as if it were a bool (using Unity's truthiness) is better
            // than checking for null, because destroyed GameObjects might not be null.
            else if (!ObjectAhead(obstacleLayers)){
                destination.position += direction;
                GlobalControls.TurnNumber++;
            }
        }
    }

    public void SetCrouching(bool crouching)
    {
        // If there's an obstacle where you are, it must be a table you're under
        Collider[] tableCheckColliders = Physics.OverlapSphere(transform.position, 0.2f, obstacleLayers);
        underTable = tableCheckColliders.Length != 0;
        this.crouching = crouching || underTable;
        head.transform.localPosition = new Vector3(head.transform.localPosition.x, this.crouching ? 0f : 0.5f,
                head.transform.localPosition.z);
    }

    void FixedUpdate()
    {
        transform.position =
            Vector3.MoveTowards(transform.position, destination.position, movementSpeed * Time.fixedDeltaTime);
    }
}