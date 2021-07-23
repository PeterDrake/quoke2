using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CheatKeyboardController : MonoBehaviour
{
    // For scenes with no meters (before and during the quake), this can remain null
    public Meters meters;
    public SceneManagement sceneManagement;
    private string currentScene;
    public bool virtualKeyboard;
    private KeyCode keyDown = KeyCode.JoystickButton0;
    public KeyCode preQuakeTeleport = KeyCode.H;
    public KeyCode quakeTeleport = KeyCode.J;
    public KeyCode strategicMapTeleport = KeyCode.K;
    public KeyCode parkTeleport = KeyCode.B;
    public KeyCode completeWater = KeyCode.O;
    public KeyCode completePoop = KeyCode.P;
    public KeyCode restart = KeyCode.N;
    public KeyCode loadPoopItems = KeyCode.L;
    public KeyCode loadWaterItems = KeyCode.Y;
    public KeyCode loadPreQuakeItems = KeyCode.V;
    public KeyCode changeCondition = KeyCode.T;
    public KeyCode angieItems = KeyCode.F;

    void Start()
    {
        meters = GameObject.Find("Managers").GetComponent<ReferenceManager>().metersCanvas.GetComponent<Meters>();
        sceneManagement = GameObject.Find("Managers").GetComponent<ReferenceManager>().sceneManagement.GetComponent<SceneManagement>();
        currentScene = SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// SetKeyDown sets a specific key to be pressed for one update frame. Pass KeyCode.JoystickButton0 for none
    /// </summary>
    /// <param name="pressed"></param>
    public void SetKeyDown(KeyCode pressed)
    {
        keyDown = pressed;
    }
    
    void Update()
    {
        if (GlobalControls.AdminMode)
        {
            if (!virtualKeyboard)
            {
                if (Input.GetKeyDown(preQuakeTeleport)) keyDown = preQuakeTeleport;
                else if (Input.GetKeyDown(quakeTeleport)) keyDown = quakeTeleport;
                else if (Input.GetKeyDown(strategicMapTeleport)) keyDown = strategicMapTeleport;
                else if (Input.GetKeyDown(parkTeleport)) keyDown = parkTeleport;
                else if (Input.GetKeyDown(completeWater)) keyDown = completeWater;
                else if (Input.GetKeyDown(completePoop)) keyDown = completePoop;
                else if (Input.GetKeyDown(restart)) keyDown = restart;
                else if (Input.GetKeyDown(loadPoopItems)) keyDown = loadPoopItems;
                else if (Input.GetKeyDown(loadWaterItems)) keyDown = loadWaterItems;
                else if (Input.GetKeyDown(loadPreQuakeItems)) keyDown = loadPreQuakeItems;
                else if (Input.GetKeyDown(changeCondition)) keyDown = changeCondition;
                else if (Input.GetKeyDown(angieItems)) keyDown = angieItems;
            }
            
            if (keyDown.Equals(preQuakeTeleport))//Load PreQuakeHouse
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
            if (keyDown.Equals(quakeTeleport))//Load QuakeHouse
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
            if (keyDown.Equals(strategicMapTeleport))//Load Strategic Map
            {
                sceneManagement.ChangeScene("StrategicMap");
            }
            if (keyDown.Equals(parkTeleport))//Load Park
            {
                sceneManagement.ChangeScene("Park");
            }
            if (meters && keyDown.Equals(completeWater)) //Complete Water
            {
                meters.MarkTaskAsDone("water");
            }
            if (meters && keyDown.Equals(completePoop)) //Complete Poop
            {
                meters.MarkTaskAsDone("poop");
            }
            if (keyDown.Equals(restart)) //Restart Game
            {
                sceneManagement.Restart();
            }
            if (keyDown.Equals(loadPoopItems)) //Load Yard with Latrine Items
            {
                if (GlobalControls.ApartmentCondition)
                {
                    GlobalItemList.Reset();
                    GlobalItemList.UpdateItemList("Bucket", "Inventory", new Vector3(0, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Bucket 2", "Inventory", new Vector3(1, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Bag", "Inventory", new Vector3(2, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Toilet Paper", "Inventory", new Vector3(3, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Wood Chips", "Inventory", new Vector3(4, 0, 0),"Player" );
                    sceneManagement.ChangeScene("Street");
                }
                else
                {
                    GlobalItemList.Reset();
                    GlobalItemList.UpdateItemList("Shovel", "Inventory", new Vector3(0, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Plywood", "Inventory", new Vector3(1, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Rope", "Inventory", new Vector3(2, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Tarp", "Inventory", new Vector3(3, 0, 0),"Player" );
                    GlobalItemList.UpdateItemList("Toilet Paper", "Inventory", new Vector3(4, 0, 0),"Player" );
                    sceneManagement.ChangeScene("Yard");
                }
                
            }
            if (keyDown.Equals(loadWaterItems)) //Load scene with water task items
            {
                GlobalItemList.Reset();
                GlobalItemList.UpdateItemList("Dirty Water Bottle", "Inventory", new Vector3(0,0,0),"Player" );
                GlobalItemList.UpdateItemList("Bleach", "Inventory", new Vector3(1,0,0),"Player" );
                sceneManagement.ChangeScene(SceneManager.GetActiveScene().name);
            }
            if (keyDown.Equals(loadPreQuakeItems)) //Load Yard with PreQuake Items
            {
                if (GlobalControls.ApartmentCondition)
                {
                    GlobalItemList.Reset();
                    GlobalItemList.UpdateItemList("Bleach", "Street", new Vector3(4.5f,1.5f,-8.5f), "Go Bag 1");
                    GlobalItemList.UpdateItemList("Book", "Street", new Vector3(4.5f,1.5f,-7.5f), "Go Bag 2");
                    sceneManagement.ChangeScene("Street");
                }
                else
                {
                    GlobalItemList.Reset();
                    GlobalItemList.UpdateItemList("Sunscreen", "Yard", new Vector3(6.5f, 0.5f, 0.5f), "");
                    GlobalItemList.UpdateItemList("Dirty Water Bottle", "Yard", new Vector3(-6.5f, 0.5f, 0.5f), "");
                    GlobalItemList.UpdateItemList("Flashlight", "Yard", new Vector3(3.5f, 0.5f, 3.5f), "");
                    GlobalItemList.UpdateItemList("Book", "Yard", new Vector3(-5.5f, 0.5f, -7.5f), "");
                    sceneManagement.ChangeScene("Yard");
                }
            }
            if (keyDown.Equals(changeCondition))
            {
                Debug.Log("Changing Global Apartment condition flag from " + GlobalControls.ApartmentCondition + 
                          " to " + !GlobalControls.ApartmentCondition);
                GlobalControls.ApartmentCondition = !GlobalControls.ApartmentCondition;
                GlobalItemList.Reset();
            }
            if (keyDown.Equals(angieItems))
            {
                Debug.Log("Giving Angie Required Items");
                GlobalItemList.Reset();
                GlobalItemList.UpdateItemList("First Aid Kit", "Inventory", new Vector3(0, 0, 0),"Player" );
                GlobalItemList.UpdateItemList("Epi Pen", "Inventory", new Vector3(1, 0, 0),"Player" );
                sceneManagement.ChangeScene("Garden");
            }
            
            keyDown = KeyCode.JoystickButton0;
        }
    }
}
