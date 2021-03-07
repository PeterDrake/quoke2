using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    private GameObject player;
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player").gameObject;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            Debug.Log("pick me");
            other.GetComponent<BasicGridMovement>().collectables.Add(this.gameObject);
            gameObject.SetActive(false);

        }
    }

    public void DropItem()
    {
        GameObject player = GameObject.FindWithTag("Player");
        transform.position = player.transform.position + player.transform.forward;
        player.GetComponent<BasicGridMovement>().collectables.Remove(this.gameObject);
    }
}
