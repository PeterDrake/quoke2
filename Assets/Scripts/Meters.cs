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
    
    private PlayerDeath playerDeathScript;
    private GameObject player;

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
        bool isStrategicMap = GlobalControls.IsStrategicMap;
        Debug.Log("IsStrategicMap = " + GlobalControls.IsStrategicMap);
        poopTimeLeft = GlobalControls.PoopTimeLeft;
        waterTimeLeft = GlobalControls.WaterTimeLeft;
        
        player = GameObject.FindGameObjectWithTag("Player");
        playerDeathScript = player.GetComponent<PlayerDeath>();
        
        if (GlobalControls.MetersEnabled && !isStrategicMap)
        {
            if (!GlobalControls.PoopTaskCompleted)
            {
                if(poopTimeLeft == 1)
                {
                    Debug.Log("You died by poop meter going to zero!!");
                    playerDeathScript.KillPlayer(this.gameObject, 4);
                }
                poopTimeLeft--;
                GlobalControls.PoopTimeLeft = poopTimeLeft;
            }
            if (!GlobalControls.WaterTaskCompleted)
            {
                if(waterTimeLeft == 1)
                {
                    Debug.Log("You died of thirst!");
                    playerDeathScript.KillPlayer(this.gameObject, 2);
                }
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
