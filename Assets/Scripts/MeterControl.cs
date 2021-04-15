using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterControl : MonoBehaviour
{
    public int totalTimeInHours;
    public float HourEquivalent = 1;
    public int poopTimeLeft;
    public int waterTimeLeft;

    public bool isStrategicMap = false;

    public GameObject PoopTaskCheck;
    public GameObject WaterTaskCheck;

    public Text PoopLevelDisplay;
    public Text WaterLevelDisplay;

    void Start()
    {
        poopTimeLeft = GlobalControls.PoopTimeLeft;
        waterTimeLeft = GlobalControls.WaterTimeLeft;
        if (GlobalControls.MetersEnabled && !isStrategicMap)
        {
            if (!GlobalControls.PoopTaskCompleted)
            {
                poopTimeLeft--;
                GlobalControls.PoopTimeLeft = poopTimeLeft;
            }
            if (!GlobalControls.WaterTaskCompleted)
            {
                waterTimeLeft--;
                GlobalControls.WaterTimeLeft = waterTimeLeft;
            }
        }
        PoopTaskCheck.SetActive(GlobalControls.PoopTaskCompleted);
        WaterTaskCheck.SetActive(GlobalControls.WaterTaskCompleted);
        UpdateVisualText();
    }

    //Used to check off meter on canvas & reset meter
    public void CheckTaskAsDone(string task)
    {
        if (task == "poop")
        {
            GlobalControls.PoopTaskCompleted = true;
            PoopTaskCheck.SetActive(true);
            
        }
        if (task == "water")
        {
            GlobalControls.WaterTaskCompleted = true;
            WaterTaskCheck.SetActive(true);
        }
    }

    //Called when you need to update visual text on canvas
    private void UpdateVisualText()
    {
        PoopLevelDisplay.text = poopTimeLeft.ToString();
        WaterLevelDisplay.text = waterTimeLeft.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown("p") && !GlobalControls.PoopTaskCompleted)
        {
            CheckTaskAsDone("poop");
        }
        if (Input.GetKeyDown("o") && !GlobalControls.WaterTaskCompleted)
        {
            CheckTaskAsDone("water");
        }
    }
}
