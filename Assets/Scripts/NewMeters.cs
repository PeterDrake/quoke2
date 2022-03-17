using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// poop and water meter at the top of the screen
/// </summary>
public class NewMeters : MonoBehaviour
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
	private GameObject waterTooltip;
    private GameObject waterTooltipText;
    private GameObject waterMeter;

    private Meters meters;
    
    private Text poopLevelNumber;
    private TextMeshProUGUI waterLevelNumber;

    private TMP_Text waterLevelNumberTMP;

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

        foreach (Transform obj in gameObject.GetComponentsInChildren<Transform>())
        {
            Debug.Log("obj: " + obj + " obj name: <" + obj.name + ">");
            if (obj.name.Equals("WaterMeter"))
            {
                Debug.Log("Obj: " + obj.gameObject);
                waterMeter = obj.gameObject;
                Debug.Log(" water meter set: " + waterMeter);
            }
            
        }

        foreach (Transform child in waterMeter.GetComponentsInChildren<Transform>())
        {
            Debug.Log("Child: " + child.name);
            if (child.name.Equals("Poop Value")) poopLevelNumber = child.gameObject.GetComponent<Text>();
            else if (child.name.Equals("Water Value Text (TMP)") || child.name.Equals("Water Value")) waterLevelNumber = child.gameObject.GetComponent<TextMeshProUGUI>();
            else if (child.name.Equals("Poop Done")) poopDoneIndicator = child.gameObject;
            else if (child.name.Equals("Water Done")) waterDoneIndicator = child.gameObject;
            else if (child.name.Equals("Poop Back")) poopDoneImage = child.gameObject.GetComponent<Image>();
            else if (child.name.Equals("Water Icon Background") || child.name.Equals("Water Back")) waterDoneImage = child.gameObject.GetComponent<Image>();
            else if (child.name.Equals("Poop Progress")) poopProgressObject = child.gameObject;
            else if (child.name.Equals("Water Fill Slider") || child.name.Equals("Water Progress")) waterProgressObject = child.gameObject;
            else if (child.name.Equals("Poop Background")) poopProgressFill = child.gameObject.GetComponent<Image>();
            else if (child.name.Equals("Water Icon Mask") || child.name.Equals("Water Background")) waterProgressFill = child.gameObject.GetComponent<Image>();
			else if (child.name.Equals("Water Tooltip"))
            {

                Debug.Log("found water tooltip");
                waterTooltip = child.gameObject;
                foreach (TextMeshProUGUI obj in child.GetComponentsInChildren<TextMeshProUGUI>())
                {
                    if (obj.name.Equals("Text (TMP)")) waterTooltipText = obj.gameObject;
                }
            }
            
            
        }
        
        Debug.Log("Water objects: " + waterTooltipText.ToString());
        waterTooltip.SetActive(false);
        
        poopTimeLeft = GlobalControls.poopTimeLeft;
        waterTimeLeft = GlobalControls.waterTimeLeft;

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

    public void UpdateVisualText()
    {
		if (waterLevelNumber) waterLevelNumber.text = waterTimeLeft.ToString();
		if (waterLevelNumberTMP) waterLevelNumberTMP.text = waterTimeLeft.ToString();
//        poopLevelNumber.text = poopTimeLeft.ToString();

		if (waterTimeLeft <= 3)
		{
			waterTooltip.SetActive(true);
		}

        if (GlobalControls.globalControlsProperties.Contains("poopTaskCompleted"))
        {
            poopDoneIndicator.SetActive(true);
            poopDoneImage.color = Color.yellow;
            poopProgressFill.color = Color.yellow;
        }
        else
        {
//            poopDoneIndicator.SetActive(false);
   //         poopDoneImage.color = Color.white;
     //       poopProgressFill.color = Color.white;
        }

        if (GlobalControls.globalControlsProperties.Contains("waterTaskCompleted"))
        {
			if (waterLevelNumberTMP) waterLevelNumberTMP.text = "Done!";
			waterTooltip.SetActive(false);
            waterDoneImage.color = Color.yellow;
            waterProgressFill.color = Color.yellow;
        }
        else
        {
            //waterDoneIndicator.SetActive(false);
            waterDoneImage.color = Color.white;
            waterProgressFill.color = Color.white;
        }
        
//        poopProgressObject.GetComponent<Slider>().value = poopTimeLeft / 24f;
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