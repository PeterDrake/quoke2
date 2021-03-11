using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OpenNPCScreen : MonoBehaviour
{

    public Button button1;
    public Button button2;
    public Button button3;
    public int cursorLocation = 0;
    public string[] textArray;

    public Button[] buttons;
    
        // Start is called before the first frame update
        private void Start()
        {
            buttons = new Button[] {button1, button2, button3};
        }

        void Update()
    {
        
        // Change the cursor's location with < and >
        if (Input.GetKeyDown(","))
        {
            cursorLocation--;
        }
        if (Input.GetKeyDown("."))
        {
            cursorLocation++;
        }
        if (cursorLocation < 0)
        {
            cursorLocation = buttons.Length - 1;
        }
        else if (cursorLocation > buttons.Length - 1)
        {
            cursorLocation = 0;
        }

        buttons[cursorLocation].Select();
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
          
        }
        
        if (Input.GetKeyDown("space"))
        {

        }
        
        //we can use the below comment to display text on our buttons through an array of strings,
        //this will be really 
        
        // button1.GetComponentInChildren<Text>().text = textArray[0];
       

    }

   
    
}
