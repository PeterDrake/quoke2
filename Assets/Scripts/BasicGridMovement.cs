using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// See https://www.youtube.com/watch?v=mbzXIOKZurA on grid movement

public class BasicGridMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Transform movePoint;
    public bool isMoving;

    static string[] collisionLayers = {"Wall"};
    int layersToCollideWith;

    public GameObject visualCheck;
    private int numberOfCollidersFound;

    // Start is called before the first frame update
    void Start()
    {
        layersToCollideWith = LayerMask.GetMask(collisionLayers);
        Debug.Log(layersToCollideWith);
        isMoving = false;
        movePoint.parent = null; // So that moving player doesn't move its child movePoint
        visualCheck.transform.position = transform.position;
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
                numberOfCollidersFound = findColliders("Horizontal");
                moveHorizontally();           
            }
            else if (Math.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                numberOfCollidersFound = findColliders("Vertical");
                moveVertically(); 
            }
            else
            {
                isMoving = false;
            }
        }
    }

    //returns number of colliders stored in hitCollider array while using OverlapSphere function in specific direction
    int findColliders(string direction)
    {
        Collider[] hitColliders = null;

        if (direction == "Horizontal")
        {   
            hitColliders = Physics.OverlapSphere(movePoint.position + 
                new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .5f, layersToCollideWith);

            //visually moves greensphere to show location of OverlapSphere function above
            visualCheck.transform.position = movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        }

        else if (direction == "Vertical")
        {
            hitColliders = Physics.OverlapSphere(movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")),
                    0.5f, layersToCollideWith);

            //visually moves greensphere to shows location of OverlapSphere function above
            visualCheck.transform.position = movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));
        }
        return hitColliders.Length;    

    }

    // Will face direction of movement & if no colliders are found in direction, will update movePoint to new positioning in horizontal direction
    void moveHorizontally()
    {
        if (numberOfCollidersFound == 0)
        {
            movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
            isMoving = true;
        }
        transform.LookAt(transform.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), transform.up);
    }

    // Will face direction of movement & if no colliders are found in direction, will update movePoint to new positioning in vertical direction
    void moveVertically()
    {
        if (numberOfCollidersFound == 0)
        {
            movePoint.position += new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));
            isMoving = true;
        }
        transform.LookAt(transform.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), transform.up);
    }
}