using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

/// <summary>
/// keeps track of what the current objective is
/// </summary>
public class ObjectiveManager : MonoBehaviour
{
    private string currentObjective;

    private ReferenceManager referenceManager;
    // Start is called before the first frame update
    void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();

        currentObjective = "";
    }
    
    public void UpdateObjectiveBanner()
    {
        SetCurrentObjective(GlobalControls.currentObjective);
        if (GlobalControls.globalControlsProperties.Contains("objectivesEnabled"))
        { 
            foreach (Image image in referenceManager.tooltipCanvas.GetComponentsInChildren<Image>(true))
            {
                if (image && image.gameObject.name.Equals("Objectives"))
                {
                    image.gameObject.SetActive(true);
                    image.GetComponentInChildren<Text>().text = currentObjective;
                }
            }
        }
    }
    
    public void SetCurrentObjective(int objective)
    {
        switch (objective)
        {
            case 0:
                currentObjective = "";
                break;
            case 1:
                currentObjective = "Clean up your house";
                break;
            case 2:
                currentObjective = "Clean up your apartment";
                break;
            case 3:
                currentObjective = "An Earthquake is coming!";
                break;
            case 4:
                currentObjective = "Leave your house";
                break;
            case 5:
                currentObjective = "Meet your neighbors";
                break;
            case 6:
                currentObjective = "Go to the tent to end the game!";
                break;
        }
    }
}
