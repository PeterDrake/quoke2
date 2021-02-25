using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterControl : MonoBehaviour
{
    public bool HasWaterStored;
    public int totalTimeInHours;
    public float HourEquivalent = 1;
    public int poopTimeLeft;
    public int waterTimeLeft = 3;
    private int StoredWaterTime = 12;
    public bool poopTaskCompleted;
    public GameObject PoopTaskCheck;
    public bool waterTaskCompleted;
    public GameObject WaterTaskCheck;

    public Text PoopLevelDisplay;
    public Text WaterLevelDisplay;

    void Start()
    {
        poopTimeLeft = totalTimeInHours;
        if (HasWaterStored)
        {
            waterTimeLeft = StoredWaterTime;
        }
        PoopTaskCheck.SetActive(false);
        WaterTaskCheck.SetActive(false);
        poopTaskCompleted = false;
        waterTaskCompleted = false;
        UpdateVisualText();
        StartCoroutine(nameof(MeterCountdown));
    }

    //Method to run meters down using HourEquivalent variable to convert realtime "seconds" into playtime "hours"
    private IEnumerator MeterCountdown(){
        while (totalTimeInHours > 0)
        {
            yield return new WaitForSeconds(HourEquivalent);
            if (!poopTaskCompleted && poopTimeLeft > 0)
            {
                poopTimeLeft--;
            }
            if (!waterTaskCompleted && waterTimeLeft > 0)
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
            poopTaskCompleted = true;
            PoopTaskCheck.SetActive(true);
            
        }
        if (task == "water")
        {
            waterTaskCompleted = true;
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
        if (Input.GetKeyDown("p") && !poopTaskCompleted)
        {
            CheckTaskAsDone("poop");
        }
        if (Input.GetKeyDown("o") && !waterTaskCompleted)
        {
            CheckTaskAsDone("water");
        }



    }



}
