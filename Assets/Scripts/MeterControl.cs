using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterControl : MonoBehaviour
{
    public bool metersEnabled = true;

    public bool HasWaterStored;
    // 12 with stored water, 3 without
    private int StoredWaterTime = 12;

    public int totalTimeInHours;
    public float HourEquivalent = 1;
    public int poopTimeLeft;
    public int waterTimeLeft;

    public GameObject PoopTaskCheck;
    public GameObject WaterTaskCheck;

    public Text PoopLevelDisplay;
    public Text WaterLevelDisplay;

    void Start()
    {
        if (metersEnabled)
        {
            poopTimeLeft = GlobalControls.PoopTimeLeft - 1;
            waterTimeLeft = GlobalControls.WaterTimeLeft - 1;
            GlobalControls.PoopTimeLeft = poopTimeLeft;
            GlobalControls.WaterTimeLeft = waterTimeLeft;
            /*if (HasWaterStored)
            {
                waterTimeLeft = StoredWaterTime;
            }*/
            PoopTaskCheck.SetActive(GlobalControls.PoopTaskCompleted);
            WaterTaskCheck.SetActive(GlobalControls.WaterTaskCompleted);
            UpdateVisualText();
            //StartCoroutine(nameof(MeterCountdown));
        }
    }

    //Method to run meters down using HourEquivalent variable to convert realtime "seconds" into playtime "hours"
    private IEnumerator MeterCountdown(){
        while (totalTimeInHours > 0)
        {
            yield return new WaitForSeconds(HourEquivalent);
            if (!GlobalControls.PoopTaskCompleted && poopTimeLeft > 0)
            {
                poopTimeLeft--;
            }
            if (!GlobalControls.WaterTaskCompleted && waterTimeLeft > 0)
            {
                waterTimeLeft--;
            }
            totalTimeInHours--;
            UpdateVisualText();
        }
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
