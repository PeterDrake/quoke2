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

    public LayerMask layersToCollideWith;
    public LayerMask layersToCollectFrom;

    public GameObject visualCheck;
    private Collider[] hitColliders;
    private int numberOfCollidersFound;
    private Collider[] hitCollectables;
    private int numberofCollectablesFound;

    public List<GameObject> CollectedItems;



    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        movePoint.parent = null; // So that moving player doesn't move its child movePoint
        visualCheck.transform.position = transform.position;
        hitColliders = null;
        hitCollectables = null;
        CollectedItems = new List<GameObject>();
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
                numberofCollectablesFound = findCollectables("Horizontal");
                // Debug.Log("Colliders = " + numberOfCollidersFound + " Collectables = " + numberofCollectablesFound);

                moveHorizontally();           
            }
            else if (Math.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                numberOfCollidersFound = findColliders("Vertical");
                numberofCollectablesFound = findCollectables("Vertical");
                // Debug.Log("Colliders = " + numberOfCollidersFound + " Collectables = " + numberofCollectablesFound);
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

    //returns number of colliders stored in hitCollectables array
    int findCollectables(string direction)
    {
        if (direction == "Horizontal")
        {   
            hitCollectables = Physics.OverlapSphere(movePoint.position + 
                new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .5f, layersToCollectFrom);
        }
        if (direction == "Vertical")
        {
            hitCollectables = Physics.OverlapSphere(movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")),
                    0.5f, layersToCollectFrom);
        }
        return hitCollectables.Length;  
    }

    

    // Will face direction of movement & if no colliders are found in direction, will update movePoint to new positioning in horizontal direction
    // if a collectable is infront will add it to CollectedItems List and deactivate and then move to that spot if able
    void moveHorizontally()
    {
        if (numberofCollectablesFound > 0)
        {
            foreach (Collider element in hitCollectables)
            {
                CollectedItems.Add(element.gameObject);
                element.gameObject.SetActive(false);
            }
        }
        if (numberOfCollidersFound == 0)
        {
            movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
            isMoving = true;
        }
        transform.LookAt(transform.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), transform.up);
    }

    // Will face direction of movement & if no colliders are found in direction, will update movePoint to new positioning in vertical direction
    // if a collectable is infront will add it to CollectedItems List and deactivate and then move to that spot if able
    void moveVertically()
    {
        if (numberofCollectablesFound > 0)
        {
            foreach (Collider element in hitCollectables)
            {
                CollectedItems.Add(element.gameObject);
                element.gameObject.SetActive(false);
            }
        }
        if (numberOfCollidersFound == 0)
        {
            movePoint.position += new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));
            isMoving = true;
        }
        transform.LookAt(transform.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), transform.up);
    }
}