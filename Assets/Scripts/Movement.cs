using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Movement : MonoBehaviour
{
    public GameObject playerHead;

    public float moveSpeed = 5f;
    public Transform movePoint;
    public bool moving;
    public bool crouching;
    public bool isUnderTable;

    int layersToCollideWith;
    int layersToInteractWith;
    
    // Start is called before the first frame update
    void Start()
    {

        layersToInteractWith = LayerMask.GetMask("NPC");
        layersToCollideWith = LayerMask.GetMask("Wall", "NPC", "Table", "StorageContainer");

        moving = false;
        movePoint.parent = null; // So that moving player doesn't move its child movePoint
    }

    // ///Returns true if there is an obstacle in direction
    // private bool ObstacleAhead(Vector3 direction)
    // {
    //     // Get all things in the space on the grid the player is trying to move to
    //     Collider[] hitColliders = Physics.OverlapSphere(
    //         movePoint.position + direction * 0.5f,
    //         0.2f, layersToCollideWith);
    //     Debug.Log("Obstacle hitColliders: " + hitColliders.Length);
    //     // Exception if the player is crouching and the object in front is a table
    //     if (hitColliders.Length == 1)
    //     {
    //         if (hitColliders[0].gameObject.layer == 10 && crouching)
    //         {
    //             return false;
    //         }
    //     }
    //
    //     return hitColliders.Length != 0;
    // }

    /// Returns the GameObject, if any, that the player would run into if they took a step in direction.
    private GameObject InteractableAhead(Vector3 direction, int layers) 
    {
        // Get all things in the space on the grid the player is trying to move to
        Collider[] hitColliders = Physics.OverlapSphere(
            movePoint.position + direction * 0.5f,
            0.2f, layers);
        Debug.Log("Interactable hitColliders: " + hitColliders.Length);
        // Exception if the player is crouching and the object in front is a table
        if (hitColliders.Length == 1)
        {
            if (hitColliders[0].gameObject.layer == 10 && crouching)
            {
                return null;
            }

            return hitColliders[0].gameObject;
        }

        return null;
    }

    public void MoveHorizontally(Vector3 direction)
    {
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            //We check for the npc object in Horizontal Direction then we load npcScreen 
            GameObject npc = InteractableAhead(direction, layersToInteractWith);
            Debug.Log("Ahead: <" + npc + ">");
            if (npc != null)
            {
                GlobalControls.CurrentNPC = npc.name;
                transform.LookAt(transform.position + direction,
                    transform.up);
                npc.GetComponent<npcscript>().LoadScene("npcScreen");
            }

            // Check if something occupying the space
            else if (InteractableAhead(direction, layersToCollideWith)) // Checks for null, taking Unity's weird idea of null into account
            {
                transform.LookAt(transform.position + direction,
                    transform.up);

            }
            else
            {
                movePoint.position += direction;
                transform.LookAt(transform.position + direction,
                    transform.up);
                moving = true;
                GlobalControls.TurnNumber++;
            }
        }
    }

    public void SetMoving(bool moving)
    {
        this.moving = moving;
    }

    public void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.fixedDeltaTime);
        
        Collider[] tableCheckColliders = Physics.OverlapSphere(transform.position, 0.2f, layersToCollideWith);
        
        // Tell us if the player is currently under a table
        isUnderTable = tableCheckColliders.Length != 0;

        // Tell us if the player is crouching by holding the key, or forced to because they are under a table
        if (Input.GetKey(KeyCode.C) || isUnderTable)
        {
            crouching = true;
            playerHead.transform.localPosition = new Vector3(playerHead.transform.localPosition.x, 0f, playerHead.transform.localPosition.z);
        }
        else
        {
            crouching = false;
            playerHead.transform.localPosition = new Vector3(playerHead.transform.localPosition.x, 0.5f, playerHead.transform.localPosition.z);
        }
    }
}