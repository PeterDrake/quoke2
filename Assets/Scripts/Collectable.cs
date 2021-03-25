using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Sprite itemSprite;
    
    void Start()
    {

    }

    /*
     * The Collectable script should be attached to any object you want to be an inventory item.
     * Both the player and the item must have a collider, and the item should also have a Rigidbody.
     * Make sure to uncheck the "Use Gravity" box in the Rigidbody options or else the item will fall
     */

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter is called");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ran into item");
            GameObject.Find("Selector").GetComponent<Selector>().AddItemToInventory(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        
    }
}
