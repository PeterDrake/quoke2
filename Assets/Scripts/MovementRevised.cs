using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementRevised : MonoBehaviour
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
        layersToInteractWith = LayerMask.GetMask("NPC");  // Interactive layer masks  
        layersToCollideWith = LayerMask.GetMask("Wall", "NPC", "Table");  // These are the layers that we collide with 
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
    private GameObject IntractableInDirectionVertical(float x, float z)  //new code 
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



    // Update is called once per frame
    void Update()
    {
        // Movement
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (Math.Abs(Input.GetAxisRaw("Horizontal")) >= 1f)
            {
                //We check for the npc object in Horizontal Direction then we load npcScreen 
                GameObject npc = IntractableInDirectionHorizontal(Input.GetAxisRaw("Horizontal"), 0.0f);
                if (npc != null)
                {
                    transform.LookAt(transform.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), transform.up);
                    npc.GetComponent<npcscript>().LoadScene("npcScreen");
                }

                // Check if something occupying the space
                else if (ObstacleInDirectionHorizontal(Input.GetAxisRaw("Horizontal"), 0.0f))
                {
                    transform.LookAt(transform.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), transform.up);
                  
                }
                else
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    transform.LookAt(transform.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), transform.up);
                    moving = true;
                    GlobalControls.TurnNumber++;
                }
            }
           
            
            else if (Math.Abs(Input.GetAxisRaw("Vertical")) >= 1f)
            {
                //We check for the npc object in Vertical Direction then we load npcScreen 
                GameObject npc = IntractableInDirectionVertical(Input.GetAxisRaw("Vertical"), 0.0f);
                if (npc != null) 
                {
                    transform.LookAt(transform.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), transform.up);
                    npc.GetComponent<npcscript>().LoadScene("npcScreen");
                }
                // Check if something occupying the space
                else if (ObstacleInDirectionVertical(Input.GetAxisRaw("Vertical"),0.0f))
                {
                    transform.LookAt(transform.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), transform.up);
                   
                }
                else 
                {
                    movePoint.position += new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));
                    transform.LookAt(transform.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), transform.up);
                    moving = true;
                    GlobalControls.TurnNumber++;
                }
            }
            else
            {
                moving = false;
            }
        }
        
        Collider[] tableCheckColliders = Physics.OverlapSphere(transform.position, 0.2f, layersToCollideWith);
        
        // Tell us if the player is currently under a table
        if (tableCheckColliders.Length != 0)
        {
            isUnderTable = true;
        }
        else
        {
            isUnderTable = false;
        }

        // Tell us if the player is crouching by holding the key, or forced to because they are under a table
        if (Input.GetKey(KeyCode.C) || tableCheckColliders.Length != 0)
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