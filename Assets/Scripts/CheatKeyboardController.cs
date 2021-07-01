using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatKeyboardController : MonoBehaviour
{
    // For scenes with no meters (before and during the quake), this can remain null
    public Meters meters;
    public SceneManagement sceneManagement;
    private string currentScene;
    public KeyCode preQuakeTeleport = KeyCode.H;
    public KeyCode quakeTeleport = KeyCode.J;
    public KeyCode strategicMapTeleport = KeyCode.K;
    public KeyCode parkTeleport = KeyCode.B;
    public KeyCode completeWater = KeyCode.O;
    public KeyCode completePoop = KeyCode.P;
    public KeyCode restart = KeyCode.N;
    public KeyCode causeAftershock = KeyCode.U;
    public KeyCode loadPoopItems = KeyCode.L;
    public KeyCode loadWaterItems = KeyCode.Y;
    public KeyCode loadPreQuakeItems = KeyCode.V;
    public KeyCode changeCondition = KeyCode.T;

    void Start()
    {
        meters = GameObject.Find("Managers").GetComponent<ReferenceManager>().metersCanvas.GetComponent<Meters>();
        sceneManagement = GameObject.Find("Managers").GetComponent<ReferenceManager>().sceneManagement.GetComponent<SceneManagement>();
        currentScene = SceneManager.GetActiveScene().name;

    }
    void Update()
    {
        if (GlobalControls.AdminMode)
        {
            if (Input.GetKeyDown(preQuakeTeleport))//Load PreQuakeHouse
            {
                if (GlobalControls.ApartmentCondition)
                {
                    sceneManagement.ChangeScene("PreQuakeApartment");
                }
                else
                {
                    sceneManagement.ChangeScene("PreQuakeHouse");
                }
            }
            if (Input.GetKeyDown(quakeTeleport))//Load QuakeHouse
            {
                if (GlobalControls.ApartmentCondition)
                {
                    sceneManagement.ChangeScene("QuakeApartment");
                }
                else
                {
                    sceneManagement.ChangeScene("QuakeHouse");
                }
            }
            if (Input.GetKeyDown(strategicMapTeleport))//Load Strategic Map
            {
                sceneManagement.ChangeScene("StrategicMap");
            }
            if (Input.GetKeyDown(parkTeleport))//Load Park
            {
                sceneManagement.ChangeScene("Park");
            }
            if (meters && Input.GetKeyDown(completeWater)) //Complete Water
            {
                meters.MarkTaskAsDone("water");
            }
            if (meters && Input.GetKeyDown(completePoop)) //Complete Poop
            {
                meters.MarkTaskAsDone("poop");
            }
            if (Input.GetKeyDown(restart)) //Restart Game
            {
                sceneManagement.Restart();
            }
            if ((currentScene.Equals("Yard") || currentScene.Equals("Street")) && Input.GetKeyDown(causeAftershock))
            {
                GameObject.Find("Quake Event Manager").GetComponent<QuakeManager>().TriggerQuake();
            }
            if (Input.GetKeyDown(loadPoopItems)) //Load Yard with Latrine Items
            {
                if (GlobalControls.ApartmentCondition)
                {
                    GlobalItemList.Reset();
                    GlobalItemList.UpdateItemList("Bucket", "Inventory", new Vector3(0, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Bucket 2", "Inventory", new Vector3(1, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Bag", "Inventory", new Vector3(2, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Wood Chips", "Inventory", new Vector3(3, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Toilet Paper", "Inventory", new Vector3(4, 0, 0),"Player" );
                    sceneManagement.ChangeScene("Street");
                }
                else
                {
                    GlobalItemList.Reset();
                    GlobalItemList.UpdateItemList("Shovel", "Inventory", new Vector3(0, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Tarp", "Inventory", new Vector3(1, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Plywood", "Inventory", new Vector3(2, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Rope", "Inventory", new Vector3(3, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Toilet Paper", "Inventory", new Vector3(4, 0, 0),"Player" );
                    sceneManagement.ChangeScene("Yard");
                }
                
            }
            if (Input.GetKeyDown(loadWaterItems)) //Load scene with water task items
            {
                GlobalItemList.Reset();
                GlobalItemList.UpdateItemList("Water Bottle", "Inventory", new Vector3(0,0,0),"Player" );
                GlobalItemList.UpdateItemList("Chlorine Tablet", "Inventory", new Vector3(1,0,0),"Player" );
                sceneManagement.ChangeScene(SceneManager.GetActiveScene().name);
            }
            if (Input.GetKeyDown(loadPreQuakeItems)) //Load Yard with PreQuake Items
            {
                if (GlobalControls.ApartmentCondition)
                {
                    GlobalItemList.Reset();
                    GlobalItemList.UpdateItemList("Chlorine Tablet", "Street", new Vector3(4.5f,1.5f,-8.5f), "Go Bag 1");
                    GlobalItemList.UpdateItemList("Book", "Street", new Vector3(4.5f,1.5f,-7.5f), "Go Bag 2");
                    sceneManagement.ChangeScene("Street");
                }
                else
                {
                    GlobalItemList.Reset();
                    GlobalItemList.UpdateItemList("Cup", "Yard", new Vector3(6.5f, 0.5f, 0.5f), "");
                    GlobalItemList.UpdateItemList("Water Bottle", "Yard", new Vector3(-6.5f, 0.5f, 0.5f), "");
                    GlobalItemList.UpdateItemList("Sandwich", "Yard", new Vector3(3.5f, 0.5f, 3.5f), "");
                    GlobalItemList.UpdateItemList("Book", "Yard", new Vector3(-5.5f, 0.5f, -7.5f), "");
                    sceneManagement.ChangeScene("Yard");
                }
            }
            if (Input.GetKeyDown(changeCondition))
            {
                Debug.Log("Changing Global Apartment condition flag from " + GlobalControls.ApartmentCondition + 
                          " to " + !GlobalControls.ApartmentCondition);
                GlobalControls.ApartmentCondition = !GlobalControls.ApartmentCondition;
                GlobalItemList.Reset();
            }
        }
    }
}
