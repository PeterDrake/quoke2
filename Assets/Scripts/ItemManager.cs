using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject capsule1;
    
    // Start is called before the first frame update
    void Start()
    {
        capsule1.transform.position = GlobalControls.capsule1Location;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
