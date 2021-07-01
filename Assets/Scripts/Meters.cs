using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Meters : MonoBehaviour
{
    public int poopTimeLeft;
    public int waterTimeLeft;

    public GameObject poopDoneIndicator;
    public GameObject waterDoneIndicator;

    public Text poopLevelDisplay;
    public Text waterLevelDisplay;

    private GameObject objectives;
    

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Contains("Quake"))
        {
            GlobalControls.MetersEnabled = false;
        }
        else
        {
            GlobalControls.MetersEnabled = true;
        }
        poopTimeLeft = GlobalControls.PoopTimeLeft;
        waterTimeLeft = GlobalControls.WaterTimeLeft;

        poopDoneIndicator.SetActive(GlobalControls.PoopTaskCompleted);
        waterDoneIndicator.SetActive(GlobalControls.WaterTaskCompleted);
        
        foreach (Image image in GameObject.Find("Managers").GetComponent<ReferenceManager>().tooltipCanvas
            .GetComponentsInChildren<Image>(true))
        {
            if (image.gameObject.name.Equals("Objectives"))
            {
                objectives = image.gameObject;
            }
        }
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

        Debug.Log("objectives");
        if (GlobalControls.ObjectivesEnabled && GlobalControls.WaterTaskCompleted && GlobalControls.PoopTaskCompleted)
        {
            GlobalControls.ObjectivesEnabled = true;
            objectives.SetActive(true);
            objectives.GetComponentInChildren<Text>(true).text = "Go to the tent to end the game!";
        }
        else
        {
            objectives.SetActive(false);
        }
    }

    private void UpdateVisualText()
    {
        poopLevelDisplay.text = poopTimeLeft.ToString();
        waterLevelDisplay.text = waterTimeLeft.ToString();
        if (GlobalControls.ObjectivesEnabled && GlobalControls.WaterTaskCompleted && GlobalControls.PoopTaskCompleted)
        {
            objectives.SetActive(true);
            objectives.GetComponentInChildren<Text>(true).text = "Go to the tent to end the game!";
        }
        else
        {
            objectives.SetActive(false);
        }
    }

}
