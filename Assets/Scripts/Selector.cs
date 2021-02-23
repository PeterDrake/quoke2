using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Selector : MonoBehaviour
{
    public Sprite oldSprite;
    public Sprite newSprite;

    void ChangeSprite(Sprite sprite)
    {
        oldSprite = sprite; 
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Slot 1 selected
    }

    // Update is called once per frame
    void Update()
    {
        // If user presses 1, select this slot
        // Else if user presses 2 - n, select that other slot
        if(Input.GetKey(KeyCode.Alpha1))
        {
            ChangeSprite(newSprite);
        }
    }

   

}