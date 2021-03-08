using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    private LayerMask layers;
    void Start()
    {
        layers = LayerMask.GetMask("Collectables");
    }

    //Used to pick up items. When collided with the player will pick up object and add it to collectable list in movement script
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            Debug.Log("picked up item");
            other.GetComponent<BasicGridMovement>().collectables.Add(this.gameObject);
            gameObject.SetActive(false);
        }
    }

    //Used to drop items. Will drop behind, if object is already there will drop in front of player 
    public void DropItem()
    {       
        Debug.Log("dropped off item");
        GameObject player = GameObject.FindWithTag("Player");
        Collider[] objectsInWay = Physics.OverlapSphere(player.transform.position + (player.transform.forward * -1), 0.4f, layers);
        if (objectsInWay.Length > 0){
            // drops item in front of player
            transform.position = player.transform.position + (player.transform.forward);
            player.GetComponent<BasicGridMovement>().collectables.Remove(this.gameObject);
        }
        else 
        {
            //drops item behind player
            transform.position = player.transform.position + (player.transform.forward*-1);
            player.GetComponent<BasicGridMovement>().collectables.Remove(this.gameObject);
        }

    }
}
