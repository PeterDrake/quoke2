using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Meters : MonoBehaviour
{
    public int poopTimeLeft;
    public int waterTimeLeft;

    public bool isStrategicMap = false;

    public GameObject poopDoneIndicator;
    public GameObject waterDoneIndicator;

    public Text poopLevelDisplay;
    public Text waterLevelDisplay;

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
        poopDoneIndicator.SetActive(GlobalControls.PoopTaskCompleted);
        waterDoneIndicator.SetActive(GlobalControls.WaterTaskCompleted);
        UpdateVisualText();
    }

    //Used to check off meter on canvas & reset meter
    public void MarkTaskAsDone(string task)
    {
        if (task == "poop")
        {
            GlobalControls.PoopTaskCompleted = true;
            poopDoneIndicator.SetActive(true);
            
        }
        if (task == "water")
        {
            GlobalControls.WaterTaskCompleted = true;
            waterDoneIndicator.SetActive(true);
        }
    }

    private void UpdateVisualText()
    {
        poopLevelDisplay.text = poopTimeLeft.ToString();
        waterLevelDisplay.text = waterTimeLeft.ToString();
    }

}
