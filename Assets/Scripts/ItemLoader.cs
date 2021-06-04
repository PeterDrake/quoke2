using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemLoader : MonoBehaviour
{
    //weknowwhatstartdoesthx
    void Start()
    {
        foreach (Item item in GlobalItemList.ItemList.Values)
        {
            if (item.scene.Equals(SceneManager.GetActiveScene()))
            {
                //poot item here
                Object prefab = AssetDatabase.LoadAssetAtPath(item.prefab, typeof(GameObject));
                Instantiate(prefab, item.location, Quaternion.identity);
            } 
            else if (item.scene.Equals("Inventory"))
            {
                //populate inventory with many things
                
            }
        }
    }

    
}
