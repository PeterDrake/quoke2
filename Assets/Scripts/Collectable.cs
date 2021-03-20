using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Sprite itemSprite;
    
    void Start()
    {

    }

    //REPLACE with OnTriggerEnter(Collider other) method to incorporate movement
    void Update()
    {
        //if B key is pressed, add a item to the inventory
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameObject.Find("Selector").GetComponent<Selector>().AddItemToInventory(this.gameObject);
        }
    }
}
