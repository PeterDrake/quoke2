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
