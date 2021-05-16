﻿using System;
using UnityEngine;

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

    ///Returns true if there is an obstacle in Horizontal Direction
    private bool ObstacleInDirectionHorizontal(float x, float z)
    {
        // Get all things in the space on the grid the player is trying to move to
        Collider[] hitColliders = Physics.OverlapSphere(
            movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal") * 0.5f, 0.0f, 0.0f),
            0.2f, layersToCollideWith);

        // Exception if the player is crouching and the object in front is a table
        if (hitColliders.Length == 1)
        {
            if (hitColliders[0].gameObject.layer == 10 && crouching)
            {
                return false;
            }
        }

        return hitColliders.Length != 0;
    }
    
    ///Returns true if there is an obstacle in Vertical Direction
    private bool ObstacleInDirectionVertical(float x, float z)
    {
        // Get all things in the space on the grid the player is trying to move to
        Collider[] hitColliders = Physics.OverlapSphere(movePoint.position + new Vector3(0f, 0.0f, Input.GetAxisRaw("Vertical") * 0.5f),
            0.2f, layersToCollideWith);

        // Exception if the player is crouching and the object in front is a table
        if (hitColliders.Length == 1)
        {
            if (hitColliders[0].gameObject.layer == 10 && crouching)
            {
                return false;
            }
        }

        return hitColliders.Length != 0;
    }
    
    /// This tells us about the interactive object that we are about to run in to. On the Horizontal direction
    private GameObject IntractableInDirectionHorizontal(float x, float z)  //new code 
    {
        // Get all things in the space on the grid the player is trying to move to
        Collider[] hitColliders = Physics.OverlapSphere(
            movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal") * 0.5f, 0.0f, 0.0f),
            0.2f, layersToInteractWith);

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
    
    /// This tells us about the interactive object that we are about to run in to. On the Vertical direction   
    private GameObject InteractableInDirectionVertical(float x, float z)  //new code 
    {
        // Get all things in the space on the grid the player is trying to move to
        Collider[] hitColliders = Physics.OverlapSphere(movePoint.position + new Vector3(0f, 0.0f, Input.GetAxisRaw("Vertical") * 0.5f),
            0.2f, layersToInteractWith);

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

    public void MoveHorizontally(float direction)
    {
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            //We check for the npc object in Horizontal Direction then we load npcScreen 
            GameObject npc = IntractableInDirectionHorizontal(direction, 0.0f);
            if (npc != null)
            {
                GlobalControls.CurrentNPC = npc.name;
                transform.LookAt(transform.position + new Vector3(direction, 0f, 0f),
                    transform.up);
                npc.GetComponent<npcscript>().LoadScene("npcScreen");
            }

            // Check if something occupying the space
            else if (ObstacleInDirectionHorizontal(direction, 0.0f))
            {
                transform.LookAt(transform.position + new Vector3(direction, 0f, 0f),
                    transform.up);

            }
            else
            {
                movePoint.position += new Vector3(direction, 0f, 0f);
                transform.LookAt(transform.position + new Vector3(direction, 0f, 0f),
                    transform.up);
                moving = true;
                GlobalControls.TurnNumber++;
            }
        }
    }

    public void MoveVertically(float direction)
    {
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {

            //We check for the npc object in Vertical Direction then we load npcScreen 
            GameObject npc = InteractableInDirectionVertical(direction, 0.0f);
            if (npc != null)
            {
                GlobalControls.CurrentNPC = npc.name;
                transform.LookAt(transform.position + new Vector3(0f, 0f, direction), transform.up);
                npc.GetComponent<npcscript>().LoadScene("npcScreen");
            }
            // Check if something occupying the space
            else if (ObstacleInDirectionVertical(direction,0.0f))
            {
                transform.LookAt(transform.position + new Vector3(0f, 0f, direction), transform.up);
               
            }
            else 
            {
                movePoint.position += new Vector3(0f, 0f, direction);
                transform.LookAt(transform.position + new Vector3(0f, 0f, direction), transform.up);
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