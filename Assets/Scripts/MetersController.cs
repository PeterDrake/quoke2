using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MetersController : MonoBehaviour
{
    
    private Meters meters;

    void Start()
    { 
        meters = new Meters();
       meters.UpdateVisualText();
    }

    //Used to check off meter on canvas & reset meter
    public void MarkTaskAsDone(string task)
    {
        meters.MarkTaskAsDone(task);
    }

    public void UpdateVisualText()
    {
        meters.UpdateVisualText();
    }

    public void setStartingValues()
    {
        meters.setStartingValues();
    }
}
