using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemLoader : MonoBehaviour
{
    private Inventory inventory;
    
    //weknowwhatstartdoesthx
    void Start()
    {
        //can i keep it?
        inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        foreach (Item item in GlobalItemList.ItemList.Values)
        {
            if (item.scene.Equals(SceneManager.GetActiveScene()))
            {
                //poot item here
                GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(item.prefab, typeof(GameObject));
                prefab.transform.position = item.location;
            } 
            else if (item.scene.Equals("Inventory"))
            {
                //populate inventory with many things
                GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(item.prefab, typeof(GameObject));
                inventory.PickUpAtSlot((int) item.location.x, prefab);
            }
        }
    }

    
}
