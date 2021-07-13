using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Meters : MonoBehaviour
{
    public int poopTimeLeft;
    public int waterTimeLeft;

    private GameObject poopDoneIndicator;
    private GameObject waterDoneIndicator;

    private Image poopDoneImage;
    private Image waterDoneImage;
    private Image poopProgressFill;
    private Image waterProgressFill;
    private GameObject poopProgressObject;
    private GameObject waterProgressObject;
    
    private Text poopLevelNumber;
    private Text waterLevelNumber;

    private ReferenceManager referenceManager;


    void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        
        if (SceneManager.GetActiveScene().name.Contains("Quake"))
        {
            GlobalControls.MetersEnabled = false;
        }
        else
        {
            GlobalControls.MetersEnabled = true;
        }
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.name.Equals("Poop Value")) poopLevelNumber = child.gameObject.GetComponent<Text>();
            else if (child.name.Equals("Water Value")) waterLevelNumber = child.gameObject.GetComponent<Text>();
            else if (child.name.Equals("Poop Done")) poopDoneIndicator = child.gameObject;
            else if (child.name.Equals("Water Done")) waterDoneIndicator = child.gameObject;
            else if (child.name.Equals("Poop Back")) poopDoneImage = child.gameObject.GetComponent<Image>();
            else if (child.name.Equals("Water Back")) waterDoneImage = child.gameObject.GetComponent<Image>();
            else if (child.name.Equals("Poop Progress")) poopProgressObject = child.gameObject;
            else if (child.name.Equals("Water Progress")) waterProgressObject = child.gameObject;
            else if (child.name.Equals("Poop Background")) poopProgressFill = child.gameObject.GetComponent<Image>();
            else if (child.name.Equals("Water Background")) waterProgressFill = child.gameObject.GetComponent<Image>();
        }
        
        poopTimeLeft = GlobalControls.PoopTimeLeft;
        waterTimeLeft = GlobalControls.WaterTimeLeft;

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
        
        if (GlobalControls.ObjectivesEnabled && GlobalControls.WaterTaskCompleted && GlobalControls.PoopTaskCompleted)
        {
            GlobalControls.CurrentObjective = 5;
            GameObject.Find("Managers").GetComponent<ReferenceManager>().objectiveManager.UpdateObjectiveBanner();
        }
        
        UpdateVisualText();
    }

    private void UpdateVisualText()
    {
        Debug.Log("Updating visual text");
        poopLevelNumber.text = poopTimeLeft.ToString();
        waterLevelNumber.text = waterTimeLeft.ToString();

        if (GlobalControls.PoopTaskCompleted)
        {
            poopDoneIndicator.SetActive(true);
            poopDoneImage.color = Color.yellow;
            poopProgressFill.color = Color.yellow;
        }
        else
        {
            poopDoneIndicator.SetActive(false);
            poopDoneImage.color = Color.white;
            poopProgressFill.color = Color.white;
        }

        if (GlobalControls.WaterTaskCompleted)
        {
            waterDoneIndicator.SetActive(true);
            waterDoneImage.color = Color.yellow;
            waterProgressFill.color = Color.yellow;
        }
        else
        {
            waterDoneIndicator.SetActive(false);
            waterDoneImage.color = Color.white;
            waterProgressFill.color = Color.white;
        }
        
        poopProgressObject.GetComponent<Slider>().value = poopTimeLeft / 24f;
        waterProgressObject.GetComponent<Slider>().value = waterTimeLeft / 24f;
        
        if (GlobalControls.ObjectivesEnabled && GlobalControls.WaterTaskCompleted && GlobalControls.PoopTaskCompleted)
        {
            GlobalControls.CurrentObjective = 6;
            referenceManager.objectiveManager.UpdateObjectiveBanner();
        }
    }

}
