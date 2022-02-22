using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// poop and water meter at the top of the screen
/// </summary>
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
            GlobalControls.globalControlsProperties.Remove("metersEnabled"); 
        }
        else
        {
            GlobalControls.globalControlsProperties.Add("metersEnabled");
        }
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.name.Equals("Poop Value") || child.name.Equals("Poop Value Text (TMP)")) poopLevelNumber = child.gameObject.GetComponent<Text>();
            else if (child.name.Equals("Water Value")) waterLevelNumber = child.gameObject.GetComponent<Text>();
            else if (child.name.Equals("Poop Done")) poopDoneIndicator = child.gameObject;
            else if (child.name.Equals("Water Done")) waterDoneIndicator = child.gameObject;
            else if (child.name.Equals("Poop Back") || child.name.Equals("Poop Icon Background")) poopDoneImage = child.gameObject.GetComponent<Image>();
            else if (child.name.Equals("Water Back")) waterDoneImage = child.gameObject.GetComponent<Image>();
            else if (child.name.Equals("Poop Progress") || child.name.Equals("Poop Fill Slider")) poopProgressObject = child.gameObject;
            else if (child.name.Equals("Water Progress")) waterProgressObject = child.gameObject;
            else if (child.name.Equals("Poop Background") || child.name.Equals("Poop Icon Mask")) poopProgressFill = child.gameObject.GetComponent<Image>();
            else if (child.name.Equals("Water Background")) waterProgressFill = child.gameObject.GetComponent<Image>();
        }
        
        poopTimeLeft = GlobalControls.poopTimeLeft;
        waterTimeLeft = GlobalControls.waterTimeLeft;

        poopDoneIndicator.SetActive(GlobalControls.globalControlsProperties.Contains("poopTaskCompleted"));
        waterDoneIndicator.SetActive(GlobalControls.globalControlsProperties.Contains("waterTaskCompleted"));

        UpdateVisualText();
    }
    
    //Used to check off meter on canvas & reset meter
    public void MarkTaskAsDone(string task)
    {
        if (task == "poop")
        {
            GlobalControls.globalControlsProperties.Add("poopTaskCompleted");
            poopDoneIndicator.SetActive(true);
        }
        if (task == "water")
        {
            GlobalControls.globalControlsProperties.Add("waterTaskCompleted");
            waterDoneIndicator.SetActive(true);
        }
        
        if (GlobalControls.globalControlsProperties.Contains("objectivesEnabled") && 
            GlobalControls.globalControlsProperties.Contains("waterTaskCompleted") &&
            GlobalControls.globalControlsProperties.Contains("poopTaskCompleted"))
        {
            GlobalControls.currentObjective = 5;
            GameObject.Find("Managers").GetComponent<ReferenceManager>().objectiveManager.UpdateObjectiveBanner();
        }
        
        UpdateVisualText();
    }

    private void UpdateVisualText()
    {
        poopLevelNumber.text = poopTimeLeft.ToString();
        waterLevelNumber.text = waterTimeLeft.ToString();

        if (GlobalControls.globalControlsProperties.Contains("poopTaskCompleted"))
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

        if (GlobalControls.globalControlsProperties.Contains("waterTaskCompleted"))
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
        
        if (GlobalControls.globalControlsProperties.Contains("objectivesEnabled") && 
            GlobalControls.globalControlsProperties.Contains("waterTaskCompleted") && 
            GlobalControls.globalControlsProperties.Contains("poopTaskCompleted") && 
            GlobalControls.currentObjective != 6)
        {
            GlobalControls.currentObjective = 6;
            referenceManager.objectiveManager.UpdateObjectiveBanner();
        }
    }

}
