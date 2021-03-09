using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// See https://www.youtube.com/watch?v=mbzXIOKZurA on grid movement

public class BasicGridMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Transform movePoint;
    public bool moving;

    static string[] collisionLayers = {"Wall"};
    //make a new layer for NPC 
    int layersToCollideWith;


    // Start is called before the first frame update
    void Start()
    {
        layersToCollideWith = LayerMask.GetMask(collisionLayers);
        Debug.Log(layersToCollideWith);
        moving = false;
        movePoint.parent = null; // So that moving player doesn't move its child movePoint
    }

    private bool ObstacleInDirection(float x, float z)
    {
        // Get all things in the space on the grid the player is trying to move to
        Collider[] hitColliders = Physics.OverlapSphere(movePoint.position + new Vector3(x * 0.5f, 0.5f, z*0.5f),
            0.2f, layersToCollideWith);

        return hitColliders.Length != 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (Math.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                // If nothing is occupying that space
                if (!ObstacleInDirection(Input.GetAxisRaw("Horizontal"), 0.0f))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    transform.LookAt(transform.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), transform.up);
                    moving = true;
                }
                else
                {
                    transform.LookAt(transform.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), transform.up);
                }
            }
            else if (Math.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                // // If nothing is occupying that space
                if (!ObstacleInDirection(Input.GetAxisRaw("Vertical"), 0.0f))
                {
                    movePoint.position += new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));
                    transform.LookAt(transform.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), transform.up);
                    moving = true;
                }
                else
                {
                    transform.LookAt(transform.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), transform.up);
                }
            }
            else
            {
                moving = false;
            }
        }
    }
}