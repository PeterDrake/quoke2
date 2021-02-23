using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Slot1 : MonoBehaviour
{
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
    }

    private void textBox1_KeyPress(object sender, KeyDownEvent e)
    {
        if (e.keyCode == KeyCode.Alpha1)
        {
            
        }
    }

}
